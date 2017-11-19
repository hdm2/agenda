using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Amon.Web.Utils
{
    public class Mensagens
    {
        //Mensagens para uso interno desta classe
        private static String sucessoOperacao = "Operação realizada com sucesso!";
        private static String erroOperacao = "Ocorreu um erro na operação!";

        private static String sucessoExclusao = "Item excluído com sucesso!";
        private static String sucessoCadastro = "Item cadastrado com sucesso!";
        private static String erroCadastro = "Ocorreu um problema ao tentar cadastrar o item!";
        private static String violacaoDeChavePrimaria = "Já existe um item definido com os identificadores especificados!";
        private static String nenhumItemEncontrado = "A busca não encontrou nenhum item.";

        //Mensagens públicas compartilhadas em toda a aplicação
        public static String PlaceholderTabelaConsulta = "O resultado da consulta aparecerá abaixo.";

        //Collection de mensagens para feedback ao usuário
        private static Dictionary<int, String> messages = new Dictionary<int, string>();
        public static int NotificacaoErro = 0,
                          NotificacaoSucesso = 1,
                          NotificacaoNenhumItemEncontrado = 2;

        static Mensagens()
        {
            messages.Add(0, erroOperacao);
            messages.Add(1, sucessoOperacao);
            messages.Add(2, nenhumItemEncontrado);
        }

        public static String GetStatusMessage(int? status)
        {
            if (status != null && messages.Count > status)
                return messages[status.Value].ToString();

            return null;
        }
    }
}