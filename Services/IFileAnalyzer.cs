using FileAnalyzer.Models;

namespace FileAnalyzer.Services;

public interface IFileAnalyzer
{
	Task<IEnumerable<FileAnalysisResult>> AnalyzeFilesAsync(IEnumerable<string> filePaths);
	Task<FileAnalysisResult> AnalyzeFileAsync(string filePath);
}