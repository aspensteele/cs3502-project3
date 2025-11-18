namespace FileManagementSystem
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // Controls
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
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.SplitContainer rightSplitContainer;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel treePanel;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Label lblPathLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
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
            treePanel = new Panel();
            tvFiles = new TreeView();
            rightPanel = new Panel();
            rightSplitContainer = new SplitContainer();
            pbPreview = new PictureBox();
            txtFileContent = new TextBox();
            lblStatus = new Label();
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
            topPanel.Size = new Size(1000, 90);
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
            lblPathLabel.TabIndex = 2;
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
            txtPath.Size = new Size(830, 33);
            txtPath.TabIndex = 0;
            // 
            // buttonPanel
            // 
            buttonPanel.Controls.Add(btnCreate);
            buttonPanel.Controls.Add(btnOpen);
            buttonPanel.Controls.Add(btnSave);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnRename);
            buttonPanel.Controls.Add(btnRefresh);
            buttonPanel.Location = new Point(5, 49);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new Size(1005, 40);
            buttonPanel.TabIndex = 1;
            // 
            // btnCreate
            // 
            btnCreate.BackColor = Color.FromArgb(0, 122, 204);
            btnCreate.Cursor = Cursors.Hand;
            btnCreate.FlatAppearance.BorderSize = 0;
            btnCreate.FlatAppearance.MouseOverBackColor = Color.FromArgb(28, 151, 234);
            btnCreate.FlatStyle = FlatStyle.Flat;
            btnCreate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCreate.ForeColor = Color.White;
            btnCreate.Location = new Point(136, -2);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(134, 42);
            btnCreate.TabIndex = 0;
            btnCreate.Text = "📄 Create";
            btnCreate.UseVisualStyleBackColor = false;
            // 
            // btnOpen
            // 
            btnOpen.BackColor = Color.FromArgb(0, 122, 204);
            btnOpen.Cursor = Cursors.Hand;
            btnOpen.FlatAppearance.BorderSize = 0;
            btnOpen.FlatAppearance.MouseOverBackColor = Color.FromArgb(28, 151, 234);
            btnOpen.FlatStyle = FlatStyle.Flat;
            btnOpen.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnOpen.ForeColor = Color.White;
            btnOpen.Location = new Point(276, -1);
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new Size(134, 40);
            btnOpen.TabIndex = 1;
            btnOpen.Text = "📂 Open";
            btnOpen.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(16, 124, 16);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(27, 145, 27);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(416, -2);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(132, 41);
            btnSave.TabIndex = 2;
            btnSave.Text = "💾 Save";
            btnSave.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(204, 40, 40);
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 60, 60);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(554, 0);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(136, 41);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "🗑️ Delete";
            btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnRename
            // 
            btnRename.BackColor = Color.FromArgb(104, 104, 104);
            btnRename.Cursor = Cursors.Hand;
            btnRename.FlatAppearance.BorderSize = 0;
            btnRename.FlatAppearance.MouseOverBackColor = Color.FromArgb(134, 134, 134);
            btnRename.FlatStyle = FlatStyle.Flat;
            btnRename.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRename.ForeColor = Color.White;
            btnRename.Location = new Point(696, -1);
            btnRename.Name = "btnRename";
            btnRename.Size = new Size(125, 42);
            btnRename.TabIndex = 4;
            btnRename.Text = "✏️ Rename";
            btnRename.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(104, 104, 104);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatAppearance.MouseOverBackColor = Color.FromArgb(134, 134, 134);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(827, 1);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(139, 39);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "🔄 Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // treePanel
            // 
            treePanel.BackColor = Color.FromArgb(37, 37, 38);
            treePanel.Controls.Add(tvFiles);
            treePanel.Dock = DockStyle.Left;
            treePanel.Location = new Point(0, 90);
            treePanel.Name = "treePanel";
            treePanel.Padding = new Padding(5);
            treePanel.Size = new Size(270, 490);
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
            tvFiles.Size = new Size(260, 480);
            tvFiles.TabIndex = 0;
            // 
            // rightPanel
            // 
            rightPanel.BackColor = Color.FromArgb(30, 30, 30);
            rightPanel.Controls.Add(rightSplitContainer);
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.Location = new Point(270, 90);
            rightPanel.Name = "rightPanel";
            rightPanel.Padding = new Padding(5);
            rightPanel.Size = new Size(730, 490);
            rightPanel.TabIndex = 0;
            // 
            // rightSplitContainer
            // 
            rightSplitContainer.BackColor = Color.FromArgb(45, 45, 48);
            rightSplitContainer.Dock = DockStyle.Fill;
            rightSplitContainer.Location = new Point(5, 5);
            rightSplitContainer.Name = "rightSplitContainer";
            rightSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // rightSplitContainer.Panel1
            // 
            rightSplitContainer.Panel1.BackColor = Color.FromArgb(37, 37, 38);
            rightSplitContainer.Panel1.Controls.Add(pbPreview);
            rightSplitContainer.Panel1.Padding = new Padding(5);
            // 
            // rightSplitContainer.Panel2
            // 
            rightSplitContainer.Panel2.BackColor = Color.FromArgb(37, 37, 38);
            rightSplitContainer.Panel2.Controls.Add(txtFileContent);
            rightSplitContainer.Panel2.Padding = new Padding(5);
            rightSplitContainer.Size = new Size(720, 480);
            rightSplitContainer.SplitterDistance = 238;
            rightSplitContainer.SplitterWidth = 6;
            rightSplitContainer.TabIndex = 0;
            // 
            // pbPreview
            // 
            pbPreview.BackColor = Color.FromArgb(30, 30, 30);
            pbPreview.Dock = DockStyle.Fill;
            pbPreview.Location = new Point(5, 5);
            pbPreview.Name = "pbPreview";
            pbPreview.Size = new Size(710, 228);
            pbPreview.SizeMode = PictureBoxSizeMode.Zoom;
            pbPreview.TabIndex = 0;
            pbPreview.TabStop = false;
            // 
            // txtFileContent
            // 
            txtFileContent.BackColor = Color.FromArgb(30, 30, 30);
            txtFileContent.BorderStyle = BorderStyle.None;
            txtFileContent.Dock = DockStyle.Fill;
            txtFileContent.Font = new Font("Consolas", 10F);
            txtFileContent.ForeColor = Color.FromArgb(220, 220, 220);
            txtFileContent.Location = new Point(5, 5);
            txtFileContent.Multiline = true;
            txtFileContent.Name = "txtFileContent";
            txtFileContent.ScrollBars = ScrollBars.Both;
            txtFileContent.Size = new Size(710, 226);
            txtFileContent.TabIndex = 0;
            txtFileContent.WordWrap = false;
            // 
            // lblStatus
            // 
            lblStatus.BackColor = Color.FromArgb(0, 122, 204);
            lblStatus.Dock = DockStyle.Bottom;
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(0, 580);
            lblStatus.Name = "lblStatus";
            lblStatus.Padding = new Padding(10, 0, 0, 0);
            lblStatus.Size = new Size(1000, 20);
            lblStatus.TabIndex = 3;
            lblStatus.Text = "Ready";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Form1
            // 
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1000, 600);
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
    }
}