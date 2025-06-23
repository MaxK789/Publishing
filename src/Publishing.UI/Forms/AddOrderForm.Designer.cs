namespace Publishing
{
    partial class AddOrderForm
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
            this.AddMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PersonalDataMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OrganizationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PrinteryComboBox = new System.Windows.Forms.ComboBox();
            this.TotalPriceLabel = new System.Windows.Forms.Label();
            this.TirageTextBox = new System.Windows.Forms.TextBox();
            this.TirageLabel = new System.Windows.Forms.Label();
            this.PrinteryLabel = new System.Windows.Forms.Label();
            this.OrderButton = new System.Windows.Forms.Button();
            this.PageNumTextBox = new System.Windows.Forms.TextBox();
            this.NameProductTextBox = new System.Windows.Forms.TextBox();
            this.PageCountLabel = new System.Windows.Forms.Label();
            this.ProductNameLabel = new System.Windows.Forms.Label();
            this.TypeComboBox = new System.Windows.Forms.ComboBox();
            this.TypeLabel = new System.Windows.Forms.Label();
            this.CalculateButton = new System.Windows.Forms.Button();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OrdersMenuItem,
            this.PersonalDataMenuItem,
            this.OrganizationMenuItem,
            this.ExitMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(800, 24);
            this.MainMenu.TabIndex = 1;
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
            // AddMenuItem
            // 
            this.AddMenuItem.Name = "AddMenuItem";
            this.AddMenuItem.Size = new System.Drawing.Size(126, 22);
            this.AddMenuItem.Text = "Додати";
            this.AddMenuItem.Click += new System.EventHandler(this.AddMenuItem_Click);
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
            this.PersonalDataMenuItem.Click += new System.EventHandler(this.PersonalDataMenuItem_Click_1);
            // 
            // OrganizationMenuItem
            // 
            this.OrganizationMenuItem.Name = "OrganizationMenuItem";
            this.OrganizationMenuItem.Size = new System.Drawing.Size(83, 20);
            this.OrganizationMenuItem.Text = "Організація";
            this.OrganizationMenuItem.Click += new System.EventHandler(this.OrganizationMenuItem_Click);
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new System.Drawing.Size(52, 20);
            this.ExitMenuItem.Text = "Вийти";
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // PrinteryComboBox
            // 
            this.PrinteryComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.PrinteryComboBox.FormattingEnabled = true;
            this.PrinteryComboBox.Items.AddRange(new object[] {
            "ПринтЛэнд",
            "Красочная Мастерская",
            "Экспресс Печать",
            "МастерПечать",
            "Радуга Принт"});
            this.PrinteryComboBox.Location = new System.Drawing.Point(267, 274);
            this.PrinteryComboBox.Name = "PrinteryComboBox";
            this.PrinteryComboBox.Size = new System.Drawing.Size(270, 28);
            this.PrinteryComboBox.TabIndex = 34;
            // 
            // TotalPriceLabel
            // 
            this.TotalPriceLabel.AutoSize = true;
            this.TotalPriceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TotalPriceLabel.Location = new System.Drawing.Point(415, 338);
            this.TotalPriceLabel.Name = "TotalPriceLabel";
            this.TotalPriceLabel.Size = new System.Drawing.Size(105, 20);
            this.TotalPriceLabel.TabIndex = 32;
            this.TotalPriceLabel.Text = "Total price:";
            // 
            // TirageTextBox
            // 
            this.TirageTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TirageTextBox.Location = new System.Drawing.Point(267, 222);
            this.TirageTextBox.Name = "TirageTextBox";
            this.TirageTextBox.Size = new System.Drawing.Size(270, 26);
            this.TirageTextBox.TabIndex = 31;
            // 
            // TirageLabel
            // 
            this.TirageLabel.AutoSize = true;
            this.TirageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TirageLabel.Location = new System.Drawing.Point(265, 199);
            this.TirageLabel.Name = "TirageLabel";
            this.TirageLabel.Size = new System.Drawing.Size(60, 20);
            this.TirageLabel.TabIndex = 30;
            this.TirageLabel.Text = "Тираж:";
            // 
            // PrinteryLabel
            // 
            this.PrinteryLabel.AutoSize = true;
            this.PrinteryLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.PrinteryLabel.Location = new System.Drawing.Point(263, 251);
            this.PrinteryLabel.Name = "PrinteryLabel";
            this.PrinteryLabel.Size = new System.Drawing.Size(85, 20);
            this.PrinteryLabel.TabIndex = 29;
            this.PrinteryLabel.Text = "Друкарня:";
            // 
            // OrderButton
            // 
            this.OrderButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OrderButton.Location = new System.Drawing.Point(349, 386);
            this.OrderButton.Name = "OrderButton";
            this.OrderButton.Size = new System.Drawing.Size(129, 30);
            this.OrderButton.TabIndex = 28;
            this.OrderButton.Text = "Замовити";
            this.OrderButton.UseVisualStyleBackColor = true;
            this.OrderButton.Click += new System.EventHandler(this.OrderButton_Click);
            // 
            // PageNumTextBox
            // 
            this.PageNumTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.PageNumTextBox.Location = new System.Drawing.Point(267, 170);
            this.PageNumTextBox.Name = "PageNumTextBox";
            this.PageNumTextBox.Size = new System.Drawing.Size(270, 26);
            this.PageNumTextBox.TabIndex = 27;
            // 
            // NameProductTextBox
            // 
            this.NameProductTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.NameProductTextBox.Location = new System.Drawing.Point(267, 118);
            this.NameProductTextBox.Name = "NameProductTextBox";
            this.NameProductTextBox.Size = new System.Drawing.Size(270, 26);
            this.NameProductTextBox.TabIndex = 26;
            // 
            // PageCountLabel
            // 
            this.PageCountLabel.AutoSize = true;
            this.PageCountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.PageCountLabel.Location = new System.Drawing.Point(265, 147);
            this.PageCountLabel.Name = "PageCountLabel";
            this.PageCountLabel.Size = new System.Drawing.Size(150, 20);
            this.PageCountLabel.TabIndex = 25;
            this.PageCountLabel.Text = "Кількість сторінок:";
            // 
            // ProductNameLabel
            // 
            this.ProductNameLabel.AutoSize = true;
            this.ProductNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ProductNameLabel.Location = new System.Drawing.Point(265, 95);
            this.ProductNameLabel.Name = "ProductNameLabel";
            this.ProductNameLabel.Size = new System.Drawing.Size(133, 20);
            this.ProductNameLabel.TabIndex = 24;
            this.ProductNameLabel.Text = "Назва продукту:";
            // 
            // TypeComboBox
            // 
            this.TypeComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TypeComboBox.FormattingEnabled = true;
            this.TypeComboBox.Items.AddRange(new object[] {
            "книга",
            "брошура",
            "буклет",
            "бюлетень для голосування"});
            this.TypeComboBox.Location = new System.Drawing.Point(267, 64);
            this.TypeComboBox.Name = "TypeComboBox";
            this.TypeComboBox.Size = new System.Drawing.Size(270, 28);
            this.TypeComboBox.TabIndex = 36;
            // 
            // TypeLabel
            // 
            this.TypeLabel.AutoSize = true;
            this.TypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TypeLabel.Location = new System.Drawing.Point(263, 41);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(113, 20);
            this.TypeLabel.TabIndex = 35;
            this.TypeLabel.Text = "Тип продукту:";
            // 
            // CalculateButton
            // 
            this.CalculateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CalculateButton.Location = new System.Drawing.Point(280, 333);
            this.CalculateButton.Name = "CalculateButton";
            this.CalculateButton.Size = new System.Drawing.Size(129, 30);
            this.CalculateButton.TabIndex = 37;
            this.CalculateButton.Text = "Розрахувати";
            this.CalculateButton.UseVisualStyleBackColor = true;
            this.CalculateButton.Click += new System.EventHandler(this.CalculateButton_Click);
            // 
            // AddOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CalculateButton);
            this.Controls.Add(this.TypeComboBox);
            this.Controls.Add(this.TypeLabel);
            this.Controls.Add(this.PrinteryComboBox);
            this.Controls.Add(this.TotalPriceLabel);
            this.Controls.Add(this.TirageTextBox);
            this.Controls.Add(this.TirageLabel);
            this.Controls.Add(this.PrinteryLabel);
            this.Controls.Add(this.OrderButton);
            this.Controls.Add(this.PageNumTextBox);
            this.Controls.Add(this.NameProductTextBox);
            this.Controls.Add(this.PageCountLabel);
            this.Controls.Add(this.ProductNameLabel);
            this.Controls.Add(this.MainMenu);
            this.Name = "AddOrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddOrderForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddOrderForm_FormClosing);
            this.Load += new System.EventHandler(this.AddOrderForm_Load);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem OrdersMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ListMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AddMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PersonalDataMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
        private System.Windows.Forms.ComboBox PrinteryComboBox;
        private System.Windows.Forms.Label TotalPriceLabel;
        private System.Windows.Forms.TextBox TirageTextBox;
        private System.Windows.Forms.Label TirageLabel;
        private System.Windows.Forms.Label PrinteryLabel;
        private System.Windows.Forms.Button OrderButton;
        private System.Windows.Forms.TextBox PageNumTextBox;
        private System.Windows.Forms.TextBox NameProductTextBox;
        private System.Windows.Forms.Label PageCountLabel;
        private System.Windows.Forms.Label ProductNameLabel;
        private System.Windows.Forms.ComboBox TypeComboBox;
        private System.Windows.Forms.Label TypeLabel;
        private System.Windows.Forms.ToolStripMenuItem OrganizationMenuItem;
        private System.Windows.Forms.Button CalculateButton;
    }
}
