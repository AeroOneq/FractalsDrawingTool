using System.Windows;

namespace Graphics
{
    public class QCDrawingParams : DrawingParameters
    {
        public bool[] SidesDrawability { get; set; }
        //initialize all properties
        public QCDrawingParams(double currentLength,
            int recLvl, Point currPoint, 
            bool[] sidesDrawability) : base(currentLength, recLvl, currPoint)
        {
            SidesDrawability = sidesDrawability;
        }
    }
}
