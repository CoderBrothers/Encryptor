using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Encryptor
{
    /// <summaryкодегора>
    /// Interaction logic for MainWindow.xaml/
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnClosed(object? sender, EventArgs e)
        {
            AppSettings.Default.WindowTop = Top;
            AppSettings.Default.WindowLeft = Left;
            AppSettings.Default.Save();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Top = AppSettings.Default.WindowTop;
            Left = AppSettings.Default.WindowLeft;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Clear()
        {
            InputTextBox.Clear();
            OutputTextBox.Clear();
        }

        private void Encrypt(object sender, RoutedEventArgs e)
        {
            if (KeyBox.Text == string.Empty) return;
            int.TryParse(KeyBox.Text, out int shift);
            var inputArray = InputTextBox.Text.ToCharArray();
            for (var i = 0; i < inputArray.Length; i++)
            {
                inputArray[i] = (char)(inputArray[i] ^ shift);
            }
            OutputTextBox.Text = new string(inputArray);
        }

        private async void LoadBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt"
            };
            if (ofd.ShowDialog() == false) return;
            Clear();
            InputTextBox.Text = await File.ReadAllTextAsync(ofd.FileName);
        }

        private void KeyBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private async void SaveBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt"
            };
            if (sfd.ShowDialog() == false) return;
            await File.WriteAllTextAsync(sfd.FileName, OutputTextBox.Text);
        }
    }
}