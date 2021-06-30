using System;
using System.IO;
using System.Text.Json;
using RedBookPlayer.GUI;

namespace RedBookPlayer
{
    public class Settings
    {
        /// <summary>
        /// Indicates if discs should start playing on load
        /// </summary>
        public bool AutoPlay { get; set; } = false;

        /// <summary>
        /// Indicates if an index change can trigger a track change
        /// </summary>
        public bool IndexButtonChangeTrack { get; set; } = false;

        /// <summary>
        /// Indicates if the index 0 of track 1 is treated like a hidden track
        /// </summary>
        public bool AllowSkipHiddenTrack { get; set; } = false;

        /// <summary>
        /// Indicates if data tracks should be played like old, non-compliant players
        /// </summary>
        public bool PlayDataTracks { get; set; } = false;

        /// <summary>
        /// Generate a TOC if the disc is missing one
        /// </summary>
        public bool GenerateMissingTOC { get; set; } = true;

        /// <summary>
        /// Indicates the default playback volume
        /// </summary>
        public int Volume { get; set; } = 100;

        /// <summary>
        /// Indicates the currently selected theme
        /// </summary>
        public string SelectedTheme { get; set; } = "default";

        /// <summary>
        /// Path to the settings file
        /// </summary>
        private string _filePath;

        public Settings() {}

        public Settings(string filePath) => _filePath = filePath;

        /// <summary>
        /// Load settings from a file
        /// </summary>
        /// <param name="filePath">Path to the settings JSON file</param>
        /// <returns>Settings derived from the input file, if possible</returns>
        public static Settings Load(string filePath)
        {
            if(File.Exists(filePath))
            {
                try
                {
                    Settings settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(filePath));
                    settings._filePath = filePath;

                    MainWindow.ApplyTheme(settings.SelectedTheme);

                    return settings;
                }
                catch(JsonException)
                {
                    Console.WriteLine("Couldn't parse settings, reverting to default");

                    return new Settings(filePath);
                }
            }

            return new Settings(filePath);
        }

        /// <summary>
        /// Save settings to a file
        /// </summary>
        public void Save()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(_filePath, json);
        }
    }
}