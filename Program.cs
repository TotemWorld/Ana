using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Python.Runtime;


var builder = new ConfigurationBuilder()
    .AddUserSecrets<Program>();
IConfiguration configuration = builder.Build();

var openAiApiKey = configuration["OPENAI_API_KEY"];
var pythonDllPath = configuration["PYTHON_DLL_PATH"];

Runtime.PythonDLL = pythonDllPath;
PythonEngine.Initialize();
using (Py.GIL())
{   
    using(var scope = Py.CreateScope())
    {
        scope.Set("person", "Diego");
    }
    dynamic module = Py.Import("test_channel");
    Console.WriteLine(module);
    dynamic value = module.ReturnValue(openAiApiKey);
    Console.WriteLine(value);
}
PythonEngine.Shutdown();

