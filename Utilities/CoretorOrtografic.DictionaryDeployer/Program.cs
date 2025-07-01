using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using SharpCompress.Archives;
using SharpCompress.Common;

namespace CoretorOrtografic.DictionaryDeployer
{
    public class Program
    {
        private static string GetAppDataPath() =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                         "CoretorOrtograficFurlan", "Dictionaries");

        public static void Main(string[] args)
        {
            try
            {
                string appDataFolder = GetAppDataPath();
                Console.WriteLine($"Deploying dictionaries to {appDataFolder}");

                string basePath = Path.Combine(AppContext.BaseDirectory, "Dictionaries");

                foreach (var zipFile in GetZipFiles(basePath))
                    ExtractArchive(zipFile, appDataFolder);

                if (Environment.ExitCode == 0)
                    Console.WriteLine("All dictionaries deployed successfully.");
                else
                    Console.WriteLine("Deployment completed with errors.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unhandled error: {e.Message}");
                Environment.ExitCode = 1;
            }
        }

        private static IEnumerable<string> GetZipFiles(string basePath) =>
            new[]
            {
                ("Elisions",      "elisions.zip"),
                ("Errors",        "errors.zip"),
                ("Frec",          "frequencies.zip"),
                ("WordsDatabase", "words_split.zip"),
                ("WordsRadixTree","words_split.zip")
            }.Select(t => Path.Combine(basePath, t.Item1, t.Item2));

        private static void ExtractArchive(string zipPath, string destinationFolder)
        {
            if (!File.Exists(zipPath))
            {
                Console.WriteLine($"ERROR: File not found: {zipPath}");
                Environment.ExitCode = 1;
                return;
            }

            Directory.CreateDirectory(destinationFolder);

            if (ArchiveAlreadyPresent(zipPath, destinationFolder))
            {
                Console.WriteLine($"Already extracted: {Path.GetFileName(zipPath)} - skipping.");
                return;
            }

            if (TryExtractWith7Zip(zipPath, destinationFolder)) return;
            if (TryExtractWithDotNet(zipPath, destinationFolder)) return;
            if (TryExtractWithSharpCompress(zipPath, destinationFolder)) return;

            Console.WriteLine($"ERROR: Unable to extract {zipPath}");
            Environment.ExitCode = 1;
        }

        private static bool ArchiveAlreadyPresent(string zipPath, string destinationFolder)
        {
            try
            {
                using var archive = ZipFile.OpenRead(zipPath);
                return archive.Entries
                              .Select(e => Path.Combine(destinationFolder, e.FullName))
                              .All(File.Exists);
            }
            catch
            {
                return false;
            }
        }

        private static bool TryExtractWith7Zip(string zipPath, string destinationFolder)
        {
            string sevenZip = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? @"C:\Program Files\7-Zip\7z.exe"
                : null;

            if (sevenZip is null || !File.Exists(sevenZip))
                return false;

            var process = Process.Start(new ProcessStartInfo
            {
                FileName = sevenZip,
                Arguments = $"x -aoa -o\"{destinationFolder}\" \"{zipPath}\"",
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            });

            process?.WaitForExit();
            if (process?.ExitCode == 0)
            {
                Console.WriteLine($"Extracted {Path.GetFileName(zipPath)} with 7-Zip.");
                return true;
            }

            Console.WriteLine($"7-Zip failed with exit code {process?.ExitCode}.");
            return false;
        }

        private static bool TryExtractWithDotNet(string zipPath, string destinationFolder)
        {
            try
            {
                ZipFile.ExtractToDirectory(zipPath, destinationFolder, overwriteFiles: true);
                Console.WriteLine($"Extracted {Path.GetFileName(zipPath)} with ZipFile.");
                return true;
            }
            catch (InvalidDataException ex)
                when (ex.Message.Contains("split", StringComparison.OrdinalIgnoreCase))
            {
                // split archives are not supported by ZipFile
                return false;
            }
            catch
            {
                return false;
            }
        }

        private static bool TryExtractWithSharpCompress(string zipPath, string destinationFolder)
        {
            try
            {
                using var archive = ArchiveFactory.Open(zipPath);
                foreach (var entry in archive.Entries.Where(e => !e.IsDirectory))
                {
                    entry.WriteToDirectory(destinationFolder, new ExtractionOptions
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }

                Console.WriteLine($"Extracted {Path.GetFileName(zipPath)} with SharpCompress.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SharpCompress failed: {ex.Message}");
                return false;
            }
        }
    }
}
