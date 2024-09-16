using System.Drawing;

namespace Grafico
{
   internal class Circulo : Ponto
   {
      int raio;
      public int Raio
      {
         get { return raio; }
         set { raio = value; }
      }
      public Circulo(int xCentro, int yCentro, int novoRaio, Color novaCor) : // Construtor para a classe Circulo
      base(xCentro, yCentro, novaCor)
      {
         raio = novoRaio;
      }
      public void SetRaio(int novoRaio)
      {
         raio = novoRaio;
      }
      public override void Desenhar(Color corDesenho, Graphics g)
      {
         Pen pen = new Pen(corDesenho);
         g.DrawEllipse(pen, base.X - raio, base.Y - raio, // centro - raio
         2 * raio, 2 * raio); // centro + raio
      }
      public override string ToString()
      {
         return TransformaString("c", 5) +
                TransformaString(X, 5) +
                TransformaString(Y, 5) +
                TransformaString(Cor.R, 5) +
                TransformaString(Cor.G, 5) +
                TransformaString(Cor.B, 5) +
                TransformaString(Raio, 5);
      }
   }
}
