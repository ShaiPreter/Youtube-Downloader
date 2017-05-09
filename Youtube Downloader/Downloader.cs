using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Collections;
using System.Net;
using YoutubeExplode;
using YoutubeExplode.Models;
using MediaToolkit;
using MediaToolkit.Model;

namespace Youtube_Downloader
{
    class Downloader
    {
        public static int Total;
        public static int Current;
        public static async Task<int> videoDownload(String url, String path)
        {
            String id;
            int find = url.IndexOf("&");
            if (find == -1)
            {
               
                id = url.Substring(url.IndexOf("v") + 2);
            }
            else
            {
               
                id = url.Substring(url.IndexOf("v") + 2, find - 9);
            }



            var client = new YoutubeClient();

            // Get video info
            var videoInfo = await client.GetVideoInfoAsync(id);

            // Select the highest quality mixed stream
            var streamInfo = videoInfo.MixedStreams.OrderBy(s => s.VideoQuality).Last();

            // Download it to file
            string fileExtension = streamInfo.Container.GetFileExtension();
            string fileName = $"{videoInfo.Title}.{fileExtension}";
            string file = path + @"\" + fileName;
            Console.WriteLine(file);
            using (var input = await client.GetMediaStreamAsync(streamInfo))
            using (var output = File.Create(file))
                await input.CopyToAsync(output);

            return 1;

        }

        public static async Task<int> audioDownload(String url, String path)
        {
            String id;
            int find = url.IndexOf("&");
            if (find == -1)
            {
                id = url.Substring(url.IndexOf("v") + 2);
            }
            else
            {
                
                id = url.Substring(url.IndexOf("v") + 2, find-9);
            }


            var client = new YoutubeClient();

            // Get video info
            var videoInfo = await client.GetVideoInfoAsync(id);

            // Select the highest quality mixed stream
            var streamInfo = videoInfo.MixedStreams.OrderBy(s => s.VideoQuality).Last();

            // Download it to file
            string fileExtension = streamInfo.Container.GetFileExtension();
            string fileName = $"{videoInfo.Title}.{fileExtension}";
            string file = path + @"\" + fileName;
            Console.WriteLine(file);
            using (var input = await client.GetMediaStreamAsync(streamInfo))
            using (var output = File.Create(file))
                await input.CopyToAsync(output);
            File.SetAttributes(file, FileAttributes.Hidden);


            var inputFile = new MediaFile { Filename = file };
            var outputFile = new MediaFile { Filename = path + @"\" + videoInfo.Title + ".mp3"};

            using (var engine = new Engine())
            {
                engine.Convert(inputFile, outputFile);
            }

            File.Delete(file);

            return 1;

        }


        private static ArrayList getVideoURLs(String url)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.OptionReadEncoding = false;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    htmlDoc.Load(stream, Encoding.UTF8);
                }
            }
            var root = htmlDoc.DocumentNode;
           
            ArrayList urls = new ArrayList();
            foreach (HtmlNode link in root.SelectNodes("//a[@href]").Where(d => d.GetAttributeValue("class", "").Contains("pl-video-title-link")))
            {
                // Get the value of the HREF attribute
               
                    string hrefValue = link.GetAttributeValue("href", string.Empty);
                    urls.Add(hrefValue);
                
            }
           foreach (string u in urls)
            {
                Console.WriteLine(u);
            }
            return urls;
        }

        
        public static async Task downloadAudioPlaylist(String url, String path)
        {
            ArrayList urls = getVideoURLs(url);
            ((MainWindow)System.Windows.Application.Current.MainWindow).ProgressBar.Maximum = urls.Count;
            string totalString = Total.ToString();
            ((MainWindow)System.Windows.Application.Current.MainWindow).label4.Content = "0/" + totalString;
            foreach (string text in urls)
            {
                
                Current += await audioDownload(text, path);
                ((MainWindow)System.Windows.Application.Current.MainWindow).ProgressBar.Value = Current;
                string currentString = Current.ToString();
                ((MainWindow)System.Windows.Application.Current.MainWindow).label4.Content = currentString + "/" + totalString;
            }
        }

        public static async Task downloadVideoPlaylist(String url, String path)
        {
            ArrayList urls = getVideoURLs(url);
            Total = urls.Count;
            ((MainWindow)System.Windows.Application.Current.MainWindow).ProgressBar.Maximum = Total;
            string totalString = Total.ToString();
            ((MainWindow)System.Windows.Application.Current.MainWindow).label4.Content = "0/" + totalString;
            foreach (string text in urls)
            {
                
                Current += await videoDownload(text, path);
                ((MainWindow)System.Windows.Application.Current.MainWindow).ProgressBar.Value = Current;
                string currentString = Current.ToString();
                
                ((MainWindow)System.Windows.Application.Current.MainWindow).label4.Content = currentString + "/" + totalString;
            }
        }

        


    }
}
