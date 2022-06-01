using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        int Ks = 0;
        int Qs = 0;

        bool[,] Allowed_cells = new bool[8, 8] {
            {true, true, true, true, true, true, true, true },
            {true, true, true, true, true, true, true, true },
            {true, true, true, false, true, true, true, true },
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
            if (is_allowed_move(1, 50, 100))
            {
                Knight.Location = new Point(Knight.Location.X + 50, Knight.Location.Y + 100);
                info.Text += "\n Конь сходил " + kx.ToString() + "; " + ky.ToString() + Environment.NewLine; ;
            }
            else
            {
                info.Text += "\n Ход конем " + kx.ToString() + "; " + ky.ToString() + " не возможен" + Environment.NewLine; ;
            }


        }
        private bool is_not_dark( int x, int y) { //передавать координаты как есть
            bool allowed = false;
            x = (x-75) / 50;
            y = (y-75) / 50;
            if (Allowed_cells[y,x] == true) {
                allowed = true;
            }
            return allowed;
        }
        private bool is_allowed_move(int figure, int x, int y) {
            bool allowed = false;
            if (figure == 1) {
                if (Knight.Location.X + x <= 425 && Knight.Location.X + x >= 75) {
                    if (Knight.Location.Y + y <= 425 && Knight.Location.Y + y >= 75) {
                        if (is_not_dark(Knight.Location.X + x, Knight.Location.Y + y))
                        {
                            allowed = true;
                        }
                    }
                }
            }
            if (figure == 2)
            {
                if (Queen.Location.X + x <= 425 && Queen.Location.X + x >= 75){
                    if (Queen.Location.Y + y <= 425 && Queen.Location.Y + y >= 75){
                        if (is_not_dark(x, y)){
                            allowed = true;
                        }
                    }
                }
            }
            return allowed;




        }
        private void virez(int x, int y) {
            x = (x - 75) / 50;
            y = (y - 75) / 50;
            int number = x * 8 + y + 1;
            if (Allowed_cells[y, x] == false)
            {

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
                    case 38:
                        pb38.BackColor = Color.Gray; break;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ks = comboBox1.SelectedIndex;
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Qs = comboBox1.SelectedIndex;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            mm = int.Parse(textBox1.Text);
        }
    }
}
