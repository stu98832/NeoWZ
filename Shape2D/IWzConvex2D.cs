namespace NeoMS.Wz.Shape2D
{
    /// <summary>  </summary>
    public interface IWzConvex2D : IWzShape2D
    {
        /// <summary>  </summary>
        int Count { get; }

        /// <summary>  </summary>
        IWzVector2D this[int index] { get; }
    }
}
