using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

namespace Dobot.API
{
  public class Feedback : DobotClient
  {
    public delegate void NewFeedbackEventHandler(object sender, FeedbackEventArgs feedback);

    private Thread mThread;

    public Feedback()
    {
      feedbackData = new FeedbackData();
    }

    public FeedbackData feedbackData { get; }

    public bool DataHasRead { get; set; }

    public event NewFeedbackEventHandler OnNewFeedbackData;

    protected override void OnConnected(Socket sock)
    {
      sock.SendTimeout = 5000;
      sock.ReceiveTimeout = 15000;

      mThread = new Thread(OnRecvData);
      mThread.IsBackground = true;
      mThread.Start();
    }

    protected override void OnDisconnected()
    {
      if (null != mThread && mThread.IsAlive)
        try
        {
          mThread.Abort();
          mThread = null;
        }
        catch (Exception ex)
        {
          Debug.WriteLine("close thread:" + ex);
        }
    }

    /// <summary>
    ///   接收返回的数据并解析处理
    /// </summary>
    private void OnRecvData()
    {
      var buffer = new byte[4320]; //1440*3
      var iHasRead = 0;
      while (IsConnected())
        try
        {
          var iRet = Receive(buffer, iHasRead, buffer.Length - iHasRead, SocketFlags.None);
          if (iRet <= 0) continue;
          iHasRead += iRet;
          if (iHasRead < 1440) continue;

          var bHasFound = false; //是否找到数据包头了
          for (var i = 0; i < iHasRead; ++i)
          {
            //找到消息头
            int iMsgSize = buffer[i + 1];
            iMsgSize <<= 8;
            iMsgSize |= buffer[i];
            iMsgSize &= 0x00FFFF;
            if (1440 != iMsgSize) continue;
            //校验
            var checkValue = BitConverter.ToUInt64(buffer, i + 48);
            if (0x0123456789ABCDEF == checkValue)
            {
              //找到了校验值
              bHasFound = true;
              if (i != 0)
              {
                //说明存在粘包，要把前面的数据清理掉
                iHasRead = iHasRead - i;
                Array.Copy(buffer, i, buffer, 0, buffer.Length - i);
              }

              break;
            }
          }

          if (!bHasFound)
          {
            //如果没找到头，判断数据长度是不是快超过了总长度，超过了，说明数据全都有问题，删掉
            if (iHasRead >= buffer.Length) iHasRead = 0;
            continue;
          }

          //再次判断字节数是否够
          if (iHasRead < 1440) continue;
          iHasRead = iHasRead - 1440;
          //按照协议的格式解析数据
          ParseData(buffer);
          Array.Copy(buffer, 1440, buffer, 0, buffer.Length - 1440);
        }
        catch (Exception ex)
        {
          Debug.WriteLine("recv thread:" + ex);
        }
    }

