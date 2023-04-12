using System.Drawing;
using System.Windows.Forms;

namespace Grafico
{
   internal class Retangulo : Ponto
   {
      private int largura, altura;
      public Retangulo(int xSupEsq, int ySupEsq, int novaLargura, int novaAltura, Color novaCor) : // Construtor para a classe Retangulo
      base(xSupEsq, ySupEsq, novaCor)
      {
         largura = novaLargura;
         altura = novaAltura;
      }

      public int Largura
      {
         get { return largura; }
         set { largura = value; }
      }
      public int Altura
      {
         get { return altura; }
         set { altura = value; }
      }
      public void SetLargura(int novaLargura) { largura = novaLargura; }
      public void SetAltura(int novaAltura) { altura = novaAltura; }
      public override void Desenhar(Color cor, Graphics g)
      {
         Pen pen = new Pen(cor);
         g.DrawRectangle(pen, X - largura, Y - altura, 2 * largura, 2 * altura);
      }
      public override string ToString()
      {
         return TransformaString("r", 5) +
                TransformaString(X, 5) +
                TransformaString(Y, 5) +
                TransformaString(Cor.R, 5) +
                TransformaString(Cor.G, 5) +
                TransformaString(Cor.B, 5) +
                TransformaString(Altura, 5) +
                TransformaString(Altura, 5);
      }
   }
}
