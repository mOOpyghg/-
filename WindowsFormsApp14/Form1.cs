using System;
using System.IO;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace WindowsFormsApp14
{
    public partial class Form1 : Form
    {
        private readonly string saveFilePath = "saved_data.txt";
        private int score = 0;
        private int itemX;
        private int itemY;
        private int plateX;
        private const int plateWidth = 75; 
        private const int plateHeight = 20;
        private Random random = new Random();
        private Image cupcakeImage;
        private Image heartImage;
        private Image plateImage;
        private int lives = 3; 
        private const string FileName = "data.txt";
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            LoadCupcakeImage();
            LoadHeartImage(); 
            LoadPlateImage(); 
            StartNewGame();
        }

        private void LoadCupcakeImage()
        {
            try
            {
                cupcakeImage = Image.FromFile(@"C:\Mac\Home\Desktop\флеш 2.0\Игра\WindowsFormsApp14\icons8-череп-64.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки изображения: " + ex.Message);
                this.Close();
            }
        }

        private void LoadHeartImage()
        {
            try
            {
                heartImage = Image.FromFile(@"C:\Mac\Home\Desktop\флеш 2.0\Игра\WindowsFormsApp14\icons8-сердце-60.png"); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки изображения сердца: " + ex.Message);
                this.Close(); 
            }
        }

        private void LoadPlateImage() 
        {
            try
            {
                plateImage = Image.FromFile(@"C:\Mac\Home\Desktop\флеш 2.0\Игра\WindowsFormsApp14\icons8-мусорный-контейнер-64.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки изображения: " + ex.Message);
                this.Close(); 
            }
        }

        private void StartNewGame()
        {
            score = 0;
            plateX = (this.ClientSize.Width - plateWidth) / 2; 
            itemX = random.Next(0, this.ClientSize.Width - cupcakeImage.Width);
            itemY = 0;
            timer1.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(saveFilePath))
            {
                
                string savedNumber = File.ReadAllText(saveFilePath);
                label1.Text = savedNumber;
            }
            else
            {
                label1.Text = "Файл отсутствует";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            itemY += 15;
            if (itemY > this.ClientSize.Height)
            {
                itemY = 0;
                itemX = random.Next(0, this.ClientSize.Width - cupcakeImage.Width);

               
                lives--;
                if (lives <= 0)
                {
                    timer1.Stop();
                    MessageBox.Show("Игра окончена! Ваш счет: " + score);
                    this.Close(); 
                }
            }

            
            if (itemY + cupcakeImage.Height >= this.ClientSize.Height - plateHeight - 30 && 
                                itemX + cupcakeImage.Width > plateX && itemX < plateX + plateWidth)
            {
                score++;
                itemY = 0;
                itemX = random.Next(0, this.ClientSize.Width - cupcakeImage.Width);
            }

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(plateImage, plateX, this.ClientSize.Height - plateHeight - 90);
            e.Graphics.DrawImage(cupcakeImage, itemX, itemY); 

            
            for (int i = 0; i < lives; i++)
            {
                e.Graphics.DrawImage(heartImage, this.ClientSize.Width - (i + 1) * heartImage.Width - 10, 10); 
            }

            lblScore.Text = "Счет: " + score;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Left && plateX > 0)
            {
                plateX -= 20;
            }
            else if (e.KeyCode == Keys.Right && plateX < this.ClientSize.Width - plateWidth) 
            {
                plateX += 20; 
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(saveFilePath, lblScore.Text);
        }
    }
}

