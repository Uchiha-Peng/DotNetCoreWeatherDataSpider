using CwTestApp.Data;
using CwTestApp.Models;
using CwTestApp.Tools;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CwTestApp
{
    class Program
    {
        private static SQLiteDB db = new SQLiteDB();
        static async Task Main(string[] args)
        {
            List<City> cities = null;
            try
            {
                cities = await City.GetCitys();
                //var t =await db.Weathers.ToListAsync();
                //Tool.SaveJson<Weather>(t);
                //foreach (var item in cities)
                //{
                //    int count = db.Weathers.Count(n => n.CityID == item.ID);
                //    Console.WriteLine(item.CityName + ":" + count);
                //}
                //Console.ReadKey();
            }
            catch (Exception ex)
            {
                Tool.WriteLog("错误", ex.Message);
                Tool.WriteLog("错误", "===========获取城市列表出错，程序终止===========");
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            foreach (City city in cities)
            {
                DateTime now = DateTime.Now;
                for (int i = 0; i < now.Month + 12; i++)
                {
                    string yearmounth = now.AddMonths(-i).ToString("yyyyMM");
                    var url = $@"http://www.tianqihoubao.com/lishi/{city.CityEnglishName}/month/{yearmounth}.html";
                    bool b = await GetDataFormHtml(url, city.ID);
                }
                Tool.WriteLog("消息", "===============" + city.CityName + "获取结束===============");
            }
            var t = await db.Weathers.ToListAsync();
            Tool.SaveJson<Weather>(t);

        }

        public static async Task<bool> GetDataFormHtml(string url, int cityID)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage resp = await client.GetAsync(url);
                    if (resp.IsSuccessStatusCode)
                    {
                        byte[] bytes = await resp.Content.ReadAsByteArrayAsync();
                        //注册RegisterProvider
                        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                        //声明GB2312
                        var GbEncoding = Encoding.GetEncoding("GB2312");
                        //byte[]转html字符串
                        var htmlStr = GbEncoding.GetString(bytes, 0, bytes.Length);
                        //从字符串解析html文档
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(htmlStr);
                        HtmlNode content = doc.DocumentNode.SelectSingleNode("//body//div//div[@id='bd']//div[@class='hd']//div[@id='content']");
                        string title = content.SelectSingleNode("h1").InnerHtml.Replace("\r\n", "").Replace(" ", "").Trim();
                        Tool.WriteLog("消息", title);
                        HtmlNodeCollection trs = content.SelectSingleNode("table").SelectNodes("tr");
                        if (trs != null && trs.Count > 0)
                        {
                            foreach (var tr in trs)
                            {
                                HtmlNodeCollection tds = tr.SelectNodes("td");
                                //跳过标题
                                if (trs.IndexOf(tr) == 0)
                                    continue;
                                Weather weather = new Weather() { ID = Guid.NewGuid().ToString().ToLower(), CityID = cityID };
                                foreach (var td in tds)
                                {
                                    HtmlNode a = td.SelectSingleNode("a");
                                    string value = string.Empty;
                                    value = a != null ? a.InnerHtml : td.InnerHtml;
                                    value = value.Replace("\r\n", "").Replace(" ", "").Trim();
                                    if (string.IsNullOrWhiteSpace(value))
                                        continue;
                                    // 日期 天气状况 气温 风力风向
                                    switch (tds.IndexOf(td))
                                    {
                                        case 0:
                                            value = value.Replace("年", "/").Replace("月", "/").Replace("日", "");
                                            if (DateTime.TryParse(value, out DateTime date))
                                                weather.Date = date;
                                            break;
                                        case 1:
                                            weather.WeatherDetail = value;
                                            break;
                                        case 2:
                                            weather.Temperature = value;
                                            break;
                                        case 3:
                                            weather.Wind = value;
                                            break;
                                    }
                                }
                                if (db == null)
                                    db = new SQLiteDB();
                                db.Weathers.Add(weather);
                                await db.SaveChangesAsync();
                            }
                        }
                    }
                }
                Tool.WriteLog("消息", "本次数据抓取成功");
                return true;
            }
            catch (Exception e)
            {
                Tool.WriteLog("错误", e.Message);

                return false;
            }
        }


        #region  从Web抓取天气数据，并打印在控制台     
        /// <summary>
        /// 从Web抓取天气数据，并打印在控制台
        /// </summary>
        public async void ConsoleWeatherData()
        {
            try
            {
                string url = "http://www.tianqihoubao.com/lishi/shiyan/month/201901.html";
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage resp = await client.GetAsync(url);
                    if (resp.IsSuccessStatusCode)
                    {
                        byte[] bytes = await resp.Content.ReadAsByteArrayAsync();
                        //注册RegisterProvider
                        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                        //声明GB2312
                        var GbEncoding = Encoding.GetEncoding("GB2312");
                        //byte[]转html字符串
                        var htmlStr = GbEncoding.GetString(bytes, 0, bytes.Length);
                        //从字符串解析html文档
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(htmlStr);
                        HtmlNode content = doc.DocumentNode.SelectSingleNode("//body//div//div[@id='bd']//div[@class='hd']//div[@id='content']");
                        string title = content.SelectSingleNode("h1").InnerHtml.Replace("\r\n", "").Replace(" ", "").Trim();
                        Console.WriteLine("\r\t" + title);
                        HtmlNodeCollection trs = content.SelectSingleNode("table").SelectNodes("tr");
                        if (trs.Count > 0)
                        {
                            foreach (var tr in trs)
                            {
                                HtmlNodeCollection tds = tr.SelectNodes("td");
                                foreach (var td in tds)
                                {
                                    if (td.ChildNodes.Count > 0)
                                    {
                                        HtmlNode b = td.SelectSingleNode("b");
                                        if (b != null)
                                        {
                                            Console.Write("\t" + b.InnerHtml.Replace("\r\n", "").Replace(" ", "").Trim() + "\t");
                                            continue;
                                        }
                                        HtmlNode a = td.SelectSingleNode("a");
                                        if (a != null)
                                            Console.Write("\t" + a.InnerHtml.Replace("\r\n", "").Replace(" ", "").Trim() + "\t");
                                        else
                                            Console.Write("\t" + td.InnerHtml.Replace("\r\n", "").Replace(" ", "").Trim() + "\t");
                                    }
                                }
                                Console.Write("\r\n");
                            }
                        }
                        Console.WriteLine("Bad Request！");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion
    }
}
