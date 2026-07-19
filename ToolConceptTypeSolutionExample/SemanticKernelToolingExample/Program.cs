using Microsoft.SemanticKernel;
using SemanticKernelToolingExample;

var builder = Kernel.CreateBuilder();

builder.AddAzureOpenAIChatCompletion(
     deploymentName: Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT",EnvironmentVariableTarget.User)!,
    endpoint: Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT", EnvironmentVariableTarget.User)!,
    apiKey: Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY", EnvironmentVariableTarget.User)!);

var kernel = builder.Build();

kernel.Plugins.AddFromType<CalculatorPlugin>();

var settings = new PromptExecutionSettings
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};

while(true)
{
    Console.Write("Ask: ");
    var question = Console.ReadLine();

    if (string.Equals(question, "exit", StringComparison.OrdinalIgnoreCase))
        break;

    var result = await kernel.InvokePromptAsync(question, new(settings));
    Console.WriteLine(result);
}