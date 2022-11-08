using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace CODARTOOL
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private int selectedARIndex;
        private int currentARIndex;
        private string selectedGame = null;
        private string[] supportedGames =
        {
            "t6mpv43",
            "t6zmv41",
            "t6zm",
            "t6mp",
            "iw5mp",
            "iw3mp",
            "iw4mp",
            "BlackOpsMP",
            "iw3xo",
            "iw4x",
            "iw4m",
            "BlackOpsSP",
            "CoDWaW",
            "CoDWaWmp"
        };
        private string _169 = "39 8E E3 3F";
        private string _219 = "CD 90 18 40";
        private string _329 = "39 8e 63 40";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CheckBoxChange(1);
            selectGamePopup.Visibility = Visibility.Visible;
            currentARLbl.Visibility = Visibility.Hidden;
            changeARBtn.Visibility = Visibility.Hidden;
            reslbl.Visibility = Visibility.Hidden;
            chooselbl.Visibility = Visibility.Hidden;
            chooselbl.Visibility = Visibility.Hidden;
            resBox.Visibility = Visibility.Hidden;
            _169Box.Visibility = Visibility.Hidden;
            _219Box.Visibility = Visibility.Hidden;
            _329Box.Visibility = Visibility.Hidden;
        }
        private void _169Box_Checked(object sender, RoutedEventArgs e)
        {
            CheckBoxChange(1);
        }
        private void _219Box_Checked(object sender, RoutedEventArgs e)
        {
            CheckBoxChange(2);
        }
        private void _329Box_Checked(object sender, RoutedEventArgs e)
        {
            CheckBoxChange(3);
        }
        private void CheckBoxChange(int boxIndex)
        {
            switch (boxIndex)
            {
                case 1:
                    {
                        _169Box.IsChecked = true;
                        _219Box.IsChecked = false;
                        _329Box.IsChecked = false;
                        selectedARIndex = 1;
                        break;
                    }
                case 2:
                    {
                        _169Box.IsChecked = false;
                        _219Box.IsChecked = true;
                        _329Box.IsChecked = false;
                        selectedARIndex = 2;
                        break;
                    }
                case 3:
                    {
                        _169Box.IsChecked = false;
                        _219Box.IsChecked = false;
                        _329Box.IsChecked = true;
                        selectedARIndex = 3;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        private void GetCurrentAR(string directory)
        {
            byte[] dirbytes = File.ReadAllBytes(directory);
            byte[] find169 = ConvertHexStringToByteArray(Regex.Replace(_169, "0x|[ ,]", string.Empty).Normalize().Trim());
            byte[] find219 = ConvertHexStringToByteArray(Regex.Replace(_219, "0x|[ ,]", string.Empty).Normalize().Trim());
            byte[] find329 = ConvertHexStringToByteArray(Regex.Replace(_329, "0x|[ ,]", string.Empty).Normalize().Trim());

            if (FindBytes(dirbytes, find169) != -1)
            {
                currentARLbl.Content = "Game's Current Aspect Ratio: 16:9";
                currentARIndex = 1;
            }
            else if (FindBytes(dirbytes, find219) != -1)
            {
                currentARLbl.Content = "Game's Current Aspect Ratio: 21:9";
                currentARIndex = 2;
            }
            else if (FindBytes(dirbytes, find329) != -1)
            {
                currentARLbl.Content = "Game's Current Aspect Ratio: 32:9";
                currentARIndex = 3;
            }
            else
            {
                currentARLbl.Content = "Game's Current Aspect Ratio: N/A";
                currentARIndex = 0;
            }
        }
        private void exeDirBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".exe";
            dlg.Filter = "EXE Files (*.exe)|*.exe";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;

                foreach (string game in supportedGames)
                {

                    if (filename.Contains(game))
                    {

                        if (filename.Contains("iw3xo") || filename.Contains("iw4x") || filename.Contains("iw4m"))
                        {
                            selectedGame = game;
                            exeDir.Text = filename;
                            changeARBtn.IsEnabled = true;
                            currentARLbl.Content = "Game's Current Aspect Ratio: N/A";
                            currentARIndex = 0;
                            resBox.Visibility = Visibility.Visible;
                            selectGamePopup.Visibility = Visibility.Hidden;
                            currentARLbl.Visibility = Visibility.Hidden;
                            reslbl.Visibility = Visibility.Visible;
                            changeARBtn.Visibility = Visibility.Visible;
                            chooselbl.Visibility = Visibility.Hidden;
                            chooselbl.Visibility = Visibility.Hidden;
                            _169Box.Visibility = Visibility.Hidden;
                            _219Box.Visibility = Visibility.Hidden;
                            _329Box.Visibility = Visibility.Hidden;
                            changeARBtn.Content = "Start Game";
                        }
                        else
                        {
                            selectedGame = game;
                            exeDir.Text = filename;
                            GetCurrentAR(filename);
                            changeARBtn.IsEnabled = true;
                            resBox.Visibility = Visibility.Hidden;
                            changeARBtn.Visibility = Visibility.Visible;
                            selectGamePopup.Visibility = Visibility.Hidden;
                            currentARLbl.Visibility = Visibility.Visible;
                            reslbl.Visibility = Visibility.Hidden;
                            chooselbl.Visibility = Visibility.Visible;
                            chooselbl.Visibility = Visibility.Visible;
                            _169Box.Visibility = Visibility.Visible;
                            _219Box.Visibility = Visibility.Visible;
                            _329Box.Visibility = Visibility.Visible;
                            changeARBtn.Content = "Change To Selected Aspect Ratio";
                        }
                        return;
                    }
                }
                if (selectedGame == null)
                {
                    MessageBox.Show("Warning, Game is not in supported list.");
                }

            }
        }
        private void changeARBtn_Click(object sender, RoutedEventArgs e)
        {
            if (exeDir.Text.Contains("iw3xo") || exeDir.Text.Contains("iw4x") || exeDir.Text.Contains("iw4m"))
            {
                ChangeArgGame();
            }
            else
            {
                ChangeHexGame();
            }
        }
        private void ChangeHexGame()
        {
            byte[] find169 = ConvertHexStringToByteArray(Regex.Replace(_169, "0x|[ ,]", string.Empty).Normalize().Trim());
            byte[] find219 = ConvertHexStringToByteArray(Regex.Replace(_219, "0x|[ ,]", string.Empty).Normalize().Trim());
            byte[] find329 = ConvertHexStringToByteArray(Regex.Replace(_329, "0x|[ ,]", string.Empty).Normalize().Trim());
            byte[] dirbytes = File.ReadAllBytes(exeDir.Text);
            byte[] finalCAR;
            byte[] finalSAR;
            byte[] output;

            switch (currentARIndex)
            {
                case 1:
                    {
                        finalCAR = find169;
                        break;
                    }
                case 2:
                    {
                        finalCAR = find219;
                        break;
                    }
                case 3:
                    {
                        finalCAR = find329;
                        break;
                    }
                case 0:
                    {
                        finalCAR = null;
                        break;
                    }
                default:
                    {
                        finalCAR = null;
                        break;
                    }
            }
            switch (selectedARIndex)
            {
                case 1:
                    {
                        finalSAR = find169;
                        break;
                    }
                case 2:
                    {
                        finalSAR = find219;
                        break;
                    }
                case 3:
                    {
                        finalSAR = find329;
                        break;
                    }
                default:
                    {
                        finalSAR = null;
                        break;
                    }
            }

            if (finalCAR == null || finalSAR == null)
            {
                MessageBox.Show("Error, Final AR = null (If your exe is modified by Airyz's AR Tool, Please Reinstall the EXE or reset back to 16:9), Please contact me.");
                Environment.Exit(0);
            }

            output = ReplaceBytes(dirbytes, finalCAR, finalSAR);
            File.WriteAllBytes(exeDir.Text, output);
            MessageBox.Show("Successfully Changed Aspect Ratio");
            Environment.Exit(0);
        }
        public void ChangeArgGame()
        {
            double arRes;
            if (resBox.Text.Contains("x"))
            {
                string[] res = resBox.Text.Split('x');
                arRes = Convert.ToDouble(res[0]) / Convert.ToDouble(res[1]);
            }
            else
            {
                MessageBox.Show("Please Include 'x' to separate width from height");
                return;
            }

            Process p = new Process();
            p.StartInfo.FileName = exeDir.Text;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            p.StartInfo.WorkingDirectory = Path.GetDirectoryName(exeDir.Text);
            MessageBox.Show("Successfully Changed Aspect Ratio");
            if (exeDir.Text.Contains("iw4x"))
            {
                p.StartInfo.Arguments = $"+set r_aspectRatio custom +set r_customMode {resBox.Text} +set r_customAspectRatio {arRes} +set r_fullscreen 0 +set cl_autoRecord 0";
                p.Start();
            }
            else if (exeDir.Text.Contains("iw3xo"))
            {
                p.StartInfo.Arguments = $"+set r_aspectRatio custom +set r_customMode {resBox.Text} +set r_aspectRatio_custom {arRes} +set r_fullscreen 0 +set cl_autoRecord 0";
                p.Start();
            }
            else if (exeDir.Text.Contains("iw4m"))
            {
                p.StartInfo.Arguments = $"+set r_aspectRatio custom +set r_customMode {resBox.Text} +set r_customAspectRatio {arRes} +set r_fullscreen 0 +set cl_autoRecord 0";
                p.Start();
            }
            Environment.Exit(0);
        }

        private static byte[] ConvertHexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
            }

            byte[] data = new byte[hexString.Length / 2];
            for (int index = 0; index < data.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }
        public int FindBytes(byte[] src, byte[] find)
        {
            int index = -1;
            int matchIndex = 0;
            // handle the complete source array
            for (int i = 0; i < src.Length; i++)
            {
                if (src[i] == find[matchIndex])
                {
                    if (matchIndex == (find.Length - 1))
                    {
                        index = i - matchIndex;
                        break;
                    }
                    matchIndex++;
                }
                else if (src[i] == find[0])
                {
                    matchIndex = 1;
                }
                else
                {
                    matchIndex = 0;
                }

            }
            return index;
        }
        public byte[] ReplaceBytes(byte[] src, byte[] search, byte[] repl)
        {
            byte[] dst = null;
            int index = FindBytes(src, search);
            if (index >= 0)
            {
                dst = new byte[src.Length - search.Length + repl.Length];
                // before found array
                Buffer.BlockCopy(src, 0, dst, 0, index);
                // repl copy
                Buffer.BlockCopy(repl, 0, dst, index, repl.Length);
                // rest of src array
                Buffer.BlockCopy(
                    src,
                    index + search.Length,
                    dst,
                    index + repl.Length,
                    src.Length - (index + search.Length));
            }
            return dst;
        }

    }
}
