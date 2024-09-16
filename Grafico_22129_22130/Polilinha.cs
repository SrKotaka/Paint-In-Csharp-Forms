using System.Drawing;

namespace Grafico
{
   internal class Polilinha : Ponto
   {
      private ListaSimples<Ponto> pontos = new ListaSimples<Ponto>();

      public ListaSimples<Ponto> Pontos
      {
         get { return pontos; }
         set 
         {
            pontos.IniciarPercursoSequencial();
            while (pontos.PodePercorrer())
            {
               Pontos.InserirAposFim(pontos.Atual.Info);
            }
         }
      }
      public Polilinha(ListaSimples<Ponto> pontos, Color cor) : // Construtor para a classe Polilinha
      base(pontos.Primeiro.Info.X, pontos.Primeiro.Info.Y, cor)
      {
         this.pontos = pontos;
      }
      public override void Desenhar(Color cor, Graphics g)
      {
         
         Pen pen = new Pen(cor);
         pontos.IniciarPercursoSequencial();
         while (pontos.PodePercorrer()) // Enquanto o nó atual não é o último
         {
            g.DrawLine(pen, pontos.Anterior.Info.X, pontos.Anterior.Info.Y, // desenha linhas com o ponto anterior como início
                            pontos.Atual.Info.X, pontos.Atual.Info.Y);      // e o ponto atual como fim
         }
         g.DrawLine(pen, pontos.Atual.Info.X, pontos.Atual.Info.Y,        // Conecta as duas extremidades da polilinha
                         pontos.Primeiro.Info.X, pontos.Primeiro.Info.Y); 
      }
      public override string ToString()
      {
         this.pontos.IniciarPercursoSequencial();
         string pontos = "";
         while (this.pontos.PodePercorrer()) // percorre os pontos do segundo ao último
         {
                pontos += TransformaString(this.pontos.Atual.Info.X, 5);
            pontos += TransformaString(this.pontos.Atual.Info.Y, 5);
         }
         return TransformaString("n", 5) +
                TransformaString(X, 5) +
                TransformaString(Y, 5) +
                TransformaString(Cor.R, 5) +
                TransformaString(Cor.G, 5) +
                TransformaString(Cor.B, 5) +
                pontos;
      }
   }
}
