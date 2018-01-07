using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KryptografiaWizualna
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VisualCryptoBasic cryptoBasic = new VisualCryptoBasic();
        Bitmap[] bmpArray = new Bitmap[3];


        public BitmapImage ConvertBitmap(System.Drawing.Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

        public MainWindow()
        {
            InitializeComponent();
            TextBoxRedThreshold.Text = "127";
            TextBoxGreenThreshold.Text = "127";
            TextBoxBlueThreshold.Text = "127";
        }

        private void ButtonLoadImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                cryptoBasic.LoadImage(openFileDialog.FileName);
                ImageInput.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }

        }

        private void ButtonEncryptBasic_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TextBoxRedThreshold.Text, out int R))
            {
                MessageBox.Show("Bład w polu threshold R");
                return;
            }
                
            if (!int.TryParse(TextBoxGreenThreshold.Text, out int G))
            {
                MessageBox.Show("Bład w polu threshold G");
                return;
            }
                
            if (!int.TryParse(TextBoxBlueThreshold.Text, out int B))
            {
                MessageBox.Show("Bład w polu threshold B");
                return;
            }

            if (cryptoBasic.ImageLoaded)
            {
                try
                {
                    if (RadioButton2Pixels.IsChecked == true)
                    {
                        var result = cryptoBasic.Encrypt2PixVersion(R, G, B, (bool)CheckBoxTransparent.IsChecked);
                        ImageComponent1.Source = ConvertBitmap(result[0]);
                        ImageComponent2.Source = ConvertBitmap(result[1]);

                        for (int i = 0; i < result.Length; i++)
                        {
                            result[i].Save(TextBoxComponents.Text + i + ComboBoxImageFormat.Text);
                        }

                        MessageBox.Show("Obrazy zapisane pomyślnie!");

                        Process.Start(Directory.GetCurrentDirectory());
                    }
                    if (RadioButton4Pixels.IsChecked == true)
                    {
                        var result = cryptoBasic.Encrypt(R, G, B, (bool)CheckBoxTransparent.IsChecked);
                        ImageComponent1.Source = ConvertBitmap(result[0]);
                        ImageComponent2.Source = ConvertBitmap(result[1]);

                        for (int i = 0; i < result.Length; i++)
                        {
                            result[i].Save(TextBoxComponents.Text + i + ComboBoxImageFormat.Text);
                        }

                        MessageBox.Show("Obrazy zapisane pomyślnie!");

                        Process.Start(Directory.GetCurrentDirectory());
                    }




                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                MessageBox.Show("Nie wczytano obrazu");
            }



        }

        private void SliderRedColor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TextBoxRedThreshold.Text = SliderRedColor.Value.ToString();

        }

        private void TextBoxRedThreshold_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (int.TryParse(TextBoxRedThreshold.Text, out int val))
            {
                if (val >= 0 && val <= 255)
                {
                    SliderRedColor.Value = val;
                }
            }
            else
            {
                return;
            }


        }

        private void SliderGreenColor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TextBoxGreenThreshold.Text = SliderGreenColor.Value.ToString();
        }

        private void TextBoxGreenThreshold_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(TextBoxGreenThreshold.Text, out int val))
            {
                if (val >= 0 && val <= 255)
                {
                    SliderGreenColor.Value = val;
                }
            }
            else
            {
                return;
            }
        }

        private void SliderBlueColor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TextBoxBlueThreshold.Text = SliderBlueColor.Value.ToString();
        }

        private void TextBoxBlueThreshold_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(TextBoxBlueThreshold.Text, out int val))
            {
                if (val >= 0 && val <= 255)
                {
                    SliderBlueColor.Value = val;
                }
            }
            else
            {
                return;
            }
        }

        private void AssemblingComponentsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AssembleComponentsButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Bitmap result = null;

                if (bmpArray[0] != null && bmpArray[1] != null || string.IsNullOrEmpty(ResultFileTextBox.Text))
                {
                    if (RadioBtn2px.IsChecked == true)
                    {
                        result = cryptoBasic.AssembleComponents(bmpArray, 2, 2, (bool)CheckBoxNoiseReduction.IsChecked);
                    }
                    if (RadioBtn4px.IsChecked == true)
                    {
                        result = cryptoBasic.AssembleComponents(bmpArray, 4, 2, (bool)CheckBoxNoiseReduction.IsChecked);
                    }
                }
                else
                {
                    MessageBox.Show("Nie wczytano wszystkich potrzebnych udziałów lub pole nazwa pliku jest puste!");
                    return;
                }

                if (result != null)
                {
                    result.Save(ResultFileTextBox.Text + ComboBoxImageFormatAssemble.Text);
                    ImageAfterAssemble.Source = ConvertBitmap(result);

                    Process.Start(Directory.GetCurrentDirectory());
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            
        }

        private void LoadComponent1Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string filename="";
            if (openFileDialog.ShowDialog() == true)
            {
                bmpArray[0] = new Bitmap(openFileDialog.FileName);
                filename = openFileDialog.SafeFileName;
            }

            if(bmpArray[0]!=null)
            {
                if(!string.IsNullOrEmpty(filename))
                {
                    Component1LoadInfo.Text = "Wczytano: " + filename;
                    Component1LoadInfo.Foreground = System.Windows.Media.Brushes.Green;
                }
                else
                {
                    Component1LoadInfo.Text = "Nie wczytano: " + filename;
                    Component1LoadInfo.Foreground = System.Windows.Media.Brushes.Red;
                    bmpArray[0] = null;
                }

            }
            else
            {
                MessageBox.Show("Nie wczytano obrazu!");
            }
        }

        private void LoadComponent2Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string filename = "";
            if (openFileDialog.ShowDialog() == true)
            {
                bmpArray[1] = new Bitmap(openFileDialog.FileName);
                filename = openFileDialog.SafeFileName;
            }

            if (bmpArray[1] != null)
            {
                Component2LoadInfo.Text = "Wczytano: " + filename;
                Component2LoadInfo.Foreground = System.Windows.Media.Brushes.Green;
            }
            else
            {
                MessageBox.Show("Nie wczytano obrazu!");
            }
        }

    }
}
