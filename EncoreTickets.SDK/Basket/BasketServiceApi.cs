using EncoreTickets.SDK.EntertainApi;
using RestSharp;
using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.XPath;
using EncoreTickets.SDK.EntertainApi.Model;

namespace EncoreTickets.SDK.Basket
{
    /// <summary>
    /// Wrapper class for the Basket service API
    /// </summary>
    public class BasketServiceApi : BaseEntertainApi
    {
        /// <summary>
        /// Default constructor for the Basket service
        /// </summary>
        /// <param name="context"></param>
        public BasketServiceApi(ApiContext context) : base(context, "basket-service.{0}tixuk.io/api/")
        {
        }
       
    }
}
