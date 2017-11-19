using System;

namespace Amon.Nucleo.Atributos
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class NaoMapear : Attribute
    {
        public NaoMapear() { }
    }
}