using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CwTestApp.Models
{
    /// <summary>
    /// 天气信息
    /// </summary>
    public class Weather
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 城市编号
        /// </summary>
        public int CityID { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 天气状况
        /// </summary>
        public string WeatherDetail { get; set; }

        /// <summary>
        /// 温度
        /// </summary>
        public string Temperature { get; set; }

        /// <summary>
        /// 风
        /// </summary>
        public string Wind { get; set; }
    }
}
