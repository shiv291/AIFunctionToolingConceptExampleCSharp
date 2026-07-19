using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;

namespace ExtensionAI.UseFunctionInvocation
{
    /*
     * dotnet add package Microsoft.Extensions.AI
dotnet add package Microsoft.Extensions.AI.OpenAI
dotnet add package Azure.AI.OpenAI
     * 
     */
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT",EnvironmentVariableTarget.User)!;
            string apiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY", EnvironmentVariableTarget.User)!;
            string deployment = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT", EnvironmentVariableTarget.User)!;

            var client = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
            IChatClient chatClient = client.GetChatClient(deployment)
                .AsIChatClient()
                .AsBuilder()
                .UseFunctionInvocation()
                .Build();

            var calc = new CalculatorTool();
            var tools = new[]
            {
                AIFunctionFactory.Create(calc.Add),
                AIFunctionFactory.Create(calc.Subtract),
                AIFunctionFactory.Create(calc.Multiply),
                AIFunctionFactory.Create(calc.Divide),
                AIFunctionFactory.Create(calc.Power),
                AIFunctionFactory.Create(calc.SquareRoot)

            };

            Console.WriteLine("AI Calculator (type 'exit' to quit)");
            while(true)
            {
                Console.WriteLine("> ");
                var prompt= Console.ReadLine();
                if (string.Equals(prompt, "exit", StringComparison.OrdinalIgnoreCase))
                    break;
                var response = await chatClient.GetResponseAsync(
                    prompt!,
                    new ChatOptions { Tools = tools }
                    );
                Console.WriteLine(response.Text);
            }

            
            Console.ReadLine();
        }
    }
}
