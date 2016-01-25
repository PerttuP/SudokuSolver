namespace SudokuGame
{
    partial class MainWindow
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
            this.btnNewGame = new System.Windows.Forms.Button();
            this.btnSolve = new System.Windows.Forms.Button();
            this.btnHint = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnNewGame
            // 
            this.btnNewGame.Location = new System.Drawing.Point(524, 370);
            this.btnNewGame.Name = "btnNewGame";
            this.btnNewGame.Size = new System.Drawing.Size(257, 42);
            this.btnNewGame.TabIndex = 0;
            this.btnNewGame.Text = "New Game";
            this.btnNewGame.UseVisualStyleBackColor = true;
            this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
            // 
            // btnSolve
            // 
            this.btnSolve.Location = new System.Drawing.Point(524, 225);
            this.btnSolve.Name = "btnSolve";
            this.btnSolve.Size = new System.Drawing.Size(257, 42);
            this.btnSolve.TabIndex = 1;
            this.btnSolve.Text = "Solve!";
            this.btnSolve.UseVisualStyleBackColor = true;
            this.btnSolve.Visible = false;
            this.btnSolve.Click += new System.EventHandler(this.btnSolve_Click);
            // 
            // btnHint
            // 
            this.btnHint.Location = new System.Drawing.Point(524, 177);
            this.btnHint.Name = "btnHint";
            this.btnHint.Size = new System.Drawing.Size(257, 42);
            this.btnHint.TabIndex = 2;
            this.btnHint.Text = "Hint";
            this.btnHint.UseVisualStyleBackColor = true;
            this.btnHint.Visible = false;
            this.btnHint.Click += new System.EventHandler(this.btnHint_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(524, 274);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(257, 42);
            this.btnReset.TabIndex = 3;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Visible = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(524, 446);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(257, 42);
            this.btnHelp.TabIndex = 4;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(524, 322);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(257, 42);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save Game";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(811, 500);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnHint);
            this.Controls.Add(this.btnSolve);
            this.Controls.Add(this.btnNewGame);
            this.Name = "MainWindow";
            this.Text = "SudokuGame";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNewGame;
        private System.Windows.Forms.Button btnSolve;
        private System.Windows.Forms.Button btnHint;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnSave;
    }
}

