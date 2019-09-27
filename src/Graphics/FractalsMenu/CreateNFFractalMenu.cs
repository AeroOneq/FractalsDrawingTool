using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Graphics
{
    public class CreateNFFractalMenu : CreateFractalMenu
    {
        public override void CreateMenu(Grid paramsGrid, Window mainWindow,
           string fractalName, int maxLength, int maxDepth, string currLength,
           string currDepth, string currStartColor, string currEndColor)
        {
            Button oldButton = null;
            if (paramsGrid.Children.Count != 4)
            {
                oldButton = (Button)paramsGrid.Children[paramsGrid.Children.Count - 1];
            }
            base.CreateMenu(paramsGrid, mainWindow, fractalName, maxLength, maxDepth,
                currLength, currDepth, currStartColor, currEndColor);
            #region Elements
            Button goBtn = new Button
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 0, 255)),
                Width = 100,
                Height = 40,
                Content = "Go",
                FontFamily = new FontFamily("Arial"),
                Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                FontSize = 15,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10, 0, 0, 20),
                Style = mainWindow.Resources["goBtnStyle"] as Style
            };
            #endregion
            #region Event Handlers
            goBtn.MouseEnter += (sender, e) =>
            {
                ((Button)sender).Background = new SolidColorBrush(
                    Color.FromRgb(210, 0, 210));
            };
            goBtn.MouseLeave += (sender, e) =>
            {
                ((Button)sender).Background = new SolidColorBrush(
                    Color.FromRgb(255, 0, 255));
            };
            #endregion
            if (oldButton == null)
            {
                paramsGrid.Children.Add(goBtn);
            }
            else
            {
                paramsGrid.Children.Add(oldButton);
            }
        }
    }
}
