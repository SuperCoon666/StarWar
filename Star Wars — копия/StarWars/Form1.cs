using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MatildaWinLib;


namespace StarWars
{
    public partial class Form1 : Form
    {
        List<SuperImage> mmm = new List<SuperImage>(); //массив метеоритов
        List<SuperImage> Fireball = new List<SuperImage>(); //массив пуль
        SuperImage ship;
        SuperImage Earth;
        bool arcada = false;// режим аркады
        byte min;// минуты
        byte sec;// секунды
        byte point;// очки игрока 
        short fireballCount; // количество пуль
        int x_S; // left ракеты
        int y_S; // top ракеты
        const int h_S=135;// стандартный размер ракеты
        const int w_S=135;//стандартный размер ракеты
        int x_E; // всё для земли
        int y_E; // всё для земли
        int w_E; // всё для земли
        int h_E; // всё для земли
        bool rocket; // для отрисовки ракеты
        byte triple; // отвечает за работу таймера для стрельбы тремя пулями
        byte protect; //  отвечает за работу таймера для защиты
        sbyte big =1;// если равно 1, то ракета увеличивается. если -1, то уменьшается
        //int size=0; //коэфицент для нескольких увеличений
        byte bonus_F =1; //коэфицент для увеличения пуль при увеличении ракеты
        byte bonTime; // время показывания пополнения
        public Form1()
        {
            InitializeComponent();
        }

        void CreateMet(int x, int y, int r)
        {
            SuperImage m = matilda1.CreateSuperImage("3_метеорит2.png", x, y, 25 * r, 25 * r);
            m.Offset = matilda1.RandomInt(-3, 3);
            m.Name = "M" + r.ToString();
            mmm.Add(m);
        }

        public void Form1_Load(object sender, EventArgs e)
        {
             x_E = 0;
             y_E = ClientRectangle.Height - 50;
             w_E = ClientRectangle.Width;
             h_E = 50;
            Earth = matilda1.CreateSuperImage("3_Земля.jpg", x_E,  y_E,  w_E,  h_E);
            Earth.Name = "Земля";
            

             x_S = ClientRectangle.Width / 2 - 35;
             y_S = ClientRectangle.Height - 40 - 70;
            ship = matilda1.CreateSuperImage("3_ракета3.png", x_S, y_S, w_S, h_S, "Ракета");
            rocket = true;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if(ship != null)
            {
                ship.Left = ClientRectangle.Width / 2 - 35;
                ship.Top = ClientRectangle.Height - 40 - 70;
            }
            
            if(Earth != null)
            {
                Earth.Top = ClientRectangle.Height - 50;
                Earth.Width = ClientRectangle.Width;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            ship.Left = e.X;
            ship.Top = e.Y;

        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            Cursor.Hide();
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            Cursor.Show();
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ship.LoadImage("3_ракета.png");
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ship.LoadImage("3_ракета2.png");
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            ship.LoadImage("3_ракета3.png");
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            ship.LoadImage("3_ракета4.png");
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button.ToString() == "Left")
            {
                if (ship.IsDisposed == false && fireballCount != 0)
                {
                    fireballCount--;
                    lFBCount.Text = fireballCount.ToString();
                    int x = (ship.Left + ship.Width / 2 - 10);
                    int y = (ship.Top - 25) ;
                    SuperImage d = matilda1.CreateSuperImage("3_fireball.png", x, y, 20 * bonus_F, 20 * bonus_F);
                    d.Name = "fire";
                    Fireball.Add(d);
                }
            }


            if (e.Button.ToString() == "Right" && triple>0)
            {
                    for (int i = -2; i <= 2; i += 2)
                    {
                    int x = (ship.Left + ship.Width / 2 - 10) ;
                    int y = (ship.Top - 25);
                    SuperImage d = matilda1.CreateSuperImage("3_fireball.png", x, y, 20 * bonus_F, 20 * bonus_F);
                        d.Name = "fire";
                        d.Offset = i;
                        Fireball.Add(d);
                    }
                
            }

            if (fireballCount == 0)
            {
                lFB.ForeColor = Color.Red;
                lFBCount.ForeColor = Color.Red;
            }
        }


