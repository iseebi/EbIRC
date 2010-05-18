using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel;
using SslTest;

namespace EbiSoft.EbIRC.IRC {
	/// <summary>
	/// IRCClient の概要の説明です。
	/// </summary>
    public class IRCClient : Component
    {
        private Thread m_thread;        // 送信スレッド
        private Socket m_socket;        // ソケット
        private SslHelper m_sslHelper;  // SSLヘルパ
        private NetworkStream m_stream; // ストリーム
        private StreamReader m_reader;  // 受信用ストリームリーダー
        private StreamWriter m_writer;  // 送信用ストリームライター

        private Queue m_sendQueue;      // 送信キュー

        private ManualResetEvent m_threadStopSignal;    // スレッド停止シグナル
        private bool m_threadStopFlag;                  // スレッド停止フラグ

        private ServerInfo m_server;    // 接続先情報
        private UserInfo m_user;        // ユーザー情報
        private string m_userString;    // このクライアントのユーザー文字列
        private Encoding m_encoding;    // エンコーディング

        private IAsyncResult m_connectAsync;        // 接続時シンクロオブジェクト
        Dictionary<string, string[]> m_namelist;    // 名前リスト一時保存用
        private bool m_online;                      // 接続完了フラグ
        private const int MaxNickLength = 9;        // NICK最大サイズ

        private Control m_ownerControl;             // 親コントロール

        #region イベント定義

        #region Connected

        private static readonly object eventKeyOfConnected = new object();

        /// <summary>
        /// サーバーに接続したときに発生します。
        /// </summary>
        public event EventHandler Connected
        {
            add
            {
                Events.AddHandler(eventKeyOfConnected, value);
            }
            remove
            {
                Events.RemoveHandler(eventKeyOfConnected, value);
            }
        }

