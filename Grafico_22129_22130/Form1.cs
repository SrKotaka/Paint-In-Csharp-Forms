using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Grafico
{
   public partial class frmGrafico : Form
   {
      bool esperaPonto = false, esperaInicioReta = false, esperaFimReta = false,
           esperaCentroCirculo = false, esperaRaioCirculo = false,
           esperaCentroElipse = false, esperaRaiosElipse = false,
           esperaCentroRetangulo = false, esperaXYRetangulo = false,
           esperaPontoPolilinha = false; //DECLARA FALSO EM TODAS AS CLASSES QUE VAMOS USAR

      int numLinhas = 0; //DECLARA NUMERO DE LINHAS OS EIXO X E Y E TODOS RECEBEM 0
      private ListaSimples<Ponto> figuras = new ListaSimples<Ponto>();//DECLARA FIGURAS RECEBEM LISTA SIMPLES
      private ListaSimples<Ponto> pontos = new ListaSimples<Ponto>();//DECLARA PONTOS RECEBEM LISTA SIMPLES
      Color corAtual = Color.Red; //COR INICIAL É VERMELHA ATÉ O USUARIO SELECIONAR ALGUMA NO BOTÃO COR
      private static Ponto p1 = new Ponto(0, 0, Color.Black);//DECLARA P1 QUE RECEBE UM PONTO INICIAL COM O VALOR X = 0 E Y = 0 E A COR PRETA


      public frmGrafico()
      {
         InitializeComponent();
      }

      private void LimparEsperas()
      {
         esperaPonto = false;
         esperaInicioReta = false;
         esperaFimReta = false;
         esperaCentroCirculo = false;
         esperaRaioCirculo = false;
         esperaCentroElipse = false;
         esperaRaiosElipse = false;
         esperaCentroRetangulo = false;
         esperaXYRetangulo = false;
      }

      private void LimparFiguras()//METODO PARA LIMPAR FIGURAS 
      {
         figuras = new ListaSimples<Ponto>();//FIGURAS RECEBE LISTA SIMPLES 
         numLinhas = 0;//NUMERO DE LINHAS RECEBE 0
         p1 = new Ponto(0, 0, Color.Black);//PONTO RECEBE UM PONTO INICIAL COM O VALOR X = 0 E Y = 0 E A COR PRETA
      }

      private void btnSalvar_Click(object sender, EventArgs e)//DECLARA O BOTÃO SALVAR
      {
         if (dlgSalvar.ShowDialog() == DialogResult.OK)
         {
            StreamWriter arquivo = new StreamWriter(dlgSalvar.FileName);
            NoLista<Ponto> aux = figuras.Primeiro;
            while (aux != null)
            {
               arquivo.WriteLine(aux.Info.ToString());
               aux = aux.Prox;
            }
            arquivo.Close();
         }
      }

      private void btnCor_Click(object sender, EventArgs e)//DECLARA O BOTÃO COR
      {
         ColorDialog clrDialog = new ColorDialog();//FAZ O clrDialog RECEBER O POP-UP DE ESCOLHA DE COR PARA O USUARIO

         if (clrDialog.ShowDialog() == DialogResult.OK)//SE clrDiaglog FOR FAVORAVEL
         {
            btnCor.BackColor = clrDialog.Color;//AO CLICAR NO btnCor A COR INSERIDA PELO O USUARIO MUDARA O BACKGROUND DO BOTÃO PARA SABER QUAL COR SELECIONOU
            corAtual = clrDialog.Color;//COR ATUAL RECEBE OQUE O USUARIO ESCOLHER NO POP-UP QUE APARECEU
         }
      }

      private void btnApagar_Click(object sender, EventArgs e)
      {
         LimparFiguras();//LIMPA AS ESFERAS
         Refresh();//ATUALIZA A PAGINA
      }

      private void btnSair_Click(object sender, EventArgs e)//DECLARA O BOTAO SAIR
      {
         Close();
      }

      private void btnAbrir_Click(object sender, EventArgs e)
      {
         LimparFiguras();
         if (dlgAbrir.ShowDialog() == DialogResult.OK)
         {
            try
            {
               StreamReader arqFiguras = new StreamReader(dlgAbrir.FileName);
               string linha;
               while ((linha = arqFiguras.ReadLine()) != null)
               {
                  string tipo = linha.Substring(0, 5).Trim();
                  int xBase = Convert.ToInt32(linha.Substring(5, 5).Trim());
                  int yBase = Convert.ToInt32(linha.Substring(10, 5).Trim());
                  int corR = Convert.ToInt32(linha.Substring(15, 5).Trim());
                  int corG = Convert.ToInt32(linha.Substring(20, 5).Trim());
                  int corB = Convert.ToInt32(linha.Substring(25, 5).Trim());
                  Color cor = new Color();
                  cor = Color.FromArgb(255, corR, corG, corB);
                  switch (tipo[0])
                  {
                     case 'p': // figura é um ponto
                        figuras.InserirAposFim(
                        new NoLista<Ponto>(new Ponto(xBase, yBase, cor), null));
                        break;
                     case 'l': // figura é uma reta
                        int xFinal = Convert.ToInt32(linha.Substring(30, 5).Trim());
                        int yFinal = Convert.ToInt32(linha.Substring(35, 5).Trim());
                        figuras.InserirAposFim(new NoLista<Ponto>(
                        new Reta(xBase, yBase, xFinal, yFinal, cor), null));
                        break;
                     case 'c': // figura é um círculo
                        int raio = Convert.ToInt32(linha.Substring(30, 5).Trim());
                        figuras.InserirAposFim(new NoLista<Ponto>(
                        new Circulo(xBase, yBase, raio, cor), null));
                        break;
                     case 'e': // figura é uma elipse
                        int raioX = Convert.ToInt32(linha.Substring(30, 5).Trim());
                        int raioY = Convert.ToInt32(linha.Substring(35, 5).Trim());
                        figuras.InserirAposFim(new NoLista<Ponto>(new Elipse(xBase, yBase, raioX, raioY, cor), null));
                        break;
                     case 'r': // figura é um retângulo
                        int xFinalRet = Convert.ToInt32(linha.Substring(30, 5).Trim());
                        int yFinalRet = Convert.ToInt32(linha.Substring(35, 5).Trim());
                        figuras.InserirAposFim(new NoLista<Ponto>(new Retangulo(xBase, yBase, xFinalRet, yFinalRet, cor), null));
                        break;
                     case 'n': //figura é uma polilinha
                        pontos.InserirAposFim(new NoLista<Ponto>(new Ponto(xBase, yBase, cor)));
                        for (int i = 30; i < linha.Length; i += 10)
                        {
                           int xPonto = Convert.ToInt32(linha.Substring(i, 5));
                           int yPonto = Convert.ToInt32(linha.Substring(i + 5, 5));
                           pontos.InserirAposFim(new NoLista<Ponto>(new Ponto(xPonto, yPonto, cor), null));
                        }
                        figuras.InserirAposFim(new NoLista<Ponto>(new Polilinha(pontos, cor), null));
                        break;
                  }
               }
               arqFiguras.Close();
               Text = dlgAbrir.FileName;
               pbAreaDesenho.Invalidate();
            }
            catch (IOException)
            {
               MessageBox.Show("Erro na leitura do arquivo");
            }
            catch (Exception)
            {
               MessageBox.Show("Erro desconhecido");
            }
         }
      }

      private void pbAreaDesenho_Paint(object sender, PaintEventArgs e)
      {
         Graphics g = e.Graphics;

         figuras.IniciarPercursoSequencial();
         if (figuras.QuantosNos() == 1)
         {
            Ponto figura = figuras.Primeiro.Info;
            figura.Desenhar(figura.Cor, g);
         }
         else
         {
            while (figuras.PodePercorrer())
            {
               Ponto figuraAtual = figuras.Atual.Info;
               figuraAtual.Desenhar(figuraAtual.Cor, g);
            }
         }
      }

      private void pbAreaDesenho_MouseMove(object sender, MouseEventArgs e)//DECLARA OS EIXOS PARA O USUARIO EM BAIXO DO FORMS
      {
         stMensagem.Items[3].Text = e.X + "," + e.Y;//CONFORME O USUARIO MOVE O MOUSE AS COORDENADAS MUDAM DE ACORDO COM SUA POSIÇÃO
      }

      private void btnElipse_Click(object sender, EventArgs e)//DECLARA O BOTÃO ELIPSE
      {
         stMensagem.Items[1].Text = "Clique no local do canto esquerdo superior do elipse";//EXIBE PARA O USUARIO A MENSAGEM DE COMO UTILIZAR O ELIPSE
         LimparEsperas();//LIMPA AS VARIAVEIS DE ESPERA
         esperaCentroElipse = true;//esperaCentroElipse RECEBE TRUE
      }

      private void btnRetangulo_Click(object sender, EventArgs e)//DECLARA O BOTÃO RETANGULO
      {
         stMensagem.Items[1].Text = "Clique no local do centro do retângulo";//EXIBE PARA O USUARIO A MENSAGEM DE COMO UTILIZAR O RETANGULO
         LimparEsperas();//LIMPA AS VARIAVEIS DE ESPERA
         esperaCentroRetangulo = true;//esperaCentroRetangulo RECEBE TRUE
      }

      private void btnPonto_Click(object sender, EventArgs e)//DECLARA BOTÃO PONTO
      {
         stMensagem.Items[1].Text = "Clique no local do ponto desejado:";//EXIBE PARA O USUARIO A MENSAGEM DE COMO UTILIZAR O PONTO
         LimparEsperas();//LIMPA AS VARIAVEIS DE ESPERA
         esperaPonto = true;//esperaPonto RECEBE TRUE
      }

      private void pbAreaDesenho_MouseClick(object sender, MouseEventArgs e)
      {
         if (esperaPonto)
         {
            Ponto novoPonto = new Ponto(e.X, e.Y, corAtual);
            figuras.InserirAposFim(new NoLista<Ponto>(novoPonto, null));
            novoPonto.Desenhar(novoPonto.Cor, pbAreaDesenho.CreateGraphics());
            esperaPonto = false;
            stMensagem.Items[1].Text = "";
         }
         else if (esperaInicioReta)
         {
            p1.Cor = corAtual;
            p1.X = e.X;
            p1.Y = e.Y;
            esperaInicioReta = false;
            esperaFimReta = true;
            stMensagem.Text = "Clique no ponto final da reta";
         }
         else if (esperaFimReta)
         {
            esperaInicioReta = false;
            esperaFimReta = false;
            Reta novaLinha = new Reta(p1.X, p1.Y, e.X, e.Y, corAtual);
            figuras.InserirAposFim(new NoLista<Ponto>(novaLinha, null));
            novaLinha.Desenhar(novaLinha.Cor, pbAreaDesenho.CreateGraphics());
         }
         else if (esperaCentroCirculo)
         {
            p1.Cor = corAtual;
            p1.X = e.X;
            p1.Y = e.Y;
            esperaCentroCirculo = false;
            esperaRaioCirculo = true;
            stMensagem.Text = "Clique no ponto limite do círculo";
         }
         else if (esperaRaioCirculo)
         {
            esperaCentroCirculo = false;
            esperaRaioCirculo = false;
            int raio;
            if (e.X - p1.X > e.Y - p1.Y)
               raio = Math.Abs(e.X - p1.X);
            else
               raio = Math.Abs(e.Y - p1.Y);
            Circulo novoCirculo = new Circulo(p1.X, p1.Y, raio, corAtual);
            figuras.InserirAposFim(new NoLista<Ponto>(novoCirculo, null));
            novoCirculo.Desenhar(novoCirculo.Cor, pbAreaDesenho.CreateGraphics());
         }
         else if (esperaCentroElipse)
         {
            p1.Cor = corAtual;
            p1.X = e.X;
            p1.Y = e.Y;
            esperaCentroElipse = false;
            esperaRaiosElipse = true;
            stMensagem.Text = "Clique na distância do centro da elipse";
         }
         else if (esperaRaiosElipse)
         {
            esperaRaiosElipse = false;
            int larguraElipse = Math.Abs(e.X - p1.X);
            int alturaElipse = Math.Abs(e.Y - p1.Y);
            Elipse novaElipse = new Elipse(p1.X, p1.Y, larguraElipse, alturaElipse, corAtual);
            figuras.InserirAposFim(new NoLista<Ponto>(novaElipse, null));
            novaElipse.Desenhar(novaElipse.Cor, pbAreaDesenho.CreateGraphics());
         }
         else if (esperaCentroRetangulo)
         {
            p1.Cor = corAtual;
            p1.X = e.X;
            p1.Y = e.Y;
            esperaCentroRetangulo = false;
            esperaXYRetangulo = true;
            stMensagem.Items[1].Text = "Clique na distancia do centro do retângulo";
         }
         else if (esperaXYRetangulo)
         {
            esperaXYRetangulo = false;
            int larguraRetangulo = Math.Abs(e.X - p1.X);
            int alturaRetangulo = Math.Abs(e.Y - p1.Y);
            Retangulo novoRetangulo = new Retangulo(p1.X, p1.Y, larguraRetangulo, alturaRetangulo, corAtual);
            figuras.InserirAposFim(new NoLista<Ponto>(novoRetangulo, null));
            novoRetangulo.Desenhar(novoRetangulo.Cor, pbAreaDesenho.CreateGraphics());
         }
         else if (esperaPontoPolilinha)
         {
            pontos.InserirAposFim(new Ponto(e.X, e.Y, corAtual));
            if (pontos.QuantosNos() > 1)
            {
               pontos.IniciarPercursoSequencial();
               while (pontos.PodePercorrer())
               {
                  Reta novaReta = new Reta(pontos.Anterior.Info.X, pontos.Anterior.Info.Y, pontos.Atual.Info.X, pontos.Atual.Info.Y, corAtual);
                  novaReta.Desenhar(novaReta.Cor, pbAreaDesenho.CreateGraphics());
               }
            }
            numLinhas++;
            stMensagem.Items[1].Text = $"Clique no {numLinhas}° ponto da polilinha";
         }
      }

      private void pbAreaDesenho_MouseDoubleClick(object sender, MouseEventArgs e)
      {
         if (esperaPontoPolilinha)
         {
            esperaPontoPolilinha = false;
            if (pontos.Anterior.Info.X == pontos.Ultimo.Info.X || pontos.Anterior.Info.Y == pontos.Ultimo.Info.Y)
            {
               pontos.Remover(pontos.Ultimo.Info);
            }
            else
               pontos.InserirAposFim(new Ponto(e.X, e.Y, corAtual));
            Polilinha novaPolilinha = new Polilinha(pontos, corAtual);
            figuras.InserirAposFim(new NoLista<Ponto>(novaPolilinha, null));
            novaPolilinha.Desenhar(novaPolilinha.Cor, pbAreaDesenho.CreateGraphics());
         }
      }

      private void btnReta_Click(object sender, EventArgs e)//DECLARA O BOTAO RETA
      {
         stMensagem.Items[1].Text = "Clique no ponto inicial da reta:";//EXIBE PARA O USUARIO A MENSAGEM DE COMO UTILIZAR A RETA
         LimparEsperas();//LIMPA AS VARIAVEIS DE ESPERA
         esperaInicioReta = true;//esperaInicioReta RECEBE TRUE
      }

      private void btnCirculo_Click(object sender, EventArgs e)//DECLARA O BOTAO CIRCULO
      {
         stMensagem.Items[1].Text = "Clique no centro do círculo";//EXIBE PARA O USUARIO A MENSAGEM DE COMO UTILIZAR O CIRCULO
         LimparEsperas();//LIMPA AS VARIAVEIS DE ESPERA
         esperaCentroCirculo = true;//esperaCentroCirculo RECEBE TRUE
      }

      private void btnPolilinha_Click(object sender, EventArgs e)//DECLARA O BOTÃO POLILINHA
      {
         pontos = new ListaSimples<Ponto>();//CRIA UMA NOVA LISTA DE PONTOS
         stMensagem.Items[1].Text = $"Clique no 1° ponto da polilinha";
         numLinhas++;
         LimparEsperas();//CHAMA O METODO LIMPAR ESFERAS
         esperaPontoPolilinha = true;//INICIA UMA NOVA POLILINHA
      }
   }
}