        private void tStrike_Tick(object sender, EventArgs e)// стрельба
        {
            Text = Fireball.Count.ToString();
            for (int i = 0; i < Fireball.Count; i++)
            {
                Fireball[i].Top -= 15;
                Fireball[i].Left += Fireball[i].Offset;

                if (Fireball[i].Top <= menuStrip1.Height)
                {
                    Fireball[i].Dispose();
                    Fireball.RemoveAt(i);

                }
            }
        }


        private void tMetior_Tick(object sender, EventArgs e)// метеориты
        {
            int r = matilda1.RandomInt(1, 3);
            int x = matilda1.RandomInt(0, ClientRectangle.Width - 25 * r);
            int y = menuStrip1.Height;
            CreateMet(x, y, r);
        }

        private void tMetiorMove_Tick(object sender, EventArgs e)// движение метеоритов
        {
            for (int i = 0; i < mmm.Count; i++)
            {
                mmm[i].Top += 5 + (1 + (min * 2));//второй вариант увеличания скорости:5 * (1 + min); градация:(5 10 15... 6 10 12 14 16 18...)
                mmm[i].Left += mmm[i].Offset;
                if (mmm[i].Left < 0 || mmm[i].Left > ClientRectangle.Width)
                {
                    mmm[i].Dispose();
                    mmm.RemoveAt(i);
                }
            }
        }

