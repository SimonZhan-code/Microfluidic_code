using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Impinj.OctaneSdk;
using System.IO;

namespace ReaderMonitor
{
    public partial class Form1 : Form
    {
        const string FILE_NAME = "TID.txt";//TID文件名
        const string READER_HOSTNAME = "impinj-14-04-15";//reader名
        const int MODE_NUM = 4;//类型数量
        const int CHIP_NUM = 3;//芯片数量
        const int COUNT_NUM = 8;//状态数量2^3

        ImpinjReader reader = new ImpinjReader();
        delegate void UpdateCallback();//跨线程委托
        List<Tag> tags = new List<Tag>();//扫描到的标签
        List<string> tid = new List<string>();//目标标签
        bool[] chip = new bool[MODE_NUM * CHIP_NUM];//芯片标记
        int count = 0;//芯片状态编码
        StreamReader sr;//读文件
        Thread showThread;//显示线程
        Image[] image = new Image[COUNT_NUM * MODE_NUM];//图片资源
        string[] label = new string[COUNT_NUM];//文本提示
        int mode = 0;//不同标签类型编号

        public Form1()
        {
            InitializeComponent();
            LoadTid();
            LoadImage();
            LoadLabel();
            ConnectToReader();
            showThread = new Thread(new ThreadStart(ThreadMethod));
            showThread.IsBackground = true;
            showThread.Start();//启动线程
        }
        //加载TID
        void LoadTid()
        {
            sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + FILE_NAME, false);
            while(!sr.EndOfStream)
            {
                string str = sr.ReadLine();
                if (str[0] == '#')//忽略注释
                    continue;
                tid.Add(str);
            }
            
        }
        //加载图片资源
        void LoadImage()
        {
            //1压力
            image[0 + COUNT_NUM * 0] = Properties.Resources.p0;
            image[1 + COUNT_NUM * 0] = Properties.Resources.p1;
            image[2 + COUNT_NUM * 0] = Properties.Resources.p2;
            image[3 + COUNT_NUM * 0] = Properties.Resources.p3;
            image[4 + COUNT_NUM * 0] = Properties.Resources.p4;
            image[5 + COUNT_NUM * 0] = Properties.Resources.p7;
            image[6 + COUNT_NUM * 0] = Properties.Resources.p6;
            image[7 + COUNT_NUM * 0] = Properties.Resources.p7;
            //2弯曲
            image[0 + COUNT_NUM * 1] = Properties.Resources.b0;
            image[1 + COUNT_NUM * 1] = Properties.Resources.b1;
            image[2 + COUNT_NUM * 1] = Properties.Resources.b2;
            image[3 + COUNT_NUM * 1] = Properties.Resources.b3;
            image[4 + COUNT_NUM * 1] = Properties.Resources.b4;
            image[5 + COUNT_NUM * 1] = Properties.Resources.b7;
            image[6 + COUNT_NUM * 1] = Properties.Resources.b6;
            image[7 + COUNT_NUM * 1] = Properties.Resources.b7;
            //3重力
            image[0 + COUNT_NUM * 2] = Properties.Resources.g0;
            image[1 + COUNT_NUM * 2] = Properties.Resources.g1;
            image[2 + COUNT_NUM * 2] = Properties.Resources.g7;
            image[3 + COUNT_NUM * 2] = Properties.Resources.g7;
            image[4 + COUNT_NUM * 2] = Properties.Resources.g4;
            image[5 + COUNT_NUM * 2] = Properties.Resources.g7;
            image[6 + COUNT_NUM * 2] = Properties.Resources.g7;
            image[7 + COUNT_NUM * 2] = Properties.Resources.g7;
            //4温度
            image[0 + COUNT_NUM * 3] = Properties.Resources.t0;
            image[1 + COUNT_NUM * 3] = Properties.Resources.t1;
            image[2 + COUNT_NUM * 3] = Properties.Resources.t2;
            image[3 + COUNT_NUM * 3] = Properties.Resources.t3;
            image[4 + COUNT_NUM * 3] = Properties.Resources.t4;
            image[5 + COUNT_NUM * 3] = Properties.Resources.t7;
            image[6 + COUNT_NUM * 3] = Properties.Resources.t6;
            image[7 + COUNT_NUM * 3] = Properties.Resources.t7;

            pictureBox.BackgroundImage = image[mode];
        }
        //加载文本提示
        void LoadLabel()
        {
            label[0] = "No Chip";
            label[1] = "Chip 1";
            label[2] = "Chip 2";
            label[3] = "Chip 1&&2";
            label[4] = "Chip 3";
            label[5] = "Chip 1&&3";
            label[6] = "Chip 2&&3";
            label[7] = "Chip 1&&2&&3";

            labelChip.Text = label[count];
        }
        //显示线程
        private void ThreadMethod()
        {
            while (reader.IsConnected)
            {
                try
                {
                    UpdateCallback d = new UpdateCallback(ShowPicture);
                    Invoke(d, new object[] { });
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Thread error " + ex.ToString());
                    break;
                }

                Thread.Sleep(250);//间隔250ms以上比较稳定
            }
        }
        //显示图片
        void ShowPicture()
        {
            //查找
            List<Tag> buff = new List<Tag>(tags);//拷贝一份
            tags.Clear();
            count = 0;
            for (int i = 0; i < chip.Length; i++)
                chip[i] = false;
            foreach (Tag tag in buff)
            {
                for (int i = 0; i < tid.Count; i++)
                {
                    if (tag.Tid.ToString() == tid[i])
                    {
                        chip[i] = true;
                        mode = i / CHIP_NUM;
                        if (i % CHIP_NUM == 0)
                            count += 1;
                        else if (i % CHIP_NUM == 1)
                            count += 2;
                        else
                            count += 4;
                    }
                }

            }
            //切换显示
            pictureBox.BackgroundImage = image[mode * COUNT_NUM + count];
            labelChip.Text = label[count];
            
            Console.WriteLine(mode+" "+count);
        }
        
