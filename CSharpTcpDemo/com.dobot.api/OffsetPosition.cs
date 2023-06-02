using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTcpDemo.com.dobot.api
{
    class OffsetPosition
    {
        /// <summary>
        /// X轴方向偏移，单位：mm
        /// </summary>
        public double x { get; set; }

        /// <summary>
        /// Y轴方向偏移，单位：mm
        /// </summary>
        public double y { get; set; }

        /// <summary>
        /// Z轴方向偏移，单位：mm
        /// </summary>
        public double z { get; set; }

        /// <summary>
        /// Rx 轴位置，单位：度
        /// </summary>
        public double r { get; set; }

        /// <summary>
        /// 选择已标定的用户坐标系，取值范围：0~9
        /// </summary>
        public int user { get; set; }

        public OffsetPosition()
        {
            x = y = z = r = 0.0;
            user = 0;
        }

        override public string ToString()
        {
            string str = String.Format("{0},{1},{2},{3}",
                this.x, this.y, this.z, this.r);
            return str;
        }
    }
}