        private void matilda1_OnConnection(SuperImage a, SuperImage b) //столкновение картинок
        {
            if (a.Name == "M1" && b.Name == "fire")
            {
                point++;
                label9.Text = point.ToString();
                mmm.Remove(a);
                a.Dispose();
                Fireball.Remove(b);
                b.Dispose();
            }
            if (a.Name == "fire" && b.Name == "M1")
            {
                point++;
                label9.Text = point.ToString();
                mmm.Remove(b);
                b.Dispose();
                Fireball.Remove(a);
                a.Dispose();
            }



            if (a.Name == "M2" && b.Name == "fire")
            {
                point++;
                label9.Text = point.ToString();
                int x = a.Left;
                int y = a.Top;

                mmm.Remove(a);
                a.Dispose();
                Fireball.Remove(b);
                b.Dispose();

                CreateMet(x, y, 1);
                CreateMet(x, y, 1);
            }
            if (a.Name == "fire" && b.Name == "M2")
            {
                point++;
                label9.Text = point.ToString();
                int x = b.Left;
                int y = b.Top;

                mmm.Remove(b);
                b.Dispose();
                Fireball.Remove(a);
                a.Dispose();

                CreateMet(x, y, 1);
                CreateMet(x, y, 1);
            }
            
       


            if (a.Name == "M3" && b.Name == "fire")
            {
                point++;
                label9.Text = point.ToString();
                int x = a.Left;
                int y = a.Top;

                mmm.Remove(a);
                a.Dispose();
                Fireball.Remove(b);
                b.Dispose();

                CreateMet(x, y, 2);
                CreateMet(x, y, 2);
            }
            if (a.Name == "fire" && b.Name == "M3")
            {
                point++;
                label9.Text = point.ToString();
                int x = b.Left;
                int y = b.Top;

                mmm.Remove(b);
                b.Dispose();
                Fireball.Remove(a);
                a.Dispose();

                CreateMet(x, y, 2);
                CreateMet(x, y, 2);
            }


            if (a.Name == "Ракета" && b.Name.Contains("M"))
            {
                if (protect > 0)
                {
                    point+=3;
                    label9.Text = point.ToString();
                }
                else
                {
                    a.Dispose();
                    ship.Dispose();
                    tStrike.Stop();
                    tMetior.Stop();
                    tMetiorMove.Stop();
                    tHelp.Stop();
                    tAllTime.Stop();
                    label1.Visible = true;
                    rocket = false;
                }
                mmm.Remove(b);
                b.Dispose();


            }
            if (a.Name.Contains("M") && b.Name == "Ракета")
            {
                if (protect > 0)
                {
                    point+=3;
                    label9.Text = point.ToString();
                }
                else
                {
                    b.Dispose();
                    ship.Dispose();
                    tStrike.Stop();
                    tMetior.Stop();
                    tMetiorMove.Stop();
                    tHelp.Stop();
                    tAllTime.Stop();
                    label1.Visible = true;
                    rocket = false;
                }

                mmm.Remove(a);
                a.Dispose();
            }


            if (a.Name == "Ракета" && b.Name == "Перезарядка")
            {
                mmm.Remove(b);
                b.Dispose();
                if (arcada == true)
                {
                    point += 100;
                    fireballCount += 20;
                    label4.Visible = true;
                    label7.Visible = true;
                }
                else
                {
                    fireballCount += 50;
                    label7.Visible = true;
                }
                lFB.ForeColor = Color.Cyan;
                lFBCount.ForeColor = Color.Cyan;
                bonTime = 2;
                tBonTime.Start();
                lFBCount.Text = fireballCount.ToString();

            }
            if (a.Name == "Перезарядка" && b.Name == "Ракета")
            {

                mmm.Remove(a);
                a.Dispose();
                if (arcada==true)
                {
                    point += 100;
                    fireballCount += 20;
                    label4.Visible = true;
                    label7.Visible = true;
                }
                else
                {
                    fireballCount += 50;
                    label7.Visible = true;
                }
                lFB.ForeColor = Color.Cyan;
                lFBCount.ForeColor = Color.Cyan;
                bonTime = 2;
                tBonTime.Start();
                lFBCount.Text = fireballCount.ToString();


            }
            if (a.Name == "Тройной" && b.Name == "Ракета")
            {
                a.Dispose();
                triple = 10;
                tTriple.Start();
                label5.Visible = true;
                label6.Visible = true;
            }
            if (b.Name == "Тройной" && a.Name == "Ракета")
            {
                b.Dispose();
                triple = 10;
                tTriple.Start();
                label5.Visible = true;
                label6.Visible = true;
            }
            if (a.Name == "Супер" && b.Name == "Ракета")
            {
                if (tProtect.Enabled== false)// чтоб не складывать несколько
                {
                    
                    protect = 15; // увеличил до 15 сек, так как за 10 ракета толком вырасти не успевает
                    tProtect.Start();
                    bonus_F = 2;
                    tBig.Start();
                    label3.Text = "Protect ON:";
                    label2.Visible = true;
                }
                a.Dispose();
            }

            if (b.Name == "Супер" && a.Name == "Ракета")
            {
                if (tProtect.Enabled == false) // чтоб не складывать несколько
                {

                    protect = 15; // увеличил до 15 сек, так как за 10 ракета толком вырасти не успевает
                    tProtect.Start();
                    bonus_F = 2;
                    tBig.Start();
                    label3.Text = "Protect ON:";
                    label2.Visible = true;
                }
                b.Dispose();
            }

        }

        private void tHelp_Tick(object sender, EventArgs e)//выбор бонус
        {
            int r = matilda1.RandomInt(1, 15);
            int x = matilda1.RandomInt(0, ClientRectangle.Width - 25);
            int y = menuStrip1.Height;
            if(r==1 || r<=7 )
            {
                SuperImage h = matilda1.CreateSuperImage("3_fireballs_pack.png", x, y, 25, 25);
                h.Name = "Перезарядка";
                mmm.Add(h);
            }
            if (r>=8 && r<=12)
            {
                SuperImage h = matilda1.CreateSuperImage("3_fireballs_pack3.png", x, y, 25, 25);
                h.Name = "Тройной";
                mmm.Add(h);
            }
            if (r>12)
            {
                SuperImage h = matilda1.CreateSuperImage("3_fireballs_pack2.png", x, y, 25, 25);
                h.Name = "Супер";
                mmm.Add(h);
            }
            tHelp.Interval = matilda1.RandomInt(5000, 15000);
        }

