using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Dobot.API
{
  public class ErrorInfoHelper
  {
    private static readonly Dictionary<int, ErrorInfoBean> mControllerBeans = new Dictionary<int, ErrorInfoBean>();
    private static readonly Dictionary<int, ErrorInfoBean> mServoBeans = new Dictionary<int, ErrorInfoBean>();

    public static void ParseControllerJsonFile(string strFullFile)
    {
      try
      {
        var strJson = File.ReadAllText(strFullFile);
        List<ErrorInfoBean> result = JsonConvert.DeserializeObject<List<ErrorInfoBean>>(strJson);
        foreach (var bean in result)
        {
          bean.Type = "Controller";
          mControllerBeans.Add(bean.id, bean);
        }
      }
      catch (Exception ex)
      {
      }
    }

    public static void ParseServoJsonFile(string strFullFile)
    {
      try
      {
        var strJson = File.ReadAllText(strFullFile);
        List<ErrorInfoBean> result = JsonConvert.DeserializeObject<List<ErrorInfoBean>>(strJson);
        foreach (var bean in result)
        {
          bean.Type = "Servo";
          mServoBeans.Add(bean.id, bean);
        }
      }
      catch (Exception ex)
      {
      }
    }

    public static ErrorInfoBean FindController(int id)
    {
      if (mControllerBeans.ContainsKey(id)) return mControllerBeans[id];
      return null;
    }

    public static ErrorInfoBean FindServo(int id)
    {
      if (mServoBeans.ContainsKey(id)) return mServoBeans[id];
      return null;
    }
  }
}