        void ConnectToReader()
        {
            try
            {
                reader.Name = "Reader #1";
                Console.WriteLine("Attempting to connect to {0} ({1}).",
                    reader.Name, READER_HOSTNAME);

                // Number of milliseconds before a 
                // connection attempt times out.
                reader.ConnectTimeout = 6000;
                // Connect to the reader.
                // Change the ReaderHostname constant in SolutionConstants.cs 
                // to the IP address or hostname of your reader.
                reader.Connect(READER_HOSTNAME);
                Console.WriteLine("Successfully connected.");

                // Tell the reader to send us any tag reports and 
                // events we missed while we were disconnected.
                reader.ResumeEventsAndReports();
                Settings settings = reader.QueryDefaultSettings();
                // Start the reader as soon as it's configured.
                // This will allow it to run without a client connected.
                settings.AutoStart.Mode = AutoStartMode.None;
                settings.AutoStop.Mode = AutoStopMode.None;
                settings.RfMode = 0;
                // Use Advanced GPO to set GPO #1 
                // when an client (LLRP) connection is present.
                settings.Gpos.GetGpo(1).Mode = GpoMode.LLRPConnectionStatus;

                // Tell the reader to include the timestamp in all tag reports.
                settings.Report.IncludeFirstSeenTime = true;
                settings.Report.IncludeLastSeenTime = true;
                settings.Report.IncludeSeenCount = true;
                settings.Report.IncludePeakRssi = true;
                settings.Report.IncludeChannel = true;
                settings.Report.IncludeFastId = true;
                // If this application disconnects from the 
                // reader, hold all tag reports and events.
                settings.HoldReportsOnDisconnect = false;

                // Enable keepalives.
                settings.Keepalives.Enabled = true;
                settings.Keepalives.PeriodInMs = 5000;

                // Enable link monitor mode.
                // If our application fails to reply to
                // five consecutive keepalive messages,
                // the reader will close the network connection.
                settings.Keepalives.EnableLinkMonitorMode = true;
                settings.Keepalives.LinkDownThreshold = 5;

                // Assign an event handler that will be called
                // when keepalive messages are received.
                reader.KeepaliveReceived += OnKeepaliveReceived;

                // Assign an event handler that will be called
                // if the reader stops sending keepalives.
                reader.ConnectionLost += OnConnectionLost;

                // Apply the newly modified settings.
                reader.ApplySettings(settings);

                // Save the settings to the reader's 
                // non-volatile memory. This will
                // allow the settings to persist
                // through a power cycle.
                reader.SaveSettings();
                reader.Start();
                // Assign the TagsReported event handler.
                // This specifies which method to call
                // when tags reports are available.
                reader.TagsReported += OnTagsReported;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to connect." + e);
            }
        }
        
        void OnConnectionLost(ImpinjReader reader)
        {
            // This event handler is called if the reader  
            // stops sending keepalive messages (connection lost).
            Console.WriteLine("Connection lost : {0} ({1})", reader.Name, reader.Address);

            // Cleanup
            reader.Disconnect();

            // Try reconnecting to the reader
            ConnectToReader();
        }

        void OnKeepaliveReceived(ImpinjReader reader)
        {
            // This event handler is called when a keepalive 
            // message is received from the reader.
            Console.WriteLine("Keepalive received from {0} ({1})", reader.Name, reader.Address);
        }

        void OnTagsReported(ImpinjReader sender, TagReport report)
        {
            // This event handler is called asynchronously 
            // when tag reports are available.
            // Loop through each tag in the report 
            // and print the data.
            Tag tag = report.Tags[0];//每次只能读到一个TAG？

            int i;
            for (i = 0; i < tags.Count; i++)
            {
                if (tag.Tid.ToString() == tags[i].Tid.ToString())
                {
                    break;
                }
            }
            //标签不在已有列表内
            if (i == tags.Count)
            {
                tags.Add(tag);
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            try
            {
                // Stop reading.
                reader.Stop();
                // Disconnect from the reader.
                reader.Disconnect();
               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to disconnect." + ex);
            }
            
        }


        private void pictureBox_DoubleClick(object sender, EventArgs e)
        {
            mode = (mode + 1) % MODE_NUM;
            pictureBox.BackgroundImage = image[mode * COUNT_NUM];
            labelChip.Text = label[0];
            Console.WriteLine("Change to mode " + mode);
        }


    }
}
