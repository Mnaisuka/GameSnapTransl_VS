
namespace Main
{
    partial class UI2_Body
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
            this.UI2_Button_TL = new System.Windows.Forms.Button();
            this.UI2_TextBox_Input = new System.Windows.Forms.TextBox();
            this.UI2_Pnel1 = new System.Windows.Forms.Panel();
            this.UI2_RichTextBox = new System.Windows.Forms.RichTextBox();
            this.UI2_Pnel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // UI2_Button_TL
            // 
            this.UI2_Button_TL.Location = new System.Drawing.Point(208, 7);
            this.UI2_Button_TL.Margin = new System.Windows.Forms.Padding(0);
            this.UI2_Button_TL.Name = "UI2_Button_TL";
            this.UI2_Button_TL.Size = new System.Drawing.Size(41, 26);
            this.UI2_Button_TL.TabIndex = 4;
            this.UI2_Button_TL.Text = "翻译";
            this.UI2_Button_TL.UseVisualStyleBackColor = true;
            this.UI2_Button_TL.Click += new System.EventHandler(this.button1_Click);
            // 
            // UI2_TextBox_Input
            // 
            this.UI2_TextBox_Input.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UI2_TextBox_Input.Location = new System.Drawing.Point(8, 8);
            this.UI2_TextBox_Input.Margin = new System.Windows.Forms.Padding(0);
            this.UI2_TextBox_Input.Multiline = true;
            this.UI2_TextBox_Input.Name = "UI2_TextBox_Input";
            this.UI2_TextBox_Input.Size = new System.Drawing.Size(192, 24);
            this.UI2_TextBox_Input.TabIndex = 3;
            this.UI2_TextBox_Input.WordWrap = false;
            this.UI2_TextBox_Input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UI2_TextBox_Input_KeyDown);
            this.UI2_TextBox_Input.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UI2_TextBox_Input_KeyUp);
            // 
            // UI2_Pnel1
            // 
            this.UI2_Pnel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UI2_Pnel1.Controls.Add(this.UI2_RichTextBox);
            this.UI2_Pnel1.Location = new System.Drawing.Point(8, 41);
            this.UI2_Pnel1.Name = "UI2_Pnel1";
            this.UI2_Pnel1.Size = new System.Drawing.Size(240, 117);
            this.UI2_Pnel1.TabIndex = 6;
            // 
            // UI2_RichTextBox
            // 
            this.UI2_RichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.UI2_RichTextBox.Location = new System.Drawing.Point(-1, -1);
            this.UI2_RichTextBox.Name = "UI2_RichTextBox";
            this.UI2_RichTextBox.Size = new System.Drawing.Size(240, 117);
            this.UI2_RichTextBox.TabIndex = 0;
            this.UI2_RichTextBox.Text = "";
            // 
            // UI2_Body
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 165);
            this.Controls.Add(this.UI2_Pnel1);
            this.Controls.Add(this.UI2_Button_TL);
            this.Controls.Add(this.UI2_TextBox_Input);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UI2_Body";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "中译外";
            this.TopMost = true;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.UI2_Body_HelpButtonClicked);
            this.Load += new System.EventHandler(this.UI_Body_2_Load);
            this.UI2_Pnel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button UI2_Button_TL;
        private System.Windows.Forms.TextBox UI2_TextBox_Input;
        private System.Windows.Forms.Panel UI2_Pnel1;
        private System.Windows.Forms.RichTextBox UI2_RichTextBox;
    }
}