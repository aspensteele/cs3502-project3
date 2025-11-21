// Form1.cs

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace FileManagementSystem
{
    public partial class Form1 : Form
    {
        private string rootPath;
        private FileSystemManager _fileSystemManager;

        private static readonly string[] ImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".ico" };
        private static readonly string[] TextExtensions = { ".txt", ".cs", ".json", ".xml", ".html", ".css", ".js", ".md", ".log" };

        private string _currentEditedFilePath = null; // Path of the file currently in the content editor

        // --- FIELDS FOR COPY/PASTE/CUT ---
        private string _clipboardSourcePath = null;
        private FileSystemEntryType _clipboardSourceType;
        private bool _isCutOperation = false; // True if it's a "cut" (move) operation


        public Form1()
        {
            InitializeComponent();

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
                rootPath = AppDomain.CurrentDomain.BaseDirectory;
                rootAttributes = FileAttributes.Directory; // Assume it's a directory
                MessageBox.Show(this, $"Error accessing user profile path. Defaulting to application directory: {rootPath}\nError: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            txtPath.Text = rootPath;
            lblInstructions.Visible = false;

            // Initialize root node in TreeView
            var rootEntry = new FileSystemEntry(Path.GetFileName(rootPath), rootPath, FileSystemEntryType.Directory, rootAttributes);
            var rootNode = new TreeNode(rootEntry.Name) { Tag = rootEntry };
            rootNode.Nodes.Add("..."); // Placeholder for lazy loading
            tvFiles.Nodes.Add(rootNode);

            // Wire up all event handlers
            tvFiles.BeforeExpand += TvFiles_BeforeExpand;
            tvFiles.AfterSelect += TvFiles_AfterSelect;
            btnCreate.Click += BtnCreate_Click;
            btnSave.Click += BtnSave_Click;
            btnRefresh.Click += BtnRefresh_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRename.Click += BtnRename_Click;
            btnCopy.Click += BtnCopy_Click;
            btnCut.Click += BtnCut_Click; // New Cut button handler
            btnPaste.Click += BtnPaste_Click;
            btnOpen.Click += BtnOpen_Click;

            // Initial state for buttons
            btnSave.Visible = false; // Hide save until a text file is open for editing
            btnPaste.Enabled = false; // Disable paste until something is copied/cut
        }


        // Event handler for TreeView node selection
        private void TvFiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selectedEntry = e.Node.Tag as FileSystemEntry;
            if (selectedEntry == null)
            {
                ClearContentArea();
                return;
            }

            txtPath.Text = selectedEntry.FullPath;

            if (selectedEntry.Type == FileSystemEntryType.File)
                ShowFileContent(selectedEntry.FullPath);
            else
                ClearContentArea();
        }

        // Handler for the Refresh button
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
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

            bool isNewFile = !_fileSystemManager.PathExists(_currentEditedFilePath);

            try
            {
                string parentDir = Path.GetDirectoryName(_currentEditedFilePath);
                if (!string.IsNullOrEmpty(parentDir) && !_fileSystemManager.IsDirectory(parentDir))
                {
                    _fileSystemManager.CreateDirectory(parentDir); // Ensure parent exists
                }

                _fileSystemManager.WriteTextFile(_currentEditedFilePath, txtFileContent.Text);

                lblStatus.Text = $"Saved: {Path.GetFileName(_currentEditedFilePath)}";
                MessageBox.Show(this, $"File saved: {Path.GetFileName(_currentEditedFilePath)}", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (isNewFile)
                {
                    string parentDirPath = Path.GetDirectoryName(_currentEditedFilePath);
                    TreeNode parentNode = FindNodeByPath(parentDirPath);
                    if (parentNode != null)
                    {
                        RefreshNode(parentNode);
                        foreach (TreeNode node in parentNode.Nodes)
                        {
                            var entry = node.Tag as FileSystemEntry;
                            if (entry != null && entry.FullPath.Equals(_currentEditedFilePath, StringComparison.OrdinalIgnoreCase))
                            {
                                tvFiles.SelectedNode = node;
                                node.EnsureVisible();
                                break;
                            }
                        }
                    }
                    else
                    {
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


        // Handler for the Create button (prompts for file or directory)
        private void BtnCreate_Click(object sender, EventArgs e)
        {
            string targetDir = GetSelectedDirectoryPath();
            if (string.IsNullOrEmpty(targetDir))
            {
                MessageBox.Show(this, "Please select a directory to create a new item in.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult typeResult = MessageBox.Show(this, "Do you want to create a new File or a new Directory?", "Create New Item",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (typeResult == DialogResult.Cancel)
            {
                lblStatus.Text = "Creation cancelled.";
                return;
            }

            bool isFile = (typeResult == DialogResult.Yes);

            string itemName = PromptForInput($"Create New {(isFile ? "File" : "Directory")}", $"Enter {(isFile ? "file" : "directory")} name:");
            if (string.IsNullOrWhiteSpace(itemName))
            {
                lblStatus.Text = $"{(isFile ? "File" : "Directory")} creation cancelled or invalid name.";
                return;
            }

            string newPath = Path.Combine(targetDir, itemName);

            if (_fileSystemManager.PathExists(newPath))
            {
                MessageBox.Show(this, $"A {(isFile ? "file" : "directory")} with that name already exists. Please choose a different name.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (isFile)
                {
                    _currentEditedFilePath = newPath;
                    txtFileContent.Text = "";
                    txtFileContent.ReadOnly = false;
                    txtFileContent.Visible = true;
                    pbPreview.Visible = false;
                    btnSave.Visible = true;
                    lblStatus.Text = $"New file '{itemName}' ready for content. Press Save to create on disk.";
                }
                else // Create Directory
                {
                    _fileSystemManager.CreateDirectory(newPath);
                    RefreshNode(FindNodeByPath(targetDir)); // Refresh parent to show new directory
                    lblStatus.Text = $"Created directory: {itemName}";
                    MessageBox.Show(this, $"Directory created: {itemName}", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(this, $"Error creating {(isFile ? "file" : "directory")}: {ex.Message}", "Error",
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


        // Handler for deleting files or directories
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var selectedNode = tvFiles.SelectedNode;
            if (selectedNode == null)
            {
                MessageBox.Show(this, "Please select a file or directory to delete.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedEntry = selectedNode.Tag as FileSystemEntry;
            if (selectedEntry == null) return;

            DialogResult confirmResult;
            if (selectedEntry.Type == FileSystemEntryType.Directory && Directory.EnumerateFileSystemEntries(selectedEntry.FullPath).Any())
            {
                confirmResult = MessageBox.Show(this,
                    $"The directory '{selectedEntry.Name}' is not empty. Deleting it will permanently remove all its contents. Are you sure?",
                    "Confirm Recursive Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
            }
            else
            {
                confirmResult = MessageBox.Show(this,
                    $"Are you sure you want to delete '{selectedEntry.Name}'?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
            }


            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    if (selectedEntry.Type == FileSystemEntryType.File)
                    {
                        _fileSystemManager.DeleteFileSystemEntry(selectedEntry.FullPath, FileSystemEntryType.File);
                    }
                    else // Directory
                    {
                        if (Directory.EnumerateFileSystemEntries(selectedEntry.FullPath).Any())
                        {
                            _fileSystemManager.DeleteDirectoryRecursive(selectedEntry.FullPath);
                        }
                        else
                        {
                            _fileSystemManager.DeleteFileSystemEntry(selectedEntry.FullPath, FileSystemEntryType.Directory);
                        }
                    }

                    // Get parent path before deleting the node itself
                    string parentDirPath = Path.GetDirectoryName(selectedEntry.FullPath);
                    TreeNode parentNode = FindNodeByPath(parentDirPath);

                    if (parentNode != null)
                    {
                        RefreshNode(parentNode); // Refresh the parent to remove the deleted item from view
                    }
                    else
                    {
                        // If parent node couldn't be found (e.g., root deleted), refresh root
                        RefreshNode(tvFiles.Nodes[0]);
                    }

                    ClearContentArea(); // Clear preview after deletion
                    lblStatus.Text = $"Deleted: {selectedEntry.Name}";
                    MessageBox.Show(this, $"Deleted: {selectedEntry.Name}", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (IOException ex)
                {
                    MessageBox.Show(this, $"Error deleting {selectedEntry.Name}: {ex.Message}", "Error",
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
        }

        // Handler for renaming files or directories
        private void BtnRename_Click(object sender, EventArgs e)
        {
            var selectedNode = tvFiles.SelectedNode;
            if (selectedNode == null)
            {
                MessageBox.Show(this, "Please select a file or directory to rename.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedEntry = selectedNode.Tag as FileSystemEntry;
            if (selectedEntry == null) return;

            string oldName = selectedEntry.Name;
            string newName = PromptForInput($"Rename {oldName}", $"Enter new name for '{oldName}':", oldName); // Pre-fill with old name

            if (string.IsNullOrWhiteSpace(newName) || newName.Equals(oldName, StringComparison.OrdinalIgnoreCase))
            {
                lblStatus.Text = "Rename cancelled or new name is invalid/same as old name.";
                return;
            }

            string parentDirPath = Path.GetDirectoryName(selectedEntry.FullPath);
            string newFullPath = Path.Combine(parentDirPath, newName);

            try
            {
                _fileSystemManager.RenameFileSystemEntry(selectedEntry.FullPath, newFullPath, selectedEntry.Type);

                // Update the TreeView node directly and then refresh its parent
                selectedEntry.Name = newName; // Update the in-memory object
                selectedEntry.FullPath = newFullPath; // Update full path
                selectedNode.Text = newName; // Update the displayed text in the TreeView

                RefreshNode(FindNodeByPath(parentDirPath)); // Refresh parent to reflect potential sorting changes

                // After rename, ensure the item is still selected and preview is updated if it's a file
                tvFiles.SelectedNode = selectedNode;
                if (selectedEntry.Type == FileSystemEntryType.File)
                {
                    ShowFileContent(newFullPath);
                }
                else
                {
                    ClearContentArea();
                }

                lblStatus.Text = $"Renamed '{oldName}' to '{newName}'";
                MessageBox.Show(this, $"Renamed: '{oldName}' to '{newName}'", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (IOException ex)
            {
                MessageBox.Show(this, $"Error renaming {oldName}: {ex.Message}", "Error",
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

        // Handler for Copy button (sets the item to be copied)
        private void BtnCopy_Click(object sender, EventArgs e)
        {
            var selectedNode = tvFiles.SelectedNode;
            if (selectedNode == null)
            {
                MessageBox.Show(this, "Please select a file or directory to copy.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedEntry = selectedNode.Tag as FileSystemEntry;
            if (selectedEntry == null) return;

            _clipboardSourcePath = selectedEntry.FullPath;
            _clipboardSourceType = selectedEntry.Type;
            _isCutOperation = false; // This is a COPY operation

            btnPaste.Enabled = true; // Enable paste button
            lblStatus.Text = $"Copied: {selectedEntry.Name}";
        }

        // Handler for Cut button (sets the item to be moved)
        private void BtnCut_Click(object sender, EventArgs e)
        {
            var selectedNode = tvFiles.SelectedNode;
            if (selectedNode == null)
            {
                MessageBox.Show(this, "Please select a file or directory to cut (move).", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedEntry = selectedNode.Tag as FileSystemEntry;
            if (selectedEntry == null) return;

            _clipboardSourcePath = selectedEntry.FullPath;
            _clipboardSourceType = selectedEntry.Type;
            _isCutOperation = true; // This is a CUT/MOVE operation

            btnPaste.Enabled = true; // Enable paste button
            lblStatus.Text = $"Cut: {selectedEntry.Name}";
        }

        // Handler for Paste button (performs copy or move operation)
        private void BtnPaste_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_clipboardSourcePath))
            {
                MessageBox.Show(this, "Nothing has been copied or cut to paste.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string destinationDir = GetSelectedDirectoryPath(); // Target directory to paste into
            if (string.IsNullOrEmpty(destinationDir))
            {
                MessageBox.Show(this, "Please select a destination directory to paste into.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string sourceItemName = Path.GetFileName(_clipboardSourcePath);
            string destinationPath = Path.Combine(destinationDir, sourceItemName);

            // Cannot paste into itself or its child (for directories)
            if (_clipboardSourceType == FileSystemEntryType.Directory && destinationPath.StartsWith(_clipboardSourcePath + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this, "Cannot paste a directory into itself or its subdirectory.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Prevent pasting an item back to its exact same location for a "move" (cut) operation
            if (_isCutOperation && _clipboardSourcePath.Equals(destinationPath, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this, "Cannot move an item to its original location.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Handle conflicts before operation
            if (_fileSystemManager.PathExists(destinationPath))
            {
                string conflictMsg = $"{sourceItemName} already exists in the destination. Overwrite?";
                DialogResult overwriteResult = MessageBox.Show(this, conflictMsg, "Item Conflict",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (overwriteResult == DialogResult.Cancel)
                {
                    lblStatus.Text = "Paste operation cancelled due to conflict.";
                    return;
                }
                if (overwriteResult == DialogResult.No)
                {
                    lblStatus.Text = "Paste operation cancelled. Not overwriting existing item.";
                    return;
                }
                // If overwriteResult is Yes, we proceed to delete the existing item first.
                try
                {
                    if (_fileSystemManager.IsFile(destinationPath))
                    {
                        _fileSystemManager.DeleteFileSystemEntry(destinationPath, FileSystemEntryType.File);
                    }
                    else if (_fileSystemManager.IsDirectory(destinationPath))
                    {
                        _fileSystemManager.DeleteDirectoryRecursive(destinationPath);
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show(this, $"Error preparing for overwrite: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblStatus.Text = $"Error: {ex.Message}";
                    return;
                }
            }

            try
            {
                string originalSourceParentPath = Path.GetDirectoryName(_clipboardSourcePath);

                if (_isCutOperation) // Perform Move
                {
                    _fileSystemManager.MoveFileSystemEntry(_clipboardSourcePath, destinationPath);
                    lblStatus.Text = $"Moved: {sourceItemName} to {destinationDir}";
                }
                else // Perform Copy
                {
                    if (_clipboardSourceType == FileSystemEntryType.File)
                    {
                        _fileSystemManager.CopyFile(_clipboardSourcePath, destinationPath, true);
                    }
                    else // Directory Copy
                    {
                        _fileSystemManager.CopyDirectoryRecursive(_clipboardSourcePath, destinationPath);
                    }
                    lblStatus.Text = $"Copied: {sourceItemName} to {destinationDir}";
                }

                // Refresh affected directories
                RefreshNode(FindNodeByPath(originalSourceParentPath)); // Always refresh source parent (for move, item disappears)
                RefreshNode(FindNodeByPath(destinationDir));          // Always refresh destination

                ClearClipboardState(); // Reset clipboard state after paste
                MessageBox.Show(this, $"Item pasted: {sourceItemName}", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (IOException ex)
            {
                MessageBox.Show(this, $"Error pasting {sourceItemName}: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = $"Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"An unexpected error occurred during paste: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = $"Error: {ex.Message}";
            }
        }

        // Handler for Open Folder button
        private void BtnOpen_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select a folder to browse:";
                fbd.ShowNewFolderButton = false;

                if (fbd.ShowDialog(this) == DialogResult.OK)
                {
                    rootPath = fbd.SelectedPath;
                    txtPath.Text = rootPath;

                    tvFiles.Nodes.Clear();
                    FileAttributes currentRootAttributes = FileAttributes.Directory;
                    try { currentRootAttributes = File.GetAttributes(rootPath); }
                    catch { /* default to directory */ }

                    var rootEntry = new FileSystemEntry(Path.GetFileName(rootPath), rootPath, FileSystemEntryType.Directory, currentRootAttributes);
                    var rootNode = new TreeNode(rootEntry.Name) { Tag = rootEntry };
                    rootNode.Nodes.Add("...");
                    tvFiles.Nodes.Add(rootNode);
                    rootNode.Expand();
                    lblStatus.Text = $"Opened: {rootPath}";
                }
                else
                {
                    lblStatus.Text = "Open folder cancelled.";
                }
            }
        }


        // Handles lazy loading of TreeView nodes
        private void TvFiles_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode node = e.Node;
            var nodeEntry = node.Tag as FileSystemEntry;
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

        // Displays a custom input dialog for user input (overloaded to allow default value)
        private string PromptForInput(string title, string prompt, string defaultValue = "")
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
                var txt = new TextBox { Left = 10, Top = 45, Width = 350, Text = defaultValue };

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


        // Helper to clear clipboard state
        private void ClearClipboardState()
        {
            _clipboardSourcePath = null;
            _clipboardSourceType = FileSystemEntryType.File; // Reset to default
            _isCutOperation = false;
            btnPaste.Enabled = false; // Disable Paste button
            lblStatus.Text = "Clipboard cleared.";
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

            if (currentNode.IsExpanded || (currentNode.Nodes.Count == 1 && currentNode.Nodes[0].Text == "..."))
            {
                foreach (TreeNode childNode in currentNode.Nodes)
                {
                    var childEntry = childNode.Tag as FileSystemEntry;
                    if (childEntry != null && childEntry.Type == FileSystemEntryType.Directory &&
                        childNode.Nodes.Count == 1 && childNode.Nodes[0].Text == "...")
                    {
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