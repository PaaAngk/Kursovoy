using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kursovaya
{
    public partial class Form1 : Form
    {
        Emitter emitter;
        Particle particle;

        private int count = 0;
        private Boolean run = true;


        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

            emitter = new TopEmitter
            {
                width = picDisplay.Width,
                gravitationY = 0.25f
            };
        }

        private void timer1_Tick(object sender, EventArgs e)
        {        
            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                // Счетчик счетает до значения трек бара, замедляя анимацию и если run = 0, то программа остановлена
                if((count >= trackBar1.Value) && (run)) {
                    emitter.UpdateState();
                    count = 0;
                }

                g.Clear(Color.White);
                emitter.Render(g);

                particle = emitter.inPart();
                if (particle != null)
                {
                    emitter.infoPart(particle, g);
                }
            }

            count++;
            picDisplay.Invalidate();
        }

        private void picDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            emitter.X = e.X;
            emitter.Y = e.Y;

        }

        private void buttonStart_Click(object sender, EventArgs e) //Запуск анимации
        {
            run = true;
        }

        private void buttonStop_Click(object sender, EventArgs e) // Остановка анимации
        {
            run = false;
        }

        private void buttonStep_Click(object sender, EventArgs e) // Шаг анимации  на 1 тик
        {
            run = true;
            count = trackBar1.Value;
            timer1_Tick(sender, e);
            run = false;
        }

        private void trackBar2_Scroll(object sender, EventArgs e) // Количество генерируемых частиц
        {
            emitter.ParticlesPerTick = trackBar2.Value;
        }

        private void trackBar3_Scroll(object sender, EventArgs e) // Разброс частиц
        {
            emitter.scatter = trackBar3.Value;
        }
    }
}

/*emitter.impactPoints.Add(new GravityPoint
{
    X = (float)(picDisplay.Width * 0.25),
    Y = picDisplay.Height / 2
});

emitter.impactPoints.Add(new AntiGravityPoint
{
    X = picDisplay.Width / 2,
    Y = picDisplay.Height / 2
});

emitter.impactPoints.Add(new GravityPoint
{
    X = (float)(picDisplay.Width * 0.75),
    Y = picDisplay.Height / 2
});*/