    /// <summary>
    ///   解析数据
    /// </summary>
    /// <param name="buffer">一包完整的数据</param>
    private void ParseData(byte[] buffer)
    {
      var iStartIndex = 0;

      feedbackData.MessageSize = BitConverter.ToInt16(buffer, iStartIndex); //unsigned short
      iStartIndex += 2;

      for (var i = 0; i < feedbackData.Reserved1.Length; ++i)
      {
        feedbackData.Reserved1[i] = BitConverter.ToInt16(buffer, iStartIndex);
        iStartIndex += 2;
      }

      feedbackData.DigitalInputs = BitConverter.ToInt64(buffer, iStartIndex);
      iStartIndex += 8;

      feedbackData.DigitalOutputs = BitConverter.ToInt64(buffer, iStartIndex);
      iStartIndex += 8;

      feedbackData.RobotMode = BitConverter.ToInt64(buffer, iStartIndex);
      iStartIndex += 8;

      feedbackData.TimeStamp = BitConverter.ToInt64(buffer, iStartIndex);
      iStartIndex += 8;

      feedbackData.Reserved2 = BitConverter.ToInt64(buffer, iStartIndex);
      iStartIndex += 8;

      feedbackData.TestValue = BitConverter.ToInt64(buffer, iStartIndex);
      iStartIndex += 8;

      feedbackData.Reserved3 = BitConverter.ToInt64(buffer, iStartIndex);
      iStartIndex += 8;

      feedbackData.SpeedScaling = BitConverter.ToDouble(buffer, iStartIndex);
      iStartIndex += 8;

      feedbackData.LinearMomentumNorm = BitConverter.ToDouble(buffer, iStartIndex);
      iStartIndex += 8;

      feedbackData.VMain = BitConverter.ToDouble(buffer, iStartIndex);
      iStartIndex += 8;

      feedbackData.VRobot = BitConverter.ToDouble(buffer, iStartIndex);
      iStartIndex += 8;

      feedbackData.IRobot = BitConverter.ToDouble(buffer, iStartIndex);
      iStartIndex += 8;

      feedbackData.Reserved4 = BitConverter.ToDouble(buffer, iStartIndex);
      iStartIndex += 8;

      feedbackData.Reserved5 = BitConverter.ToDouble(buffer, iStartIndex);
      iStartIndex += 8;

      for (var i = 0; i < feedbackData.ToolAccelerometerValues.Length; ++i)
      {
        feedbackData.ToolAccelerometerValues[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.ElbowPosition.Length; ++i)
      {
        feedbackData.ElbowPosition[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.ElbowVelocity.Length; ++i)
      {
        feedbackData.ElbowVelocity[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.QTarget.Length; ++i)
      {
        feedbackData.QTarget[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.QdTarget.Length; ++i)
      {
        feedbackData.QdTarget[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.QddTarget.Length; ++i)
      {
        feedbackData.QddTarget[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.ITarget.Length; ++i)
      {
        feedbackData.ITarget[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.MTarget.Length; ++i)
      {
        feedbackData.MTarget[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.QActual.Length; ++i)
      {
        feedbackData.QActual[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.QdActual.Length; ++i)
      {
        feedbackData.QdActual[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.IActual.Length; ++i)
      {
        feedbackData.IActual[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.IControl.Length; ++i)
      {
        feedbackData.IControl[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.ToolVectorActual.Length; ++i)
      {
        feedbackData.ToolVectorActual[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.TCPSpeedActual.Length; ++i)
      {
        feedbackData.TCPSpeedActual[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.TCPForce.Length; ++i)
      {
        feedbackData.TCPForce[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.ToolVectorTarget.Length; ++i)
      {
        feedbackData.ToolVectorTarget[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.TCPSpeedTarget.Length; ++i)
      {
        feedbackData.TCPSpeedTarget[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.MotorTempetatures.Length; ++i)
      {
        feedbackData.MotorTempetatures[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.JointModes.Length; ++i)
      {
        feedbackData.JointModes[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.VActual.Length; ++i)
      {
        feedbackData.VActual[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.Handtype.Length; ++i)
      {
        feedbackData.Handtype[i] = buffer[iStartIndex];
        iStartIndex += 1;
      }

      feedbackData.User = buffer[iStartIndex];
      iStartIndex += 1;

      feedbackData.Tool = buffer[iStartIndex];
      iStartIndex += 1;

      feedbackData.RunQueuedCmd = buffer[iStartIndex];
      iStartIndex += 1;

      feedbackData.PauseCmdFlag = buffer[iStartIndex];
      iStartIndex += 1;

      feedbackData.VelocityRatio = buffer[iStartIndex];
      iStartIndex += 1;

      feedbackData.AccelerationRatio = buffer[iStartIndex];
      iStartIndex += 1;

      feedbackData.JerkRatio = buffer[iStartIndex];
      iStartIndex += 1;

      feedbackData.XYZVelocityRatio = buffer[iStartIndex];
      iStartIndex += 1;

      feedbackData.RVelocityRatio = buffer[iStartIndex];
      iStartIndex += 1;

      feedbackData.XYZAccelerationRatio = buffer[iStartIndex];
      iStartIndex += 1;

      feedbackData.RAccelerationRatio = buffer[iStartIndex];
      iStartIndex += 1;

      feedbackData.XYZJerkRatio = buffer[iStartIndex];
      iStartIndex += 1;

      feedbackData.RJerkRatio = buffer[iStartIndex];
      iStartIndex += 1;

      feedbackData.BrakeStatus = buffer[iStartIndex];
      iStartIndex += 1;
      feedbackData.EnableStatus = buffer[iStartIndex];
      iStartIndex += 1;
      feedbackData.DragStatus = buffer[iStartIndex];
      iStartIndex += 1;
      feedbackData.RunningStatus = buffer[iStartIndex];
      iStartIndex += 1;
      feedbackData.ErrorStatus = buffer[iStartIndex];
      iStartIndex += 1;
      feedbackData.JogStatus = buffer[iStartIndex];
      iStartIndex += 1;
      feedbackData.RobotType = buffer[iStartIndex];
      iStartIndex += 1;
      feedbackData.DragButtonSignal = buffer[iStartIndex];
      iStartIndex += 1;
      feedbackData.EnableButtonSignal = buffer[iStartIndex];
      iStartIndex += 1;
      feedbackData.RecordButtonSignal = buffer[iStartIndex];
      iStartIndex += 1;
      feedbackData.ReappearButtonSignal = buffer[iStartIndex];
      iStartIndex += 1;
      feedbackData.JawButtonSignal = buffer[iStartIndex];
      iStartIndex += 1;
      feedbackData.SixForceOnline = buffer[iStartIndex];
      iStartIndex += 1;

      for (var i = 0; i < feedbackData.Reserved6.Length; ++i)
      {
        feedbackData.Reserved6[i] = buffer[iStartIndex];
        iStartIndex += 1;
      }

      for (var i = 0; i < feedbackData.MActual.Length; ++i)
      {
        feedbackData.MActual[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      feedbackData.Load = BitConverter.ToDouble(buffer, iStartIndex);
      iStartIndex += 8;

      feedbackData.CenterX = BitConverter.ToDouble(buffer, iStartIndex);
      iStartIndex += 8;

      feedbackData.CenterY = BitConverter.ToDouble(buffer, iStartIndex);
      iStartIndex += 8;

      feedbackData.CenterZ = BitConverter.ToDouble(buffer, iStartIndex);
      iStartIndex += 8;

      for (var i = 0; i < feedbackData.UserValu.Length; ++i)
      {
        feedbackData.UserValu[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.Tools.Length; ++i)
      {
        feedbackData.Tools[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      feedbackData.TraceIndex = BitConverter.ToDouble(buffer, iStartIndex);
      iStartIndex += 8;

      for (var i = 0; i < feedbackData.SixForceValue.Length; ++i)
      {
        feedbackData.SixForceValue[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.TargetQuaternion.Length; ++i)
      {
        feedbackData.TargetQuaternion[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.ActualQuaternion.Length; ++i)
      {
        feedbackData.ActualQuaternion[i] = BitConverter.ToDouble(buffer, iStartIndex);
        iStartIndex += 8;
      }

      for (var i = 0; i < feedbackData.Reserved7.Length; ++i)
      {
        feedbackData.Reserved7[i] = buffer[iStartIndex];
        iStartIndex += 1;
      }

      DataHasRead = true;
      OnNewFeedbackData?.Invoke(this, new FeedbackEventArgs(feedbackData.Clone()));
    }

    public string ConvertRobotMode()
    {
      switch (feedbackData.RobotMode)
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
      }

      return string.Format("UNKNOWN：RobotMode={0}", feedbackData.RobotMode);
    }
  }

  public class FeedbackEventArgs : EventArgs
  {
    public FeedbackEventArgs(FeedbackData feedback)
    {
      Feedback = feedback;
    }

    public FeedbackData Feedback { get; }
  }
}