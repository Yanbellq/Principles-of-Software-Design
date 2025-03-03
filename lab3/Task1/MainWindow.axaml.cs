using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Task1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnChangeCaseButtonClick(object sender, RoutedEventArgs e)
        {
            string input = InputTextBox.Text;
            string result = ChangeCase(input);
            ResultTextBlock.Text = result;
        }

        private string ChangeCase(string input)
        {
            char[] chars = input.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (char.IsUpper(chars[i]))
                {
                    chars[i] = char.ToLower(chars[i]);
                }
                else if (char.IsLower(chars[i]))
                {
                    chars[i] = char.ToUpper(chars[i]);
                }
            }
            return new string(chars);
        }
    }
}