using System;

namespace Question1_Async_Parallel
{
    /// <summary>
    /// Represents a single file found during the parallel file search.
    /// Contains metadata needed for reporting and CSV output.
    /// </summary>
    public class FileResult
    {
        /// <summary>
        /// The name of the file (e.g., "report.txt").
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The full absolute path to the file on the filesystem.
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// The size of the file in kilobytes (KB).
        /// </summary>
        public double SizeKB { get; set; }

        /// <summary>
        /// The timestamp of the file's last modification.
        /// </summary>
        public DateTime LastModified { get; set; }
    }
}
