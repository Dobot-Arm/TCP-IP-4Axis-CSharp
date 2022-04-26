using CSharpTcpDemo.com.dobot.api;
using CSharthiscpDemo.com.dobot.api;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CSharpTcpDemo
{
    public partial class MainForm : Form
    {
        private Feedback mFeedback = new Feedback();
        private DobotMove mDobotMove = new DobotMove();
        private Dashboard mDashboard = new Dashboard();

        //定时获取数据并显示到UI
        private System.Timers.Timer mTimerReader = new System.Timers.Timer(300);

        public MainForm()
        {
            InitializeComponent();

            this.textBoxIP.Text = "192.168.5.1";
            this.textBoxDashboardPort.Text = "29999";
            this.textBoxMovePort.Text = "30003";
            this.textBoxFeedbackPort.Text = "30004";
            this.textBoxSpeedRatio.Text = "10";

            #region +按钮事件
            BindBtn_MoveEvent(this.btnAdd1, "J1+");
            BindBtn_MoveEvent(this.btnAdd2, "J2+");
            BindBtn_MoveEvent(this.btnAdd3, "J3+");
            BindBtn_MoveEvent(this.btnAdd4, "J4+");
            #endregion

            #region -按钮事件
            BindBtn_MoveEvent(this.btnMinus1, "J1-");
            BindBtn_MoveEvent(this.btnMinus2, "J2-");
            BindBtn_MoveEvent(this.btnMinus3, "J3-");
            BindBtn_MoveEvent(this.btnMinus4, "J4-");
            #endregion

            #region XYZR+按钮事件
            BindBtn_MoveEvent(this.btnAddX, "X+");
            BindBtn_MoveEvent(this.btnAddY, "Y+");
            BindBtn_MoveEvent(this.btnAddZ, "Z+");
            BindBtn_MoveEvent(this.btnAddRX, "R+");
            #endregion

            #region XYZR-按钮事件
            BindBtn_MoveEvent(this.btnMinusX, "X-");
            BindBtn_MoveEvent(this.btnMinusY, "Y-");
            BindBtn_MoveEvent(this.btnMinusZ, "Z-");
            BindBtn_MoveEvent(this.btnMinusRX, "R-");
            #endregion

            //启动定时器
            mTimerReader.Elapsed += new System.Timers.ElapsedEventHandler(TimeoutEvent);
            mTimerReader.AutoReset = true;

            //默认禁止窗口中的大部分控件
            DisableWindow();

            string strPath = System.Windows.Forms.Application.StartupPath + "\\";
            ErrorInfoHelper.ParseControllerJsonFile(strPath+ "alarm_controller.json");
            ErrorInfoHelper.ParseServoJsonFile(strPath + "alarm_servo.json");
        }

        private void BindBtn_MoveEvent(Button btn, string strTag)
        {
            btn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMoveJogEvent);
            btn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnStopMoveJogEvent);
            btn.Tag = strTag;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mTimerReader.Close();
            if (this.mFeedback != null)
            {
                this.mFeedback.Disconnect();
            }
            if (this.mDashboard != null)
            {
                this.mDashboard.Disconnect();
            }
            if (this.mDobotMove != null)
            {
                this.mDobotMove.Disconnect();
            }
        }
        private void InsertLogToRichBox(RichTextBox box, string str)
        {
            if (box.GetLineFromCharIndex(box.TextLength) > 100)
            {
                box.Text = (str += "\r\n");
            }
            else
            {
                box.Text += (str + "\r\n");
            }
            box.Focus();
            box.Select(box.TextLength, 0);
            box.ScrollToCaret();
        }
        private void PrintLog(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return;
            }
            if (this.richTextBoxLog.InvokeRequired)
            {
                this.richTextBoxLog.Invoke(new Action<string>(log => {
                    InsertLogToRichBox(this.richTextBoxLog, log);
                }), str);
            }
            else
            {
                InsertLogToRichBox(this.richTextBoxLog, str);
            }
        }
        private void PrintErrorInfo(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return;
            }
            if (this.richTextBoxErrInfo.InvokeRequired)
            {
                this.richTextBoxErrInfo.Invoke(new Action<string>(log => {
                    InsertLogToRichBox(this.richTextBoxErrInfo, log);
                }), str);
            }
            else
            {
                InsertLogToRichBox(this.richTextBoxErrInfo, str);
            }
        }

        private void DisableWindow()
        {
            foreach (Control ctr in this.Controls)
            {
                if (ctr == this.groupBoxConnect)
                {
                    ctr.Enabled = true;
                }
                else if (ctr == this.groupBoxLog)
                {
                    ctr.Enabled = true;
                }
                else
                {
                    ctr.Enabled = false;
                }
            }
        }
        private void EnableWindow()
        {
            foreach (Control ctr in this.Controls)
            {
                ctr.Enabled = true;
            }
        }

        private void OnMoveJogEvent(object sender, MouseEventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                string str = btn.Tag.ToString();
                DoMoveJog(str);
            }
        }
        private void OnStopMoveJogEvent(object sender, MouseEventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                DoStopMoveJog();
            }
        }

        private void DoMoveJog(string str)
        {
            PrintLog(string.Format("send to {0}:{1}: MoveJog({2})", mDobotMove.IP,mDobotMove.Port,str));
            Thread thd = new Thread(() => {
                string ret = mDobotMove.MoveJog(str);
                PrintLog(string.Format("Receive From {0}:{1}: {2}", mDobotMove.IP, mDobotMove.Port, ret));
            });
            thd.Start();
        }

        private void DoStopMoveJog()
        {
            PrintLog(string.Format("send to {0}:{1}: MoveJog()", mDobotMove.IP, mDobotMove.Port));
            Thread thd = new Thread(() => {
                string ret = mDobotMove.StopMoveJog();
                PrintLog(string.Format("Receive From {0}:{1}: {2}", mDobotMove.IP, mDobotMove.Port, ret));
            });
            thd.Start();
        }
        private void TimeoutEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!mFeedback.DataHasRead)
            {
                return;
            }
            mFeedback.DataHasRead = false;
            if (this.labDI.InvokeRequired)
            {
                this.labDI.Invoke(new Action(() => {
                    ShowDataResult();
                }));
            }
            else
            {
                ShowDataResult();
            }
        }

        private bool IsValidIP(string strIp)
        {
            try
            {
                IPAddress.Parse(strIp);
            }
            catch
            {
                return false;
            }
            return true;
        }
        private int Parse2Int(string str)
        {
            int iValue = 0;
            try
            {
                iValue = int.Parse(str);
            }
            catch
            {
            }
            return iValue;
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (this.btnConnect.Text.Equals("Disconnect"))
            {
                Disconnect();
                return;
            }
            Connect();
        }

        private void Connect()
        {
            string strIp = textBoxIP.Text;
            if (!IsValidIP(strIp))
            {
                MessageBox.Show("IP Address Invalid");
                return;
            }
            int iPortFeedback = Parse2Int(this.textBoxFeedbackPort.Text);
            int iPortMove = Parse2Int(this.textBoxMovePort.Text);
            int iPortDashboard = Parse2Int(this.textBoxDashboardPort.Text);

            PrintLog("Connecting...");
            Thread thd = new Thread(() => {
                if (!mDashboard.Connect(strIp, iPortDashboard))
                {
                    PrintLog(string.Format("Connect {0}:{1} Fail!!", strIp, iPortDashboard));
                    return;
                }
                if (!mDobotMove.Connect(strIp, iPortMove))
                {
                    PrintLog(string.Format("Connect {0}:{1} Fail!!", strIp, iPortMove));
                    return;
                }
                if (!mFeedback.Connect(strIp, iPortFeedback))
                {
                    PrintLog(string.Format("Connect {0}:{1} Fail!!", strIp, iPortFeedback));
                    return;
                }

                mTimerReader.Start();

                PrintLog("Connect Success!!!");

                this.Invoke(new Action(() => {
                    EnableWindow();
                    this.btnConnect.Text = "Disconnect";
                }));
            });
            thd.Start();
        }

        private void Disconnect()
        {
            PrintLog("Disconnecting...");
            Thread thd = new Thread(() => {
                mFeedback.Disconnect();
                mDobotMove.Disconnect();
                mDashboard.Disconnect();
                PrintLog("Disconnect success!!!");

                mTimerReader.Stop();

                this.Invoke(new Action(() => {
                    DisableWindow();
                    this.btnConnect.Text = "Connect";
                }));
            });
            thd.Start();
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            bool bEnable = this.btnEnable.Text.Equals("Enable");

            PrintLog(string.Format("send to {0}:{1}: {2}()", mDashboard.IP, mDashboard.Port, bEnable? "EnableRobot" : "DisableRobot"));
            Thread thd = new Thread(() => {
                string ret = bEnable ? mDashboard.EnableRobot() : mDashboard.DisableRobot();
                bool bOk = ret.StartsWith("0");

                this.btnEnable.Invoke(new Action(() => {
                    if (bOk)
                    {
                        this.btnEnable.Text = bEnable ? "Disable" : "Enable";
                    }
                }));

                PrintLog(string.Format("Receive From {0}:{1}: {2}", mDashboard.IP, mDashboard.Port, ret));
            });
            thd.Start();
        }

        private void btnResetRobot_Click(object sender, EventArgs e)
        {
            PrintLog(string.Format("send to {0}:{1}: ResetRobot()", mDashboard.IP, mDashboard.Port));
            Thread thd = new Thread(() => {
                string ret = mDashboard.ResetRobot();
                PrintLog(string.Format("Receive From {0}:{1}: {2}", mDashboard.IP, mDashboard.Port, ret));
            });
            thd.Start();
        }

        private void btnClearError_Click(object sender, EventArgs e)
        {
            PrintLog(string.Format("send to {0}:{1}: ClearError()", mDashboard.IP, mDashboard.Port));
            Thread thd = new Thread(() => {
                string ret = mDashboard.ClearError();
                PrintLog(string.Format("Receive From {0}:{1}: {2}", mDashboard.IP, mDashboard.Port, ret));
            });
            thd.Start();
        }

        private void btnSpeedConfirm_Click(object sender, EventArgs e)
        {
            int iValue = Parse2Int(this.textBoxSpeedRatio.Text);
            PrintLog(string.Format("send to {0}:{1}: SpeedFactor({1})", mDashboard.IP, mDashboard.Port, iValue));
            Thread thd = new Thread(() => {
                string ret = mDashboard.SpeedFactor(iValue);
                PrintLog(string.Format("Receive From {0}:{1}: {2}", mDashboard.IP, mDashboard.Port, ret));
            });
            thd.Start();
        }

        private double Parse2Double(string str)
        {
            double value = 0.0;
            try
            {
                value = Double.Parse(str);
            }
            catch { }
            return value;
        }
        private void btnMovJ_Click(object sender, EventArgs e)
        {
            DescartesPoint pt = new DescartesPoint();
            pt.x = Parse2Double(this.textBoxX.Text);
            pt.y = Parse2Double(this.textBoxY.Text);
            pt.z = Parse2Double(this.textBoxZ.Text);
            pt.rx = Parse2Double(this.textBoxRx.Text);

            PrintLog(string.Format("send to {0}:{1}: MovJ({2})", mDobotMove.IP, mDobotMove.Port,pt.ToString()));
            Thread thd = new Thread(() => {
                string ret = mDobotMove.MovJ(pt);
                PrintLog(string.Format("Receive From {0}:{1}: {2}", mDobotMove.IP, mDobotMove.Port, ret));
            });
            thd.Start();
        }

        private void btnMovL_Click(object sender, EventArgs e)
        {
            DescartesPoint pt = new DescartesPoint();
            pt.x = Parse2Double(this.textBoxX.Text);
            pt.y = Parse2Double(this.textBoxY.Text);
            pt.z = Parse2Double(this.textBoxZ.Text);
            pt.rx = Parse2Double(this.textBoxRx.Text);

            PrintLog(string.Format("send to {0}:{1}: MovL({2})", mDobotMove.IP, mDobotMove.Port, pt.ToString()));
            Thread thd = new Thread(() => {
                string ret = mDobotMove.MovL(pt);
                PrintLog(string.Format("Receive From {0}:{1}: {2}", mDobotMove.IP, mDobotMove.Port, ret));
            });
            thd.Start();
        }

        private void btnJointMovJ_Click(object sender, EventArgs e)
        {
            JointPoint pt = new JointPoint();
            pt.j1 = Parse2Double(this.textBoxJ1.Text);
            pt.j2 = Parse2Double(this.textBoxJ2.Text);
            pt.j3 = Parse2Double(this.textBoxJ3.Text);
            pt.j4 = Parse2Double(this.textBoxJ4.Text);

            PrintLog(string.Format("send to {0}:{1}: JointMovJ({2})", mDobotMove.IP, mDobotMove.Port, pt.ToString()));
            Thread thd = new Thread(() => {
                string ret = mDobotMove.JointMovJ(pt);
                PrintLog(string.Format("Receive From {0}:{1}: {2}", mDobotMove.IP, mDobotMove.Port, ret));
            });
            thd.Start();
        }

        private void btnDOInput_Click(object sender, EventArgs e)
        {
            int idx = Parse2Int(this.textBoxIdx.Text);
            bool bIsOn = string.Compare("on", this.cboStatus.Text, true) == 0;

            PrintLog(string.Format("send to {0}:{1}: DigitalOutputs({2},{3})", mDashboard.IP, mDashboard.Port,
                idx, bIsOn));
            Thread thd = new Thread(() => {
                string ret = mDashboard.DigitalOutputs(idx, bIsOn);
                PrintLog(string.Format("Receive From {0}:{1}: {2}", mDashboard.IP, mDashboard.Port, ret));
            });
            thd.Start();
        }

        private void btnClearErrorInfo_Click(object sender, EventArgs e)
        {
            this.richTextBoxErrInfo.Clear();
        }

        private void ShowDataResult()
        {
            this.labCurrentSpeedRatio.Text = string.Format("Current Speed Ratio:{0:F2}%", mFeedback.feedbackData.SpeedScaling);
            this.labRobotMode.Text = string.Format("Robot Mode:{0}", mFeedback.ConvertRobotMode());

            if (null != mFeedback.feedbackData.QActual && mFeedback.feedbackData.QActual.Length >= 4)
            {
                this.labJ1.Text = string.Format("J1:{0:F3}", mFeedback.feedbackData.QActual[0]);
                this.labJ2.Text = string.Format("J2:{0:F3}", mFeedback.feedbackData.QActual[1]);
                this.labJ3.Text = string.Format("J3:{0:F3}", mFeedback.feedbackData.QActual[2]);
                this.labJ4.Text = string.Format("J4:{0:F3}", mFeedback.feedbackData.QActual[3]);
            }

            if (null != mFeedback.feedbackData.ToolVectorActual && mFeedback.feedbackData.ToolVectorActual.Length >= 4)
            {
                this.labX.Text = string.Format("X:{0:F3}", mFeedback.feedbackData.ToolVectorActual[0]);
                this.labY.Text = string.Format("Y:{0:F3}", mFeedback.feedbackData.ToolVectorActual[1]);
                this.labZ.Text = string.Format("Z:{0:F3}", mFeedback.feedbackData.ToolVectorActual[2]);
                this.labRx.Text = string.Format("Rx:{0:F3}", mFeedback.feedbackData.ToolVectorActual[3]);
            }

            this.labDI.Text = "Digital Inputs:" + Convert.ToString(mFeedback.feedbackData.DigitalInputs, 2).PadLeft(64, '0');
            this.labDO.Text = "Digital Outputs:" + Convert.ToString(mFeedback.feedbackData.DigitalOutputs, 2).PadLeft(64, '0');

            ParseWarn();
        }

        private void ParseWarn()
        {
            if (this.mFeedback.feedbackData.RobotMode != 9) return;
            string strResult = mDashboard.GetErrorID();
            //strResult=ErrorID,{[[id,...,id], [id], [id], [id], [id], [id], [id]]},GetErrorID()
            if (!strResult.StartsWith("0")) return;

            //截取第一个{}内容
            int iBegPos = strResult.IndexOf('{');
            if (iBegPos < 0) return;
            int iEndPos = strResult.IndexOf('}', iBegPos + 1);
            if (iEndPos <= iBegPos) return;
            strResult = strResult.Substring(iBegPos + 1, iEndPos - iBegPos - 1);
            if (string.IsNullOrEmpty(strResult)) return;

            //剩余7组[]，第1组是控制器报警，其他6组是伺服报警
            StringBuilder sb = new StringBuilder();
            JArray arrWarn = JArray.Parse(strResult);
            for (int i = 0; i < arrWarn.Count; ++i)
            {
                JArray arr = arrWarn[i].ToObject<JArray>();
                for (int j = 0; j < arr.Count; ++j)
                {
                    ErrorInfoBean bean = null;
                    if (0 == i)
                    {//控制器报警
                        bean = ErrorInfoHelper.FindController(arr[j].ToObject<int>());
                    }
                    else
                    {//伺服报警
                        bean = ErrorInfoHelper.FindServo(arr[j].ToObject<int>());
                    }
                    if (null != bean)
                    {
                        sb.Append("ID:" + bean.id + "\r\n");
                        sb.Append("Type:" + bean.Type + "\r\n");
                        sb.Append("Level:" + bean.level + "\r\n");
                        sb.Append("Solution:" + bean.en.solution + "\r\n");
                    }
                }
            }

            if (sb.Length > 0)
            {
                DateTime dt = DateTime.Now;
                string strTime = string.Format("Time Stamp:{0}.{1}.{2} {3}:{4}:{5}", dt.Year,
                    dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
                PrintErrorInfo(strTime + "\r\n" + sb.ToString());
            }
            return;
        }
    }
}