        private void аркадаToolStripMenuItem_Click(object sender, EventArgs e)// режим аркады
        {
            arcada = true;
            lFB.ForeColor = Color.Cyan;
            lFBCount.ForeColor = Color.Cyan;
            fireballCount = 10000;
            lFBCount.Text = 10000.ToString();

        }

        private void выживаниеToolStripMenuItem_Click(object sender, EventArgs e)// режим выживания
        {
            arcada = false;
            lFB.ForeColor = Color.Cyan;
            lFBCount.ForeColor = Color.Cyan;
            fireballCount = 50;
            lFBCount.Text = 50.ToString();
            label7.Text = "+50";
        }

        public void sTARTToolStripMenuItem_Click(object sender, EventArgs e)// начало игры
        {
            tStrike.Enabled = true;
            tMetior.Enabled = true;
            tMetiorMove.Enabled = true;
            tHelp.Enabled = true;
            tAllTime.Start();
            if (rocket == false) {
                mmm.Clear();
                Fireball.Clear();
                matilda1.ClearAllImages();
                sec = 00;
                min = 0;

                ship = matilda1.CreateSuperImage("3_ракета3.png", x_S, y_S, w_S, h_S, "Ракета");
                Earth = matilda1.CreateSuperImage("3_Земля.jpg", x_E, y_E, w_E, h_E);
                Earth.Name = "Земля";
                rocket = true;
                if (arcada == true)
                {
                    fireballCount = 50;
                    lFBCount.Text = fireballCount.ToString();
                }
                else
                {
                    fireballCount = 10000;
                    lFBCount.Text = fireballCount.ToString();
                }
            }
            label1.Visible = false;
            point = 0;
        }

        private void Triple_Tick(object sender, EventArgs e)// время тройного выстрела
        {
            if (triple>0) {
                triple -= 1;
                label6.Text = triple.ToString();
            }
            else
            {
                tTriple.Stop();
                label5.Visible = false;
                label6.Visible = false;
            }
        }

        // Отсчёт времени, в течение которого будет действовать "синий" бонус
        private void tProtect_Tick(object sender, EventArgs e)// движение защиты 
        {

            protect--;
            label2.Text = protect.ToString();
            Text = protect.ToString();

            //tProtect и tBig запускаются вместе. Через 3 секунды выключаем tBig
            if (protect == 12)
            {
                tBig.Stop();
                big = -1; //выставляем индикатор в режим "уменьшение"
            }

            if(protect == 3)
            {
                tBig.Start();
            }

            // Через 10 сек выключаем таймер действия защиты
            if (protect == 0)
            {
                tBig.Stop();
                label2.Text = protect.ToString();
                Text = protect.ToString();
                bonus_F = 1;
                big = 1; //выставляем индикатор в режим "увеличение" (для следующего запуска)
                tProtect.Stop();
                label3.Text = "Protect OFF";
                label2.Visible = false;
            }
        }

        private void tBig_Tick(object sender, EventArgs e) // изменение размера 
        {
            ship.Height += 1 * big; 
            ship.Width += 1 * big; 
        }

        private void tBonTime_Tick(object sender, EventArgs e) // показать пополнение
        {
            if (bonTime>0) 
            {
                bonTime--;
            }
            else
            {
                tBonTime.Stop();
                label7.Visible = false;
                label4.Visible = false;
            }
        }

        private void tAllTime_Tick(object sender, EventArgs e) // время игры
        {
            sec++;
            if (sec == 60)
            {
                min++;
                sec = 0;
            }
            if (sec<10)
            {
                label10.Text = min.ToString() + ":" +"0"+ sec.ToString();
            }
            else 
            {
                label10.Text = min.ToString() + ":" + sec.ToString();
            }


        }


    }
}

                