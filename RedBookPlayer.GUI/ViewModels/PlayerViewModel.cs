using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using ReactiveUI;
using RedBookPlayer.GUI.Views;
using RedBookPlayer.Models.Hardware;

namespace RedBookPlayer.GUI.ViewModels
{
    public class PlayerViewModel : ReactiveObject
    {
        /// <summary>
        /// Player representing the internal state
        /// </summary>
        private Player _player;

        /// <summary>
        /// Set of images representing the digits for the UI
        /// </summary>
        private Image[] _digits;

        #region Player Passthrough

        #region OpticalDisc Passthrough

        /// <summary>
        /// Current track number
        /// </summary>
        public int CurrentTrackNumber
        {
            get => _currentTrackNumber;
            private set => this.RaiseAndSetIfChanged(ref _currentTrackNumber, value);
        }

        /// <summary>
        /// Current track index
        /// </summary>
        public ushort CurrentTrackIndex
        {
            get => _currentTrackIndex;
            private set => this.RaiseAndSetIfChanged(ref _currentTrackIndex, value);
        }

        /// <summary>
        /// Current sector number
        /// </summary>
        public ulong CurrentSector
        {
            get => _currentSector;
            private set => this.RaiseAndSetIfChanged(ref _currentSector, value);
        }

        /// <summary>
        /// Represents the sector starting the section
        /// </summary>
        public ulong SectionStartSector
        {
            get => _sectionStartSector;
            private set => this.RaiseAndSetIfChanged(ref _sectionStartSector, value);
        }

        /// <summary>
        /// Represents if the disc has a hidden track
        /// </summary>
        public bool HiddenTrack
        {
            get => _hasHiddenTrack;
            private set => this.RaiseAndSetIfChanged(ref _hasHiddenTrack, value);
        }

        /// <summary>
        /// Represents the 4CH flag [CompactDisc only]
        /// </summary>
        public bool QuadChannel
        {
            get => _quadChannel;
            private set => this.RaiseAndSetIfChanged(ref _quadChannel, value);
        }

        /// <summary>
        /// Represents the DATA flag [CompactDisc only]
        /// </summary>
        public bool IsDataTrack
        {
            get => _isDataTrack;
            private set => this.RaiseAndSetIfChanged(ref _isDataTrack, value);
        }

        /// <summary>
        /// Represents the DCP flag [CompactDisc only]
        /// </summary>
        public bool CopyAllowed
        {
            get => _copyAllowed;
            private set => this.RaiseAndSetIfChanged(ref _copyAllowed, value);
        }

        /// <summary>
        /// Represents the PRE flag [CompactDisc only]
        /// </summary>
        public bool TrackHasEmphasis
        {
            get => _trackHasEmphasis;
            private set => this.RaiseAndSetIfChanged(ref _trackHasEmphasis, value);
        }

        /// <summary>
        /// Represents the total tracks on the disc
        /// </summary>
        public int TotalTracks => _player.TotalTracks;

        /// <summary>
        /// Represents the total indices on the disc
        /// </summary>
        public int TotalIndexes => _player.TotalIndexes;

        /// <summary>
        /// Total sectors in the image
        /// </summary>
        public ulong TotalSectors => _player.TotalSectors;

        /// <summary>
        /// Represents the time adjustment offset for the disc
        /// </summary>
        public ulong TimeOffset => _player.TimeOffset;

        /// <summary>
        /// Represents the total playing time for the disc
        /// </summary>
        public ulong TotalTime => _player.TotalTime;

        private int _currentTrackNumber;
        private ushort _currentTrackIndex;
        private ulong _currentSector;
        private ulong _sectionStartSector;

        private bool _hasHiddenTrack;
        private bool _quadChannel;
        private bool _isDataTrack;
        private bool _copyAllowed;
        private bool _trackHasEmphasis;

        #endregion

        #region SoundOutput Passthrough

        /// <summary>
        /// Indicate if the model is ready to be used
        /// </summary>
        public bool Initialized => _player?.Initialized ?? false;

