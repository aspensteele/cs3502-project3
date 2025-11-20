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

            // Load root node lazily
            var rootNode = new TreeNode(rootPath) { Tag = rootPath };
            rootNode.Nodes.Add("..."); // Placeholder for lazy loading
            tvFiles.Nodes.Add(rootNode);

            tvFiles.BeforeExpand += TvFiles_BeforeExpand;

            // Event fires when user clicks a node in the tree
            tvFiles.AfterSelect += (s, e) =>
            {
                string path = e.Node.Tag as string;
                txtPath.Text = path;

                if (File.Exists(path))
                    ShowFilePreview(path);
                else
                    ClearPreview();
            };

            btnCreate.Click += BtnCreate_Click;
        }


        // Creates a new file in the selected directory
        private void BtnCreate_Click(object sender, EventArgs e)
        {
            string targetDir = GetSelectedDirectory();
            if (targetDir == null) return;

            // Prompt user for filename
            string fileName = PromptForInput("Create New File", "Enter file name:");
            if (string.IsNullOrWhiteSpace(fileName)) return;

            string newPath = Path.Combine(targetDir, fileName);

            // Check if file already exists
            if (File.Exists(newPath))
            {
                MessageBox.Show(this, "A file with that name already exists.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (File.Create(newPath)) { } // Create and properly close the file handle

                // Refresh only the selected folder (avoids freezing)
                TreeNode selected = tvFiles.SelectedNode ?? tvFiles.Nodes[0];
                RefreshNode(selected);

                lblStatus.Text = $"Created: {fileName}";
                MessageBox.Show(this, $"File created: {fileName}", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Error creating file: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Loads children only when node is expanded (improves performance)
        private void TvFiles_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode node = e.Node;

            // If dummy node exists, we must load actual children
            if (node.Nodes.Count == 1 && node.Nodes[0].Text == "...")
            {
                node.Nodes.Clear();
                LoadNodeChildren(node);
            }
        }

        // Populates a node with its subdirectories and files
        private void LoadNodeChildren(TreeNode node)
        {
            string fullPath = node.Tag as string;

            try
            {
                // Add subdirectories (skip hidden)
                foreach (var dir in Directory.GetDirectories(fullPath))
                {
                    var info = new DirectoryInfo(dir);
                    // Bitwise AND checks if Hidden flag is set in file attributes
                    // FileAttributes is a flags enum, so multiple attributes can be combined
                    if ((info.Attributes & FileAttributes.Hidden) != 0) continue;

                    var newNode = new TreeNode(info.Name)
                    {
                        Tag = info.FullName
                    };
                    newNode.Nodes.Add("..."); // Lazy load placeholder
                    node.Nodes.Add(newNode);
                }

                // Add files (skip hidden)
                foreach (var file in Directory.GetFiles(fullPath))
                {
                    var info = new FileInfo(file);
                    if ((info.Attributes & FileAttributes.Hidden) != 0) continue;

                    node.Nodes.Add(new TreeNode(info.Name)
                    {
                        Tag = info.FullName
                    });
                }
            }
            catch (UnauthorizedAccessException) { } // Silently skip folders we can't access
            catch { }
        }

        // Refresh only ONE folder after creation (fast, avoids full tree reload)
        private void RefreshNode(TreeNode node)
        {
            if (node == null) return;

            node.Nodes.Clear();
            node.Nodes.Add("..."); // Reset to placeholder

            // Force it to expand and load children
            tvFiles.BeforeExpand -= TvFiles_BeforeExpand;
            TvFiles_BeforeExpand(tvFiles, new TreeViewCancelEventArgs(node, false, TreeViewAction.Expand));
            tvFiles.BeforeExpand += TvFiles_BeforeExpand;
        }



        // Returns the directory path of the current selection
        private string GetSelectedDirectory()
        {
            if (tvFiles.SelectedNode == null) return rootPath;

            string path = tvFiles.SelectedNode.Tag as string;

            // If a file is selected, return its parent directory
            if (File.Exists(path))
                return Path.GetDirectoryName(path);

            return path;
        }

        // Simple input dialog using a form
        private string PromptForInput(string title, string prompt)
        {
            using (var form = new Form())
            {
                form.Text = title;
                form.Size = new Size(380, 200);   // taller form so buttons fit
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MaximizeBox = false;
                form.MinimizeBox = false;
                form.TopMost = true;

                var lbl = new Label { Text = prompt, Left = 10, Top = 15, Width = 350 };
                var txt = new TextBox { Left = 10, Top = 45, Width = 350 };

                var btnOk = new Button { Text = "OK", Left = 190, Top = 100, Width = 80, Height = 35, DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "Cancel", Left = 280, Top = 100, Width = 80, Height = 35, DialogResult = DialogResult.Cancel };

                form.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnCancel });
                form.AcceptButton = btnOk;
                form.CancelButton = btnCancel;

                var result = form.ShowDialog(this);
                return result == DialogResult.OK ? txt.Text.Trim() : null;
            }
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