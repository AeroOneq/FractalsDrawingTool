using System.Windows.Media;
using System.Windows;


namespace Graphics
{
    public class PointData
    {
        public double Length { get; set; } 
        public SolidColorBrush CurrentBrush{ get; set; } 
        public Point Coords { get; set; }

        public PointData(double length, SolidColorBrush brush, Point point)
        {
            Length = length;
            CurrentBrush = brush;
            Coords = point;
        }
    }
}
