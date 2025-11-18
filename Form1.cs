using System;
using System.IO;
using System.Windows.Forms;

namespace FileManagementSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Set initial current directory
            string currentDirectory = Environment.CurrentDirectory;
            txtPath.Text = currentDirectory;

            // Optional: populate TreeView with this directory
            PopulateTree(currentDirectory);
        }

        private void PopulateTree(string path)
        {
            tvFiles.Nodes.Clear();
            TreeNode rootNode = new TreeNode(path) { Tag = path };
            tvFiles.Nodes.Add(rootNode);
            AddDirectories(rootNode);
            rootNode.Expand();
        }

        private void AddDirectories(TreeNode node)
        {
            string path = node.Tag.ToString();

            try
            {
                foreach (var dir in Directory.GetDirectories(path))
                {
                    TreeNode dirNode = new TreeNode(Path.GetFileName(dir)) { Tag = dir };
                    node.Nodes.Add(dirNode);
                    AddDirectories(dirNode); // recursively add subdirectories
                }

                foreach (var file in Directory.GetFiles(path))
                {
                    TreeNode fileNode = new TreeNode(Path.GetFileName(file)) { Tag = file };
                    node.Nodes.Add(fileNode);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Ignore folders you don't have access to
            }
        }
    }
}
