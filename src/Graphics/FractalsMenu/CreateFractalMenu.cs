using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Graphics
{
    public class CreateFractalMenu
    {
        #region Private Variables
        private string LengthTextBoxText { get; } = "Enter the start length here...";
        private string RecDepthBoxText { get; } = "Enter the recursion depth here...";
        private string StartColorBoxText { get; } = "Enter the #HEX value of start color...";
        private string EndColorBoxText { get; } = "Enter the #HEX value of end color...";
        #endregion
        public virtual void CreateMenu(Grid paramsGrid, Window mainWindow,
            string fractalName, int maxLength, int maxDepth,
            string currentLength, string currentDepth, string currStartColor,
            string currEndColor)
        {
            paramsGrid.Children.Clear();
            #region Elements
            //create elements
            TextBlock headerTextBox = new TextBlock
            {
                Text = $"Set the params of the \n{fractalName} here: ",
                TextAlignment = TextAlignment.Left,
                TextWrapping = TextWrapping.Wrap,
                Width = 200,
                Height = 55,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10, 10, 0, 0),
                Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170)),
                FontSize = 15,
                FontWeight = FontWeight.FromOpenTypeWeight(600)
            };
            TextBox startLengthTextBox = new TextBox
            {
                Style = mainWindow.Resources["paramsTextBoxStyle"] as Style,
                Text = (currentLength == string.Empty) ? LengthTextBoxText : currentLength,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10, 80, 0, 0),
                Height = 20,
                ToolTip = $"Max Length less or equal {maxLength}",
                Width = 200
            };
            if ((!int.TryParse(currentLength, out int cLength) ||
                cLength <= 0 || cLength > maxLength) &&
                startLengthTextBox.Text != LengthTextBoxText)
            {
                startLengthTextBox.Style = mainWindow.Resources["paramsTextBoxStyleWrong"] as Style;
            }
            Slider startLengthSlider = new Slider
            {
                Width = 150,
                Height = 20,
                Minimum = 1,
                Maximum = maxLength,
                Margin = new Thickness(10, 110, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                IsSelectionRangeEnabled = true,
                SelectionStart = 1,
                SelectionEnd = maxLength,
                ToolTip = $"Max Length less or equal {maxLength}"
            };
            if (int.TryParse(currentLength, out int length) && length <= maxLength)
            {
                startLengthSlider.Value = length;
            }
            TextBox recursionDepthTextBox = new TextBox
            {
                Style = mainWindow.Resources["paramsTextBoxStyle"] as Style,
                Text = (currentDepth == string.Empty) ? RecDepthBoxText : currentDepth,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10, 150, 0, 0),
                Height = 20,
                ToolTip = $"Max depth less or equal {maxDepth}",
                Width = 200
            };
            if ((!int.TryParse(currentDepth, out int cDepth)
                || cDepth <= 0 || cDepth > maxDepth) &&
                recursionDepthTextBox.Text != RecDepthBoxText)
            {
                recursionDepthTextBox.Style = mainWindow.Resources["paramsTextBoxStyleWrong"] as Style;
            }
            Slider recursionDepthSlider = new Slider
            {
                Width = 150,
                Minimum = 1,
                Height = 20,
                Maximum = maxDepth,
                Margin = new Thickness(10, 180, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                IsSelectionRangeEnabled = true,
                SelectionStart = 1,
                SelectionEnd = maxDepth,
                ToolTip = $"Max depth less or equal {maxDepth}"
            };
            if (int.TryParse(currentDepth, out int depth) && depth <= maxDepth)
            {
                recursionDepthSlider.Value = depth;
            }
            TextBox startColorTextBox = new TextBox
            {
                Style = mainWindow.Resources["paramsTextBoxStyle"] as Style,
                Text = (currStartColor == string.Empty) ? StartColorBoxText : currStartColor,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10, 220, 0, 0),
                Height = 20,
                Width = 200
            };
            if (!CheckColorString.Check(currStartColor) &&
                startColorTextBox.Text != StartColorBoxText)
            {
                startColorTextBox.Style = mainWindow.Resources["paramsTextBoxStyleWrong"] as Style;
            }
            TextBox endColorTextBox = new TextBox
            {
                Style = mainWindow.Resources["paramsTextBoxStyle"] as Style,
                Text = (currEndColor == string.Empty) ? EndColorBoxText : currEndColor,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10, 260, 0, 0),
                Height = 20,
                Width = 200
            };
            if (!CheckColorString.Check(currEndColor) &&
                endColorTextBox.Text != EndColorBoxText)
            {
                endColorTextBox.Style = mainWindow.Resources["paramsTextBoxStyleWrong"] as Style;
            }
            CheckBox drawSpeedRadioBtn = new CheckBox
            {
                //Style = mainWindow.Resources["radioBtnStyle"] as Style,
                Padding = new Thickness(1, 1, 1, 1),
                Content = "Show drawing process",
                FontFamily = new FontFamily("Arial"),
                Foreground = new SolidColorBrush(Color.FromRgb(169, 169, 169)),
                Height = 20,
                Width = 200,
                Margin = new Thickness(10, 300, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            TextBlock hintForCheckBox = new TextBlock
            {
                Text = "(recommended for recursion levels, greater than 6)",
                FontFamily = new FontFamily("Arial"),
                FontSize = 10,
                Foreground = new SolidColorBrush(Color.FromRgb(169, 169, 169)),
                Margin = new Thickness(10, 320, 0, 0),
            };
            #endregion
            #region Event Handlers
            //event handlers:
            startLengthTextBox.GotFocus += (sender, e) =>
            {
                TextBox textBox = (TextBox)sender;
                if (textBox.Text == LengthTextBoxText)
                {
                    textBox.Text = string.Empty;
                }
                if (!int.TryParse(textBox.Text, out int x) || x <= 0
                    || x > maxLength)
                {
                    textBox.Style = mainWindow.Resources["paramsTextBoxStyleWrong"] as Style;
                }
                else
                {
                    textBox.Style = mainWindow.Resources["paramsTextBoxStyle"] as Style;
                }
            };
            recursionDepthTextBox.GotFocus += (sender, e) =>
            {
                TextBox textBox = (TextBox)sender;
                if (!int.TryParse(textBox.Text, out int x) || x <= 0 || x > maxDepth)
                {
                    textBox.Style = mainWindow.Resources["paramsTextBoxStyleWrong"] as Style;
                }
                else
                {
                    textBox.Style = mainWindow.Resources["paramsTextBoxStyle"] as Style;
                }
                if (textBox.Text == RecDepthBoxText)
                {
                    textBox.Text = string.Empty;
                }
            };
            startLengthTextBox.LostFocus += (sender, e) =>
            {
                TextBox textBox = (TextBox)sender;
                if (textBox.Text == string.Empty)
                {
                    textBox.Text = LengthTextBoxText;
                    textBox.Style = mainWindow.Resources["paramsTextBoxStyle"] as Style;
                }
                else
                {
                    if (!double.TryParse(textBox.Text, out double x) || x <= 0
                        || x > maxLength)
                    {
                        textBox.Style = mainWindow.Resources["paramsTextBoxStyleWrong"] as Style;
                    }
                    else
                    {
                        textBox.Style = mainWindow.Resources["paramsTextBoxStyle"] as Style;
                    }
                }
            };
            recursionDepthTextBox.LostFocus += (sender, e) =>
            {
                TextBox textBox = (TextBox)sender;
                if (textBox.Text == string.Empty)
                {
                    textBox.Text = RecDepthBoxText;
                    textBox.Style = mainWindow.Resources["paramsTextBoxStyle"] as Style;
                }
                else
                {
                    if (!int.TryParse(textBox.Text, out int x) || x <= 0 || x > maxDepth)
                    {
                        textBox.Style = mainWindow.Resources["paramsTextBoxStyleWrong"] as Style;
                    }
                    else
                    {
                        textBox.Style = mainWindow.Resources["paramsTextBoxStyle"] as Style;
                    }
                }
            };
            startColorTextBox.GotFocus += (sender, e) =>
            {
                TextBox textBox = (TextBox)sender;
                if (textBox.Text == StartColorBoxText)
                {
                    textBox.Text = string.Empty;
                }
                if (!CheckColorString.Check(textBox.Text))
                {
                    textBox.Style = mainWindow.Resources["paramsTextBoxStyleWrong"] as Style;
                }
                else
                {
                    textBox.Style = mainWindow.Resources["paramsTextBoxStyle"] as Style;
                }
            };
            startColorTextBox.LostFocus += (sender, e) =>
            {
                TextBox textBox = (TextBox)sender;
                textBox.BorderBrush = new SolidColorBrush(
                    Color.FromRgb(170, 170, 170));
                if (textBox.Text == string.Empty)
                {
                    textBox.Text = StartColorBoxText;
                    textBox.Style = mainWindow.Resources["paramsTextBoxStyle"] as Style;
                }
                else
                {
                    if (!CheckColorString.Check(textBox.Text))
                    {
                        textBox.Style = mainWindow.Resources["paramsTextBoxStyleWrong"] as Style;
                    }
                    else
                    {
                        textBox.Style = mainWindow.Resources["paramsTextBoxStyle"] as Style;
                    }
                }
            };
            endColorTextBox.GotFocus += (sender, e) =>
            {
                TextBox textBox = (TextBox)sender;
                if (textBox.Text == EndColorBoxText
)
                {
                    textBox.Text = string.Empty;
                }
                if (!CheckColorString.Check(textBox.Text))
                {
                    textBox.Style = mainWindow.Resources["paramsTextBoxStyleWrong"] as Style;
                }
                else
                {
                    textBox.Style = mainWindow.Resources["paramsTextBoxStyle"] as Style;
                }
            };
            endColorTextBox.LostFocus += (sender, e) =>
            {
                TextBox textBox = (TextBox)sender;
                textBox.BorderBrush = new SolidColorBrush(
                    Color.FromRgb(170, 170, 170));
                if (textBox.Text == string.Empty)
                {
                    textBox.Text = EndColorBoxText;
                    textBox.Style = mainWindow.Resources["paramsTextBoxStyle"] as Style;
                }
                else
                {
                    if (!CheckColorString.Check(textBox.Text))
                    {
                        textBox.Style = mainWindow.Resources["paramsTextBoxStyleWrong"] as Style;
                    }
                    else
                    {
                        textBox.Style = mainWindow.Resources["paramsTextBoxStyle"] as Style;
                    }
                }
            };
            //if the input is incorrect then make the bottom border red to inform user
            recursionDepthTextBox.KeyUp += (sender, e) =>
            {
                TextBox textBox = (TextBox)sender;
                if (!int.TryParse(textBox.Text, out int x) || x > maxDepth || x <= 0)
                {
                    textBox.Style = mainWindow.Resources["paramsTextBoxStyleWrong"] as Style;
                }
                else
                {
                    textBox.Style = mainWindow.Resources["paramsTextBoxStyle"] as Style;
                    recursionDepthSlider.Value = double.Parse(textBox.Text);
                }
            };
            startLengthTextBox.KeyUp += (sender, e) =>
            {
                TextBox textBox = (TextBox)sender;
                if (!double.TryParse(textBox.Text, out double x) || x <= 0 ||
                    x > maxLength)
                {
                    textBox.Style = mainWindow.Resources["paramsTextBoxStyleWrong"] as Style;
                }
                else
                {
                    textBox.Style = mainWindow.Resources["paramsTextBoxStyle"] as Style;
                    startLengthSlider.Value = double.Parse(textBox.Text);
                }
            };
            startLengthSlider.ValueChanged += (sender, e) =>
            {
                startLengthTextBox.Style = mainWindow.Resources["paramsTextBoxStyle"] as Style;
                startLengthTextBox.Text = ((int)startLengthSlider.Value).ToString();
            };
            recursionDepthSlider.ValueChanged += (sender, e) =>
            {
                recursionDepthTextBox.Style = mainWindow.Resources["paramsTextBoxStyle"] as Style;
                recursionDepthTextBox.Text = ((int)recursionDepthSlider.Value).ToString();
            };
            void BoxesKeyUp(object sender, EventArgs e)
            {
                TextBox textBox = (TextBox)sender;
                if (CheckColorString.Check(textBox.Text))
                {
                    textBox.Style = mainWindow.Resources["paramsTextBoxStyle"] as Style;
                }
                else
                {
                    textBox.Style = mainWindow.Resources["paramsTextBoxStyleWrong"] as Style;
                }
            }
            startColorTextBox.KeyUp += BoxesKeyUp;
            endColorTextBox.KeyUp += BoxesKeyUp;
            #endregion
            #region Add Children
            paramsGrid.Children.Add(headerTextBox);
            paramsGrid.Children.Add(startLengthTextBox);
            paramsGrid.Children.Add(startLengthSlider);
            paramsGrid.Children.Add(recursionDepthTextBox);
            paramsGrid.Children.Add(recursionDepthSlider);
            paramsGrid.Children.Add(startColorTextBox);
            paramsGrid.Children.Add(endColorTextBox);
            paramsGrid.Children.Add(hintForCheckBox);
            paramsGrid.Children.Add(drawSpeedRadioBtn);
            #endregion
        }
    }
}
