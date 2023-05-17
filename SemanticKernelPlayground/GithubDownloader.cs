using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.TextCompletion;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.Skills.Web;
using SemanticKernelPlayground.RepoUtils;
using SemanticKernelPlayground.Skills;

public class GithubDownloader
{
    public static async Task Run()
    {
        ITextCompletion CompletionFactory(IKernel k) => new WebLlmTextCompletion();

        var kernel = Kernel.Builder.WithLogger(ConsoleLogger.Log)
            .Configure(c => {
                c.AddTextCompletionService(CompletionFactory);
                c.AddTextEmbeddingGenerationService
                })
            .Build();
        string skillsFolder = RepoFiles.SkillsPath();

        var githubSkill = kernel.ImportSkill(new GitHubSkill(kernel, new WebFileDownloadSkill()), nameof(GitHubSkill));
        var contextVariables1 = new ContextVariables();
        contextVariables1.Set("repositoryBranch", "main");
        contextVariables1.Set("INPUT", "https://github.com/microsoft/semantic-kernel");

        var ress = await kernel.RunAsync(contextVariables1, githubSkill["SummarizeRepository"]);
     
    }
}
