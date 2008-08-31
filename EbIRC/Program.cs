using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Resources;
using System.IO;
using System.Diagnostics;

namespace EbiSoft.EbIRC
{
    static class Program
    {
        private static readonly string crashReportFile = Path.Combine(Path.GetDirectoryName(
        System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName),
        Properties.Resources.ResourceManager.GetString("CrashReportFile"));

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [MTAThread]
        static void Main()
        {
            // デフォルト例外ハンドラの定義
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

#if DEBUG
            /*
            // トレースログ
            System.Diagnostics.Debug.Listeners.Add(
                new TextWriterTraceListener(
                    Path.Combine(
                        Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName), 
                        "EbIRC_TraceLog.txt"
                    )
                )
            );
            */ 
#endif

            Application.Run(new EbIrcMainForm());
        }

        /// <summary>
        /// デフォルト例外ハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception) e.ExceptionObject;
                using (StreamWriter writer = new StreamWriter(crashReportFile, true))
                {
                    writer.WriteLine("BEGIN-------------------------------------------");
                    writer.WriteLine(string.Format("UnhandledExecption {0}", DateTime.Now.ToString()));
                    writer.WriteLine("------------------------------------------------");
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine("END---------------------------------------------");
                }
                MessageBox.Show(
                    Properties.Resources.ResourceManager.GetString("CrashMessage"), "例外",
                    MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
            catch (Exception)
            {
            }
        }
    }
}