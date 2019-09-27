using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;

namespace Graphics
{
    abstract public class Fractal
    {
        #region Private variables
        //length of the start element (radius, segment)
        public double StartLength { get; } 
        public Color StartColor { get; }
        public Color EndColor { get; }
        public int MaxRecursionLevel { get; }
        #endregion
        public Fractal(double startLength, Color startColor, Color endColor, int maxRec)
        {
            StartLength = startLength;
            StartColor = startColor;
            EndColor = endColor;
            MaxRecursionLevel = maxRec;
        }
        public abstract Task<Canvas> Draw(Dispatcher dispatcher, DispatcherPriority priority,
            CancellationToken token, DrawingParameters drawingParameters);
    }
}
