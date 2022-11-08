using System;
using System.Net.Sockets;

namespace Dobot.API
{
  public class Dashboard : DobotClient
  {
    protected override void OnConnected(Socket sock)
    {
      sock.SendTimeout = 5000;
      sock.ReceiveTimeout = 5000;
    }

    protected override void OnDisconnected()
    {
    }
    /// <summary>
    /// 复位，用于清除错误
    /// </summary>
    /// <returns>返回执行结果的描述信息</returns>
    public string ClearError()
    {
      if (!IsConnected())
      {
        return "device does not connected!!!";
      }

      string str = "ClearError()";
      if (!SendData(str))
      {
        return str + ":send error";
      }

      return WaitReply(5000);
    }

    /// <summary>
    /// 机器人上电
    /// </summary>
    /// <returns>返回执行结果的描述信息</returns>
    public string PowerOn()
    {
      if (!IsConnected())
      {
        return "device does not connected!!!";
      }

      string str = "PowerOn()";
      if (!SendData(str))
      {
        return str + ":send error";
      }

      return WaitReply(15000);
    }

    /// <summary>
    /// 急停
    /// </summary>
    /// <returns>返回执行结果的描述信息</returns>
    public string PowerOff()
    {
      return EmergencyStop();
    }

    /// <summary>
    /// 急停
    /// </summary>
    /// <returns>返回执行结果的描述信息</returns>
    public string EmergencyStop()
    {
      if (!IsConnected())
      {
        return "device does not connected!!!";
      }

      string str = "EmergencyStop()";
      if (!SendData(str))
      {
        return str + ":send error";
      }

      return WaitReply(15000);
    }

    /// <summary>
    /// 使能机器人
    /// </summary>
    /// <returns>返回执行结果的描述信息</returns>
    public string EnableRobot()
    {
      if (!IsConnected())
      {
        return "device does not connected!!!";
      }

      string str = "EnableRobot()";
      if (!SendData(str))
      {
        return str + ":send error";
      }

      return WaitReply(20000);
    }

    /// <summary>
    /// 下使能机器人
    /// </summary>
    /// <returns>返回执行结果的描述信息</returns>
    public string DisableRobot()
    {
      if (!IsConnected())
      {
        return "device does not connected!!!";
      }

      string str = "DisableRobot()";
      if (!SendData(str))
      {
        return str + ":send error";
      }

      return WaitReply(20000);
    }

    /// <summary>
    /// 机器人停止
    /// </summary>
    /// <returns>返回执行结果的描述信息</returns>
    public string ResetRobot()
    {
      if (!IsConnected())
      {
        return "device does not connected!!!";
      }

      string str = "ResetRobot()";
      if (!SendData(str))
      {
        return str + ":send error";
      }

      return WaitReply(20000);
    }

    /// <summary>
    /// 设置全局速度比例。
    /// </summary>
    /// <param name="ratio">运动速度比例，取值范围：1~100</param>
    /// <returns>返回执行结果的描述信息</returns>
    public string SpeedFactor(int ratio)
    {
      if (!IsConnected())
      {
        return "device does not connected!!!";
      }

      string str = String.Format("SpeedFactor({0})", ratio);
      if (!SendData(str))
      {
        return str + ":send error";
      }

      return WaitReply(5000);
    }

    /// <summary>
    /// 设置数字输出端口状态（队列指令）
    /// </summary>
    /// <param name="index">数字输出索引，取值范围：1~16或100~1000</param>
    /// <param name="status">数字输出端口状态，true：高电平；false：低电平</param>
    /// <returns>返回执行结果的描述信息</returns>
    public string DigitalOutputs(int index, bool status)
    {
      if (!IsConnected())
      {
        return "device does not connected!!!";
      }

      string str = String.Format("DO({0},{1})", index, status ? 1 : 0);
      if (!SendData(str))
      {
        return str + ":send error";
      }

      return WaitReply(5000);
    }

    public string GetErrorID()
    {
      if (!IsConnected())
      {
        return "device does not connected!!!";
      }

      string str = "GetErrorID()";
      if (!SendData(str))
      {
        return str + ":send error";
      }

      return WaitReply(5000);
    }
  }
}
