using EGTDigital.Entities;
using EGTDigital.Helpers;
using EGTDigital.RabbitMq;
using EGTDigital.Requests;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace EGTDigital.Controllers
{
    [ApiController]
    public class StatisticsCollectorController : Controller
    {
        private EgtDbContext dbContext;

        private MessageSender messageSender;

        public StatisticsCollectorController()
        {
            this.dbContext = DBConnectionManager.GetDBContext();
            this.messageSender = MessageSenderService.GetMessageSender();
        }

        [HttpPost]
        [Route("json_api/current")]
        public IActionResult Current([FromBody] BaseRequest request)
        {
            if (!CheckForRequestDuplication(request.RequestId))
            {
                request.ServiceName = ServiceType.EXT_SERVICE_2.ToString();

                AddRequestToDatabase(request);
                CurrencyRate currentCurrency = GetCurrentCurrency(request.Currency);
                if(currentCurrency != null)
                {
                    var currencyRateObject = JsonConvert.SerializeObject(currentCurrency);
                    return Content(currencyRateObject.ToString());
                }
                else
                {
                    return NotFound(MessagesHelper.CurrencyNotFound);
                }
            }
            return BadRequest(MessagesHelper.DuplicateRequestFoundMessage);
        }

        [HttpPost]
        [Route("json_api/history")]
        public IActionResult History([FromBody] PeriodRequest request)
        {
            if (!CheckForRequestDuplication(request.RequestId))
            {
                request.ServiceName = ServiceType.EXT_SERVICE_2.ToString();

                AddRequestToDatabase(request);
                List<CurrencyRate> currentCurrencyHistory = GetCurrencyHistoryList(request.Timestamp, request.Period, request.Currency);
                if (currentCurrencyHistory != null && currentCurrencyHistory.Count() > 0)
                {
                    var serializedList = JsonConvert.SerializeObject(currentCurrencyHistory);
                    return Content(serializedList);
                }
                else
                {
                    return NotFound(MessagesHelper.CurrencyNotFound);
                }
            }
            return BadRequest(MessagesHelper.DuplicateRequestFoundMessage);
        }

        [HttpPost]
        [Route("xml_api/command")]
        public IActionResult Command()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var serializer = new XmlSerializer(typeof(XmlRequest));
                using (var xmlReader = XmlReader.Create(reader))
                {
                    XmlRequest deserializedXml = (XmlRequest)serializer.Deserialize(xmlReader);

                    if (!CheckForRequestDuplication(deserializedXml.Id))
                    {
                        if (deserializedXml.Get != null)
                        {
                            //Since no timestamp is provided in the request, now is added for reference
                            BaseRequest baseRequest = new BaseRequest
                            {
                                Client = deserializedXml.Get.Consumer,
                                Currency = deserializedXml.Get.Currency,
                                RequestId = deserializedXml.Id,
                                Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                                ServiceName = ServiceType.EXT_SERVICE_1.ToString()
                            };

                            AddRequestToDatabase(baseRequest);
                            return GetCurrencyInfo(deserializedXml);
                        }
                        else if (deserializedXml.History != null)
                        {
                            //Since no timestamp is provided in the request, now is added for reference
                            BaseRequest baseRequest = new BaseRequest
                            {
                                Client = deserializedXml.History.Consumer,
                                Currency = deserializedXml.History.Currency,
                                RequestId = deserializedXml.Id,
                                Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                                ServiceName = ServiceType.EXT_SERVICE_1.ToString()
                            };

                            AddRequestToDatabase(baseRequest);
                            return GetCurrencyHistory(deserializedXml);
                        }
                        else
                        {
                            return BadRequest(MessagesHelper.RequestDeserializationException);
                        }
                    }
                    return BadRequest(MessagesHelper.DuplicateRequestFoundMessage);
                }
            }
        }

        //Gets period timestamp based on a given hour period.
        private long GetPeriodStartTimestamp(long requestTimestamp, int requestPeriod)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(requestTimestamp).ToLocalTime();
            return new DateTimeOffset(dtDateTime.AddHours(-requestPeriod)).ToUnixTimeSeconds();
        }

        //Check if the given request id already exists in the database.
        private bool CheckForRequestDuplication(string requestId)
        {
            return dbContext.Requests.Any(x => x.RequestId.Equals(requestId));
        }

        //Used to create a request record in the database
        private void AddRequestToDatabase(BaseRequest request)
        {
            Request requestEntity = new Request() { Client = request.Client, Currency = request.Currency, RequestId = request.RequestId, Timestamp = request.Timestamp, ServiceName = request.ServiceName };
            dbContext.Requests.Add(requestEntity);
            dbContext.SaveChanges();

            this.messageSender.SendMessage(requestEntity.ToString());
        }

        //Returns currency rate record in xml format that matches the given timestamp and currency name in xml format
        private IActionResult GetCurrencyInfo(XmlRequest request)
        {
            CurrencyRate currentCurrency = GetCurrentCurrency(request.Get.Currency);
            if (currentCurrency != null)
            {
                var serializerCurrencyRate = new XmlSerializer(typeof(CurrencyRate));
                using (var sww = new StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(sww))
                    {
                        serializerCurrencyRate.Serialize(writer, currentCurrency);
                        string xml = sww.ToString();

                        return Content(xml);
                    }
                }
            }
            else
            {
                return NotFound(MessagesHelper.CurrencyNotFound);
            }
        }

        //Returns CurrencyRate entity if found
        private CurrencyRate GetCurrentCurrency(string currency)
        {
            CurrencyData currentCurrencyInfo = dbContext.CurrencyDatas.OrderByDescending(x => x.Timestamp).FirstOrDefault();
            if (currentCurrencyInfo != null)
            {
                CurrencyRate currentCurrency = dbContext.CurrencyRates
                    .Where(x => x.CurrencyDataTimestamp.Equals(currentCurrencyInfo.Timestamp))
                    .Where(x => x.CurrencyName.Equals(currency.ToUpperInvariant()))
                    .FirstOrDefault();
                return currentCurrency;
            }
            return null;
        }
        //Returns a list of CurrencyRates
        private List<CurrencyRate> GetCurrencyHistoryList(long timeStamp, int period, string currency)
        {
            long timestamp = GetPeriodStartTimestamp(timeStamp, period);
            List<CurrencyRate> currentCurrency = dbContext.CurrencyRates
                .Where(x => x.CurrencyDataTimestamp > timestamp)
                .Where(x => x.CurrencyName.Equals(currency.ToUpperInvariant()))
                .OrderByDescending(x => x.CurrencyDataTimestamp)
                .ToList();
            return currentCurrency;
        }

        //Returns a list of currency rate records in xml format that matches the given timestamp and currency name in xml format
        private IActionResult GetCurrencyHistory(XmlRequest request)
        {
            List<CurrencyRate> currentCurrencyHistory = GetCurrencyHistoryList(new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds(), Convert.ToInt16(request.History.Period), request.History.Currency);

            if (currentCurrencyHistory != null && currentCurrencyHistory.Count() > 0)
            {
                var serializerCurrencyRate = new XmlSerializer(typeof(List<CurrencyRate>));
                using (var sww = new StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(sww))
                    {
                        serializerCurrencyRate.Serialize(writer, currentCurrencyHistory);
                        string xml = sww.ToString();

                        return Content(xml);
                    }
                }
            }
            else
            {
                return NotFound(MessagesHelper.CurrencyNotFound);
            }
        }
    }
}
