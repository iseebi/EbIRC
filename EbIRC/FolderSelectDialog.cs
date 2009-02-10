using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace EbiSoft.Library.Mobile
{
    /// <summary>
    /// フォルダを選択するダイアログを提供します
    /// </summary>
    public partial class FolderSelectDialog : Form
    {
        private const string DUMMY_NODE_STR = "**DUMMY**";
        private TreeNode rootNode;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FolderSelectDialog()
        {
            InitializeComponent();
            folderTreeView.PathSeparator = Path.DirectorySeparatorChar.ToString();
            rootNode = new TreeNode("\\");
            rootNode.Nodes.Add(DUMMY_NODE_STR);
            folderTreeView.Nodes.Clear();
            folderTreeView.Nodes.Add(rootNode);
        }

        /// <summary>
        /// 選択されているフォルダ
        /// </summary>
        public string SelectedDirectory
        {
            get
            {
                string path = folderTreeView.SelectedNode.FullPath;
                if (path.StartsWith("\\\\"))
                {
                    path = path.Substring(1);
                }
                return path;
            }
            set {
                DrillToDirectory(value); 
            }
        }

        /// <summary>
        /// 指定されたフォルダまで階層を進める
        /// </summary>
        /// <param name="targetDir">対象のフォルダ</param>
        private void DrillToDirectory(string targetDir)
        {
            folderTreeView.SuspendLayout();

            string[] directories = (("**ROOT**" + targetDir) as string).Split(Path.DirectorySeparatorChar);
            string drilled = string.Empty;
            TreeNode currentNode = null;
            foreach (string entry in directories)
            {
                if (entry == "**ROOT**")
                {
                    drilled = Path.DirectorySeparatorChar.ToString();
                    folderTreeView.SelectedNode = rootNode;
                }
                else
                {
                    CreateChildTree(currentNode);
                    foreach (TreeNode searchNode in currentNode.Nodes)
                    {
                        if (searchNode.Text.ToUpper() == entry.ToUpper())
                        {
                            folderTreeView.SelectedNode = searchNode;
                            break;
                        }
                    }
                    if (folderTreeView.SelectedNode == currentNode)
                    {
                        break;
                    }
                }
                currentNode = folderTreeView.SelectedNode;
            }

            folderTreeView.ResumeLayout();
        }

        /// <summary>
        /// 子ツリーを作成します。
        /// </summary>
        /// <param name="node"></param>
        private void CreateChildTree(TreeNode node)
        {
            // ダミーノードしかなければクリア
            if ((node.Nodes.Count == 1) && (node.Nodes[0].Text == DUMMY_NODE_STR))
            {
                node.Nodes.Clear();
            }
            else
            {
                // ノードがあれば何もしない
                return;
            }

            // ディレクトリのノードを追加
            string path = node.FullPath;
            if (path.StartsWith("\\\\"))
            {
                path = path.Substring(1);
            }
            foreach (string directory in System.IO.Directory.GetDirectories(path))
            {
                TreeNode newNode = new TreeNode(Path.GetFileName(directory));
                newNode.Nodes.Add(DUMMY_NODE_STR);
                newNode.Collapse();
                node.Nodes.Add(newNode);
            }
        }

        /// <summary>
        /// ツリー展開時イベント
        /// </summary>
        private void folderTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            CreateChildTree(e.Node);
        }

        private void acceptMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

    }
}