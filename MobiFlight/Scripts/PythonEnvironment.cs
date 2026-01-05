using System;
using System.IO;
using System.IO.Compression;

namespace MobiFlight.Scripts
{
    /// <summary>
    /// Manages the Python runtime environment initialization
    /// </summary>
    internal class PythonEnvironment
    {
        public const string PYTHON_BASE_FOLDER = "Python";
        public const string PYTHON_RUNTIME_FOLDER = "3.14.2";

        /// <summary>
        /// Initializes the Python runtime by extracting archived files if needed
        /// </summary>
        public static void Initialize()
        {
            // Get Python paths from settings
            string pythonBaseFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PYTHON_BASE_FOLDER);
            string pythonRuntimeFolder = Path.Combine(pythonBaseFolder, PYTHON_RUNTIME_FOLDER);

            // Check if Python runtime is already extracted
            if (Directory.Exists(pythonRuntimeFolder))
            {
                Log.Instance.log("Python runtime already initialized.", LogSeverity.Debug);
                return;
            }

            // Ensure Python base folder exists
            if (!Directory.Exists(pythonBaseFolder))
            {
                Log.Instance.log($"Python folder not found: {pythonBaseFolder}", LogSeverity.Warn);
                return;
            }

            try
            {
                // Find all .zip files in Python folder
                string[] zipFiles = Directory.GetFiles(pythonBaseFolder, "*.zip", SearchOption.TopDirectoryOnly);

                if (zipFiles.Length == 0)
                {
                    Log.Instance.log("No Python runtime .zip files found to extract.", LogSeverity.Warn);
                    return;
                }

                Log.Instance.log($"Found {zipFiles.Length} Python runtime archive(s) to extract.", LogSeverity.Info);

                // Extract all .zip files
                foreach (string zipFile in zipFiles)
                {
                    try
                    {
                        Log.Instance.log($"Extracting Python runtime: {Path.GetFileName(zipFile)}", LogSeverity.Info);
                        ZipFile.ExtractToDirectory(zipFile, pythonBaseFolder);
                        Log.Instance.log($"Successfully extracted: {Path.GetFileName(zipFile)}", LogSeverity.Info);
                    }
                    catch (Exception ex)
                    {
                        Log.Instance.log($"Failed to extract {Path.GetFileName(zipFile)}: {ex.Message}", LogSeverity.Error);
                    }
                }

                // Verify extraction was successful
                if (Directory.Exists(pythonRuntimeFolder))
                {
                    Log.Instance.log("Python runtime successfully initialized.", LogSeverity.Info);
                }
                else
                {
                    Log.Instance.log("Python runtime folder not found after extraction. Archive may not contain expected structure.", LogSeverity.Warn);
                }
            }
            catch (Exception ex)
            {
                Log.Instance.log($"Error initializing Python runtime: {ex.Message}", LogSeverity.Error);
            }
        }
    }
}
