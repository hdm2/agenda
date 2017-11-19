using System.Collections.Generic;

namespace Amon.Nucleo.Servico
{
    public interface ICrudServico<E>
    {
        E obterPorIndice(int id);

        IList<E> obterTodos();

        E incluir(E entidade);

        void atualizar(E entidadeAtualizada);

        void excluir(E entidade);
    }
}