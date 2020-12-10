using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SudokuSolverWPF
{
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    public class SudokuCellControl : Control
    {
        private TextBox _textBox;

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(SudokuCellControl), new UIPropertyMetadata(null));

        public static readonly DependencyProperty TextForegroundProperty =
            DependencyProperty.Register(nameof(TextForeground), typeof(Brush), typeof(SudokuCellControl), new UIPropertyMetadata(null));

        public static readonly DependencyProperty TextBoxBackgroundProperty =
            DependencyProperty.Register(nameof(TextBoxBackground), typeof(Brush), typeof(SudokuCellControl), new UIPropertyMetadata(null));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        
        public Brush TextForeground
        {
            get { return (Brush)GetValue(TextForegroundProperty); }
            set { SetValue(TextForegroundProperty, value); }
        }

        public Brush TextBoxBackground
        {
            get { return (Brush)GetValue(TextBoxBackgroundProperty); }
            set { SetValue(TextBoxBackgroundProperty, value); }
        }
        
        static SudokuCellControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SudokuCellControl), new FrameworkPropertyMetadata(typeof(SudokuCellControl)));
        }

        public override void OnApplyTemplate()
        {
            _textBox = (TextBox)Template.FindName("PART_TextBox", this);
            _textBox.TextChanged += OnTextChanged;
            TextForeground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            TextBoxBackground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            GotFocus += OnCellHasFocus;
        }

        private void OnCellHasFocus(object sender, RoutedEventArgs e)
        {
            _textBox.Focus();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!TryValidateSudokuCell(_textBox.Text, out int result))
            {
                Text = string.Empty;
            }
            else
            {
                Text = result.ToString();
            }
            _textBox.SelectAll();
        }

        private bool TryValidateSudokuCell(string text, out int result)
        {
            if (int.TryParse(text, out result)
                && result > 0 && result < 10)
            {
                return true;
            }

            return false;
        }
    }
}
