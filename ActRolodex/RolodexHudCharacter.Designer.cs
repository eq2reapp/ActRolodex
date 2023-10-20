
namespace ACT_Plugin
{
    partial class RolodexHudCharacter
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblChar = new System.Windows.Forms.LinkLabel();
            this.lblGuild = new System.Windows.Forms.LinkLabel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lblClass = new System.Windows.Forms.Label();
            this.lblRank = new System.Windows.Forms.Label();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblChar
            // 
            this.lblChar.ActiveLinkColor = System.Drawing.Color.Teal;
            this.lblChar.AutoSize = true;
            this.lblChar.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChar.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lblChar.LinkColor = System.Drawing.SystemColors.Highlight;
            this.lblChar.Location = new System.Drawing.Point(6, 9);
            this.lblChar.Name = "lblChar";
            this.lblChar.Size = new System.Drawing.Size(81, 18);
            this.lblChar.TabIndex = 0;
            this.lblChar.TabStop = true;
            this.lblChar.Text = "Character";
            this.lblChar.VisitedLinkColor = System.Drawing.SystemColors.Highlight;
            this.lblChar.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblChar_LinkClicked);
            // 
            // lblGuild
            // 
            this.lblGuild.ActiveLinkColor = System.Drawing.Color.Teal;
            this.lblGuild.AutoSize = true;
            this.lblGuild.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGuild.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lblGuild.LinkColor = System.Drawing.SystemColors.Highlight;
            this.lblGuild.Location = new System.Drawing.Point(6, 34);
            this.lblGuild.Name = "lblGuild";
            this.lblGuild.Size = new System.Drawing.Size(44, 18);
            this.lblGuild.TabIndex = 1;
            this.lblGuild.TabStop = true;
            this.lblGuild.Text = "Guild";
            this.lblGuild.VisitedLinkColor = System.Drawing.SystemColors.Highlight;
            this.lblGuild.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblGuild_LinkClicked);
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.SystemColors.Window;
            this.pnlMain.Controls.Add(this.lblGuild);
            this.pnlMain.Controls.Add(this.lblChar);
            this.pnlMain.Controls.Add(this.lblClass);
            this.pnlMain.Controls.Add(this.lblRank);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(2, 2);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(539, 63);
            this.pnlMain.TabIndex = 2;
            // 
            // lblClass
            // 
            this.lblClass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClass.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClass.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblClass.Location = new System.Drawing.Point(279, 7);
            this.lblClass.Name = "lblClass";
            this.lblClass.Size = new System.Drawing.Size(184, 23);
            this.lblClass.TabIndex = 3;
            this.lblClass.Text = "Class";
            this.lblClass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRank
            // 
            this.lblRank.BackColor = System.Drawing.SystemColors.Highlight;
            this.lblRank.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblRank.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblRank.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblRank.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRank.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.lblRank.Location = new System.Drawing.Point(469, 0);
            this.lblRank.Name = "lblRank";
            this.lblRank.Size = new System.Drawing.Size(70, 63);
            this.lblRank.TabIndex = 2;
            this.lblRank.Text = "150";
            this.lblRank.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRank.Click += new System.EventHandler(this.lblRank_Click);
            // 
            // RolodexHudCharacter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.Controls.Add(this.pnlMain);
            this.Name = "RolodexHudCharacter";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(543, 67);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.LinkLabel lblChar;
        private System.Windows.Forms.LinkLabel lblGuild;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblRank;
        private System.Windows.Forms.Label lblClass;
    }
}
