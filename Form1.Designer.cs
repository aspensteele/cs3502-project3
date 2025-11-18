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
        private System.Windows.Forms.Label lblInstructions;
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
            lblInstructions = new Label();

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
            topPanel.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            topPanel.Controls.Add(lblPathLabel);
            topPanel.Controls.Add(txtPath);
            topPanel.Controls.Add(buttonPanel);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new System.Drawing.Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Padding = new Padding(10, 8, 10, 8);
            topPanel.Size = new System.Drawing.Size(1000, 90);
            topPanel.TabIndex = 2;

            // 
            // lblPathLabel
            // 
            lblPathLabel.AutoSize = true;
            lblPathLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblPathLabel.ForeColor = System.Drawing.Color.White;
            lblPathLabel.Location = new System.Drawing.Point(10, 11);
            lblPathLabel.Name = "lblPathLabel";
            lblPathLabel.Size = new System.Drawing.Size(125, 25);
            lblPathLabel.TabIndex = 2;
            lblPathLabel.Text = "Current Path:";

            // 
            // txtPath
            // 
            txtPath.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            txtPath.BorderStyle = BorderStyle.FixedSingle;
            txtPath.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            txtPath.ForeColor = System.Drawing.Color.White;
            txtPath.Location = new System.Drawing.Point(141, 11);
            txtPath.Name = "txtPath";
            txtPath.ReadOnly = true;
            txtPath.Size = new System.Drawing.Size(830, 33);
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
            buttonPanel.Location = new System.Drawing.Point(5, 49);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new System.Drawing.Size(1005, 40);
            buttonPanel.TabIndex = 1;

            // Button styles (simplified with icons)
            btnCreate.Text = "📄 Create";
            btnOpen.Text = "📂 Open";
            btnSave.Text = "💾 Save";
            btnDelete.Text = "🗑️ Delete";
            btnRename.Text = "✏️ Rename";
            btnRefresh.Text = "🔄 Refresh";

            Color btnBlue = System.Drawing.Color.FromArgb(0, 122, 204);
            Color btnGreen = System.Drawing.Color.FromArgb(16, 124, 16);
            Color btnRed = System.Drawing.Color.FromArgb(204, 40, 40);
            Color btnGray = System.Drawing.Color.FromArgb(104, 104, 104);

            // btnCreate
            btnCreate.BackColor = btnBlue;
            btnCreate.FlatStyle = FlatStyle.Flat;
            btnCreate.FlatAppearance.BorderSize = 0;
            btnCreate.ForeColor = System.Drawing.Color.White;
            btnCreate.Location = new System.Drawing.Point(10, -2);
            btnCreate.Size = new System.Drawing.Size(120, 42);

            // btnOpen
            btnOpen.BackColor = btnBlue;
            btnOpen.FlatStyle = FlatStyle.Flat;
            btnOpen.FlatAppearance.BorderSize = 0;
            btnOpen.ForeColor = System.Drawing.Color.White;
            btnOpen.Location = new System.Drawing.Point(135, -2);
            btnOpen.Size = new System.Drawing.Size(120, 42);

            // btnSave
            btnSave.BackColor = btnGreen;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.ForeColor = System.Drawing.Color.White;
            btnSave.Location = new System.Drawing.Point(260, -2);
            btnSave.Size = new System.Drawing.Size(120, 42);

            // btnDelete
            btnDelete.BackColor = btnRed;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.ForeColor = System.Drawing.Color.White;
            btnDelete.Location = new System.Drawing.Point(385, -2);
            btnDelete.Size = new System.Drawing.Size(120, 42);

            // btnRename
            btnRename.BackColor = btnGray;
            btnRename.FlatStyle = FlatStyle.Flat;
            btnRename.FlatAppearance.BorderSize = 0;
            btnRename.ForeColor = System.Drawing.Color.White;
            btnRename.Location = new System.Drawing.Point(510, -2);
            btnRename.Size = new System.Drawing.Size(120, 42);

            // btnRefresh
            btnRefresh.BackColor = btnGray;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.ForeColor = System.Drawing.Color.White;
            btnRefresh.Location = new System.Drawing.Point(635, -2);
            btnRefresh.Size = new System.Drawing.Size(120, 42);

            // 
            // treePanel
            // 
            treePanel.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            treePanel.Controls.Add(tvFiles);
            treePanel.Dock = DockStyle.Left;
            treePanel.Location = new System.Drawing.Point(0, 90);
            treePanel.Name = "treePanel";
            treePanel.Padding = new Padding(5);
            treePanel.Size = new System.Drawing.Size(270, 490);
            treePanel.TabIndex = 1;

            // 
            // tvFiles
            // 
            tvFiles.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            tvFiles.BorderStyle = BorderStyle.None;
            tvFiles.Dock = DockStyle.Fill;
            tvFiles.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            tvFiles.ForeColor = System.Drawing.Color.White;
            tvFiles.LineColor = System.Drawing.Color.FromArgb(100, 100, 100);
            tvFiles.Location = new System.Drawing.Point(5, 5);
            tvFiles.Name = "tvFiles";
            tvFiles.Size = new System.Drawing.Size(260, 480);
            tvFiles.TabIndex = 0;

            // 
            // rightPanel
            // 
            rightPanel.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            rightPanel.Controls.Add(lblInstructions); // instruction label on top
            rightPanel.Controls.Add(rightSplitContainer);
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.Location = new System.Drawing.Point(270, 90);
            rightPanel.Name = "rightPanel";
            rightPanel.Padding = new Padding(5);
            rightPanel.Size = new System.Drawing.Size(730, 490);
            rightPanel.TabIndex = 0;

            // 
            // lblInstructions
            // 
            lblInstructions.Dock = DockStyle.Fill;
            lblInstructions.ForeColor = System.Drawing.Color.White;
            lblInstructions.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Italic);
            lblInstructions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblInstructions.Text = "📂 Press 'Open' to choose a folder and explore its contents.";
            lblInstructions.Name = "lblInstructions";

            // 
            // rightSplitContainer
            // 
            rightSplitContainer.Dock = DockStyle.Fill;
            rightSplitContainer.Location = new System.Drawing.Point(5, 5);
            rightSplitContainer.Name = "rightSplitContainer";
            rightSplitContainer.Orientation = Orientation.Horizontal;
            rightSplitContainer.Panel1.Controls.Add(pbPreview);
            rightSplitContainer.Panel2.Controls.Add(txtFileContent);
            rightSplitContainer.Size = new System.Drawing.Size(720, 480);
            rightSplitContainer.SplitterDistance = 238;
            rightSplitContainer.SplitterWidth = 6;
            rightSplitContainer.TabIndex = 0;

            // 
            // pbPreview
            // 
            pbPreview.Dock = DockStyle.Fill;
            pbPreview.Name = "pbPreview";
            pbPreview.SizeMode = PictureBoxSizeMode.Zoom;

            // 
            // txtFileContent
            // 
            txtFileContent.Dock = DockStyle.Fill;
            txtFileContent.Multiline = true;
            txtFileContent.ScrollBars = ScrollBars.Both;
            txtFileContent.WordWrap = false;
            txtFileContent.Font = new System.Drawing.Font("Consolas", 10F);
            txtFileContent.ForeColor = System.Drawing.Color.White;
            txtFileContent.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            txtFileContent.BorderStyle = BorderStyle.None;

            // 
            // lblStatus
            // 
            lblStatus.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            lblStatus.Dock = DockStyle.Bottom;
            lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblStatus.ForeColor = System.Drawing.Color.White;
            lblStatus.Location = new System.Drawing.Point(0, 580);
            lblStatus.Name = "lblStatus";
            lblStatus.Padding = new Padding(10, 0, 0, 0);
            lblStatus.Size = new System.Drawing.Size(1000, 20);
            lblStatus.TabIndex = 3;
            lblStatus.Text = "Ready";
            lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 
            // Form1
            // 
            BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            ClientSize = new System.Drawing.Size(1000, 600);
            Controls.Add(rightPanel);
            Controls.Add(treePanel);
            Controls.Add(topPanel);
            Controls.Add(lblStatus);
            Font = new System.Drawing.Font("Segoe UI", 9F);
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
