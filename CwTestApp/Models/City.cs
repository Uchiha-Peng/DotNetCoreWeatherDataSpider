using CwTestApp.Data;
using CwTestApp.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace CwTestApp.Models
{
    /// <summary>
    /// 城市信息
    /// </summary>
    public class City
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 城市名
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 拼音
        /// </summary>
        public string CityEnglishName { get; set; }

        /// <summary>
        /// 天气明细
        /// </summary>
        public ICollection<Weather> Weathers { get; set; }

        public static async Task<List<City>> GetCitys()
        {
            SQLiteDB db = new SQLiteDB();
            if (!db.Database.CanConnect())
            {
                Tool.WriteLog("错误", "数据库无法连接");
                return new List<City>();
            }
            List<City> cities = await db.Citys.ToListAsync();
            if (cities != null && cities.Count > 0)
                return cities;
            cities = new List<City>
            {
                new City(){
                     ID=1,
                      CityName="十堰",
                       CityEnglishName="ShiYan"
                },
                new City(){
                     ID=2,
                      CityName="武汉",
                       CityEnglishName="WuHan"
                },
                new City(){
                     ID=3,
                      CityName="南京",
                       CityEnglishName="NanJing"
                },
                new City(){
                     ID=4,
                      CityName="杭州",
                       CityEnglishName="HangZhou"
                },
                new City(){
                     ID=5,
                      CityName="成都",
                       CityEnglishName="ChengDu"
                },
                new City(){
                     ID=6,
                      CityName="无锡",
                       CityEnglishName="WuXi"
                },new City(){
                     ID=7,
                      CityName="广州",
                       CityEnglishName="GuangZhou"
                },new City(){
                     ID=8,
                      CityName="海口",
                       CityEnglishName="HaiKou"
                },new City(){
                     ID=9,
                      CityName="南宁",
                       CityEnglishName="NanNing"
                },new City(){
                     ID=10,
                      CityName="拉萨",
                       CityEnglishName="LaSa"
                }
            };
            db.AddRange(cities);
            int rows = await db.SaveChangesAsync();
            if (rows == cities.Count)
                return cities;
            return new List<City>();
        }
    }
}
