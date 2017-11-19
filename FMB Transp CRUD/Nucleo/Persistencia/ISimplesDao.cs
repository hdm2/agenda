using System;

namespace Amon.Nucleo.Persistencia
{
    public interface ISimplesDao
    {
        void abreConexao();
        void fechaConexao();
        void iniciarTransacao();
        void cancelarTransacao();
        void concluirTransacao();
        Object obterOutroDao(Type t);
        Resultado obterOutroDao<Resultado>() where Resultado : ISimplesDao;
    }
}