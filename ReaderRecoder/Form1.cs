using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Impinj.OctaneSdk;
using System.IO;
using System.Media;

namespace ReaderRecorder
{

    public partial class Form1 : Form
    {

        const string READER_HOSTNAME = "impinj-14-04-15";  // NEED to set to your speedway!
        // Create an instance of the ImpinjReader class.
        ImpinjReader reader = new ImpinjReader();
        delegate void UpdateCallback();
        string savePath = AppDomain.CurrentDomain.BaseDirectory;
        List<Tag> tags = new List<Tag>();
        bool isRecord = false;
        StreamWriter sw;
        ImpinjTimestamp ts;
        Thread demoThread;
        bool show = false;
        
        public Form1()
        {
            InitializeComponent();
            demoThread = new Thread(new ThreadStart(ThreadMethod));
            demoThread.IsBackground = true;
            demoThread.Start();//启动线程

        }
        private void ThreadMethod()
        {
            while (true)
            {

                if (reader.IsConnected)
                {
                    if (TagList.InvokeRequired)
                    {
                        UpdateCallback d = new UpdateCallback(ShowReport);
                        Invoke(d, new object[] { });
                    }
                    else
                    {
                        ShowReport();
                    }
                    
                }
                Thread.Sleep(500);
            }
        }
        void ShowReport()
        {
            
            if (show)//未刷新
            {
                TagList.BeginUpdate();
                TagList.Items.Clear();
                TagList.EndUpdate();
                return;
            }
                
            List<Tag> buff = new List<Tag>(tags);
            if(btnBeep.Tag.ToString() == "1" && buff.Count != 0)
                SystemSounds.Beep.Play();
            TagList.BeginUpdate();
            TagList.Items.Clear();
            for (int i=0; i< buff.Count;i++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = buff[i].Tid.ToString();
                lvi.SubItems.Add(buff[i].Epc.ToString());
                lvi.SubItems.Add(buff[i].PeakRssiInDbm.ToString());
                lvi.SubItems.Add(buff[i].ChannelInMhz.ToString());
                lvi.SubItems.Add(buff[i].PhaseAngleInRadians.ToString());
                lvi.SubItems.Add(buff[i].LastSeenTime.ToString());
                TagList.Items.Add(lvi);
            }
            TagList.Sort();
            TagList.EndUpdate();
            show = true;

        }
        bool ConnectToReader()
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
                //settings.Gpos.GetGpo(1).Mode = GpoMode.LLRPConnectionStatus;
                settings.SearchMode = SearchMode.DualTarget;
                
                // Tell the reader to include the timestamp in all tag reports.
                settings.Report.IncludeFirstSeenTime = true;
                settings.Report.IncludeLastSeenTime = true;
                settings.Report.IncludeSeenCount = true;
                settings.Report.IncludePeakRssi = true;
                settings.Report.IncludeChannel = true;
                settings.Report.IncludeFastId = true;
                settings.Report.IncludePhaseAngle = true;
                // If this application disconnects from the 
                // reader, hold all tag reports and events.
                settings.HoldReportsOnDisconnect = true;
                
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
                Console.WriteLine("Failed to connect.");
                //throw e;
                return false;
            }
            return true;
        }
        bool DisconnectToReader()
        {
            try
            {
                // Stop reading.
                reader.Stop();

                // Disconnect from the reader.
                reader.Disconnect();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to connect.");
                //throw e;
                return false;
            }
            return true;
        }
        void StartRecord()
        {
            string path = savePath + "\\"+DateTime.Now.ToString("yyyyMMddHHmmss")+".csv";
            sw = new StreamWriter(path, false);
            string title = "TS,TID,EPC,RSSI,CH,PH,";
            sw.WriteLine(title);
            isRecord = true;
        }
        void EndRecord()
        {
            if (isRecord)
            {
                isRecord = false;
                sw.Close();
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
            string data = "";
            if (isRecord)
            {
                if (ts != tag.LastSeenTime)
                {
                    data += tag.LastSeenTime;
                    data += ',';
                    data += tag.Tid.ToHexString();
                    data += ',';
                    data += tag.Epc.ToHexString();
                    data += ',';
                    data += tag.PeakRssiInDbm.ToString();
                    data += ',';
                    data += tag.ChannelInMhz.ToString();
                    data += ',';
                    data += tag.PhaseAngleInRadians.ToString();
                    data += ',';
                    sw.WriteLine(data);
                }
            }
            ts = tag.LastSeenTime;
            //Console.WriteLine("TS:{0}, NUM:{1}", tag.LastSeenTime,report.Tags.Count);
            Console.WriteLine("TID : {0} PH : {1}", tag.Tid.ToHexWordString(), tag.PhaseAngleInRadians.ToString());
            if (show)
            {
                tags.Clear();
                tags.Add(tag);
                show = false;
            }
            else
            {
                int i;
                for (i = 0; i < tags.Count; i++)
                {
                    if (tag.Tid.ToString() == tags[i].Tid.ToString())
                    {
                        tags[i] = tag;
                        break;
                    }
                }
                if (i == tags.Count)
                {
                    tags.Add(tag);
                }
            }
        }
        private void ButtonConnect_Click(object sender, EventArgs e)
        {
            if (!reader.IsConnected)
            {
                if (ConnectToReader())
                {
                    ButtonConnect.Text = "Disconnect";
                }
            }
            else
            {
                if (DisconnectToReader())
                {
                    ButtonConnect.Text = "Connect";
                }
            }
        }
        private void ButtonClear_Click(object sender, EventArgs e)
        {
            tags.Clear();
            ShowReport();
        }

        private void ButtonSettings_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.SelectedPath = savePath;
            if (fb.ShowDialog() == DialogResult.OK)
            {
                savePath = fb.SelectedPath;
            }
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if(reader.IsConnected && !isRecord)
            {
                StartRecord();
                ButtonStart.Text = "End";
            }
            else
            {
                EndRecord();
                ButtonStart.Text = "Start";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            demoThread.Abort();
            DisconnectToReader();
        }

        private void TagList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point m_MBRpt = TagList.PointToClient(MousePosition);//鼠标右键点击时Point
                if (TagList.GetItemAt(0, m_MBRpt.Y) != null)
                {
                    ListViewItem lstrow = TagList.GetItemAt(0, m_MBRpt.Y);// 得到屏幕鼠标的坐标，转换为列表控件的坐标
                    Clipboard.SetDataObject(lstrow.Text);//设置到粘贴板
                }

            }
        }

        private void btnBeep_Click(object sender, EventArgs e)
        {
            if(btnBeep.Tag.ToString() == "1")
            {
                btnBeep.Tag = "0";
                btnBeep.Text = "Beep Off";
            }
            else
            {
                btnBeep.Tag = "1";
                btnBeep.Text = "Beep On";
            }
        }
    }
}
