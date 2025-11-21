// Form1.cs

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic; // Add this for IEnumerable

namespace FileManagementSystem
{
    public partial class Form1 : Form
    {
        // Store root path for building full paths
        private string rootPath;
        private FileSystemManager _fileSystemManager; // Declare an instance of our manager

        // Define constants for file extensions to enhance readability and maintainability
        private static readonly string[] ImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".ico" };
        private static readonly string[] TextExtensions = { ".txt", ".cs", ".json", ".xml", ".html", ".css", ".js", ".md", ".log" };


        public Form1()
        {
            InitializeComponent();

            _fileSystemManager = new FileSystemManager(); // Initialize the manager

            // Load user's folder on startup (e.g., C:\Users\YourName)
            rootPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            txtPath.Text = rootPath;
            lblInstructions.Visible = false;

            // Load root node lazily
            // The Tag property will now store our custom FileSystemEntry object
            var rootEntry = new FileSystemEntry(rootPath, rootPath, FileSystemEntryType.Directory, File.GetAttributes(rootPath));
            var rootNode = new TreeNode(rootEntry.Name) { Tag = rootEntry };
            rootNode.Nodes.Add("..."); // Placeholder for lazy loading
            tvFiles.Nodes.Add(rootNode);

            tvFiles.BeforeExpand += TvFiles_BeforeExpand;

            // Event fires when user clicks a node in the tree
            tvFiles.AfterSelect += (s, e) =>
            {
                // Retrieve our custom FileSystemEntry from the Tag
                var selectedEntry = e.Node.Tag as FileSystemEntry;
                if (selectedEntry == null) return; // Should not happen if Tag is always set

                txtPath.Text = selectedEntry.FullPath;

                if (selectedEntry.Type == FileSystemEntryType.File)
                    ShowFilePreview(selectedEntry.FullPath);
                else
                    ClearPreview();
            };

            btnCreate.Click += BtnCreate_Click;
            // TODO: Add click handlers for btnUpdate, btnDelete, btnRename once implemented
        }


