using Microsoft.Win32;
using System;
using System.Windows;

namespace CallofDutyAspectRatioTool.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private Core.CallofDuty.Game Game;
        private void OpenGameExecutableBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileOk += OpenFileDialog_FileOk;
            openFileDialog.ShowDialog();
            void OpenFileDialog_FileOk(object sender2, System.ComponentModel.CancelEventArgs e2)
            {
                try
                {
                    Game = new Core.CallofDuty.Game(openFileDialog.FileName);
                    WidthARTextBox.Text = Game.AspectRatioModifier.AspectRatio.Numerator.ToString();
                    HeightARTextBox.Text = Game.AspectRatioModifier.AspectRatio.Denominator.ToString();
                    ChangeAspectRatioBtn.IsEnabled = true;
                    GameExecutableTextBox.Text = openFileDialog.FileName;
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Please Select A Support Game.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    GameExecutableTextBox.Text = "Game Executable";
                    ChangeAspectRatioBtn.IsEnabled = false;
                }
                
            }
        }

        private void ChangeAspectRatioBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Game.AspectRatioModifier.AspectRatio = new Core.Utils.Fraction()
                {
                    Numerator = Convert.ToUInt16(WidthARTextBox.Text.Trim()),
                    Denominator = Convert.ToUInt16(HeightARTextBox.Text.Trim()),
                };
                MessageBox.Show("Successfully Changed Aspect Ratio", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                Environment.Exit(0);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please Input Only Numbers", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (OverflowException)
            {
                MessageBox.Show("Please Input Only Positive Numbers", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
