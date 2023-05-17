public record TextGenerationUiSettings(string Prompt, int MaxNewTokens, bool DoSample, double Temperature, double TopP, 
    double TypicalP, double EncoderRepetitionPenalty, double RepetitionPenalty, int TopK, 
    int MinLength, int NoRepeatNgramSize, int NumBeams, int PenaltyAlpha, double LengthPenalty, bool EarlyStopping, int Seed, bool AddBosToken, 
    int TruncationLength, bool BanEosToken, bool SkipSpecialTokens, IList<string> StoppingStrings);
public record TextGenerationResults(IEnumerable<TextGenerationResultText> Results);
public record TextGenerationResultText(string Text);
