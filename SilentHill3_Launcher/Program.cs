using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using IniParser;
using IniParser.Model;

namespace SilentHill3Launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize the INI parser
            var parser = new FileIniDataParser();
            IniData data;

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

            try
            {
                // Read the INI file
                data = parser.ReadFile(configFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading '{configFilePath}': {ex.Message}");
                Console.WriteLine("Please ensure the INI file is correctly formatted and all paths are valid.");
                return;
            }

            // Validate the [Paths] section
            if (!data.Sections.ContainsSection("Paths"))
            {
                Console.WriteLine("The [Paths] section is missing in 'launch_config.ini'.");
                Console.WriteLine("Creating a default [Paths] section.");
                CreateDefaultIniFile(configFilePath);
                return;
            }

            var requiredKeys = new[] { "ReloadedIIPath", "SilentHill3Path", "ReloadedIIWorkingDirectory" };
            foreach (var key in requiredKeys)
            {
                if (!data["Paths"].ContainsKey(key))
                {
                    Console.WriteLine($"The key '{key}' is missing under the [Paths] section in 'launch_config.ini'.");
                    Console.WriteLine("Creating a default 'launch_config.ini' file. Please update the paths as needed.");
                    CreateDefaultIniFile(configFilePath);
                    return;
                }
            }

            // Retrieve paths from the INI file
            string reloadedIIPath = data["Paths"]["ReloadedIIPath"];
            string silentHill3Path = data["Paths"]["SilentHill3Path"];
            string reloadedIIWorkingDirectory = data["Paths"]["ReloadedIIWorkingDirectory"];

            // Validate that none of the paths are empty
            if (string.IsNullOrWhiteSpace(reloadedIIPath) ||
                string.IsNullOrWhiteSpace(silentHill3Path) ||
                string.IsNullOrWhiteSpace(reloadedIIWorkingDirectory))
            {
                Console.WriteLine("One or more paths in 'launch_config.ini' are missing or empty.");
                Console.WriteLine("Please ensure all paths are correctly specified under the [Paths] section.");
                return;
            }

            // Validate paths
            if (!File.Exists(reloadedIIPath))
            {
                Console.WriteLine($"Reloaded-II.exe not found at: {reloadedIIPath}");
                Console.WriteLine("Please verify the path in 'launch_config.ini' or reinstall Reloaded-II.");
                return;
            }

            if (!File.Exists(silentHill3Path))
            {
                Console.WriteLine($"sh3.exe not found at: {silentHill3Path}");
                Console.WriteLine("Please verify the path in 'launch_config.ini' or reinstall Silent Hill 3.");
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
                        Thread.Sleep(1000); // Check every second
                    }

                    Console.WriteLine("Silent Hill 3 has exited.");
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
    }
}
