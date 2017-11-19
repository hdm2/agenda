using System;

namespace Amon.Nucleo.Retorno
{
    public interface IRetorno
    {
        void escreveRetorno(String texto, params Object[] arg);
        void escreveRetornoQuebraLinha(String texto, params Object[] arg);
    }
}