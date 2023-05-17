public class Program
{
    public static async Task Main(string[] args)
    {
        await GithubDownloader.Run();
    }
}
