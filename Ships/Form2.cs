using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ships
{
    public partial class Form2 : Form
    {
        PictureBox pictureBox = new PictureBox(); 
        int x, y;

        int cnt = 0; int round = 1;

        int[,] check = new int[10, 10];
        int[,] check1 = new int[10, 10];

        int hits1 = 0, misses1 = 0;
        int hits2 = 0, misses2 = 0;
        public Form2()
        {
            InitializeComponent();
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            //Setup Board1 (Player)
            board1.BackgroundColor = Color.Black;
            board1.DefaultCellStyle.BackColor = Color.Navy;
            board1.GridColor = Color.LightBlue;
            board1.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            for (int i = 0; i < 10; i++)
            {
                board1.Rows.Add();
            }

            foreach (DataGridViewColumn c in board1.Columns)
            {
                c.Width = board1.Width / board1.Columns.Count;
            }

            foreach (DataGridViewRow r in board1.Rows)
            {
                r.Height = board1.Height / board1.Rows.Count;
            }

            Color color = Color.White;
            int tip = - 1;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    tip = Globals.tabla1[i, j];

                    switch (tip)
                    {
                        case -1: color = Color.Navy; break;
                        case 0: color = Color.Yellow; break;
                        case 1: color = Color.Red; break;
                        case 2: color = Color.Green; break;
                        case 3: color = Color.Blue; break;
                        case 4: color = Color.Purple; break;
                    }

                    board1[i, j].ReadOnly = true;
                    board1.Rows[i].Cells[j].Style.BackColor = color;
                    board1.Rows[i].Cells[j].Style.ForeColor = Color.White;
                }
            }

           //Setup Board2 (Bot)
            board2.BackgroundColor = Color.Black;
            board2.DefaultCellStyle.BackColor = Color.Navy;
            board2.GridColor = Color.LightBlue;
            board2.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            for (int i = 0; i < 10; i++)
            {
                board2.Rows.Add();
            }

            foreach (DataGridViewColumn c in board2.Columns)
            {
                c.Width = board2.Width / board2.Columns.Count;
            }

            foreach (DataGridViewRow r in board2.Rows)
            {
                r.Height = board2.Height / board2.Rows.Count;
            }

            for (int row = 0; row < board2.Rows.Count; row++)
            {
                for (int col = 0; col < board2.Columns.Count; col++)
                {
                    board2[row, col].ReadOnly = true;
                }
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    check[i, j] = 0;
                    check1[i, j] = 0;
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void CheckForWin()
        {
            int max1 = 0;
            int max2 = 0;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (Globals.tabla1[i, j] != -1) max1++;
                    if (Globals.tabla2[i, j] != -1) max2++;
                }
            }

            if (hits2 >= max1 && hits1 >= max2)
            {
                MessageBox.Show("Draw!");
                System.Environment.Exit(0);
            }
            else if (hits2 >= max1)
            {
                MessageBox.Show("You lost!");
                System.Environment.Exit(0);
            }
            else if (hits1 >= max2)
            {
                MessageBox.Show("You won!");
                System.Environment.Exit(0);
            }
        }

        private void board2_SelectionChanged(object sender, EventArgs e)
        {
            board1.ClearSelection();
        }

        private void board1_SelectionChanged(object sender, EventArgs e)
        {
            board2.ClearSelection();
        }

        private async void board2_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            int x = (int)e.RowIndex;
            int y = (int)e.ColumnIndex;
            if (cnt % 2 == 0 && check1[x, y] == 0)
            {

                PictureBox pictureBox = new PictureBox();
                pictureBox.Image = Properties.Resources.question;
                pictureBox.Location = new Point(y * 30 + 1, x * 30 + 1);
                pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                board2.Controls.Add(pictureBox);

                int tip = Globals.tabla2[x, y];
                Color color = Color.White;
                string nume = "";

                switch (tip)
                {
                    case -1: color = Color.Navy; nume = "nimic"; break;
                    case 0: color = Color.Yellow; nume = "Patrol Boat"; break;
                    case 1: color = Color.Red; nume = "Submarine"; break;
                    case 2: color = Color.Green; nume = "Destroyer"; break;
                    case 3: color = Color.Blue; nume = "battleship"; break;
                    case 4: color = Color.Purple; nume = "Aircraft Carrier"; break;
                }

                string mesaj;
                if (tip == -1)
                {
                    misses1++;

                    mesaj = $"Round: " + textBox1.Text + "  You attacked cell(" + x + ", " + y + "), and you failed to hit your opponents ships.\n";

                    pictureBox.Image = Properties.Resources.splashImage;
                    pictureBox.Location = new Point(y * 30 + 1, x * 30 + 1);
                    pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                    board2.Controls.Add(pictureBox);
                }
                else
                {
                    hits1++;

                    mesaj = $"Round: " + textBox1.Text + "  You attacked cell(" + x + ", " + y + "), and you managed to hit " + nume + ".\n";
                    pictureBox.Image = Properties.Resources.hitImage;
                    pictureBox.Location = new Point(y * 30 + 1, x * 30 + 1);
                    pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                    board2.Controls.Add(pictureBox);
                }
                int start = richTextBox1.TextLength;
                richTextBox1.AppendText(mesaj);
                richTextBox1.Select(start, mesaj.Length);
                richTextBox1.SelectionColor = color;
                richTextBox1.SelectionStart = richTextBox1.TextLength;
                richTextBox1.SelectionLength = 0;
                richTextBox1.ScrollToCaret();

                label1.Text = "Hits: " + hits1;
                label2.Text = "Misses: " + misses1;

                check1[x, y] = 1;

                if (cnt % 2 == 0) { 
                    cnt++; timer1.Start(); 
                    label7.Visible = false;
                    CheckForWin();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Random rnd = new Random();

            int x = rnd.Next(0, 10);
            int y = rnd.Next(0, 10);
            while (check[x, y] != 0)
            {
                x = rnd.Next(0, 10);
                y = rnd.Next(0, 10);
            }

            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = Properties.Resources.question;
            pictureBox.Location = new Point(y * 30 + 1, x * 30 + 1);
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            board2.Controls.Add(pictureBox);

            check[x, y] = 1;
            int tip = Globals.tabla1[x, y];
            Color color = Color.White;
            string nume = "";

            switch (tip)
            {
                case -1: color = Color.Navy; nume = "nimic"; break;
                case 0: color = Color.Yellow; nume = "Patrol Boat"; break;
                case 1: color = Color.Red; nume = "Submarine"; break;
                case 2: color = Color.Green; nume = "Destroyer"; break;
                case 3: color = Color.Blue; nume = "battleship"; break;
                case 4: color = Color.Purple; nume = "Aircraft Carrier"; break;
            }

            string mesaj;
            if (tip == -1)
            {
                misses2++;

                mesaj = $"Round: " + textBox1.Text + "  You have been attacked by your opponent at cell(" + x + ", " + y + "). Luckly nothing happend. It hit the water.\n";

                pictureBox.Image = Properties.Resources.splashImage;
                pictureBox.Location = new Point(y * 30 + 1, x * 30 + 1);
                pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                board1.Controls.Add(pictureBox);
            }
            else
            {
                hits2++;

                mesaj = $"Round: " + textBox1.Text + "  You have been attacked by your opponent at cell(" + x + ", " + y + "), and they managed to hit " + nume + ". Unlucky! =_=\n";

                pictureBox.Image = Properties.Resources.hitImage;
                pictureBox.Location = new Point(y * 30 + 1, x * 30 + 1);
                pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                board1.Controls.Add(pictureBox);
            }
            int start = richTextBox1.TextLength;
            richTextBox1.AppendText(mesaj);
            richTextBox1.Select(start, mesaj.Length);
            richTextBox1.SelectionColor = color;


            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;
            richTextBox1.ScrollToCaret();

            label3.Text = "Hits: " + hits2;
            label4.Text = "Misses: " + misses2;

            cnt++;
            round++;
            if (round < 10) textBox1.Text = "0" + round.ToString();
            else textBox1.Text = round.ToString();

            label7.Visible = true;
            timer1.Stop();
            CheckForWin();
        }
    }
}
