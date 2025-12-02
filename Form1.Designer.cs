// Form1.Designer.cs

namespace FileManagementSystem
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            topPanel = new Panel();
            lblPathLabel = new Label();
            txtPath = new TextBox();
            buttonPanel = new Panel();
            btnCreate = new Button();
            btnOpen = new Button();
            btnSave = new Button();
            btnDelete = new Button();
            btnRename = new Button();
            btnRefresh = new Button();
            btnCopy = new Button();
            btnCut = new Button();
            btnPaste = new Button();
            treePanel = new Panel();
            tvFiles = new TreeView();
            rightPanel = new Panel();
            rightSplitContainer = new SplitContainer();
            pbPreview = new PictureBox();
            txtFileContent = new TextBox();
            lblInstructions = new Label();
            lblStatus = new Label();
            toolTip1 = new ToolTip(components);
            topPanel.SuspendLayout();
            buttonPanel.SuspendLayout();
            treePanel.SuspendLayout();
            rightPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)rightSplitContainer).BeginInit();
            rightSplitContainer.Panel1.SuspendLayout();
            rightSplitContainer.Panel2.SuspendLayout();
            rightSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbPreview).BeginInit();
            SuspendLayout();
            // 
            // topPanel
            // 
            topPanel.BackColor = Color.FromArgb(45, 45, 48);
            topPanel.Controls.Add(lblPathLabel);
            topPanel.Controls.Add(txtPath);
            topPanel.Controls.Add(buttonPanel);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Padding = new Padding(10, 8, 10, 8);
            topPanel.Size = new Size(1126, 90);
            topPanel.TabIndex = 2;
            // 
            // lblPathLabel
            // 
            lblPathLabel.AutoSize = true;
            lblPathLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPathLabel.ForeColor = Color.White;
            lblPathLabel.Location = new Point(10, 11);
            lblPathLabel.Name = "lblPathLabel";
            lblPathLabel.Size = new Size(125, 25);
            lblPathLabel.TabIndex = 0;
            lblPathLabel.Text = "Current Path:";
            // 
            // txtPath
            // 
            txtPath.BackColor = Color.FromArgb(37, 37, 38);
            txtPath.BorderStyle = BorderStyle.FixedSingle;
            txtPath.Font = new Font("Segoe UI", 9.5F);
            txtPath.ForeColor = Color.White;
            txtPath.Location = new Point(141, 11);
            txtPath.Name = "txtPath";
            txtPath.ReadOnly = true;
            txtPath.Size = new Size(940, 33);
            txtPath.TabIndex = 1;
            toolTip1.SetToolTip(txtPath, "Current directory path.");
            // 
            // buttonPanel
            // 
            buttonPanel.Controls.Add(btnCreate);
            buttonPanel.Controls.Add(btnOpen);
            buttonPanel.Controls.Add(btnSave);
            buttonPanel.Controls.Add(btnPaste);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnRename);
            buttonPanel.Controls.Add(btnRefresh);
            buttonPanel.Controls.Add(btnCopy);
            buttonPanel.Controls.Add(btnCut);
            buttonPanel.Location = new Point(5, 49);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new Size(1088, 40);
            buttonPanel.TabIndex = 2;
            // 
            // btnCreate
            // 
            btnCreate.BackColor = Color.FromArgb(0, 122, 204);
            btnCreate.FlatAppearance.BorderSize = 0;
            btnCreate.FlatStyle = FlatStyle.Flat;
            btnCreate.Font = new Font("Segoe UI", 9F);
            btnCreate.ForeColor = Color.White;
            btnCreate.Location = new Point(10, -2);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(110, 42);
            btnCreate.TabIndex = 0;
            btnCreate.Text = "📄 Create";
            toolTip1.SetToolTip(btnCreate, "Create New File or Directory");
            btnCreate.UseVisualStyleBackColor = false;
            // 
            // btnOpen
            // 
            btnOpen.BackColor = Color.FromArgb(0, 122, 204);
            btnOpen.FlatAppearance.BorderSize = 0;
            btnOpen.FlatStyle = FlatStyle.Flat;
            btnOpen.Font = new Font("Segoe UI", 9F);
            btnOpen.ForeColor = Color.White;
            btnOpen.Location = new Point(125, -2);
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new Size(110, 42);
            btnOpen.TabIndex = 1;
            btnOpen.Text = "📂 Open";
            toolTip1.SetToolTip(btnOpen, "Open a different root folder");
            btnOpen.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(16, 124, 16);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9F);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(240, -2);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(110, 42);
            btnSave.TabIndex = 2;
            btnSave.Text = "💾 Save";
            toolTip1.SetToolTip(btnSave, "Save changes to the current file");
            btnSave.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(204, 40, 40);
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 9F);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(355, -2);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(110, 42);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "🗑️ Delete";
            toolTip1.SetToolTip(btnDelete, "Delete selected file or directory");
            btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnRename
            // 
            btnRename.BackColor = Color.Gray;
            btnRename.FlatAppearance.BorderSize = 0;
            btnRename.FlatStyle = FlatStyle.Flat;
            btnRename.Font = new Font("Segoe UI", 9F);
            btnRename.ForeColor = Color.White;
            btnRename.Location = new Point(471, 0);
            btnRename.Name = "btnRename";
            btnRename.Size = new Size(141, 42);
            btnRename.TabIndex = 4;
            btnRename.Text = "✏️ Rename";
            toolTip1.SetToolTip(btnRename, "Rename selected file or directory");
            btnRename.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.Gray;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 9F);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(618, -2);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(110, 42);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "🔄 Refresh";
            toolTip1.SetToolTip(btnRefresh, "Refresh current directory contents");
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // btnCopy
            // 
            btnCopy.BackColor = Color.Gray;
            btnCopy.FlatAppearance.BorderSize = 0;
            btnCopy.FlatStyle = FlatStyle.Flat;
            btnCopy.Font = new Font("Segoe UI", 9F);
            btnCopy.ForeColor = Color.White;
            btnCopy.Location = new Point(734, -2);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(110, 42);
            btnCopy.TabIndex = 6;
            btnCopy.Text = "📋 Copy";
            toolTip1.SetToolTip(btnCopy, "Copy selected file or directory");
            btnCopy.UseVisualStyleBackColor = false;
            // 
            // btnCut
            // 
            btnCut.BackColor = Color.Gray;
            btnCut.FlatAppearance.BorderSize = 0;
            btnCut.FlatStyle = FlatStyle.Flat;
            btnCut.Font = new Font("Segoe UI", 9F);
            btnCut.ForeColor = Color.White;
            btnCut.Location = new Point(850, -2);
            btnCut.Name = "btnCut";
            btnCut.Size = new Size(110, 42);
            btnCut.TabIndex = 7;
            btnCut.Text = "✂️ Cut";
            toolTip1.SetToolTip(btnCut, "Cut (move) selected file or directory");
            btnCut.UseVisualStyleBackColor = false;
            // 
            // btnPaste
            // 
            btnPaste.BackColor = Color.Gray;
            btnPaste.FlatAppearance.BorderSize = 0;
            btnPaste.FlatStyle = FlatStyle.Flat;
            btnPaste.Font = new Font("Segoe UI", 9F);
            btnPaste.ForeColor = Color.White;
            btnPaste.Location = new Point(966, 1);
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new Size(110, 42);
            btnPaste.TabIndex = 8;
            btnPaste.Text = "➕ Paste";
            toolTip1.SetToolTip(btnPaste, "Paste copied/cut item to current directory");
            btnPaste.UseVisualStyleBackColor = false;
            // 
            // treePanel
            // 
            treePanel.BackColor = Color.FromArgb(37, 37, 38);
            treePanel.Controls.Add(tvFiles);
            treePanel.Dock = DockStyle.Left;
            treePanel.Location = new Point(0, 90);
            treePanel.Name = "treePanel";
            treePanel.Padding = new Padding(5);
            treePanel.Size = new Size(270, 472);
            treePanel.TabIndex = 1;
            // 
            // tvFiles
            // 
            tvFiles.BackColor = Color.FromArgb(30, 30, 30);
            tvFiles.BorderStyle = BorderStyle.None;
            tvFiles.Dock = DockStyle.Fill;
            tvFiles.Font = new Font("Segoe UI", 9.5F);
            tvFiles.ForeColor = Color.White;
            tvFiles.LineColor = Color.FromArgb(100, 100, 100);
            tvFiles.Location = new Point(5, 5);
            tvFiles.Name = "tvFiles";
            tvFiles.Size = new Size(260, 462);
            tvFiles.TabIndex = 0;
            toolTip1.SetToolTip(tvFiles, "File system navigation tree");
            // 
            // rightPanel
            // 
            rightPanel.BackColor = Color.FromArgb(30, 30, 30);
            rightPanel.Controls.Add(rightSplitContainer);
            rightPanel.Controls.Add(lblInstructions);
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.Location = new Point(270, 90);
            rightPanel.Name = "rightPanel";
            rightPanel.Padding = new Padding(5);
            rightPanel.Size = new Size(856, 472);
            rightPanel.TabIndex = 0;
            // 
            // rightSplitContainer
            // 
            rightSplitContainer.Dock = DockStyle.Fill;
            rightSplitContainer.Location = new Point(5, 45);
            rightSplitContainer.Name = "rightSplitContainer";
            rightSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // rightSplitContainer.Panel1
            // 
            rightSplitContainer.Panel1.Controls.Add(pbPreview);
            // 
            // rightSplitContainer.Panel2
            // 
            rightSplitContainer.Panel2.Controls.Add(txtFileContent);
            rightSplitContainer.Size = new Size(846, 422);
            rightSplitContainer.SplitterDistance = 210;
            rightSplitContainer.TabIndex = 0;
            // 
            // pbPreview
            // 
            pbPreview.Dock = DockStyle.Fill;
            pbPreview.Location = new Point(0, 0);
            pbPreview.Name = "pbPreview";
            pbPreview.Size = new Size(846, 210);
            pbPreview.SizeMode = PictureBoxSizeMode.Zoom;
            pbPreview.TabIndex = 0;
            pbPreview.TabStop = false;
            toolTip1.SetToolTip(pbPreview, "Image preview area");
            // 
            // txtFileContent
            // 
            txtFileContent.BackColor = Color.FromArgb(30, 30, 30);
            txtFileContent.BorderStyle = BorderStyle.None;
            txtFileContent.Dock = DockStyle.Fill;
            txtFileContent.Font = new Font("Consolas", 10F);
            txtFileContent.ForeColor = Color.White;
            txtFileContent.Location = new Point(0, 0);
            txtFileContent.Multiline = true;
            txtFileContent.Name = "txtFileContent";
            txtFileContent.ScrollBars = ScrollBars.Both;
            txtFileContent.Size = new Size(846, 208);
            txtFileContent.TabIndex = 0;
            toolTip1.SetToolTip(txtFileContent, "File content editor");
            // 
            // lblInstructions
            // 
            lblInstructions.Dock = DockStyle.Top;
            lblInstructions.Font = new Font("Segoe UI", 11F, FontStyle.Italic);
            lblInstructions.ForeColor = Color.White;
            lblInstructions.Location = new Point(5, 5);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(846, 40);
            lblInstructions.TabIndex = 1;
            lblInstructions.Text = "📂 Select a file or folder from the tree view to get started.";
            lblInstructions.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblStatus
            // 
            lblStatus.BackColor = Color.FromArgb(0, 122, 204);
            lblStatus.Dock = DockStyle.Bottom;
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(0, 562);
            lblStatus.Name = "lblStatus";
            lblStatus.Padding = new Padding(10, 0, 0, 0);
            lblStatus.Size = new Size(1126, 23);
            lblStatus.TabIndex = 3;
            lblStatus.Text = "Ready";
            toolTip1.SetToolTip(lblStatus, "Application status messages");
            // 
            // Form1
            // 
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1126, 585);
            Controls.Add(rightPanel);
            Controls.Add(treePanel);
            Controls.Add(topPanel);
            Controls.Add(lblStatus);
            Font = new Font("Segoe UI", 9F);
            Name = "Form1";
            Text = "File Management System";
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            buttonPanel.ResumeLayout(false);
            treePanel.ResumeLayout(false);
            rightPanel.ResumeLayout(false);
            rightSplitContainer.Panel1.ResumeLayout(false);
            rightSplitContainer.Panel2.ResumeLayout(false);
            rightSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)rightSplitContainer).EndInit();
            rightSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbPreview).EndInit();
            ResumeLayout(false);
        }

        #endregion

        // Declare your controls here
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.TreeView tvFiles;
        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.TextBox txtFileContent;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnCut;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.SplitContainer rightSplitContainer;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel treePanel;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Label lblPathLabel;
        private System.Windows.Forms.ToolTip toolTip1; 
    }
}