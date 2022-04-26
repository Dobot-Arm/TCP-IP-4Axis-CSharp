using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CSharpTcpDemo.com.dobot.api
{
    class ErrorInfoHelper
    {
        private static Dictionary<int, ErrorInfoBean> mControllerBeans = new Dictionary<int, ErrorInfoBean>();
        private static Dictionary<int, ErrorInfoBean> mServoBeans = new Dictionary<int, ErrorInfoBean>();
        public static void ParseControllerJsonFile(string strFullFile)
        {
            try
            {
                string strJson = System.IO.File.ReadAllText(strFullFile);
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
                string strJson = System.IO.File.ReadAllText(strFullFile);
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
            if (mControllerBeans.ContainsKey(id))
            {
                return mControllerBeans[id];
            }
            return null;
        }
        public static ErrorInfoBean FindServo(int id)
        {
            if (mServoBeans.ContainsKey(id))
            {
                return mServoBeans[id];
            }
            return null;
        }
    }
}
