using EGTDigital.Entities;
using EGTDigital.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EGTDigital
{
    public class RatesCollector
    {
        private EgtDbContext dbContext;

        public RatesCollector()
        {
            dbContext = DBConnectionManager.GetDBContext();
        }

        public async Task StartUpdating(int interval, string url)
        {
            while (true)
            {
                await UpdateRates(url);
                Thread.Sleep(TimeSpan.FromSeconds(30));
            }
        }

        public async Task UpdateRates (string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    if (response != null)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var currencyDataJson = JsonConvert.DeserializeObject<CurrencyDataJson>(jsonString);
                        CurrencyData currencyData = new CurrencyData
                        {
                            Status = currencyDataJson.Success,
                            Timestamp = currencyDataJson.Timestamp,
                            BaseCurrency = currencyDataJson.Base,
                            Date = currencyDataJson.Date,
                            Rates = new List<CurrencyRate>()
                        };
                        
                        ConvertRates(currencyDataJson.Rates, currencyData.Rates, currencyDataJson.Timestamp);
                        AddCurrencyDataToDB(currencyData);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void AddCurrencyDataToDB(CurrencyData currencyData)
        {
            dbContext.CurrencyDatas.Add(currencyData);
            dbContext.SaveChanges();
        }

        private static void ConvertRates(Dictionary<string, decimal> ratesDictionary, List<CurrencyRate> rates, long timeStamp)
        {
            foreach (var rate in ratesDictionary)
            {
                rates.Add(new CurrencyRate { CurrencyDataTimestamp = timeStamp, CurrencyName = rate.Key, Rate = rate.Value });
            }
        }
    }
}
