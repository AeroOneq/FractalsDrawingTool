using System.Windows.Media;

namespace Graphics
{
    public static class GetCurrentColor
    {
        /// <summary>
        /// Calculates delta which is then added to a minimum color (R, G, B) value
        /// </summary>
        /// <param name="cMax">
        /// Max value of R or G or B in the "end color"
        /// </param>
        /// <param name="cMin">
        /// Min value of R or G or B in the "start" color
        /// </param>
        private static int GetDelta(int cMin, int cMax, int currRec, int size)
        {
            return (int)((double)(cMax - cMin) * currRec / size);
        }
        /// <summary>
        /// Creates a new solid color brush which is initialized with the color
        /// which is calculated with a linear gradient formula
        /// </summary>
        public static SolidColorBrush Get(Color startColor, Color endColor,
            int currRecNum, int maxRecNum)
        {
                byte rAverage = (byte)(startColor.R + GetDelta(startColor.R, endColor.R,
                    currRecNum, maxRecNum));
                byte gAverage = (byte)(startColor.G + GetDelta(startColor.G, endColor.G,
                    currRecNum, maxRecNum));
                byte bAverage = (byte)(startColor.B + GetDelta(startColor.B, endColor.B,
                    currRecNum, maxRecNum));
                return new SolidColorBrush(Color.FromRgb(rAverage, gAverage, bAverage));
        }
    }
}
