using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Amon.Nucleo.Utils
{
    public class XMLUtils
    {
        public static String converterParaXML(Object obj)
        {
            XmlSerializer xmls = new XmlSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            XmlTextWriter xmltw = new XmlTextWriter(ms, Encoding.UTF8);
            xmls.Serialize(xmltw, obj);
            ms = (MemoryStream)xmltw.BaseStream;
            return UTF8ByteArrayToString(ms.ToArray());
        }

        public static Object converterParaClasse(String xml, Type tipo)
        {
            XmlSerializer xmls = new XmlSerializer(tipo);
            MemoryStream ms = new MemoryStream(StringToUTF8ByteArray(xml));
            XmlTextWriter xmltw = new XmlTextWriter(ms, Encoding.UTF8);
            return xmls.Deserialize(ms);
        }

        private static String UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        private static Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
    }
}