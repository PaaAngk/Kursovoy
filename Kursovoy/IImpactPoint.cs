using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovaya
{
    public abstract class IImpactPoint
    {
        public float X;
        public float Y;

        public abstract void impactParticle(Particle particle);

        public void Render(Graphics g)
        {
            g.FillEllipse(
                new SolidBrush(Color.Red),
                X - 5,
                Y - 5,
                10,
                10
            );
        }
    }

    public class GravityPoint : IImpactPoint
    {
        public int power = 100;

        public override void impactParticle(Particle particle)
        {
            float gX = X - particle.x;
            float gY = Y - particle.y;
            float r2 = (float)Math.Max(100, gX * gX + gY * gY);

            particle.speedX += gX * power / r2;
            particle.speedY += gY * power / r2;
        }

    }
    public class AntiGravityPoint : IImpactPoint
    {
        public int power = 100;

        public override void impactParticle(Particle particle)
        {
            float gX = X - particle.x;
            float gY = Y - particle.y;
            float r2 = (float)Math.Max(100, gX * gX + gY * gY);

            particle.speedX -= gX * power / r2;
            particle.speedY -= gY * power / r2;
        }
    }
}
