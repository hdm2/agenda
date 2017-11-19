using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Amon.Nucleo.Utils
{
    public static class Info
    {
        public static Resultado obterAtributo<Resultado>(this MemberInfo mi, bool herancao) where Resultado : class
        {
            object[] attributes = mi.GetCustomAttributes(typeof (Resultado), true);
            return attributes.FirstOrDefault() as Resultado;
        }

        public static object GetValue(this PropertyInfo mi, object obj)
        {
            return mi.GetValue(obj, (object[]) null);
        }
    }
}