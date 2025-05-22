using FileAnalyzer.Services;
using FileAnalyzer.Utilities;
using System.Text;

namespace FileAnalyzer;

class Program
{
	static async Task Main(string[] args)
	{
		Console.OutputEncoding = Encoding.UTF8;
		Console.InputEncoding = Encoding.UTF8;

		try
		{
			if (args.Contains("--help") || args.Contains("-h"))
			{
				ConsoleLogger.PrintHelp();
				return;
			}

			var fileArgs = args.Where(arg => !arg.StartsWith('-')).ToArray();
			var filePaths = GetFilePaths(fileArgs);

			if (filePaths.Length == 0)
			{
				Console.WriteLine("Не указаны файлы для анализа. Используйте --help для справки.");
				return;
			}

			Console.WriteLine($"Начинаем анализ {filePaths.Length} файлов...");

			IFileAnalyzer analyzer = new FileAnalyzerService();
			var results = await analyzer.AnalyzeFilesAsync(filePaths);

			ConsoleLogger.PrintResults(results);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Критическая ошибка: {ex.Message}");
		}
	}

	private static string[] GetFilePaths(string[] args)
	{
		if (args.Length > 0) return args;

		return Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt");
	}
}