namespace Publishing
{
    partial class addOrderForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.головнаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.списокToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.додатиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.видалитиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.змінитиДаніToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.організаціяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.вийтиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printeryBox = new System.Windows.Forms.ComboBox();
            this.totalPriceLabel = new System.Windows.Forms.Label();
            this.tirageTextBox = new System.Windows.Forms.TextBox();
            this.emailLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.orderButton = new System.Windows.Forms.Button();
            this.pageNumTextBox = new System.Windows.Forms.TextBox();
            this.nameProductTextBox = new System.Windows.Forms.TextBox();
            this.LNameLabel = new System.Windows.Forms.Label();
            this.FNameLabel = new System.Windows.Forms.Label();
            this.typeBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.calculateButton = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.головнаToolStripMenuItem,
            this.змінитиДаніToolStripMenuItem,
            this.організаціяToolStripMenuItem,
            this.вийтиToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // головнаToolStripMenuItem
            // 
            this.головнаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.списокToolStripMenuItem,
            this.додатиToolStripMenuItem,
            this.видалитиToolStripMenuItem});
            this.головнаToolStripMenuItem.Name = "головнаToolStripMenuItem";
            this.головнаToolStripMenuItem.Size = new System.Drawing.Size(87, 20);
            this.головнаToolStripMenuItem.Text = "Замовлення";
            // 
            // списокToolStripMenuItem
            // 
            this.списокToolStripMenuItem.Name = "списокToolStripMenuItem";
            this.списокToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.списокToolStripMenuItem.Text = "Список";
            this.списокToolStripMenuItem.Click += new System.EventHandler(this.списокToolStripMenuItem_Click);
            // 
            // додатиToolStripMenuItem
            // 
            this.додатиToolStripMenuItem.Name = "додатиToolStripMenuItem";
            this.додатиToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.додатиToolStripMenuItem.Text = "Додати";
            this.додатиToolStripMenuItem.Click += new System.EventHandler(this.додатиToolStripMenuItem_Click);
            // 
            // видалитиToolStripMenuItem
            // 
            this.видалитиToolStripMenuItem.Name = "видалитиToolStripMenuItem";
            this.видалитиToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.видалитиToolStripMenuItem.Text = "Видалити";
            this.видалитиToolStripMenuItem.Click += new System.EventHandler(this.видалитиToolStripMenuItem_Click);
            // 
            // змінитиДаніToolStripMenuItem
            // 
            this.змінитиДаніToolStripMenuItem.Name = "змінитиДаніToolStripMenuItem";
            this.змінитиДаніToolStripMenuItem.Size = new System.Drawing.Size(115, 20);
            this.змінитиДаніToolStripMenuItem.Text = "Персональні дані";
            this.змінитиДаніToolStripMenuItem.Click += new System.EventHandler(this.змінитиДаніToolStripMenuItem_Click_1);
            // 
            // організаціяToolStripMenuItem
            // 
            this.організаціяToolStripMenuItem.Name = "організаціяToolStripMenuItem";
            this.організаціяToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.організаціяToolStripMenuItem.Text = "Організація";
            this.організаціяToolStripMenuItem.Click += new System.EventHandler(this.організаціяToolStripMenuItem_Click);
            // 
            // вийтиToolStripMenuItem
            // 
            this.вийтиToolStripMenuItem.Name = "вийтиToolStripMenuItem";
            this.вийтиToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.вийтиToolStripMenuItem.Text = "Вийти";
            this.вийтиToolStripMenuItem.Click += new System.EventHandler(this.вийтиToolStripMenuItem_Click);
            // 
            // printeryBox
            // 
            this.printeryBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.printeryBox.FormattingEnabled = true;
            this.printeryBox.Items.AddRange(new object[] {
            "ПринтЛэнд",
            "Красочная Мастерская",
            "Экспресс Печать",
            "МастерПечать",
            "Радуга Принт"});
            this.printeryBox.Location = new System.Drawing.Point(267, 274);
            this.printeryBox.Name = "printeryBox";
            this.printeryBox.Size = new System.Drawing.Size(270, 28);
            this.printeryBox.TabIndex = 34;
            // 
            // totalPriceLabel
            // 
            this.totalPriceLabel.AutoSize = true;
            this.totalPriceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.totalPriceLabel.Location = new System.Drawing.Point(415, 338);
            this.totalPriceLabel.Name = "totalPriceLabel";
            this.totalPriceLabel.Size = new System.Drawing.Size(105, 20);
            this.totalPriceLabel.TabIndex = 32;
            this.totalPriceLabel.Text = "Кінцева ціна:";
            // 
            // tirageTextBox
            // 
            this.tirageTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tirageTextBox.Location = new System.Drawing.Point(267, 222);
            this.tirageTextBox.Name = "tirageTextBox";
            this.tirageTextBox.Size = new System.Drawing.Size(270, 26);
            this.tirageTextBox.TabIndex = 31;
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.emailLabel.Location = new System.Drawing.Point(265, 199);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(60, 20);
            this.emailLabel.TabIndex = 30;
            this.emailLabel.Text = "Тираж:";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.statusLabel.Location = new System.Drawing.Point(263, 251);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(85, 20);
            this.statusLabel.TabIndex = 29;
            this.statusLabel.Text = "Друкарня:";
            // 
            // orderButton
            // 
            this.orderButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.orderButton.Location = new System.Drawing.Point(349, 386);
            this.orderButton.Name = "orderButton";
            this.orderButton.Size = new System.Drawing.Size(129, 30);
            this.orderButton.TabIndex = 28;
            this.orderButton.Text = "Замовити";
            this.orderButton.UseVisualStyleBackColor = true;
            this.orderButton.Click += new System.EventHandler(this.orderButton_Click);
            // 
            // pageNumTextBox
            // 
            this.pageNumTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pageNumTextBox.Location = new System.Drawing.Point(267, 170);
            this.pageNumTextBox.Name = "pageNumTextBox";
            this.pageNumTextBox.Size = new System.Drawing.Size(270, 26);
            this.pageNumTextBox.TabIndex = 27;
            // 
            // nameProductTextBox
            // 
            this.nameProductTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nameProductTextBox.Location = new System.Drawing.Point(267, 118);
            this.nameProductTextBox.Name = "nameProductTextBox";
            this.nameProductTextBox.Size = new System.Drawing.Size(270, 26);
            this.nameProductTextBox.TabIndex = 26;
            // 
            // LNameLabel
            // 
            this.LNameLabel.AutoSize = true;
            this.LNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LNameLabel.Location = new System.Drawing.Point(265, 147);
            this.LNameLabel.Name = "LNameLabel";
            this.LNameLabel.Size = new System.Drawing.Size(150, 20);
            this.LNameLabel.TabIndex = 25;
            this.LNameLabel.Text = "Кількість сторінок:";
            // 
            // FNameLabel
            // 
            this.FNameLabel.AutoSize = true;
            this.FNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FNameLabel.Location = new System.Drawing.Point(265, 95);
            this.FNameLabel.Name = "FNameLabel";
            this.FNameLabel.Size = new System.Drawing.Size(133, 20);
            this.FNameLabel.TabIndex = 24;
            this.FNameLabel.Text = "Назва продукту:";
            // 
            // typeBox
            // 
            this.typeBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.typeBox.FormattingEnabled = true;
            this.typeBox.Items.AddRange(new object[] {
            "книга",
            "брошура",
            "буклет",
            "бюлетень для голосування"});
            this.typeBox.Location = new System.Drawing.Point(267, 64);
            this.typeBox.Name = "typeBox";
            this.typeBox.Size = new System.Drawing.Size(270, 28);
            this.typeBox.TabIndex = 36;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(263, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 20);
            this.label1.TabIndex = 35;
            this.label1.Text = "Тип продукту:";
            // 
            // calculateButton
            // 
            this.calculateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.calculateButton.Location = new System.Drawing.Point(280, 333);
            this.calculateButton.Name = "calculateButton";
            this.calculateButton.Size = new System.Drawing.Size(129, 30);
            this.calculateButton.TabIndex = 37;
            this.calculateButton.Text = "Розрахувати";
            this.calculateButton.UseVisualStyleBackColor = true;
            this.calculateButton.Click += new System.EventHandler(this.calculateButton_Click);
            // 
            // addOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.calculateButton);
            this.Controls.Add(this.typeBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.printeryBox);
            this.Controls.Add(this.totalPriceLabel);
            this.Controls.Add(this.tirageTextBox);
            this.Controls.Add(this.emailLabel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.orderButton);
            this.Controls.Add(this.pageNumTextBox);
            this.Controls.Add(this.nameProductTextBox);
            this.Controls.Add(this.LNameLabel);
            this.Controls.Add(this.FNameLabel);
            this.Controls.Add(this.menuStrip1);
            this.Name = "addOrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "addOrderForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.addOrderForm_FormClosing);
            this.Load += new System.EventHandler(this.addOrderForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem головнаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem списокToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem додатиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem видалитиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem змінитиДаніToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem вийтиToolStripMenuItem;
        private System.Windows.Forms.ComboBox printeryBox;
        private System.Windows.Forms.Label totalPriceLabel;
        private System.Windows.Forms.TextBox tirageTextBox;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button orderButton;
        private System.Windows.Forms.TextBox pageNumTextBox;
        private System.Windows.Forms.TextBox nameProductTextBox;
        private System.Windows.Forms.Label LNameLabel;
        private System.Windows.Forms.Label FNameLabel;
        private System.Windows.Forms.ComboBox typeBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem організаціяToolStripMenuItem;
        private System.Windows.Forms.Button calculateButton;
    }
}