using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace CSharpTcpDemo.com.dobot.api
{
    public abstract class DobotClient
    {
        private Socket mSocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public string IP { get; private set; }
        public int Port { get; private set; }

        /// <summary>
        /// 连接设备
        /// </summary>
        /// <param name="strIp">设备地址</param>
        /// <param name="iPort">指定端口</param>
        /// <returns>true成功，false失败</returns>
        public bool Connect(string strIp, int iPort)
        {
            if (null != mSocketClient && IsConnected())
            {
                if (strIp != this.IP || iPort != this.Port)
                {
                    this.Disconnect();
                }
                else
                {
                    return true;
                }
            }
            return this.ConnectDobotServer(strIp, iPort);
        }
        private bool ConnectDobotServer(string strIp, int iPort)
        {
            bool bOk = false;
            try
            {
                this.IP = strIp;
                this.Port = iPort;

                IPAddress addr = IPAddress.Parse(strIp);
                IPEndPoint endpt = new IPEndPoint(addr, iPort);

                mSocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                mSocketClient.Connect(endpt);

                mSocketClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                uint dummy = 0;
                byte[] optVal = new byte[Marshal.SizeOf(dummy) * 3];
                BitConverter.GetBytes((uint)1).CopyTo(optVal, 0);//是否启用Keep-Alive
                BitConverter.GetBytes((uint)5000).CopyTo(optVal, Marshal.SizeOf(dummy));//第一次开始发送探测包时间间隔
                BitConverter.GetBytes((uint)500).CopyTo(optVal, Marshal.SizeOf(dummy) * 2);//连续发送探测包时间间隔
                mSocketClient.IOControl(IOControlCode.KeepAliveValues, optVal, null);

                OnConnected(mSocketClient);

                bOk = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Connect failed:" + ex.ToString());
            }
            return bOk;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            try
            {
                mSocketClient.Shutdown(SocketShutdown.Both);
                mSocketClient.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("close socket:" + ex.ToString());
            }
        }

        public bool IsConnected()
        {
            try
            {
                return mSocketClient.Connected;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return false;
        }

        protected abstract void OnConnected(Socket sock);
        protected abstract void OnDisconnected();

        public delegate void OnNetworkError(DobotClient sender, SocketError iErrCode);
        public event OnNetworkError NetworkErrorEvent;

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="str">发送内容</param>
        /// <returns>成功-true，失败-false</returns>
        protected bool SendData(string str)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(str);
                return (mSocketClient.Send(data) == data.Length) ? true : false;
            }
            catch (SocketException ex)
            {
                if (null != NetworkErrorEvent && !IsConnected())
                {//发送抛异常，并且连接真的断开了，则触发这个事件
                    NetworkErrorEvent(this, ex.SocketErrorCode);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("send error:" + ex.ToString());
            }
            return false;
        }

        /// <summary>
        /// 等待响应
        /// </summary>
        /// <param name="iTimeoutMillsecond">等待时间，毫秒单位</param>
        /// <returns>返回响应的内容</returns>
        protected string WaitReply(int iTimeoutMillsecond)
        {
            try
            {
                if (iTimeoutMillsecond != mSocketClient.ReceiveTimeout)
                {
                    mSocketClient.ReceiveTimeout = iTimeoutMillsecond;
                }
                byte[] buffer = new byte[1024];
                int length = mSocketClient.Receive(buffer);
                string str = Encoding.UTF8.GetString(buffer, 0, length);

                return str;
            }
            catch (SocketException ex)
            {
                if (null != NetworkErrorEvent && !IsConnected())
                {//发送抛异常，并且连接真的断开了，则触发这个事件
                    NetworkErrorEvent(this, ex.SocketErrorCode);
                }
                return "send error:" + ex.Message;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("send error:" + ex.ToString());
                return "send error:" + ex.Message;
            }
        }

        protected int Receive(byte[] buffer, int offset, int size, SocketFlags flag)
        {
            int iRet = 0;
            try
            {
                iRet = mSocketClient.Receive(buffer, offset, size, flag);
                if (0 == iRet)
                {
                    if (null != NetworkErrorEvent && !IsConnected())
                    {
                        NetworkErrorEvent(this, SocketError.ConnectionAborted);
                    }
                }
            }
            catch (SocketException ex)
            {
                if (null != NetworkErrorEvent && !IsConnected())
                {
                    NetworkErrorEvent(this, ex.SocketErrorCode);
                }
                return -1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Receive error:" + ex.ToString());
            }
            return iRet;
        }
    }
}
