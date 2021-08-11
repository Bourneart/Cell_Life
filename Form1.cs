using System;
using System.Drawing;
using System.Windows.Forms;

namespace CreateLife2
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private int resolution;
        private GameLogic gameLogic;
        public Form1()
        {
            InitializeComponent();
        }

        private void StartGenerate() //Начало генерации поколения клеток (старт)
        {
            if (timer1.Enabled)
                return;

            numResolution.Enabled = false;
            numDensity.Enabled = false;
            resolution = (int)numResolution.Value;

            gameLogic = new GameLogic
            (
                rows: pictureBox1.Height / resolution,
                cols: pictureBox1.Width / resolution,
                density: (int)(numDensity.Minimum) + (int)numDensity.Maximum - (int)numDensity.Value
            );

            Text = $"Iteration {gameLogic.iterations}";

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }

        private void DrawNextGeneration() //Отрисовка следующего поколения клеток
        {
            graphics.Clear(Color.Black);

            var field = gameLogic.GetCurrentGeneration();
            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x,y])
                        graphics.FillRectangle(Brushes.Green, x * resolution, y * resolution, resolution - 1, resolution - 1);
                }
            }
            pictureBox1.Refresh();
            Text = $"Iteration {gameLogic.iterations}";
            gameLogic.NextGeneration();
        }

        

        private void StopGeneration() //Остановка генерации клеток
        {
            if (!timer1.Enabled)
                return;

            timer1.Stop();
            numResolution.Enabled = true;
            numDensity.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e) //Таймер для генерации следующих поколений (итераций)
        {
            DrawNextGeneration();
        }

        private void btnStart_Click(object sender, EventArgs e) //Кнопка старт
        {
            StartGenerate();
        }

        private void btnStop_Click(object sender, EventArgs e) //Кнопка стоп
        {
            StopGeneration();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e) //Обработчик события доваления и удаления клеток при движении мышки по игровому полю и нажатии кнопок
        {
            if (!timer1.Enabled)
                return;

            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                gameLogic.AddCell(x, y);
            }

            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                gameLogic.DeleteCell(x, y);
            }
        }

        
    }
}
