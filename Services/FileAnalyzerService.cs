using FileAnalyzer.Models;

namespace FileAnalyzer.Services;

public class FileAnalyzerService : IFileAnalyzer
{
	private readonly Mutex _resultsMutex = new();
	private readonly List<FileAnalysisResult> _results = [];

	public async Task<IEnumerable<FileAnalysisResult>> AnalyzeFilesAsync(IEnumerable<string> filePaths)
	{
		var tasks = filePaths.Select(filePath =>
			AnalyzeFileWithMutexAsync(filePath));

		await Task.WhenAll(tasks);
		return _results.AsReadOnly();
	}

	private async Task AnalyzeFileWithMutexAsync(string filePath)
	{
		var result = await AnalyzeFileAsync(filePath);

		try
		{
			_resultsMutex.WaitOne();
			_results.Add(result);
		}
		finally
		{
			_resultsMutex.ReleaseMutex();
		}
	}

	public async Task<FileAnalysisResult> AnalyzeFileAsync(string filePath)
	{
		try
		{
			string fileName = Path.GetFileName(filePath);
			string content = await File.ReadAllTextAsync(filePath);

			return new FileAnalysisResult
			{
				FileName = fileName,
				WordCount = CountWords(content),
				CharCount = content.Length,
				Success = true
			};
		}
		catch (Exception ex)
		{
			return new FileAnalysisResult
			{
				FileName = Path.GetFileName(filePath),
				Success = false,
				ErrorMessage = GetErrorMessage(ex)
			};
		}
	}

	private int CountWords(string content)
	{
		return content.Split(new[] { ' ', '\t', '\n', '\r' },
			StringSplitOptions.RemoveEmptyEntries).Length;
	}

	private string GetErrorMessage(Exception ex)
	{
		return ex switch
		{
			FileNotFoundException _ => "Файл не найден",
			DirectoryNotFoundException _ => "Директория не найдена",
			IOException _ => "Ошибка ввода/вывода",
			UnauthorizedAccessException _ => "Нет доступа к файлу",
			_ => "Неизвестная ошибка"
		};
	}
}