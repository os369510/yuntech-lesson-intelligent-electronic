using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Research.Kinect.Nui;
using Coding4Fun.Kinect.Wpf;
using System.Drawing;
using System.Reflection;

namespace Body_embedded
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        Runtime nui; // 建立一個 Runtime 物件，代表 Kinect 設備

        byte[] backgroundImage = new byte[640 * 480 * 3]; //建置背景圖片陣列空間
        byte[] colorImage; //建置彩色圖片
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //讀取背景圖片
            string bg_path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\..\..\bg.bmp";
            //將背景圖片bmp檔的路徑找出存到bg_path
            Bitmap bmp = new Bitmap(bg_path);
            //封裝bg_path成為一個影像物件
            ImageConverter ic = new ImageConverter();
            //創建一個用來轉換圖片的方法
            byte[] buffer = (byte[])ic.ConvertTo(bmp, typeof(byte[]));
            for (int i = 0; i < backgroundImage.Length; i++)
            {
                backgroundImage[i] = buffer[i + 54];
                // BMP 檔前 54 bytes 是標頭 ,將BMP檔轉為無標頭檔的一般陣列
            }

            nui = Runtime.Kinects[0];

            //初始化 NUI API 
            nui.Initialize(RuntimeOptions.UseDepthAndPlayerIndex | RuntimeOptions.UseColor);
            //RuntimeOptions.UseColor用來擷取彩色影像串流
            //RuntimeOptions.UseDepthAndPlayerIndex用來擷取影像深度
            //串流資料是以影像畫格 (frame) 的方式得到
            nui.VideoFrameReady += new EventHandler<ImageFrameReadyEventArgs>(nui_VideoFrameReady);
            //事件 當每擷取到一張畫面時就傳到VideoFrameReady事件裡
            nui.DepthFrameReady += new EventHandler<ImageFrameReadyEventArgs>(nui_DepthFrameReady);
            //事件 當每擷取到一張畫面時就傳到DepthFrameReady事件裡

            //開啓串流時要指定額外的資訊
            nui.VideoStream.Open(ImageStreamType.Video, 2, ImageResolution.Resolution640x480, ImageType.Color);
            //命令kinect開始擷取影像串流nui.VideoStream.Open(影像串流類型,緩衝區數量1~4,影像解析度,取得影像的格式)
            nui.DepthStream.Open(ImageStreamType.Depth, 2, ImageResolution.Resolution320x240, ImageType.DepthAndPlayerIndex);
            //命令kinect開始擷取影像串流nui.DepthStream.Open(影像串流類型,緩衝區數量1~4,影像解析度,取得影像的格式) 
            //在這用DepthAndPlayerIndex代表取得的是影像深度+玩家編號格式
        }

        void nui_DepthFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            image2.Source = e.ImageFrame.ToBitmapSource();
            //將深度資訊轉換成BMP檔到image2上
            PlanarImage data = e.ImageFrame.Image;
            //宣告一個data陣列指標指到深度影像資料
            byte[] coloredFrame = GenerateColoredBytes(data);
            //將深度影像傳送到GenerateColoredBytes副函式作處理, 產生一張新畫面存到coloredFrame裡
            image3.Source = BitmapSource.Create(data.Width, data.Height, 96, 96, PixelFormats.Bgr32, null, coloredFrame, data.Width * PixelFormats.Bgr32.BitsPerPixel / 8);
            //將coloredFrame影像轉成BMP檔存到image3.Source
        }

        void nui_VideoFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            image1.Source = e.ImageFrame.ToBitmapSource();
            //使用Codeing4Fun函式庫,將圖片秀在image1
            colorImage = e.ImageFrame.Image.Bits;
            //把圖片的bit陣列存在colorImage
        }

        int GetPlayerIndex(byte first)//Block3
        {
            //回傳這個像素點的玩家編號
            return (int)first & 7;
        }

        byte[] GenerateColoredBytes(PlanarImage img) //將黑白的深度影像轉換成彩色深度影像
        {
            //擷取影像相關參數
            int width = img.Width;
            int height = img.Height;
            byte[] depthData = img.Bits;
            //每一個像素有 紅 + 綠 + 藍 + 空byte = 4 byte = 32 bits
            //BGR32 = 紅 + 綠 + 藍 + 空byte
            byte[] colorFrame = new byte[width * height * 4];
            //宣告每個顏色的位置索引
            const int Blue_index = 0;  //當用到Blue_index他會自動看為0
            const int Green_index = 1;
            const int red_index = 2;
            const int Alphalindex = 3;
            int depthindex = -32 * width;
            //修正 Kinect 深度資料的垂直偏移，也就是說要少掉幾列

            for (int y = 0; y < height; y++) //列迴圈
            {
                 for (int x = 0; x < width; x++) //行迴圈
                 {
                      int hightOffset = y * width;
                      //每一列最左邊的像素位置
                      int index = (x + hightOffset) * 4; 
                      //計算每行目前像素的位置，一個像素為4個byte(BGRA)
                      int background_index = (x * 2 + (height - y - 1) * width * 4) * 3;    
                      //BGR，計算被讀取的圖片的行初始值
                      int colorIndex = (x * 2 + hightOffset * 4) * 4;
                      //BGRA，計算儲存好的彩色圖片的行的初始值
                      if (depthindex >= 0 && GetPlayerIndex(depthData[depthindex]) > 0) 
                      //如果有偵測到玩家的話
                      { //填入偵測到的玩家圖片
                        colorFrame[index + Blue_index] = colorImage[colorIndex + Blue_index +48];
                        //要再修正左右偏差 大概4*12左右
                        colorFrame[index + Green_index] = colorImage[colorIndex + Green_index + 48]; //要再修正左右偏差
                        colorFrame[index + red_index] = colorImage[colorIndex + red_index + 48]; //要再修正左右偏差
                        colorFrame[index + Alphalindex] = colorImage[colorIndex + Alphalindex + 48]; //要再修正左右偏差
                      }
                      else //如果沒有偵測到玩家的話
                      {
                          //填入被讀取的背景圖片
                          colorFrame[index + Blue_index] = backgroundImage[background_index + Blue_index];
                          colorFrame[index + Green_index] = backgroundImage[background_index + Green_index];
                          colorFrame[index + red_index] = backgroundImage[background_index + red_index];
                      }
                      depthindex += 2;
                     //深度計算，每一個像素 2 個 bytes
                 }
            }
            return colorFrame;
        }



        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            nui.Uninitialize(); //關閉NUI API 
        }
    }
}
