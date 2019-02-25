using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graphics
{
    public class NFFractal : Fractal
    {
        public Canvas MainCanvas { get; set; }
        private List<PointData> nfFractalPointsList = new List<PointData>();
        public NFFractal(double startLength, Color startColor, Color endColor,
             int maxRec, Canvas canvas) : base(startLength, startColor,
             endColor, maxRec)
        {
            MainCanvas = canvas;
        }
        /// <summary>
        /// Creates a list of all points, which are used in the fractal 
        /// </summary>
        private void FindDots(DrawingParameters drawingParameters, Dispatcher dispatcher)
        {
            try
            {
                NFDrawingParams nfDrawingParams = (NFDrawingParams)drawingParameters;
                Point[] tPointsArr = GetFractalPoints(nfDrawingParams.CurrentCoords,
                        nfDrawingParams.CurrentLength);
                SolidColorBrush brush = GetCurrentColor.Get(StartColor, EndColor,
                        nfDrawingParams.RecursionLevel - 1, MaxRecursionLevel - 1);
                nfFractalPointsList.Add(new NFFractalPointData(tPointsArr, brush));
                if (nfDrawingParams.RecursionLevel < MaxRecursionLevel)
                {
                    //define new drawing params for four new elements and find the data about them
                    NFDrawingParams newDrawingParams = new NFDrawingParams(
                        nfDrawingParams.CurrentLength / 2, nfDrawingParams.RecursionLevel + 1,
                        tPointsArr[2]);
                    FindDots(newDrawingParams, dispatcher);
                    newDrawingParams.CurrentCoords = tPointsArr[3];
                    FindDots(newDrawingParams, dispatcher);
                    newDrawingParams.CurrentCoords = tPointsArr[4];
                    FindDots(newDrawingParams, dispatcher);
                    newDrawingParams.CurrentCoords = tPointsArr[5];
                    FindDots(newDrawingParams, dispatcher);
                }
            }
            catch (OutOfMemoryException ex)
            {
                dispatcher.Invoke(() => MessageBox.Show(ex.Message,
                    "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information));
            }
            catch (StackOverflowException ex)
            {
                dispatcher.Invoke(() => MessageBox.Show(ex.Message,
                    "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information));
            }
            catch (Exception ex)
            {
                dispatcher.Invoke(() => MessageBox.Show(ex.Message,
                    "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information));
            }
        }
        /// <summary>
        /// Draws the farctal itself, based on the list of points we created before
        /// </summary>
        public override async Task<Canvas> Draw(Dispatcher dispatcher, DispatcherPriority priority,
            CancellationToken token, DrawingParameters drawingParameters)
        {
            try
            {
                await dispatcher.BeginInvoke(new Action(() => FindDots(drawingParameters, dispatcher)));
                for (int i = 0; i < nfFractalPointsList.Count; i++)
                {
                    if (token != CancellationToken.None && token.IsCancellationRequested)
                    {
                        throw new TaskCanceledException();
                    }
                    dispatcher.Invoke(new Action(() =>
                    {
                        try
                        {
                            Line horizontalLine = new Line
                            {
                                Stroke = nfFractalPointsList[i].CurrentBrush,
                                X1 = ((NFFractalPointData)nfFractalPointsList[i]).PointsArr[0].X,
                                Y1 = ((NFFractalPointData)nfFractalPointsList[i]).PointsArr[0].Y,
                                X2 = ((NFFractalPointData)nfFractalPointsList[i]).PointsArr[1].X,
                                Y2 = ((NFFractalPointData)nfFractalPointsList[i]).PointsArr[1].Y,
                            };
                            Line leftVerticalLine = new Line
                            {
                                Stroke = nfFractalPointsList[i].CurrentBrush,
                                X1 = ((NFFractalPointData)nfFractalPointsList[i]).PointsArr[2].X,
                                Y1 = ((NFFractalPointData)nfFractalPointsList[i]).PointsArr[2].Y,
                                X2 = ((NFFractalPointData)nfFractalPointsList[i]).PointsArr[3].X,
                                Y2 = ((NFFractalPointData)nfFractalPointsList[i]).PointsArr[3].Y,
                            };
                            Line rightVerticalLine = new Line
                            {
                                Stroke = nfFractalPointsList[i].CurrentBrush,
                                X1 = ((NFFractalPointData)nfFractalPointsList[i]).PointsArr[4].X,
                                Y1 = ((NFFractalPointData)nfFractalPointsList[i]).PointsArr[4].Y,
                                X2 = ((NFFractalPointData)nfFractalPointsList[i]).PointsArr[5].X,
                                Y2 = ((NFFractalPointData)nfFractalPointsList[i]).PointsArr[5].Y,
                            };
                            //add the lines to the canvas
                            MainCanvas.Children.Add(horizontalLine);
                            MainCanvas.Children.Add(leftVerticalLine);
                            MainCanvas.Children.Add(rightVerticalLine);
                        }
                        catch (NullReferenceException ex)
                        {
                            MessageBox.Show(ex.Message, "Soft's message", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        }
                        catch (OutOfMemoryException ex)
                        {
                            MessageBox.Show(ex.Message, "Soft's message", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Soft's message", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        }
                    }), priority);
                }
                return MainCanvas;
            }
            catch (TaskCanceledException)
            {
                dispatcher.Invoke(() => MessageBox.Show("Drawing task " +
                    "was canceled", "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information));
                return null;
            }
            catch (IndexOutOfRangeException ex)
            {
                dispatcher.Invoke(() => MessageBox.Show(ex.Message,
                    "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information));
                return null;
            }
            catch (OutOfMemoryException ex)
            {
                dispatcher.Invoke(() => MessageBox.Show(ex.Message,
                    "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information));
                return null;
            }
            catch (StackOverflowException ex)
            {
                dispatcher.Invoke(() => MessageBox.Show(ex.Message,
                    "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information));
                return null;
            }
            catch (Exception ex)
            {
                dispatcher.Invoke(() => MessageBox.Show(ex.Message,
                    "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information));
                return null;
            }
        }
        /// <summary>
        /// Get the six points, which then will be used when drawing the base element
        /// </summary>
        private Point[] GetFractalPoints(Point startPoint, double currLength)
        {
            return new Point[]
            {
                //horizontal line
                new Point(startPoint.X - currLength / 2, startPoint.Y),
                new Point(startPoint.X + currLength / 2, startPoint.Y),
                //two vertical lines, firstly left line
                new Point(startPoint.X - currLength/2, startPoint.Y - currLength/2),
                new Point(startPoint.X - currLength/2, startPoint.Y + currLength/2),
                //right line
                new Point(startPoint.X + currLength/2, startPoint.Y - currLength/2),
                new Point(startPoint.X + currLength/2, startPoint.Y + currLength/2),
            };
        }
    }
}
