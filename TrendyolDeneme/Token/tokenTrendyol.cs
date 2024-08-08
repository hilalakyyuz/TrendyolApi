using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace TrendyolDeneme
{
    public class TokenTrendyol
    {
        public static void SetApiKeySecret(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var byteArray = Encoding.ASCII.GetBytes("***********" + ":" + "************");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            httpClient.DefaultRequestHeaders.Add("User-Agent", "-----" + " ------");
            httpClient.DefaultRequestHeaders.Add("x-clientip", "195.87.197.34");
            httpClient.DefaultRequestHeaders.Add("x-correlationid", Guid.NewGuid().ToString());
            httpClient.DefaultRequestHeaders.Add("x-agentname", "------------");
        }


        public static void SetApiKeySecret2(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var byteArray = Encoding.ASCII.GetBytes("***********" + ":" + "***********");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            httpClient.DefaultRequestHeaders.Add("x-clientip", "-----------------");
            httpClient.DefaultRequestHeaders.Add("x-correlationid", Guid.NewGuid().ToString());
            httpClient.DefaultRequestHeaders.Add("x-agentname", "-------------");
        }


    }
}