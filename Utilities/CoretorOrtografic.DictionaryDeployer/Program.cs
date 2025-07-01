using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;

namespace CoretorOrtografic.DictionaryDeployer
{
    public class Program
    {
        private static string GetAppDataPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "CoretorOrtograficFurlan", "Dictionaries");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "CoretorOrtograficFurlan", "Dictionaries");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "CoretorOrtograficFurlan", "Dictionaries");
            }

            throw new PlatformNotSupportedException();
        }

        private static void ExtractArchive(string zipPath, string destinationFolder)
        {
            if (!File.Exists(zipPath))
            {
                Console.WriteLine($"File not found: {zipPath}. Skipping...");
                return;
            }

            Directory.CreateDirectory(destinationFolder);

            List<string> expectedFiles = new();
            try
            {
                using var archive = ZipFile.OpenRead(zipPath);
                expectedFiles.AddRange(archive.Entries.Select(e => Path.Combine(destinationFolder, e.FullName)));
            }
            catch
            {
                // Could not read archive; continue without expected file list
            }

            if (expectedFiles.Count > 0 && expectedFiles.All(File.Exists))
            {
                Console.WriteLine($"Files from {zipPath} already exist. Skipping extraction.");
                return;
            }

            string executable;
            string arguments = $"x -o\"{destinationFolder}\" \"{zipPath}\"";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                executable = "C:\\Program Files\\7-Zip\\7z.exe";
            }
            else
            {
                executable = "7z";
            }

            if (!File.Exists(executable) && executable != "7z")
            {
                Console.WriteLine("7-Zip is not installed. Please install 7-Zip to continue.");
                return;
            }

            var process = Process.Start(new ProcessStartInfo
            {
                FileName = executable,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            });

            process?.WaitForExit();
        }

        public static void Main(string[] args)
        {
            try
            {
                string appDataFolder = GetAppDataPath();
                Console.WriteLine($"Deploying dictionaries to {appDataFolder}");

                string basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "Dictionaries"));

                var zipFiles = new List<string>
                {
                    Path.Combine(basePath, "Elisions", "elisions.zip"),
                    Path.Combine(basePath, "Errors", "errors.zip"),
                    Path.Combine(basePath, "Frec", "frequencies.zip"),
                    Path.Combine(basePath, "WordsDatabase", "words_split.zip"),
                    Path.Combine(basePath, "WordsRadixTree", "words_split.zip")
                };

                foreach (var zipFile in zipFiles)
                {
                    ExtractArchive(zipFile, appDataFolder);
                }

                Console.WriteLine($"All files extracted to {appDataFolder}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
        }
    }
}
