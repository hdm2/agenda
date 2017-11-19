using System;
using System.Data;

namespace Amon.Nucleo.Utils
{
    public static class ConverteUtils
    {
        public static Double sempreConverteDouble(Object vl)
        {
            Double r = 0;
            try
            {
                r = Convert.ToDouble(vl);
            }
            catch
            {
                r = 0;
            }
            return r;
        }
        public static Decimal sempreConverteDecimal(Object vl)
        {
            Decimal r = 0;
            try
            {
                r = Convert.ToDecimal(vl);
            }
            catch
            {
                r = 0;
            }
            return r;
        }

        public static int sempreConverteInt32(Object vl)
        {
            int r = 0;
            try
            {
                r = Convert.ToInt32(vl);
            }
            catch
            {
                r = 0;
            }
            return r;
        }
        
        public static Int64 sempreConverteInt64(Object vl)
        {
            Int64 r = 0;
            try
            {
                r = Convert.ToInt64(vl);
            }
            catch
            {
                r = 0;
            }
            return r;
        }

        public static DateTime sempreConverteDate(Object vl)
        {
            DateTime r;
            try
            {
                r = Convert.ToDateTime(vl);
            }
            catch
            {
                r = DateTime.Now;
            }
            return r;
        }

        public static Boolean sempreConverteBoleano(Object vl, Boolean padraoParaNulo = false)
        {
            Boolean r = padraoParaNulo;
            try
            {
                r = Convert.ToBoolean(vl);
            }
            catch
            {
                r = padraoParaNulo;
            }
            return r;
        }

        public static DbType converteParaDbType(Type type)
        {
            if (type == typeof(Int64))
                return DbType.Int64;

            if (type.Name.Equals("int"))
                return DbType.Int32;

            if (type.Name.ToLower().Equals("string") || type.Name.ToLower().Equals("char"))
                return DbType.String;

            if (type == typeof (TimeSpan))
                return DbType.Time; //.DateTime;

            String[] tipos = Enum.GetNames(typeof(DbType));
            foreach (String tpStr in tipos)
            {
                if (tpStr.Equals(type.Name))
                    return (DbType)Enum.Parse(typeof(DbType), tpStr, true);
            }
            return DbType.Object;
        }

        public static DateTime TimeSpanToDateTime(TimeSpan time)
        {
            return new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, time.Hours, time.Minutes, time.Seconds);
        }

        public static TimeSpan DateTimeToTimeSpan(DateTime time)
        {
            return new TimeSpan(time.Hour, time.Minute, time.Second);
        }
    }
}