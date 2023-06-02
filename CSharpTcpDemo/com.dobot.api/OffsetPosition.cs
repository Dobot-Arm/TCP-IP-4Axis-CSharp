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
        public double rx { get; set; }

        /// <summary>
        /// Ry 轴位置，单位：度
        /// </summary>
        public double ry { get; set; }

        /// <summary>
        /// Rz 轴位置，单位：度
        /// </summary>
        public double rz { get; set; }

        /// <summary>
        /// 选择已标定的用户坐标系，取值范围：0~9
        /// </summary>
        public int user { get; set; }

        public OffsetPosition()
        {
            x = y = z = rx = ry = rz = 0.0;
            user = 0;
        }

        override public string ToString()
        {
            string str = String.Format("{0},{1},{2},{3},{4},{5}",
                this.x, this.y, this.z, this.rx, this.ry, this.rz);
            return str;
        }
    }
}
