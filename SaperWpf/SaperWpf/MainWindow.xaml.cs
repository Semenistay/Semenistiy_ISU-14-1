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
using System.Collections;
using System.Windows.Threading;

namespace SaperWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int H = 10;
        int W = 10;
        int K = 10;
        Button[,] btns;
        int[,] nums;
        TextBox inputK;
        TextBox inputW;
        TextBox inputH;
        Label lblSeconds;
        DispatcherTimer dispatcherTimer;
        TextBlock Info1;
        int time;
        int left_bombs;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            wrapPanel1.Margin = new Thickness(20, 20, 0, 0);
            //кнопка новая игра
            Button newGame = new Button();
            newGame.Height = 50;
            newGame.Width = 100;
            newGame.Content = "Новая игра";
            newGame.Margin = new Thickness(20, 20, 0, 0);
            newGame.Click += new RoutedEventHandler(newgame_click);
            MainWrap.Children.Add(newGame);
            //-----------------
            //поля для новой игры
            inputW = new TextBox();
            inputW.Width = 30;
            inputW.Height = 20;
            inputW.Margin = new Thickness(10, 10, 0, 0);
            MainWrap.Children.Add(inputW);
            inputH = new TextBox();
            inputH.Width = 30;
            inputH.Height = 20;
            inputH.Margin = new Thickness(10, 10, 0, 0);
            MainWrap.Children.Add(inputH);
            inputK = new TextBox();
            inputK.Width = 30;
            inputK.Height = 20;
            inputK.Margin = new Thickness(10, 10, 0, 0);
            MainWrap.Children.Add(inputK);
            TextBlock Info = new TextBlock();
            Info.Text = " Задайте высоту, ширину и количество \n мин и нажмите 'Новая игра' \n Минимум: 7*7 и 5 бомб. \n Максимум: 20*20";
            Info.Margin = new Thickness(0, 0, 0, 0);
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            wrapPanel2.Children.Add(Info);
            //-----------------
            //Timer
            
            lblSeconds = new Label();
            lblSeconds.Content = "Ваше время:  " + 0;
            wrapPanel2.Children.Add(lblSeconds);
            //-------
            //-left
            Info1 = new TextBlock();
            Info1.Text = "";
            Info1.Margin = new Thickness(10, 10, 0, 0);

            wrapPanel2.Children.Add(Info1);
            //-----
            
            CreateField(H,W,K);
           
            
        }
        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            time++;
            lblSeconds.Content = "Ваше время:  " + (time);
            
            CommandManager.InvalidateRequerySuggested();
        }
        Button create_Btn(int i, int j)
        {
            Button btn = new Button();
            btn.Content = "";
            btn.Height = 30;
            btn.Width = 30;
            btn.Name = "A" + i.ToString() + "A" + j.ToString();
            btn.Click += new RoutedEventHandler(x_mouse);
            btn.MouseRightButtonDown += new MouseButtonEventHandler(x_right);
            return btn;
        }

        void CreateField(int h, int w, int k)
        {
            left_bombs = K;
            Info1.Text = "Осталось угадать: " + K + " бомб.";
            lblSeconds.Content = "Ваше время:  " + 0;
            wrapPanel1.Children.Clear();
            time = 0;
            dispatcherTimer.Stop();
            btns = new Button[h,w];
            nums = new int[h,w];
           
            //заполняем поле минами
            //-1 - мина
            for (int i = 0; i < h; i++) 
                for (int j = 0; j < w; j++)
                {
                    nums[i, j] = 0;
                }
            pair[] all = new pair[h * w];
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    all[i * w + j].first = i;
                    all[i * w + j].second = j;
                }
            }
            shuffle(all);
            for (int i = 0; i < k; i++)
            {
                nums[all[i].first, all[i].second] = -1;
            }
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if (nums[i, j] != -1)
                    {
                        nums[i, j] = cnt(i, j);    
                    }
                }
            }

            //добавляем на форму поле в виде кнопок
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    btns[i, j] = new Button();
                   
                    
                }
            }

            wrapPanel1.Height = H * 30;
            wrapPanel1.Width = W * 30;
            for (int i = 0; i < H; i++)
            {
                for (int j = 0; j < W; j++)
                {
                    btns[i, j] = create_Btn(i, j);
                    wrapPanel1.Children.Add(btns[i, j]);
                }
            }
            MyWindow.Height = H * 30 + 150;
            MyWindow.Width = W * 30 + 300;
            MyWindow.ResizeMode = System.Windows.ResizeMode.NoResize;
        }

        int cnt(int i, int j)
        {
            int count = 0;
            if (i > 0 && j > 0 && nums[i - 1, j - 1] == -1) count += 1;
            if (i > 0 && nums[i - 1, j] == -1) count += 1;
            if (j > 0 && nums[i, j - 1] == -1) count += 1;
            if (i < H - 1 && nums[i + 1, j] == -1) count += 1;
            if (j < W - 1 && nums[i, j + 1] == -1) count += 1;
            if (i < H - 1 && j < W - 1 && nums[i + 1, j + 1] == -1) count += 1;
            if (i < H - 1 && j > 0 && nums[i + 1, j - 1] == -1) count += 1;
            if (i > 0 && j < W - 1 && nums[i - 1, j + 1] == -1) count += 1;
            return count;
        }

        struct pair
        {
            public int first;
            public int second;
            public pair(int x, int y)
            {
                first = x;
                second = y;
            }
        };
        pair get_i_j(string s)
        {
            pair r;
            
            int i = 0;
            int j = 0;
            int d = 1;
            while (s[s.Length - 1] != 'A')
            {
                
                i += ((int)s[s.Length - 1] - 48) * d;
                s = s.Remove(s.Length - 1, 1);
                d *= 10;
            }
            s = s.Remove(s.Length - 1, 1);
            d = 1;
            while (s.Length != 1)
            {
                
                j += ((int)s[s.Length - 1] - 48) * d;
                s = s.Remove(s.Length - 1, 1);
                d *= 10;
            }
            r.first = j;
            r.second = i;
            return r;            
        }
        void shuffle(pair[] a)
        {
            Random rnd = new Random();
            for (int i = 0; i < 10000; i++)
            {
                int i1 = rnd.Next(0, H * W - 1);
                int j1 = rnd.Next(0, H * W - 1);
                pair r = a[i1];
                a[i1] = a[j1];
                a[j1] = r;
            }
        }
        void trygo(pair[] q, ref int u, int i, int j)
        {
            if (i < 0 || j < 0 || i >= H || j >= W || !btns[i, j].IsEnabled) return;
            q[u] = new pair();
            q[u].first = i;
            q[u].second = j;
            u++;
            btns[i, j].Content = "";
            btns[i, j].IsEnabled = false;
        }
        int toInt(string s)
        {
            if (s.Length > 3) return 100;
            int ans = 0;
            for (int i = 0; i < s.Length; i++)
            {
                ans *= 10;
                int r = (int)s[i] - 48;
                if (r < 0 || r > 9) return 100;
                ans += r;
            }
            return ans;
        }
        void newgame_click(object sender, RoutedEventArgs e)
        {
            H = toInt(inputH.Text);
            W = toInt(inputW.Text);
            K = toInt(inputK.Text);
            if (H > 20 || W > 20 || K > W * H || H<7 || W<7 || K<5)
            {
                H = 10;
                W = 10;
                K = 20;
            }
            CreateField(H,W,K);
        }

        void x_right(object sender, MouseButtonEventArgs e){

            dispatcherTimer.Start();
            Button x = (Button)sender;
            pair r = get_i_j(x.Name);
            if (btns[r.first, r.second].Content == "")
            {
                if (left_bombs == 0) return;
                left_bombs--;
                btns[r.first, r.second].Content = "?";
                Info1.Text = "Осталось угадать: " + left_bombs + " бомб.";
            }
            else
            {
                left_bombs++;
                btns[r.first, r.second].Content = "";
                Info1.Text = "Осталось угадать: " + left_bombs + " бомб.";
            }
        }

        int count_l()
        {
            int ans = 0;
            for (int i = 0; i < H; i++)
            {
                for (int j = 0; j < W; j++)
                {
                    if (btns[i, j].IsEnabled) ans++;
                }
            }
            return ans;
        }

        void x_mouse(object sender, RoutedEventArgs e)
        {

            dispatcherTimer.Start();
            Button x = (Button)sender;
            pair r = get_i_j(x.Name);
            if (nums[r.first, r.second] == -1)
            {
                dispatcherTimer.Stop();
                if (x.Content == "?") return;
                for (int i = 0; i < H; i++)
                {
                    for (int j = 0; j < W; j++)
                    {
                        
                        if (nums[i, j] == -1)
                        {
                            btns[i, j].Background = new SolidColorBrush(Colors.Red);
                            btns[i, j].Content = "Θ";

                        }
                        else
                        {
                            btns[i, j].IsEnabled = false;
                        }

                    }
                }
                MessageBox.Show("Вы проиграли", "Результат");
                return;
            }
            int uk = 1, un = 0;
            pair[] queue = new pair[H * W * 10];
            queue[0] = r;
            while (uk != un)
            {
                pair t = queue[un];
                un++;
                if (nums[t.first, t.second] == 0)
                {
                    btns[t.first, t.second].Content = "";
                    btns[t.first, t.second].IsEnabled = false;
                    trygo(queue, ref uk, t.first - 1, t.second - 1);
                    trygo(queue, ref uk, t.first - 1, t.second);
                    trygo(queue, ref uk, t.first - 1, t.second + 1);
                    trygo(queue, ref uk, t.first, t.second + 1);
                    trygo(queue, ref uk, t.first + 1, t.second + 1);
                    trygo(queue, ref uk, t.first + 1, t.second);
                    trygo(queue, ref uk, t.first + 1, t.second - 1);
                    trygo(queue, ref uk, t.first, t.second - 1);
                }
                else
                {
                    btns[t.first, t.second].Content = nums[t.first, t.second].ToString();
                    btns[t.first, t.second].IsEnabled = false;
                }
            }
            int countL = count_l();
            if (countL == K)
            {
                dispatcherTimer.Stop();
                MessageBox.Show("Вы выйграли за " + (time) + " секунд!!!", "Результат");
            }
        }
    }
}
