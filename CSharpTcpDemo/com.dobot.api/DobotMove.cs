using CSharthiscpDemo.com.dobot.api;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace CSharpTcpDemo.com.dobot.api
{
    class DobotMove : DobotClient
    {
        protected override void OnConnected(Socket sock)
        {
            sock.SendTimeout = 5000;
            sock.ReceiveTimeout = 15000;
        }

        protected override void OnDisconnected()
        {
        }

        /// <summary>
        /// 关节点动运动，不固定距离运动
        /// </summary>
        /// <param name="s">点动运动轴</param>
        /// <returns>返回执行结果的描述信息</returns>
        public string MoveJog(string s)
        {
            if (!IsConnected())
            {
                return "device does not connected!!!";
            }

            string str;
            if (string.IsNullOrEmpty(s))
            {
                str = "MoveJog()";
            }
            else
            {
                string strPattern = "^(J[1-6][+-])|([XYZ][+-])|(R[+-])$";
                if (Regex.IsMatch(s, strPattern))
                {
                    str = "MoveJog(" + s + ")";
                }
                else
                {
                    return "send error:invalid parameter!!!";
                }
            }
            if (!SendData(str))
            {
                return str + ":send error";
            }

            return WaitReply(5000);
        }
        /// <summary>
        /// 停止关节点动运动
        /// </summary>
        /// <returns>返回执行结果的描述信息</returns>
        public string StopMoveJog()
        {
            return MoveJog(null);
        }

        /// <summary>
        /// 点到点运动，目标点位为笛卡尔点位
        /// </summary>
        /// <param name="pt">笛卡尔点位</param>
        /// <returns>返回执行结果的描述信息</returns>
        public string MovJ(DescartesPoint pt)
        {
            if (!IsConnected())
            {
                return "device does not connected!!!";
            }

            if (null == pt)
            {
                return "send error:invalid parameter!!!";
            }
            string str = String.Format("MovJ({0},{1},{2},{3})", pt.x, pt.y, pt.z, pt.r);
            if (!SendData(str))
            {
                return str + ":send error";
            }

            return WaitReply(5000);
        }

        /// <summary>
        /// 直线运动，目标点位为笛卡尔点位
        /// </summary>
        /// <param name="pt">笛卡尔点位</param>
        /// <returns>返回执行结果的描述信息</returns>
        public string MovL(DescartesPoint pt)
        {
            if (!IsConnected())
            {
                return "device does not connected!!!";
            }
            if (null == pt)
            {
                return "send error:invalid parameter!!!";
            }
            string str = String.Format("MovL({0},{1},{2},{3})", pt.x, pt.y, pt.z, pt.r);
            if (!SendData(str))
            {
                return str + ":send error";
            }

            return WaitReply(5000);
        }

        /// <summary>
        /// 点到点运动，目标点位为关节点位。
        /// </summary>
        /// <param name="pt"></param>
        /// <returns>返回执行结果的描述信息</returns>
        public string JointMovJ(JointPoint pt)
        {
            if (!IsConnected())
            {
                return "device does not connected!!!";
            }
            if (null == pt)
            {
                return "send error:invalid parameter!!!";
            }
            string str = String.Format("JointMovJ({0},{1},{2},{3})", pt.j1, pt.j2, pt.j3, pt.j4);
            if (!SendData(str))
            {
                return str + ":send error";
            }

            return WaitReply(5000);
        }
        /// <summary>
        /// 沿用户坐标系进行相对运动指令，末端运动方式为关节运动。
        /// </summary>
        /// <param name="pt">相对位置</param>
        /// <returns>返回执行结果的描述信息</returns>
        public string RelMovJUser(OffsetPosition pt)
        {
            if (!IsConnected())
            {
                return "device does not connected!!!";
            }

            if (null == pt)
            {
                return "send error:invalid parameter!!!";
            }
            string str = String.Format("RelMovJUser({0},{1},{2},{3},{4},{5},{6})", pt.x, pt.y, pt.z, pt.rx, pt.ry, pt.rz, pt.user);
            if (!SendData(str))
            {
                return str + ":send error";
            }

            return WaitReply(5000);
        }

        /// <summary>
        /// 沿用户坐标系进行相对运动指令，末端运动方式为直线运动。
        /// </summary>
        /// <param name="pt">相对位置</param>
        /// <returns>返回执行结果的描述信息</returns>
        public string RelMovLUser(OffsetPosition pt)
        {
            if (!IsConnected())
            {
                return "device does not connected!!!";
            }
            if (null == pt)
            {
                return "send error:invalid parameter!!!";
            }
            string str = String.Format("RelMovLUser({0},{1},{2},{3},{4},{5},{6})", pt.x, pt.y, pt.z, pt.rx, pt.ry, pt.rz, pt.user);
            if (!SendData(str))
            {
                return str + ":send error";
            }

            return WaitReply(5000);
        }

        /// <summary>
        /// 沿各轴关节坐标系进行相对运动指令，末端运动方式为关节运动。
        /// </summary>
        /// <param name="pt">相对位置</param>
        /// <returns>返回执行结果的描述信息</returns>
        public string RelJointMovJ(OffsetPosition pt)
        {
            if (!IsConnected())
            {
                return "device does not connected!!!";
            }
            if (null == pt)
            {
                return "send error:invalid parameter!!!";
            }
            string str = String.Format("RelJointMovJ({0},{1},{2},{3},{4},{5})", pt.x, pt.y, pt.z, pt.rx, pt.ry, pt.rz);
            if (!SendData(str))
            {
                return str + ":send error";
            }

            return WaitReply(5000);
        }
    }
}