        /// <summary>
        /// Indicate if the output is playing
        /// </summary>
        public bool? Playing
        {
            get => _playing;
            private set => this.RaiseAndSetIfChanged(ref _playing, value);
        }

        /// <summary>
        /// Indicates if de-emphasis should be applied
        /// </summary>
        public bool ApplyDeEmphasis
        {
            get => _applyDeEmphasis;
            private set => this.RaiseAndSetIfChanged(ref _applyDeEmphasis, value);
        }

        /// <summary>
        /// Current playback volume
        /// </summary>
        public int Volume
        {
            get => _volume;
            private set => this.RaiseAndSetIfChanged(ref _volume, value);
        }

        private bool? _playing;
        private bool _applyDeEmphasis;
        private int _volume;

        #endregion

        #endregion

        #region Commands

        /// <summary>
        /// Command for loading a disc
        /// </summary>
        public ReactiveCommand<Unit, Unit> LoadCommand { get; }

        #region Playback

        /// <summary>
        /// Command for beginning playback
        /// </summary>
        public ReactiveCommand<Unit, Unit> PlayCommand { get; }

        /// <summary>
        /// Command for pausing playback
        /// </summary>
        public ReactiveCommand<Unit, Unit> PauseCommand { get; }

        /// <summary>
        /// Command for pausing playback
        /// </summary>
        public ReactiveCommand<Unit, Unit> TogglePlayPauseCommand { get; }

        /// <summary>
        /// Command for stopping playback
        /// </summary>
        public ReactiveCommand<Unit, Unit> StopCommand { get; }

        /// <summary>
        /// Command for moving to the next track
        /// </summary>
        public ReactiveCommand<Unit, Unit> NextTrackCommand { get; }

        /// <summary>
        /// Command for moving to the previous track
        /// </summary>
        public ReactiveCommand<Unit, Unit> PreviousTrackCommand { get; }

        /// <summary>
        /// Command for moving to the next index
        /// </summary>
        public ReactiveCommand<Unit, Unit> NextIndexCommand { get; }

        /// <summary>
        /// Command for moving to the previous index
        /// </summary>
        public ReactiveCommand<Unit, Unit> PreviousIndexCommand { get; }

        /// <summary>
        /// Command for fast forwarding
        /// </summary>
        public ReactiveCommand<Unit, Unit> FastForwardCommand { get; }

        /// <summary>
        /// Command for rewinding
        /// </summary>
        public ReactiveCommand<Unit, Unit> RewindCommand { get; }

        #endregion

        #region Volume

        /// <summary>
        /// Command for incrementing volume
        /// </summary>
        public ReactiveCommand<Unit, Unit> VolumeUpCommand { get; }

        /// <summary>
        /// Command for decrementing volume
        /// </summary>
        public ReactiveCommand<Unit, Unit> VolumeDownCommand { get; }

        /// <summary>
        /// Command for toggling mute
        /// </summary>
        public ReactiveCommand<Unit, Unit> ToggleMuteCommand { get; }

        #endregion

        #region Emphasis

        /// <summary>
        /// Command for enabling de-emphasis
        /// </summary>
        public ReactiveCommand<Unit, Unit> EnableDeEmphasisCommand { get; }

        /// <summary>
        /// Command for disabling de-emphasis
        /// </summary>
        public ReactiveCommand<Unit, Unit> DisableDeEmphasisCommand { get; }

        /// <summary>
        /// Command for toggling de-emphasis
        /// </summary>
        public ReactiveCommand<Unit, Unit> ToggleDeEmphasisCommand { get; }

