namespace Amon.Nucleo.Persistencia
{
    public interface IMapeamentoSQL<Entidade>
    {
        void configurarMapeamento();
        void configurarChave();
    }
}