        // Creates a new file in the selected directory
        private void BtnCreate_Click(object sender, EventArgs e)
        {
            string targetDir = GetSelectedDirectoryPath();
            if (string.IsNullOrEmpty(targetDir)) return;

            // Prompt user for filename
            string fileName = PromptForInput("Create New File", "Enter file name:");
            if (string.IsNullOrWhiteSpace(fileName))
            {
                lblStatus.Text = "File creation cancelled or invalid name.";
                return;
            }

            string newPath = Path.Combine(targetDir, fileName);

            // UI check for existing file (to give immediate feedback)
            if (File.Exists(newPath))
            {
                MessageBox.Show(this, "A file or directory with that name already exists. Please choose a different name.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Call the FileSystemManager to create the file
                _fileSystemManager.CreateFile(newPath);

                // Refresh only the selected folder
                TreeNode selected = tvFiles.SelectedNode ?? tvFiles.Nodes[0];
                RefreshNode(selected);

                lblStatus.Text = $"Created: {fileName}";
                MessageBox.Show(this, $"File created: {fileName}", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (IOException ex)
            {
                MessageBox.Show(this, $"Error creating file: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = $"Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"An unexpected error occurred: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = $"Error: {ex.Message}";
            }
        }


        // Loads children only when node is expanded (improves performance)
        private void TvFiles_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode node = e.Node;
            // Retrieve our custom FileSystemEntry from the Tag
            var nodeEntry = node.Tag as FileSystemEntry;
            if (nodeEntry == null || nodeEntry.Type == FileSystemEntryType.File) return; // Only load for directories

            // If dummy node exists, we must load actual children
            if (node.Nodes.Count == 1 && node.Nodes[0].Text == "...")
            {
                node.Nodes.Clear();
                LoadNodeChildren(node);
            }
        }

        // Populates a node with its subdirectories and files
        private void LoadNodeChildren(TreeNode parentNode)
        {
            var parentEntry = parentNode.Tag as FileSystemEntry;
            if (parentEntry == null) return;

            try
            {
                // Use the FileSystemManager to get the contents
                IEnumerable<FileSystemEntry> children = _fileSystemManager.GetDirectoryContents(parentEntry.FullPath);

                foreach (var entry in children)
                {
                    // Skip hidden entries as per project requirement
                    if (entry.IsHidden) continue;

                    TreeNode newNode = new TreeNode(entry.Name)
                    {
                        Tag = entry // Store the custom FileSystemEntry in the Tag
                    };

                    // Add lazy load placeholder only for directories
                    if (entry.Type == FileSystemEntryType.Directory)
                    {
                        newNode.Nodes.Add("...");
                    }
                    parentNode.Nodes.Add(newNode);
                }
            }
            // The FileSystemManager handles specific exceptions, so here we might just catch a general one
            // or rely on the manager's internal logging if silent skipping is desired for UI.
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading contents of {parentEntry.Name}: {ex.Message}";
                // Optionally, display a MessageBox for more critical errors
                // MessageBox.Show(this, $"Error loading directory: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Refresh only ONE folder after creation (fast, avoids full tree reload)
        private void RefreshNode(TreeNode node)
        {
            if (node == null) return;

            // Clear existing children
            node.Nodes.Clear();

            // Check if the node itself is a directory (it should be if we're refreshing it)
            var nodeEntry = node.Tag as FileSystemEntry;
            if (nodeEntry == null || nodeEntry.Type == FileSystemEntryType.File)
            {
                // If it was a file, or corrupted tag, we can't refresh its children
                return;
            }

            // Add the lazy load placeholder back for directories, then expand
            node.Nodes.Add("...");

            // Temporarily detach the event handler to force immediate expansion and loading
            tvFiles.BeforeExpand -= TvFiles_BeforeExpand;
            // Manually trigger the logic that BeforeExpand would normally perform
            TvFiles_BeforeExpand(tvFiles, new TreeViewCancelEventArgs(node, false, TreeViewAction.Expand));
            tvFiles.BeforeExpand += TvFiles_BeforeExpand;

            // Ensure the node is expanded in the UI if it was before refreshing
            node.Expand();
        }

        // Returns the directory path of the current selection
        private string GetSelectedDirectoryPath()
        {
            // If nothing is selected, default to rootPath
            if (tvFiles.SelectedNode == null) return rootPath;

            var selectedEntry = tvFiles.SelectedNode.Tag as FileSystemEntry;
            if (selectedEntry == null) return rootPath; // Should ideally not happen

            // If a file is selected, return its parent directory
            if (selectedEntry.Type == FileSystemEntryType.File)
                return Path.GetDirectoryName(selectedEntry.FullPath);

            // If a directory is selected, return its own path
            return selectedEntry.FullPath;
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

            if (ImageExtensions.Contains(ext))
            {
                txtFileContent.Visible = false;
                pbPreview.Visible = true;
                pbPreview.Image?.Dispose(); // Free memory from previous image
                try
                {
                    pbPreview.Image = Image.FromFile(path);
                }
                catch (OutOfMemoryException)
                {
                    ClearPreview();
                    lblStatus.Text = "Error: Image file is too large or corrupted to preview.";
                }
                catch (Exception ex)
                {
                    ClearPreview();
                    lblStatus.Text = $"Error loading image: {ex.Message}";
                }
                return;
            }

            if (TextExtensions.Contains(ext))
            {
                pbPreview.Visible = false;
                txtFileContent.Visible = true;
                try
                {
                    txtFileContent.Text = _fileSystemManager.ReadTextFile(path); // Use FileSystemManager
                }
                catch (IOException ex)
                {
                    txtFileContent.Text = $"Error reading file: {ex.Message}";
                }
                catch (Exception ex)
                {
                    txtFileContent.Text = $"An unexpected error occurred while reading file: {ex.Message}";
                }
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
            txtFileContent.Visible = true; // Default to text content area visible, but empty
        }
    }
}