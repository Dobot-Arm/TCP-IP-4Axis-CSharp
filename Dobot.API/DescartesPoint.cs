using System;

namespace Dobot.API
{
  public class DescartesPoint
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

    public DescartesPoint()
    {
      x = y = z = rx = ry = rz = 0.0;
    }

    override public string ToString()
    {
      string str = String.Format("{0},{1},{2},{3},{4},{5}",
          this.x, this.y, this.z, this.rx, this.ry, this.rz);
      return str;
    }
  }
}
