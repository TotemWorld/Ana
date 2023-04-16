using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Python.Runtime;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using DiscordCommands = Discord.Commands;
using Discord.Interactions;

namespace Ana.Service
{
    public class OpenAiService
    {
        private readonly IConfiguration _configuration;
        private readonly string _openAiApiKey;
        private readonly string _pythonDllPath;

        public OpenAiService(IConfiguration configuration)
        {
            _configuration = configuration;
            _openAiApiKey = _configuration["OPENAI_API_KEY"]!;
            _pythonDllPath = _configuration["PYTHON_DLL_PATH"]!; 
        }

        public string RunQuestionQuery(string input)
        {
            string value = "";
            Runtime.PythonDLL = _pythonDllPath;
            PythonEngine.Initialize();
            using(Py.GIL())
            {
                using(var scope = Py.CreateScope())
                {
                    dynamic module = Py.Import("question_embedding");
                    value =  module.RunQuestionQueryPy(_openAiApiKey, input);
                }
            }
            
            PythonEngine.Shutdown();
            return value;
        }

    }
}
