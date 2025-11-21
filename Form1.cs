// Form1.cs - Your custom logic and event handlers go here

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace FileManagementSystem
{
    public partial class Form1 : Form // This is the 'other part' of the Form1 class
    {
        private string rootPath;
        private FileSystemManager _fileSystemManager;

        private static readonly string[] ImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".ico" };
        private static readonly string[] TextExtensions = { ".txt", ".cs", ".json", ".xml", ".html", ".css", ".js", ".md", ".log" };

        private string _currentEditedFilePath = null;


        public Form1()
        {
            InitializeComponent(); // This calls the method from Form1.Designer.cs

            _fileSystemManager = new FileSystemManager();

            // Set initial root path and handle potential issues
            rootPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            FileAttributes rootAttributes = FileAttributes.Normal;
            try
            {
                if (Directory.Exists(rootPath))
                {
                    rootAttributes = File.GetAttributes(rootPath);
                }
                else
                {
                    rootPath = AppDomain.CurrentDomain.BaseDirectory; // Fallback to application directory
                    rootAttributes = File.GetAttributes(rootPath);
                    MessageBox.Show(this, $"User profile path not found, defaulting to application directory: {rootPath}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // More robust fallback if even getting attributes fails
                rootPath = AppDomain.CurrentDomain.BaseDirectory;
                rootAttributes = FileAttributes.Directory; // Assume it's a directory
                MessageBox.Show(this, $"Error accessing user profile path. Defaulting to application directory: {rootPath}\nError: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            txtPath.Text = rootPath;
            lblInstructions.Visible = false; // Hide instructions after initial load

            // Initialize root node in TreeView
            var rootEntry = new FileSystemEntry(Path.GetFileName(rootPath), rootPath, FileSystemEntryType.Directory, rootAttributes);
            var rootNode = new TreeNode(rootEntry.Name) { Tag = rootEntry };
            rootNode.Nodes.Add("..."); // Placeholder for lazy loading
            tvFiles.Nodes.Add(rootNode);

            // Wire up event handlers
            tvFiles.BeforeExpand += TvFiles_BeforeExpand;
            tvFiles.AfterSelect += TvFiles_AfterSelect; // Use a specific handler method for clarity
            btnCreate.Click += BtnCreate_Click;
            btnSave.Click += BtnSave_Click;
            btnRefresh.Click += BtnRefresh_Click; // Assuming you have a btnRefresh in designer
            // TODO: Add click handlers for btnDelete, btnRename, btnOpen
        }


        // Event handler for TreeView node selection
        private void TvFiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selectedEntry = e.Node.Tag as FileSystemEntry;
            if (selectedEntry == null)
            {
                ClearContentArea(); // Clear if selection is invalid or null
                return;
            }

            txtPath.Text = selectedEntry.FullPath;

            if (selectedEntry.Type == FileSystemEntryType.File)
                ShowFileContent(selectedEntry.FullPath);
            else
                ClearContentArea(); // Clear content area if a directory is selected
        }

        // Handler for the Refresh button
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            // Refresh the currently selected node, or the root if nothing is selected
            TreeNode selectedNode = tvFiles.SelectedNode ?? tvFiles.Nodes[0];
            if (selectedNode != null)
            {
                var entry = selectedNode.Tag as FileSystemEntry;
                if (entry != null && entry.Type == FileSystemEntryType.Directory)
                {
                    RefreshNode(selectedNode);
                    lblStatus.Text = $"Refreshed: {entry.Name}";
                }
                else
                {
                    lblStatus.Text = "Cannot refresh a file. Select a directory.";
                }
            }
        }

        // Handler for the Save button
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentEditedFilePath))
            {
                MessageBox.Show(this, "No file is currently selected for saving.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if it's a new file (doesn't exist yet on disk)
            bool isNewFile = !_fileSystemManager.PathExists(_currentEditedFilePath);

            try
            {
                // If it's a new file, ensure its parent directory exists
                string parentDir = Path.GetDirectoryName(_currentEditedFilePath);
                if (!string.IsNullOrEmpty(parentDir) && !_fileSystemManager.IsDirectory(parentDir))
                {
                    // This implies a serious issue if targetDir wasn't valid, but good for robustness.
                    _fileSystemManager.CreateDirectory(parentDir);
                }

                _fileSystemManager.WriteTextFile(_currentEditedFilePath, txtFileContent.Text);

                lblStatus.Text = $"Saved: {Path.GetFileName(_currentEditedFilePath)}";
                MessageBox.Show(this, $"File saved: {Path.GetFileName(_currentEditedFilePath)}", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // If it was a new file, refresh the parent node to show it
                if (isNewFile)
                {
                    string parentDirPath = Path.GetDirectoryName(_currentEditedFilePath);
                    TreeNode parentNode = FindNodeByPath(parentDirPath);
                    if (parentNode != null)
                    {
                        RefreshNode(parentNode);
                        // After refreshing, try to select the newly created node
                        // This ensures the new file is visible and selected
                        foreach (TreeNode node in parentNode.Nodes)
                        {
                            var entry = node.Tag as FileSystemEntry;
                            if (entry != null && entry.FullPath.Equals(_currentEditedFilePath, StringComparison.OrdinalIgnoreCase))
                            {
                                tvFiles.SelectedNode = node;
                                node.EnsureVisible(); // Scrolls the tree view to make the node visible
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Fallback: If parent node not found (e.g., root not fully loaded), refresh root
                        RefreshNode(tvFiles.Nodes[0]);
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(this, $"Error saving file: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = $"Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"An unexpected error occurred while saving: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = $"Error: {ex.Message}";
            }
        }


        // Handler for the Create button
        private void BtnCreate_Click(object sender, EventArgs e)
        {
            string targetDir = GetSelectedDirectoryPath();
            if (string.IsNullOrEmpty(targetDir))
            {
                MessageBox.Show(this, "Please select a directory to create the new file in.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string fileName = PromptForInput("Create New File", "Enter file name:");
            if (string.IsNullOrWhiteSpace(fileName))
            {
                lblStatus.Text = "File creation cancelled or invalid name.";
                return;
            }

            string newPath = Path.Combine(targetDir, fileName);

            // Check if file or directory already exists at the new path using FileSystemManager
            if (_fileSystemManager.PathExists(newPath))
            {
                MessageBox.Show(this, "A file or directory with that name already exists. Please choose a different name.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Set the content area for a new, unsaved file
            _currentEditedFilePath = newPath;
            txtFileContent.Text = ""; // Start with empty content
            txtFileContent.ReadOnly = false; // Enable editing
            txtFileContent.Visible = true;
            pbPreview.Visible = false;
            btnSave.Visible = true; // Show Save button to persist the new file

            lblStatus.Text = $"New file '{fileName}' created. Enter content and save.";
            // The file itself is not created on disk until BtnSave_Click is called.
        }


        // Handles lazy loading of TreeView nodes
        private void TvFiles_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode node = e.Node;
            var nodeEntry = node.Tag as FileSystemEntry;
            // Only load children for directories and if it has the placeholder
            if (nodeEntry == null || nodeEntry.Type == FileSystemEntryType.File || node.Nodes.Count != 1 || node.Nodes[0].Text != "...")
            {
                return;
            }

            node.Nodes.Clear(); // Remove the placeholder
            LoadNodeChildren(node); // Load actual children
        }

        // Populates a TreeView node with its subdirectories and files
        private void LoadNodeChildren(TreeNode parentNode)
        {
            var parentEntry = parentNode.Tag as FileSystemEntry;
            if (parentEntry == null) return;

            try
            {
                IEnumerable<FileSystemEntry> children = _fileSystemManager.GetDirectoryContents(parentEntry.FullPath);

                foreach (var entry in children)
                {
                    if (entry.IsHidden) continue; // Skip hidden entries

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
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading contents of {parentEntry.Name}: {ex.Message}";
                MessageBox.Show(this, $"Error loading directory contents: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Refreshes a specific TreeView node (reloads its children)
        private void RefreshNode(TreeNode node)
        {
            if (node == null) return;

            bool wasExpanded = node.IsExpanded; // Store current expansion state

            node.Nodes.Clear(); // Clear existing children

            var nodeEntry = node.Tag as FileSystemEntry;
            if (nodeEntry == null || nodeEntry.Type == FileSystemEntryType.File)
            {
                return; // Cannot refresh children of a file
            }

            node.Nodes.Add("..."); // Re-add placeholder for lazy loading

            // Temporarily detach the event handler to prevent infinite loops during programmatic expansion
            tvFiles.BeforeExpand -= TvFiles_BeforeExpand;
            // Manually trigger the logic that BeforeExpand would normally perform
            TvFiles_BeforeExpand(tvFiles, new TreeViewCancelEventArgs(node, false, TreeViewAction.Expand));
            // Re-attach the event handler
            tvFiles.BeforeExpand += TvFiles_BeforeExpand;

            // Restore expansion state
            if (wasExpanded)
            {
                node.Expand();
            }
        }

        // Returns the full path of the selected directory (or parent directory if a file is selected)
        private string GetSelectedDirectoryPath()
        {
            if (tvFiles.SelectedNode == null) return rootPath; // Default to root if nothing selected

            var selectedEntry = tvFiles.SelectedNode.Tag as FileSystemEntry;
            if (selectedEntry == null) return rootPath;

            if (selectedEntry.Type == FileSystemEntryType.File)
                return Path.GetDirectoryName(selectedEntry.FullPath);

            return selectedEntry.FullPath; // If a directory is selected, return its own path
        }

        // Displays a custom input dialog for user input
        private string PromptForInput(string title, string prompt)
        {
            using (var form = new Form())
            {
                form.Text = title;
                form.Size = new Size(380, 200);
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

        // Displays file content (image or editable text) in the preview area
        private void ShowFileContent(string path)
        {
            _currentEditedFilePath = null; // Reset edited path when a new selection is made
            btnSave.Visible = false;       // Hide save button by default

            var ext = Path.GetExtension(path).ToLower();

            if (ImageExtensions.Contains(ext))
            {
                txtFileContent.Visible = false;
                pbPreview.Visible = true;
                pbPreview.Image?.Dispose(); // Release previous image resource
                try
                {
                    // Using FileStream to avoid file locking issues when displaying images
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        pbPreview.Image = Image.FromStream(fs);
                    }
                    lblStatus.Text = $"Previewing image: {Path.GetFileName(path)}";
                }
                catch (OutOfMemoryException)
                {
                    ClearContentArea();
                    lblStatus.Text = "Error: Image file is too large or corrupted to preview.";
                }
                catch (Exception ex)
                {
                    ClearContentArea();
                    lblStatus.Text = $"Error loading image: {ex.Message}";
                }
                return;
            }

            if (TextExtensions.Contains(ext))
            {
                _currentEditedFilePath = path; // Set current file for potential editing
                pbPreview.Visible = false;
                txtFileContent.Visible = true;
                txtFileContent.ReadOnly = false; // Enable editing for text files
                btnSave.Visible = true;          // Show Save button

                try
                {
                    txtFileContent.Text = _fileSystemManager.ReadTextFile(path);
                    lblStatus.Text = $"Viewing and editing: {Path.GetFileName(path)}";
                }
                catch (IOException ex)
                {
                    txtFileContent.Text = $"Error reading file: {ex.Message}";
                    txtFileContent.ReadOnly = true; // Make read-only on error
                    btnSave.Visible = false;        // Hide save button on error
                    lblStatus.Text = $"Error reading {Path.GetFileName(path)}: {ex.Message}";
                }
                catch (Exception ex)
                {
                    txtFileContent.Text = $"An unexpected error occurred while reading file: {ex.Message}";
                    txtFileContent.ReadOnly = true; // Make read-only on error
                    btnSave.Visible = false;        // Hide save button on error
                    lblStatus.Text = $"Unexpected error reading {Path.GetFileName(path)}: {ex.Message}";
                }
                return;
            }

            // For other file types, clear the content area
            ClearContentArea();
        }

        // Clears the content/preview area and resets its state
        private void ClearContentArea()
        {
            _currentEditedFilePath = null; // No file is being edited
            btnSave.Visible = false;       // Hide save button

            pbPreview.Image?.Dispose(); // Free image resource
            pbPreview.Image = null;
            pbPreview.Visible = false;

            txtFileContent.Text = "";
            txtFileContent.ReadOnly = true; // Make content area read-only by default
            txtFileContent.Visible = true;  // Keep visible, but empty and read-only
            lblStatus.Text = "No file selected or file type not previewable/editable.";
        }

        // Helper method to find a TreeNode by its full path (used for refreshing after changes)
        private TreeNode FindNodeByPath(string path)
        {
            foreach (TreeNode node in tvFiles.Nodes)
            {
                TreeNode found = FindNodeByPathRecursive(node, path);
                if (found != null) return found;
            }
            return null;
        }

        // Recursive helper for FindNodeByPath
        private TreeNode FindNodeByPathRecursive(TreeNode currentNode, string path)
        {
            var entry = currentNode.Tag as FileSystemEntry;
            if (entry != null && entry.FullPath.Equals(path, StringComparison.OrdinalIgnoreCase))
            {
                return currentNode;
            }

            // Only search expanded nodes or nodes that have the placeholder (meaning children haven't been loaded yet)
            // If the target path is deep, its parent nodes might need to be explicitly expanded/loaded.
            if (currentNode.IsExpanded || (currentNode.Nodes.Count == 1 && currentNode.Nodes[0].Text == "..."))
            {
                foreach (TreeNode childNode in currentNode.Nodes)
                {
                    // If a child node is a directory and not yet loaded (has placeholder),
                    // we can temporarily load it to search within it.
                    var childEntry = childNode.Tag as FileSystemEntry;
                    if (childEntry != null && childEntry.Type == FileSystemEntryType.Directory &&
                        childNode.Nodes.Count == 1 && childNode.Nodes[0].Text == "...")
                    {
                        // Temporarily expand/load children to search deeper
                        // Detach/reattach BeforeExpand to avoid infinite loops
                        tvFiles.BeforeExpand -= TvFiles_BeforeExpand;
                        TvFiles_BeforeExpand(tvFiles, new TreeViewCancelEventArgs(childNode, false, TreeViewAction.Expand));
                        tvFiles.BeforeExpand += TvFiles_BeforeExpand;
                    }

                    TreeNode found = FindNodeByPathRecursive(childNode, path);
                    if (found != null) return found;
                }
            }
            return null;
        }
    }
}