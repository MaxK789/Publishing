namespace Publishing
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

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

        private void InitializeComponent()
        {
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.OrdersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ListMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PersonalDataMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OrganizationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatisticsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OrdersGridView = new System.Windows.Forms.DataGridView();
            this.OrdersLabel = new System.Windows.Forms.Label();
            this.AddMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OrdersGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OrdersMenuItem,
            this.PersonalDataMenuItem,
            this.OrganizationMenuItem,
            this.StatisticsMenuItem,
            this.ExitMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(800, 24);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "MainMenu";
            // 
            // OrdersMenuItem
            // 
            this.OrdersMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ListMenuItem,
            this.AddMenuItem,
            this.DeleteMenuItem});
            this.OrdersMenuItem.Name = "OrdersMenuItem";
            this.OrdersMenuItem.Size = new System.Drawing.Size(87, 20);
            this.OrdersMenuItem.Text = "Замовлення";
            // 
            // ListMenuItem
            // 
            this.ListMenuItem.Name = "ListMenuItem";
            this.ListMenuItem.Size = new System.Drawing.Size(126, 22);
            this.ListMenuItem.Text = "Список";
            this.ListMenuItem.Click += new System.EventHandler(this.ListMenuItem_Click);
            // 
            // DeleteMenuItem
            // 
            this.DeleteMenuItem.Name = "DeleteMenuItem";
            this.DeleteMenuItem.Size = new System.Drawing.Size(126, 22);
            this.DeleteMenuItem.Text = "Видалити";
            this.DeleteMenuItem.Click += new System.EventHandler(this.DeleteMenuItem_Click);
            // 
            // PersonalDataMenuItem
            // 
            this.PersonalDataMenuItem.Name = "PersonalDataMenuItem";
            this.PersonalDataMenuItem.Size = new System.Drawing.Size(115, 20);
            this.PersonalDataMenuItem.Text = "Персональні дані";
            this.PersonalDataMenuItem.Click += new System.EventHandler(this.PersonalDataMenuItem_Click);
            // 
            // OrganizationMenuItem
            // 
            this.OrganizationMenuItem.Name = "OrganizationMenuItem";
            this.OrganizationMenuItem.Size = new System.Drawing.Size(83, 20);
            this.OrganizationMenuItem.Text = "Організація";
            this.OrganizationMenuItem.Click += new System.EventHandler(this.OrganizationMenuItem_Click);
            // 
            // StatisticsMenuItem
            // 
            this.StatisticsMenuItem.Name = "StatisticsMenuItem";
            this.StatisticsMenuItem.Size = new System.Drawing.Size(80, 20);
            this.StatisticsMenuItem.Text = "Статистика";
            this.StatisticsMenuItem.Click += new System.EventHandler(this.StatisticsMenuItem_Click);
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new System.Drawing.Size(52, 20);
            this.ExitMenuItem.Text = "Вийти";
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // OrdersGridView
            // 
            this.OrdersGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.OrdersGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OrdersGridView.Location = new System.Drawing.Point(12, 75);
            this.OrdersGridView.Name = "OrdersGridView";
            this.OrdersGridView.Size = new System.Drawing.Size(776, 363);
            this.OrdersGridView.TabIndex = 1;
            // 
            // OrdersLabel
            // 
            this.OrdersLabel.AutoSize = true;
            this.OrdersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OrdersLabel.Location = new System.Drawing.Point(12, 52);
            this.OrdersLabel.Name = "OrdersLabel";
            this.OrdersLabel.Size = new System.Drawing.Size(234, 20);
            this.OrdersLabel.TabIndex = 2;
            this.OrdersLabel.Text = "Поточні замовлення в роботі:";
            // 
            // AddMenuItem
            // 
            this.AddMenuItem.Name = "AddMenuItem";
            this.AddMenuItem.Size = new System.Drawing.Size(180, 22);
            this.AddMenuItem.Text = "Додати";
            this.AddMenuItem.Click += new System.EventHandler(this.AddMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.OrdersLabel);
            this.Controls.Add(this.OrdersGridView);
            this.Controls.Add(this.MainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.MainMenu;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OrdersGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem PersonalDataMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OrdersMenuItem;
        private System.Windows.Forms.DataGridView OrdersGridView;
        private System.Windows.Forms.ToolStripMenuItem ListMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteMenuItem;
        private System.Windows.Forms.Label OrdersLabel;
        private System.Windows.Forms.ToolStripMenuItem StatisticsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OrganizationMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AddMenuItem;
    }
}