        #endregion

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public PlayerViewModel()
        {
            LoadCommand = ReactiveCommand.Create(ExecuteLoad);

            PlayCommand = ReactiveCommand.Create(ExecutePlay);
            PauseCommand = ReactiveCommand.Create(ExecutePause);
            TogglePlayPauseCommand = ReactiveCommand.Create(ExecuteTogglePlayPause);
            StopCommand = ReactiveCommand.Create(ExecuteStop);
            NextTrackCommand = ReactiveCommand.Create(ExecuteNextTrack);
            PreviousTrackCommand = ReactiveCommand.Create(ExecutePreviousTrack);
            NextIndexCommand = ReactiveCommand.Create(ExecuteNextIndex);
            PreviousIndexCommand = ReactiveCommand.Create(ExecutePreviousIndex);
            FastForwardCommand = ReactiveCommand.Create(ExecuteFastForward);
            RewindCommand = ReactiveCommand.Create(ExecuteRewind);

            VolumeUpCommand = ReactiveCommand.Create(ExecuteVolumeUp);
            VolumeDownCommand = ReactiveCommand.Create(ExecuteVolumeDown);
            ToggleMuteCommand = ReactiveCommand.Create(ExecuteToggleMute);

            EnableDeEmphasisCommand = ReactiveCommand.Create(ExecuteEnableDeEmphasis);
            DisableDeEmphasisCommand = ReactiveCommand.Create(ExecuteDisableDeEmphasis);
            ToggleDeEmphasisCommand = ReactiveCommand.Create(ExecuteToggleDeEmphasis);
        }

        /// <summary>
        /// Initialize the view model with a given image path
        /// </summary>
        /// <param name="path">Path to the disc image</param>
        /// <param name="generateMissingToc">Generate a TOC if the disc is missing one [CompactDisc only]</param>
        /// <param name="loadHiddenTracks">Load hidden tracks for playback [CompactDisc only]</param>
        /// <param name="loadDataTracks">Load data tracks for playback [CompactDisc only]</param>
        /// <param name="autoPlay">True if playback should begin immediately, false otherwise</param>
        /// <param name="defaultVolume">Default volume between 0 and 100 to use when starting playback</param>
        public void Init(string path, bool generateMissingToc, bool loadHiddenTracks, bool loadDataTracks, bool autoPlay, int defaultVolume)
        {
            // Stop current playback, if necessary
            if(Playing != null) ExecuteStop();

            // Create and attempt to initialize new Player
            _player = new Player(path, generateMissingToc, loadHiddenTracks, loadDataTracks, autoPlay, defaultVolume);
            if(Initialized)
            {
                _player.PropertyChanged += PlayerStateChanged;
                PlayerStateChanged(this, null);
            }
        }

        #region Playback

        /// <summary>
        /// Begin playback
        /// </summary>
        public void ExecutePlay() => _player?.Play();

        /// <summary>
        /// Pause current playback
        /// </summary>
        public void ExecutePause() => _player?.Pause();

        /// <summary>
        /// Toggle playback
        /// </summary>
        public void ExecuteTogglePlayPause() => _player?.TogglePlayback();

        /// <summary>
        /// Stop current playback
        /// </summary>
        public void ExecuteStop() => _player?.Stop();

        /// <summary>
        /// Move to the next playable track
        /// </summary>
        public void ExecuteNextTrack() => _player?.NextTrack();

        /// <summary>
        /// Move to the previous playable track
        /// </summary>
        public void ExecutePreviousTrack() => _player?.PreviousTrack();

        /// <summary>
        /// Move to the next index
        /// </summary>
        public void ExecuteNextIndex() => _player?.NextIndex(App.Settings.IndexButtonChangeTrack);

        /// <summary>
        /// Move to the previous index
        /// </summary>
        public void ExecutePreviousIndex() => _player?.PreviousIndex(App.Settings.IndexButtonChangeTrack);

        /// <summary>
        /// Fast-forward playback by 75 sectors, if possible
        /// </summary>
        public void ExecuteFastForward() => _player?.FastForward();

        /// <summary>
        /// Rewind playback by 75 sectors, if possible
        /// </summary>
        public void ExecuteRewind() => _player?.Rewind();

        #endregion

        #region Volume

        /// <summary>
        /// Increment the volume value
        /// </summary>
        public void ExecuteVolumeUp() => _player?.VolumeUp();

        /// <summary>
        /// Decrement the volume value
        /// </summary>
        public void ExecuteVolumeDown() => _player?.VolumeDown();

        /// <summary>
        /// Set the value for the volume
        /// </summary>
        /// <param name="volume">New volume value</param>
        public void ExecuteSetVolume(int volume) => _player?.SetVolume(volume);

