using System.Collections.Generic;
using Amon.Nucleo.Entidade;

namespace Amon.Nucleo.Persistencia
{
    public interface IDao<E> : ISimplesDao where E : IEntidade
    {
        E obter(E chave);
        IList<E> obterTodos();
        E incluir(E entidade);
        void atualizar(E entidade);
        void excluir(E entidade);
    }
}