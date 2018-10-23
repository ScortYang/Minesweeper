using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Minesweeper
{
    /// <summary>
    /// Gamescreen.xaml 的交互逻辑
    /// </summary>
    public partial class Gamescreen : Window
    {
        public Gamescreen(int a, int b)
        {
            Theme = b;
            InitializeComponent();
            CurrentLevel = a;
            OriginLevel = a;
            SafeNum = 0;
            Score = 0;
            LifeNum = 5;
            ClickTimer.Elapsed += new ElapsedEventHandler(ClickOnTimedEvent);
            ClickTimer.AutoReset = true;
            ClickTimer.Enabled = true;
            InitTimer.Elapsed += new ElapsedEventHandler(InitOnTimedEvent);
            InitTimer.AutoReset = true;
            InitTimer.Enabled = true;
            Init();
            Background = (b == 1) ? new SolidColorBrush(Color.FromRgb(187, 187, 187)) : new SolidColorBrush(Color.FromRgb(18, 103, 72));
            mnu.Background = (b == 1) ? new SolidColorBrush(Color.FromRgb(255, 255, 255)) : new SolidColorBrush(Color.FromRgb(27, 124, 89));
            NumBdr.Background = (b == 1) ? new SolidColorBrush(Color.FromRgb(170, 170, 170)) : new SolidColorBrush(Color.FromRgb(39, 172, 123));
            NumBdr.BorderBrush = (b == 1) ? new SolidColorBrush(Color.FromRgb(116, 116, 116)) : new SolidColorBrush(Color.FromRgb(0, 108, 5));
        }

        Style CMB = new Style(typeof(Button));
        MineButton[,] MineArry = new MineButton[25, 10];//按钮容器
        Random ra = new Random();//随机数种子
        Timer ClickTimer = new Timer(200);
        Timer InitTimer = new Timer(50);

        private static int OriginLevel;
        private const int MINEBUTTONSCOUNTX = 25;
        private const int MINEBUTTONSCOUNTY = 10;
        private const int MINEBUTTONWIDTH = 32;
        private int Theme;
        private int CurrentLevel;
        private int Score;
        private int BoomNum;
        private int LifeNum;
        private int SafeNum;
        private int FirstClickCx, FirstClickCy;
        private bool isFirstClick = false;
        private bool isLevelOver = false;
        private bool isGameOver = false;
        private bool isAroundSetable = false;

        private void Init()
        {
            GridMines.Children.Clear();
            InitMineButton();
            SetScoreImg();
            InitBoom();
            SetLevelImg();
            SetLifeImg();
            SafeNum = 0;
        }

        private void Restart()
        {
            CurrentLevel = OriginLevel;
            Score = 0;
            BoomNum = 0;
            LifeNum = 5;
            SafeNum = 0;
            isFirstClick = false;
            isGameOver = false;
            Init();
            SetScoreImg();
        }

        private void InitMineButton()
        {
            for (int i = 0; i < MINEBUTTONSCOUNTX * MINEBUTTONSCOUNTY; i++)
            {
                MineButton NewMineButton = new MineButton
                {
                    cx = i % MINEBUTTONSCOUNTX,
                    cy = i / MINEBUTTONSCOUNTX,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };
                NewMineButton.Margin = new Thickness(NewMineButton.cx * MINEBUTTONWIDTH, NewMineButton.cy * MINEBUTTONWIDTH, 0, 0);
                NewMineButton.ButtonImage.Source = (Theme == 1) ? new BitmapImage(new Uri("pack://application:,,,/img/方块.bmp", UriKind.RelativeOrAbsolute)) : new BitmapImage(new Uri("pack://application:,,,/img/方块.png", UriKind.RelativeOrAbsolute));
                NewMineButton.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(LeftDown), true);
                NewMineButton.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(LeftUp), true);
                NewMineButton.MouseRightButtonUp += new MouseButtonEventHandler(RightUp);
                NewMineButton.MouseRightButtonDown += new MouseButtonEventHandler(RightDown);
                NewMineButton.MouseLeave += new MouseEventHandler(MouseLeft);
                MineArry[NewMineButton.cx, NewMineButton.cy] = NewMineButton;
                GridMines.Children.Add(NewMineButton);
            }
        }

        private void InitBoom()
        {
            BoomNum = CurrentLevel * 2 + 18;
            isFirstClick = false;
            int i = 1;
            while (i <= BoomNum)
            {
                int x = ra.Next(0, 25);
                int y = ra.Next(0, 10);
                if (MineArry[x, y].isboom == false)
                {
                    MineArry[x, y].isboom = true;
                    i++;
                }
            }
            SetBoomImg();
        }

        private void MouseLeft(object sender, MouseEventArgs e)
        {
            MineButton p = sender as MineButton;
            if (!p.bclick)
            {
                if (!p.bflag)
                    SetButtonImage(p, -3);
            }
            else
            {
                if (!p.bflag)
                    SetAround(p, -3);
            }
            p.isRightDown = false;
            p.isLeftDown = false;
            isAroundSetable = false;
            ClickTimer.Stop();
        }

        private void LeftDown(object sender, MouseButtonEventArgs e)
        {
            if (isGameOver)
                return;
            if (isLevelOver)
                return;
            MineButton p = sender as MineButton;
            p.isLeftDown = true;
            if (!p.bclick)
            {
                if (!p.bflag)
                    SetButtonImage(p, -2);
            }
            else
            {
                if (p.isLeftDown && p.isRightDown)
                    SetAround(p, -2);
            }
            return;
        }

        private void LeftUp(object sender, MouseButtonEventArgs e)
        {
            if (isGameOver)
                return;
            MineButton p = sender as MineButton;
            if (p.isLeftDown)
            {
                if (p.isRightDown)
                {
                    isAroundSetable = true;
                    ClickTimer.Start();
                }
                if (!isAroundSetable)
                    ClickButton(p);
            }
            p.isLeftDown = false;
        }

        private void ClickButton(MineButton p)                            //左键动作
        {
            if (isLevelOver) return;
            if (p.bclick) return;
            if (p.bflag) return;
            if (p.isboom)
            {
                if (isFirstClick == false)//保证第一次点不到炸弹
                {
                    FirstClickCx = p.cx;
                    FirstClickCy = p.cy;
                    Init();
                    LifeNum++;
                    ClickButton(MineArry[FirstClickCx, FirstClickCy]);
                }

                p.bclick = true;
                LifeNum--;
                SetLifeImg();
                SetButtonImage(p, -1);
                if (LifeNum == 0)
                {
                    isGameOver = true;
                    MessageBox.Show("游戏结束，您的得分是" + Score.ToString() + "分");
                    return;
                }
                BoomNum--;
                SetBoomImg();
                return;
            }
            isFirstClick = true;
            Score += 1;
            SetScoreImg();
            p.bclick = true;
            SafeNum++;
            if (SafeNum == 250 - (CurrentLevel * 2 + 18))
            {
                int moreScore = 0;
                for (int i = 0; i < MINEBUTTONSCOUNTX; i++)
                {
                    for (int j = 0; j < MINEBUTTONSCOUNTY; j++)
                    {
                        if (!MineArry[i, j].bclick)
                        {
                            moreScore += 5;
                        }
                    }
                }
                MessageBox.Show("通过成功,本局额外加分" + moreScore.ToString() + "!");
                Score += moreScore;
                CurrentLevel++;
                LifeNum++;
                SetLifeImg();
                InitTimer.Start();
                p.isLeftDown = false;
                isLevelOver = true;
                Init();
                return;
            }
            int ncount = CountBoom(p);
            if (ncount == 0) //递归消除空格
            {
                if (p.cx > 0)
                    ClickButton(MineArry[p.cx - 1, p.cy]);
                if (p.cx > 0 && p.cy > 0)
                    ClickButton(MineArry[p.cx - 1, p.cy - 1]);
                if (p.cy > 0)
                    ClickButton(MineArry[p.cx, p.cy - 1]);
                if (p.cy > 0 && p.cx < MINEBUTTONSCOUNTX - 1)
                    ClickButton(MineArry[p.cx + 1, p.cy - 1]);
                if (p.cx < MINEBUTTONSCOUNTX - 1)
                    ClickButton(MineArry[p.cx + 1, p.cy]);
                if (p.cx > 0 && p.cy < MINEBUTTONSCOUNTY - 1)
                    ClickButton(MineArry[p.cx - 1, p.cy + 1]);
                if (p.cy < MINEBUTTONSCOUNTY - 1)
                    ClickButton(MineArry[p.cx, p.cy + 1]);
                if (p.cx < MINEBUTTONSCOUNTX - 1 && p.cy < MINEBUTTONSCOUNTY - 1)
                    ClickButton(MineArry[p.cx + 1, p.cy + 1]);
            }
            if (ncount == 0)
                SetButtonImage(p, 10);
            else
                SetButtonImage(p, ncount);
        }

        private void RightDown(object sender, MouseButtonEventArgs e)
        {
            MineButton p = sender as MineButton;
            if (isLevelOver)
                return;
            p.isRightDown = true;
            if (!p.bclick)
            {
                if (!p.bflag)
                    SetButtonImage(p, -2);
            }
            else
                if (p.isLeftDown && p.isRightDown)
                SetAround(p, -2);
        }

        private void RightUp(object sender, RoutedEventArgs e)            //右键动作
        {
            if (isGameOver)
                return;
            if (isLevelOver)
                return;
            MineButton p = sender as MineButton;
            if (p.isRightDown)
            {
                if (p.isLeftDown)
                {
                    isAroundSetable = true;
                    ClickTimer.Start();
                }
                SetAround(p, -3);
                if (p.bclick)
                {
                    if (isAroundSetable)
                    {
                        if (CountBoom(p) == CountFlag(p))
                        {
                            ClickAround(p);
                            return;
                        }
                        return;
                    }
                    return;
                }
                if (p.bflag)
                {
                    p.bflag = false;
                    SetButtonImage(p, -3);
                    Score = (Score > CurrentLevel * CurrentLevel * 10) ? Score - CurrentLevel * CurrentLevel * 10 : 0;
                    SetScoreImg();
                    BoomNum++;
                    SetBoomImg();
                }
                else
                {
                    if (BoomNum <= 0)
                    {
                        MessageBox.Show("旗子个数已经达到上限，请修改错误位置的旗子");
                        return;
                    }
                    p.bflag = true;
                    BoomNum--;
                    SetButtonImage(p, 11);
                    SetBoomImg();
                    if (BoomNum == 250 - SafeNum && SafeNum == 250 - (CurrentLevel * 2 + 18))
                    {
                        //for (int i = 0; i < MINEBUTTONSCOUNTX; i++)
                        //    for (int j = 0; j < MINEBUTTONSCOUNTY; j++)
                        //        if (MineArry[i, j].bflag && !MineArry[i, j].isboom)
                        //            return;
                        int moreScore = 0;
                        for (int i = 0; i < MINEBUTTONSCOUNTX; i++)
                            for (int j = 0; j < MINEBUTTONSCOUNTY; j++)
                                if (!MineArry[i, j].bclick)
                                    moreScore += 5;

                        MessageBox.Show("通过成功,本局额外加分" + moreScore.ToString() + "!");
                        isLevelOver = true;
                        p.isRightDown = false;
                        Score += moreScore;
                        InitTimer.Start();
                        CurrentLevel++;
                        LifeNum++;
                        SetLifeImg();
                        Init();
                        return;
                    }
                }
            }
            p.isRightDown = false;
            return;
        }

        private void ClickAround(MineButton p)
        {
            if (isLevelOver)
                return;
            if (p.cx > 0)
            {
                ClickButton(MineArry[p.cx - 1, p.cy]);
            }
            if (p.cx > 0 && p.cy > 0)
            {
                ClickButton(MineArry[p.cx - 1, p.cy - 1]);
            }
            if (p.cy > 0)
            {
                ClickButton(MineArry[p.cx, p.cy - 1]);
            }
            if (p.cy > 0 && p.cx < MINEBUTTONSCOUNTX - 1)
            {
                ClickButton(MineArry[p.cx + 1, p.cy - 1]);
            }
            if (p.cx < MINEBUTTONSCOUNTX - 1)
            {
                ClickButton(MineArry[p.cx + 1, p.cy]);
            }
            if (p.cx > 0 && p.cy < MINEBUTTONSCOUNTY - 1)
            {
                ClickButton(MineArry[p.cx - 1, p.cy + 1]);
            }
            if (p.cy < MINEBUTTONSCOUNTY - 1)
            {
                ClickButton(MineArry[p.cx, p.cy + 1]);
            }
            if (p.cx < MINEBUTTONSCOUNTX - 1 && p.cy < MINEBUTTONSCOUNTY - 1)
            {
                ClickButton(MineArry[p.cx + 1, p.cy + 1]);
            }
        }

        private int CountBoom(MineButton p)
        {
            int i = 0;
            if (p.cx > 0)
                if (MineArry[p.cx - 1, p.cy].isboom)
                    i++;
            if (p.cx > 0 && p.cy > 0)
                if (MineArry[p.cx - 1, p.cy - 1].isboom)
                    i++;
            if (p.cy > 0)
                if (MineArry[p.cx, p.cy - 1].isboom)
                    i++;
            if (p.cy > 0 && p.cx < MINEBUTTONSCOUNTX - 1)
                if (MineArry[p.cx + 1, p.cy - 1].isboom)
                    i++;
            if (p.cx < MINEBUTTONSCOUNTX - 1)
                if (MineArry[p.cx + 1, p.cy].isboom)
                    i++;
            if (p.cx > 0 && p.cy < MINEBUTTONSCOUNTY - 1)
                if (MineArry[p.cx - 1, p.cy + 1].isboom)
                    i++;
            if (p.cy < MINEBUTTONSCOUNTY - 1)
                if (MineArry[p.cx, p.cy + 1].isboom)
                    i++;
            if (p.cx < MINEBUTTONSCOUNTX - 1 && p.cy < MINEBUTTONSCOUNTY - 1)
                if (MineArry[p.cx + 1, p.cy + 1].isboom)
                    i++;
            return i;
        }

        private int CountFlag(MineButton p)
        {
            int i = 0;
            if (p.cx > 0)
                if (MineArry[p.cx - 1, p.cy].bflag || (MineArry[p.cx - 1, p.cy].bclick && MineArry[p.cx - 1, p.cy].isboom))
                    i++;
            if (p.cx > 0 && p.cy > 0)
                if (MineArry[p.cx - 1, p.cy - 1].bflag || (MineArry[p.cx - 1, p.cy - 1].bclick && MineArry[p.cx - 1, p.cy - 1].isboom))
                    i++;
            if (p.cy > 0)
                if (MineArry[p.cx, p.cy - 1].bflag || (MineArry[p.cx, p.cy - 1].bclick && MineArry[p.cx, p.cy - 1].isboom))
                    i++;
            if (p.cy > 0 && p.cx < MINEBUTTONSCOUNTX - 1)
                if (MineArry[p.cx + 1, p.cy - 1].bflag || (MineArry[p.cx + 1, p.cy - 1].bclick && MineArry[p.cx + 1, p.cy - 1].isboom))
                    i++;
            if (p.cx < MINEBUTTONSCOUNTX - 1)
                if (MineArry[p.cx + 1, p.cy].bflag || (MineArry[p.cx + 1, p.cy].bclick && MineArry[p.cx + 1, p.cy].isboom))
                    i++;
            if (p.cx > 0 && p.cy < MINEBUTTONSCOUNTY - 1)
                if (MineArry[p.cx - 1, p.cy + 1].bflag || (MineArry[p.cx - 1, p.cy + 1].bclick && MineArry[p.cx - 1, p.cy + 1].isboom))
                    i++;
            if (p.cy < MINEBUTTONSCOUNTY - 1)
                if (MineArry[p.cx, p.cy + 1].bflag || (MineArry[p.cx, p.cy + 1].bclick && MineArry[p.cx, p.cy + 1].isboom))
                    i++;
            if (p.cx < MINEBUTTONSCOUNTX - 1 && p.cy < MINEBUTTONSCOUNTY - 1)
                if (MineArry[p.cx + 1, p.cy + 1].bflag || (MineArry[p.cx + 1, p.cy + 1].bclick && MineArry[p.cx + 1, p.cy + 1].isboom))
                    i++;
            return i;
        }


        private void SetLevelImg()
        {
            SetNum(btnlevel_ten, CurrentLevel / 10);
            SetNum(btnlevel_one, CurrentLevel % 10);
        }

        private void SetLifeImg()
        {
            SetNum(btnlife_ten, LifeNum / 10);
            SetNum(btnlife_one, LifeNum % 10);
        }

        private void SetBoomImg()
        {
            SetNum(btnboom_hus, BoomNum / 100);
            SetNum(btnboom_ten, (BoomNum % 100) / 10);
            SetNum(btnboom_one, (BoomNum % 100) % 10);
        }

        private void SetScoreImg()
        {
            SetNum(btnsrc_ths, Score / 1000);
            SetNum(btnsrc_hus, Score % 1000 / 100);
            SetNum(btnsrc_ten, (Score % 1000 % 100) / 10);
            SetNum(btnsrc_one, (Score % 1000 % 100) % 10);
        }

        private void SetAround(MineButton p, int n)
        {
            if (p.cx > 0)
                if (!MineArry[p.cx - 1, p.cy].bclick && !MineArry[p.cx - 1, p.cy].bflag)
                    SetButtonImage(MineArry[p.cx - 1, p.cy], n);
            if (p.cx > 0 && p.cy > 0)
                if (!MineArry[p.cx - 1, p.cy - 1].bclick && !MineArry[p.cx - 1, p.cy - 1].bflag)
                    SetButtonImage(MineArry[p.cx - 1, p.cy - 1], n);
            if (p.cy > 0)
                if (!MineArry[p.cx, p.cy - 1].bclick && !MineArry[p.cx, p.cy - 1].bflag)
                    SetButtonImage(MineArry[p.cx, p.cy - 1], n);
            if (p.cy > 0 && p.cx < MINEBUTTONSCOUNTX - 1)
                if (!MineArry[p.cx + 1, p.cy - 1].bclick && !MineArry[p.cx + 1, p.cy - 1].bflag)
                    SetButtonImage(MineArry[p.cx + 1, p.cy - 1], n);
            if (p.cx < MINEBUTTONSCOUNTX - 1)
                if (!MineArry[p.cx + 1, p.cy].bclick && !MineArry[p.cx + 1, p.cy].bflag)
                    SetButtonImage(MineArry[p.cx + 1, p.cy], n);
            if (p.cx > 0 && p.cy < MINEBUTTONSCOUNTY - 1)
                if (!MineArry[p.cx - 1, p.cy + 1].bclick && !MineArry[p.cx - 1, p.cy + 1].bflag)
                    SetButtonImage(MineArry[p.cx - 1, p.cy + 1], n);
            if (p.cy < MINEBUTTONSCOUNTY - 1)
                if (!MineArry[p.cx, p.cy + 1].bclick && !MineArry[p.cx, p.cy + 1].bflag)
                    SetButtonImage(MineArry[p.cx, p.cy + 1], n);
            if (p.cx < MINEBUTTONSCOUNTX - 1 && p.cy < MINEBUTTONSCOUNTY - 1)
                if (!MineArry[p.cx + 1, p.cy + 1].bclick && !MineArry[p.cx + 1, p.cy + 1].bflag)
                    SetButtonImage(MineArry[p.cx + 1, p.cy + 1], n);
        }

        private void SetButtonImage(MineButton p, int n)
        {
            string strurl = "";
            if (Theme == 0)
            {
                if (n == -3)
                    strurl = "pack://application:,,,/img/方块.png";
                if (n == -2)
                    strurl = "pack://application:,,,/img/按下.png";
                if (n == -1)
                    strurl = "pack://application:,,,/img/炸弹.png";
                if (n == 10)
                    strurl = "pack://application:,,,/img/空白.png";
                if (n >= 1 && n <= 8)
                    strurl = "pack://application:,,,/img/数字" + n.ToString() + ".png";
                if (n == 11)
                    strurl = "pack://application:,,,/img/旗子.png";
            }
            else
            {
                if (n == -3)
                    strurl = "pack://application:,,,/img/方块.bmp";
                if (n == -2)
                    strurl = "pack://application:,,,/img/按下.bmp";
                if (n == -1)
                    strurl = "pack://application:,,,/img/炸弹.bmp";
                if (n == 10)
                    strurl = "pack://application:,,,/img/空白.bmp";
                if (n >= 1 && n <= 8)
                    strurl = "pack://application:,,,/img/数字" + n.ToString() + ".bmp";
                if (n == 11)
                    strurl = "pack://application:,,,/img/旗子.bmp";
            }
            p.ButtonImage.Source = new BitmapImage(new Uri(strurl, UriKind.RelativeOrAbsolute));
        }

        private void SetNum(Image p, int n)
        {
            try
            {
                p.Source = new BitmapImage(new Uri("pack://application:,,,/img/num" + n.ToString() + ".png", UriKind.RelativeOrAbsolute));
            }
            catch
            {
                MessageBox.Show("分数爆表了！！！");
                Score = Score - 10000;
            }
        }


        public class MineButton : Button
        {
            public MineButton()
            {
                Width = 32;
                Height = 32;
                BorderThickness = new Thickness(0);
                Background = Brushes.Transparent;
                StackPanel ButtonPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new System.Windows.Thickness(0)
                };

                ButtonImage = new Image
                {
                    Margin = new System.Windows.Thickness(0, 0, 0, 0)
                };
                ButtonPanel.Children.Add(ButtonImage);
                Content = ButtonPanel;

                isboom = false;
            }
            public Image ButtonImage = null;
            public int cx;
            public int cy;
            public bool isboom;
            public string DiamondName;
            public bool bflag = false;
            public bool bclick = false;
            internal bool isRightDown;
            internal bool isLeftDown;
        }


        private void ClickOnTimedEvent(object sender, ElapsedEventArgs e)
        {
            isAroundSetable = false;
            ClickTimer.Stop();
        }

        private void InitOnTimedEvent(object sender, ElapsedEventArgs e)
        {
            isLevelOver = false;
            InitTimer.Stop();
        }


        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MnuAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("MineSweeper \n version 1.3 \n By Scort");
        }

        private void MenuRestart_Click(object sender, RoutedEventArgs e)
        {
            Restart();
        }

        private void EventHandler(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 25; i++)
                for (int j = 0; j < 10; j++)
                    if (MineArry[i, j].isboom == false)
                        ClickButton(MineArry[i, j]);
        }
    }
}