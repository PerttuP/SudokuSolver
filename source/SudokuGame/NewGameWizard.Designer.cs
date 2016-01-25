namespace SudokuGame
{
    partial class NewGameWizard
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
            this.tabLoadGame = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnFileOK = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.btnFileCancel = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblSelectFile = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabNumberEditor = new System.Windows.Forms.TabPage();
            this.tabLayoutEditor = new System.Windows.Forms.TabPage();
            this.btnEditorOK = new System.Windows.Forms.Button();
            this.btnEditorCancel = new System.Windows.Forms.Button();
            this.tabLoadGame.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabLoadGame
            // 
            this.tabLoadGame.Controls.Add(this.tabPage1);
            this.tabLoadGame.Controls.Add(this.tabPage2);
            this.tabLoadGame.Location = new System.Drawing.Point(12, 12);
            this.tabLoadGame.Name = "tabLoadGame";
            this.tabLoadGame.SelectedIndex = 0;
            this.tabLoadGame.Size = new System.Drawing.Size(546, 387);
            this.tabLoadGame.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblSelectFile);
            this.tabPage1.Controls.Add(this.btnFileOK);
            this.tabPage1.Controls.Add(this.txtFileName);
            this.tabPage1.Controls.Add(this.btnFileCancel);
            this.tabPage1.Controls.Add(this.btnBrowse);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(538, 361);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Load Game";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnFileOK
            // 
            this.btnFileOK.Location = new System.Drawing.Point(460, 332);
            this.btnFileOK.Name = "btnFileOK";
            this.btnFileOK.Size = new System.Drawing.Size(75, 23);
            this.btnFileOK.TabIndex = 2;
            this.btnFileOK.Text = "OK";
            this.btnFileOK.UseVisualStyleBackColor = true;
            this.btnFileOK.Click += new System.EventHandler(this.btnFileOK_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(6, 70);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(450, 20);
            this.txtFileName.TabIndex = 1;
            // 
            // btnFileCancel
            // 
            this.btnFileCancel.Location = new System.Drawing.Point(379, 332);
            this.btnFileCancel.Name = "btnFileCancel";
            this.btnFileCancel.Size = new System.Drawing.Size(75, 23);
            this.btnFileCancel.TabIndex = 1;
            this.btnFileCancel.Text = "Cancel";
            this.btnFileCancel.UseVisualStyleBackColor = true;
            this.btnFileCancel.Click += new System.EventHandler(this.btnFileCancel_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(462, 70);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(70, 23);
            this.btnBrowse.TabIndex = 0;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnEditorCancel);
            this.tabPage2.Controls.Add(this.btnEditorOK);
            this.tabPage2.Controls.Add(this.tabControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(538, 361);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Create Game";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblSelectFile
            // 
            this.lblSelectFile.AutoSize = true;
            this.lblSelectFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectFile.Location = new System.Drawing.Point(6, 3);
            this.lblSelectFile.Name = "lblSelectFile";
            this.lblSelectFile.Size = new System.Drawing.Size(188, 25);
            this.lblSelectFile.TabIndex = 3;
            this.lblSelectFile.Text = "Select game file:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabNumberEditor);
            this.tabControl1.Controls.Add(this.tabLayoutEditor);
            this.tabControl1.Location = new System.Drawing.Point(6, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(526, 320);
            this.tabControl1.TabIndex = 0;
            // 
            // tabNumberEditor
            // 
            this.tabNumberEditor.Location = new System.Drawing.Point(4, 22);
            this.tabNumberEditor.Name = "tabNumberEditor";
            this.tabNumberEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tabNumberEditor.Size = new System.Drawing.Size(518, 323);
            this.tabNumberEditor.TabIndex = 0;
            this.tabNumberEditor.Text = "Initial Numbers";
            this.tabNumberEditor.UseVisualStyleBackColor = true;
            // 
            // tabLayoutEditor
            // 
            this.tabLayoutEditor.Location = new System.Drawing.Point(4, 22);
            this.tabLayoutEditor.Name = "tabLayoutEditor";
            this.tabLayoutEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tabLayoutEditor.Size = new System.Drawing.Size(518, 294);
            this.tabLayoutEditor.TabIndex = 1;
            this.tabLayoutEditor.Text = "Layout";
            this.tabLayoutEditor.UseVisualStyleBackColor = true;
            // 
            // btnEditorOK
            // 
            this.btnEditorOK.Location = new System.Drawing.Point(457, 332);
            this.btnEditorOK.Name = "btnEditorOK";
            this.btnEditorOK.Size = new System.Drawing.Size(75, 23);
            this.btnEditorOK.TabIndex = 1;
            this.btnEditorOK.Text = "OK";
            this.btnEditorOK.UseVisualStyleBackColor = true;
            this.btnEditorOK.Click += new System.EventHandler(this.btnEditorOK_Click);
            // 
            // btnEditorCancel
            // 
            this.btnEditorCancel.Location = new System.Drawing.Point(376, 332);
            this.btnEditorCancel.Name = "btnEditorCancel";
            this.btnEditorCancel.Size = new System.Drawing.Size(75, 23);
            this.btnEditorCancel.TabIndex = 2;
            this.btnEditorCancel.Text = "Cancel";
            this.btnEditorCancel.UseVisualStyleBackColor = true;
            this.btnEditorCancel.Click += new System.EventHandler(this.btnEditorCancel_Click);
            // 
            // NewGameWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 411);
            this.Controls.Add(this.tabLoadGame);
            this.Name = "NewGameWizard";
            this.Text = "NewGameWizard";
            this.tabLoadGame.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabLoadGame;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnFileCancel;
        private System.Windows.Forms.Button btnFileOK;
        private System.Windows.Forms.Label lblSelectFile;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabNumberEditor;
        private System.Windows.Forms.TabPage tabLayoutEditor;
        private System.Windows.Forms.Button btnEditorCancel;
        private System.Windows.Forms.Button btnEditorOK;
    }
}