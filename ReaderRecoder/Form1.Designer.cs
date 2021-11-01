namespace ReaderRecorder
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.TagList = new System.Windows.Forms.ListView();
            this.TID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EPC = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RSSI = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CH = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PH = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ButtonConnect = new System.Windows.Forms.Button();
            this.ButtonSettings = new System.Windows.Forms.Button();
            this.ButtonClear = new System.Windows.Forms.Button();
            this.ButtonStart = new System.Windows.Forms.Button();
            this.btnBeep = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TagList
            // 
            this.TagList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TagList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TID,
            this.EPC,
            this.RSSI,
            this.CH,
            this.PH,
            this.TS});
            this.TagList.GridLines = true;
            this.TagList.HideSelection = false;
            this.TagList.HoverSelection = true;
            this.TagList.LabelEdit = true;
            this.TagList.Location = new System.Drawing.Point(12, 12);
            this.TagList.Name = "TagList";
            this.TagList.Size = new System.Drawing.Size(1023, 492);
            this.TagList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.TagList.TabIndex = 0;
            this.TagList.UseCompatibleStateImageBehavior = false;
            this.TagList.View = System.Windows.Forms.View.Details;
            this.TagList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TagList_MouseDown);
            // 
            // TID
            // 
            this.TID.Text = "TID";
            this.TID.Width = 200;
            // 
            // EPC
            // 
            this.EPC.Text = "EPC";
            this.EPC.Width = 200;
            // 
            // RSSI
            // 
            this.RSSI.Text = "RSSI";
            // 
            // CH
            // 
            this.CH.Text = "CH";
            // 
            // PH
            // 
            this.PH.Text = "PH";
            this.PH.Width = 120;
            // 
            // TS
            // 
            this.TS.Text = "TS";
            this.TS.Width = 120;
            // 
            // ButtonConnect
            // 
            this.ButtonConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonConnect.Location = new System.Drawing.Point(1052, 12);
            this.ButtonConnect.Name = "ButtonConnect";
            this.ButtonConnect.Size = new System.Drawing.Size(100, 50);
            this.ButtonConnect.TabIndex = 1;
            this.ButtonConnect.Text = "Connect";
            this.ButtonConnect.UseVisualStyleBackColor = true;
            this.ButtonConnect.Click += new System.EventHandler(this.ButtonConnect_Click);
            // 
            // ButtonSettings
            // 
            this.ButtonSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonSettings.Location = new System.Drawing.Point(1052, 180);
            this.ButtonSettings.Name = "ButtonSettings";
            this.ButtonSettings.Size = new System.Drawing.Size(100, 50);
            this.ButtonSettings.TabIndex = 2;
            this.ButtonSettings.Text = "Path";
            this.ButtonSettings.UseVisualStyleBackColor = true;
            this.ButtonSettings.Click += new System.EventHandler(this.ButtonSettings_Click);
            // 
            // ButtonClear
            // 
            this.ButtonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonClear.Location = new System.Drawing.Point(1052, 68);
            this.ButtonClear.Name = "ButtonClear";
            this.ButtonClear.Size = new System.Drawing.Size(100, 50);
            this.ButtonClear.TabIndex = 3;
            this.ButtonClear.Text = "Clear";
            this.ButtonClear.UseVisualStyleBackColor = true;
            this.ButtonClear.Click += new System.EventHandler(this.ButtonClear_Click);
            // 
            // ButtonStart
            // 
            this.ButtonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonStart.Location = new System.Drawing.Point(1052, 124);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(100, 50);
            this.ButtonStart.TabIndex = 4;
            this.ButtonStart.Text = "Start";
            this.ButtonStart.UseVisualStyleBackColor = true;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // btnBeep
            // 
            this.btnBeep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBeep.Location = new System.Drawing.Point(1052, 236);
            this.btnBeep.Name = "btnBeep";
            this.btnBeep.Size = new System.Drawing.Size(100, 50);
            this.btnBeep.TabIndex = 5;
            this.btnBeep.Tag = "1";
            this.btnBeep.Text = "Beep On";
            this.btnBeep.UseVisualStyleBackColor = true;
            this.btnBeep.Click += new System.EventHandler(this.btnBeep_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1164, 516);
            this.Controls.Add(this.btnBeep);
            this.Controls.Add(this.ButtonStart);
            this.Controls.Add(this.ButtonClear);
            this.Controls.Add(this.ButtonSettings);
            this.Controls.Add(this.ButtonConnect);
            this.Controls.Add(this.TagList);
            this.Name = "Form1";
            this.Text = "ReaderRecorder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView TagList;
        private System.Windows.Forms.Button ButtonConnect;
        private System.Windows.Forms.Button ButtonSettings;
        private System.Windows.Forms.ColumnHeader TID;
        private System.Windows.Forms.ColumnHeader EPC;
        private System.Windows.Forms.ColumnHeader RSSI;
        private System.Windows.Forms.ColumnHeader CH;
        private System.Windows.Forms.Button ButtonClear;
        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.ColumnHeader TS;
        private System.Windows.Forms.ColumnHeader PH;
        private System.Windows.Forms.Button btnBeep;
    }
}

