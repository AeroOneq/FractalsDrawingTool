using System.Windows;

namespace Graphics
{
    public class SCDrawingParams : DrawingParameters
    {
        public SCDrawingParams(double currentLength, int recLvl,
         Point currPoint) : base(currentLength,
            recLvl, currPoint)
        { }
    }
}
