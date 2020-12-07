using System;
using System.Collections.Generic;
using System.Drawing;

namespace kursovaya
{
    public class Emitter
    {
        List<Particle> particles = new List<Particle>();
        public int mousePositionX = 0;
        public int mousePositionY = 0;

        public List<IImpactPoint> impactPoints = new List<IImpactPoint>();
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
        public int LifeMin = 20; // минимальное время жизни частицы
        public int LifeMax = 100; // максимальное время жизни частицы

        public int ParticlesPerTick = 1;

        public Color ColorFrom = Color.White; // начальный цвет частицы
        public Color ColorTo = Color.FromArgb(0, Color.Black); // конечный цвет частиц

        public void UpdateState(int gravitation)
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

        public void Render(Graphics g)
        {
            foreach (var particle in particles)
            {
                particle.draw(g);
            }

            foreach (var point in impactPoints)
            {
                point.Render(g);
            }
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

            particle.speedY = 1;// падаем вниз по умолчанию
            particle.speedX = Particle.rnd.Next(-2, 2);// разброс влево и вправа у частиц
        }
    }
}
