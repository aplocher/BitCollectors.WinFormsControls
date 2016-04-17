namespace BitCollectors.WinFormsControls
{
    partial class BorderEditControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BorderEditControl));
            this._penSizeTextbox = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._systemDefaultCheckbox = new System.Windows.Forms.CheckBox();
            this.PreviewPicBox = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ColorSimpleComposite = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.PreviewPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this._colorComboBox = new BitCollectors.WinFormsControls.ColorComboBoxEx();
            this._colorTextbox = new BitCollectors.WinFormsControls.ColorTextBoxEx();
            ((System.ComponentModel.ISupportInitialize)(this._penSizeTextbox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewPicBox)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.ColorSimpleComposite.SuspendLayout();
            this.panel2.SuspendLayout();
            this.PreviewPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _penSizeTextbox
            // 
            this._penSizeTextbox.Location = new System.Drawing.Point(39, 3);
            this._penSizeTextbox.Name = "_penSizeTextbox";
            this._penSizeTextbox.Size = new System.Drawing.Size(62, 20);
            this._penSizeTextbox.TabIndex = 1;
            this._penSizeTextbox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._penSizeTextbox.ValueChanged += new System.EventHandler(this.PenSizeNumTextboxValueChanged);
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(8, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Color:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Size:";
            // 
            // _systemDefaultCheckbox
            // 
            this._systemDefaultCheckbox.AutoSize = true;
            this._systemDefaultCheckbox.BackColor = System.Drawing.Color.Transparent;
            this._systemDefaultCheckbox.ForeColor = System.Drawing.Color.Black;
            this._systemDefaultCheckbox.Location = new System.Drawing.Point(5, 7);
            this._systemDefaultCheckbox.Name = "_systemDefaultCheckbox";
            this._systemDefaultCheckbox.Size = new System.Drawing.Size(115, 17);
            this._systemDefaultCheckbox.TabIndex = 1;
            this._systemDefaultCheckbox.Text = "Use system default";
            this._systemDefaultCheckbox.UseVisualStyleBackColor = false;
            // 
            // PreviewPicBox
            // 
            this.PreviewPicBox.BackColor = System.Drawing.Color.White;
            this.PreviewPicBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PreviewPicBox.Location = new System.Drawing.Point(0, 0);
            this.PreviewPicBox.Name = "PreviewPicBox";
            this.PreviewPicBox.Size = new System.Drawing.Size(116, 67);
            this.PreviewPicBox.TabIndex = 7;
            this.PreviewPicBox.TabStop = false;
            this.PreviewPicBox.MouseEnter += new System.EventHandler(this.PreviewPicBoxMouseEnter);
            this.PreviewPicBox.MouseLeave += new System.EventHandler(this.PreviewPicBoxMouseLeave);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(15, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "PREVIEW";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this._colorComboBox);
            this.flowLayoutPanel1.Controls.Add(this.ColorSimpleComposite);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(5, 32);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(183, 118);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // ColorSimpleComposite
            // 
            this.ColorSimpleComposite.AutoSize = true;
            this.ColorSimpleComposite.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ColorSimpleComposite.Controls.Add(this._colorTextbox);
            this.ColorSimpleComposite.Location = new System.Drawing.Point(40, 21);
            this.ColorSimpleComposite.Margin = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.ColorSimpleComposite.Name = "ColorSimpleComposite";
            this.ColorSimpleComposite.Size = new System.Drawing.Size(140, 22);
            this.ColorSimpleComposite.TabIndex = 9;
            this.ColorSimpleComposite.WrapContents = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this._penSizeTextbox);
            this.panel2.Controls.Add(this.PreviewPanel);
            this.panel2.Location = new System.Drawing.Point(0, 46);
            this.panel2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(180, 35);
            this.panel2.TabIndex = 1;
            // 
            // PreviewPanel
            // 
            this.PreviewPanel.Controls.Add(this.label3);
            this.PreviewPanel.Controls.Add(this.PreviewPicBox);
            this.PreviewPanel.Location = new System.Drawing.Point(103, 2);
            this.PreviewPanel.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.PreviewPanel.Name = "PreviewPanel";
            this.PreviewPanel.Size = new System.Drawing.Size(116, 67);
            this.PreviewPanel.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Location = new System.Drawing.Point(3, 87);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(177, 28);
            this.panel1.TabIndex = 3;
            this.panel1.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(115, 2);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(62, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(50, 2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(62, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // _colorComboBox
            // 
            this._colorComboBox.Location = new System.Drawing.Point(40, 0);
            this._colorComboBox.Margin = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this._colorComboBox.Name = "_colorComboBox";
            this._colorComboBox.ShowColorPreview = false;
            this._colorComboBox.Size = new System.Drawing.Size(140, 21);
            this._colorComboBox.TabIndex = 0;
            // 
            // _colorTextbox
            // 
            this._colorTextbox.BackColor = System.Drawing.SystemColors.Window;
            this._colorTextbox.ButtonImage = ((System.Drawing.Image)(resources.GetObject("_colorTextbox.ButtonImage")));
            this._colorTextbox.ButtonImageMouseOver = ((System.Drawing.Image)(resources.GetObject("_colorTextbox.ButtonImageMouseOver")));
            this._colorTextbox.ButtonText = null;
            this._colorTextbox.ButtonTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._colorTextbox.Font = new System.Drawing.Font("Consolas", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._colorTextbox.ForeColor = System.Drawing.Color.Red;
            this._colorTextbox.Location = new System.Drawing.Point(0, 0);
            this._colorTextbox.Margin = new System.Windows.Forms.Padding(0);
            this._colorTextbox.MoreColorsImage = null;
            this._colorTextbox.MoreColorsImageMouseOver = null;
            this._colorTextbox.Name = "_colorTextbox";
            this._colorTextbox.ShowColorPreview = false;
            this._colorTextbox.Size = new System.Drawing.Size(140, 22);
            this._colorTextbox.TabIndex = 0;
            this._colorTextbox.WatermarkForeColor = System.Drawing.Color.Red;
            this._colorTextbox.TextChanged += new System.EventHandler(this.ColorTextboxTextChanged);
            // 
            // BorderEditControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this._systemDefaultCheckbox);
            this.Name = "BorderEditControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(193, 155);
            ((System.ComponentModel.ISupportInitialize)(this._penSizeTextbox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewPicBox)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ColorSimpleComposite.ResumeLayout(false);
            this.ColorSimpleComposite.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.PreviewPanel.ResumeLayout(false);
            this.PreviewPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown _penSizeTextbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox _systemDefaultCheckbox;
        private System.Windows.Forms.PictureBox PreviewPicBox;
        private System.Windows.Forms.Label label3;
        private ColorComboBoxEx _colorComboBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel ColorSimpleComposite;
        private System.Windows.Forms.Panel PreviewPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private ColorTextBoxEx _colorTextbox;
    }
}
