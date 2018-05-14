namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dtp = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtp2 = new System.Windows.Forms.DateTimePicker();
            this.dtpTime2 = new System.Windows.Forms.DateTimePicker();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.btnDate1 = new System.Windows.Forms.Button();
            this.btnYear = new System.Windows.Forms.Button();
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.timer4 = new System.Windows.Forms.Timer(this.components);
            this.label13 = new System.Windows.Forms.Label();
            this.timer5 = new System.Windows.Forms.Timer(this.components);
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(212, 196);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "InsertDB";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
            // 
            // dtp
            // 
            this.dtp.Location = new System.Drawing.Point(99, 40);
            this.dtp.Name = "dtp";
            this.dtp.Size = new System.Drawing.Size(200, 22);
            this.dtp.TabIndex = 3;
            this.dtp.Value = new System.DateTime(2018, 3, 21, 15, 29, 35, 0);
            this.dtp.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(12, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "開始時間";
            // 
            // dtpTime
            // 
            this.dtpTime.Location = new System.Drawing.Point(314, 40);
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.Size = new System.Drawing.Size(200, 22);
            this.dtpTime.TabIndex = 5;
            this.dtpTime.Value = new System.DateTime(2018, 3, 21, 0, 0, 0, 0);
            this.dtpTime.ValueChanged += new System.EventHandler(this.dtpTime_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(13, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "結束時間";
            // 
            // dtp2
            // 
            this.dtp2.Location = new System.Drawing.Point(99, 71);
            this.dtp2.Name = "dtp2";
            this.dtp2.Size = new System.Drawing.Size(200, 22);
            this.dtp2.TabIndex = 7;
            this.dtp2.ValueChanged += new System.EventHandler(this.dtp2_ValueChanged);
            // 
            // dtpTime2
            // 
            this.dtpTime2.Location = new System.Drawing.Point(314, 71);
            this.dtpTime2.Name = "dtpTime2";
            this.dtpTime2.Size = new System.Drawing.Size(200, 22);
            this.dtpTime2.TabIndex = 8;
            this.dtpTime2.ValueChanged += new System.EventHandler(this.dtpTime2_ValueChanged_1);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button2.Location = new System.Drawing.Point(520, 42);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(129, 51);
            this.button2.TabIndex = 9;
            this.button2.Text = "設定需量反應";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "start";
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(277, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "end";
            this.label4.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(489, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "now";
            this.label5.Visible = false;
            // 
            // button3
            // 
            this.button3.AutoSize = true;
            this.button3.Location = new System.Drawing.Point(305, 224);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(107, 23);
            this.button3.TabIndex = 13;
            this.button3.Text = "Adapter_DataTable";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(305, 264);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(107, 23);
            this.button4.TabIndex = 15;
            this.button4.Text = "資料筆數";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(435, 215);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(79, 72);
            this.button5.TabIndex = 17;
            this.button5.Text = "form2";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Visible = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(210, 159);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 12);
            this.label8.TabIndex = 18;
            this.label8.Text = "Insert功能";
            this.label8.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(303, 159);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 12);
            this.label9.TabIndex = 19;
            this.label9.Text = "select功能";
            this.label9.Visible = false;
            // 
            // button6
            // 
            this.button6.AutoSize = true;
            this.button6.Location = new System.Drawing.Point(305, 196);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(107, 23);
            this.button6.TabIndex = 20;
            this.button6.Text = "Json_sqlReader";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Visible = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("標楷體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(15, 196);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(199, 19);
            this.label6.TabIndex = 21;
            this.label6.Text = "目　前　電　價　： ";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label12.Location = new System.Drawing.Point(212, 234);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(61, 19);
            this.label12.TabIndex = 30;
            this.label12.Text = "Update";
            this.label12.Visible = false;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(214, 264);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 23);
            this.button9.TabIndex = 31;
            this.button9.Text = "Update";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Visible = false;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // btnDate1
            // 
            this.btnDate1.Font = new System.Drawing.Font("標楷體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnDate1.Location = new System.Drawing.Point(520, 202);
            this.btnDate1.Name = "btnDate1";
            this.btnDate1.Size = new System.Drawing.Size(129, 32);
            this.btnDate1.TabIndex = 33;
            this.btnDate1.Text = "Clear";
            this.btnDate1.UseVisualStyleBackColor = true;
            this.btnDate1.Click += new System.EventHandler(this.btnDate1_Click);
            // 
            // btnYear
            // 
            this.btnYear.Location = new System.Drawing.Point(574, 4);
            this.btnYear.Name = "btnYear";
            this.btnYear.Size = new System.Drawing.Size(75, 23);
            this.btnYear.TabIndex = 36;
            this.btnYear.Text = "Year";
            this.btnYear.UseVisualStyleBackColor = true;
            this.btnYear.Visible = false;
            this.btnYear.Click += new System.EventHandler(this.btnYear_Click);
            // 
            // timer3
            // 
            this.timer3.Enabled = true;
            this.timer3.Interval = 60000;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // timer4
            // 
            this.timer4.Interval = 3600000;
            this.timer4.Tick += new System.EventHandler(this.timer4_Tick);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("標楷體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label13.Location = new System.Drawing.Point(516, 114);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(39, 19);
            this.label13.TabIndex = 37;
            this.label13.Text = "   ";
            // 
            // timer5
            // 
            this.timer5.Enabled = true;
            this.timer5.Interval = 10000;
            this.timer5.Tick += new System.EventHandler(this.timer5_Tick);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("標楷體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label14.Location = new System.Drawing.Point(15, 126);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(199, 19);
            this.label14.TabIndex = 38;
            this.label14.Text = "展示設備 節電量 ： ";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("標楷體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label15.Location = new System.Drawing.Point(15, 160);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(199, 19);
            this.label15.TabIndex = 39;
            this.label15.Text = "模擬家電 節電量 ： ";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("標楷體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label16.Location = new System.Drawing.Point(95, 9);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(93, 19);
            this.label16.TabIndex = 40;
            this.label16.Text = "現在時間";
            // 
            // Form1
            // 
            this.AccessibleName = "";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 256);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.btnYear);
            this.Controls.Add(this.btnDate1);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dtpTime2);
            this.Controls.Add(this.dtp2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtpTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtp);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DateTimePicker dtp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtp2;
        private System.Windows.Forms.DateTimePicker dtpTime2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button btnDate1;
        private System.Windows.Forms.Button btnYear;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Timer timer4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Timer timer5;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
    }
}

