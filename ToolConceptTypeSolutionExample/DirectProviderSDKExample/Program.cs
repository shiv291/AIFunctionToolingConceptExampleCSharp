using Azure.AI.OpenAI;
using DirectProviderSDKExample;
using OpenAI.Chat;
using System.ClientModel;
using System.Text.Json;

string endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT", EnvironmentVariableTarget.User)!;
string apiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY", EnvironmentVariableTarget.User)!;
string deployment = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT", EnvironmentVariableTarget.User)!;

var addTool = ChatTool.CreateFunctionTool(
    functionName: "add",
    functionDescription: "Adds two numbers.",
    functionParameters: BinaryData.FromString("""
    {
      "type":"object",
      "properties":{
        "a":{"type":"integer"},
        "b":{"type":"integer"}
      },
      "required":["a","b"]
    }
    """));

var multiplyTool = ChatTool.CreateFunctionTool(
    functionName: "multiply",
    functionDescription: "Multiplies two numbers.",
    functionParameters: BinaryData.FromString("""
    {
      "type":"object",
      "properties":{
        "a":{"type":"integer"},
        "b":{"type":"integer"}
      },
      "required":["a","b"]
    }
    """));

var client = new AzureOpenAIClient(
    new Uri(endpoint),
    new ApiKeyCredential(apiKey));

ChatClient chatClient = client.GetChatClient(deployment);

var options = new ChatCompletionOptions();
options.Tools.Add(addTool);
options.Tools.Add(multiplyTool);

Console.Write("Enter your question: ");
string userInput = Console.ReadLine()!;

List<ChatMessage> messages =
[
    new UserChatMessage(userInput)
];

while (true)
{
    ChatCompletion completion = chatClient.CompleteChat(messages, options);

    // No tool calls => final answer
    if (completion.ToolCalls.Count == 0)
    {
        Console.WriteLine(completion.Content[0].Text);
        break;
    }

    // IMPORTANT: Add assistant message containing tool calls
    messages.Add(new AssistantChatMessage(completion));

    foreach (ChatToolCall toolCall in completion.ToolCalls)
    {
        string toolResult = "";

        switch (toolCall.FunctionName)
        {
            case "add":
                {
                    using JsonDocument json = JsonDocument.Parse(toolCall.FunctionArguments);

                    int a = json.RootElement.GetProperty("a").GetInt32();
                    int b = json.RootElement.GetProperty("b").GetInt32();

                    toolResult = Calculator.Add(a, b).ToString();
                    break;
                }

            case "multiply":
                {
                    using JsonDocument json = JsonDocument.Parse(toolCall.FunctionArguments);

                    int a = json.RootElement.GetProperty("a").GetInt32();
                    int b = json.RootElement.GetProperty("b").GetInt32();

                    toolResult = Calculator.Multiply(a, b).ToString();
                    break;
                }

            default:
                throw new InvalidOperationException($"Unknown tool: {toolCall.FunctionName}");
        }

        messages.Add(new ToolChatMessage(toolCall.Id, toolResult));
    }
}

Console.ReadLine();