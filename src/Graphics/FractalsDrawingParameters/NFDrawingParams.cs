using System.Windows;

namespace Graphics
{
    public class NFDrawingParams : DrawingParameters
    {
        public NFDrawingParams(double currentLength, int recLvl,
          Point currPoint) : base(currentLength,
            recLvl, currPoint) { }
    }
}
