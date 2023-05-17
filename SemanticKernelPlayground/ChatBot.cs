using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.TextCompletion;
using Microsoft.SemanticKernel.Orchestration;
using SemanticKernelPlayground.RepoUtils;

public class ChatBot
{
    public static async Task Run()
    {
        ITextCompletion CompletionFactory(IKernel k) => new WebLlmTextCompletion();

        var kernel = Kernel.Builder.WithLogger(ConsoleLogger.Log)
            .Configure(c=> c.AddTextCompletionService(CompletionFactory))
            .Build();
        string skillsFolder = RepoFiles.SkillsPath();


        var chatSkill = kernel.ImportSemanticSkillFromDirectory(skillsFolder, "ChatSkill");
        
        string history = "";
        while (true)
        {
            Console.Write("User: ");
            var ask = Console.ReadLine();

            var contextVariables = new ContextVariables();
            contextVariables.Set("input", ask);
            contextVariables.Set("user", "User: ");
            contextVariables.Set("bot", "AI Assistant: ");
            contextVariables.Set("history", history);
            var res = await kernel.RunAsync(
                contextVariables,
                chatSkill["Chat"]
            );
            var botResponse = $"AI Assistant: {res.Result.Trim()}\n";
            var userRequest = $"User: {ask}\n";

            history += userRequest;
            history += botResponse;
            Console.WriteLine(botResponse);
        }
    }
}
