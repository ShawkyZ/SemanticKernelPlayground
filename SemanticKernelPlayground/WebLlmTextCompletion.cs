using Microsoft.SemanticKernel.AI.TextCompletion;
using System.Text;
using System.Text.Json;

public class WebLlmTextCompletion : ITextCompletion
{
    private const string TextUiCompletionEndpoint = "http://localhost:5000/api/v1/generate";

    public async Task<string> CompleteAsync(string text, CompleteRequestSettings requestSettings, CancellationToken cancellationToken = default)
    {
        using var webClient = new HttpClient();
        var textGenerationUiSettings = new TextGenerationUiSettings(
            Prompt: text,
            MaxNewTokens: requestSettings.MaxTokens,
            DoSample: true,
            Temperature: requestSettings.Temperature,
            TopP: requestSettings.TopP,
            TypicalP: 1,
            EncoderRepetitionPenalty: requestSettings.FrequencyPenalty,
            RepetitionPenalty: requestSettings.PresencePenalty,
            TopK: 40,
            MinLength: 0,
            NoRepeatNgramSize: 0,
            NumBeams: 1,
            PenaltyAlpha: 0,
            LengthPenalty: 1,
            EarlyStopping: false,
            Seed: -1,
            AddBosToken: true,
            TruncationLength: 2048,
            BanEosToken: false,
            SkipSpecialTokens: false,
            StoppingStrings: requestSettings.StopSequences);

        var json = JsonSerializer.Serialize(textGenerationUiSettings, new JsonSerializerOptions
        {
            PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance
        });

        var request = new HttpRequestMessage(HttpMethod.Post, TextUiCompletionEndpoint);
        request.Headers.TryAddWithoutValidation("Content-Length", json.Length.ToString());
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await webClient.SendAsync(request);
        res.EnsureSuccessStatusCode();
        var lastres = JsonSerializer.Deserialize<TextGenerationResults>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return lastres == null ? string.Empty : lastres.Results?.FirstOrDefault()?.Text?.Replace("</s>", "") ?? string.Empty;
    }

    public async IAsyncEnumerable<string> CompleteStreamAsync(string text, CompleteRequestSettings requestSettings, CancellationToken cancellationToken = default)
    {
        using var webClient = new HttpClient();
        var textGenerationUiSettings = new TextGenerationUiSettings(
                  text,
                  requestSettings.MaxTokens,
                  false,
                  requestSettings.Temperature,
                  0.1,
                  1,
                  requestSettings.FrequencyPenalty,
                  requestSettings.PresencePenalty,
                  40,
                  0,
                  0,
                  1,
                  0,
                  0,
                  false,
                  -1,
                  true,
                  2048,
                  false,
                  false,
                  requestSettings.StopSequences);
        var json = JsonSerializer.Serialize(textGenerationUiSettings, new JsonSerializerOptions
        {
            PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance
        });

        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/v1/generate");
        request.Headers.TryAddWithoutValidation("Content-Length", json.Length.ToString());
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await webClient.SendAsync(request);
        res.EnsureSuccessStatusCode();
        var lastres = JsonSerializer.Deserialize<TextGenerationResults>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        yield return lastres == null ? string.Empty : lastres.Results?.FirstOrDefault()?.Text.Replace("</s>", "") ?? string.Empty;
    }
}
