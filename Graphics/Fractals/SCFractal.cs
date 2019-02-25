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
    public class SCFractal : Fractal
    {
        public Canvas MainCanvas { get; set; }
        public List<PointData> pointsList = new List<PointData>();
        public SCFractal(double startLength, Color startColor, Color endColor,
             int maxRec, Canvas canvas) : base(startLength, startColor,
             endColor, maxRec)
        {
            MainCanvas = canvas;
        }
        /// <summary>
        /// Draws the first main triangle
        /// </summary>
        public void InitialDraw(DrawingParameters drawingParameters)
        {
            SCDrawingParams scDrawingParams = (SCDrawingParams)drawingParameters;
            Point[] tPointsArr = GetTrianglePoints(scDrawingParams.CurrentCoords,
                scDrawingParams.CurrentLength);
            PointCollection tPointsCollection = new PointCollection(tPointsArr);
            Polyline line = new Polyline
            {
                Points = tPointsCollection,
                StrokeThickness = 0.3,
                Stroke = GetCurrentColor.Get(StartColor, EndColor,
                         scDrawingParams.RecursionLevel, MaxRecursionLevel)
            };
            MainCanvas.Children.Add(line);
        }
        /// <summary>
        /// Creates a list of all points, which are used in the fractal 
        /// </summary>
        private void FindDots(DrawingParameters drawingParameters, Dispatcher dispatcher)
        {
            try
            {
                SCDrawingParams scDrawingParams = (SCDrawingParams)drawingParameters;
                double currX = drawingParameters.CurrentCoords.X;
                double currY = drawingParameters.CurrentCoords.Y;
                double side = scDrawingParams.CurrentLength;
                Point centerPoint = new Point(currX, currY);
                SolidColorBrush brush = GetCurrentColor.Get(StartColor, EndColor,
                        drawingParameters.RecursionLevel - 1, MaxRecursionLevel - 1);
                pointsList.Add(new PointData(side, brush, new Point(currX, currY)));
                SCDrawingParams newQCParams = new SCDrawingParams(scDrawingParams.CurrentLength / 2,
                        scDrawingParams.RecursionLevel + 1, new Point(0, 0));
                if (scDrawingParams.RecursionLevel < MaxRecursionLevel)
                {
                    SCDrawingParams newDrawingParams = new SCDrawingParams(side / 2,
                        scDrawingParams.RecursionLevel + 1,
                        new Point(centerPoint.X, centerPoint.Y - 
                            side * Math.Sqrt(3) / 8));
                    FindDots(newDrawingParams, dispatcher);
                    newDrawingParams = new SCDrawingParams(side / 2,
                        scDrawingParams.RecursionLevel + 1,
                        new Point(centerPoint.X - side / 4.0, centerPoint.Y + 
                            side * Math.Sqrt(3) / 8));
                    FindDots(newDrawingParams, dispatcher);
                    newDrawingParams = new SCDrawingParams(side / 2,
                        scDrawingParams.RecursionLevel + 1,
                        new Point(centerPoint.X + side / 4.0, centerPoint.Y + 
                            side * Math.Sqrt(3) / 8));
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
        public async override Task<Canvas> Draw(Dispatcher dispatcher, DispatcherPriority priority,
            CancellationToken token, DrawingParameters drawingParameters)
        {
            try
            {
                await dispatcher.BeginInvoke(new Action(() => FindDots(drawingParameters, dispatcher)));
                await dispatcher.BeginInvoke(new Action(() => InitialDraw(drawingParameters)));
                for (int i = 0; i < pointsList.Count; i++)
                {
                    if (token != CancellationToken.None && token.IsCancellationRequested)
                    {
                        throw new TaskCanceledException();
                    }
                    dispatcher.Invoke(new Action(() =>
                    {
                        try
                        {
                            Point centerPoint = pointsList[i].Coords;
                            double side = pointsList[i].Length;
                            Point[] tPointsArr = new Point[]
                            {
                            new Point(centerPoint.X - side/4, centerPoint.Y - 0.3),
                            new Point(centerPoint.X + side/4, centerPoint.Y - 0.3),
                            new Point(centerPoint.X, centerPoint.Y + side * Math.Sqrt(3) / 4 - 0.3),
                            new Point(centerPoint.X - side/4, centerPoint.Y - 0.3),
                            };
                            PointCollection tPointsCollection = new PointCollection(tPointsArr);
                            Polyline line = new Polyline
                            {
                                Points = tPointsCollection,
                                StrokeThickness = 0.3,
                                Stroke = pointsList[i].CurrentBrush
                            };
                            MainCanvas.Children.Add(line);
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
                dispatcher.Invoke(() => MessageBox.Show(ex.Message));
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
        public Point[] GetTrianglePoints(Point centerPoint, double sideLength)
        {
            double median = sideLength * Math.Sqrt(3) / 2;
            return new Point[]
            {
                new Point(centerPoint.X, centerPoint.Y - median / 2.0),
                new Point(centerPoint.X - (sideLength / 2.0), centerPoint.Y + median / 2.0),
                new Point(centerPoint.X + (sideLength / 2.0), centerPoint.Y + median / 2.0),
                new Point(centerPoint.X, centerPoint.Y - median / 2.0)
            };
        }
    }
}
