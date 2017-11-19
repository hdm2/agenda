using System;

namespace Amon.Nucleo.Atributos
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class Mapear : Attribute
    {
        private String colunaBanco;
        private String sufixoTabela;

        public Mapear()
        {
        }

        public Mapear(String nomeColuna)
        {
            colunaBanco = nomeColuna;
        }

        public Mapear(String nomeColuna, String nomeSufixo)
        {
            colunaBanco = nomeColuna;
            sufixoTabela = nomeSufixo;
        }

        public String ColunaBanco
        {
            get { return colunaBanco; }
        }

        public String SufixoTabela
        {
            get { return sufixoTabela; }
        }
    }
}