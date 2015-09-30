using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FreeMobile.FreeMobileSmsApi.Models;

namespace FreeMobile.FreeMobileSmsApi
{
    public class Api
    {
        private readonly HttpClient _httpClient;
        private readonly string _pass;
        private readonly Uri _smsapiUri;
        private readonly string _user;

        public Api(string user, string pass, Uri smsapiUri = null)
        {
            _user = user;
            _pass = pass;
            _httpClient = new HttpClient();
            _smsapiUri = smsapiUri ?? new Uri("https://smsapi.free-mobile.fr", UriKind.Absolute);
        }

        public async Task<SendSmsResult> SendSms(string message)
        {
            var queryParameter = new[]
            {
                new KeyValuePair<string, string>("user", _user),
                new KeyValuePair<string, string>("pass", _pass),
                new KeyValuePair<string, string>("msg", message)
            };

            HttpContent content = new FormUrlEncodedContent(queryParameter);
            var query = await content.ReadAsStringAsync();

            var response = await _httpClient.GetAsync(new Uri(_smsapiUri, "/sendmsg?" + query));
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return SendSmsResult.Sent;
                case HttpStatusCode.BadRequest:
                    return SendSmsResult.MissingParameter;
                case HttpStatusCode.PaymentRequired:
                    return SendSmsResult.Throttled;
                case HttpStatusCode.Forbidden:
                    return SendSmsResult.ServiceNotActivated;
                case HttpStatusCode.InternalServerError:
                    return SendSmsResult.ServerError;
            }
            return SendSmsResult.ServerError;
        }
    }
}