using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Python.Runtime;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using DiscordCommands = Discord.Commands;
using Discord.Interactions;
using Newtonsoft.Json;
using System.Text;

namespace Ana.Service
{
    public class OpenAiService
    {
        private readonly IConfiguration _configuration;
        private readonly string _openAiApiKey;
        private readonly HttpClient _httpClient;
        private readonly string _pythonServerUrl;

        public OpenAiService(IConfiguration configuration)
        {
            _configuration = configuration;
            _openAiApiKey = _configuration["OPENAI_API_KEY"]!;
            _httpClient = new HttpClient();
            _pythonServerUrl = _configuration["PYTHON_SERVER_URL"]!;
        }

        public async Task<string> RunQuestionQuery(string input)
        {
            var inputObject = new { Query = input };
            var json = JsonConvert.SerializeObject(inputObject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            Uri uri = new($"{_pythonServerUrl}/answer");
            var response = await _httpClient.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                Console.WriteLine($"Error upoading the asset: {response.StatusCode}");
                return "I'm experiencing internal troubles. Can you please repeat your question?";
            }
        }

    }
}
