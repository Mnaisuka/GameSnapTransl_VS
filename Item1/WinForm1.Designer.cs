namespace Main
{
    partial class UI_Body
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.UI_PanelBox = new System.Windows.Forms.Panel();
            this.UI_TextBox = new System.Windows.Forms.TextBox();
            this.UI_Label_Tip = new System.Windows.Forms.Label();
            this.UI_PanelBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // UI_PanelBox
            // 
            this.UI_PanelBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UI_PanelBox.Controls.Add(this.UI_TextBox);
            this.UI_PanelBox.Controls.Add(this.UI_Label_Tip);
            this.UI_PanelBox.Location = new System.Drawing.Point(0, 0);
            this.UI_PanelBox.Name = "UI_PanelBox";
            this.UI_PanelBox.Size = new System.Drawing.Size(330, 216);
            this.UI_PanelBox.TabIndex = 3;
            this.UI_PanelBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.UI_PanelBox_MouseDown);
            // 
            // UI_TextBox
            // 
            this.UI_TextBox.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UI_TextBox.Location = new System.Drawing.Point(8, 8);
            this.UI_TextBox.Multiline = true;
            this.UI_TextBox.Name = "UI_TextBox";
            this.UI_TextBox.Size = new System.Drawing.Size(312, 176);
            this.UI_TextBox.TabIndex = 0;
            this.UI_TextBox.DoubleClick += new System.EventHandler(this.UI_TextBox_DoubleClick);
            // 
            // UI_Label_Tip
            // 
            this.UI_Label_Tip.Location = new System.Drawing.Point(8, 184);
            this.UI_Label_Tip.Margin = new System.Windows.Forms.Padding(0);
            this.UI_Label_Tip.Name = "UI_Label_Tip";
            this.UI_Label_Tip.Size = new System.Drawing.Size(312, 28);
            this.UI_Label_Tip.TabIndex = 1;
            this.UI_Label_Tip.Text = "可拖动窗口";
            this.UI_Label_Tip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.UI_Label_Tip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.UI_Label_Tip_MouseDown);
            // 
            // UI_Body
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 216);
            this.Controls.Add(this.UI_PanelBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UI_Body";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.UI_Body_Load);
            this.UI_PanelBox.ResumeLayout(false);
            this.UI_PanelBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel UI_PanelBox;
        private System.Windows.Forms.TextBox UI_TextBox;
        private System.Windows.Forms.Label UI_Label_Tip;
    }
}

