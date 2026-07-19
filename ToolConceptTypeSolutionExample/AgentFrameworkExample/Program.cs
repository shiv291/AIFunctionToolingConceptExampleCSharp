using Azure.AI.OpenAI;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.OpenAI;
using Microsoft.Extensions.AI;
using OpenAI.Chat;
using System.ClientModel;

string endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT", EnvironmentVariableTarget.User)!;
string apiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY", EnvironmentVariableTarget.User)!;
string deployment = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT", EnvironmentVariableTarget.User)!;

var client = new AzureOpenAIClient(
    new Uri(endpoint),
    new ApiKeyCredential(apiKey));

ChatClient chatClient = client.GetChatClient(deployment);

AIAgent agent = chatClient.AsAIAgent(
    name: "Calculator Agent",
    instructions: "You are a helpful calculator assistant.",
    tools:
    [
        AIFunctionFactory.Create(CalculatorTools.Add),
        AIFunctionFactory.Create(CalculatorTools.Multiply)
    ]);

Console.Write("Enter your question: ");
string input = Console.ReadLine()!;

var result = await agent.RunAsync(input);

Console.WriteLine(result);

public static class CalculatorTools
{
    public static int Add(int a, int b) => a + b;

    public static int Multiply(int a, int b) => a * b;
}