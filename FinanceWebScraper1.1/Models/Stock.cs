using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceWebScraper.Models
{
    public class Stock
    {
        public int ID { get; set; }
        public string Symbol { get; set; }
        public decimal Change { get; set; }
        public decimal PercentChange { get; set; }
        public string Currency { get; set; }
        public string AverageVolume { get; set; }
        public string MarketCap { get; set; }
        public decimal Price { get; set; }
        public DateTime SnapshotTime { get; set; }
    }
}
