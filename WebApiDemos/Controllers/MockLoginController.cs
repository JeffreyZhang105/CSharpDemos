using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web.Http;
using WebApiDemos.Filters;

namespace WebApiDemos.Controllers
{
    [System.Web.Http.RoutePrefix("mocklogin")]
    public class MockLoginController : ApiController
    {
        [HttpPost]
        [HttpGet]
        [Route("login")]
        [MyweatherExceptionFilter]
        public HttpResponseMessage Post(FormDataCollection datas)
        {
            //var uid = datas.Get("username");
            //var password = datas.Get("password");

            var token = Guid.NewGuid().ToString("N");
            var responseStr = "{\"result\": \"OK\", \"token\": \"" + token + "\"}";
            var response = new HttpResponseMessage
            {
                Content = new StringContent(responseStr, Encoding.UTF8, "application/json")
            };
            return response;
        }
    }
}