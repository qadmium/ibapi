namespace IBPlugin
{
    partial class SymbolLookupPage
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SecIdType = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.SecId = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.Strike = new System.Windows.Forms.NumericUpDown();
            this.Call = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.Multiplier = new System.Windows.Forms.NumericUpDown();
            this.LocalSymbol = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.IncludeExpired = new System.Windows.Forms.CheckBox();
            this.Expiry = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Exchange = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Currency = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ContractId = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.Search = new System.Windows.Forms.Button();
            this.InstrumentTypeBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.What = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ContractsView = new System.Windows.Forms.DataGridView();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.ErrorLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Strike)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Multiplier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContractId)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ContractsView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.SecIdType);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.SecId);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.Strike);
            this.groupBox1.Controls.Add(this.Call);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.Multiplier);
            this.groupBox1.Controls.Add(this.LocalSymbol);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.IncludeExpired);
            this.groupBox1.Controls.Add(this.Expiry);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.Exchange);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.Currency);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.ContractId);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.Search);
            this.groupBox1.Controls.Add(this.InstrumentTypeBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.What);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(778, 127);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search";
            // 
            // SecIdType
            // 
            this.SecIdType.Location = new System.Drawing.Point(493, 99);
            this.SecIdType.Name = "SecIdType";
            this.SecIdType.Size = new System.Drawing.Size(100, 20);
            this.SecIdType.TabIndex = 24;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(424, 100);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "SecIdType:";
            // 
            // SecId
            // 
            this.SecId.Location = new System.Drawing.Point(318, 97);
            this.SecId.Name = "SecId";
            this.SecId.Size = new System.Drawing.Size(100, 20);
            this.SecId.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(274, 100);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "SecId:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(108, 101);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Strike:";
            // 
            // Strike
            // 
            this.Strike.DecimalPlaces = 6;
            this.Strike.Increment = new decimal(new int[] {
            1,
            0,
            0,
            393216});
            this.Strike.Location = new System.Drawing.Point(148, 98);
            this.Strike.Maximum = new decimal(new int[] {
            -294967296,
            0,
            0,
            0});
            this.Strike.Name = "Strike";
            this.Strike.Size = new System.Drawing.Size(120, 20);
            this.Strike.TabIndex = 19;
            // 
            // Call
            // 
            this.Call.AutoSize = true;
            this.Call.Location = new System.Drawing.Point(14, 99);
            this.Call.Name = "Call";
            this.Call.Size = new System.Drawing.Size(88, 17);
            this.Call.TabIndex = 18;
            this.Call.Text = "Call (Options)";
            this.Call.ThreeState = true;
            this.Call.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(447, 75);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Multiplier:";
            // 
            // Multiplier
            // 
            this.Multiplier.DecimalPlaces = 6;
            this.Multiplier.Increment = new decimal(new int[] {
            1,
            0,
            0,
            393216});
            this.Multiplier.Location = new System.Drawing.Point(504, 71);
            this.Multiplier.Maximum = new decimal(new int[] {
            -294967296,
            0,
            0,
            0});
            this.Multiplier.Name = "Multiplier";
            this.Multiplier.Size = new System.Drawing.Size(120, 20);
            this.Multiplier.TabIndex = 16;
            // 
            // LocalSymbol
            // 
            this.LocalSymbol.Location = new System.Drawing.Point(341, 72);
            this.LocalSymbol.Name = "LocalSymbol";
            this.LocalSymbol.Size = new System.Drawing.Size(100, 20);
            this.LocalSymbol.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(263, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Local symbol:";
            // 
            // IncludeExpired
            // 
            this.IncludeExpired.AutoSize = true;
            this.IncludeExpired.Location = new System.Drawing.Point(159, 76);
            this.IncludeExpired.Name = "IncludeExpired";
            this.IncludeExpired.Size = new System.Drawing.Size(98, 17);
            this.IncludeExpired.TabIndex = 13;
            this.IncludeExpired.Text = "Include expired";
            this.IncludeExpired.UseVisualStyleBackColor = true;
            // 
            // Expiry
            // 
            this.Expiry.Location = new System.Drawing.Point(53, 73);
            this.Expiry.Name = "Expiry";
            this.Expiry.Size = new System.Drawing.Size(100, 20);
            this.Expiry.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Expiry:";
            // 
            // Exchange
            // 
            this.Exchange.Location = new System.Drawing.Point(430, 44);
            this.Exchange.Name = "Exchange";
            this.Exchange.Size = new System.Drawing.Size(100, 20);
            this.Exchange.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(365, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Exchange:";
            // 
            // Currency
            // 
            this.Currency.Location = new System.Drawing.Point(259, 44);
            this.Currency.Name = "Currency";
            this.Currency.Size = new System.Drawing.Size(100, 20);
            this.Currency.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(200, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Currency:";
            // 
            // ContractId
            // 
            this.ContractId.Location = new System.Drawing.Point(73, 45);
            this.ContractId.Maximum = new decimal(new int[] {
            -294967296,
            0,
            0,
            0});
            this.ContractId.Name = "ContractId";
            this.ContractId.Size = new System.Drawing.Size(120, 20);
            this.ContractId.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "ContractId:";
            // 
            // Search
            // 
            this.Search.Dock = System.Windows.Forms.DockStyle.Right;
            this.Search.Location = new System.Drawing.Point(666, 16);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(109, 108);
            this.Search.TabIndex = 3;
            this.Search.Text = "Search";
            this.Search.UseVisualStyleBackColor = true;
            this.Search.Click += new System.EventHandler(this.Search_Click);
            // 
            // InstrumentTypeBox
            // 
            this.InstrumentTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InstrumentTypeBox.FormattingEnabled = true;
            this.InstrumentTypeBox.Items.AddRange(new object[] {
            "Any instrument"});
            this.InstrumentTypeBox.Location = new System.Drawing.Point(191, 18);
            this.InstrumentTypeBox.Name = "InstrumentTypeBox";
            this.InstrumentTypeBox.Size = new System.Drawing.Size(100, 21);
            this.InstrumentTypeBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Symbol:";
            // 
            // What
            // 
            this.What.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.What.Location = new System.Drawing.Point(57, 18);
            this.What.Name = "What";
            this.What.Size = new System.Drawing.Size(128, 20);
            this.What.TabIndex = 0;
            this.What.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.What_KeyPress);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.ContractsView);
            this.groupBox2.Location = new System.Drawing.Point(0, 127);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox2.Size = new System.Drawing.Size(778, 457);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Results";
            // 
            // ContractsView
            // 
            this.ContractsView.AllowUserToAddRows = false;
            this.ContractsView.AllowUserToDeleteRows = false;
            this.ContractsView.AllowUserToOrderColumns = true;
            this.ContractsView.AllowUserToResizeRows = false;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Info;
            this.ContractsView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
            this.ContractsView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.ContractsView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ContractsView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ContractsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ContractsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContractsView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.ContractsView.Location = new System.Drawing.Point(5, 18);
            this.ContractsView.MultiSelect = false;
            this.ContractsView.Name = "ContractsView";
            this.ContractsView.ReadOnly = true;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ContractsView.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.ContractsView.RowHeadersVisible = false;
            this.ContractsView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ContractsView.ShowCellErrors = false;
            this.ContractsView.ShowCellToolTips = false;
            this.ContractsView.ShowEditingIcon = false;
            this.ContractsView.ShowRowErrors = false;
            this.ContractsView.Size = new System.Drawing.Size(768, 434);
            this.ContractsView.TabIndex = 4;
            // 
            // ProgressBar
            // 
            this.ProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBar.Location = new System.Drawing.Point(0, 584);
            this.ProgressBar.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.ProgressBar.MarqueeAnimationSpeed = 200;
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(778, 23);
            this.ProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.ProgressBar.TabIndex = 2;
            this.ProgressBar.Visible = false;
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ErrorLabel.Location = new System.Drawing.Point(0, 608);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(778, 22);
            this.ErrorLabel.TabIndex = 3;
            // 
            // SymbolLookupPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 639);
            this.Controls.Add(this.ErrorLabel);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "SymbolLookupPage";
            this.Text = "Select IB symbol";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SymbolLookupPage_FormClosed);
            this.Shown += new System.EventHandler(this.SymbolLookupPage_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Strike)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Multiplier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContractId)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ContractsView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox What;
        private System.Windows.Forms.ComboBox InstrumentTypeBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView ContractsView;
        private System.Windows.Forms.Button Search;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.Label ErrorLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ContractId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Currency;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Exchange;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Expiry;
        private System.Windows.Forms.CheckBox IncludeExpired;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox LocalSymbol;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown Multiplier;
        private System.Windows.Forms.CheckBox Call;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown Strike;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox SecId;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox SecIdType;

    }
}