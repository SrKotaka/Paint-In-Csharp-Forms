﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;

namespace Grafico
{
   public class ListaSimples<Dado> where Dado : IComparable<Dado>,
   ICriterioDeSeparacao, IRegistro
   {
      NoLista<Dado> primeiro, ultimo, atual, anterior;
      int quantosNos;
      bool primeiroAcessoDoPercurso;

      public ListaSimples()
      {
         primeiro = ultimo = anterior = atual = null;
         quantosNos = 0;
         primeiroAcessoDoPercurso = false;
      }

      public bool EstaVazia
      {
         get => primeiro == null;
      }

      public NoLista<Dado> Primeiro { get => primeiro; }
      public NoLista<Dado> Ultimo { get => ultimo; }
      public NoLista<Dado> Atual { get => atual; }
      public NoLista<Dado> Anterior { get => anterior; }

      public List<Dado> Lista()
      {
         var lista = new List<Dado>();
         atual = primeiro;
         while (atual != null)
         {
            lista.Add(atual.Info);
            atual = atual.Prox;
         }
         return lista;
      }

      public void InserirAntesDoInicio(Dado novoDado)
      {
         var novoNo = new NoLista<Dado>(novoDado);
         if (EstaVazia)          // se a lista está vazia, estamos
            ultimo = novoNo;    // incluindo o 1o e o último nós!

         novoNo.Prox = primeiro;
         primeiro = novoNo;
         quantosNos++;
      }

      public void InserirAposFim(Dado novoDado)
      {
         var novoNo = new NoLista<Dado>(novoDado);
         if (EstaVazia)
            primeiro = novoNo;
         else
            ultimo.Prox = novoNo;

         ultimo = novoNo;
         ultimo.Prox = null;
         quantosNos++;
      }

      public void InserirAposFim(NoLista<Dado> noExistente)
      {
         if (EstaVazia)
            primeiro = noExistente;
         else
            ultimo.Prox = noExistente;

         ultimo = noExistente;
         ultimo.Prox = null;

         quantosNos++;
      }
      public bool ExisteDado(Dado procurado)
      {
         // atual apontará o nó com o Dado procurado, e
         // anterior apontará o nó anterior a este
         anterior = atual = null;
         if (EstaVazia)
            return false;

         // se a lista não está vazia, podemos usar os ponteiros da lista

         // se o campo chave do Dado procurado é menor que o
         // campo chave do primeiro nó, o Dado procurado não existe

         if (procurado.CompareTo(primeiro.Info) < 0)
         {
            atual = primeiro;  // anterior continua com null
            return false;
         }

         // se o campo chave do Dado procurado é maior que o
         // campo chave do último nó, o Dado procurado não existe

         if (procurado.CompareTo(ultimo.Info) > 0)
         {
            anterior = ultimo;  // atual continua com null
            return false;
         }

         // o Dado procurado pode estar na lista, temos que procurar
         // um a um, até encontrá-lo ou achar uma chave maior que a 
         // do procurado

         atual = primeiro;
         bool achou = false;      // fica true quando acha chave igual
         bool maiorOuFim = false; // fica true quando acha chave maior
                                  // ou chegou no fim da lista
         while (!achou && !maiorOuFim)
            if (atual == null)
               maiorOuFim = true;
            else // como atual != null, podemos acessar o nó atual
                 // se atual aponta nó com chave maior que a procurada
                if (atual.Info.CompareTo(procurado) > 0)
               maiorOuFim = true;
            else
                  // testamos se as chaves são iguais
                  if (atual.Info.CompareTo(procurado) == 0)
               achou = true; // encontramos o nó com a chave procurada,
                             // e atual aponta para ele
            else
            {
               anterior = atual;
               atual = atual.Prox;
            }

         return achou;  // anterior e atual definem região do nó
      }
      public void InserirEmOrdem(Dado dados)
      {
         if (EstaVazia) // se a lista está vazia, então o
            InserirAntesDoInicio(dados); // novo nó é o primeiro da lista
         else
           // testa se nova chave < primeira chave
           if (dados.CompareTo(primeiro.Info) < 0)  // novo nó será ligado
            InserirAntesDoInicio(dados);          // antes do primeiro
         else
             // testa se nova chave > última chave
             if (dados.CompareTo(ultimo.Info) > 0)
            InserirAposFim(dados);  // cria nó e o liga no fim da lista
         else
               if (!ExisteDado(dados))  // insere entre os nós anterior 
         {                        // e atual 
            var novo = new NoLista<Dado>(dados, null);
            anterior.Prox = novo; // liga anterior ao novo
            novo.Prox = atual; // e novo no atual
            if (anterior == ultimo) // se incluiu ao final da lista,
               ultimo = novo; // atualiza o apontador ultimo
            quantosNos++;
         }
         else
            throw new Exception("Já existe!");
      }
      public bool Remover(Dado dadoARemover)
      {
         if (ExisteDado(dadoARemover))
         {  // lista não está vazia, temos um 1o e um último
            if (atual == primeiro)  // caso especial
            {
               primeiro = primeiro.Prox;
               atual = primeiro;
               if (primeiro == null)  // se esvaziou a lista!!!!
                  ultimo = null;     // ultimo passa a apontar nada
            }
            else
                if (atual == ultimo)  // caso especial
            {
               ultimo = anterior;
               ultimo.Prox = null;
               atual = ultimo;
            }
            else  // caso geral, nó interno sendo excluído
            {
               anterior.Prox = atual.Prox;
               atual = atual.Prox;
            }

            quantosNos--;
            return true;  // conseguiu excluir
         }

         return false;
      }
      public void Ordenar()
      {
         var listaOrdenada = new ListaSimples<Dado>();
         while (!this.EstaVazia)
         {
            // percorre - se a lista original, não ordenada, e encontra-se
            // o elemento com a menor chave da lista. Deve-se guardar o
            // apontador para o nó anterior e para o nó onde se encontra o
            // menor elemento

            NoLista<Dado> anteriorAoMenor = null;
            NoLista<Dado> oMenor = this.primeiro;
            this.anterior = null;
            this.atual = this.primeiro;
            while (this.atual != null)
            {
               if (atual.Info.CompareTo(oMenor.Info) < 0)
               {
                  anteriorAoMenor = this.anterior;
                  oMenor = this.atual;
               }
               this.anterior = this.atual;
               this.atual = this.atual.Prox;
            }

            // Remove-se o menor elemento da lista original (para isso
            // usam-se os apontadores anterior ao menor e menor)

            if (anteriorAoMenor == null) // estamos removendo o 1o
                                         // da lista original!!
            {
               this.primeiro = this.primeiro.Prox;
            }
            else
            {
               anteriorAoMenor.Prox = oMenor.Prox;
               // cuidado ao remover o último!! tratar disso!
            }
            this.quantosNos--;

            // Inclui-se ao final da lista ordenada o nó recém-
            // retirado da lista original (o menor de todos)

            listaOrdenada.InserirAposFim(oMenor);
         }
         this.primeiro = listaOrdenada.primeiro;
         this.ultimo = listaOrdenada.ultimo;
         this.atual = primeiro;
         this.anterior = null;
         this.quantosNos = listaOrdenada.quantosNos;
      }
      public void GravarArquivo(string nomeArquivo)
      {
         var arquivo = new StreamWriter(nomeArquivo);
         atual = primeiro;
         while (atual != null)
         {
            arquivo.WriteLine(atual.Info.FormatoDeRegistro());
            atual = atual.Prox;
         }
         arquivo.Close();
      }
      public int QuantosNos()
      {
         int contador = 0;
         NoLista<Dado> primeiroAtual = atual;
         atual = primeiro;
         while (atual != null)
         {
            contador++;

            atual = atual.Prox;
         }
         atual = primeiroAtual;
         return contador;
      }
      public int QuantosNos2()
      {
         int contador = 0;
         for (atual = primeiro; atual != null; atual = atual.Prox, contador++) ;
         return contador;
      }
      public void Separar(ref ListaSimples<Dado> l1, ref ListaSimples<Dado> l2)
      {
         l1 = new ListaSimples<Dado>();
         l2 = new ListaSimples<Dado>();
         atual = primeiro;
         while (atual != null)
         {
            var proximoNo = atual.Prox;
            if (atual.Info.PodeSeparar())
               l1.InserirAposFim(atual);
            else
               l2.InserirAposFim(atual);
            atual = proximoNo;
         }
         this.primeiro = this.ultimo = null;
         this.quantosNos = 0;
      }
      public ListaSimples<Dado> Juntar(ListaSimples<Dado> outra)
      {
         var nova = new ListaSimples<Dado>();
         this.atual = this.primeiro;
         outra.atual = outra.primeiro;
         while (this.atual != null && outra.atual != null)
         {
            if (this.atual.Info.CompareTo(outra.atual.Info) < 0)
            {
               var proximoNo = this.atual.Prox;
               nova.InserirAposFim(this.atual);
               this.atual = proximoNo;
            }
            else
                if (outra.atual.Info.CompareTo(this.atual.Info) < 0)
            {
               var proximoNo = outra.atual.Prox;
               nova.InserirAposFim(outra.atual);
               outra.atual = proximoNo;
            }
            else  // empate de chaves
            {
               var proximoNo = outra.atual.Prox;
               nova.InserirAposFim(outra.atual);
               outra.atual = proximoNo;
               // desliga nó da lista this e avança nessa lista
               proximoNo = this.atual.Prox;
               this.atual.Prox = null;
               this.atual = proximoNo;

            }
         }
         // terminar de ligar os nós da lista que não foi percorrida
         // na sua totalidade

         while (this.atual != null)
         {
            var proximoNo = this.atual.Prox;
            nova.InserirAposFim(this.atual);
            this.atual = proximoNo;
         }

         while (outra.atual != null)
         {
            var proximoNo = outra.atual.Prox;
            nova.InserirAposFim(outra.atual);
            outra.atual = proximoNo;
         }

         // garantimos que os ponteiros das duas listas originais
         // fiquem limpos, sem apontar nenhum nó da lista nova
         this.primeiro = this.ultimo = this.atual = this.anterior = null;
         this.quantosNos = 0;
         outra.primeiro = outra.ultimo = outra.atual = outra.anterior = null;
         outra.quantosNos = 0;

         return nova;
      }
      public void Inverter()
      {
         if (!EstaVazia)
         {
            var um = primeiro;
            var dois = primeiro.Prox;
            while (dois != null)
            {
               var tres = dois.Prox;
               dois.Prox = um;
               um = dois;
               dois = tres;
            }
            ultimo = primeiro;
            primeiro.Prox = null;
            primeiro = um;
         }
      }
      public void IniciarPercursoSequencial()
      {
         anterior = null;
         atual = primeiro;
      }
      public void IniciarPercursoSequencial2()
      {
         atual = primeiro;
         this.atual = primeiro;
      }
      public void AvançarUmNo()
      {
         anterior = atual;
         atual = atual.Prox;
      }
      public bool PodePercorrer()
      {
         bool podePerc = false;
         if (atual != ultimo)
         {
            podePerc = true;
            AvançarUmNo();
         }
         return podePerc;
      }
   }
}
