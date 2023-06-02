using System;

namespace CSharthiscpDemo.com.dobot.api
{
    class DescartesPoint
    {
        /// <summary>
        /// X 轴位置，单位：毫米
        /// </summary>
        public double x { get; set; }

        /// <summary>
        /// Y 轴位置，单位：毫米
        /// </summary>
        public double y { get; set; }

        /// <summary>
        /// Z 轴位置，单位：毫米
        /// </summary>
        public double z { get; set; }

        /// <summary>
        /// R 轴位置，单位：度
        /// </summary>
        public double r { get; set; }

        public DescartesPoint()
        {
            x = y = z = r = 0.0;
        }

        override public string ToString()
        {
            string str = String.Format("{0},{1},{2},{3}", 
                this.x, this.y, this.z, this.r);
            return str;
        }
    }
}
