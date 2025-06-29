using System;
using System.Collections.Concurrent;
using System.IO;

namespace Question1_Async_Parallel
{
    /// <summary>
    /// Entry point for the parallel file search application.
    /// Prompts the user for a directory and filename pattern, performs the search,
    /// and writes the results to a CSV file.
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            // Prompt user for root directory
            Console.Write("Enter the root directory to search: ");
            string rootPath = Console.ReadLine();

            // Prompt user for file search pattern (e.g., *.txt)
            Console.Write("Enter the file search pattern (e.g., *.txt): ");
            string pattern = Console.ReadLine();

            // Validate input
            if (string.IsNullOrWhiteSpace(rootPath) || string.IsNullOrWhiteSpace(pattern))
            {
                Console.WriteLine("Invalid input. Exiting.");
                return;
            }

            // Check if directory exists
            if (!Directory.Exists(rootPath))
            {
                Console.WriteLine("The specified directory does not exist.");
                return;
            }

            Console.WriteLine("\nSearching for files...\n");

            // Perform parallel file search using a thread-safe collection
            ConcurrentBag<FileResult> results = FileSearcher.SearchFiles(rootPath, pattern);

            Console.WriteLine($"\nSearch complete. {results.Count} file(s) found.");

            // Output results to CSV
            string outputPath = "search_results.csv";
            try
            {
                FileSearcher.WriteToCsv(outputPath, results);
                Console.WriteLine($"Results saved to: {Path.GetFullPath(outputPath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing CSV: {ex.Message}");
            }
        }
    }
}
