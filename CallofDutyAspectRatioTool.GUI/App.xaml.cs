using System;
using System.IO;
using System.Windows;

namespace CallofDutyAspectRatioTool.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string ErrorDumpPath = Path.Combine(Environment.CurrentDirectory, $"error_{DateTime.Now:yyyy-dd-M--HH-mm-ss}.txt");
            File.WriteAllText(ErrorDumpPath, e.Exception.ToString());
            MessageBox.Show($"Error:\n\n{e.Exception.Message}\n\nWrote full error details to:\n\n\"{ErrorDumpPath}\"", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
