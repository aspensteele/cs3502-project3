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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            topPanel = new Panel();
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
            topPanel.Controls.Add(txtPath);
            topPanel.Controls.Add(buttonPanel);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(1000, 70);
            topPanel.TabIndex = 2;
            // 
            // txtPath
            // 
            txtPath.Dock = DockStyle.Top;
            txtPath.Location = new Point(0, 0);
            txtPath.Name = "txtPath";
            txtPath.ReadOnly = true;
            txtPath.Size = new Size(1000, 31);
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
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Location = new Point(0, 30);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new Size(1000, 40);
            buttonPanel.TabIndex = 1;
            // 
            // btnCreate
            // 
            btnCreate.Location = new Point(5, 2);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(80, 35);
            btnCreate.TabIndex = 0;
            btnCreate.Text = "Create";
            // 
            // btnOpen
            // 
            btnOpen.Location = new Point(90, 2);
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new Size(80, 35);
            btnOpen.TabIndex = 1;
            btnOpen.Text = "Open";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(175, 2);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(80, 35);
            btnSave.TabIndex = 2;
            btnSave.Text = "Save";
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(260, 2);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(80, 35);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "Delete";
            // 
            // btnRename
            // 
            btnRename.Location = new Point(345, 2);
            btnRename.Name = "btnRename";
            btnRename.Size = new Size(90, 35);
            btnRename.TabIndex = 4;
            btnRename.Text = "Rename";
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(441, 2);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(80, 35);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "Refresh";
            // 
            // treePanel
            // 
            treePanel.Controls.Add(tvFiles);
            treePanel.Dock = DockStyle.Left;
            treePanel.Location = new Point(0, 70);
            treePanel.Name = "treePanel";
            treePanel.Size = new Size(250, 510);
            treePanel.TabIndex = 1;
            // 
            // tvFiles
            // 
            tvFiles.Dock = DockStyle.Fill;
            tvFiles.Location = new Point(0, 0);
            tvFiles.Name = "tvFiles";
            tvFiles.Size = new Size(250, 510);
            tvFiles.TabIndex = 0;
            // 
            // rightPanel
            // 
            rightPanel.Controls.Add(rightSplitContainer);
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.Location = new Point(250, 70);
            rightPanel.Name = "rightPanel";
            rightPanel.Size = new Size(750, 510);
            rightPanel.TabIndex = 0;
            // 
            // rightSplitContainer
            // 
            rightSplitContainer.Dock = DockStyle.Fill;
            rightSplitContainer.Location = new Point(0, 0);
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
            rightSplitContainer.Size = new Size(750, 510);
            rightSplitContainer.SplitterDistance = 255;
            rightSplitContainer.TabIndex = 0;
            // 
            // pbPreview
            // 
            pbPreview.Dock = DockStyle.Fill;
            pbPreview.Location = new Point(0, 0);
            pbPreview.Name = "pbPreview";
            pbPreview.Size = new Size(750, 255);
            pbPreview.SizeMode = PictureBoxSizeMode.Zoom;
            pbPreview.TabIndex = 0;
            pbPreview.TabStop = false;
            // 
            // txtFileContent
            // 
            txtFileContent.Dock = DockStyle.Fill;
            txtFileContent.Location = new Point(0, 0);
            txtFileContent.Multiline = true;
            txtFileContent.Name = "txtFileContent";
            txtFileContent.ScrollBars = ScrollBars.Both;
            txtFileContent.Size = new Size(750, 251);
            txtFileContent.TabIndex = 0;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Bottom;
            lblStatus.Location = new Point(0, 580);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(1000, 20);
            lblStatus.TabIndex = 3;
            // 
            // Form1
            // 
            ClientSize = new Size(1000, 600);
            Controls.Add(rightPanel);
            Controls.Add(treePanel);
            Controls.Add(topPanel);
            Controls.Add(lblStatus);
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
