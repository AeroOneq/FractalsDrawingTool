using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;

namespace Graphics
{
    public class StartDraw
    {
        #region Properties
        public Canvas MainCanvas { get; set; }
        public Canvas TempCanvas { get; set; }
        public int CurrentFractal { get; set; }
        public Color StartColor { get; set; }
        public Color EndColor { get; set; }
        public Button GoBtn { get; set; }
        public DispatcherPriority DispatcherPriority { get; set; }
        public bool[] QCSidesDrawability { get; set; }
        #endregion
        /// <summary>
        /// Enables go btn and set the text to "Go!"
        /// </summary>
        public void EnableGoBtn()
        {
            GoBtn.IsEnabled = true;
            GoBtn.Content = "Go!";
        }
        /// <summary>
        /// Disables go btn and set the text to "Drawing..."
        /// </summary>
        public void DisableGoBtn()
        {
            GoBtn.IsEnabled = false;
            GoBtn.Content = "Drawing...";
        }
        /// <summary>
        /// Draws a fractal on the main Canvas using the several pararmeters 
        /// to initalize the drawing process
        /// </summary>
        public async Task<Canvas> DrawFractal(double length, int depth, Point startPoint,
            Dispatcher dispatcher, CancellationToken token)
        {
            try
            {
                Disable();
                switch (CurrentFractal)
                {
                    case 1:
                        QCFractal qcFractal = new QCFractal(length,
                             StartColor, EndColor, depth, MainCanvas);
                        DrawingParameters qcParams = new QCDrawingParams(length, 1,
                             startPoint, QCSidesDrawability);
                        if (DispatcherPriority == DispatcherPriority.Send)
                        {
                            TempCanvas = new Canvas
                            {
                                Width = 5000,
                                Height = 5000,
                                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255))
                            };
                            qcFractal.MainCanvas = TempCanvas;
                            return await Task.Run(() => qcFractal.Draw(dispatcher,
                                DispatcherPriority, token, qcParams));
                        }
                        else
                        {
                            await Task.Run(() => qcFractal.Draw(dispatcher,
                                DispatcherPriority, token, qcParams));
                        }
                        return null;
                    case 2:
                        SCFractal scFractal = new SCFractal(length,
                            StartColor, EndColor,
                            depth, MainCanvas);
                        DrawingParameters scParams = new SCDrawingParams(length, 1,
                             startPoint);
                        scFractal.InitialDraw(scParams);
                        if (DispatcherPriority == DispatcherPriority.Send)
                        {
                            TempCanvas = new Canvas
                            {
                                Width = 5000,
                                Height = 5000,
                                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255))
                            };
                            scFractal.MainCanvas = TempCanvas;
                            return await Task.Run(() => scFractal.Draw(dispatcher, DispatcherPriority,
                                token, scParams));
                        }
                        else
                        {
                            await Task.Run(() => scFractal.Draw(dispatcher, DispatcherPriority,
                                token, scParams));
                        }
                        return null;
                    case 3:
                        NFFractal nfFractal = new NFFractal(length,
                            StartColor, EndColor,
                            depth, MainCanvas);
                        DrawingParameters nfParams = new NFDrawingParams(length, 1,
                             startPoint);
                        if (DispatcherPriority == DispatcherPriority.Send)
                        {
                            TempCanvas = new Canvas
                            {
                                Width = 5000,
                                Height = 5000,
                                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255))
                            };
                            nfFractal.MainCanvas = TempCanvas;
                            return await Task.Run(() => nfFractal.Draw(dispatcher, DispatcherPriority,
                                token, nfParams));
                        }
                        else
                        {
                            await Task.Run(() => nfFractal.Draw(dispatcher, DispatcherPriority,
                                token, nfParams));
                        }
                        return null;
                    default:
                        MessageBox.Show("The fatal error happened, this should be never seen...");
                        return null;
                }
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
            catch (IndexOutOfRangeException ex)
            {
                dispatcher.Invoke(() => MessageBox.Show(ex.Message,
                    "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information));
                return null;
            }
            catch (NullReferenceException ex)
            {
                dispatcher.Invoke(() => MessageBox.Show(ex.Message,
                    "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information));
                return null;
            }
            catch (TaskCanceledException ex)
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
            finally
            {
                Enable();
            }
        }
        public void Enable()
        {
            EnableGoBtn();
        }
        public void Disable()
        {
            DisableGoBtn();
        }
    }
}
