using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chests
{
    public partial class Form1 : Form
    {
        private int[,] map = new int[8, 8];
        int start_x = 75;
        int finish_y = 475;
        int kx = 0;
        int ky = 0;
        int qx = 0;
        int qy = 0;
        int last_Quen_Cell = -1;
        int order = 1;
        int mm = 20;
        int cycles = 1;
        int Ks = 0;
        int Qs = 0;
        int Kw = 0;
        int Qw = 0;
        int flag = 0;
        int viborFiguri = 0;
        int progres = 0;
        int napravleniye = 0;
        float seconds = 1;
        int seconds2 = 1;
        int Strategiya_konya = -1;
        int Strategiya_ferzya = -1;
        Random rnd = new Random();
        bool[,] Allowed_cells = new bool[8, 8] {
            {true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true },
        };
        int[,] Knight_Moves = new int[8, 2]
        {
            { 1, 2 },
            { -1, 2 },
            { 1, -2 },
            { -1, -2 },
            { 2, 1 },
            { 2, -1 },
            { -2, 1 },
            { -2, -1 },
        };
        int[,] Queen_Moves = new int[8, 2]
        {
            { 1, 1 },
            { -1, 1 },
            { 1, -1 },
            { -1, -1 },
            { 0, 1 },
            { 0, -1 },
            { 1, 0 },
            { -1, 0 },
        };



        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Strategiya_konya = comboBox1.SelectedIndex;
            Strategiya_ferzya = comboBox2.SelectedIndex;
            try
            {
                cycles = int.Parse(textBox2.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Format Exception");
                cycles = 0;
            }
            try
            {
                mm = int.Parse(textBox1.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Format Exception");
                cycles = 0;
            }
            try
            {
                seconds = float.Parse(textBox3.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Format Exception");
                cycles = 0;
            }
            if (seconds < 0)
            {

                MessageBox.Show("Значение секунд меньше нуля");
                cycles = 0;
            }
            try
            {
                //seconds = seconds * 1000;
                seconds2 = Convert.ToInt32(seconds * 1000);
            }
            catch (Exception)
            {
                MessageBox.Show("Значение секунд не корректно");
                cycles = 0;
            }
            
            kx = Knight.Location.X;
            ky = Knight.Location.Y;
            qx = Queen.Location.X;
            qy = Queen.Location.Y;
            progressBar1.Value = 0;

            for (int i = 0; i < cycles; i++) // цикл по партиям
            {
                if (checkBox1.Checked == true)
                {
                    random_sets();
                }
                if(checkBox2.Checked) {Update(); }
                progressBar1.Value = 0;
                flag = 0;
                Thread.Sleep(seconds2);
                for (int j = 0; j < mm; j++) // цикл по колличеству ходов
                {
                    switch (Strategiya_konya)
                    {
                        case 0:
                            if (First_Knight_Strategy() == false)
                            {
                                info.Text += "конь обуздан" + Environment.NewLine;
                                Qw++; // 1
                                flag = 1;
                            }
                            break;
                        case 1:
                            if (perimeter_Knight() == false)
                            {
                                info.Text += "конь обуздан" + Environment.NewLine;
                                Qw++; // 1
                                flag = 1;
                            }
                            break;
                        case 2:
                            if (center_Knight() == false)
                            {
                                info.Text += "конь обуздан" + Environment.NewLine;
                                Qw++; // 1
                                flag = 1;
                            }
                            break;
                        case 3:
                            if (New_Knight_Strategy() == false)
                            {
                                info.Text += "конь обуздан" + Environment.NewLine;
                                Qw++; // 1
                                flag = 1;

                            }
                            break;
                    }
                    if (checkBox2.Checked) { Update(); }
                    if (Knight.Location.X == Queen.Location.X && Knight.Location.Y == Queen.Location.Y) { Kw++; break; }
                    Thread.Sleep(seconds2);
                    if (flag == 1) { break; }
                    switch (Strategiya_ferzya)
                    {
                        case 0:
                            if (First_Queen_Strategy() == false)
                            {
                                info.Text += "ферзь запутался" + Environment.NewLine;
                                Kw++;
                                flag = 1;
                            }
                            break;
                        case 1:
                            if (Second_Queen_Strategy() == false)
                            {
                                info.Text += "ферзь запутался" + Environment.NewLine;
                                Kw++;
                                flag = 1;
                            }
                            break;
                    }
                    if (checkBox2.Checked) { Update(); }
                    Thread.Sleep(seconds2);
                    if (flag == 1) { break; }
                    if (j == mm - 1)
                    {
                        if (New_Knight_Strategy() == true) {  Kw++; break; } //3     info.Text += "ферзь запутался" + Environment.NewLine;
                        if (First_Knight_Strategy() == false) { Qw++; break; } //4
                    }
                }   
                Scores.Text = "конь победил: " + Kw.ToString() + Environment.NewLine + "ферзь победил: " + Qw.ToString();
                progres = ((i + 1) * 100 / cycles);
                progressBar1.Value = progres;
                progressBar1.Update();
                Rebute();
            }
        }
        private bool is_not_dark(int x, int y) //функция проверки значения клетки массива с выколотыми клетками
        { //передавать координаты как есть
            bool allowed = false;
            x = (x - 75) / 50;
            y = (y - 75) / 50;
            if (Allowed_cells[y, x] == true)
            {
                allowed = true;
            }
            return allowed;
        }
        private bool is_allowed_move(int figure, int x, int y) //функция проверки доступности хода для любой фигуры(первый параметр) на х и у (второй и третий параметры)
        {
            bool allowed = false;
            if (figure == 1)
            {
                if (Knight.Location.X + x <= 425 && Knight.Location.X + x >= 75)
                {
                    if (Knight.Location.Y + y <= 425 && Knight.Location.Y + y >= 75)
                    {
                        if (is_not_dark(Knight.Location.X + x, Knight.Location.Y + y))
                        {
                            if (((Knight.Location.Y + y) != Queen.Location.Y) || ((Knight.Location.X + x) != Queen.Location.X))
                            {
                                allowed = true;
                                int xx = Knight.Location.X + x - Queen.Location.X;
                                int yy = Knight.Location.Y + y - Queen.Location.Y;
                                if (xx == 0 || yy == 0 || xx == yy || xx == -yy)
                                {
                                    if (Queen_Dostigima(Knight.Location.X + x, Knight.Location.Y + y))
                                    {
                                        allowed = false;
                                    }
                                }
                            }
                            //else { info.Text += "это позиция ферзя! "; }
                        }
                        //else { info.Text += "запрещенная клетка "; }
                    }
                    //else { info.Text += "выход за границы по Y "; }
                }
                //else { info.Text += "выход за границы по Х "; }
            }
            if (figure == 2)
            {
                if ((Queen.Location.X + x) <= 425 && (Queen.Location.X + x) >= 75)
                {
                    if ((Queen.Location.Y + y) <= 425 && (Queen.Location.Y + y) >= 75)
                    {
                        if (is_not_dark(Queen.Location.X + x, Queen.Location.Y + y))
                        {
                            if (Queen.Location.Y + y != Knight.Location.Y || Queen.Location.X + x != Knight.Location.X)
                            {
                                if (Queen_Dostigima(Queen.Location.X + x, Queen.Location.Y + y))
                                {
                                    allowed = true;
                                }
                            }
                            //else { info.Text += "это позиция коня! "; }
                        }
                        //else { info.Text += "запрещенная клетка "; }
                    }
                    //else { info.Text += "выход за границы по Y "; }
                }
                //else { info.Text += "выход за границы по Х "; }

            }
            return allowed;
        }
        private bool Queen_Dostigima(int x, int y) //функция проверки для ферзя, нет ли на пути выколотых клеток
        { // x и y проверяемой точки
            bool allowed = true;
            int shag_x = (x - Queen.Location.X) / 50;
            int shag_y = (y - Queen.Location.Y) / 50;
            int shag = Math.Max(Math.Abs(shag_x), Math.Abs(shag_y));
            int new_shag_x = 1;
            int new_shag_y = 1;
            if (shag_x == 0)
            {
                new_shag_x = 0;
            }
            else if (shag_x < 1)
            {
                new_shag_x = -1;
            }
            if (shag_y == 0)
            {
                new_shag_y = 0;
            }
            else if (shag_y < 1)
            {
                new_shag_y = -1;
            }

            for (int i = 1; i < shag + 1; i++)
            {
                if (is_not_dark(Queen.Location.X + new_shag_x * i * 50, Queen.Location.Y + new_shag_y * i * 50) == false)
                {
                    allowed = false;
                    break;
                }
                if (Queen.Location.X + new_shag_x * i * 50 == Knight.Location.X && Queen.Location.Y + new_shag_y * i * 50 == Knight.Location.Y)
                {
                    allowed = false;
                    break;
                }
            }
            return allowed;
        }
        private void virez(int x, int y) //функция "выкалывания" клеток и в массиве доступных клеток, и на поле
        {
            int new_x = (x - 75) / 50;
            int new_y = (y - 75) / 50;
            int number = new_y * 8 + new_x + 1;
            switch (number)
            {
                case 1:
                    pb1.BackColor = Color.Gray; break;
                case 2:
                    pb2.BackColor = Color.Gray; break;
                case 3:
                    pb3.BackColor = Color.Gray; break;
                case 4:
                    pb4.BackColor = Color.Gray; break;
                case 5:
                    pb5.BackColor = Color.Gray; break;
                case 6:
                    pb6.BackColor = Color.Gray; break;
                case 7:
                    pb7.BackColor = Color.Gray; break;
                case 8:
                    pb8.BackColor = Color.Gray; break;
                case 9:
                    pb9.BackColor = Color.Gray; break;
                case 10:
                    pb10.BackColor = Color.Gray; break;
                case 11:
                    pb11.BackColor = Color.Gray; break;
                case 12:
                    pb12.BackColor = Color.Gray; break;
                case 13:
                    pb13.BackColor = Color.Gray; break;
                case 14:
                    pb14.BackColor = Color.Gray; break;
                case 15:
                    pb15.BackColor = Color.Gray; break;
                case 16:
                    pb16.BackColor = Color.Gray; break;
                case 17:
                    pb17.BackColor = Color.Gray; break;
                case 18:
                    pb18.BackColor = Color.Gray; break;
                case 19:
                    pb19.BackColor = Color.Gray; break;
                case 20:
                    pb20.BackColor = Color.Gray; break;
                case 21:
                    pb21.BackColor = Color.Gray; break;
                case 22:
                    pb22.BackColor = Color.Gray; break;
                case 23:
                    pb23.BackColor = Color.Gray; break;
                case 24:
                    pb24.BackColor = Color.Gray; break;
                case 25:
                    pb25.BackColor = Color.Gray; break;
                case 26:
                    pb26.BackColor = Color.Gray; break;
                case 27:
                    pb27.BackColor = Color.Gray; break;
                case 28:
                    pb28.BackColor = Color.Gray; break;
                case 29:
                    pb29.BackColor = Color.Gray; break;
                case 30:
                    pb30.BackColor = Color.Gray; break;
                case 31:
                    pb31.BackColor = Color.Gray; break;
                case 32:
                    pb32.BackColor = Color.Gray; break;
                case 33:
                    pb33.BackColor = Color.Gray; break;
                case 34:
                    pb34.BackColor = Color.Gray; break;
                case 35:
                    pb35.BackColor = Color.Gray; break;
                case 36:
                    pb36.BackColor = Color.Gray; break;
                case 37:
                    pb37.BackColor = Color.Gray; break;
                case 38:
                    pb38.BackColor = Color.Gray; break;
                case 39:
                    pb39.BackColor = Color.Gray; break;
                case 40:
                    pb40.BackColor = Color.Gray; break;
                case 41:
                    pb41.BackColor = Color.Gray; break;
                case 42:
                    pb42.BackColor = Color.Gray; break;
                case 43:
                    pb43.BackColor = Color.Gray; break;
                case 44:
                    pb44.BackColor = Color.Gray; break;
                case 45:
                    pb45.BackColor = Color.Gray; break;
                case 46:
                    pb46.BackColor = Color.Gray; break;
                case 47:
                    pb47.BackColor = Color.Gray; break;
                case 48:
                    pb48.BackColor = Color.Gray; break;
                case 49:
                    pb49.BackColor = Color.Gray; break;
                case 50:
                    pb50.BackColor = Color.Gray; break;
                case 51:
                    pb51.BackColor = Color.Gray; break;
                case 52:
                    pb52.BackColor = Color.Gray; break;
                case 53:
                    pb53.BackColor = Color.Gray; break;
                case 54:
                    pb54.BackColor = Color.Gray; break;
                case 55:
                    pb55.BackColor = Color.Gray; break;
                case 56:
                    pb56.BackColor = Color.Gray; break;
                case 57:
                    pb57.BackColor = Color.Gray; break;
                case 58:
                    pb58.BackColor = Color.Gray; break;
                case 59:
                    pb59.BackColor = Color.Gray; break;
                case 60:
                    pb60.BackColor = Color.Gray; break;
                case 61:
                    pb61.BackColor = Color.Gray; break;
                case 62:
                    pb62.BackColor = Color.Gray; break;
                case 63:
                    pb63.BackColor = Color.Gray; break;
                case 64:
                    pb64.BackColor = Color.Gray; break;

            }
            Allowed_cells[new_y, new_x] = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ks = comboBox1.SelectedIndex;
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Qs = comboBox1.SelectedIndex;
        }

        private bool First_Knight_Strategy() //первая стратегия коня (случайные ходы)
        {
            bool shodil = false;
            int value = rnd.Next(0, 7);
            int value1 = value;
            for (; shodil != true;)
            {

                if (is_allowed_move(1, Knight_Moves[value, 0] * 50, Knight_Moves[value, 1] * 50))
                {
                    Knight.Location = new Point(Knight.Location.X + Knight_Moves[value, 0] * 50, Knight.Location.Y + Knight_Moves[value, 1] * 50);
                    if (Knight.BackColor == Color.White)
                    {
                        Knight.BackColor = Color.FromArgb(192, 255, 192);
                    }
                    else { Knight.BackColor = Color.White; }
                    shodil = true;
                }
                //if (shodil == true) { info.Text += "конь закончил, стратегия номер: " + value.ToString() + " успешна" + Environment.NewLine; break; }
                if (value < 7)
                {
                    //info.Text += "стратегия номер: " + value.ToString() + " отсеяна" + Environment.NewLine; ;
                    value++;
                }
                else if (value == 7)
                {
                    //info.Text += "стратегия номер: 7 отсеяна" + Environment.NewLine; ;
                    value = 0;
                }
                if (value == value1)
                {
                    //info.Text += "конь не может двигаться" + Environment.NewLine;
                    break;
                }
            }

            return shodil;
        }
        private bool First_Queen_Strategy() //первая стратегия ферзя (случайные ходы)
        {
            bool shodil = false;
            int x = Queen.Location.X;
            int y = Queen.Location.Y;
            int value = rnd.Next(0, 7);
            int value1 = value;
            for (; shodil != true;)
            {
                int dlina = rnd.Next(1, 7);
                for (int i = 0; i < 7; i++)
                {
                    if (is_allowed_move(2, Queen_Moves[value, 0] * dlina * 50, Queen_Moves[value, 1] * dlina * 50))
                    {
                        Queen.Location = new Point(Queen.Location.X + Queen_Moves[value, 0] * dlina * 50, Queen.Location.Y + Queen_Moves[value, 1] * dlina * 50);
                        shodil = true;
                        //if (shodil == true) { info.Text += "ферзь закончил, стратегия номер: " + value.ToString() + " на длину " + dlina.ToString() + " успешна" + Environment.NewLine; break; }
                    }
                    if (dlina < 7)
                    {
                        //info.Text += "стратегия номер: " + value.ToString() + " на длину " + dlina.ToString() + " отсеяна" + Environment.NewLine; ;
                        dlina++;
                    }
                    else if (dlina == 7)
                    {
                        //info.Text += "стратегия номер: 7 отсеяна" + Environment.NewLine; ;
                        dlina = 1;
                    }
                }

                if (value < 7)
                {
                    //info.Text += "стратегия номер: " + value.ToString() + " отсеяна" + Environment.NewLine; ;
                    value++;
                }
                else if (value == 7)
                {
                    //info.Text += "стратегия номер: 7 отсеяна" + Environment.NewLine; ;
                    value = 0;
                }
                if (value == value1)
                {
                    info.Text += "ферзь не может двигаться" + Environment.NewLine;
                    break;
                }
            }
            if (shodil == true)
            {
                virez(x, y);
            }
            return shodil;
        }

        private bool perimeter_Knight() //вторая стратегия коня (бег по периметру)
        {
            bool shodil = false;
            int stage = 0;
            if ((125 < Knight.Location.X && Knight.Location.X < 375) && (125 < Knight.Location.Y && Knight.Location.Y < 375))
            {
                for (int i = 0; i < 8; i++)
                {
                    if ((((Knight.Location.X + Knight_Moves[i, 0] * 50) < 175) || ((Knight.Location.X + Knight_Moves[i, 0] * 50) > 325)) && (((Knight.Location.Y + Knight_Moves[i, 1] * 50) < 175) || ((Knight.Location.Y + Knight_Moves[i, 1] * 50) > 325)))
                    {
                        if (is_allowed_move(1, Knight_Moves[i, 0] * 50, Knight_Moves[i, 1] * 50))
                        {
                            Knight.Location = new Point(Knight.Location.X + Knight_Moves[i, 0] * 50, Knight.Location.Y + Knight_Moves[i, 1] * 50);
                            if (Knight.BackColor == Color.White)
                            {
                                Knight.BackColor = Color.FromArgb(192, 255, 192);
                            }
                            else { Knight.BackColor = Color.White; }
                            stage = 1;
                            shodil = true;
                            //break;
                            return shodil;
                        }
                    }
                }
            }
            for (int k = 0; k < 2; k++)
            {
                if (napravleniye == 0)
                {
                    if (Knight.Location.X < 175 && Knight.Location.Y < 375 && Knight.Location.Y > 75) //синий кусок на схеме
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if (((Knight.Location.Y + Knight_Moves[i, 1] * 50) < Knight.Location.Y) && !((125 < (Knight.Location.X + Knight_Moves[i, 0] * 50) && (Knight.Location.X + Knight_Moves[i, 0] * 50) < 375) && (125 < (Knight.Location.Y + Knight_Moves[i, 1] * 50) && (Knight.Location.Y + Knight_Moves[i, 1] * 50) < 375)))
                            {
                                if (is_allowed_move(1, Knight_Moves[i, 0] * 50, Knight_Moves[i, 1] * 50))
                                {
                                    Knight.Location = new Point(Knight.Location.X + Knight_Moves[i, 0] * 50, Knight.Location.Y + Knight_Moves[i, 1] * 50);
                                    if (Knight.BackColor == Color.White)
                                    {
                                        Knight.BackColor = Color.FromArgb(192, 255, 192);
                                    }
                                    else { Knight.BackColor = Color.White; }
                                    stage = 1;
                                    shodil = true;
                                    //break;
                                    return shodil;
                                }
                            }
                        }
                    }
                    if (Knight.Location.Y < 125 && Knight.Location.X < 175) // слепая зона синего
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if ((Knight.Location.X + Knight_Moves[i, 0] * 50) > Knight.Location.X)
                            {
                                if (is_allowed_move(1, Knight_Moves[i, 0] * 50, Knight_Moves[i, 1] * 50))
                                {
                                    Knight.Location = new Point(Knight.Location.X + Knight_Moves[i, 0] * 50, Knight.Location.Y + Knight_Moves[i, 1] * 50);
                                    if (Knight.BackColor == Color.White)
                                    {
                                        Knight.BackColor = Color.FromArgb(192, 255, 192);
                                    }
                                    else { Knight.BackColor = Color.White; }
                                    stage = 1;
                                    shodil = true;
                                    //break;
                                    return shodil;
                                }
                            }
                        }
                    }

                    if (Knight.Location.Y > 325 && Knight.Location.X < 375 && Knight.Location.X > 75) // красный кусок на схеме
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if (((Knight.Location.X + Knight_Moves[i, 0] * 50) < Knight.Location.X) && !((125 < (Knight.Location.X + Knight_Moves[i, 0] * 50) && (Knight.Location.X + Knight_Moves[i, 0] * 50) < 375) && (125 < (Knight.Location.Y + Knight_Moves[i, 1] * 50) && (Knight.Location.Y + Knight_Moves[i, 1] * 50) < 375)))
                            {
                                if (is_allowed_move(1, Knight_Moves[i, 0] * 50, Knight_Moves[i, 1] * 50))
                                {
                                    Knight.Location = new Point(Knight.Location.X + Knight_Moves[i, 0] * 50, Knight.Location.Y + Knight_Moves[i, 1] * 50);
                                    if (Knight.BackColor == Color.White)
                                    {
                                        Knight.BackColor = Color.FromArgb(192, 255, 192);
                                    }
                                    else { Knight.BackColor = Color.White; }
                                    stage = 1;
                                    shodil = true;
                                    //break;
                                    return shodil;
                                }
                            }
                        }
                    }
                    if (Knight.Location.X < 125 && Knight.Location.Y > 325) // слепая зона красного
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if ((Knight.Location.Y + Knight_Moves[i, 1] * 50) < Knight.Location.Y)
                            {
                                if (is_allowed_move(1, Knight_Moves[i, 0] * 50, Knight_Moves[i, 1] * 50))
                                {
                                    Knight.Location = new Point(Knight.Location.X + Knight_Moves[i, 0] * 50, Knight.Location.Y + Knight_Moves[i, 1] * 50);
                                    if (Knight.BackColor == Color.White)
                                    {
                                        Knight.BackColor = Color.FromArgb(192, 255, 192);
                                    }
                                    else { Knight.BackColor = Color.White; }
                                    stage = 1;
                                    shodil = true;
                                    //break;
                                    return shodil;
                                }
                            }
                        }
                    }

                    if (Knight.Location.Y < 175 && Knight.Location.X < 425 && Knight.Location.X > 125) //зеленая зона
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            //if ((Knight.Location.X + Knight_Moves[i, 0] * 50) > Knight.Location.X) { info.Text += "новый Х больше старого" + Environment.NewLine;
                            //    if (125 < (Knight.Location.X + Knight_Moves[i, 0] * 50)) { info.Text += "новый Х больше 125" + Environment.NewLine; }
                            //    if ((Knight.Location.X + Knight_Moves[i, 0] * 50) < 375) { info.Text += "новый Х меньше 375" + Environment.NewLine; }
                            //    if (125 < (Knight.Location.Y + Knight_Moves[i, 1] * 50)) { info.Text += "новый Y больше 125" + Environment.NewLine; }
                            //    if ((Knight.Location.Y + Knight_Moves[i, 1] * 50) < 375) { info.Text += "новый Y меньше 375" + Environment.NewLine; }
                            //}
                            if (((Knight.Location.X + Knight_Moves[i, 0] * 50) > Knight.Location.X) && !(((125 < (Knight.Location.X + Knight_Moves[i, 0] * 50)) && ((Knight.Location.X + Knight_Moves[i, 0] * 50) < 375)) && ((125 < (Knight.Location.Y + Knight_Moves[i, 1] * 50)) && ((Knight.Location.Y + Knight_Moves[i, 1] * 50) < 375))))
                            {
                                if (is_allowed_move(1, Knight_Moves[i, 0] * 50, Knight_Moves[i, 1] * 50))
                                {
                                    Knight.Location = new Point(Knight.Location.X + Knight_Moves[i, 0] * 50, Knight.Location.Y + Knight_Moves[i, 1] * 50);
                                    if (Knight.BackColor == Color.White)
                                    {
                                        Knight.BackColor = Color.FromArgb(192, 255, 192);
                                    }
                                    else { Knight.BackColor = Color.White; }
                                    stage = 1;
                                    shodil = true;
                                    //break;
                                    return shodil;
                                }
                            }
                        }
                    }
                    if (Knight.Location.X > 375 && Knight.Location.Y < 175) // слепая зона зеленого
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if ((Knight.Location.Y + Knight_Moves[i, 1] * 50) > Knight.Location.Y)//здесь всё было плохо проверить другие места
                            {
                                if (is_allowed_move(1, Knight_Moves[i, 0] * 50, Knight_Moves[i, 1] * 50))
                                {
                                    Knight.Location = new Point(Knight.Location.X + Knight_Moves[i, 0] * 50, Knight.Location.Y + Knight_Moves[i, 1] * 50);
                                    if (Knight.BackColor == Color.White)
                                    {
                                        Knight.BackColor = Color.FromArgb(192, 255, 192);
                                    }
                                    else { Knight.BackColor = Color.White; }
                                    stage = 1;
                                    shodil = true;
                                    //break;
                                    return shodil;
                                }
                            }
                        }
                    }

                    if (Knight.Location.X > 325 && Knight.Location.Y < 425 && Knight.Location.Y > 125) // фиолетовая зона
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if (((Knight.Location.Y + Knight_Moves[i, 1] * 50) > Knight.Location.Y) && !((125 < (Knight.Location.X + Knight_Moves[i, 0] * 50) && (Knight.Location.X + Knight_Moves[i, 0] * 50) < 375) && (125 < (Knight.Location.Y + Knight_Moves[i, 1] * 50) && (Knight.Location.Y + Knight_Moves[i, 1] * 50) < 375)))
                            {
                                if (is_allowed_move(1, Knight_Moves[i, 0] * 50, Knight_Moves[i, 1] * 50))
                                {
                                    Knight.Location = new Point(Knight.Location.X + Knight_Moves[i, 0] * 50, Knight.Location.Y + Knight_Moves[i, 1] * 50);
                                    if (Knight.BackColor == Color.White)
                                    {
                                        Knight.BackColor = Color.FromArgb(192, 255, 192);
                                    }
                                    else { Knight.BackColor = Color.White; }
                                    stage = 1;
                                    shodil = true;
                                    //break;
                                    return shodil;
                                }
                            }
                        }
                    }
                    if (Knight.Location.Y > 375 && Knight.Location.X > 325) // слепая зона фиолетового
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if ((Knight.Location.X + Knight_Moves[i, 0] * 50) < Knight.Location.X)
                            {
                                if (is_allowed_move(1, Knight_Moves[i, 0] * 50, Knight_Moves[i, 1] * 50))
                                {
                                    Knight.Location = new Point(Knight.Location.X + Knight_Moves[i, 0] * 50, Knight.Location.Y + Knight_Moves[i, 1] * 50);
                                    if (Knight.BackColor == Color.White)
                                    {
                                        Knight.BackColor = Color.FromArgb(192, 255, 192);
                                    }
                                    else { Knight.BackColor = Color.White; }
                                    stage = 1;
                                    shodil = true;
                                    //break;
                                    return shodil;
                                }
                            }
                        }
                    }
                    napravleniye = 1;

                }
                if (napravleniye == 1)
                {
                    if (Knight.Location.X > 325 && Knight.Location.Y < 375 && Knight.Location.Y > 75) //синий кусок на схеме
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if (((Knight.Location.Y + Knight_Moves[i, 1] * 50) < Knight.Location.Y) && !((125 < (Knight.Location.X + Knight_Moves[i, 0] * 50) && (Knight.Location.X + Knight_Moves[i, 0] * 50) < 375) && (125 < (Knight.Location.Y + Knight_Moves[i, 1] * 50) && (Knight.Location.Y + Knight_Moves[i, 1] * 50) < 375)))
                            {
                                if (is_allowed_move(1, Knight_Moves[i, 0] * 50, Knight_Moves[i, 1] * 50))
                                {
                                    Knight.Location = new Point(Knight.Location.X + Knight_Moves[i, 0] * 50, Knight.Location.Y + Knight_Moves[i, 1] * 50);
                                    if (Knight.BackColor == Color.White)
                                    {
                                        Knight.BackColor = Color.FromArgb(192, 255, 192);
                                    }
                                    else { Knight.BackColor = Color.White; }
                                    stage = 1;
                                    shodil = true;
                                    //break;
                                    return shodil;
                                }
                            }
                        }
                    }
                    if (Knight.Location.Y < 125 && Knight.Location.X > 325) // слепая зона синего
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if ((Knight.Location.X + Knight_Moves[i, 0] * 50) < Knight.Location.X)
                            {
                                if (is_allowed_move(1, Knight_Moves[i, 0] * 50, Knight_Moves[i, 1] * 50))
                                {
                                    Knight.Location = new Point(Knight.Location.X + Knight_Moves[i, 0] * 50, Knight.Location.Y + Knight_Moves[i, 1] * 50);
                                    if (Knight.BackColor == Color.White)
                                    {
                                        Knight.BackColor = Color.FromArgb(192, 255, 192);
                                    }
                                    else { Knight.BackColor = Color.White; }
                                    stage = 1;
                                    shodil = true;
                                    //break;
                                    return shodil;
                                }
                            }
                        }
                    }

                    if (Knight.Location.Y > 325 && Knight.Location.X < 425 && Knight.Location.X > 125) // красный кусок на схеме
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if (((Knight.Location.X + Knight_Moves[i, 0] * 50) > Knight.Location.X) && !((125 < (Knight.Location.X + Knight_Moves[i, 0] * 50) && (Knight.Location.X + Knight_Moves[i, 0] * 50) < 375) && (125 < (Knight.Location.Y + Knight_Moves[i, 1] * 50) && (Knight.Location.Y + Knight_Moves[i, 1] * 50) < 375)))
                            {
                                if (is_allowed_move(1, Knight_Moves[i, 0] * 50, Knight_Moves[i, 1] * 50))
                                {
                                    Knight.Location = new Point(Knight.Location.X + Knight_Moves[i, 0] * 50, Knight.Location.Y + Knight_Moves[i, 1] * 50);
                                    if (Knight.BackColor == Color.White)
                                    {
                                        Knight.BackColor = Color.FromArgb(192, 255, 192);
                                    }
                                    else { Knight.BackColor = Color.White; }
                                    stage = 1;
                                    shodil = true;
                                    //break;
                                    return shodil;
                                }
                            }
                        }
                    }
                    if (Knight.Location.X > 375 && Knight.Location.Y > 325) // слепая зона красного
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if ((Knight.Location.Y + Knight_Moves[i, 1] * 50) < Knight.Location.Y)
                            {
                                if (is_allowed_move(1, Knight_Moves[i, 0] * 50, Knight_Moves[i, 1] * 50))
                                {
                                    Knight.Location = new Point(Knight.Location.X + Knight_Moves[i, 0] * 50, Knight.Location.Y + Knight_Moves[i, 1] * 50);
                                    if (Knight.BackColor == Color.White)
                                    {
                                        Knight.BackColor = Color.FromArgb(192, 255, 192);
                                    }
                                    else { Knight.BackColor = Color.White; }
                                    stage = 1;
                                    shodil = true;
                                    //break;
                                    return shodil;
                                }
                            }
                        }
                    }

                    if (Knight.Location.Y < 175 && Knight.Location.X < 375 && Knight.Location.X > 75) //зеленая зона
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            //if ((Knight.Location.X + Knight_Moves[i, 0] * 50) > Knight.Location.X) { info.Text += "новый Х больше старого" + Environment.NewLine;
                            //    if (125 < (Knight.Location.X + Knight_Moves[i, 0] * 50)) { info.Text += "новый Х больше 125" + Environment.NewLine; }
                            //    if ((Knight.Location.X + Knight_Moves[i, 0] * 50) < 375) { info.Text += "новый Х меньше 375" + Environment.NewLine; }
                            //    if (125 < (Knight.Location.Y + Knight_Moves[i, 1] * 50)) { info.Text += "новый Y больше 125" + Environment.NewLine; }
                            //    if ((Knight.Location.Y + Knight_Moves[i, 1] * 50) < 375) { info.Text += "новый Y меньше 375" + Environment.NewLine; }
                            //}
                            if (((Knight.Location.X + Knight_Moves[i, 0] * 50) < Knight.Location.X) && !(((125 < (Knight.Location.X + Knight_Moves[i, 0] * 50)) && ((Knight.Location.X + Knight_Moves[i, 0] * 50) < 375)) && ((125 < (Knight.Location.Y + Knight_Moves[i, 1] * 50)) && ((Knight.Location.Y + Knight_Moves[i, 1] * 50) < 375))))
                            {
                                if (is_allowed_move(1, Knight_Moves[i, 0] * 50, Knight_Moves[i, 1] * 50))
                                {
                                    Knight.Location = new Point(Knight.Location.X + Knight_Moves[i, 0] * 50, Knight.Location.Y + Knight_Moves[i, 1] * 50);
                                    if (Knight.BackColor == Color.White)
                                    {
                                        Knight.BackColor = Color.FromArgb(192, 255, 192);
                                    }
                                    else { Knight.BackColor = Color.White; }
                                    stage = 1;
                                    shodil = true;
                                    //break;
                                    return shodil;
                                }
                            }
                        }
                    }
                    if (Knight.Location.X < 125 && Knight.Location.Y < 175) // слепая зона зеленого
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if ((Knight.Location.Y + Knight_Moves[i, 1] * 50) > Knight.Location.Y)//здесь всё было плохо проверить другие места
                            {
                                if (is_allowed_move(1, Knight_Moves[i, 0] * 50, Knight_Moves[i, 1] * 50))
                                {
                                    Knight.Location = new Point(Knight.Location.X + Knight_Moves[i, 0] * 50, Knight.Location.Y + Knight_Moves[i, 1] * 50);
                                    if (Knight.BackColor == Color.White)
                                    {
                                        Knight.BackColor = Color.FromArgb(192, 255, 192);
                                    }
                                    else { Knight.BackColor = Color.White; }
                                    stage = 1;
                                    shodil = true;
                                    //break;
                                    return shodil;
                                }
                            }
                        }
                    }

                    if (Knight.Location.X < 175 && Knight.Location.Y < 425 && Knight.Location.Y > 125) // фиолетовая зона
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if (((Knight.Location.Y + Knight_Moves[i, 1] * 50) > Knight.Location.Y) && !((125 < (Knight.Location.X + Knight_Moves[i, 0] * 50) && (Knight.Location.X + Knight_Moves[i, 0] * 50) < 375) && (125 < (Knight.Location.Y + Knight_Moves[i, 1] * 50) && (Knight.Location.Y + Knight_Moves[i, 1] * 50) < 375)))
                            {
                                if (is_allowed_move(1, Knight_Moves[i, 0] * 50, Knight_Moves[i, 1] * 50))
                                {
                                    Knight.Location = new Point(Knight.Location.X + Knight_Moves[i, 0] * 50, Knight.Location.Y + Knight_Moves[i, 1] * 50);
                                    if (Knight.BackColor == Color.White)
                                    {
                                        Knight.BackColor = Color.FromArgb(192, 255, 192);
                                    }
                                    else { Knight.BackColor = Color.White; }
                                    stage = 1;
                                    shodil = true;
                                    //break;
                                    return shodil;
                                }
                            }
                        }
                    }
                    if (Knight.Location.Y > 375 && Knight.Location.X < 175) // слепая зона фиолетового
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if ((Knight.Location.X + Knight_Moves[i, 0] * 50) > Knight.Location.X)
                            {
                                if (is_allowed_move(1, Knight_Moves[i, 0] * 50, Knight_Moves[i, 1] * 50))
                                {
                                    Knight.Location = new Point(Knight.Location.X + Knight_Moves[i, 0] * 50, Knight.Location.Y + Knight_Moves[i, 1] * 50);
                                    if (Knight.BackColor == Color.White)
                                    {
                                        Knight.BackColor = Color.FromArgb(192, 255, 192);
                                    }
                                    else { Knight.BackColor = Color.White; }
                                    stage = 1;
                                    shodil = true;
                                    //break;
                                    return shodil;
                                }
                            }
                        }
                    }
                    napravleniye = 0;
                }

            }
            shodil = First_Knight_Strategy();
            return shodil;

        }

        private bool Second_Queen_Strategy() //вторая стратегия ферзя (ни единого шанса)
        {
            bool shodil = false;
            int x = Queen.Location.X;
            int y = Queen.Location.Y;
            int knight_chetvert;   //пв=1, лв=2, лн=3, пн=4
            if (Knight.Location.X < 275)
            {
                if (Knight.Location.Y < 275)
                {
                    knight_chetvert = 2;
                }
                else { knight_chetvert = 3; }
            }
            else
            {
                if (Knight.Location.Y < 275)
                {
                    knight_chetvert = 1;
                }
                else { knight_chetvert = 4; }
            }
            int[,,] udachniy_hod = new int[4, 10, 2] {
            {
                {0, 50 },
                {-50, 0 },
                {-100, 100 },
                {0, 150 },// усё
                {-150, 0 },
                {-50, 50 },
                {0, 100 },
                {-150, 100 },
                {-50, 200 },
                {-100, 0 },},
            { {0, 50 },
                {50, 0 },
                {100, 100 },
                {0, 150 },
                {150, 0 },
                {50, 50 },
                {0, 100 },
                {150, 100 },
                {50, 200 },
                {100, 0 },},
            { {0, -50 },
                {50, 0 },
                {100, -100 },
                {0, -150 },
                {150, 0 },
                {50, -50 },
                {0, -100 },
                {150, -100 },
                {50, -200 },
                {100, 0 },},
            { {0, -50 },
                {-50, 0 },
                {-100, -100 },
                {0, -150 },
                {-150, 0 },
                {-50, -50 },
                {0, -100 },
                {-150, -100 },
                {-50, -200 },
                {-100, 0 },},
            };
            for (int i = 0; i < 10; i++)
            {
                int xx = Knight.Location.X + udachniy_hod[knight_chetvert - 1, i, 0] - Queen.Location.X;
                int yy = Knight.Location.Y + udachniy_hod[knight_chetvert - 1, i, 1] - Queen.Location.Y;
                if (xx == 0 || yy == 0 || xx == yy || xx == -yy)
                {
                    if (is_allowed_move(2, (Knight.Location.X + udachniy_hod[knight_chetvert - 1, i, 0] - Queen.Location.X), (Knight.Location.Y + udachniy_hod[knight_chetvert - 1, i, 1] - Queen.Location.Y)))
                    {
                        Queen.Location = new Point(Knight.Location.X + udachniy_hod[knight_chetvert - 1, i, 0], Knight.Location.Y + udachniy_hod[knight_chetvert - 1, i, 1]);
                        shodil = true;
                        break;
                    }
                }
                //if () {
                //    if (is_allowed_move(2, (Knight.Location.X + udachniy_hod[knight_chetvert - 1, i, 0] - Queen.Location.X), (Knight.Location.Y + udachniy_hod[knight_chetvert - 1, i, 1] - Queen.Location.Y))) {
                //        Queen.Location = new Point(Knight.Location.X + udachniy_hod[knight_chetvert - 1, i, 0], Knight.Location.Y + udachniy_hod[knight_chetvert - 1, i, 1]);
                //        shodil = true;
                //        break;
                //    }
                //}
            }
            if (shodil == true)
            {
                virez(x, y);
            }
            else
            {
                shodil = First_Queen_Strategy();
            }
            return shodil;
        }
        private int Anti_Second_Queen_Strategy(int plus_x, int plus_y) //допольнительный метод для четвертой стратегии коня
        {
            int shodil = 0;
            int kx = Knight.Location.X + plus_x;
            int ky = Knight.Location.Y + plus_y;
            int x = Queen.Location.X;
            int y = Queen.Location.Y;
            int knight_chetvert;   //пв=1, лв=2, лн=3, пн=4
            if (kx < 275)
            {
                if (ky < 275)
                {
                    knight_chetvert = 2;
                }
                else { knight_chetvert = 3; }
            }
            else
            {
                if (ky < 275)
                {
                    knight_chetvert = 1;
                }
                else { knight_chetvert = 4; }
            }
            int[,,] udachniy_hod = new int[4, 10, 2] {
            {
                {0, 50 },
                {-50, 0 },
                {-100, 100 },
                {0, 150 },// усё
                {-150, 0 },
                {-50, 50 },
                {0, 100 },
                {-150, 100 },
                {-50, 200 },
                {-100, 0 },},
            { {0, 50 },
                {50, 0 },
                {100, 100 },
                {0, 150 },
                {150, 0 },
                {50, 50 },
                {0, 100 },
                {150, 100 },
                {50, 200 },
                {100, 0 },},
            { {0, -50 },
                {50, 0 },
                {100, -100 },
                {0, -150 },
                {150, 0 },
                {50, -50 },
                {0, -100 },
                {150, -100 },
                {50, -200 },
                {100, 0 },},
            { {0, -50 },
                {-50, 0 },
                {-100, -100 },
                {0, -150 },
                {-150, 0 },
                {-50, -50 },
                {0, -100 },
                {-150, -100 },
                {-50, -200 },
                {-100, 0 },},
            };
            for (int i = 0; i < 10; i++)
            {
                int xx = kx + udachniy_hod[knight_chetvert - 1, i, 0] - Queen.Location.X;
                int yy = ky + udachniy_hod[knight_chetvert - 1, i, 1] - Queen.Location.Y;
                if (xx == 0 || yy == 0 || xx == yy || xx == -yy)
                {
                    if (is_allowed_move(2, (kx + udachniy_hod[knight_chetvert - 1, i, 0] - Queen.Location.X), (ky + udachniy_hod[knight_chetvert - 1, i, 1] - Queen.Location.Y)))
                    {//выдача степеней опасности, чем больше тем хуже
                        if (i == 0)
                        {
                            shodil = 3;
                            break;
                        }
                        else if (i > 0 && i < 9)
                        {
                            shodil = 2;
                            break;
                        }
                        else
                        {
                            shodil = 1;
                            break;
                        }
                    }
                }
            }
            return shodil;
        }
        private bool New_Knight_Strategy()  //четвертая стратегия коня (на шаг впереди)
        {
            bool shodil = false;
            int[] moves = new int[8];
            int[] blizost_k_centru = new int[8];
            int min = int.MaxValue;
            for (int i = 0; i < 8; i++)
            {
                //moves[i, 0] = i;
                moves[i] = Anti_Second_Queen_Strategy(Knight_Moves[i, 0], Knight_Moves[i, 1]);
            }
            //поиск наименьшей оценки опасности
            for (int i = 0; i < 8; i++)
            {
                if (moves[i] < min)
                {
                    min = moves[i];
                }
            }
            //заполнения массива удаленности от центра
            for (int i = 0; i < 8; i++)
            {
                if (325 > Knight.Location.X + Knight_Moves[i, 0] * 50 && Knight.Location.X + Knight_Moves[i, 0] * 50 > 175 && 325 > Knight.Location.Y + Knight_Moves[i, 1] * 50 && Knight.Location.Y + Knight_Moves[i, 1] * 50 > 175)
                {
                    blizost_k_centru[i] = 0;
                }
                else if (375 > Knight.Location.X + Knight_Moves[i, 0] * 50 && Knight.Location.X + Knight_Moves[i, 0] * 50 > 125 && 375 > Knight.Location.Y + Knight_Moves[i, 1] * 50 && Knight.Location.Y + Knight_Moves[i, 1] * 50 > 125)
                {
                    blizost_k_centru[i] = 1;
                }
                else if (425 > Knight.Location.X + Knight_Moves[i, 0] * 50 && Knight.Location.X + Knight_Moves[i, 0] * 50 > 75 && 425 > Knight.Location.Y + Knight_Moves[i, 1] * 50 && Knight.Location.Y + Knight_Moves[i, 1] * 50 > 75)
                {
                    blizost_k_centru[i] = 2;
                }
                else
                {
                    blizost_k_centru[i] = 3;
                }

            }
            //поиск наилучшего хода
            for (int j = min; j < 4; j++) //цикл по оценке опасности
            {
                for (int i = 0; i < 4; i++)// цикл по удаленности
                {
                    for (int k = 0; k < 8; k++)
                    {
                        if (moves[k] == j && blizost_k_centru[k] == i)
                        {
                            if (is_allowed_move(1, Knight_Moves[k, 0] * 50, Knight_Moves[k, 1] * 50))
                            {
                                Knight.Location = new Point(Knight.Location.X + Knight_Moves[k, 0] * 50, Knight.Location.Y + Knight_Moves[k, 1] * 50);
                                if (Knight.BackColor == Color.White)
                                {
                                    Knight.BackColor = Color.FromArgb(192, 255, 192);
                                }
                                else { Knight.BackColor = Color.White; }
                                shodil = true;
                                //break;
                                return shodil;
                            }
                        }
                    }
                }
            }

            return shodil;
        }

        private bool center_Knight() //третья стратегия коня (стремление к центру)
        {
            bool shodil = false;
            for (int i = 0; i < 8; i++)
            {
                if (325 > Knight.Location.X + Knight_Moves[i, 0] * 50 && Knight.Location.X + Knight_Moves[i, 0] * 50 > 175 && 325 > Knight.Location.Y + Knight_Moves[i, 1] * 50 && Knight.Location.Y + Knight_Moves[i, 1] * 50 > 175)
                {
                    if (is_allowed_move(1, Knight_Moves[i, 0] * 50, Knight_Moves[i, 1] * 50))
                    {
                        Knight.Location = new Point(Knight.Location.X + Knight_Moves[i, 0] * 50, Knight.Location.Y + Knight_Moves[i, 1] * 50);
                        if (Knight.BackColor == Color.White)
                        {
                            Knight.BackColor = Color.FromArgb(192, 255, 192);
                        }
                        else { Knight.BackColor = Color.White; }
                        shodil = true;
                        return shodil;
                    }

                }
            }
            for (int i = 0; i < 8; i++)
            {
                if (375 > Knight.Location.X + Knight_Moves[i, 0] * 50 && Knight.Location.X + Knight_Moves[i, 0] * 50 > 125 && 375 > Knight.Location.Y + Knight_Moves[i, 1] * 50 && Knight.Location.Y + Knight_Moves[i, 1] * 50 > 125)
                {
                    if (is_allowed_move(1, Knight_Moves[i, 0] * 50, Knight_Moves[i, 1] * 50))
                    {
                        Knight.Location = new Point(Knight.Location.X + Knight_Moves[i, 0] * 50, Knight.Location.Y + Knight_Moves[i, 1] * 50);
                        if (Knight.BackColor == Color.White)
                        {
                            Knight.BackColor = Color.FromArgb(192, 255, 192);
                        }
                        else { Knight.BackColor = Color.White; }
                        shodil = true;
                        return shodil;
                    }

                }
            }
            for (int i = 0; i < 8; i++)
            {
                if (425 > Knight.Location.X + Knight_Moves[i, 0] * 50 && Knight.Location.X + Knight_Moves[i, 0] * 50 > 75 && 425 > Knight.Location.Y + Knight_Moves[i, 1] * 50 && Knight.Location.Y + Knight_Moves[i, 1] * 50 > 75)
                {
                    if (is_allowed_move(1, Knight_Moves[i, 0] * 50, Knight_Moves[i, 1] * 50))
                    {
                        Knight.Location = new Point(Knight.Location.X + Knight_Moves[i, 0] * 50, Knight.Location.Y + Knight_Moves[i, 1] * 50);
                        if (Knight.BackColor == Color.White)
                        {
                            Knight.BackColor = Color.FromArgb(192, 255, 192);
                        }
                        else { Knight.BackColor = Color.White; }
                        shodil = true;
                        return shodil;
                    }

                }
            }
            shodil = First_Knight_Strategy();
            return shodil;
        }

        private void Update() //функция обновления отрисовки (перерисовывание всех объектов)
        {
            pb1.Update();
            pb2.Update();
            pb3.Update();
            pb4.Update();
            pb5.Update();
            pb6.Update();
            pb7.Update();
            pb8.Update();
            pb9.Update();
            pb10.Update();
            pb11.Update();
            pb12.Update();
            pb13.Update();
            pb14.Update();
            pb15.Update();
            pb16.Update();
            pb17.Update();
            pb18.Update();
            pb19.Update();
            pb20.Update();
            pb21.Update();
            pb22.Update();
            pb23.Update();
            pb24.Update();
            pb25.Update();
            pb26.Update();
            pb27.Update();
            pb28.Update();
            pb29.Update();
            pb30.Update();
            pb31.Update();
            pb32.Update();
            pb33.Update();
            pb34.Update();
            pb35.Update();
            pb36.Update();
            pb37.Update();
            pb38.Update();
            pb39.Update();
            pb40.Update();
            pb41.Update();
            pb42.Update();
            pb43.Update();
            pb44.Update();
            pb45.Update();
            pb46.Update();
            pb47.Update();
            pb48.Update();
            pb49.Update();
            pb50.Update();
            pb51.Update();
            pb52.Update();
            pb53.Update();
            pb54.Update();
            pb55.Update();
            pb56.Update();
            pb57.Update();
            pb58.Update();
            pb59.Update();
            pb60.Update();
            pb61.Update();
            pb62.Update();
            pb63.Update();
            pb64.Update();
            Knight.Update();
            Queen.Update();
            info.Update();
            Scores.Update();
            progressBar1.Update();
        }
        private void Rebute()  //функция сброса всех выколотых клеток
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Allowed_cells[i, j] = true;
                }
            }
            Knight.Location = new Point(kx, ky);
            Queen.Location = new Point(qx, qy);

            pb1.BackColor = Color.White;
            pb2.BackColor = Color.FromArgb(192, 255, 192);
            pb3.BackColor = Color.White;
            pb4.BackColor = Color.FromArgb(192, 255, 192);
            pb5.BackColor = Color.White;
            pb6.BackColor = Color.FromArgb(192, 255, 192);
            pb7.BackColor = Color.White;
            pb8.BackColor = Color.FromArgb(192, 255, 192);
            pb9.BackColor = Color.FromArgb(192, 255, 192);
            pb10.BackColor = Color.White;
            pb11.BackColor = Color.FromArgb(192, 255, 192);
            pb12.BackColor = Color.White;
            pb13.BackColor = Color.FromArgb(192, 255, 192);
            pb14.BackColor = Color.White;
            pb15.BackColor = Color.FromArgb(192, 255, 192);
            pb16.BackColor = Color.White;
            pb17.BackColor = Color.White;
            pb18.BackColor = Color.FromArgb(192, 255, 192);
            pb19.BackColor = Color.White;
            pb20.BackColor = Color.FromArgb(192, 255, 192);
            pb21.BackColor = Color.White;
            pb22.BackColor = Color.FromArgb(192, 255, 192);
            pb23.BackColor = Color.White;
            pb24.BackColor = Color.FromArgb(192, 255, 192);
            pb25.BackColor = Color.FromArgb(192, 255, 192);
            pb26.BackColor = Color.White;
            pb27.BackColor = Color.FromArgb(192, 255, 192);
            pb28.BackColor = Color.White;
            pb29.BackColor = Color.FromArgb(192, 255, 192);
            pb30.BackColor = Color.White;
            pb31.BackColor = Color.FromArgb(192, 255, 192);
            pb32.BackColor = Color.White;
            pb33.BackColor = Color.White;
            pb34.BackColor = Color.FromArgb(192, 255, 192);
            pb35.BackColor = Color.White;
            pb36.BackColor = Color.FromArgb(192, 255, 192);
            pb37.BackColor = Color.White;
            pb38.BackColor = Color.FromArgb(192, 255, 192);
            pb39.BackColor = Color.White;
            pb40.BackColor = Color.FromArgb(192, 255, 192);
            pb41.BackColor = Color.FromArgb(192, 255, 192);
            pb42.BackColor = Color.White;
            pb43.BackColor = Color.FromArgb(192, 255, 192);
            pb44.BackColor = Color.White;
            pb45.BackColor = Color.FromArgb(192, 255, 192);
            pb46.BackColor = Color.White;
            pb47.BackColor = Color.FromArgb(192, 255, 192);
            pb48.BackColor = Color.White;
            pb49.BackColor = Color.White;
            pb50.BackColor = Color.FromArgb(192, 255, 192);
            pb51.BackColor = Color.White;
            pb52.BackColor = Color.FromArgb(192, 255, 192);
            pb53.BackColor = Color.White;
            pb54.BackColor = Color.FromArgb(192, 255, 192);
            pb55.BackColor = Color.White;
            pb56.BackColor = Color.FromArgb(192, 255, 192);
            pb57.BackColor = Color.FromArgb(192, 255, 192);
            pb58.BackColor = Color.White;
            pb59.BackColor = Color.FromArgb(192, 255, 192);
            pb60.BackColor = Color.White;
            pb61.BackColor = Color.FromArgb(192, 255, 192);
            pb62.BackColor = Color.White;
            pb63.BackColor = Color.FromArgb(192, 255, 192);
            pb64.BackColor = Color.White;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (viborFiguri == 0)
            {
                viborFiguri = 1;
                button2.BackColor = Color.FromArgb(192, 255, 192);
                button2.Image = Properties.Resources.horse2;
            }
            else if (viborFiguri == 1)
            {
                viborFiguri = 2;
                button2.Image = Properties.Resources.icons8_craft_50;
                button2.BackColor = Color.Orange;
                pictureBox1.Location = new Point(80, 80);
            }
            else
            {
                viborFiguri = 0;
                button2.Image = Properties.Resources.queen_chess_piece_black_shape;
                pictureBox1.Location = new Point(24, 80);
                button2.BackColor = Color.FromArgb(192, 255, 192);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (viborFiguri == 1)
            {
                if (Knight.Location.X + 50 <= 425)
                {
                    if (Knight.Location.X + 50 != Queen.Location.X || Knight.Location.Y != Queen.Location.Y)
                    {
                        Knight.Location = new Point(Knight.Location.X + 50, Knight.Location.Y);
                        kx = Knight.Location.X;
                        if (Knight.BackColor == Color.White)
                        {
                            Knight.BackColor = Color.FromArgb(192, 255, 192);
                        }
                        else { Knight.BackColor = Color.White; }
                    }
                }
            }
            if (viborFiguri == 0)
            {
                if (Queen.Location.X + 50 <= 425)
                {
                    if (Knight.Location.X != Queen.Location.X + 50 || Knight.Location.Y != Queen.Location.Y)
                    {
                        Queen.Location = new Point(Queen.Location.X + 50, Queen.Location.Y);
                        qx = Queen.Location.X;
                        if (Queen.BackColor == Color.White)
                        {
                            Queen.BackColor = Color.FromArgb(192, 255, 192);
                        }
                        else { Queen.BackColor = Color.White; }
                    }
                }
            }

            if (viborFiguri == 2)
            {
                if (pictureBox1.Location.X + 40 <= 425)
                {
                    if (Knight.Location.X != pictureBox1.Location.X + 40 || Knight.Location.Y != pictureBox1.Location.Y)
                    {
                        if (Queen.Location.X != pictureBox1.Location.X + 40 || Queen.Location.Y != pictureBox1.Location.Y)
                        {
                            pictureBox1.Location = new Point(pictureBox1.Location.X + 50, pictureBox1.Location.Y);

                        }
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (viborFiguri == 1)
            {
                if (Knight.Location.X - 50 >= 75)
                {
                    if (Knight.Location.X - 50 != Queen.Location.X || Knight.Location.Y != Queen.Location.Y)
                    {
                        Knight.Location = new Point(Knight.Location.X - 50, Knight.Location.Y);
                        kx = Knight.Location.X;
                        if (Knight.BackColor == Color.White)
                        {
                            Knight.BackColor = Color.FromArgb(192, 255, 192);
                        }
                        else { Knight.BackColor = Color.White; }
                    }
                }
            }
            if (viborFiguri == 0)
            {
                if (Queen.Location.X - 50 >= 75)
                {
                    if (Knight.Location.X != Queen.Location.X - 50 || Knight.Location.Y != Queen.Location.Y)
                    {
                        Queen.Location = new Point(Queen.Location.X - 50, Queen.Location.Y);
                        qx = Queen.Location.X;
                        if (Queen.BackColor == Color.White)
                        {
                            Queen.BackColor = Color.FromArgb(192, 255, 192);
                        }
                        else { Queen.BackColor = Color.White; }
                    }
                }
            }
            if (viborFiguri == 2)
            {
                if (pictureBox1.Location.X - 50 >= 75)
                {
                    if (Knight.Location.X != pictureBox1.Location.X - 60 || Knight.Location.Y != pictureBox1.Location.Y)
                    {
                        if (Queen.Location.X != pictureBox1.Location.X - 60 || Queen.Location.Y != pictureBox1.Location.Y)
                        {
                            pictureBox1.Location = new Point(pictureBox1.Location.X - 50, pictureBox1.Location.Y);

                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (viborFiguri == 1)
            {
                if (Knight.Location.Y - 50 >= 75)
                {
                    if (Knight.Location.X != Queen.Location.X || Knight.Location.Y - 50 != Queen.Location.Y)
                    {
                        Knight.Location = new Point(Knight.Location.X, Knight.Location.Y - 50);
                        ky = Knight.Location.Y;
                        if (Knight.BackColor == Color.White)
                        {
                            Knight.BackColor = Color.FromArgb(192, 255, 192);
                        }
                        else { Knight.BackColor = Color.White; }
                    }
                }
            }
            if (viborFiguri == 0)
            {
                if (Queen.Location.Y - 50 >= 75)
                {
                    if (Knight.Location.X != Queen.Location.X || Knight.Location.Y != Queen.Location.Y - 50)
                    {
                        Queen.Location = new Point(Queen.Location.X, Queen.Location.Y - 50);
                        qy = Queen.Location.Y;
                        if (Queen.BackColor == Color.White)
                        {
                            Queen.BackColor = Color.FromArgb(192, 255, 192);
                        }
                        else { Queen.BackColor = Color.White; }
                    }
                }
            }
            if (viborFiguri == 2)
            {
                if (pictureBox1.Location.Y - 50 >= 75)
                {
                    if (Knight.Location.X != pictureBox1.Location.X || Knight.Location.Y != pictureBox1.Location.Y - 60)
                    {
                        if (Queen.Location.X != pictureBox1.Location.X || Queen.Location.Y != pictureBox1.Location.Y - 60)
                        {
                            pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y - 50);

                        }
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (viborFiguri == 1)
            {
                if (Knight.Location.Y + 50 <= 425)
                {
                    if (Knight.Location.X != Queen.Location.X || Knight.Location.Y + 50 != Queen.Location.Y)
                    {
                        Knight.Location = new Point(Knight.Location.X, Knight.Location.Y + 50);
                        ky = Knight.Location.Y;
                        if (Knight.BackColor == Color.White)
                        {
                            Knight.BackColor = Color.FromArgb(192, 255, 192);
                        }
                        else { Knight.BackColor = Color.White; }
                    }
                }
            }
            if (viborFiguri == 0)
            {
                if (Queen.Location.Y + 50 <= 425)
                {
                    if (Knight.Location.X != Queen.Location.X || Knight.Location.Y != Queen.Location.Y + 50)
                    {
                        Queen.Location = new Point(Queen.Location.X, Queen.Location.Y + 50);
                        qy = Queen.Location.Y;
                        if (Queen.BackColor == Color.White)
                        {
                            Queen.BackColor = Color.FromArgb(192, 255, 192);
                        }
                        else { Queen.BackColor = Color.White; }
                    }
                }
            }
            if (viborFiguri == 2)
            {
                if (pictureBox1.Location.Y + 40 <= 425)
                {
                    if (Knight.Location.Y != pictureBox1.Location.Y + 40 || Knight.Location.X != pictureBox1.Location.X)
                    {
                        if (Queen.Location.Y != pictureBox1.Location.Y + 40 || Queen.Location.X != pictureBox1.Location.X)
                        {
                            pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + 50);

                        }
                    }
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Qw = 0;
            Kw = 0;
            Scores.Text = "";
            Rebute();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Location.X > 75)
            {
                virez(pictureBox1.Location.X - 5, pictureBox1.Location.Y - 5);
            }
        }
        private bool bet_li_kon_ferzya() //вспомогательная функция для "random_sets" возврващающая истину, если ферзь не бьётся конем и их позиции не совпадают
        { 
            bool allowed = true;
            for (int i = 0; i < 8; i++) {
                if (Queen.Location.X == Knight.Location.X + Knight_Moves[i,0] && Queen.Location.Y == Knight.Location.Y + Knight_Moves[i, 1]) {
                    allowed = false;
                    break;
                }
            }
            return allowed;
        }

        private void random_sets() //метод расставляющий фигуры на доске в случайном порядке
        {
            Knight.Location = new Point(rnd.Next(1,8)*50+75, rnd.Next(1, 8) * 50 + 75);
            Queen.Location = new Point(rnd.Next(1, 8) * 50 + 75, rnd.Next(1, 8) * 50 + 75);
            while ((Knight.Location == Queen.Location) || (!bet_li_kon_ferzya()))
            { 
            Queen.Location = new Point(rnd.Next(1, 8) * 50 + 75, rnd.Next(1, 8) * 50 + 75);
            }
        }
    }
}
