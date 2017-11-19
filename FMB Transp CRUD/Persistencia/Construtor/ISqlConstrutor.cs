using System;
using System.Collections.Generic;

namespace Amon.Persistencia.Construtor
{
    public interface ISqlConstrutor
    {
        Dictionary<string, string> Mapeamento { get; }

        ISqlConstrutor innerJoin(String propriedade, String tabela, String apelido = null,
                                                String condicionalAdicional = null);

        ISqlConstrutor leftJoin(String propriedade, String tabela, String apelido = null,
                                               String condicionalAdicional = null);

        ISqlConstrutor rightJoin(String propriedade, String tabela, String apelido = null,
                                                String condicionalAdicional = null);

        String obterSelect();
        String obterFrom();
        String obterSqlCompleto();
        String obterNomeMapeado(string propriedade);
        String obterNomeMapeadoParametro(string propriedade);
    }
}