using System;
using System.Collections.Generic;
using System.Drawing;

namespace kursovaya
{
    public class Emitter
    {
        public List<Particle> particles = new List<Particle>();

        public float gravitationX = 0;
        public float gravitationY = 0;

        public int particlesCount = 5;

        public int X; // координата X центра эмиттера, будем ее использовать вместо MousePositionX
        public int Y; // соответствующая координата Y 
        public int Direction = 0; // вектор направления в градусах куда сыпет эмиттер
        public int Spreading = 360; // разброс частиц относительно Direction
        public int Speed = 1; // начальная минимальная скорость движения частицы
        public int RadiusMin = 10; // минимальный радиус частицы
        public int RadiusMax = 30; // максимальный радиус частицы
        public int scatter = 2;
        public int LifeMin = 20; // минимальное время жизни частицы
        public int LifeMax = 100; // максимальное время жизни частицы

        public int ParticlesPerTick = 1;

        public Color ColorFrom = Color.White; // начальный цвет частицы
        public Color ColorTo = Color.FromArgb(0, Color.Black); // конечный цвет частиц

        public void UpdateState()
        {
            int particlesToCreate = ParticlesPerTick;
            foreach (var particle in particles)
            {
                particle.life -= 1;
                if (particle.life <= 0)
                {
                    if (particlesToCreate > 0)
                    {
                        ResetParticle(particle);
                    }
                }
                else
                {
                    particle.x += particle.speedX;
                    particle.y += particle.speedY;

                    particle.speedX += gravitationX;
                    particle.speedY += Speed;
                }      
            }
              
            while (particlesToCreate >= 1)
            {
                particlesToCreate -= 1;
                var particle = CreateParticle();
                ResetParticle(particle);
                particles.Add(particle);
            }
        }

        public void infoPart(Particle particle, Graphics g) // Отображение иформации о частицах
        {
            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            var text = $"X = {(int)particle.x}\n" + $"Y = {(int)particle.y}\n" + $"Жизнь = {particle.life}";
            var font = new Font("Verdana", 10);

            var size = g.MeasureString(text, font);
            Color customColor = Color.FromArgb(150, Color.Green);

            g.DrawEllipse(
               new Pen(Color.Green),
               particle.x - particle.radius,
               particle.y - particle.radius,
               particle.radius * 2,
               particle.radius * 2
           );

            g.FillRectangle(
                new SolidBrush(customColor),
                particle.x,
                particle.y,
                size.Width,
                size.Height
            );

            g.DrawString(
                text,
                font,
                new SolidBrush(Color.White),
                particle.x + 45,
                particle.y + 26,
                stringFormat
            );
        }

        public Particle inPart() // Проверка, нахождения на частице курсора
        {
            foreach (var particle in particles)
            {
                float gX = X - particle.x;
                float gY = Y - particle.y;

                double r = Math.Sqrt(gX * gX + gY * gY); // считаем расстояние от центра точки до центра част
                if (r + particle.radius < particle.radius * 2) // если частица оказалось внутри окружности
                {
                    return particle;
                }
            }
            return null;
        }

        public void Render(Graphics g) // Отображение частиц и их векторов
        {
            foreach (var particle in particles)
            {
                particle.draw(g);
                drawVector(particle, g);
            }
        }

        public void drawVector(Particle particle, Graphics g) // Рисую вектора направления частиц
        {
            var myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
            g.DrawLine(myPen, particle.x  , particle.y, particle.x + particle.speedX, particle.y + particle.radius);
        }

        public virtual void ResetParticle(Particle particle)
        {
            particle.life = Particle.rnd.Next(LifeMin, LifeMax);
            particle.x = X;
            particle.y = Y;

            var direction = Direction + (double)Particle.rnd.Next(Spreading) - Spreading / 2;
            var speed = Particle.rnd.Next(10);

            particle.speedX = (float)(Math.Cos(direction / 180 * Math.PI) * Speed);
            particle.speedY = -(float)(Math.Sin(direction / 180 * Math.PI) * Speed);

            particle.radius = Particle.rnd.Next(RadiusMin, RadiusMax);
        }

        public virtual Particle CreateParticle()
        {
            var particle = new ParticleColorful();
            particle.fromColor = ColorFrom;
            particle.toColor = ColorTo;

            return particle;
        }
    }

    public class TopEmitter : Emitter
    {
        public int width;

        public override void ResetParticle(Particle particle)
        {
            base.ResetParticle(particle);// вызываем базовый сброс частицы, там жизнь переопределяется

            particle.x = Particle.rnd.Next(width);// позиция X -- произвольная точка от 0 до Width
            particle.y = 0;// ноль -- это верх экрана

            particle.speedY = Speed;// падаем вниз по умолчанию
            particle.speedX = Particle.rnd.Next(-scatter, scatter);// разброс влево и вправа у частиц

        }
    }
}