        /// <summary>
        /// Temporarily mute playback
        /// </summary>
        public void ExecuteToggleMute() => _player?.ToggleMute();

        #endregion

        #region Emphasis

        /// <summary>
        /// Enable de-emphasis
        /// </summary>
        public void ExecuteEnableDeEmphasis() => _player?.EnableDeEmphasis();

        /// <summary>
        /// Disable de-emphasis
        /// </summary>
        public void ExecuteDisableDeEmphasis() => _player?.DisableDeEmphasis();

        /// <summary>
        /// Toggle de-emphasis
        /// </summary>
        public void ExecuteToggleDeEmphasis() => _player?.ToggleDeEmphasis();

        #endregion

        #region Helpers

        /// <summary>
        /// Load a disc image from a selection box
        /// </summary>
        public async void ExecuteLoad()
        {
            string path = await GetPath();
            if(path == null)
                return;

            await LoadImage(path);
        }

        /// <summary>
        /// Initialize the displayed digits array
        /// </summary>
        public void InitializeDigits()
        {
            PlayerView playerView = MainWindow.Instance.ContentControl.Content as PlayerView;

            _digits = new Image[]
            {
                playerView.FindControl<Image>("TrackDigit1"),
                playerView.FindControl<Image>("TrackDigit2"),

                playerView.FindControl<Image>("IndexDigit1"),
                playerView.FindControl<Image>("IndexDigit2"),

                playerView.FindControl<Image>("TimeDigit1"),
                playerView.FindControl<Image>("TimeDigit2"),
                playerView.FindControl<Image>("TimeDigit3"),
                playerView.FindControl<Image>("TimeDigit4"),
                playerView.FindControl<Image>("TimeDigit5"),
                playerView.FindControl<Image>("TimeDigit6"),

                playerView.FindControl<Image>("TotalTracksDigit1"),
                playerView.FindControl<Image>("TotalTracksDigit2"),

                playerView.FindControl<Image>("TotalIndexesDigit1"),
                playerView.FindControl<Image>("TotalIndexesDigit2"),

                playerView.FindControl<Image>("TotalTimeDigit1"),
                playerView.FindControl<Image>("TotalTimeDigit2"),
                playerView.FindControl<Image>("TotalTimeDigit3"),
                playerView.FindControl<Image>("TotalTimeDigit4"),
                playerView.FindControl<Image>("TotalTimeDigit5"),
                playerView.FindControl<Image>("TotalTimeDigit6"),
            };
        }

        /// <summary>
        /// Load an image from the path
        /// </summary>
        /// <param name="path">Path to the image to load</param>
        public async Task<bool> LoadImage(string path)
        {
            return await Dispatcher.UIThread.InvokeAsync(() =>
            {
                Init(path, App.Settings.GenerateMissingTOC, App.Settings.PlayHiddenTracks, App.Settings.PlayDataTracks, App.Settings.AutoPlay, App.Settings.Volume);
                if(Initialized)
                    MainWindow.Instance.Title = "RedBookPlayer - " + path.Split('/').Last().Split('\\').Last();

                return Initialized;
            });
        }

        /// <summary>
        /// Set the value for loading data tracks [CompactDisc only]
        /// </summary>
        /// <param name="load">True to enable loading data tracks, false otherwise</param>
        public void SetLoadDataTracks(bool load) => _player?.SetLoadDataTracks(load);

        /// <summary>
        /// Set the value for loading hidden tracks [CompactDisc only]
        /// </summary>
        /// <param name="load">True to enable loading hidden tracks, false otherwise</param>
        public void SetLoadHiddenTracks(bool load) => _player?.SetLoadHiddenTracks(load);

