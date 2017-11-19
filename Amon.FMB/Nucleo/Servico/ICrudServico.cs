using System.Collections.Generic;

namespace Amon.Nucleo.Servico
{
    public interface ICrudServico<E>
    {
        E obter(int id);

        IList<E> obterTodos();

        E incluir(E entidade);

        void atualizar(E entidade);

        void excluir(E entidade);
    }
}