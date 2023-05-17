using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.TextCompletion;
using Microsoft.SemanticKernel.Orchestration;
using SemanticKernelPlayground.RepoUtils;

public class CodeAnalyzer
{
    public static async Task Run()
    {
        ITextCompletion CompletionFactory(IKernel k) => new WebLlmTextCompletion();

        var kernel = Kernel.Builder.WithLogger(ConsoleLogger.Log)
            .Configure(c => c.AddTextCompletionService(CompletionFactory))
            .Build();
        string skillsFolder = RepoFiles.SkillsPath();


        var secAnalysisSkill = kernel.ImportSemanticSkillFromDirectory(skillsFolder, "SecurityScannerSkill");
       
        string history = "";
        while (true)
        {
            Console.Write("User: ");
            var ask = Console.ReadLine();


            var contextVariables = new ContextVariables();
            contextVariables.Set("input", @"public void VulnerableFunction(string query) {
    // Execute the provided SQL query
    SqlConnection conn = new SqlConnection(""connection string"");
    SqlCommand cmd = new SqlCommand(query, conn);
    conn.Open();
    SqlDataReader reader = cmd.ExecuteReader();
    while (reader.Read()) {
        Console.WriteLine(reader[0]);
    }
    reader.Close();
    conn.Close();
}");
            contextVariables.Set("user", "User: ");
            contextVariables.Set("bot", "AI Assistant: ");
            contextVariables.Set("history", history);
            var res = await kernel.RunAsync(
                contextVariables,
                secAnalysisSkill["Analyzer"]
            );
            var botResponse = $"AI Assistant: {res.Result.Trim()}\n";
            var userRequest = $"User: {ask}\n";

            history += userRequest;
            history += botResponse;
            Console.WriteLine(botResponse);

        }
    }
}
