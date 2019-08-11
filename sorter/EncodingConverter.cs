using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace sorter
{
    public class EncodingConverter
    {

        public List<offer> Source { get; set; }

        public EncodingConverter()
        {

        }
        public EncodingConverter(List<offer> source)
        {
            Source = source;
        }

        public List<offer> ConvertSourceEncodingtoTarget()
        {
            Encoding utf = Encoding.UTF8;
            Encoding windows1254 = Encoding.GetEncoding(1254);
            List<offer> result = new List<offer>();
            foreach (offer of in Source)
            {
                Type oftype = of.GetType();
                PropertyInfo[] propArray = oftype.GetProperties();
                foreach (PropertyInfo prop in propArray)
                {
                    if (prop.PropertyType.Name == "string") // или String
                    {
                        string sourceString = (string)prop.GetValue(of);
                        byte[] winBytes = windows1254.GetBytes(sourceString);
                        byte[] utfBytes = Encoding.Convert(utf, windows1254, winBytes);
                        string utfstring = utf.GetString(utfBytes);
                        prop.SetValue(of, (object)utfstring);
                    }
                }
                result.Add(of);
            }
            return result;
        }





    }
}
