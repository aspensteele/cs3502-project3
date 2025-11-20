using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FileManagementSystem
{
    public partial class Form1 : Form
    {
        // Store root path for building full paths
        private string rootPath;

        public Form1()
        {
            InitializeComponent();

            // Load user's folder on startup (e.g., C:\Users\YourName)
            rootPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            txtPath.Text = rootPath;
            lblInstructions.Visible = false;

            var rootNode = CreateNode(new DirectoryInfo(rootPath));
            rootNode.Text = rootPath; // Show full path for root node
            tvFiles.Nodes.Add(rootNode);
            tvFiles.Nodes[0].Expand(); // Auto-expand root so user sees contents

            // Event fires when user clicks a node in the tree
            tvFiles.AfterSelect += (s, e) =>
            {
                string path = GetFullPath(e.Node);
                txtPath.Text = path;

                if (File.Exists(path))
                    ShowFilePreview(path);
                else
                    ClearPreview();
            };
        }

        // Recursively creates tree nodes for directories and files
        private TreeNode CreateNode(DirectoryInfo dir)
        {
            var node = new TreeNode(dir.Name);

            try
            {
                // Add subdirectories (skip hidden)
                foreach (var subDir in dir.GetDirectories())
                {
                    // Bitwise AND checks if Hidden flag is set in file attributes
                    // FileAttributes is a flags enum, so multiple attributes can be combined
                    if ((subDir.Attributes & FileAttributes.Hidden) != 0) continue;
                    node.Nodes.Add(CreateNode(subDir)); 
                }

                // Add files (skip hidden)
                foreach (var file in dir.GetFiles())
                {
                    if ((file.Attributes & FileAttributes.Hidden) != 0) continue;
                    node.Nodes.Add(new TreeNode(file.Name));
                }
            }
            catch (UnauthorizedAccessException) { } // Silently skip folders we can't access

            return node;
        }

        // Builds full path by walking up the tree from selected node to root
        private string GetFullPath(TreeNode node)
        {
            var parts = new System.Collections.Generic.List<string>();

            // Walk up the tree, collecting each node's text
            while (node != null)
            {
                parts.Insert(0, node.Text); // Insert at beginning to maintain order
                node = node.Parent;
            }

            // Skip first part (root node text) since rootPath already contains it
            // Then combine remaining parts into a valid file path
            if (parts.Count > 1)
                return Path.Combine(rootPath, Path.Combine(parts.Skip(1).ToArray()));

            return rootPath;
        }

        // Shows image or text preview based on file type
        private void ShowFilePreview(string path)
        {
            var ext = Path.GetExtension(path).ToLower();

            // Image files
            string[] imageExts = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".ico" };
            if (imageExts.Contains(ext))
            {
                txtFileContent.Visible = false;
                pbPreview.Visible = true;
                pbPreview.Image?.Dispose(); // Free memory from previous image (?.Dispose = only if not null)
                pbPreview.Image = Image.FromFile(path);
                return;
            }

            // Text files
            string[] textExts = { ".txt", ".cs", ".json", ".xml", ".html", ".css", ".js", ".md", ".log" };
            if (textExts.Contains(ext))
            {
                pbPreview.Visible = false;
                txtFileContent.Visible = true;
                try { txtFileContent.Text = File.ReadAllText(path); }
                catch { txtFileContent.Text = "Error reading file"; }
                return;
            }

            ClearPreview();
        }

        // Clears the preview area
        private void ClearPreview()
        {
            pbPreview.Image?.Dispose(); // Free memory
            pbPreview.Image = null;
            pbPreview.Visible = false;
            txtFileContent.Text = "";
            txtFileContent.Visible = true;
        }
    }
}