        /// <summary>
        /// Generate the digit string to be interpreted by the frontend
        /// </summary>
        /// <returns>String representing the digits for the frontend</returns>
        private string GenerateDigitString()
        {
            // If the disc isn't initialized, return all '-' characters
            if(Initialized != true)
                return string.Empty.PadLeft(20, '-');

            int usableTrackNumber = CurrentTrackNumber;
            if(usableTrackNumber < 0)
                usableTrackNumber = 0;
            else if(usableTrackNumber > 99)
                usableTrackNumber = 99;

            // Otherwise, take the current time into account
            ulong sectorTime = GetCurrentSectorTime();

            int[] numbers = new int[]
            {
                usableTrackNumber,
                CurrentTrackIndex,

                (int)(sectorTime / (75 * 60)),
                (int)(sectorTime / 75 % 60),
                (int)(sectorTime % 75),

                TotalTracks,
                TotalIndexes,

                (int)(TotalTime / (75 * 60)),
                (int)(TotalTime / 75 % 60),
                (int)(TotalTime % 75),
            };

            return string.Join("", numbers.Select(i => i.ToString().PadLeft(2, '0').Substring(0, 2)));
        }

        /// <summary>
        /// Load the png image for a given character based on the theme
        /// </summary>
        /// <param name="character">Character to load the image for</param>
        /// <returns>Bitmap representing the loaded image</returns>
        private Bitmap GetBitmap(char character)
        {
            try
            {
                if(App.Settings.SelectedTheme == "default")
                {
                    IAssetLoader assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

                    return new Bitmap(assets.Open(new Uri($"avares://RedBookPlayer/Assets/{character}.png")));
                }
                else
                {
                    string themeDirectory = $"{Directory.GetCurrentDirectory()}/themes/{App.Settings.SelectedTheme}";
                    using FileStream stream = File.Open($"{themeDirectory}/{character}.png", FileMode.Open);
                    return new Bitmap(stream);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get current sector time, accounting for offsets
        /// </summary>
        /// <returns>ulong representing the current sector time</returns>
        private ulong GetCurrentSectorTime()
        {
            ulong sectorTime = CurrentSector;
            if(SectionStartSector != 0)
                sectorTime -= SectionStartSector;
            else if(CurrentTrackNumber > 0)
                sectorTime += TimeOffset;

            return sectorTime;
        }

        /// <summary>
        /// Generate a path selection dialog box
        /// </summary>
        /// <returns>User-selected path, if possible</returns>
        private async Task<string> GetPath()
        {
            return await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                var dialog = new OpenFileDialog { AllowMultiple = false };
                List<string> knownExtensions = new Aaru.DiscImages.AaruFormat().KnownExtensions.ToList();
                dialog.Filters.Add(new FileDialogFilter()
                {
                    Name = "Aaru Image Format (*" + string.Join(", *", knownExtensions) + ")",
                    Extensions = knownExtensions.ConvertAll(e => e.TrimStart('.'))
                });

                return (await dialog.ShowAsync(MainWindow.Instance))?.FirstOrDefault();
            });
        }

        /// <summary>
        /// Update the view-model from the Player
        /// </summary>
        private void PlayerStateChanged(object sender, PropertyChangedEventArgs e)
        {
            if(_player?.Initialized != true)
                return;

            CurrentTrackNumber = _player.CurrentTrackNumber;
            CurrentTrackIndex = _player.CurrentTrackIndex;
            CurrentSector = _player.CurrentSector;
            SectionStartSector = _player.SectionStartSector;

            HiddenTrack = _player.HiddenTrack;

            QuadChannel = _player.QuadChannel;
            IsDataTrack = _player.IsDataTrack;
            CopyAllowed = _player.CopyAllowed;
            TrackHasEmphasis = _player.TrackHasEmphasis;

            Playing = _player.Playing;
            ApplyDeEmphasis = _player.ApplyDeEmphasis;
            Volume = _player.Volume;

            UpdateDigits();
        }

        /// <summary>
        /// Update UI 
        /// </summary>
        private void UpdateDigits()
        {
            Dispatcher.UIThread.Post(() =>
            {
                string digitString = GenerateDigitString() ?? string.Empty.PadLeft(20, '-');
                for(int i = 0; i < _digits.Length; i++)
                {
                    Bitmap digitImage = GetBitmap(digitString[i]);
                    if(_digits[i] != null && digitImage != null)
                        _digits[i].Source = digitImage;
                }
            });
        }

        #endregion
    }
}