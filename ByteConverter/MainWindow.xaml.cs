using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace ByteConverter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.DefaultExt = ".jpg";
            dlg.Filter = "JPEG Files (*.jpg)|*.jpg";

            bool? res = dlg.ShowDialog();

            if (res == true)
            {
                string fname = dlg.FileName;

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource= new Uri(fname);
                bitmap.EndInit();

                MainImage.Source= bitmap;
            }
        }
        string res = null;
        //private void ConverImage_Click(object sender, RoutedEventArgs e)
        //{
        //    BitmapSource bitmapSource = (BitmapSource)MainImage.Source;

        //    byte[] imageBytes;
        //    string binaryString;

        //    using (MemoryStream ms = new MemoryStream()) 
        //    {
        //        BitmapEncoder encoder = new JpegBitmapEncoder();

        //        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
        //        encoder.Save(ms);
        //        imageBytes = ms.ToArray();

        //        binaryString = string.Join("", imageBytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
        //    }

        //    res = string.Join("", binaryString);

        //    BinaryTXT.Content = "Успешно";
            
        //}
        

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(res);
            BinaryTXT.Content = "Скопировано";
        }

        private void ReconvertImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            bool? res = dlg.ShowDialog();

            if (res == true)
            {
                string filepatch = dlg.FileName;

                string binaryString = File.ReadAllText(filepatch);

                int numBytes = binaryString.Length/8;

                byte[] imageBytes = new byte[numBytes];

                for (int i = 0; i < numBytes; i++)
                {
                    imageBytes[i] = Convert.ToByte(binaryString.Substring(i*8,8),2);

                }

                BitmapImage bitmapImage= new BitmapImage();

                using (MemoryStream ms = new MemoryStream(imageBytes)) 
                { 
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption= BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = ms;
                    bitmapImage.EndInit();

                }

                MainImage.Source = bitmapImage;
            }
        }

        private void SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource bitmapSource = (BitmapSource)MainImage.Source;

            byte[] imageBytes;
            string binaryString;

            using (MemoryStream ms = new MemoryStream())
            {
                BitmapEncoder encoder = new JpegBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(ms);
                imageBytes = ms.ToArray();

                binaryString = string.Join("", imageBytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
            }

            
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.DefaultExt = ".txt";

            
            if (saveFileDialog.ShowDialog() == true)
            {
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                {
                    writer.Write(binaryString);
                }

                BinaryTXT.Content = "Успешно";
            }
        }

        private void SaveDoneImage_Click(object sender, RoutedEventArgs e)
        {
            
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "JPEG Image (*.jpg)|*.jpg";
            dialog.DefaultExt = ".jpg";
            dialog.AddExtension = true;

            
            if (dialog.ShowDialog() == true)
            {
                
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                                
                BitmapFrame frame = BitmapFrame.Create((BitmapSource)MainImage.Source);
                encoder.Frames.Add(frame);
                                
                using (FileStream stream = new FileStream(dialog.FileName, FileMode.Create))
                {
                    encoder.Save(stream);
                }
            }
        }
    }
}
