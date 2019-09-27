using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;

namespace Graphics
{
    /// <summary>
    /// Class where all initial positions and sizes are set
    /// (also this class is used when the size of the main window is changed)
    /// </summary>
    public class InitializeMainGrids
    {
        private Window MainWindow { get; }
        Dispatcher Dispatcher { get; set; } = Dispatcher.CurrentDispatcher;
        public InitializeMainGrids(Window mainWindow)
        {
            MainWindow = mainWindow;
            MainWindow.Height = SystemParameters.MaximizedPrimaryScreenHeight;
            MainWindow.Width = SystemParameters.MaximizedPrimaryScreenWidth;
            MainWindow.Top = 0;
            MainWindow.Left = 0;
        }
        /// <summary>
        /// Initializes the position and size of three main grids
        /// </summary>
        public void DoGridInitialization(Grid headerGrid, Grid leftGrid, Grid mainGrid,
            Grid topLineGrid, Grid leftLineGrid)
        {
            //main three grids
            headerGrid.Width = MainWindow.Width;
            leftGrid.Height = MainWindow.Height - headerGrid.Height;
            mainGrid.Margin = new Thickness(leftGrid.Width, headerGrid.Height, 0, 0);
            mainGrid.Width = MainWindow.Width - leftGrid.Width;
            mainGrid.Height = MainWindow.Height - headerGrid.Height;
            //devision lines grids
            topLineGrid.Width = MainWindow.Width;
            leftLineGrid.Height = leftGrid.Height;
        }
        /// <summary>
        /// Draw fractals in the menu's cells
        /// </summary>
        public void DrawMenuItems(Canvas qcFractalCanvas, Canvas nfFractalCanvas,
            Canvas scFractalCanvas, Color startColor, Color endColor)
        {
            DrawQCMenuFractal(qcFractalCanvas, startColor, endColor);
            DrawNFMenuFractal(nfFractalCanvas, startColor, endColor);
            DrawSCMenuFractal(scFractalCanvas, startColor, endColor);
        }
        public async void DrawQCMenuFractal(Canvas qcFractalCanvas, Color startColor,
            Color endColor)
        {
            //draw qasiclover fractal in the first cell
            qcFractalCanvas.Children.Clear();
            QCFractal qcFractal = new QCFractal(qcFractalCanvas.Width / 6,
                startColor, endColor, 2, qcFractalCanvas);
            //start point
            Point startPoint = new Point(qcFractalCanvas.Width / 2,
                qcFractalCanvas.Height / 2);
            QCDrawingParams drawingParams = new QCDrawingParams(qcFractal.StartLength, 1,
                startPoint, new bool[] { true, true, true, false });
            qcFractalCanvas = await qcFractal.Draw(Dispatcher, DispatcherPriority.Normal, CancellationToken.None,
                drawingParams);
        }
        public async void DrawNFMenuFractal(Canvas nfFractalCanvas, Color startColor,
            Color endColor)
        {
            //draw the H-Fractal in the third cell
            nfFractalCanvas.Children.Clear();
            NFFractal nfFractal = new NFFractal(nfFractalCanvas.Width / 2,
                 startColor, endColor, 2, nfFractalCanvas);
            Point startPoint = new Point(nfFractalCanvas.Width / 2,
                    nfFractalCanvas.Height / 2);
            NFDrawingParams nfDrawingParams = new NFDrawingParams(nfFractal.StartLength,
                1, startPoint);
            nfFractalCanvas = await nfFractal.Draw(Dispatcher, DispatcherPriority.Normal, CancellationToken.None,
                nfDrawingParams);
        }
        public async void DrawSCMenuFractal(Canvas scFractalCanvas, Color startColor,
            Color endColor)
        {
            //draw the Serpinskiy's triangle int the second cell
            scFractalCanvas.Children.Clear();
            SCFractal scFractal = new SCFractal(2.0 * scFractalCanvas.Width / 3,
                 startColor, endColor, 2, scFractalCanvas);
            Point startPoint = new Point(scFractalCanvas.Width / 2, scFractalCanvas.Height / 2);
            SCDrawingParams scDrawingParams = new SCDrawingParams(scFractal.StartLength,
                1, startPoint);
            scFractal.InitialDraw(scDrawingParams);
            scFractalCanvas = await scFractal.Draw(Dispatcher, DispatcherPriority.Normal, CancellationToken.None,
                            scDrawingParams);
        }
        public void DoParamsGridInitialization(Grid mainGrid, Grid paramsGrid,
            Grid canvasOptionsGrid, ScrollViewer scroll, Grid qcExtraParamsGrid)
        {
            paramsGrid.Margin = new Thickness(10, 10, 0, 10);
            paramsGrid.Height = 148 * 3;
            scroll.Width = mainGrid.Width - paramsGrid.Width - 50
                - canvasOptionsGrid.Width - qcExtraParamsGrid.ActualWidth;
            scroll.Height = MainWindow.Height - 170;
            scroll.Margin = new Thickness(scroll.Margin.Left, scroll.Margin.Top,
                scroll.Margin.Right, 30);
            canvasOptionsGrid.Margin = new Thickness(scroll.Margin.Left + scroll.Width,
                10, 10, 0);
        }
    }
}
