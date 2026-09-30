using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Ships
{
    public partial class Form1 : Form
    {
        int[,] m = new int[10, 10];
        int[,] matrice = new int[10, 10];

        string direction = "left"; int cnt = 0;

        bool[] care = new bool[] { false, false, false, false, false };
        int index = 0;

        bool ok1 = false;

        int hoverRow = -1;
        int hoverCol = -1;
        public Form1()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            InitializeBoard();
        }

        private void InitializeBoard()
        {
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

            for (int row = 0; row < board1.Rows.Count; row++)
            {
                for(int col = 0;col < board1.Columns.Count; col++)
                {
                    board1[row, col].ReadOnly = true;
                    m[row, col] = -1;
                }
            }

            board1.ClearSelection();
        }

        private void ship0_Click(object sender, EventArgs e)
        {
            PictureBox[] pictureBoxes = new PictureBox[] { ship0, ship1, ship2, ship3, ship4 };

            PictureBox p = sender as PictureBox;
            int cnt = 0;

            if (p.Tag.ToString() == "ship")
            {
                foreach (PictureBox pictureBox in pictureBoxes)
                {
                    if (pictureBox.BorderStyle == BorderStyle.Fixed3D)
                    {
                        pictureBox.BorderStyle = BorderStyle.None;
                        pictureBox.BackColor = Color.Silver;
                    }
                }
                foreach (PictureBox pictureBox in pictureBoxes)
                {
                    if (p == pictureBox)
                    {
                        index = cnt;
                    }
                    cnt++;
                }
                if (p.BackColor != Color.Black)
                {
                    p.BorderStyle = BorderStyle.Fixed3D;
                    p.BackColor = Color.White;
                }

            }
            if (p.Tag.ToString() == "x")
            {
                char nr = p.Name[1];
                foreach (PictureBox p1 in pictureBoxes)
                {
                    char nr1 = p1.Name[4];
                    if (nr == nr1)
                    {
                        p1.BorderStyle = BorderStyle.None;
                        p1.BackColor = Color.Silver;
                    }
                }
            }
        }

        private void board1_MouseMove(object sender, MouseEventArgs e)
        {
            var hit = board1.HitTest(e.X, e.Y);
            int row = hit.RowIndex;
            int col = hit.ColumnIndex;

            if (row < 0 || col < 0 || row > 9 || col > 9)
                return;

            if (row == hoverRow && col == hoverCol)
            {
                return;
            }

            hoverRow = row;
            hoverCol = col;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (m[i, j] == -1)
                    {
                        board1.Rows[i].Cells[j].Style.BackColor = Color.Navy;
                        board1.Rows[i].Cells[j].Style.ForeColor = Color.White;
                    }
                }
            }

            PictureBox activeShip = null;
            PictureBox[] ships = new PictureBox[] { ship0, ship1, ship2, ship3, ship4 };

            foreach (PictureBox ship in ships)
            {
                if (ship.Tag?.ToString() == "ship" && ship.BorderStyle == BorderStyle.Fixed3D)
                {
                    activeShip = ship;
                    break;
                }
            }

            if (activeShip == null) return;

            Color color = Color.White;
            int length = 1;
            switch (activeShip.Name)
            {
                case "ship0": color = Color.Yellow; length = 2; break;
                case "ship1": color = Color.Red; length = 3; break;
                case "ship2": color = Color.Green; length = 3; break;
                case "ship3": color = Color.Blue; length = 4; break;
                case "ship4": color = Color.Purple; length = 5; break;
            }

            if (direction == "left")
            {
                if (col - length + 1 < 0) return;
            }
            if (direction == "up")
            {
                if (row - length + 1 < 0) return;
            }

            if (care[index] == false)
            {
                if (direction == "left")
                {
                    bool verifica = true;
                    for (int i = 0; i < length; i++)
                    {
                        if (m[row, col - i] != -1)
                        {
                            verifica = false;
                        }
                    }

                    if (verifica)
                    {
                        for (int i = 0; i < length; i++)
                        {
                            board1.Rows[row].Cells[col - i].Style.BackColor = color;
                            board1.Rows[row].Cells[col - i].Style.ForeColor = Color.White;
                        }
                    }
                }
                else if (direction == "up")
                {
                    bool verifica = true;
                    for (int i = 0; i < length; i++)
                    {
                        if (m[row - i, col] != - 1)
                        {
                            verifica = false;
                        }
                    }

                    if (verifica)
                    {
                        for (int i = 0; i < length; i++)
                        {
                            board1.Rows[row - i].Cells[col].Style.BackColor = color;
                            board1.Rows[row - i].Cells[col].Style.ForeColor = Color.White;
                        }
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void board1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;

            if (row < 0 || col < 0 || row > 9 || col > 9)
                return;

            PictureBox activeShip = null;
            PictureBox[] ships = new PictureBox[] { ship0, ship1, ship2, ship3, ship4 };

            foreach (PictureBox ship in ships)
            {
                if (ship.Tag?.ToString() == "ship" && ship.BorderStyle == BorderStyle.Fixed3D)
                {
                    activeShip = ship;
                    break;
                }
            }

            if (activeShip == null) return;

            Color color = Color.White;
            int length = 1;
            int tip = -1;
            switch (activeShip.Name)
            {
                case "ship0": color = Color.Yellow; length = 2; tip = 0; break;
                case "ship1": color = Color.Red; length = 3; tip = 1; break;
                case "ship2": color = Color.Green; length = 3; tip = 2; break;
                case "ship3": color = Color.Blue; length = 4; tip = 3; break;
                case "ship4": color = Color.Purple; length = 5;  tip = 4; break;
            }


            if (direction == "left")
            {
                if (col - length + 1 < 0) return;
            }
            if (direction == "up")
            {
                if (row - length + 1 < 0) return;
            }

            if (care[index] == false)
            {
                for (int i = 0; i < length; i++)
                {
                    if (direction == "left")
                    {
                        board1.Rows[row].Cells[col - i].Style.BackColor = color;
                        board1.Rows[row].Cells[col - i].Style.ForeColor = Color.White;

                        m[row, col - i] = tip;

                    }
                    else if (direction == "up")
                    {
                        board1.Rows[row - i].Cells[col].Style.BackColor = color;
                        board1.Rows[row - i].Cells[col].Style.ForeColor = Color.White;

                        m[row - i, col] = tip;
                    }
                }
            }
            activeShip.BorderStyle = BorderStyle.None;
            activeShip.BackColor = Color.Black;
            care[index] = true;
        }

        private void rotate_Click(object sender, EventArgs e)
        {
            cnt++;
            if (cnt % 2 == 0)
            {
                direction = "left";
            }
            
            if ((cnt % 2) == 1)
            {
                direction = "up";
            }
        }

        private void CheckAllShips()
        {
            ok1 = false;
            int cnt = 0;
            PictureBox[] ships = new PictureBox[] { ship0, ship1, ship2, ship3, ship4 };

            foreach (PictureBox ship in ships)
            {
                if (ship.BackColor == Color.Black)
                {
                    cnt++;
                }
            }
            if (cnt == 5)
            {
                ok1 = true;
            }
        }

        private void check_Click(object sender, EventArgs e)
        {
            CheckAllShips();

            if (ok1)
            {
                board2.BackgroundColor = Color.Black;
                board2.DefaultCellStyle.BackColor = Color.Navy;

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

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        board2[i, j].ReadOnly = true;
                        matrice[i, j] = -1;
                    }
                }

                Random rnd = new Random();
                bool[] care1 = new bool[] { false, false, false, false, false };
                PictureBox[] ships = new PictureBox[] { ship0, ship1, ship2, ship3, ship4 };
                string directie1 = "";
                PictureBox activeShip = null;

                for (int i = 0; i <= 4; i++)
                {
                    int nr = rnd.Next(0, 5);
                    while (care1[nr] == true)
                    {
                        nr = rnd.Next(0, 5);
                    }

                    care1[nr] = true;
                    activeShip = ships[nr];

                    int nr1 = rnd.Next(0, 2);
                    if (nr1 == 0)
                    {
                        directie1 = "left";
                    }
                    else if (nr1 == 1)
                    {
                        directie1 = "up";
                    }

                    Color color = Color.White;
                    int length = 1;
                    int tip = -1;
                    int x = 0, y = 0;

                    switch (activeShip.Name)
                    {
                        case "ship0": color = Color.Yellow; length = 2; tip = 0; break;
                        case "ship1": color = Color.Red; length = 3; tip = 1; break;
                        case "ship2": color = Color.Green; length = 3; tip = 2; break;
                        case "ship3": color = Color.Blue; length = 4; tip = 3; break;
                        case "ship4": color = Color.Purple; length = 5; tip = 4; break;
                    }

                    if (directie1 == "left")
                    {
                        bool ok = false;
                        while (ok == false)
                        {
                            int cnt = 0;
                            x = rnd.Next(0, 10);
                            y = rnd.Next(length - 1, 10);
                            while (matrice[x, y] != -1)
                            {
                                x = rnd.Next(0, 10);
                                y = rnd.Next(length + 1, 10);
                            }
                            for (int k = 0; k < length; k++)
                            {
                                if (matrice[x, y - k] == -1)
                                {
                                    cnt++;
                                }
                            }
                            if (cnt == length) ok = true;
                        }
                    }
                    else if (directie1 == "up")
                    {
                        bool ok = false;
                        while (ok == false)
                        {
                            int cnt = 0;
                            x = rnd.Next(length - 1, 10);
                            y = rnd.Next(0, 10);
                            while (matrice[x, y] != -1)
                            {
                                x = rnd.Next(length + 1, 10);
                                y = rnd.Next(0, 10);
                            }
                            for (int k = 0; k < length; k++)
                            {
                                if (matrice[x - k, y] == -1)
                                {
                                    cnt++;
                                }
                            }
                            if (cnt == length) ok = true;
                        }
                    }

                    for (int j = 0; j < length; j++)
                    {
                        if (directie1 == "left")
                        {
                            board2.Rows[x].Cells[y - j].Style.BackColor = color;
                            board2.Rows[x].Cells[y - j].Style.ForeColor = Color.White;

                            matrice[x, y - j] = tip;

                        }
                        else if (directie1 == "up")
                        {
                            board2.Rows[x - j].Cells[y].Style.BackColor = color;
                            board2.Rows[x - j].Cells[y].Style.ForeColor = Color.White;

                            matrice[x - j, y] = tip;
                        }
                    }
                }

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Globals.tabla1[i, j] = m[i, j];
                        Globals.tabla2[i, j] = matrice[i, j];
                    }

                }

                this.Hide();
                Form2 form2 = new Form2();
                form2.Show();
            }
            else { MessageBox.Show("To continue please place all your battleships."); }
        }

        private void board1_SelectionChanged(object sender, EventArgs e)
        {
            board1.ClearSelection();
        }
    }

    public static class Globals
    {
        public static int[,] tabla1 = new int[10,10];
        public static int[,] tabla2 = new int[10,10];
    }
}
