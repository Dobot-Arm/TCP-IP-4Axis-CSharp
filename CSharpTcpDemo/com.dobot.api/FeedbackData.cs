using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTcpDemo.com.dobot.api
{
    class FeedbackData
    {
        #region 机器人模式
        public const int NO_CONTROLLER = -1;
        public const int NO_CONNECTED = 0;
        public const int ROBOT_MODE_INIT = 1;
        public const int ROBOT_MODE_BRAKE_OPEN = 2;
        public const int ROBOT_RESERVED = 3;
        public const int ROBOT_MODE_DISABLED = 4;
        public const int ROBOT_MODE_ENABLE = 5;
        public const int ROBOT_MODE_BACKDRIVE = 6;
        public const int ROBOT_MODE_RUNNING = 7;
        public const int ROBOT_MODE_RECORDING = 8;
        public const int ROBOT_MODE_ERROR = 9;
        public const int ROBOT_MODE_PAUSE = 10;
        public const int ROBOT_MODE_JOG = 11;
        #endregion

        public short MessageSize = 0;//消息字节总长度

        public short[] Reserved1 = new short[3];//保留位

        public long DigitalInputs = 0;//数字输入
        public long DigitalOutputs = 0;//数字输出
        public long RobotMode = -1;//机器人模式
        public long TimeStamp = 0;//时间戳（单位ms）

        public long Reserved2 = 0;//保留位
        public long TestValue = 0;//内存结构测试标准值  0x0123 4567 89AB CDEF
        public double Reserved3 = 0;//保留位

        public double SpeedScaling = 0;//速度比例
        public double LinearMomentumNorm = 0; //机器人当前动量
        public double VMain = 0;//控制板电压
        public double VRobot = 0;//机器人电压
        public double IRobot = 0;//机器人电流

        public double Reserved4 = 0;//保留位
        public double Reserved5 = 0;//保留位

        public double[] ToolAccelerometerValues = new double[3];//TCP加速度
        public double[] ElbowPosition = new double[3];//肘位置
        public double[] ElbowVelocity = new double[3];//肘速度

        public double[] QTarget = new double[6];//目标关节位置
        public double[] QdTarget = new double[6];//目标关节速度
        public double[] QddTarget = new double[6];//目标关节加速度
        public double[] ITarget = new double[6];//目标关节加速度
        public double[] MTarget = new double[6];//目标关节电流
        public double[] QActual = new double[6];//实际关节位置
        public double[] QdActual = new double[6];//实际关节速度
        public double[] IActual = new double[6];//实际关节电流
        public double[] IControl = new double[6];//TCP传感器力值
        public double[] ToolVectorActual = new double[6];//TCP笛卡尔实际坐标值
        public double[] TCPSpeedActual = new double[6]; //TCP笛卡尔实际速度值
        public double[] TCPForce = new double[6];//TCP力值
        public double[] ToolVectorTarget = new double[6];//TCP笛卡尔目标坐标值
        public double[] TCPSpeedTarget = new double[6];//TCP笛卡尔目标速度值
        public double[] MotorTempetatures = new double[6];//关节温度
        public double[] JointModes = new double[6];//关节控制模式
        public double[] VActual = new double[6];//关节电压

        public byte[] Handtype = new byte[4];//手系
        public byte User = 0;//用户坐标
        public byte Tool = 0;//工具坐标
        public byte RunQueuedCmd = 0;//算法队列运行标志
        public byte PauseCmdFlag = 0;//算法队列暂停标志
        public byte VelocityRatio = 0;//关节速度比例(0~100)
        public byte AccelerationRatio = 0;//关节加速度比例(0~100)
        public byte JerkRatio = 0;//关节加加速度比例(0~100)
        public byte XYZVelocityRatio = 0;//笛卡尔位置速度比例(0~100)
        public byte RVelocityRatio = 0;//笛卡尔姿态速度比例(0~100)
        public byte XYZAccelerationRatio = 0;//笛卡尔位置加速度比例(0~100)
        public byte RAccelerationRatio = 0;//笛卡尔姿态加速度比例(0~100)
        public byte XYZJerkRatio = 0;//笛卡尔位置加加速度比例(0~100)
        public byte RJerkRatio = 0;//笛卡尔姿态加加速度比例(0~100)

        public byte BrakeStatus = 0; //机器人抱闸状态
        public byte EnableStatus = 0;//机器人使能状态
        public byte DragStatus = 0;//机器人拖拽状态
        public byte RunningStatus = 0;//机器人运行状态
        public byte ErrorStatus = 0;//机器人报警状态
        public byte JogStatus = 0;//机器人点动状态
        public byte RobotType = 0; //机器类型
        public byte DragButtonSignal = 0; //按钮板拖拽信号
        public byte EnableButtonSignal = 0;//按钮板使能信号
        public byte RecordButtonSignal = 0;//按钮板录制信号
        public byte ReappearButtonSignal = 0;//按钮板复现信号
        public byte JawButtonSignal = 0; //按钮板夹爪控制信号
        public byte SixForceOnline = 0;//六维力在线状态

        public byte[] Reserved6 = new byte[82];//保留位

        public double[] MActual = new double[6];//实际扭矩
        public double Load = 0;//负载重量kg
        public double CenterX = 0;//X方向偏心距离mm
        public double CenterY = 0;//Y方向偏心距离mm
        public double CenterZ = 0;//Z方向偏心距离mm
        public double[] UserValu = new double[6];//用户坐标值
        public double[] Tools = new double[6];//工具坐标值
        public double TraceIndex = 0;//轨迹复现运行索引
        public double[] SixForceValue = new double[6];//当前六维力数据原始值
        public double[] TargetQuaternion = new double[4]; //[qw,qx,qy,qz] 目标四元数
        public double[] ActualQuaternion = new double[4];//[qw,qx,qy,qz]  实际四元数

        public byte[] Reserved7 = new byte[24];//保留位
    }
}
