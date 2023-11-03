namespace MobiFlight.UI.Panels.Input
{
    partial class ButtonPanel
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.onPressActionConfigPanel = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.onPressTabPage = new System.Windows.Forms.TabPage();
            this.onPressActionTypePanel = new MobiFlight.UI.Panels.Config.ActionTypePanel();
            this.onReleaseTabPage = new System.Windows.Forms.TabPage();
            this.onReleaseActionConfigPanel = new System.Windows.Forms.Panel();
            this.onReleaseActionTypePanel = new MobiFlight.UI.Panels.Config.ActionTypePanel();
            this.onLongPressTabPage = new System.Windows.Forms.TabPage();
            this.onLongPressActionConfigPanel = new System.Windows.Forms.Panel();
            this.actionTypePanel1 = new MobiFlight.UI.Panels.Config.ActionTypePanel();
            this.onLongPressSettingsPanel = new System.Windows.Forms.Panel();
            this.msLabel = new System.Windows.Forms.Label();
            this.repeatTextBox = new System.Windows.Forms.TextBox();
            this.repeatLabel = new System.Windows.Forms.Label();
            this.delayLabel = new System.Windows.Forms.Label();
            this.delayTextBox = new System.Windows.Forms.TextBox();
            this.onLongReleaseTabPage = new System.Windows.Forms.TabPage();
            this.onLongRelActionConfigPanel = new System.Windows.Forms.Panel();
            this.onLongReleaseActionTypePanel = new MobiFlight.UI.Panels.Config.ActionTypePanel();        
            this.tabControl1.SuspendLayout();
            this.onPressTabPage.SuspendLayout();
            this.onReleaseTabPage.SuspendLayout();
            this.onLongPressTabPage.SuspendLayout();
            this.onLongPressSettingsPanel.SuspendLayout();
            this.onLongReleaseTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // onPressActionConfigPanel
            // 
            this.onPressActionConfigPanel.AutoSize = true;
            this.onPressActionConfigPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.onPressActionConfigPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.onPressActionConfigPanel.Location = new System.Drawing.Point(0, 59);
            this.onPressActionConfigPanel.Margin = new System.Windows.Forms.Padding(0);
            this.onPressActionConfigPanel.MinimumSize = new System.Drawing.Size(100, 154);
            this.onPressActionConfigPanel.Name = "onPressActionConfigPanel";
            this.onPressActionConfigPanel.Size = new System.Drawing.Size(592, 154);
            this.onPressActionConfigPanel.TabIndex = 19;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.onPressTabPage);
            this.tabControl1.Controls.Add(this.onReleaseTabPage);
            this.tabControl1.Controls.Add(this.onLongPressTabPage);
            this.tabControl1.Controls.Add(this.onLongReleaseTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(600, 154);
            this.tabControl1.TabIndex = 21;
            // 
            // onPressTabPage
            // 
            this.onPressTabPage.Controls.Add(this.onPressActionConfigPanel);
            this.onPressTabPage.Controls.Add(this.onPressActionTypePanel);
            this.onPressTabPage.Location = new System.Drawing.Point(4, 29);
            this.onPressTabPage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.onPressTabPage.Name = "onPressTabPage";
            this.onPressTabPage.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.onPressTabPage.Size = new System.Drawing.Size(592, 121);
            this.onPressTabPage.TabIndex = 1;
            this.onPressTabPage.Text = "On Press";
            this.onPressTabPage.UseVisualStyleBackColor = true;
            // 
            // onPressActionTypePanel
            // 
            this.onPressActionTypePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.onPressActionTypePanel.Location = new System.Drawing.Point(0, 5);
            this.onPressActionTypePanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 9);
            this.onPressActionTypePanel.Name = "onPressActionTypePanel";
            this.onPressActionTypePanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 9);
            this.onPressActionTypePanel.Size = new System.Drawing.Size(592, 54);
            this.onPressActionTypePanel.TabIndex = 20;
            // 
            // onReleaseTabPage
            // 
            this.onReleaseTabPage.Controls.Add(this.onReleaseActionConfigPanel);
            this.onReleaseTabPage.Controls.Add(this.onReleaseActionTypePanel);
            this.onReleaseTabPage.Location = new System.Drawing.Point(4, 29);
            this.onReleaseTabPage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.onReleaseTabPage.Name = "onReleaseTabPage";
            this.onReleaseTabPage.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.onReleaseTabPage.Size = new System.Drawing.Size(592, 121);
            this.onReleaseTabPage.TabIndex = 2;
            this.onReleaseTabPage.Text = "On Release";
            this.onReleaseTabPage.UseVisualStyleBackColor = true;
            // 
            // onReleaseActionConfigPanel
            // 
            this.onReleaseActionConfigPanel.AutoSize = true;
            this.onReleaseActionConfigPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.onReleaseActionConfigPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.onReleaseActionConfigPanel.Location = new System.Drawing.Point(0, 59);
            this.onReleaseActionConfigPanel.Margin = new System.Windows.Forms.Padding(0);
            this.onReleaseActionConfigPanel.MinimumSize = new System.Drawing.Size(0, 154);
            this.onReleaseActionConfigPanel.Name = "onReleaseActionConfigPanel";
            this.onReleaseActionConfigPanel.Size = new System.Drawing.Size(592, 154);
            this.onReleaseActionConfigPanel.TabIndex = 19;
            // 
            // onReleaseActionTypePanel
            // 
            this.onReleaseActionTypePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.onReleaseActionTypePanel.Location = new System.Drawing.Point(0, 5);
            this.onReleaseActionTypePanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 9);
            this.onReleaseActionTypePanel.Name = "onReleaseActionTypePanel";
            this.onReleaseActionTypePanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 9);
            this.onReleaseActionTypePanel.Size = new System.Drawing.Size(592, 54);
            this.onReleaseActionTypePanel.TabIndex = 20;
            // 
            // onLongPressTabPage
            // 
            this.onLongPressTabPage.Controls.Add(this.onLongPressActionConfigPanel);
            this.onLongPressTabPage.Controls.Add(this.actionTypePanel1);
            this.onLongPressTabPage.Controls.Add(this.onLongPressSettingsPanel);
            this.onLongPressTabPage.Location = new System.Drawing.Point(4, 29);
            this.onLongPressTabPage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.onLongPressTabPage.Name = "onLongPressTabPage";
            this.onLongPressTabPage.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.onLongPressTabPage.Size = new System.Drawing.Size(592, 121);
            this.onLongPressTabPage.TabIndex = 3;
            this.onLongPressTabPage.Text = "On Long Press";
            this.onLongPressTabPage.UseVisualStyleBackColor = true;
            // 
            // onLongPressActionConfigPanel
            // 
            this.onLongPressActionConfigPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.onLongPressActionConfigPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.onLongPressActionConfigPanel.Location = new System.Drawing.Point(0, 115);
            this.onLongPressActionConfigPanel.MinimumSize = new System.Drawing.Size(0, 154);
            this.onLongPressActionConfigPanel.Name = "onLongPressActionConfigPanel";
            this.onLongPressActionConfigPanel.Size = new System.Drawing.Size(592, 154);
            this.onLongPressActionConfigPanel.TabIndex = 2;
            // 
            // actionTypePanel1
            // 
            this.actionTypePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.actionTypePanel1.Location = new System.Drawing.Point(0, 59);
            this.actionTypePanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 9);
            this.actionTypePanel1.Name = "actionTypePanel1";
            this.actionTypePanel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 9);
            this.actionTypePanel1.Size = new System.Drawing.Size(592, 56);
            this.actionTypePanel1.TabIndex = 1;
            // 
            // onLongPressSettingsPanel
            // 
            this.onLongPressSettingsPanel.BackColor = System.Drawing.Color.Transparent;
            this.onLongPressSettingsPanel.Controls.Add(this.msLabel);
            this.onLongPressSettingsPanel.Controls.Add(this.repeatTextBox);
            this.onLongPressSettingsPanel.Controls.Add(this.repeatLabel);
            this.onLongPressSettingsPanel.Controls.Add(this.delayLabel);
            this.onLongPressSettingsPanel.Controls.Add(this.delayTextBox);
            this.onLongPressSettingsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.onLongPressSettingsPanel.Location = new System.Drawing.Point(0, 5);
            this.onLongPressSettingsPanel.Name = "onLongPressSettingsPanel";
            this.onLongPressSettingsPanel.Padding = new System.Windows.Forms.Padding(3);
            this.onLongPressSettingsPanel.Size = new System.Drawing.Size(592, 54);
            this.onLongPressSettingsPanel.TabIndex = 0;
            // 
            // msLabel
            // 
            this.msLabel.AutoSize = true;
            this.msLabel.Location = new System.Drawing.Point(410, 18);
            this.msLabel.Name = "msLabel";
            this.msLabel.Size = new System.Drawing.Size(30, 20);
            this.msLabel.TabIndex = 4;
            this.msLabel.Text = "ms";
            // 
            // repeatTextBox
            // 
            this.repeatTextBox.Location = new System.Drawing.Point(348, 15);
            this.repeatTextBox.Name = "repeatTextBox";
            this.repeatTextBox.Size = new System.Drawing.Size(56, 26);
            this.repeatTextBox.TabIndex = 3;
            this.repeatTextBox.Text = "0";
            this.repeatTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.repeatTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.repeatTextBox_KeyPress);
            // 
            // repeatLabel
            // 
            this.repeatLabel.AutoSize = true;
            this.repeatLabel.Location = new System.Drawing.Point(217, 18);
            this.repeatLabel.Name = "repeatLabel";
            this.repeatLabel.Size = new System.Drawing.Size(125, 20);
            this.repeatLabel.TabIndex = 2;
            this.repeatLabel.Text = "ms, repeat every";
            // 
            // delayLabel
            // 
            this.delayLabel.AutoSize = true;
            this.delayLabel.Location = new System.Drawing.Point(17, 18);
            this.delayLabel.Name = "delayLabel";
            this.delayLabel.Size = new System.Drawing.Size(134, 20);
            this.delayLabel.TabIndex = 1;
            this.delayLabel.Text = "Long Press delay:";
            // 
            // delayTextBox
            // 
            this.delayTextBox.Location = new System.Drawing.Point(160, 15);
            this.delayTextBox.Name = "delayTextBox";
            this.delayTextBox.Size = new System.Drawing.Size(51, 26);
            this.delayTextBox.TabIndex = 0;
            this.delayTextBox.Text = "400";
            this.delayTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.delayTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.delayTextBox_KeyPress);
            // 
            // onLongReleaseTabPage
            // 
            this.onLongReleaseTabPage.Controls.Add(this.onLongRelActionConfigPanel);
            this.onLongReleaseTabPage.Controls.Add(this.onLongReleaseActionTypePanel);
            this.onLongReleaseTabPage.Location = new System.Drawing.Point(4, 29);
            this.onLongReleaseTabPage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.onLongReleaseTabPage.Name = "onLongReleaseTabPage";
            this.onLongReleaseTabPage.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.onLongReleaseTabPage.Size = new System.Drawing.Size(592, 121);
            this.onLongReleaseTabPage.TabIndex = 4;
            this.onLongReleaseTabPage.Text = "On Long Release";
            this.onLongReleaseTabPage.UseVisualStyleBackColor = true;
            // 
            // onLongRelActionConfigPanel
            // 
            this.onLongRelActionConfigPanel.AutoSize = true;
            this.onLongRelActionConfigPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.onLongRelActionConfigPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.onLongRelActionConfigPanel.Location = new System.Drawing.Point(0, 60);
            this.onLongRelActionConfigPanel.MinimumSize = new System.Drawing.Size(0, 154);
            this.onLongRelActionConfigPanel.Name = "onLongRelActionConfigPanel";
            this.onLongRelActionConfigPanel.Size = new System.Drawing.Size(592, 154);
            this.onLongRelActionConfigPanel.TabIndex = 2;
            // 
            // onLongReleaseActionTypePanel
            // 
            this.onLongReleaseActionTypePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.onLongReleaseActionTypePanel.Location = new System.Drawing.Point(0, 5);
            this.onLongReleaseActionTypePanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 9);
            this.onLongReleaseActionTypePanel.Name = "onLongReleaseActionTypePanel";
            this.onLongReleaseActionTypePanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 9);
            this.onLongReleaseActionTypePanel.Size = new System.Drawing.Size(592, 55);
            this.onLongReleaseActionTypePanel.TabIndex = 1;
            // 
            // ButtonPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(600, 154);
            this.Name = "ButtonPanel";
            this.Size = new System.Drawing.Size(600, 154);
            this.tabControl1.ResumeLayout(false);
            this.onPressTabPage.ResumeLayout(false);
            this.onPressTabPage.PerformLayout();
            this.onReleaseTabPage.ResumeLayout(false);
            this.onReleaseTabPage.PerformLayout();
            this.onLongPressTabPage.ResumeLayout(false);
            this.onLongPressSettingsPanel.ResumeLayout(false);
            this.onLongPressSettingsPanel.PerformLayout();
            this.onLongReleaseTabPage.ResumeLayout(false);
            this.onLongReleaseTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel onPressActionConfigPanel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage onPressTabPage;
        private MobiFlight.UI.Panels.Config.ActionTypePanel onPressActionTypePanel;
        private System.Windows.Forms.TabPage onReleaseTabPage;
        private System.Windows.Forms.Panel onReleaseActionConfigPanel;
        private MobiFlight.UI.Panels.Config.ActionTypePanel onReleaseActionTypePanel;
        private System.Windows.Forms.TabPage onLongReleaseTabPage;
        private System.Windows.Forms.Panel onLongRelActionConfigPanel;
        private Config.ActionTypePanel onLongReleaseActionTypePanel;
        private System.Windows.Forms.TabPage onLongPressTabPage;
        private System.Windows.Forms.Panel onLongPressSettingsPanel;
        private Config.ActionTypePanel actionTypePanel1;
        private System.Windows.Forms.Panel onLongPressActionConfigPanel;    
        private System.Windows.Forms.TextBox delayTextBox;
        private System.Windows.Forms.Label msLabel;
        private System.Windows.Forms.TextBox repeatTextBox;
        private System.Windows.Forms.Label repeatLabel;
        private System.Windows.Forms.Label delayLabel;
    }
}
