using System;

namespace Amon.Nucleo.Atributos
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class Transiente : Attribute
    {
        public Transiente(){ }
    }
}