using System.Diagnostics;

namespace SilentHill3Launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the INI configuration file
            string configFilePath = "launch_config.ini";

            // Check if the launch_config.ini file exists
            if (!File.Exists(configFilePath))
            {
                Console.WriteLine($"Configuration file '{configFilePath}' not found.");
                Console.WriteLine("Creating a default 'launch_config.ini' file. Please update the paths as needed.");
                CreateDefaultIniFile(configFilePath);
                return;
            }

            // Retrieve paths from the INI file
            var paths = ParseIniFile(configFilePath);

            // Validate that paths are available
            if (paths == null || !paths.ContainsKey("ReloadedIIPath") || !paths.ContainsKey("SilentHill3Path") || !paths.ContainsKey("ReloadedIIWorkingDirectory"))
            {
                Console.WriteLine("One or more paths are missing in 'launch_config.ini'.");
                Console.WriteLine("Creating a default 'launch_config.ini' file. Please update the paths as needed.");
                CreateDefaultIniFile(configFilePath);
                return;
            }

            string reloadedIIPath = paths["ReloadedIIPath"];
            string silentHill3Path = paths["SilentHill3Path"];
            string reloadedIIWorkingDirectory = paths["ReloadedIIWorkingDirectory"];

            // Validate that none of the paths are empty
            if (string.IsNullOrWhiteSpace(reloadedIIPath) ||
                string.IsNullOrWhiteSpace(silentHill3Path) ||
                string.IsNullOrWhiteSpace(reloadedIIWorkingDirectory))
            {
                Console.WriteLine("One or more paths in 'launch_config.ini' are missing or empty.");
                return;
            }

            // Validate paths
            if (!File.Exists(reloadedIIPath))
            {
                Console.WriteLine($"Reloaded-II.exe not found at: {reloadedIIPath}");
                return;
            }

            if (!File.Exists(silentHill3Path))
            {
                Console.WriteLine($"sh3.exe not found at: {silentHill3Path}");
                return;
            }

            // Start the Reloaded-II launcher with arguments
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = reloadedIIPath,
                Arguments = $"--launch \"{silentHill3Path}\"",
                WorkingDirectory = reloadedIIWorkingDirectory,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                Console.WriteLine("Waiting for Reloaded-II to launch Silent Hill 3...");
                using (Process reloadedProcess = Process.Start(startInfo))
                {
                    // Optionally, wait for Reloaded-II to finish initializing
                    Thread.Sleep(5000); // Wait for 5 seconds

                    Console.WriteLine("Silent Hill 3 is running...");

                    // Monitor the sh3.exe process
                    while (IsProcessRunning("sh3"))
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while launching the game: " + ex.Message);
            }
        }

        static bool IsProcessRunning(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            return processes.Length > 0;
        }

        static void CreateDefaultIniFile(string path)
        {
            var defaultIniContent = @"
[Paths]
ReloadedIIPath = C:\SILENT HILL 3\Reloaded-II\Reloaded-II.exe
SilentHill3Path = C:\SILENT HILL 3\sh3.exe
ReloadedIIWorkingDirectory = C:\SILENT HILL 3\Reloaded-II
".TrimStart();

            try
            {
                File.WriteAllText(path, defaultIniContent);
                Console.WriteLine($"Default configuration file created at '{path}'. Please update the paths as needed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create default INI file: {ex.Message}");
            }
        }

        static Dictionary<string, string> ParseIniFile(string filePath)
        {
            var paths = new Dictionary<string, string>();

            try
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    if (line.StartsWith("[") || string.IsNullOrWhiteSpace(line))
                        continue;

                    var keyValue = line.Split(new[] { '=' }, 2);
                    if (keyValue.Length == 2)
                    {
                        paths[keyValue[0].Trim()] = keyValue[1].Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading '{filePath}': {ex.Message}");
                return null;
            }

            return paths;
        }
    }
}
