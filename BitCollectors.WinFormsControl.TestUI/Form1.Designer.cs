using BitCollectors.WinFormsControls;

namespace BitCollectors.WinFormsControl.TestUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControlEx1 = new BitCollectors.WinFormsControls.TabControlEx();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.uberTextBox1 = new BitCollectors.WinFormsControls.TextBoxEx();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.treeViewEx1 = new BitCollectors.WinFormsControls.TreeViewEx();
            this.button2 = new System.Windows.Forms.Button();
            this.btnAddTestNodes = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tabControlEx1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlEx1
            // 
            this.tabControlEx1.Controls.Add(this.tabPage1);
            this.tabControlEx1.Controls.Add(this.tabPage2);
            this.tabControlEx1.Location = new System.Drawing.Point(12, 12);
            this.tabControlEx1.Name = "tabControlEx1";
            this.tabControlEx1.SelectedIndex = 0;
            this.tabControlEx1.Size = new System.Drawing.Size(674, 508);
            this.tabControlEx1.TabGroupName = null;
            this.tabControlEx1.TabIndex = 13;
            this.tabControlEx1.TabReorderingMode = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.uberTextBox1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.btnAddTestNodes);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(666, 482);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "TreeViewEx and TextBoxEx";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // uberTextBox1
            // 
            this.uberTextBox1.BackColor = System.Drawing.SystemColors.Window;
            //this.uberTextBox1.EscapeClearsInput = false;
            this.uberTextBox1.ForeColor = System.Drawing.Color.Red;
            this.uberTextBox1.Location = new System.Drawing.Point(321, 359);
            this.uberTextBox1.Multiline = true;
            this.uberTextBox1.Name = "uberTextBox1";
            this.uberTextBox1.Size = new System.Drawing.Size(231, 20);
            this.uberTextBox1.TabIndex = 14;
            this.uberTextBox1.WatermarkText = "";
            this.uberTextBox1.WatermarkForeColor = System.Drawing.Color.Red;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(272, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(388, 179);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "TextBoxEx";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.treeViewEx1);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 470);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Coupled";
            // 
            // treeViewEx1
            // 
            this.treeViewEx1.AllowHorizontalScrollbars = false;
            this.treeViewEx1.AllowVerticalScrollbars = false;
            this.treeViewEx1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewEx1.AttachedSearchControl = null;
            this.treeViewEx1.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeViewEx1.EnterKeyCyclesMatches = false;
            this.treeViewEx1.Location = new System.Drawing.Point(6, 39);
            this.treeViewEx1.Name = "treeViewEx1";
            this.treeViewEx1.SelectedNode = null;
            this.treeViewEx1.Size = new System.Drawing.Size(248, 425);
            this.treeViewEx1.TabIndex = 1;
            this.treeViewEx1.TabStop = false;
            this.treeViewEx1.ThemeStyle = BitCollectors.WinFormsControls.TreeViewEx.TreeViewExThemeStyles.Default;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(528, 454);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(132, 22);
            this.button2.TabIndex = 4;
            this.button2.Text = "Switch Themes";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnTest3_Click);
            // 
            // btnAddTestNodes
            // 
            this.btnAddTestNodes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddTestNodes.Location = new System.Drawing.Point(390, 454);
            this.btnAddTestNodes.Name = "btnAddTestNodes";
            this.btnAddTestNodes.Size = new System.Drawing.Size(132, 22);
            this.btnAddTestNodes.TabIndex = 3;
            this.btnAddTestNodes.Text = "Add Test Nodes";
            this.btnAddTestNodes.UseVisualStyleBackColor = true;
            this.btnAddTestNodes.Click += new System.EventHandler(this.btnAddTestNodes_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.toolStrip1);
            this.tabPage2.Controls.Add(this.statusStrip1);
            this.tabPage2.Controls.Add(this.menuStrip1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(666, 482);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1,
            this.toolStripComboBox1,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(3, 27);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(660, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(3, 457);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(660, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(3, 3);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(660, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 532);
            this.Controls.Add(this.tabControlEx1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControlEx1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddTestNodes;
        private TreeViewEx treeViewEx1;
        private System.Windows.Forms.Button button2;
        private TabControlEx tabControlEx1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private ComboBoxEx maskedTextBox1;
        private TextBoxEx uberTextBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}

