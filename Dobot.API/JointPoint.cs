namespace Dobot.API
{
  public class JointPoint
  {
    public JointPoint()
    {
      j1 = j2 = j3 = j4 = j5 = j6 = 0.0;
    }

    /// <summary>
    ///   J1 轴位置，单位：度
    /// </summary>
    public double j1 { get; set; }

    /// <summary>
    ///   J2 轴位置，单位：度
    /// </summary>
    public double j2 { get; set; }

    /// <summary>
    ///   J3 轴位置，单位：度
    /// </summary>
    public double j3 { get; set; }

    /// <summary>
    ///   J4 轴位置，单位：度
    /// </summary>
    public double j4 { get; set; }

    /// <summary>
    ///   J5 轴位置，单位：度
    /// </summary>
    public double j5 { get; set; }

    /// <summary>
    ///   J6 轴位置，单位：度
    /// </summary>
    public double j6 { get; set; }

    public override string ToString()
    {
      var str = string.Format("{0},{1},{2},{3},{4},{5}",
        j1, j2, j3, j4, j5, j6);
      return str;
    }
  }
}