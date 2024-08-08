using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TrendyolDeneme
{
    class dbMusteriSoruCvp
    {
        //get questions
        public static EntityContent GetTrendyolDataAnswers(DateTime startDate, DateTime endDate, string status = "")
        {
            string supplierId = "**********";
            int pageSize = 50;

            long startUnixTime = new DateTimeOffset(startDate).ToUnixTimeMilliseconds();
            long endUnixTime = new DateTimeOffset(endDate).ToUnixTimeMilliseconds();

            List<Content> allData = new List<Content>();
            int currentPage = 0;

            using (HttpClient client = new HttpClient())
            {
                TokenTrendyol.SetApiKeySecret2(client); 

                try
                {
                    do
                    {
                        string apiUrl = $"https://api.trendyol.com/sapigw/suppliers/{supplierId}/questions/filter?startDate={startUnixTime}&endDate={endUnixTime}&status={status}&page={currentPage}&size={pageSize}&orderByDirection=DESC";

                        Console.WriteLine("Request URL: " + apiUrl);

                        HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = response.Content.ReadAsStringAsync().Result;
                            Console.WriteLine("Response Body: " + responseBody); 

                            EntityContent pageData = JsonConvert.DeserializeObject<EntityContent>(responseBody);
                            Console.WriteLine("Deserialized Data: " + JsonConvert.SerializeObject(pageData)); 

                            if (pageData != null && pageData.Content != null)
                            {
                                allData.AddRange(pageData.Content);
                                currentPage++;
                                if (currentPage >= pageData.TotalPages)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Deserialized object is null or Content is null.");
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("API request failed. Error code: " + response.StatusCode);
                            Console.WriteLine("Error message: " + response.Content.ReadAsStringAsync().Result);
                            return null;
                        }
                    } while (true);

                    EntityContent result = new EntityContent
                    {
                        Content = allData.ToArray(),
                        Page = currentPage - 1,
                        Size = pageSize,
                        TotalElements = allData.Count,
                        TotalPages = (int)Math.Ceiling((double)allData.Count / pageSize)
                    };

                    return result;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("An error occurred during the HTTP request: " + ex.Message);
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An unexpected error occurred: " + ex.Message);
                    return null;
                }
            }
        }
    //POST createAnswer
    public static EntityContent PostCreateAnswer(long questionId, string answerText)
        {
            string apiUrl = $"https://api.trendyol.com/sapigw/suppliers/238853/questions/{questionId}/answers";

            using (HttpClient client = new HttpClient())
            {
                TokenTrendyol.SetApiKeySecret(client);

                try
                {
                    var requestData = new
                    {
                        text = answerText
                    };

                    string requestDataJson = JsonConvert.SerializeObject(requestData);

                    var httpContent = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = client.PostAsync(apiUrl, httpContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = response.Content.ReadAsStringAsync().Result;

                        EntityContent pageData = JsonConvert.DeserializeObject<EntityContent>(responseBody);
                        return pageData;
                    }
                    else
                    {
                        Console.WriteLine("API isteği başarısız oldu. Hata kodu: " + response.StatusCode);
                        return null;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("HTTP isteği sırasında bir hata oluştu: " + ex.Message);
                    return null;
                }
            }
        }
    }
}
