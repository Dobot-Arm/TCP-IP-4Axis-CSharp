using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTcpDemo.com.dobot.api
{
    class ErrorInfoBean
    {
        public int id { get; set; }
        public int level { get; set; }
        public string Type { get; set; }
        public Description en { get; set; }
        public Description zh_CN { get; set; }
    }

    class Description
    {
        public string description { get; set; }
        public string cause { get; set; }
        public string solution { get; set; }
    }
}
