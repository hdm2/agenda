using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Amon.Nucleo.Entidade
{
    public interface IEntidade
    {
        void deReader(IDataReader dr);

        void deTable(DataRow ln);

        void deHashtable(Hashtable ht);

        String instrucaoParaInsert(String campoAutoIncrementado);

        String instrucaoParaUpdate(String campoAutoIncrementado);

        String whereComChave();

        Dictionary<String, Type> obterTiposMapeado();

        Dictionary<String, Type> obterTiposMapeadoChave();

        Object this[String propriedade] { get; }
    }
}