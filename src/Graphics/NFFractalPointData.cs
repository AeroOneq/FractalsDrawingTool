using System.Windows;
using System.Windows.Media;

namespace Graphics
{
    public class NFFractalPointData : PointData
    {
        public Point[] PointsArr { get; set; }

        public NFFractalPointData(Point[] points, SolidColorBrush brush)
            : base(0, brush, new Point())
        {
            PointsArr = points;
        }
    }
}
