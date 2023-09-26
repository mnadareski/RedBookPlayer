using NAudio.Wave;

namespace RedBookPlayer.Models.Audio.Linux
{
    public class AudioBackend : IAudioBackend
    {
        /// <summary>
        /// Sound output instance
        /// </summary>
        private readonly WaveOutEvent _soundOut;

        public AudioBackend() { }

        public AudioBackend(PlayerSource source)
        {
            _soundOut = new WaveOutEvent() { Volume = 1.0f };
            _soundOut.Init(source);
        }

        #region IAudioBackend Implementation

        /// <inheritdoc/>
        public void Pause() => _soundOut.Pause();

        /// <inheritdoc/>
        public void Play() => _soundOut.Play();

        /// <inheritdoc/>
        public void Stop() => _soundOut.Stop();

        /// <inheritdoc/>
        public PlayerState GetPlayerState()
        {
            return (_soundOut?.PlaybackState) switch
            {
                PlaybackState.Paused => PlayerState.Paused,
                PlaybackState.Playing => PlayerState.Playing,
                PlaybackState.Stopped => PlayerState.Stopped,
                _ => PlayerState.NoDisc,
            };
        }

        /// <inheritdoc/>
        public void SetVolume(float volume)
        {
            if(_soundOut != null)
                _soundOut.Volume = volume;
        }

        #endregion
    }
}