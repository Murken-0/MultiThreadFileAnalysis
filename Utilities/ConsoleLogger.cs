using FileAnalyzer.Models;

namespace FileAnalyzer.Utilities;

public static class ConsoleLogger
{
	public static void PrintResults(IEnumerable<FileAnalysisResult> results)
	{
		Console.WriteLine("\nРезультаты анализа:");

		int index = 1;
		foreach (var result in results)
		{
			Console.WriteLine($"{index++}. {result}");
		}

		if (results.Any(r => r.Success))
		{
			int totalWords = results.Where(r => r.Success).Sum(r => r.WordCount);
			int totalChars = results.Where(r => r.Success).Sum(r => r.CharCount);
			Console.WriteLine($"\nИтог: {totalWords} слов, {totalChars} символов.");
		}
	}

	public static void PrintHelp()
	{
		Console.WriteLine("Использование:");
		Console.WriteLine("  FileAnalyzer [файл1] [файл2] ... [файлN]");
		Console.WriteLine("  FileAnalyzer (без параметров - анализирует все .txt файлы в текущей директории)");
		Console.WriteLine("\nПример:");
		Console.WriteLine("  FileAnalyzer document.txt notes.txt");
	}
}