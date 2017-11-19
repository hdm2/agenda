using System;
using Amon.Persistencia;
using Amon.PontoE.Modelo.Ponto;
using System.Collections.Generic;
using System.Linq;

namespace Amon.PontoE.Persistencia.Ponto
{
   public class TipoAutorizacaoDAO : SimplesAbstractDAO<TipoAutorizacao>
    {
        protected override string obterTabela()
        {
            return "TipoAutorizacao";
        }

        protected override string obterCampoAutoIncrementado()
        {
            return "Id";
        }

        public IList<TipoAutorizacao> obterTiposVisiveis(String idLotacao)
        {
            IList<TipoAutorizacao> todos = obterTodos();

            //Remove os espaçoes em branco, caso haja
            todos.ToList().ForEach(a => a.VisivelPara = a.VisivelPara.Replace(" ", ""));

            IList<TipoAutorizacao> listaTiposVisiveis = todos.Where(t => t.VisivelPara == "*").ToList();

            foreach (TipoAutorizacao tipo in todos.Where(t => t.VisivelPara != "*").ToList())
            {
                if (tipo.VisivelPara.StartsWith("*-"))
                {
                    //Remove os sinais do início da string
                    tipo.VisivelPara = tipo.VisivelPara.Replace("*-", "");
                    //Verifica se a lotação passada consta na lista de não visibilidade
                    tipo.VisivelPara.Split(',').Where(t => !idLotacao.StartsWith(t)).ToList().ForEach(t => listaTiposVisiveis.Add(tipo));
                }
                else //Trata o caso de apenas a lista de lotações visíveis ser cadastradas
                    tipo.VisivelPara.Split(',').Where(idLotacao.StartsWith).ToList().ForEach(t => listaTiposVisiveis.Add(tipo));
            }

            //Caso nenhum tipo tenha sido adicionado à lista, todos serão disponibilizados por padrão
            if (!listaTiposVisiveis.Any())
                listaTiposVisiveis = obterTodos();

            return listaTiposVisiveis;
        }

        //Obtém os tipos que estiverem dentro do período de validade, definido pelas colunas "inicio" e "fim"
        public IList<TipoAutorizacao> filtrarTiposVigentes(IList<TipoAutorizacao> tipos)
        {
            return tipos.Where(t => (DateTime.Now >= t.Inicio && DateTime.Now <= t.Fim) || //entre as datas
                                    (t.Inicio == null && t.Fim == null) ||                 //tipo ilimitado
                                    (t.Inicio <= DateTime.Today && t.Fim == null) ||       //a partir de
                                    (t.Inicio == null && t.Fim >= DateTime.Today))         //até a data
                                    .ToList();
        }
    }
}
