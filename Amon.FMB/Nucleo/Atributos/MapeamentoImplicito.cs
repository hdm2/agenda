using System;

namespace Amon.Nucleo.Atributos
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class MapeamentoImplicito : Attribute
    {
        public MapeamentoImplicito()
        {
            //if (DateTime.Today > new DateTime(2014, 7, 30))
            //    throw new Exception("VERSÃO DE DEMONSTRAÇÃ EXPIROU");
        }
    }
}