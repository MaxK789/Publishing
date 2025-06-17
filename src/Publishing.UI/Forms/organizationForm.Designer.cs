namespace Publishing
{
    partial class organizationForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.головнаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.списокToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.додатиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.видалитиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.змінитиДаніToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.організаціяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.вийтиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addressTextBox = new System.Windows.Forms.TextBox();
            this.addressLabel = new System.Windows.Forms.Label();
            this.faxTextBox = new System.Windows.Forms.TextBox();
            this.faxLabel = new System.Windows.Forms.Label();
            this.phoneTextBox = new System.Windows.Forms.TextBox();
            this.phoneLabel = new System.Windows.Forms.Label();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.emailLabel = new System.Windows.Forms.Label();
            this.changeButton = new System.Windows.Forms.Button();
            this.orgNameTextBox = new System.Windows.Forms.TextBox();
            this.organizationNameLabel = new System.Windows.Forms.Label();
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
            this.списокToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.списокToolStripMenuItem.Text = "Список";
            this.списокToolStripMenuItem.Click += new System.EventHandler(this.списокToolStripMenuItem_Click);
            // 
            // додатиToolStripMenuItem
            // 
            this.додатиToolStripMenuItem.Name = "додатиToolStripMenuItem";
            this.додатиToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.додатиToolStripMenuItem.Text = "Додати";
            this.додатиToolStripMenuItem.Click += new System.EventHandler(this.додатиToolStripMenuItem_Click);
            // 
            // видалитиToolStripMenuItem
            // 
            this.видалитиToolStripMenuItem.Name = "видалитиToolStripMenuItem";
            this.видалитиToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.видалитиToolStripMenuItem.Text = "Видалити";
            this.видалитиToolStripMenuItem.Click += new System.EventHandler(this.видалитиToolStripMenuItem_Click);
            // 
            // змінитиДаніToolStripMenuItem
            // 
            this.змінитиДаніToolStripMenuItem.Name = "змінитиДаніToolStripMenuItem";
            this.змінитиДаніToolStripMenuItem.Size = new System.Drawing.Size(115, 20);
            this.змінитиДаніToolStripMenuItem.Text = "Персональні дані";
            this.змінитиДаніToolStripMenuItem.Click += new System.EventHandler(this.змінитиДаніToolStripMenuItem_Click);
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
            // addressTextBox
            // 
            this.addressTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.addressTextBox.Location = new System.Drawing.Point(266, 302);
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.Size = new System.Drawing.Size(270, 26);
            this.addressTextBox.TabIndex = 55;
            // 
            // addressLabel
            // 
            this.addressLabel.AutoSize = true;
            this.addressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.addressLabel.Location = new System.Drawing.Point(264, 279);
            this.addressLabel.Name = "addressLabel";
            this.addressLabel.Size = new System.Drawing.Size(70, 20);
            this.addressLabel.TabIndex = 54;
            this.addressLabel.Text = "Адреса:";
            // 
            // faxTextBox
            // 
            this.faxTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.faxTextBox.Location = new System.Drawing.Point(266, 250);
            this.faxTextBox.Name = "faxTextBox";
            this.faxTextBox.Size = new System.Drawing.Size(270, 26);
            this.faxTextBox.TabIndex = 53;
            // 
            // faxLabel
            // 
            this.faxLabel.AutoSize = true;
            this.faxLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.faxLabel.Location = new System.Drawing.Point(264, 227);
            this.faxLabel.Name = "faxLabel";
            this.faxLabel.Size = new System.Drawing.Size(53, 20);
            this.faxLabel.TabIndex = 52;
            this.faxLabel.Text = "Факс:";
            // 
            // phoneTextBox
            // 
            this.phoneTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.phoneTextBox.Location = new System.Drawing.Point(266, 198);
            this.phoneTextBox.Name = "phoneTextBox";
            this.phoneTextBox.Size = new System.Drawing.Size(270, 26);
            this.phoneTextBox.TabIndex = 50;
            // 
            // phoneLabel
            // 
            this.phoneLabel.AutoSize = true;
            this.phoneLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.phoneLabel.Location = new System.Drawing.Point(264, 175);
            this.phoneLabel.Name = "phoneLabel";
            this.phoneLabel.Size = new System.Drawing.Size(83, 20);
            this.phoneLabel.TabIndex = 49;
            this.phoneLabel.Text = "Телефон:";
            // 
            // emailTextBox
            // 
            this.emailTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.emailTextBox.Location = new System.Drawing.Point(266, 146);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(270, 26);
            this.emailTextBox.TabIndex = 48;
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.emailLabel.Location = new System.Drawing.Point(264, 123);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(65, 20);
            this.emailLabel.TabIndex = 47;
            this.emailLabel.Text = "Пошта:";
            // 
            // changeButton
            // 
            this.changeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.changeButton.Location = new System.Drawing.Point(336, 374);
            this.changeButton.Name = "changeButton";
            this.changeButton.Size = new System.Drawing.Size(129, 30);
            this.changeButton.TabIndex = 45;
            this.changeButton.Text = "Змінити";
            this.changeButton.UseVisualStyleBackColor = true;
            this.changeButton.Click += new System.EventHandler(this.changeButton_Click);
            // 
            // orgNameTextBox
            // 
            this.orgNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.orgNameTextBox.Location = new System.Drawing.Point(266, 94);
            this.orgNameTextBox.Name = "orgNameTextBox";
            this.orgNameTextBox.Size = new System.Drawing.Size(270, 26);
            this.orgNameTextBox.TabIndex = 43;
            // 
            // organizationNameLabel
            // 
            this.organizationNameLabel.AutoSize = true;
            this.organizationNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.organizationNameLabel.Location = new System.Drawing.Point(264, 71);
            this.organizationNameLabel.Name = "organizationNameLabel";
            this.organizationNameLabel.Size = new System.Drawing.Size(60, 20);
            this.organizationNameLabel.TabIndex = 41;
            this.organizationNameLabel.Text = "Назва:";
            // 
            // organizationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.addressTextBox);
            this.Controls.Add(this.addressLabel);
            this.Controls.Add(this.faxTextBox);
            this.Controls.Add(this.faxLabel);
            this.Controls.Add(this.phoneTextBox);
            this.Controls.Add(this.phoneLabel);
            this.Controls.Add(this.emailTextBox);
            this.Controls.Add(this.emailLabel);
            this.Controls.Add(this.changeButton);
            this.Controls.Add(this.orgNameTextBox);
            this.Controls.Add(this.organizationNameLabel);
            this.Controls.Add(this.menuStrip1);
            this.Name = "organizationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "organizationForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.organizationForm_FormClosing);
            this.Load += new System.EventHandler(this.organizationForm_Load);
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
        private System.Windows.Forms.ToolStripMenuItem організаціяToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem вийтиToolStripMenuItem;
        private System.Windows.Forms.TextBox addressTextBox;
        private System.Windows.Forms.Label addressLabel;
        private System.Windows.Forms.TextBox faxTextBox;
        private System.Windows.Forms.Label faxLabel;
        private System.Windows.Forms.TextBox phoneTextBox;
        private System.Windows.Forms.Label phoneLabel;
        private System.Windows.Forms.TextBox emailTextBox;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.Button changeButton;
        private System.Windows.Forms.TextBox orgNameTextBox;
        private System.Windows.Forms.Label organizationNameLabel;
    }
}