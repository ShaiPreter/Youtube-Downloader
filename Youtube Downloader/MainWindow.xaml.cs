using System;
using System.Collections.Generic;
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
using System.Windows.Forms;

namespace Youtube_Downloader
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

        private void buttonPath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();
            d.ShowDialog();
            String f = d.SelectedPath;
            Console.WriteLine(f);
            Path.Text = f;
        }

        private void downloadButton_Click(object sender, RoutedEventArgs e)
        {

            if (radioVideo.IsChecked == false && radioAudio.IsChecked == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select a file type.");
            }
            String path = Path.Text;
            String url = URL.Text;

            if (url == "" || url == null)
            {
                System.Windows.Forms.MessageBox.Show("Please enter a URL.");
            }
            if (path == " || path == null")
            {
                System.Windows.Forms.MessageBox.Show("Please select a folder to download file(s) to.");
            }
            if (url.IndexOf("playlist") == -1 && radioAudio.IsChecked == true)
            {
                Console.WriteLine("Audio file");
                Downloader.audioDownload(url, path);
            }
            else if (url.IndexOf("playlist") != -1 && radioAudio.IsChecked == true)
            {
                Console.WriteLine("Audio playlist");
                Downloader.downloadAudioPlaylist(url, path);
            }
            else if (url.IndexOf("playlist") == -1 && radioVideo.IsChecked == true)
            {
                Console.WriteLine("Video file");
                Downloader.videoDownload(url, path);
            }
            else if (url.IndexOf("playlist") != -1 && radioVideo.IsChecked == true)
            {
                Console.WriteLine("Video Playlist");
                Downloader.downloadVideoPlaylist(url, path);
            }
           
        }
    }
}
