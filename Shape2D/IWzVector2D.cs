namespace NeoMS.Wz.Shape2D
{
    /// <summary> Wizet 2D Vector Interface </summary>
    public interface IWzVector2D : IWzShape2D
    {
        /// <summary> Get or set X value of this object </summary>
        int X { get; set; }

        /// <summary> Get or set Y value of this object </summary>
        int Y { get; set; }
    }
}
