using System;
using System.Drawing;
using System.Runtime.ConstrainedExecution;

namespace Grafico
{
   public class Ponto : IComparable<Ponto>, IRegistro, ICriterioDeSeparacao
   {
      private int y, x;
      private Color cor;
      public Ponto(int cX, int cY, Color qualCor)
      {
         x = cX;
         y = cY;
         cor = qualCor;
      }
      public int X
      {
         get { return x; }
         set { x = value; }
      }
      public int Y
      {
         get { return y; }
         set { y = value; }
      }
      public Color Cor
      {
         get { return cor; }
         set { cor = value; }
      }

      public int CompareTo(Ponto other)
      {
         int diferencaX = X - other.X;
         if (diferencaX == 0)
            return Y - other.Y;
         return diferencaX;
      }

      public int CompareTo(object obj)
      {
         throw new NotImplementedException();
      }

      public virtual void Desenhar(Color cor, Graphics g)
      {
         Pen pen = new Pen(cor);
         g.DrawLine(pen, x, y, x, y);

      }

      public string FormatoDeRegistro()
      {
         throw new NotImplementedException();
      }

      public bool PodeSeparar()
      {
         throw new NotImplementedException();
      }
      public string TransformaString(int valor, int quantasPosicoes)
      {
         string cadeia = valor + "";
         while (cadeia.Length < quantasPosicoes)
            cadeia = " " + cadeia;
         return cadeia.Substring(0, quantasPosicoes); // corta, se necessário, para
                                                      // tamanho máximo
      }
      public string TransformaString(string valor, int quantasPosicoes)
      {
         string cadeia = "" + valor;
         while (cadeia.Length < quantasPosicoes)
            cadeia = " " + cadeia;
         return cadeia.Substring(0, quantasPosicoes); // corta, se necessário, para
                                                      // tamanho máximo
      }
      public override string ToString()
      {
         return TransformaString("p", 5) +
                TransformaString(X, 5) +
                TransformaString(Y, 5) +
                TransformaString(Cor.R, 5) +
                TransformaString(Cor.G, 5) +
                TransformaString(Cor.B, 5);
      }
   }
}
