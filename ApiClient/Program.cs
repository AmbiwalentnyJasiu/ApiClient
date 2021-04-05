using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net;
using System.Collections.Generic;

namespace ApiClient
{
    class Program
    {
        const string baseUrl = "https://localhost:44334/api/ItemModels";
        
        private static readonly HttpClient client = new HttpClient();

        private static async Task<List<Model>> GetAllEntries()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var httpResponse = await client.GetAsync(baseUrl);
            if (Convert.ToInt32(httpResponse.StatusCode) >= 200 && Convert.ToInt32(httpResponse.StatusCode) < 300)
            {
                Console.WriteLine("Success!\n");
                var streamResponse = await httpResponse.Content.ReadAsStreamAsync();
                var resultList = await JsonSerializer.DeserializeAsync<List<Model>>(streamResponse);

                return resultList;
            }
            else if (httpResponse.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine("No entries!\n");
                return null;
            }
            else if (Convert.ToInt32(httpResponse.StatusCode) >= 400 && Convert.ToInt32(httpResponse.StatusCode) < 600)
            {
                Console.WriteLine("Different Error!\n");
                return null;
            }
            else
                return null;
        }

        private static async Task<Model> GetSingleEntry(string id)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var url = baseUrl + $"/{id}";

            var httpResponse = await client.GetAsync(url);
            if (Convert.ToInt32(httpResponse.StatusCode) >= 200 && Convert.ToInt32(httpResponse.StatusCode) < 300)
            {
                Console.WriteLine("Success!\n");
                var streamResponse = await httpResponse.Content.ReadAsStreamAsync();
                var resultList = await JsonSerializer.DeserializeAsync<Model>(streamResponse);

                return resultList;
            }
            else if (httpResponse.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine("No such entry!\n");
                return null;
            }
            else if (Convert.ToInt32(httpResponse.StatusCode) >= 400 && Convert.ToInt32(httpResponse.StatusCode) < 600)
            {
                Console.WriteLine("Different Error!\n");
                return null;
            }
            else
                return null;
        }

        private static async Task PostEntry(string id)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var content = new StringContent($"{{\"id\":\"{id}\"}}", System.Text.Encoding.UTF8, "application/json");

            var result = await client.PostAsync(baseUrl, content);

            if(result.StatusCode == HttpStatusCode.Conflict)
            {
                Console.WriteLine("Entry already exists!\n");
            }
            else if(Convert.ToInt32(result.StatusCode) >= 200 && Convert.ToInt32(result.StatusCode) < 300)
            {
                Console.WriteLine("Success!\n");
            }
            else if(Convert.ToInt32(result.StatusCode) >= 400 && Convert.ToInt32(result.StatusCode) < 600)
            {
                Console.WriteLine("Different Error\n");
            }
        }

        static async Task Main(string[] args)
        {
            int choice;

            Console.WriteLine("Welcome!");

            do
            {
                Console.WriteLine("/nType '1' to GET all entries");
                Console.WriteLine("Or   '2' to GET an entry");
                Console.WriteLine("Or   '3' to POST an entry");
                Console.WriteLine("Or   '4' to exit\n");

                if(int.TryParse(Console.ReadLine(),out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            var resultAll = await GetAllEntries();

                            if (resultAll != null)
                                foreach (var elem in resultAll)
                                    elem.Write();

                            break;
                        case 2:
                            Console.Write("Which entry do you want to see?");
                            var identifierG = Console.ReadLine();

                            var resultOne = await GetSingleEntry(identifierG);

                            resultOne.Write();
                            break;
                        case 3:
                            Console.Write("Input the identifier:");
                            var identiferP = Console.ReadLine();
                            await PostEntry(identiferP);
                            Console.WriteLine();
                            break;
                        case 4:
                            Console.WriteLine("Exiting...\n");
                            break;
                        default:
                            Console.WriteLine("Wrong Choice!");
                            break;

                    }
                }
            } while (choice != 4);
        }
    }
}
