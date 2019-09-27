using System.Windows;

namespace Graphics
{
    /// <summary>
    /// Parent class to all drawing parameters classes 
    /// which are transmited to the Draw method
    /// </summary>
    public class DrawingParameters
    {
        public double CurrentLength { get; }
        public int RecursionLevel { get; }
        public Point CurrentCoords { get; set; }

        public DrawingParameters(double currentLength,
            int recLvl, Point currPoint)
        {
            CurrentLength = currentLength;
            RecursionLevel = recLvl;
            CurrentCoords = currPoint;
        }
    }
}
