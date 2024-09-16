using System.Drawing;

namespace Grafico
{
   class Reta : Ponto
   {
      private Ponto pontoFinal;

      public Reta(int x1, int y1, int x2, int y2, Color novaCor) : // Construtor para a classe Reta
      base(x1, y1, novaCor)
      {
         pontoFinal = new Ponto(x2, y2, novaCor);
      }
      public override void Desenhar(Color cor, Graphics g)
      {
         Pen pen = new Pen(cor);
         g.DrawLine(pen, X, Y, pontoFinal.X, pontoFinal.Y);
      }
      public override string ToString()
      {
         return TransformaString("l", 5) +
                TransformaString(X.ToString(), 5) +
                TransformaString(Y.ToString(), 5) +
                TransformaString(Cor.R, 5) +
                TransformaString(Cor.G, 5) +
                TransformaString(Cor.B, 5) +
                TransformaString(pontoFinal.X, 5) +
                TransformaString(pontoFinal.Y, 5);
      }
   }
}
