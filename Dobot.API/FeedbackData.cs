namespace Dobot.API
{
  public class FeedbackData
  {
    public byte AccelerationRatio; //关节加速度比例(0~100)
    public double[] ActualQuaternion = new double[4]; //[qw,qx,qy,qz]  实际四元数

    public byte BrakeStatus; //机器人抱闸状态
    public double CenterX; //X方向偏心距离mm
    public double CenterY; //Y方向偏心距离mm
    public double CenterZ; //Z方向偏心距离mm

    public long DigitalInputs; //数字输入
    public long DigitalOutputs; //数字输出
    public byte DragButtonSignal; //按钮板拖拽信号
    public byte DragStatus; //机器人拖拽状态
    public double[] ElbowPosition = new double[3]; //肘位置
    public double[] ElbowVelocity = new double[3]; //肘速度
    public byte EnableButtonSignal; //按钮板使能信号
    public byte EnableStatus; //机器人使能状态
    public byte ErrorStatus; //机器人报警状态

    public byte[] Handtype = new byte[4]; //手系
    public double[] IActual = new double[6]; //实际关节电流
    public double[] IControl = new double[6]; //TCP传感器力值
    public double IRobot; //机器人电流
    public double[] ITarget = new double[6]; //目标关节加速度
    public byte JawButtonSignal; //按钮板夹爪控制信号
    public byte JerkRatio; //关节加加速度比例(0~100)
    public byte JogStatus; //机器人点动状态
    public double[] JointModes = new double[6]; //关节控制模式
    public double LinearMomentumNorm; //机器人当前动量
    public double Load; //负载重量kg

    public double[] MActual = new double[6]; //实际扭矩
    public short MessageSize; //消息字节总长度
    public double[] MotorTempetatures = new double[6]; //关节温度
    public double[] MTarget = new double[6]; //目标关节电流
    public byte PauseCmdFlag; //算法队列暂停标志
    public double[] QActual = new double[6]; //实际关节位置
    public double[] QdActual = new double[6]; //实际关节速度
    public double[] QddTarget = new double[6]; //目标关节加速度
    public double[] QdTarget = new double[6]; //目标关节速度

    public double[] QTarget = new double[6]; //目标关节位置
    public byte RAccelerationRatio; //笛卡尔姿态加速度比例(0~100)
    public byte ReappearButtonSignal; //按钮板复现信号
    public byte RecordButtonSignal; //按钮板录制信号

    public short[] Reserved1 = new short[3]; //保留位

    public long Reserved2; //保留位
    public double Reserved3; //保留位

    public double Reserved4; //保留位
    public double Reserved5; //保留位

    public byte[] Reserved6 = new byte[82]; //保留位

    public byte[] Reserved7 = new byte[24]; //保留位
    public byte RJerkRatio; //笛卡尔姿态加加速度比例(0~100)
    public long RobotMode; //机器人模式
    public byte RobotType; //机器类型
    public byte RunningStatus; //机器人运行状态
    public byte RunQueuedCmd; //算法队列运行标志
    public byte RVelocityRatio; //笛卡尔姿态速度比例(0~100)
    public byte SixForceOnline; //六维力在线状态
    public double[] SixForceValue = new double[6]; //当前六维力数据原始值

    public double SpeedScaling; //速度比例
    public double[] TargetQuaternion = new double[4]; //[qw,qx,qy,qz] 目标四元数
    public double[] TCPForce = new double[6]; //TCP力值
    public double[] TCPSpeedActual = new double[6]; //TCP笛卡尔实际速度值
    public double[] TCPSpeedTarget = new double[6]; //TCP笛卡尔目标速度值
    public long TestValue; //内存结构测试标准值  0x0123 4567 89AB CDEF
    public long TimeStamp; //时间戳（单位ms）
    public byte Tool; //工具坐标

    public double[] ToolAccelerometerValues = new double[3]; //TCP加速度
    public double[] Tools = new double[6]; //工具坐标值
    public double[] ToolVectorActual = new double[6]; //TCP笛卡尔实际坐标值
    public double[] ToolVectorTarget = new double[6]; //TCP笛卡尔目标坐标值
    public double TraceIndex; //轨迹复现运行索引
    public byte User; //用户坐标
    public double[] UserValu = new double[6]; //用户坐标值
    public double[] VActual = new double[6]; //关节电压
    public byte VelocityRatio; //关节速度比例(0~100)
    public double VMain; //控制板电压
    public double VRobot = 0; //机器人电压
    public byte XYZAccelerationRatio; //笛卡尔位置加速度比例(0~100)
    public byte XYZJerkRatio; //笛卡尔位置加加速度比例(0~100)
    public byte XYZVelocityRatio; //笛卡尔位置速度比例(0~100)

    public string GetRobotModeString()
    {
      return GetRobotModeString(RobotMode);
    }

    public static string GetRobotModeString(long robotMode)
    {
      switch (robotMode)
      {
        case -1:
          return "NO_CONTROLLER";
        case 0:
          return "NO_CONNECTED";
        case 1:
          return "ROBOT_MODE_INIT";
        case 2:
          return "ROBOT_MODE_BRAKE_OPEN";
        case 3:
          return "ROBOT_RESERVED";
        case 4:
          return "ROBOT_MODE_DISABLED";
        case 5:
          return "ROBOT_MODE_ENABLE";
        case 6:
          return "ROBOT_MODE_BACKDRIVE";
        case 7:
          return "ROBOT_MODE_RUNNING";
        case 8:
          return "ROBOT_MODE_RECORDING";
        case 9:
          return "ROBOT_MODE_ERROR";
        case 10:
          return "ROBOT_MODE_PAUSE";
        case 11:
          return "ROBOT_MODE_JOG";
        default:
          return $"UNKNOWN: {robotMode}";
      }
    }

    public FeedbackData Clone()
    {
      return Clone(this);
    }

    public static FeedbackData Clone(FeedbackData original)
    {
      return new FeedbackData
      {
        MessageSize = original.MessageSize,
        Reserved1 = (short[])original.Reserved1.Clone(),
        DigitalInputs = original.DigitalInputs,
        DigitalOutputs = original.DigitalOutputs,
        RobotMode = original.RobotMode,
        TimeStamp = original.TimeStamp,
        Reserved2 = original.Reserved2,
        TestValue = original.TestValue,
        Reserved3 = original.Reserved3,
        SpeedScaling = original.SpeedScaling,
        LinearMomentumNorm = original.LinearMomentumNorm,
        VMain = original.VMain,
        IRobot = original.IRobot,
        Reserved4 = original.Reserved4,
        Reserved5 = original.Reserved5,

        ToolAccelerometerValues = (double[])original.ToolAccelerometerValues.Clone(),
        ElbowPosition = (double[])original.ElbowPosition.Clone(),
        ElbowVelocity = (double[])original.ElbowVelocity.Clone(),
        QTarget = (double[])original.QTarget.Clone(),
        QdTarget = (double[])original.QdTarget.Clone(),
        QddTarget = (double[])original.QddTarget.Clone(),
        ITarget = (double[])original.ITarget.Clone(),
        QActual = (double[])original.QActual.Clone(),
        QdActual = (double[])original.QdActual.Clone(),
        IActual = (double[])original.IActual.Clone(),
        ToolVectorActual = (double[])original.ToolVectorActual.Clone(),
        TCPSpeedActual = (double[])original.TCPSpeedActual.Clone(),
        TCPForce = (double[])original.TCPForce.Clone(),
        ToolVectorTarget = (double[])original.ToolVectorTarget.Clone(),
        TCPSpeedTarget = (double[])original.TCPSpeedTarget.Clone(),
        MotorTempetatures = (double[])original.MotorTempetatures.Clone(),
        JointModes = (double[])original.JointModes.Clone(),
        VActual = (double[])original.VActual.Clone(),

        Handtype = (byte[])original.Handtype.Clone(),

        User = original.User,
        Tool = original.Tool,
        RunQueuedCmd = original.RunQueuedCmd,
        PauseCmdFlag = original.PauseCmdFlag,
        VelocityRatio = original.VelocityRatio,
        AccelerationRatio = original.AccelerationRatio,
        JerkRatio = original.JerkRatio,
        XYZVelocityRatio = original.XYZVelocityRatio,
        RVelocityRatio = original.RVelocityRatio,
        XYZAccelerationRatio = original.XYZAccelerationRatio,
        RAccelerationRatio = original.RAccelerationRatio,
        XYZJerkRatio = original.XYZJerkRatio,
        RJerkRatio = original.RJerkRatio,

        BrakeStatus = original.BrakeStatus,
        EnableStatus = original.EnableStatus,
        DragStatus = original.DragStatus,
        RunningStatus = original.RunningStatus,
        ErrorStatus = original.ErrorStatus,
        JogStatus = original.JogStatus,
        RobotType = original.RobotType,
        DragButtonSignal = original.DragButtonSignal,
        EnableButtonSignal = original.EnableButtonSignal,
        RecordButtonSignal = original.RecordButtonSignal,
        ReappearButtonSignal = original.ReappearButtonSignal,
        JawButtonSignal = original.JawButtonSignal,
        SixForceOnline = original.SixForceOnline,

        Reserved6 = (byte[])original.Reserved6.Clone(),
        MActual = (double[])original.MActual.Clone(),
        Load = original.Load,
        CenterX = original.CenterX,
        CenterY = original.CenterY,
        CenterZ = original.CenterZ,
        UserValu = (double[])original.UserValu.Clone(),
        Tools = (double[])original.Tools.Clone(),
        TraceIndex = original.TraceIndex,
        SixForceValue = (double[])original.SixForceValue.Clone(),
        TargetQuaternion = (double[])original.TargetQuaternion.Clone(),
        ActualQuaternion = (double[])original.ActualQuaternion.Clone(),
        Reserved7 = (byte[])original.Reserved7.Clone()
      };
    }
  }
}