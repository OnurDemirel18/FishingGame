using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;

namespace Fishing_Game
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    #region Main
    public partial class MainWindow : Window
    {
        /// <summary>
        /// gametimer oluşturuldu.
        /// gereksiz itemleri silmek için itemstoremove adında liste oluşturuldu.
        /// </summary>

        DispatcherTimer gametimer = new DispatcherTimer();

        int score = 0;
        int fishCounter = 50;
        int limit = 100;
        List<Rectangle> itemstoremove = new List<Rectangle>();

        /// <summary>
        /// MainWindow adında hazır bir metot var içerisinde müzik oynatıldı.
        /// gametimer değerleri verilip başlatıldı.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Mainmusic.Play();

            gametimer.Interval = TimeSpan.FromMilliseconds(10);
            gametimer.Tick += gameEngine;

            gametimer.Start();

            MyCanvas.Focus();

            


        }
        #endregion

        #region MouseMoveHandler

        /// <summary>
        /// MouseMoveHandler adında parametre alan bir metot oluşturuldu.
        /// İçerisinde mousenin o anki kordinatlarını alan kodlar yazıldı.
        /// </summary>
        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            // Get the x and y coordinates of the mouse pointer.
            System.Windows.Point position = e.GetPosition(this);
            double pX = position.X;
            double pY = position.Y;

            // Sets the Height/Width of the circle to the mouse coordinates.
            line.X2 = pX;
            line.Y2 = pY;
        }
        #endregion

        #region Make metodları

        /// <summary>
        /// MakeRob adında oltanın iğnesini çizdirmek için bir metot oluşturuldu.
        /// İçerisinde bir rectangle oluşturuldu ve fill ine resim atandı.
        /// Canvas.Set ile ekrana belirli kordinasyonlara çizdirildi.
        /// listeye eklendi.
        /// </summary>
        public void MakeRob()
        {
            ImageBrush rob = new ImageBrush();

            rob.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/olta.png"));

            Rectangle newRob = new Rectangle
            {               
                Tag = "olta",
                Height = 25,
                Width = 30,
                Fill = rob
            };

            Canvas.SetLeft(newRob, line.X2 - 35);
            Canvas.SetTop(newRob, line.Y2 - 3);

            MyCanvas.Children.Add(newRob);
        }

        /// <summary>
        /// MakeFishes adında balıkları çizdirmek için bir metot oluşturuldu.
        /// Switch ile gelecek balık ayarlandı.
        /// 2 Adet rectangle oluşturularak balıkların resimleri rectangle a dolduruldu.
        /// Canvas.Set ile ekrana belirli kordinasyonlara çizdirildi.
        /// listeye eklendi.
        /// </summary>
        public void MakeFishes()
        {
            ImageBrush fishLeft = new ImageBrush();
            ImageBrush fishRight = new ImageBrush();

            Random rand = new Random();
            int fishLeftCounter;
            fishLeftCounter = rand.Next(1, 3);

            switch (fishLeftCounter)
            {
                case 1:
                    fishLeft.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/fish.png"));
                    break;
                case 2:
                    fishLeft.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/fish2.png"));
                    break;
            }

            fishRight.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/fish3.png"));

            Rectangle Fishright = new Rectangle
            {
                Tag = "right",
                Height = 30,
                Width = 50,
                Fill = fishRight
            };

            Rectangle Fishleft = new Rectangle
            {               
                Tag = "left",
                Height = 30,
                Width = 70,
                Fill = fishLeft
            };


            Canvas.SetRight(Fishright, -50);
            Canvas.SetTop(Fishright, rand.Next(150, 350));

            Canvas.SetLeft(Fishleft, -100);
            Canvas.SetTop(Fishleft, rand.Next(150, 350));

            MyCanvas.Children.Add(Fishright);
            MyCanvas.Children.Add(Fishleft);

            

        }
        #endregion

        #region GameEngine

        /// <summary>
        /// gameEngine Adında bir metot oluşturuldu.
        /// içerisinde MakeRob metodu çağırıldı.
        /// foreach ile balıkarın ve oltanın hareket etmesi sağlandı.
        /// diğer bir foreach ile balıkların tutulması kontrol edildi ve tutulduğunda score artırıldı.
        /// tutulan balıklar itemstoremove listesine eklendi.
        /// belirli bir skora ulaştığında işlemler yapılabilmesi için if kullanıldı.
        /// if içerisine girdiğinde
        /// müzik durduruldu.
        /// gametimer durduruldu.
        /// arka plan resmi değiştirildi.
        /// bazı bileşenlerin visible değerleri değiştirildi.
        /// skor dosyaya yazdırıldı.
        /// foreach ile itemstoremove listesindeki elemanlar tek tek silindi.
        /// son olarak da çöp toplayıcı çağrılarak bellek temizlendi
        /// </summary>
        private void gameEngine(object sender, EventArgs e)
        {
 
            fishCounter--;

            MakeRob();

            if (fishCounter < 0)
            {

                MakeFishes();
                fishCounter = limit; 
            }

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {

                if (x is Rectangle && (string)x.Tag == "left")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 0.1);
                    Canvas.SetLeft(x, Canvas.GetLeft(x) + 3);

                    Rect fish = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (Canvas.GetLeft(x) > 720)
                    {
                        itemstoremove.Add(x);
                    }

                }


                if (x is Rectangle && (string)x.Tag == "right")
                {

                    Canvas.SetTop(x, Canvas.GetTop(x) + 0.1);
                    Canvas.SetRight(x, Canvas.GetRight(x) + 6);

                    Rect fish2 = new Rect(Canvas.GetRight(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (Canvas.GetRight(x) > 720)
                    {
                        itemstoremove.Add(x);
                    }

                }
                if (x is Rectangle && (string)x.Tag == "olta")
                {

                    Canvas.SetLeft(x, line.X2 - 22);
                    Canvas.SetTop(x, line.Y2 - 3);

                    Rect olta = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    foreach (var y in MyCanvas.Children.OfType<Rectangle>())
                    {

                        if (((string)y.Tag == "left") || ((string)y.Tag == "right"))
                        {
                            Rect fishright = new Rect(Canvas.GetRight(y), Canvas.GetTop(y), y.Width, y.Height);
                            Rect fishleft = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

                            if (olta.IntersectsWith(fishleft))
                            {
                                itemstoremove.Add(y);
                                score++;
                            }

                            if (olta.IntersectsWith(fishright) && ((string)y.Tag == "right"))
                            {
                                itemstoremove.Add(y);
                                score++;
                            }                        
                        }
                    }
                }
            } 

            if (score > 1000)
            {
                line.Visibility = Visibility.Hidden;
                Mainmusic.Stop();
                gametimer.Stop();
                ImageBrush bck = new ImageBrush();
                bck.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Sahil.png"));
                MyCanvas.Background = bck;               
                lbl.Visibility = Visibility.Visible;
                btn.Visibility = Visibility.Visible;

                FileStream fs = new FileStream("C:\\Users\\Bilgi\\source\\repos\\Fishing_Game\\Fishing_Game\\Veri\\score.txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(score);
                sw.Flush();
                sw.Close();
                fs.Close();
            }

            foreach (Rectangle y in itemstoremove)
            {
                MyCanvas.Children.Remove(y);
            }

            GC.Collect();
        }

        /// <summary>
        /// mouse nin double click eventine uygulamayı kapatma ve yeniden başlatma işlemleri yapıldı.
        /// </summary>
        private void btn_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
    #endregion
}
