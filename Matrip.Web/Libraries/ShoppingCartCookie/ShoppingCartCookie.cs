﻿using Matrip.Domain.Models.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Matrip.Web.Libraries.ShoppingCartCookie
{
    public class ShoppingCartCookie
    {
        private readonly string Key = "Carrinho.Compras";
        private readonly Cookie.Cookie _cookie;

        public ShoppingCartCookie(Cookie.Cookie cookie)
        {
            _cookie = cookie;
        }

        /*
         * CRUD - Cadastrar, Read, Update, Delete
         * Adicionar Item, Remover Item, Alterar Quantidade
         */
        public void Salvar(List<ma18tripitemshoppingcart> Lista)
        {
            string Valor = JsonConvert.SerializeObject(Lista);
            _cookie.Add(Key, Valor);
        }
        public void Remover()
        {
            _cookie.Remove(Key);
        }
        public List<ma18tripitemshoppingcart> Get()
        {
            if (_cookie.Exists(Key))
            {
                string valor = _cookie.Consult(Key);
                return JsonConvert.DeserializeObject<List<ma18tripitemshoppingcart>>(valor);
            }
            else
            {
                return null;
            }
        }
        /*
      public void Add(ViewTripsInShoppingCart ViewTripsInShoppingCart)
      {
          List<Passeios> Lista;
          if (_cookie.Existe(Key))
          {
              Lista = Consultar();
              var ItemLocalizado = Lista.SingleOrDefault(a => a.Id == item.Id);

              if (ItemLocalizado == null)
              {
                  Lista.Add(item);
              }
              else
              {
                  ItemLocalizado.UnidadesPedidas = ItemLocalizado.UnidadesPedidas + 1;
              }
          }
          else
          {
              Lista = new List<ProdutoItem>();
              Lista.Add(item);
          }

          Salvar(Lista);
      }
      public void Atualizar(ProdutoItem item)
      {
          var Lista = Consultar();
          var ItemLocalizado = Lista.SingleOrDefault(a => a.Id == item.Id);

          if (ItemLocalizado != null)
          {
              ItemLocalizado.UnidadesPedidas = item.UnidadesPedidas;
              Salvar(Lista);
          }
      }
      public void Remover(ProdutoItem item)
      {
          var Lista = Consultar();
          var ItemLocalizado = Lista.SingleOrDefault(a => a.Id == item.Id);

          if (ItemLocalizado != null)
          {
              Lista.Remove(ItemLocalizado);
              Salvar(Lista);
          }
      }
      public List<ProdutoItem> Consultar()
      {
          if (_cookie.Existe(Key))
          {
              string valor = _cookie.Consultar(Key);
              return JsonConvert.DeserializeObject<List<ProdutoItem>>(valor);
          }
          else
          {
              return new List<ProdutoItem>();
          }
      }
      public void Salvar(List<ProdutoItem> Lista)
      {
          string Valor = JsonConvert.SerializeObject(Lista);
          _cookie.Cadastrar(Key, Valor);
      }

      public bool Existe(string Key)
      {
          if (_cookie.Existe(Key))
          {
              return false;
          }

          return true;
      }
      public void RemoverTodos()
      {
          _cookie.Remover(Key);
      }*/
    }
}
