using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Graphics;
using System.IO;
using Microsoft.Win32;

namespace Fractals
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private variables/properties
        private InitializeMainGrids initialize;
        //start and end colors for passive and active menu images
        private Color startMenuDefaultColor = Color.FromRgb(255, 50, 0);
        private Color endMenuDefaultColor = Color.FromRgb(255, 150, 0);
        private Color startMenuActiveColor = Color.FromRgb(255, 100, 0);
        private Color endMenuActiveColor = Color.FromRgb(255, 200, 0);
        //current fractal, 0=nothing, 1=QC, 2=SC, 3=NF
        private int CurrentFractal { get; set; } = 0;
        //start draw object (initializes the main drawing process)
        private StartDraw StartDraw { get; set; }
        //tokern source, which token is used to stop the drawing process
        private CancellationTokenSource TokenSource { get; set; } = new CancellationTokenSource();
        private double CurrentScale { get; set; } = 1;
        private bool IsButtonEventSet { get; set; } = true;
        private double ScopeCoeff { get; set; } = 1;
        private int MaxScopeEnlargement { get; set; } = 6;
        //the max start length of the fractal
        private int MaxLength { get; set; }
        //the default margin of the scroll (when additional settings of QC are closed)
        private Thickness InitialScrollMargin { get; set; }
        //indicates which sides of the QC will be drawn
        private bool[] QCSidesDrawability { get; set; } = { true, true, true, false };
        //the number of selected ellipses in additional settings of QC
        private int NumberOfQCSides { get; set; } = 3;
        //the maximum recursion depth of the current fractal
        private int MaxRecDepth { get; set; }
        private Canvas MainCanvas { get; set; }
        //current values of a textboxes in a params grid
        private string CurrentLength { get; set; } = string.Empty;
        private string CurrentDetpth { get; set; } = string.Empty;
        private string CurrentStartColor { get; set; } = string.Empty;
        private string CurrentEndColor { get; set; } = string.Empty;
        //if the drawing is going 
        private bool IsDrawingGoing { get; set; } = false;
        #endregion
        public MainWindow()
        { 
            try
            {
                InitializeComponent();
                //set this culture in order to make all messages display in English
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                initialize = new InitializeMainGrids(mainWindow);
                //initialize three main grids and two line grids
                initialize.DoGridInitialization(headerGrid, leftGrid, mainGrid,
                    toplineGrid, leftLineGrid);
                initialize.DrawMenuItems(qcFractal, nfFractal, scFractal,
                    startMenuDefaultColor, endMenuDefaultColor);
                //initialize the parametres' grid (grid where all fractals params are set)
                initialize.DoParamsGridInitialization(mainGrid, paramsGrid,
                    canvasOptionsGrid, mainCanvasScroll, qCExtraParamsGrid);
                InitialScrollMargin = mainCanvasScroll.Margin;
                MainCanvas = mainCanvas;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message, "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Soft's message", MessageBoxButton.OK,
                MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
        #region Mouse Enter Events
        private void EnlargeIconMouseEnter(object sender, MouseEventArgs e)
        {
            enlargeGridFirst.Visibility = Visibility.Collapsed;
            enlargeGridSecond.Visibility = Visibility.Visible;
        }
        private void ReduceIconMouseEnter(object sender, MouseEventArgs e)
        {
            reduceGridFirst.Visibility = Visibility.Collapsed;
            reduceGridSecond.Visibility = Visibility.Visible;
        }
        private void CancelDrawingMouseEnter(object sender, MouseEventArgs e)
        {
            cancelDrawingFirst.Visibility = Visibility.Collapsed;
            cancelDrawingSecond.Visibility = Visibility.Visible;
        }
        private void ExportPngFirstEnter(object sender, MouseEventArgs e)
        {
            exportPngFirst.Visibility = Visibility.Collapsed;
            exportPngSecond.Visibility = Visibility.Visible;
        }
        private void SelectEllipseMouseEnter(object sender, MouseEventArgs e)
        {
            Ellipse ellipse = (Ellipse)sender;
            ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 215, 0));
        }
        private void MenuCanvasMouseEnter(object sender, MouseEventArgs e)
        {
            Canvas canvas = (Canvas)sender;
            switch (canvas.Name)
            {
                case "qcFractal":
                    initialize.DrawQCMenuFractal(canvas, startMenuActiveColor,
                        endMenuActiveColor);
                    break;
                case "nfFractal":
                    initialize.DrawNFMenuFractal(canvas, startMenuActiveColor,
                        endMenuActiveColor);
                    break;
                case "scFractal":
                    initialize.DrawSCMenuFractal(canvas, startMenuActiveColor,
                        endMenuActiveColor);
                    break;
            }
        }
        #endregion
        #region Mosue Leave Events
        private void EnlargeIconMouseLeave(object sender, MouseEventArgs e)
        {
            enlargeGridFirst.Visibility = Visibility.Visible;
            enlargeGridSecond.Visibility = Visibility.Collapsed;
        }
        private void ReduceIconMouseLeave(object sender, MouseEventArgs e)
        {
            reduceGridFirst.Visibility = Visibility.Visible;
            reduceGridSecond.Visibility = Visibility.Collapsed;
        }
        private void CancelDrawingMouseLeave(object sender, MouseEventArgs e)
        {
            cancelDrawingFirst.Visibility = Visibility.Visible;
            cancelDrawingSecond.Visibility = Visibility.Collapsed;
        }
        private void ExportPngSecondLeave(object sender, MouseEventArgs e)
        {
            exportPngSecond.Visibility = Visibility.Collapsed;
            exportPngFirst.Visibility = Visibility.Visible;
        }
        private void SelectEllipseMouseLeave(object sender, MouseEventArgs e)
        {
            Ellipse ellipse = (Ellipse)sender;
            switch (ellipse.Name)
            {
                case "leftEllipse":
                    if (QCSidesDrawability[0])
                        ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 165, 0));
                    else
                        ellipse.Fill = new SolidColorBrush(Color.FromRgb(169, 169, 169));
                    break;
                case "topEllipse":
                    if (QCSidesDrawability[1])
                        ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 165, 0));
                    else
                        ellipse.Fill = new SolidColorBrush(Color.FromRgb(169, 169, 169));
                    break;
                case "rightEllipse":
                    if (QCSidesDrawability[2])
                        ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 165, 0));
                    else
                        ellipse.Fill = new SolidColorBrush(Color.FromRgb(169, 169, 169));
                    break;
                case "bottomEllipse":
                    if (QCSidesDrawability[3])
                        ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 165, 0));
                    else
                        ellipse.Fill = new SolidColorBrush(Color.FromRgb(169, 169, 169));
                    break;
            }
        }
        /// <summary>
        /// Change the color again to it's original value when the mouse leaves
        /// </summary>
        private void MenuCanvasMouseLeave(object sender, MouseEventArgs e)
        {
            Canvas canvas = (Canvas)sender;
            switch (canvas.Name)
            {
                case "qcFractal":
                    initialize.DrawQCMenuFractal(canvas, startMenuDefaultColor, endMenuDefaultColor);
                    break;
                case "nfFractal":
                    initialize.DrawNFMenuFractal(canvas, startMenuDefaultColor, endMenuDefaultColor);
                    break;
                case "scFractal":
                    initialize.DrawSCMenuFractal(canvas, startMenuDefaultColor, endMenuDefaultColor);
                    break;
            }
        }
        #endregion
        #region Size Changed Events
        /// <summary>
        /// Change elements' position and size when the window is resized
        /// </summary>
        private void MainWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (WindowState == WindowState.Maximized)
                {
                    initialize = new InitializeMainGrids(mainWindow);
                }
                initialize.DoGridInitialization(headerGrid, leftGrid, mainGrid,
                    toplineGrid, leftLineGrid);
                initialize.DoParamsGridInitialization(mainGrid, paramsGrid,
                    canvasOptionsGrid, mainCanvasScroll, qCExtraParamsGrid);
                Scroll(mainCanvasScroll.ScrollableWidth / 2, mainCanvasScroll.ScrollableHeight / 2);
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        #region Mouse Down/Click
        /// <summary>
        /// This event accurs when one ellipse in additional settings in QC
        /// Refreshes the QCSidesDrawability and the NumberOfQCSides
        /// </summary>
        private void SelectEllipseMouseDown(object sender, MouseEventArgs e)
        {
            Ellipse ellipse = (Ellipse)sender;
            switch (ellipse.Name)
            {
                case "leftEllipse":
                    if (QCSidesDrawability[0])
                    {
                        ellipse.Fill = new SolidColorBrush(Color.FromRgb(169, 169, 169));
                        NumberOfQCSides--;
                    }
                    else
                    {
                        ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 165, 0));
                        NumberOfQCSides++;
                    }
                    QCSidesDrawability[0] = !QCSidesDrawability[0];
                    break;
                case "topEllipse":
                    if (QCSidesDrawability[1])
                    {
                        ellipse.Fill = new SolidColorBrush(Color.FromRgb(169, 169, 169));
                        NumberOfQCSides--;
                    }
                    else
                    {
                        ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 165, 0));
                        NumberOfQCSides++;
                    }
                    QCSidesDrawability[1] = !QCSidesDrawability[1];
                    break;
                case "rightEllipse":
                    if (QCSidesDrawability[2])
                    {
                        ellipse.Fill = new SolidColorBrush(Color.FromRgb(169, 169, 169));
                        NumberOfQCSides--;
                    }
                    else
                    {
                        ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 165, 0));
                        NumberOfQCSides++;
                    }
                    QCSidesDrawability[2] = !QCSidesDrawability[2];
                    break;
                case "bottomEllipse":
                    if (QCSidesDrawability[3])
                    {
                        ellipse.Fill = new SolidColorBrush(Color.FromRgb(169, 169, 169));
                        NumberOfQCSides--;
                    }
                    else
                    {
                        ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 165, 0));
                        NumberOfQCSides++;
                    }
                    QCSidesDrawability[3] = !QCSidesDrawability[3];
                    break;
            }
        }
        private void EnlargeIconMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentScale <= MaxScopeEnlargement)
            {
                double verticalOffSet = mainCanvasScroll.ContentVerticalOffset;
                double horizontalOffSet = mainCanvasScroll.ContentHorizontalOffset;
                ScaleTransform transform = new ScaleTransform
                {
                    ScaleX = CurrentScale + ScopeCoeff,
                    ScaleY = CurrentScale += ScopeCoeff,
                    CenterX = MainCanvas.Width / 2,
                    CenterY = MainCanvas.Height / 2
                };
                MainCanvas.RenderTransform = transform;
                Scroll(horizontalOffSet, verticalOffSet);
            }
        }
        private void ReduceIconMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentScale - ScopeCoeff >= 1)
            {
                double verticalOffSet = mainCanvasScroll.ContentVerticalOffset;
                double horizontalOffSet = mainCanvasScroll.ContentHorizontalOffset;
                ScaleTransform transform = new ScaleTransform
                {
                    ScaleX = CurrentScale - ScopeCoeff,
                    ScaleY = CurrentScale -= ScopeCoeff,
                    CenterX = MainCanvas.Width / 2,
                    CenterY = MainCanvas.Height / 2,
                };
                MainCanvas.RenderTransform = transform;
                Scroll(horizontalOffSet, verticalOffSet);
            }
        }
        /// <summary>
        /// Cancels the current drawing task with the token source
        /// </summary>
        private void CancelCurrentDrawingMouseDown(object sender, MouseButtonEventArgs e)
        {
            TokenSource.Cancel();
            TokenSource = new CancellationTokenSource();
        }
        /// <summary>
        /// Saves the image of a fractal as a PNG file
        /// </summary>
        private void ExportPngSecondClick(object sender, MouseButtonEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                DefaultExt = "png",
                Filter = "*PNG(*.png)|*.png",
                OverwritePrompt = true,
                Title = "Save the fractal as a PNG file"
            };
            try
            {
                double horizontalOffSet = mainCanvasScroll.HorizontalOffset;
                double verticalOffSet = mainCanvasScroll.VerticalOffset;
                saveFileDialog.ShowDialog();
                if (saveFileDialog.FileName != string.Empty)
                {
                    Transform transform = MainCanvas.LayoutTransform;
                    //reset current transform (in case it is scaled or rotated)
                    MainCanvas.LayoutTransform = null;
                    Size size = new Size(MainCanvas.Width, MainCanvas.Height);
                    MainCanvas.Measure(size);
                    MainCanvas.Arrange(new Rect(size));
                    RenderTargetBitmap rtb = new RenderTargetBitmap((int)size.Width,
                        (int)size.Height, 96d, 96d, PixelFormats.Pbgra32);
                    rtb.Render(MainCanvas);
                    // Create a file stream for saving image
                    using (FileStream outStream = (FileStream)saveFileDialog.OpenFile())
                    {
                        // Use png encoder for our data
                        PngBitmapEncoder encoder = new PngBitmapEncoder();
                        // push the rendered bitmap to it
                        encoder.Frames.Add(BitmapFrame.Create(rtb));
                        // save the data to the stream
                        encoder.Save(outStream);
                    }
                    // Restore previously saved layout
                    MainCanvas.LayoutTransform = transform;
                    Scroll(mainCanvasScroll.ScrollableWidth / 2, mainCanvasScroll.ScrollableHeight / 2);
                    MessageBox.Show("The fractal is saved", "Soft's message", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message, "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
        /// <summary>
        /// Starts the process of drawing
        /// </summary>
        private void MenuCanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //get all text boxes out of the params grid
                List<TextBox> pararmsTextBoxes = paramsGrid.Children.OfType<TextBox>().ToList();
                Canvas canvas = (Canvas)sender;
                if (CurrentFractal != 0)
                {
                    CurrentLength = pararmsTextBoxes[0].Text;
                    CurrentDetpth = pararmsTextBoxes[1].Text;
                    CurrentStartColor = pararmsTextBoxes[2].Text;
                    CurrentEndColor = pararmsTextBoxes[3].Text;
                }
                switch (canvas.Name)
                {
                    case "qcFractal":
                        MaxLength = 115;
                        MaxRecDepth = 10;
                        CreateQCFractalMenu createQCMenu = new CreateQCFractalMenu();
                        createQCMenu.CreateMenu(paramsGrid, mainWindow,
                            "Quasi-clover fractal", MaxLength, MaxRecDepth, CurrentLength,
                            CurrentDetpth, CurrentStartColor, CurrentEndColor);
                        CurrentFractal = 1;
                        break;
                    case "scFractal":
                        MaxLength = 700;
                        MaxRecDepth = 10;
                        CreateSCFractalMenu createSCMenu = new CreateSCFractalMenu();
                        createSCMenu.CreateMenu(paramsGrid, mainWindow,
                            "Serpinskiy's triangle", MaxLength, MaxRecDepth, CurrentLength,
                            CurrentDetpth, CurrentStartColor, CurrentEndColor);
                        CurrentFractal = 2;
                        //if the additional settings are still open - close them
                        if (mainCanvasScroll.Margin.Left != InitialScrollMargin.Left)
                        {
                            AnimateScroll(-175);
                        }
                        break;
                    case "nfFractal":
                        MaxLength = 325;
                        MaxRecDepth = 8;
                        CreateNFFractalMenu createNFMenu = new CreateNFFractalMenu();
                        createNFMenu.CreateMenu(paramsGrid, mainWindow,
                            "H-Fractral", MaxLength, MaxRecDepth, CurrentLength,
                            CurrentDetpth, CurrentStartColor, CurrentEndColor);
                        CurrentFractal = 3;
                        //if the additional settings are still open - close them
                        if (mainCanvasScroll.Margin.Left != InitialScrollMargin.Left)
                        {
                            AnimateScroll(-175);
                        }
                        break;
                }
                //attach a new button to the StartDraw property
                if (StartDraw != null)
                {
                    StartDraw.GoBtn = (Button)paramsGrid.Children[paramsGrid.Children.Count - 1];
                }
                Button goBtn = (Button)paramsGrid.Children[paramsGrid.Children.Count - 1];
                //add a mouse down handler to the "Go" btn
                if (IsButtonEventSet)
                {
                    goBtn.Click += (obj, evArgs) =>
                    {
                        DrawFractalEventDown(true);
                    };
                    IsButtonEventSet = false;
                }
                //add an aditional settings event
                if (CurrentFractal == 1)
                {
                    Button additionalSettings = (Button)paramsGrid.Children[
                        paramsGrid.Children.Count - 2];
                    additionalSettings.Click += (s, ev) =>
                    {
                        if (mainCanvasScroll.Margin.Left == InitialScrollMargin.Left)
                        {
                            additionalSettings.Content = "Close";
                            AnimateScroll(175);
                        }
                        else
                        {
                            additionalSettings.Content = "Additional settings";
                            AnimateScroll(-175);
                        }
                    };
                }
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message, "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (InvalidCastException ex)
            {
                MessageBox.Show(ex.Message, "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
        private async void DrawFractalEventDown(bool showMsgBox)
        {
            try
            {
                IsDrawingGoing = true;
                //start length & recursion depth
                TextBox sLengthBox = (TextBox)paramsGrid.Children[1];
                TextBox recDepthBox = (TextBox)paramsGrid.Children[3];
                Point startPoint = new Point(MainCanvas.Width / (2),
                    MainCanvas.Height / (2));
                //hex values of start and end color
                string startColorText = ((TextBox)paramsGrid.Children[5]).Text;
                string endColorText = ((TextBox)paramsGrid.Children[6]).Text;
                if (double.TryParse(sLengthBox.Text, out double length) &&
                    int.TryParse(recDepthBox.Text, out int depth) && depth > 0
                    && depth <= MaxRecDepth && length > 0 && length <= MaxLength &&
                    CheckColorString.Check(startColorText) && CheckColorString.Check(endColorText))
                {
                    int numberOfTrues = 0;
                    foreach (bool b in QCSidesDrawability)
                    {
                        if (b)
                            numberOfTrues++;
                    }
                    if (numberOfTrues != 3 && CurrentFractal == 1)
                    {
                        MessageBox.Show("Select three ellipses to draw...",
                            "Soft's message", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else
                    {
                        //reset the canvas
                        Canvas newCanvas = new Canvas
                        {
                            Width = 5000,
                            Height = 5000,
                            Background = new SolidColorBrush(Color.FromRgb(255, 255, 255))
                        };
                        mainCanvasScroll.Content = MainCanvas = newCanvas;
                        //transform the canvas to it's original size
                        ScaleTransform transform = new ScaleTransform
                        {
                            ScaleX = CurrentScale = 1,
                            ScaleY = CurrentScale = 1,
                        };
                        MainCanvas.RenderTransform = transform;
                        Scroll(mainCanvasScroll.ScrollableWidth / 2, mainCanvasScroll.ScrollableHeight / 2);
                        //get the checkbox
                        List<CheckBox> checkBoxes = paramsGrid.Children.OfType<CheckBox>().ToList();
                        CheckBox checkBox = checkBoxes[0];
                        //intialize start draw object
                        StartDraw = new StartDraw
                        {
                            CurrentFractal = CurrentFractal,
                            MainCanvas = MainCanvas,
                            StartColor = (Color)ColorConverter.ConvertFromString(startColorText),
                            EndColor = (Color)ColorConverter.ConvertFromString(endColorText),
                            GoBtn = (Button)paramsGrid.Children[paramsGrid.Children.Count - 1],
                            QCSidesDrawability = QCSidesDrawability
                        };
                        CancellationToken token = TokenSource.Token;
                        //set the priority
                        if (checkBox.IsChecked == false || checkBox.IsChecked == null)
                        {
                            cancelDrawingFirst.IsEnabled = false;
                            StartDraw.DispatcherPriority = DispatcherPriority.Send;
                            mainCanvasScroll.Content = MainCanvas = await StartDraw.DrawFractal(length, depth,
                                startPoint, Dispatcher, token);
                            cancelDrawingFirst.IsEnabled = true;
                        }
                        else
                        {
                            StartDraw.DispatcherPriority = DispatcherPriority.Background;
                            await StartDraw.DrawFractal(length, depth,
                                startPoint, Dispatcher, token);
                        }
                    }
                }
                else
                {
                    if (showMsgBox)
                        MessageBox.Show("Error in the input data",
                            "Soft's message", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message, "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (InvalidCastException ex)
            {
                MessageBox.Show(ex.Message, "Soft's message", MessageBoxButton.OK,
                     MessageBoxImage.Information);
            }
            catch (OutOfMemoryException ex)
            {
                MessageBox.Show(ex.Message, "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (StackOverflowException ex)
            {
                MessageBox.Show(ex.Message, "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Soft's message", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            finally
            {
                IsDrawingGoing = false;
            }
        }
        #endregion
        #region Animations
        /// <summary>
        /// Animates the scroll, offsetting it to the left
        /// or to the right, depending on the offset's sign
        /// </summary>
        private void AnimateScroll(double offset)
        {
            DoubleAnimation scrollWidthAnimation = new DoubleAnimation
            {
                From = mainCanvasScroll.Width,
                To = mainCanvasScroll.Width - offset,
                Duration = TimeSpan.FromMilliseconds(100),
                FillBehavior = FillBehavior.Stop
            };
            ThicknessAnimation scrollMarginAnimation = new ThicknessAnimation
            {
                From = mainCanvasScroll.Margin,
                To = new Thickness(mainCanvasScroll.Margin.Left + offset,
                    mainCanvasScroll.Margin.Top, 0, 0),
                Duration = TimeSpan.FromMilliseconds(100),
                FillBehavior = FillBehavior.Stop
            };
            if (mainCanvasScroll.Margin.Left + offset >= InitialScrollMargin.Left)
            {
                mainCanvasScroll.BeginAnimation(MarginProperty, scrollMarginAnimation);
                mainCanvasScroll.BeginAnimation(WidthProperty, scrollWidthAnimation);
                mainCanvasScroll.Width -= offset;
                mainCanvasScroll.Margin = new Thickness(mainCanvasScroll.Margin.Left + offset,
                    mainCanvasScroll.Margin.Top, 0, 0);
            }
            if (offset > 0)
                qCExtraParamsGrid.Visibility = Visibility.Visible;
            else
                qCExtraParamsGrid.Visibility = Visibility.Collapsed;
        }
        #endregion
        private void Scroll(double horizontalOffSet, double verticalOffSet)
        {
            mainCanvasScroll.ScrollToHorizontalOffset(horizontalOffSet);
            mainCanvasScroll.ScrollToVerticalOffset(verticalOffSet);
            mainCanvasScroll.LineUp();
        }
        private void EnterKeyDown(object sender, KeyEventArgs e)
        {
            if (CurrentFractal != 0 && e.Key == Key.Enter && !IsDrawingGoing)
                DrawFractalEventDown(true);
        }
    }
}
