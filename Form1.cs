using System;
using System.Windows.Forms;
using System.IO;

namespace FileManagementSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Event for open button
            btnOpen.Click += BtnOpen_Click;
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtPath.Text = fbd.SelectedPath;
                    lblInstructions.Visible = false; // hide instruction
                    PopulateTree(fbd.SelectedPath);
                }
            }
        }

        private void PopulateTree(string path)
        {
            tvFiles.Nodes.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            tvFiles.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));
        }

        private TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeNode(directoryInfo.Name);
            foreach (var directory in directoryInfo.GetDirectories())
                directoryNode.Nodes.Add(CreateDirectoryNode(directory));
            foreach (var file in directoryInfo.GetFiles())
                directoryNode.Nodes.Add(new TreeNode(file.Name));
            return directoryNode;
        }
    }
}
