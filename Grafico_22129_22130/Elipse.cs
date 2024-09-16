using System.Drawing;

namespace Grafico
{
   internal class Elipse : Ponto
   {
      private int largura, altura;
      public int Largura
      {
         get { return largura; }
         set { largura = value; }
      }
      public int Height
      {
         get { return altura; }
         set { altura = value; }
      }
      public void SetWidth(int novaLargura) { largura = novaLargura; }
      public void SetHeight(int novaAltura) { altura = novaAltura; }
      public Elipse(int supEsqX, int supEsqY, int largura, int altura, Color cor) : // Construtor para a classe Elipse
      base(supEsqX, supEsqY, cor)
      {
         this.largura = largura;
         this.altura = altura;
      }
      public override void Desenhar(Color cor, Graphics g)
      {
         Pen pen = new Pen(cor);
         g.DrawEllipse(pen, X - largura, Y - altura, 2 * largura, 2 * altura);
      }
      public override string ToString()
      {
         return TransformaString("e", 5) +
                TransformaString(X, 5) +
                TransformaString(Y, 5) +
                TransformaString(Cor.R, 5) +
                TransformaString(Cor.G, 5) +
                TransformaString(Cor.B, 5) +
                TransformaString(Largura, 5) +
                TransformaString(Height, 5);
      }
   }
}