        /// <summary>
        /// Connected イベントを発生させます。
        /// </summary>
        protected void OnConnected()
        {
            EventHandler handler = (EventHandler)Events[eventKeyOfConnected];
            if (handler != null)
            {
                Control owner = GetOwner();
                if ((owner != null) && owner.InvokeRequired)
                {
                    owner.Invoke(handler, this, EventArgs.Empty);
                }
                else
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region ConnectionFailed

        private static readonly object eventKeyOfConnectionFailed = new object();

        /// <summary>
        /// サーバーへの接続が失敗したときに発生します。
        /// </summary>
        public event EventHandler ConnectionFailed
        {
            add
            {
                Events.AddHandler(eventKeyOfConnectionFailed, value);
            }
            remove
            {
                Events.RemoveHandler(eventKeyOfConnectionFailed, value);
            }
        }

        /// <summary>
        /// ConnectionFailed イベントを発生させます。
        /// </summary>
        protected void OnConnectionFailed()
        {
            EventHandler handler = (EventHandler)Events[eventKeyOfConnectionFailed];
            if (handler != null)
            {
                Control owner = GetOwner();
                if ((owner != null) && owner.InvokeRequired)
                {
                    owner.Invoke(handler, this, EventArgs.Empty);
                }
                else
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region Disconnected

        private static readonly object eventKeyOfDisconnected = new object();

        /// <summary>
        /// 接続が切断した時に発生します。
        /// </summary>
        public event EventHandler Disconnected
        {
            add
            {
                Events.AddHandler(eventKeyOfDisconnected, value);
            }
            remove
            {
                Events.RemoveHandler(eventKeyOfDisconnected, value);
            }
        }

        /// <summary>
        /// Disconnected イベントを発生させます。
        /// </summary>
        protected void OnDisconnected()
        {
            EventHandler handler = (EventHandler)Events[eventKeyOfDisconnected];
            if (handler != null)
            {
                Control owner = GetOwner();
                if ((owner != null) && owner.InvokeRequired)
                {
                    owner.Invoke(handler, this, EventArgs.Empty);
                }
                else
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region StartMessageEvents

        private static object eventKeyOfStartMessageEvents = new object();

        /// <summary>
        /// メッセージイベントの処理が始まる前に発生します。
        /// </summary>
        public event EventHandler StartMessageEvents
        {
            add
            {
                Events.AddHandler(eventKeyOfStartMessageEvents, value);
            }
            remove
            {
                Events.RemoveHandler(eventKeyOfStartMessageEvents, value);
            }
        }

        /// <summary>
        /// StartMessageEvents イベントを発生させます。
        /// </summary>
        protected void OnStartMessageEvents()
        {
            EventHandler handler = (EventHandler)Events[eventKeyOfStartMessageEvents];
            if (handler != null)
            {
                Control owner = GetOwner();
                if ((owner != null) && owner.InvokeRequired)
                {
                    owner.Invoke(handler, this, EventArgs.Empty);
                }
                else
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region FinishMessageEvents

        private static object eventKeyOfFinishMessageEvents = new object();

        /// <summary>
        /// メッセージイベントの処理が完了した後にに発生します。
        /// </summary>
        public event EventHandler FinishMessageEvents
        {
            add
            {
                Events.AddHandler(eventKeyOfFinishMessageEvents, value);
            }
            remove
            {
                Events.RemoveHandler(eventKeyOfFinishMessageEvents, value);
            }
        }

        /// <summary>
        /// FinishMessageEvents イベントを発生させます。
        /// </summary>
        protected void OnFinishMessageEvents()
        {
            EventHandler handler = (EventHandler)Events[eventKeyOfFinishMessageEvents];
            if (handler != null)
            {
                Control owner = GetOwner();
                if ((owner != null) && owner.InvokeRequired)
                {
                    owner.Invoke(handler, this, EventArgs.Empty);
                }
                else
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region ProcessedConnection

        private static readonly object eventKeyOfProcessedConnection = new object();

        /// <summary>
        /// 接続が完了したときにに発生します。
        /// </summary>
        public event EventHandler ProcessedConnection
        {
            add
            {
                Events.AddHandler(eventKeyOfProcessedConnection, value);
            }
            remove
            {
                Events.RemoveHandler(eventKeyOfProcessedConnection, value);
            }
        }

        /// <summary>
        /// ProcessedConnection イベントを発生させます。
        /// </summary>
        protected void OnProcessedConnection()
        {
            EventHandler handler = (EventHandler)Events[eventKeyOfProcessedConnection];
            if (handler != null)
            {
                Control owner = GetOwner();
                if ((owner != null) && owner.InvokeRequired)
                {
                    owner.Invoke(handler, this, EventArgs.Empty);
                }
                else
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region ChangedMyNickname

        private static readonly object eventKeyOfChangedMyNickname = new object();

        /// <summary>
        /// ユーザーのニックネームが変更されたときに発生します。
        /// </summary>
        public event NickNameChangeEventHandler ChangedMyNickname
        {
            add
            {
                Events.AddHandler(eventKeyOfChangedMyNickname, value);
            }
            remove
            {
                Events.RemoveHandler(eventKeyOfChangedMyNickname, value);
            }
        }

        /// <summary>
        /// ChangedMyNickname イベントを発生させます。
        /// </summary>
        protected void OnChangedMyNickname(NickNameChangeEventArgs e)
        {
            NickNameChangeEventHandler handler = (NickNameChangeEventHandler)Events[eventKeyOfChangedMyNickname];
            if (handler != null)
            {
                Control owner = GetOwner();
                if ((owner != null) && owner.InvokeRequired)
                {
                    owner.Invoke(handler, this, e);
                }
                else
                {
                    handler(this, e);
                }
            }
        }

        #endregion

        #region ChangedNickname

        private static readonly object eventKeyOfChangedNickname = new object();

        /// <summary>
        /// ユーザーのニックネームが変更されたときに発生します。
        /// </summary>
        public event NickNameChangeEventHandler ChangedNickname
        {
            add
            {
                Events.AddHandler(eventKeyOfChangedNickname, value);
            }
            remove
            {
                Events.RemoveHandler(eventKeyOfChangedNickname, value);
            }
        }

        /// <summary>
        /// ChangedMyNickname イベントを発生させます。
        /// </summary>
        protected void OnChangedNickname(NickNameChangeEventArgs e)
        {
            NickNameChangeEventHandler handler = (NickNameChangeEventHandler)Events[eventKeyOfChangedNickname];
            if (handler != null)
            {
                Control owner = GetOwner();
                if ((owner != null) && owner.InvokeRequired)
                {
                    owner.Invoke(handler, this, e);
                }
                else
                {
                    handler(this, e);
                }
            }
        }

        #endregion

        #region ReceiveServerReply

        private static readonly object eventKeyOfReceiveServerReply = new object();

        /// <summary>
        /// サーバーメッセージを受信したときに発生します。
        /// </summary>
        public event ReceiveServerReplyEventHandler ReceiveServerReply
        {
            add
            {
                Events.AddHandler(eventKeyOfReceiveServerReply, value);
            }
            remove
            {
                Events.RemoveHandler(eventKeyOfReceiveServerReply, value);
            }
        }

        /// <summary>
        /// ReceiveServerReply イベントを発生させます。
        /// </summary>
        protected void OnReceiveServerReply(ReceiveServerReplyEventArgs e)
        {
            ReceiveServerReplyEventHandler handler = (ReceiveServerReplyEventHandler)Events[eventKeyOfReceiveServerReply];
            if (handler != null)
            {
                Control owner = GetOwner();
                if ((owner != null) && owner.InvokeRequired)
                {
                    owner.Invoke(handler, this, e);
                }
                else
                {
                    handler(this, e);
                }
            }
        }

        #endregion

        #region ReceiveMotdMesage

        private static readonly object eventKeyOfReceiveMotdMesage = new object();

        /// <summary>
        /// MOTDを受信したときに発生します。
        /// </summary>
        public event ReceiveMessageEventHandler ReceiveMotdMesage
        {
            add
            {
                Events.AddHandler(eventKeyOfReceiveMotdMesage, value);
            }
            remove
            {
                Events.RemoveHandler(eventKeyOfReceiveMotdMesage, value);
            }
        }

        /// <summary>
        /// ReceiveMotdMesage イベントを発生させます。
        /// </summary>
        protected void OnReceiveMotdMesage(ReceiveMessageEventArgs e)
        {
            ReceiveMessageEventHandler handler = (ReceiveMessageEventHandler)Events[eventKeyOfReceiveMotdMesage];
            if (handler != null)
            {
                Control owner = GetOwner();
                if ((owner != null) && owner.InvokeRequired)
                {
                    owner.Invoke(handler, this, e);
                }
                else
                {
                    handler(this, e);
                }
            }
        }

        #endregion

        #region ReceiveMessage

        private static readonly object eventKeyOfReceiveMessage = new object();

        /// <summary>
        /// PRIVMSGメッセージを受信したときに発生します。
        /// </summary>
        public event ReceiveMessageEventHandler ReceiveMessage
        {
            add
            {
                Events.AddHandler(eventKeyOfReceiveMessage, value);
            }
            remove
            {
                Events.RemoveHandler(eventKeyOfReceiveMessage, value);
            }
        }

        /// <summary>
        /// ReceiveMessage イベントを発生させます。
        /// </summary>
        protected void OnReceiveMessage(ReceiveMessageEventArgs e)
        {
            ReceiveMessageEventHandler handler = (ReceiveMessageEventHandler)Events[eventKeyOfReceiveMessage];
            if (handler != null)
            {
                Control owner = GetOwner();
                if ((owner != null) && owner.InvokeRequired)
                {
                    owner.Invoke(handler, this, e);
                }
                else
                {
                    handler(this, e);
                }
            }
        }

        #endregion

        #region ReceiveNotice

        private static readonly object eventKeyOfReceiveNotice = new object();

        /// <summary>
        /// NOTICEメッセージを受信したときにに発生します。
        /// </summary>
        public event ReceiveMessageEventHandler ReceiveNotice
        {
            add
            {
                Events.AddHandler(eventKeyOfReceiveNotice, value);
            }
            remove
            {
                Events.RemoveHandler(eventKeyOfReceiveNotice, value);
            }
        }

        /// <summary>
        /// ReceiveNotice イベントを発生させます。
        /// </summary>
        protected void OnReceiveNotice(ReceiveMessageEventArgs e)
        {
            ReceiveMessageEventHandler handler = (ReceiveMessageEventHandler)Events[eventKeyOfReceiveNotice];
            if (handler != null)
            {
                Control owner = GetOwner();
                if ((owner != null) && owner.InvokeRequired)
                {
                    owner.Invoke(handler, this, e);
                }
                else
                {
                    handler(this, e);
                }
            }
        }

        #endregion

        #region ReceiveNames

        private static readonly object eventKeyOfReceiveNames = new object();

        /// <summary>
        /// 参加者リストを受信したときに発生します。
        /// </summary>
        public event ReceiveNamesEventHandler ReceiveNames
        {
            add
            {
                Events.AddHandler(eventKeyOfReceiveNames, value);
            }
            remove
            {
                Events.RemoveHandler(eventKeyOfReceiveNames, value);
            }
        }

        /// <summary>
        /// ReceiveNames イベントを発生させます。
        /// </summary>
        protected void OnReceiveNames(ReceiveNamesEventArgs e)
        {
            ReceiveNamesEventHandler handler = (ReceiveNamesEventHandler)Events[eventKeyOfReceiveNames];
            if (handler != null)
            {
                Control owner = GetOwner();
                if ((owner != null) && owner.InvokeRequired)
                {
                    owner.Invoke(handler, this, e);
                }
                else
                {
                    handler(this, e);
                }
            }
        }

        #endregion

        #region ReceiveCtcpQuery

        private static readonly object eventKeyOfReceiveCtcpQuery = new object();

        /// <summary>
        /// CTCPクエリを受信したときに発生します。
        /// </summary>
        public event CtcpEventHandler ReceiveCtcpQuery
        {
            add
            {
                Events.AddHandler(eventKeyOfReceiveCtcpQuery, value);
            }
            remove
            {
                Events.RemoveHandler(eventKeyOfReceiveCtcpQuery, value);
            }
        }

        /// <summary>
        /// ReceiveCtcpQuery イベントを発生させます。
        /// </summary>
        protected void OnReceiveCtcpQuery(CtcpEventArgs e)
        {
            CtcpEventHandler handler = (CtcpEventHandler)Events[eventKeyOfReceiveCtcpQuery];
            if (handler != null)
            {
                Control owner = GetOwner();
                if ((owner != null) && owner.InvokeRequired)
                {
                    owner.Invoke(handler, this, e);
                }
                else
                {
                    handler(this, e);
                }
                if (e.Reply != string.Empty)
                {
                    SendCtcpReply(e.Sender, string.Format("{0} {1}", e.Command, e.Reply));
                }
            }
        }

        #endregion

        #region ReceiveCtcpReply

        private static readonly object eventKeyOfReceiveCtcpReply = new object();

        /// <summary>
        /// CTCPリプライを受信したときに発生します。
        /// </summary>
        public event CtcpEventHandler ReceiveCtcpReply
        {
            add
            {
                Events.AddHandler(eventKeyOfReceiveCtcpReply, value);
            }
            remove
            {
                Events.RemoveHandler(eventKeyOfReceiveCtcpReply, value);
            }
        }

        /// <summary>
        /// ReceiveCtcpReply イベントを発生させます。
        /// </summary>
        protected void OnReceiveCtcpReply(CtcpEventArgs e)
        {
            CtcpEventHandler handler = (CtcpEventHandler)Events[eventKeyOfReceiveCtcpReply];
            if (handler != null)
            {
                Control owner = GetOwner();
                if ((owner != null) && owner.InvokeRequired)
                {
                    owner.Invoke(handler, this, e);
                }
                else
                {
                    handler(this, e);
                }
            }
        }

        #endregion

        #region UserInOut

        private static readonly object eventKeyOfUserInOut = new object();

        /// <summary>
        /// ユーザーの出入りがあったときに発生します。
        /// </summary>
        public event UserInOutEventHandler UserInOut
        {
            add
            {
                Events.AddHandler(eventKeyOfUserInOut, value);
            }
            remove
            {
                Events.RemoveHandler(eventKeyOfUserInOut, value);
            }
        }

        /// <summary>
        /// UserInOut イベントを発生させます。
        /// </summary>
        protected void OnUserInOut(UserInOutEventArgs e)
        {
            UserInOutEventHandler handler = (UserInOutEventHandler)Events[eventKeyOfUserInOut];
            if (handler != null)
            {
                Control owner = GetOwner();
                if ((owner != null) && owner.InvokeRequired)
                {
                    owner.Invoke(handler, this, e);
                }
                else
                {
                    handler(this, e);
                }
            }
        }

        #endregion

        #region Kick

        private static readonly object eventKeyOfKick = new object();

        /// <summary>
        /// キックが実行されたときに発生します。
        /// </summary>
        public event KickEventHandler Kick
        {
            add
            {
                Events.AddHandler(eventKeyOfKick, value);
            }
            remove
            {
                Events.RemoveHandler(eventKeyOfKick, value);
            }
        }

        /// <summary>
        /// Kick イベントを発生させます。
        /// </summary>
        protected void OnKick(KickEventArgs e)
        {
            KickEventHandler handler = (KickEventHandler)Events[eventKeyOfKick];
            if (handler != null)
            {
                Control owner = GetOwner();
                if ((owner != null) && owner.InvokeRequired)
                {
                    owner.Invoke(handler, this, e);
                }
                else
                {
                    handler(this, e);
                }
            }
        }

        #endregion

        #region TopicChange

        private static readonly object eventKeyOfTopicChange = new object();

        /// <summary>
        /// トピックが変更されたときに発生します。
        /// </summary>
        public event TopicChangeEventDelegate TopicChange
        {
            add
            {
                Events.AddHandler(eventKeyOfTopicChange, value);
            }
            remove
            {
                Events.RemoveHandler(eventKeyOfTopicChange, value);
            }
        }

        /// <summary>
        /// TopicChange イベントを発生させます。
        /// </summary>
        protected void OnTopicChange(TopicChangeEventArgs e)
        {
            TopicChangeEventDelegate handler = (TopicChangeEventDelegate)Events[eventKeyOfTopicChange];
            if (handler != null)
            {
                Control owner = GetOwner();
                if ((owner != null) && owner.InvokeRequired)
                {
                    owner.Invoke(handler, this, e);
                }
                else
                {
                    handler(this, e);
                }
            }
        }

        #endregion

        #region ModeChange

        private static readonly object eventKeyOfModeChange = new object();

        /// <summary>
        /// モードが変更されたときに発生します。
        /// </summary>
        public event ModeChangeEventHandler ModeChange
        {
            add
            {
                Events.AddHandler(eventKeyOfModeChange, value);
            }
            remove
            {
                Events.RemoveHandler(eventKeyOfModeChange, value);
            }
        }

        /// <summary>
        /// ModeChange イベントを発生させます。
        /// </summary>
        protected void OnModeChange(ModeChangeEventArgs e)
        {
            ModeChangeEventHandler handler = (ModeChangeEventHandler)Events[eventKeyOfModeChange];
            if (handler != null)
            {
                Control owner = GetOwner();
                if ((owner != null) && owner.InvokeRequired)
                {
                    owner.Invoke(handler, this, e);
                }
                else
                {
                    handler(this, e);
                }
            }
        }

        #endregion

        private Control GetOwner()
        {
            return OwnerControl;
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IRCClient()
        {
            m_encoding = new UTF8Encoding(false);

            m_sendQueue = new Queue();
            m_namelist = new Dictionary<string, string[]>();

            m_threadStopSignal = new ManualResetEvent(false);
            m_threadStopFlag = false;

            m_ownerControl = null;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ownerControl">通知先コントロール</param>
        public IRCClient(Control ownerControl) : this()
        {
            m_ownerControl = ownerControl;
        }

        #endregion

        #region 接続

        /// <summary>
        /// 接続
        /// </summary>
        /// <param name="name">サーバー名</param>
        /// <param name="port">接続ポート</param>
        /// <param name="password">サーバーパスワード</param>
        /// <param name="nickname">ニックネーム</param>
        /// <param name="realname">名前</param>
        public void Connect(string name, int port, string password, string nickname, string realname)
        {
            Connect(new ServerInfo(name, port, password), new UserInfo(nickname, realname));
        }

        /// <summary>
        /// 接続
        /// </summary>
        /// <param name="name">サーバー名</param>
        /// <param name="port">接続ポート</param>
        /// <param name="password">サーバーパスワード</param>
        /// <param name="nickname">ニックネーム</param>
        /// <param name="realname">名前</param>
        /// <param name="useSsl">SSL使用</param>
        public void Connect(string name, int port, string password, bool useSsl, string nickname, string realname)
        {
            Connect(new ServerInfo(name, port, password, useSsl), new UserInfo(nickname, realname));
        }


        /// <summary>
        /// 接続
        /// </summary>
        /// <param name="name">サーバー名</param>
        /// <param name="nickname">ニックネーム</param>
        /// <param name="realname">名前</param>
        public void Connect(ServerInfo server, string nickname, string realname)
        {
            Connect(server, new UserInfo(nickname, realname));
        }

        /// <summary>
        /// 接続
        /// </summary>
        /// <param name="name">サーバー名</param>
        /// <param name="port">接続ポート</param>
        /// <param name="password">サーバーパスワード</param>
        /// <param name="user">ユーザーデータ</param>
        public void Connect(string name, int port, string password, UserInfo user)
        {
            Connect(new ServerInfo(name, port, password), user);
        }

        /// <summary>
        /// 接続
        /// </summary>
        /// <param name="server">サーバーデータ</param>
        /// <param name="user">ユーザーデータ</param>
        public void Connect(ServerInfo server, UserInfo user)
        {
            if (Status != IRCClientStatus.Disconnected)
            {
                throw new InvalidOperationException();
            }

            try
            {
                // 準備。
                m_server = server;
                m_user = user;
                m_online = false;
                m_threadStopFlag = false;
                lock (m_sendQueue) { m_sendQueue.Clear(); }
                lock (m_namelist) { m_namelist.Clear(); }

                // ソケット作成・接続
                Debug.WriteLine("接続を開始します。", "IRCClient");
                m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                if (Server.UseSsl)
                {
                    m_sslHelper = new SslHelper(m_socket, Server.Name);
                }
                m_connectAsync = m_socket.BeginConnect(server.GetEndPoint(), new AsyncCallback(OnConnected), m_socket);
            }
            catch
            {
                OnConnectionFailed();
                Close();
            }
        }

        /// <summary>
        /// 接続完了コールバック
        /// </summary>
        /// <param name="ar">非同期処理ステータス</param>
        protected void OnConnected(IAsyncResult ar)
        {
            try
            {
                // 接続処理の完了
                // (接続エラーの場合、EndConnectの段階でSocketExceptionがスローされる)
                Socket socket = (Socket)ar.AsyncState;
                socket.EndConnect(ar);

                // 接続完了イベント発行
                Debug.WriteLine("接続しました。", "IRCClient");
                OnConnected();

                // ストリーム作成
                m_stream = new NetworkStream(socket);

                // ストリームリーダ・ライタ作成
                m_reader = new StreamReader(m_stream, this.Encoding);
                m_writer = new StreamWriter(m_stream, this.Encoding);
                m_writer.NewLine = "\r\n";
                m_writer.AutoFlush = true;

                // 送信スレッド作成
                m_thread = new Thread(new ThreadStart(ThreadAction));
                m_thread.Name = "IRCThread";
                m_thread.IsBackground = true;
                m_thread.Start();
                Debug.WriteLine("スレッドを開始しました。", "IRCClient");

                // ログインコマンド送信
                Debug.WriteLine("ログインコマンド送信します。", "IRCClient");
                SendConnectCommand();
            }
            catch
            {
                // 接続失敗時の処理
                OnConnectionFailed();
                Close();
            }
        }

        #endregion

        #region 切断

        /// <summary>
        /// 切断
        /// </summary>
        /// <param name="message">切断時メッセージ</param>
        public void Disconnect(string message)
        {
            // オンライン状態のときはQUITを送る
            if (Status == IRCClientStatus.Online)
            {
                SendCommand(string.Format("QUIT :{0}", message));
            }
            Close();
        }

        /// <summary>
        /// 切断
        /// </summary>
        public void Disconnect()
        {
            Disconnect("EOF From client.");
        }

        /// <summary>
        /// 強制切断
        /// </summary>
        public void Close()
        {
            bool isDisconnected = (Status != IRCClientStatus.Disconnected);

            // 接続中なら、接続を完了させる
            if ((m_connectAsync != null) && (!m_connectAsync.IsCompleted))
            {
                ((Socket)m_connectAsync.AsyncState).EndConnect(m_connectAsync);
            }

            // スレッドを止める
            if (!m_threadStopFlag)
            {
                m_threadStopFlag = true;
                if (m_threadStopSignal.WaitOne(5000, false))
                {
                    // 5秒で止まらなければ強制終了
                    m_thread.Abort();
                }
            }
        }

        /// <summary>
        /// 強制切断
        /// </summary>
        [Obsolete]
        public void Close(bool report)
        {
            Close();
        }

        #endregion

        #region スレッド処理

        /// <summary>
        /// スレッドメイン処理
        /// </summary>
        private void ThreadAction()
        {
            try
            {
                m_threadStopSignal.Reset();
                Queue<EventData> eventQueue = new Queue<EventData>();

                while (!m_threadStopFlag)
                {
                    // 送信処理
                    lock (m_sendQueue)
                    {
                        // 送信キューがなくなるまで、キューを読み取って送信する
                        while (m_sendQueue.Count > 0)
                        {
                            string sendLine = m_sendQueue.Dequeue() as string;
                            if (sendLine != null)
                            {
                                Debug.WriteLine("Send> " + sendLine, "IRCClient");
                                m_writer.WriteLine(sendLine);

                                if (sendLine.StartsWith("QUIT ", StringComparison.OrdinalIgnoreCase))
                                {
                                    m_threadStopFlag = true;
                                }
                            }
                        }
                    }

                    // 受信処理
                    if (Server.UseSsl && !m_stream.DataAvailable)
                    {
                        m_socket.Poll(100, SelectMode.SelectRead);
                    }
                    while (m_stream.DataAvailable)
                    {
                        try
                        {
                            ProcessIRCMessage(eventQueue, m_reader.ReadLine());
                        }
                        catch (MessageParseException)
                        {
                            // TODO:不明メッセージ受信
                        }
                    }
                    

                    // イベント発生処理
                    if (eventQueue.Count > 0)
                    {
                        OnStartMessageEvents();
                        try
                        {
                            // キューにあるイベントをすべて発生させる
                            while (eventQueue.Count > 0)
                            {
                                EventData eventData = eventQueue.Dequeue();

                                #region イベントディスパッチ

                                if (eventData.EventKey == eventKeyOfProcessedConnection)
                                {
                                    OnProcessedConnection();
                                }
                                else if (eventData.EventKey == eventKeyOfChangedMyNickname)
                                {
                                    OnChangedMyNickname(eventData.Argument as NickNameChangeEventArgs);
                                }
                                else if (eventData.EventKey == eventKeyOfChangedNickname)
                                {
                                    OnChangedNickname(eventData.Argument as NickNameChangeEventArgs);
                                }
                                else if (eventData.EventKey == eventKeyOfReceiveServerReply)
                                {
                                    OnReceiveServerReply(eventData.Argument as ReceiveServerReplyEventArgs);
                                }
                                else if (eventData.EventKey == eventKeyOfReceiveMotdMesage)
                                {
                                    OnReceiveMotdMesage(eventData.Argument as ReceiveMessageEventArgs);
                                }
                                else if (eventData.EventKey == eventKeyOfReceiveMessage)
                                {
                                    OnReceiveMessage(eventData.Argument as ReceiveMessageEventArgs);
                                }
                                else if (eventData.EventKey == eventKeyOfReceiveNotice)
                                {
                                    OnReceiveNotice(eventData.Argument as ReceiveMessageEventArgs);
                                }
                                else if (eventData.EventKey == eventKeyOfReceiveNames)
                                {
                                    OnReceiveNames(eventData.Argument as ReceiveNamesEventArgs);
                                }
                                else if (eventData.EventKey == eventKeyOfReceiveCtcpQuery)
                                {
                                    OnReceiveCtcpQuery(eventData.Argument as CtcpEventArgs);
                                }
                                else if (eventData.EventKey == eventKeyOfReceiveCtcpReply)
                                {
                                    OnReceiveCtcpReply(eventData.Argument as CtcpEventArgs);
                                }
                                else if (eventData.EventKey == eventKeyOfUserInOut)
                                {
                                    OnUserInOut(eventData.Argument as UserInOutEventArgs);
                                }
                                else if (eventData.EventKey == eventKeyOfModeChange)
                                {
                                    OnModeChange(eventData.Argument as ModeChangeEventArgs);
                                }
                                else if (eventData.EventKey == eventKeyOfTopicChange)
                                {
                                    OnTopicChange(eventData.Argument as TopicChangeEventArgs);
                                }
                                else if (eventData.EventKey == eventKeyOfKick)
                                {
                                    OnKick(eventData.Argument as KickEventArgs);
                                }
                                else
                                {
                                    Debug.WriteLine("Undefined event:" + eventData.Argument.GetType().ToString());
                                }

                                #endregion
                            }
                        }
                        catch (Exception) { }
                        finally
                        {
                            OnFinishMessageEvents();
                        }
                    }

                    Thread.Sleep(50);
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception)
            {
            }
            finally
            {
                // 接続中であればソケットを閉じる
                if (m_socket.Connected)
                {
                    try
                    {
                        m_socket.Close();
                    }
                    catch (Exception) { }
                }
                if (m_sslHelper != null)
                {
                    m_sslHelper.Dispose();
                }

                // 使用したデータをクリアする
                m_socket = null;
                m_sslHelper = null;
                m_reader = null;
                m_writer = null;
                m_online = false;

                OnDisconnected();

                // 処理完了シグナル送信
                m_threadStopSignal.Set();
            }
        }

        #endregion

        #region 受信メッセージ解析

        /// <summary>
        /// 受信したメッセージを処理
        /// </summary>
        /// <param name="queue">追加先のイベントキュー</param>
        /// <param name="message">受信メッセージ</param>
        /// <returns>発生させるイベントの情報</returns>
        protected void ProcessIRCMessage(Queue<EventData> queue, string message)
        {
            try
            {
                #region メッセージパース
                Debug.WriteLine("Recv> " + message, "IRCClient");

                // とりあえず、半角スペースで区切る
                string[] messageParams = message.TrimEnd(' ').Split(' ');

                // メッセージ情報格納変数
                string sender;       // 送信者
                string command;      // コマンド
                string[] parameters; // パラメータ

                int parameterStartIndex; // パラメータの開始インデックス

                // 基本情報を得る
                if (messageParams[0].StartsWith(":"))
                {
                    // 通常のメッセージ
                    sender = messageParams[0].TrimStart(':');
                    command = messageParams[1];
                    parameterStartIndex = 2;
                }
                else
                {
                    // 通常以外のメッセージ
                    sender = string.Empty;
                    command = messageParams[0];
                    parameterStartIndex = 1;
                }

                // パラメータがまだ残っている場合、残りのパラメータを処理する
                if (parameterStartIndex < messageParams.Length)
                {
                    // パラメータ項目の最大数は半角スペーススプリット済みと開始インデックスの差よりも少なくなる
                    ArrayList parameterList = new ArrayList(messageParams.Length - parameterStartIndex);
                    for (int i = parameterStartIndex; i < messageParams.Length; i++)
                    {
                        // : で始まる場合、そこから先は一つのパラメータとして処理する
                        if (messageParams[i].StartsWith(":"))
                        {
                            string tempParam = messageParams[i].Substring(1);
                            // まだパラメータの残っている場合は一気につなぐ
                            if ((i + 1) < messageParams.Length)
                            {
                                tempParam += " " + string.Join(" ", messageParams, (i + 1), messageParams.Length - (i + 1));
                            }
                            parameterList.Add(tempParam);
                            break;
                        }
                        else
                        {
                            parameterList.Add(messageParams[i]);
                        }
                    }
                    parameters = new string[parameterList.Count];
                    parameterList.CopyTo(parameters);
                }
                else
                {
                    // パラメータなし
                    parameters = new string[] { };
                }
                #endregion

                // メッセージ振り分けで共通使用する変数
                string channel;        // チャンネル
                string receiver;       // 受信者
                string[] receivers;      // 受信者一覧

                if (char.IsNumber(command[0]))
                {
                    #region ニューメリックリプライ

                    // リプライ番号
                    ReplyNumbers number = (ReplyNumbers)int.Parse(command);

                    // ニューメリックリプライの parameter[0] は必ず受信者
                    receiver = parameters[0];

                    switch (number)
                    {
                        // 接続完了
                        case ReplyNumbers.RPL_WELCOME:
                            if (Status != IRCClientStatus.Online)
                            {
                                m_online = true;

                                // ニックネームをセット
                                User.setNick(GetUserName(receiver));

                                // ユーザーストリングを得る (最後のパラメータをスペースで切った最後の項目)
                                string[] userStringParseTemp = parameters[parameters.Length - 1].Split(' ');
                                m_userString = userStringParseTemp[userStringParseTemp.Length - 1];

                                // イベントを通知
                                queue.Enqueue(new EventData(eventKeyOfProcessedConnection, EventArgs.Empty));
                                queue.Enqueue(new EventData(eventKeyOfReceiveServerReply, new ReceiveServerReplyEventArgs(number, SliceArray(parameters, 1))));
                            }
                            break;

                        // 名前のコンフリクト
                        case ReplyNumbers.ERR_NICKNAMEINUSE:
                            queue.Enqueue(new EventData(eventKeyOfReceiveServerReply, new ReceiveServerReplyEventArgs(number, SliceArray(parameters, 1))));
                            // 新しいNickname を用意する。
                            string newNickname = GetNextNick(m_user.NickName);
                            m_user.setNick(newNickname);
                            ChangeNickname(newNickname);
                            break;

                        #region MOTDメッセージ
                        case ReplyNumbers.RPL_MOTDSTART:
                            queue.Enqueue(new EventData(eventKeyOfReceiveMotdMesage, new ReceiveMessageEventArgs(sender, receiver, parameters[1])));
                            break;
                        case ReplyNumbers.RPL_MOTD:
                            queue.Enqueue(new EventData(eventKeyOfReceiveMotdMesage, new ReceiveMessageEventArgs(sender, receiver, parameters[1])));
                            break;
                        case ReplyNumbers.RPL_ENDOFMOTD:
                            queue.Enqueue(new EventData(eventKeyOfReceiveMotdMesage, new ReceiveMessageEventArgs(sender, receiver, parameters[1])));
                            break;
                        #endregion

                        #region 参加者の受信

                        // 参加者一覧の受信
                        case ReplyNumbers.RPL_NAMREPLY:
                            // チャンネル名を名前リストを取得
                            channel = parameters[2];
                            string[] names = parameters[3].Split(' ');

                            // 名前リストに項目があれば追加
                            if (m_namelist.ContainsKey(channel))
                            {
                                // 新しい配列を作って、そこに新旧のデータを貼り付ける
                                string[] tempArr = new string[m_namelist[channel].Length + names.Length];
                                m_namelist[channel].CopyTo(tempArr, 0);
                                names.CopyTo(tempArr, m_namelist[channel].Length);
                                m_namelist[channel] = tempArr;
                            }
                            // 名前リストに項目がなければ追加
                            else
                            {
                                m_namelist.Add(channel, names);
                            }
                            break;

                        // 参加者受信完了
                        case ReplyNumbers.RPL_ENDOFNAMES:
                            channel = parameters[1];

                            // チャンネルのデータがあるときのみ
                            if (m_namelist.ContainsKey(channel))
                            {
                                // イベントで一覧を通知した後、リストから削除
                                queue.Enqueue(new EventData(eventKeyOfReceiveNames, new ReceiveNamesEventArgs(channel, m_namelist[channel])));
                                m_namelist.Remove(channel);
                            }
                            break;
                        #endregion

                        // トピックの受信
                        case ReplyNumbers.RPL_TOPIC:
                            string topic;
                            channel = parameters[1];
                            topic = parameters[2];
                            queue.Enqueue(new EventData(eventKeyOfTopicChange, new TopicChangeEventArgs(channel, topic)));
                            break;

                        // チャンネルのモード変更
                        case ReplyNumbers.RPL_CHANNELMODEIS:
                            channel = parameters[1];
                            queue.Enqueue(new EventData(eventKeyOfModeChange, new ModeChangeEventArgs(string.Empty, channel, parameters[2])));
                            break;

                        // それ以外
                        default:
                            queue.Enqueue(new EventData(eventKeyOfReceiveServerReply, new ReceiveServerReplyEventArgs(number, SliceArray(parameters, 1))));
                            break;
                    }

                    #endregion
                }
                else
                {
                    #region コマンド

                    // とりあえず parameter[0] を受信者とする
                    receiver = parameters[0];

                    switch (command.ToUpper())
                    {
                        #region メッセージ受信コマンド

                        case "PRIVMSG":
                            receivers = receiver.Split(',');
                            if (parameters[1].StartsWith("\x001") && parameters[1].EndsWith("\x001"))
                            {
                                // CTCPクエリを受信した場合
                                CtcpEventArgs arg = CreateCtcpEventArgs(sender, parameters[1]);
                                queue.Enqueue(new EventData(eventKeyOfReceiveCtcpQuery, arg));
                            }
                            else
                            {
                                foreach (string r in receivers)
                                {
                                    queue.Enqueue(new EventData(eventKeyOfReceiveMessage, 
                                        new ReceiveMessageEventArgs(sender, r, parameters[1].Replace("\x001", ""))));
                                }
                            }
                            break;

                        case "NOTICE":
                            receivers = receiver.Split(',');

                            if (parameters[1].StartsWith("\x001") && parameters[1].EndsWith("\x001"))
                            {
                                // CTCP Reply のとき
                                queue.Enqueue(new EventData(eventKeyOfReceiveCtcpReply,
                                    CreateCtcpEventArgs(sender, parameters[1])));
                            }
                            else
                            {
                                foreach (string r in receivers)
                                {
                                    // イベントを通知
                                    queue.Enqueue(new EventData(eventKeyOfReceiveNotice, 
                                        new ReceiveMessageEventArgs(sender, r, parameters[1].Replace("\x001", ""))));
                                }
                            }
                            break;

                        #endregion

                        case "PING":
                            SendCommand("PONG :" + string.Join(" ", parameters));
                            break;

                        case "JOIN":
                            ProcessUserInOut(queue, InOutCommands.Join, sender, receiver);
                            break;

                        case "QUIT":
                            ProcessUserInOut(queue, InOutCommands.Quit, sender, receiver);
                            break;

                        case "PART":
                            ProcessUserInOut(queue, InOutCommands.Leave, sender, receiver);
                            break;

                        case "KICK":
                            // イベントを通知
                            queue.Enqueue(new EventData(eventKeyOfKick, 
                                new KickEventArgs(sender, receiver, parameters[1])));
                            break;

                        case "NICK":
                            if (sender == m_userString)
                            {
                                // 自分の変更を受信したとき

                                // ユーザーストリングを更新
                                m_userString = receiver + m_userString.Substring(m_userString.IndexOf("!"));

                                // 自分のニックネームを変更
                                User.setNick(IRCClient.GetUserName(m_userString));
                                queue.Enqueue(new EventData(eventKeyOfChangedMyNickname, 
                                    new NickNameChangeEventArgs(IRCClient.GetUserName(sender), receiver)));
                            }
                            else
                            {
                                // 他人の名前の変更を受信したとき
                                queue.Enqueue(new EventData(eventKeyOfChangedNickname, 
                                    new NickNameChangeEventArgs(IRCClient.GetUserName(sender), receiver)));
                            }
                            break;

                        case "TOPIC":
                            queue.Enqueue(new EventData(eventKeyOfTopicChange,
                                new TopicChangeEventArgs(sender, receiver, parameters[1])));
                            break;

                        case "MODE":
                            if (parameters.Length > 1)
                            {
                                receivers = new string[parameters.Length - 1];
                                Array.Copy(parameters, 1, receivers, 0, receivers.Length);
                                queue.Enqueue(new EventData(eventKeyOfModeChange, 
                                    new ModeChangeEventArgs(sender, receiver, parameters[1], receivers)));
                            }
                            else if (parameters.Length == 1)
                            {
                                queue.Enqueue(new EventData(eventKeyOfModeChange, 
                                    new ModeChangeEventArgs(sender, receiver, parameters[1])));
                            }
                            break;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MessageParseException:" + ex.Message);
                throw new MessageParseException(ex);
            }
        }

        /// <summary>
        /// ユーザー入退室メッセージを処理する
        /// </summary>
        /// <param name="command">コマンド</param>
        /// <param name="sender">送信者</param>
        /// <param name="receiver">送信先</param>
        protected void ProcessUserInOut(Queue<EventData> queue, InOutCommands command, string sender, string receiver)
        {

            string[] targets = receiver.Split(',');

            // 受信先ごとにイベントを呼ぶ
            foreach (string target in targets)
            {
                queue.Enqueue(new EventData(eventKeyOfUserInOut, new UserInOutEventArgs(sender, command, target)));
            }
        }

        /// <summary>
        /// CTCPイベントデータを生成する
        /// </summary>
        /// <param name="sender">送信者</param>
        /// <param name="parameter">パラメータ</param>
        /// <returns>生成されたイベントデータ</returns>
        private CtcpEventArgs CreateCtcpEventArgs(string sender, string parameter)
        {
            // CTCP Query
            string query = parameter.Trim('\x001');
            string cmd;
            string param;
            int idx = query.IndexOf(" ");
            if (idx < 1)
            {
                cmd = query;
                param = string.Empty;
            }
            else
            {
                cmd = query.Substring(0, idx);
                param = query.Substring(idx + 1);
            }
            return new CtcpEventArgs(sender, cmd, param);
        }

        #endregion

        #region 送信系メソッド

        /// <summary>
        /// 接続コマンドの送信
        /// </summary>
        private void SendConnectCommand()
        {
            // パスワードが設定されていれば送信
            if (m_server.Password != string.Empty)
            {
                SendCommand(string.Format("PASS {0}", m_server.Password));
            }
            
            // ニックネームを送信
            ChangeNickname(m_user.NickName);

            // ユーザー情報送信
            SendCommand(string.Format("USER {0} {1} {2} :{3}", m_user.NickName, "LocalEndPoint", "RemoteEndPoint", m_user.RealName));
        }

        /// <summary>
        /// ニックネーム変更コマンドの送信
        /// </summary>
        /// <param name="newnickname">新しいニックネーム</param>
        public void ChangeNickname(string newnickname)
        {
            // ニックネーム送信
            SendCommand(string.Format("NICK {0}", newnickname));
        }

        /// <summary>
        /// コマンドの送信
        /// </summary>
        /// <param name="message">送信するコマンド</param>
        public void SendCommand(string message)
        {
            // 接続済みのときは追加
            if ((Status == IRCClientStatus.Connected) || (Status == IRCClientStatus.Online))
            {
                lock (m_sendQueue)
                {
                    m_sendQueue.Enqueue(message);
                }
                
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// チャンネルに参加
        /// </summary>
        /// <param name="channel">参加するチャンネル</param>
        public void JoinChannel(string channel)
        {
            SendCommand(string.Format("JOIN {0}", channel));
        }

        /// <summary>
        /// チャンネルから退室
        /// </summary>
        /// <param name="channel">退室するチャンネル</param>
        public void LeaveChannel(string channel)
        {
            SendCommand(string.Format("PART {0}", channel));
        }

        /// <summary>
        /// プライベートメッセージ送信
        /// </summary>
        /// <param name="receiver">送信先</param>
        /// <param name="message">メッセージ</param>
        public void SendPrivateMessage(string receiver, string message)
        {
            SendCommand(string.Format("PRIVMSG {0} :{1}", receiver, message));
        }

        /// <summary>
        /// notice メッセージ送信
        /// </summary>
        /// <param name="receiver">送信先</param>
        /// <param name="message">メッセージ</param>
        public void SendNoticeMessage(string receiver, string message)
        {
            SendCommand(string.Format("NOTICE {0} :{1}", receiver, message));
        }

        /// <summary>
        /// CTCPクエリ送信
        /// </summary>
        /// <param name="receiver">送信先</param>
        /// <param name="message">メッセージ</param>
        public void SendCtcpQuery(string receiver, string message)
        {
            SendPrivateMessage(receiver, string.Format("{0}{1}{0}", '\x001', message));
        }

        /// <summary>
        /// CTCPリプライ送信
        /// </summary>
        /// <param name="receiver">送信先</param>
        /// <param name="message">メッセージ</param>
        public void SendCtcpReply(string receiver, string message)
        {
            SendNoticeMessage(receiver, string.Format("{0}{1}{0}", '\x001', message));
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 接続ステータスを取得します
        /// </summary>
        public IRCClientStatus Status
        {
            get
            {
                // オンライン
                if (m_online)
                {
                    return IRCClientStatus.Online;
                }

                if (m_socket != null)
                {
                    // 接続済み
                    if (m_socket.Connected)
                    {
                        return IRCClientStatus.Connected;
                    }
                    // 接続中
                    if ((m_connectAsync != null) && (!m_connectAsync.IsCompleted))
                    {
                        return IRCClientStatus.EstablishConnection;
                    }
                }

                // 接続されていない
                return IRCClientStatus.Disconnected;
            }
        }

        /// <summary>
        /// サーバー情報を取得します
        /// </summary>
        public ServerInfo Server
        {
            get { return m_server; }
        }

        /// <summary>
        /// ユーザー情報を取得します
        /// </summary>
        public UserInfo User
        {
            get { return m_user; }
        }

        /// <summary>
        /// ユーザーフルネームを取得します
        /// </summary>
        public string UserString
        {
            get { return m_userString; }
        }

        /// <summary>
        /// エンコーディングを取得または設定します
        /// </summary>
        public Encoding Encoding
        {
            get { return m_encoding; }
            set { m_encoding = value; }
        }

        /// <summary>
        /// イベント通知先のコントロールを取得または設定します
        /// </summary>
        public Control OwnerControl
        {
            get { return m_ownerControl; }
            set { m_ownerControl = value; }
        }
	

        #endregion

        #region スタティックメソッド

        /// <summary>
        /// 配列の先頭の要素を指定された数だけ切り落とします
        /// </summary>
        /// <param name="array"></param>
        /// <param name="sliceLength"></param>
        /// <returns></returns>
        private static string[] SliceArray(string[] array, int sliceLength)
        {
            // 受信者の分のパラメータを切り詰める
            string[] sliceTempArray = new string[array.Length - 1];
            Array.Copy(array, sliceLength, sliceTempArray, 0, sliceTempArray.Length);
            return sliceTempArray;
        }

        /// <summary>
        /// ユーザーフルネームからユーザー名を取得します
        /// </summary>
        /// <param name="userstring"></param>
        /// <returns></returns>
        public static string GetUserName(string userstring)
        {
            int temp = userstring.IndexOf("!");
            if (temp > 0)
            {
                return userstring.Substring(0, temp);
            }
            else
            {
                return userstring;
            }
        }

        /// <summary>
        /// NICKコンフリクト時用の新しいニックネームを生成します。
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public static string GetNextNick(string nick)
        {
            // 最大文字数オーバー時は、最大文字数以内になるように削る
            if (nick.Length >= MaxNickLength)
            {
                nick = nick.Substring(0, MaxNickLength);
            }

            // 末尾の数字を取得する
            string name;
            string number;
            Match match = Regex.Match(nick, @"([^\d]*)(\d+)$");
            if (match.Success)
            {
                name = match.Groups[1].Value;
                number = (int.Parse(match.Groups[2].Value) + 1).ToString();
            }
            else
            {
                name = nick;
                number = "0";
            }

            // 名前を生成する
            if ((name.Length + number.Length) > MaxNickLength)
            {
                return name.Substring(0, MaxNickLength - number.Length) + number;
            }
            else
            {
                return name + number;
            }

        }

        /// <summary>
        /// 指定された文字列がチャンネル形式かどうか調べます。
        /// </summary>
        /// <param name="text">調べる文字列</param>
        /// <returns>チャンネルの形式の場合は true</returns>
        public static bool IsChannelString(string text)
        {
            if (text.StartsWith("#") || text.StartsWith("&")
                || text.StartsWith("+") || text.StartsWith("!"))
            {
                if (text.Length <= 50)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        #endregion

        #region 受信処理用内部クラス

        /// <summary>
        /// 受信処理のパース結果のイベントのデータを表すクラス
        /// </summary>
        protected class EventData
        {
            private object m_key;
            private EventArgs m_arg;

            /// <summary>
            /// イベントデータのキー
            /// </summary>
            public object EventKey
            {
                get { return m_key; }
            }

            /// <summary>
            /// イベントデータ
            /// </summary>
            public EventArgs Argument
            {
                get { return m_arg; }
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="eventKey">イベントキー</param>
            /// <param name="argument">イベントデータ</param>
            public EventData(object eventKey, EventArgs argument)
            {
                m_key = eventKey;
                m_arg = argument;
            }	
        }

        #endregion
    }

    /// <summary>
    /// IRCClient のステータスをあらわす定数
    /// </summary>
    public enum IRCClientStatus
    {
        /// <summary>
        /// 接続していない
        /// </summary>
        Disconnected,

        /// <summary>
        /// 接続処理中
        /// </summary>
        EstablishConnection,

        /// <summary>
        /// サーバーに接続済み
        /// </summary>
        Connected,

        /// <summary>
        /// 接続処理完了
        /// </summary>
        Online
    }
}
