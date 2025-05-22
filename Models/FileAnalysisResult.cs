namespace FileAnalyzer.Models;

public class FileAnalysisResult
{
	public string FileName { get; set; }
	public int WordCount { get; set; }
	public int CharCount { get; set; }
	public bool Success { get; set; }
	public string ErrorMessage { get; set; }

	public override string ToString()
	{
		return Success
			? $"{FileName}: {WordCount} слов, {CharCount} символов"
			: $"{FileName}: ошибка - {ErrorMessage}";
	}
}