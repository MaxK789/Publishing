namespace Publishing
{
    partial class StatisticForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.OrdersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ListMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatisticsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OrdersChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.OrderCountPerMonthButton = new System.Windows.Forms.Button();
            this.OrdersPerMonthLabel = new System.Windows.Forms.Label();
            this.OrdersPerAuthorLabel = new System.Windows.Forms.Label();
            this.OrderCountPerAuthorButton = new System.Windows.Forms.Button();
            this.TirageLabel = new System.Windows.Forms.Label();
            this.TirageButton = new System.Windows.Forms.Button();
            this.DateRangeLabel = new System.Windows.Forms.Label();
            this.FromDateToDateButton = new System.Windows.Forms.Button();
            this.AuthorsLabel = new System.Windows.Forms.Label();
            this.FromDatePicker = new System.Windows.Forms.DateTimePicker();
            this.ToDatePicker = new System.Windows.Forms.DateTimePicker();
            this.AuthorsBox = new System.Windows.Forms.ComboBox();
            this.MainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OrdersChart)).BeginInit();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OrdersMenuItem,
            this.StatisticsMenuItem,
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
            this.DeleteMenuItem});
            this.OrdersMenuItem.Name = "OrdersMenuItem";
            this.OrdersMenuItem.Size = new System.Drawing.Size(87, 20);
            this.OrdersMenuItem.Text = "Замовлення";
            // 
            // ListMenuItem
            // 
            this.ListMenuItem.Name = "ListMenuItem";
            this.ListMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ListMenuItem.Text = "Список";
            this.ListMenuItem.Click += new System.EventHandler(this.ListMenuItem_Click);
            // 
            // DeleteMenuItem
            // 
            this.DeleteMenuItem.Name = "DeleteMenuItem";
            this.DeleteMenuItem.Size = new System.Drawing.Size(180, 22);
            this.DeleteMenuItem.Text = "Видалити";
            this.DeleteMenuItem.Click += new System.EventHandler(this.DeleteMenuItem_Click_1);
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
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click_1);
            // 
            // OrdersChart
            // 
            chartArea3.Name = "ChartArea1";
            this.OrdersChart.ChartAreas.Add(chartArea3);
            this.OrdersChart.Location = new System.Drawing.Point(12, 129);
            this.OrdersChart.Name = "OrdersChart";
            series3.ChartArea = "ChartArea1";
            series3.Name = "Series1";
            this.OrdersChart.Series.Add(series3);
            this.OrdersChart.Size = new System.Drawing.Size(776, 309);
            this.OrdersChart.TabIndex = 2;
            this.OrdersChart.Text = "OrdersChart";
            // 
            // OrderCountPerMonthButton
            // 
            this.OrderCountPerMonthButton.Location = new System.Drawing.Point(296, 37);
            this.OrderCountPerMonthButton.Name = "OrderCountPerMonthButton";
            this.OrderCountPerMonthButton.Size = new System.Drawing.Size(75, 23);
            this.OrderCountPerMonthButton.TabIndex = 3;
            this.OrderCountPerMonthButton.Text = "Показати";
            this.OrderCountPerMonthButton.UseVisualStyleBackColor = true;
            this.OrderCountPerMonthButton.Click += new System.EventHandler(this.OrderCountPerMonthButton_Click);
            // 
            // OrdersPerMonthLabel
            // 
            this.OrdersPerMonthLabel.AutoSize = true;
            this.OrdersPerMonthLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OrdersPerMonthLabel.Location = new System.Drawing.Point(8, 37);
            this.OrdersPerMonthLabel.Name = "OrdersPerMonthLabel";
            this.OrdersPerMonthLabel.Size = new System.Drawing.Size(278, 20);
            this.OrdersPerMonthLabel.TabIndex = 4;
            this.OrdersPerMonthLabel.Text = "Замовлення цього року по місяцям:";
            // 
            // OrdersPerAuthorLabel
            // 
            this.OrdersPerAuthorLabel.AutoSize = true;
            this.OrdersPerAuthorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OrdersPerAuthorLabel.Location = new System.Drawing.Point(11, 78);
            this.OrdersPerAuthorLabel.Name = "OrdersPerAuthorLabel";
            this.OrdersPerAuthorLabel.Size = new System.Drawing.Size(198, 20);
            this.OrdersPerAuthorLabel.TabIndex = 5;
            this.OrdersPerAuthorLabel.Text = "Замовлення за автором:";
            // 
            // OrderCountPerAuthorButton
            // 
            this.OrderCountPerAuthorButton.Location = new System.Drawing.Point(215, 78);
            this.OrderCountPerAuthorButton.Name = "OrderCountPerAuthorButton";
            this.OrderCountPerAuthorButton.Size = new System.Drawing.Size(75, 23);
            this.OrderCountPerAuthorButton.TabIndex = 6;
            this.OrderCountPerAuthorButton.Text = "Показати";
            this.OrderCountPerAuthorButton.UseVisualStyleBackColor = true;
            this.OrderCountPerAuthorButton.Click += new System.EventHandler(this.OrderCountPerAuthorButton_Click);
            // 
            // TirageLabel
            // 
            this.TirageLabel.AutoSize = true;
            this.TirageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TirageLabel.Location = new System.Drawing.Point(450, 78);
            this.TirageLabel.Name = "TirageLabel";
            this.TirageLabel.Size = new System.Drawing.Size(228, 20);
            this.TirageLabel.TabIndex = 7;
            this.TirageLabel.Text = "Найбільш видаваємі автори:";
            // 
            // TirageButton
            // 
            this.TirageButton.Location = new System.Drawing.Point(684, 78);
            this.TirageButton.Name = "TirageButton";
            this.TirageButton.Size = new System.Drawing.Size(75, 23);
            this.TirageButton.TabIndex = 8;
            this.TirageButton.Text = "Показати";
            this.TirageButton.UseVisualStyleBackColor = true;
            this.TirageButton.Click += new System.EventHandler(this.TirageButton_Click);
            // 
            // DateRangeLabel
            // 
            this.DateRangeLabel.AutoSize = true;
            this.DateRangeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DateRangeLabel.Location = new System.Drawing.Point(377, 37);
            this.DateRangeLabel.Name = "DateRangeLabel";
            this.DateRangeLabel.Size = new System.Drawing.Size(116, 20);
            this.DateRangeLabel.TabIndex = 10;
            this.DateRangeLabel.Text = "Замовлення з";
            // 
            // FromDateToDateButton
            // 
            this.FromDateToDateButton.Location = new System.Drawing.Point(713, 36);
            this.FromDateToDateButton.Name = "FromDateToDateButton";
            this.FromDateToDateButton.Size = new System.Drawing.Size(75, 23);
            this.FromDateToDateButton.TabIndex = 9;
            this.FromDateToDateButton.Text = "Показати";
            this.FromDateToDateButton.UseVisualStyleBackColor = true;
            this.FromDateToDateButton.Click += new System.EventHandler(this.FromDateToDateButton_Click);
            // 
            // AuthorsLabel
            // 
            this.AuthorsLabel.AutoSize = true;
            this.AuthorsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AuthorsLabel.Location = new System.Drawing.Point(589, 37);
            this.AuthorsLabel.Name = "AuthorsLabel";
            this.AuthorsLabel.Size = new System.Drawing.Size(27, 20);
            this.AuthorsLabel.TabIndex = 11;
            this.AuthorsLabel.Text = "по";
            // 
            // FromDatePicker
            // 
            this.FromDatePicker.Location = new System.Drawing.Point(499, 36);
            this.FromDatePicker.Name = "FromDatePicker";
            this.FromDatePicker.Size = new System.Drawing.Size(84, 20);
            this.FromDatePicker.TabIndex = 12;
            // 
            // ToDatePicker
            // 
            this.ToDatePicker.Location = new System.Drawing.Point(623, 36);
            this.ToDatePicker.Name = "ToDatePicker";
            this.ToDatePicker.Size = new System.Drawing.Size(84, 20);
            this.ToDatePicker.TabIndex = 13;
            // 
            // AuthorsBox
            // 
            this.AuthorsBox.FormattingEnabled = true;
            this.AuthorsBox.Location = new System.Drawing.Point(296, 80);
            this.AuthorsBox.Name = "AuthorsBox";
            this.AuthorsBox.Size = new System.Drawing.Size(121, 21);
            this.AuthorsBox.TabIndex = 14;
            // 
            // StatisticForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.AuthorsBox);
            this.Controls.Add(this.ToDatePicker);
            this.Controls.Add(this.FromDatePicker);
            this.Controls.Add(this.AuthorsLabel);
            this.Controls.Add(this.DateRangeLabel);
            this.Controls.Add(this.FromDateToDateButton);
            this.Controls.Add(this.TirageButton);
            this.Controls.Add(this.TirageLabel);
            this.Controls.Add(this.OrderCountPerAuthorButton);
            this.Controls.Add(this.OrdersPerAuthorLabel);
            this.Controls.Add(this.OrdersPerMonthLabel);
            this.Controls.Add(this.OrderCountPerMonthButton);
            this.Controls.Add(this.OrdersChart);
            this.Controls.Add(this.MainMenu);
            this.Name = "StatisticForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StatisticForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StatisticForm_FormClosing);
            this.Load += new System.EventHandler(this.StatisticForm_Load);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OrdersChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem OrdersMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ListMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem StatisticsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
        private System.Windows.Forms.DataVisualization.Charting.Chart OrdersChart;
        private System.Windows.Forms.Button OrderCountPerMonthButton;
        private System.Windows.Forms.Label OrdersPerMonthLabel;
        private System.Windows.Forms.Label OrdersPerAuthorLabel;
        private System.Windows.Forms.Button OrderCountPerAuthorButton;
        private System.Windows.Forms.Label TirageLabel;
        private System.Windows.Forms.Button TirageButton;
        private System.Windows.Forms.Label DateRangeLabel;
        private System.Windows.Forms.Button FromDateToDateButton;
        private System.Windows.Forms.Label AuthorsLabel;
        private System.Windows.Forms.DateTimePicker FromDatePicker;
        private System.Windows.Forms.DateTimePicker ToDatePicker;
        private System.Windows.Forms.ComboBox AuthorsBox;
    }
}
