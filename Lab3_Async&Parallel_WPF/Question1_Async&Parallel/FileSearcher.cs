using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question1_Async_Parallel
{
    public static class FileSearcher
    {
        // Recursively and concurrently collect all directories starting from rootPath
        public static ConcurrentBag<string> GetAllDirectoriesRecursive(string rootPath)
        {
            ConcurrentBag<string> result = new ConcurrentBag<string>();

            void Recurse(string path)
            {
                try
                {
                    result.Add(path); // include current directory

                    var subdirs = Directory.GetDirectories(path);

                    // Process subdirectories in parallel
                    Parallel.ForEach(subdirs, subdir =>
                    {
                        Recurse(subdir); // recurse into each subdirectory
                    });
                }
                catch (UnauthorizedAccessException)
                {
                    // skip folders with permission issues
                }
                catch (DirectoryNotFoundException)
                {
                    // skip folders that may have been deleted during traversal
                }
            }

            Recurse(rootPath);
            return result;
        }

        // Scans all collected directories for files matching the given pattern
        public static ConcurrentBag<FileResult> SearchFiles(string rootPath, string pattern)
        {
            ConcurrentBag<FileResult> results = new ConcurrentBag<FileResult>();
            ConcurrentBag<string> directories;

            try
            {
                directories = GetAllDirectoriesRecursive(rootPath); // get all directories recursively
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error collecting directories: " + ex.Message);
                return results;
            }

            // Parallel file search
            Parallel.ForEach(directories, dir =>
            {
                try
                {
                    foreach (var file in Directory.GetFiles(dir, pattern))
                    {
                       
                        FileInfo fi = new FileInfo(file);
                        results.Add(new FileResult
                        {
                            FileName = fi.Name,
                            FullPath = fi.FullName,
                            SizeKB = Math.Round(fi.Length / 1024.0, 2),
                            LastModified = fi.LastWriteTime
                        });
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"[Error accessing]: {dir} - {ex.Message}");
                }
            });

            return results;
        }

        // Writes search results to a CSV file
        public static void WriteToCsv(string outputPath, IEnumerable<FileResult> results)
        {
            using StreamWriter writer = new StreamWriter(outputPath, false, Encoding.UTF8);
            writer.WriteLine("File Name,Full Path,Size (KB),Last Modified");

            foreach (var result in results)
            {
                writer.WriteLine($"\"{result.FileName}\",\"{result.FullPath}\",{result.SizeKB},\"{result.LastModified:O}\"");
            }
        }
    }
}
