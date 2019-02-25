using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graphics
{
    public class QCFractal : Fractal
    {
        public Canvas MainCanvas { get; set; }
        List<PointData> pointsList = new List<PointData>();
        public QCFractal(double startLength, Color startColor, Color endColor,
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
                QCDrawingParams qcDrawingParams = (QCDrawingParams)drawingParameters;
                double currX = drawingParameters.CurrentCoords.X - qcDrawingParams.CurrentLength;
                double currY = drawingParameters.CurrentCoords.Y - qcDrawingParams.CurrentLength;
                double length = qcDrawingParams.CurrentLength;
                SolidColorBrush brush = GetCurrentColor.Get(StartColor, EndColor,
                        drawingParameters.RecursionLevel - 1, MaxRecursionLevel - 1);
                pointsList.Add(new PointData(length, brush, new Point(currX, currY)));
                QCDrawingParams newQCParams = new QCDrawingParams(qcDrawingParams.CurrentLength / 2,
                        qcDrawingParams.RecursionLevel + 1, new Point(0, 0), new bool[] { });
                if (drawingParameters.RecursionLevel < MaxRecursionLevel)
                {
                    bool[][] newSidesDrawability = GetNewSidesDrawability(qcDrawingParams.SidesDrawability);
                    for (int i = 0; i < qcDrawingParams.SidesDrawability.Length; i++)
                    {
                        if (qcDrawingParams.SidesDrawability[i])
                        {
                            newQCParams.CurrentCoords = GetNewPoint(qcDrawingParams.CurrentCoords,
                                qcDrawingParams.CurrentLength, i);
                            newQCParams.SidesDrawability = newSidesDrawability[i];
                            FindDots(newQCParams, dispatcher);
                        }
                    }
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
                            //draw. firstly create an ellipse with the given size
                            Ellipse ellipse = new Ellipse
                            {
                                Width = pointsList[i].Length * 2,
                                Height = pointsList[i].Length * 2,
                                Fill = pointsList[i].CurrentBrush
                            };
                            //set the position of the created ellipse and add attach to the parent
                            Canvas.SetLeft(ellipse, pointsList[i].Coords.X);
                            Canvas.SetTop(ellipse, pointsList[i].Coords.Y);
                            MainCanvas.Children.Add(ellipse);
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
        /// Creates four new arrays with new sides drawability for each side
        /// </summary>
        /// <param name="sidesDrawability">
        /// Old drawability boolean array
        /// </param>
        public bool[][] GetNewSidesDrawability(bool[] sidesDrawability)
        {
            bool[][] newArr = new bool[4][];
            if (sidesDrawability[0])
            {
                newArr[0] = new bool[] { true, true, false, true };
            }
            if (sidesDrawability[1])
            {
                newArr[1] = new bool[] { true, true, true, false };
            }
            if (sidesDrawability[2])
            {
                newArr[2] = new bool[] { false, true, true, true };
            }
            if (sidesDrawability[3])
            {
                newArr[3] = new bool[] { true, false, true, true };
            }
            return newArr;
        }
        /// <summary>
        /// Gets a point where the next fractal base element will be drawn
        /// </summary>
        public Point GetNewPoint(Point oldPoint, double oldLength, int elementNum)
        {
            switch (elementNum)
            {
                case 0:
                    double x = oldPoint.X - 1.5 * oldLength;
                    return new Point(x, oldPoint.Y);
                case 1:
                    double y = oldPoint.Y - 1.5 * oldLength;
                    return new Point(oldPoint.X, y);
                case 2:
                    x = oldPoint.X + 1.5 * oldLength;
                    return new Point(x, oldPoint.Y);
                case 3:
                    y = oldPoint.Y + 1.5 * oldLength;
                    return new Point(oldPoint.X, y);
                default:
                    MessageBox.Show("The fatal error, this should have never happened, " +
                        "restart the app.", "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                    return new Point(0, 0);
            }
        }
    }
}
