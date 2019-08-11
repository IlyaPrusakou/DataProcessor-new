using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace sorter
{
    public class Parser
    {
        public List<offer> Offers { get; set; }
        public List<string> ErrorLog { get; set; }
        public HttpClient Http { get; set; }
        public Parser()
        {

        }
        public Parser(List<offer> list)
        {
            Offers = list;
            //ErrorOffers = new List<offer>();
            ErrorLog = new List<string>();
            Http = new HttpClient();
        }
        public void SerializeDescription(Description des)
        {
            XmlWriterSettings set = new XmlWriterSettings { Indent = true }; // вынести в общее поле
            XmlSerializer xs = new XmlSerializer(typeof(Description));

            using (FileStream fs = new FileStream(@"D:\c# excel\description.txt", FileMode.Append))
            {
                using (XmlWriter str = XmlWriter.Create(fs, set))
                {
                    xs.Serialize(str, des);
                }
            }
        }

        private static async Task<byte[]> Get(string path, HttpClient http)
        {
                byte[] response = null;
            try
            {
                response = await http.GetByteArrayAsync(path);
            }
            catch (Exception ex)
            {
                throw;
            }
                
                return response;
        }
        private HtmlDocument LoadHtmlDoc(string url)
        {
            byte[] mass;
            try
            {
                mass = Get(url, Http).Result;
            }
            catch
            {
                throw;
            }
            string source = Encoding.GetEncoding("windows-1251").GetString(mass, 0, mass.Length - 1);
            source = WebUtility.HtmlDecode(source);
            HtmlDocument resultat = new HtmlDocument();
            resultat.LoadHtml(source);
            return resultat;
        }
        private string ReceiveDescription(string offerid)
        {
            string sku = $"/sku_{offerid}/";
            string url = @"https://firma-gamma.ru/ishop" + sku + "#itemdesc";
            //string url = @"https://firma-gamma.ru" + sku + "#itemdesc";
            string descr;
            string errormassage;
            try
            {
                HtmlDocument doc = LoadHtmlDoc(url);
                HtmlNode node = doc.DocumentNode.SelectSingleNode("//*[@id='itemdesc']");
                descr = node.InnerText.Replace("Поделиться", " ");
            }
            catch (Exception ex)
            {
                descr = "Error";
                errormassage = offerid + " - exception " + ex.GetType().Name + " - message " + ex.Message + " - time " + DateTime.Now + "\r\n";
                ErrorLog.Add(errormassage);

                using (FileStream SourceStream = File.Open(@"D:\c# excel\errors.txt", FileMode.Append))
                {
                    byte[] input = Encoding.Default.GetBytes(errormassage);
                    SourceStream.Write(input, 0, input.Length);
                }
            }
            return descr;
        }
        public List<offer> InsertDescriptionToOffer() // обязательно надо иметь агрегэйтнэйм
        {
            List<offer> result = new List<offer>();
            List<string> list = new List<string>();
            foreach (offer item in Offers)
            {
                list.Add(item.AggregateName);
            }
            List<string> distinctlist = list.Distinct().ToList();

            distinctlist.Remove(null);
            Dictionary<string, List<offer>> dict = new Dictionary<string, List<offer>>();
            foreach (string item in distinctlist)
            {
                List<offer> d = Offers.Where(offer => offer.AggregateName == item).ToList();
                dict.Add(item, d);
            }
            foreach (string item in dict.Keys)
            {
                string d = ReceiveDescription(dict[item][0].OfferId);
                if (d == "Error")
                {
                    foreach (offer i in dict[item])
                    {
                        i.Description = d;
                        result.Add(i);
                    }
                }
                else
                {
                    foreach (offer s in dict[item])
                    {
                        s.Description = d;
                        Description des = new Description { OfferId = s.OfferId, Content = d };
                        SerializeDescription(des);
                        result.Add(s);
                    }
                }
                Thread.Sleep(5000);
            }
            return result;






            //foreach (offer item in Offers)
            //{
                //string d = ReceiveDescription(item.OfferId);
                //if (d == "Error")
                //{
                    //item.Description = d;
                    
                //}
                //else
                //{
                    //item.Description = d;
                    //Description des = new Description {OfferId = item.OfferId, Content = d };
                    //SerializeDescription(des);
                //}
                //Thread.Sleep(5000);
            //}
            
        }

    }
}
