using System;
using NAudio.Wave;

namespace RedBookPlayer.Models.Audio
{
    public class PlayerSource : IWaveProvider
    {
        public delegate int ReadFunction(byte[] buffer, int offset, int count);

        readonly ReadFunction _read;

        public bool Run = true;

        public PlayerSource(ReadFunction read) => _read = read;

        public WaveFormat WaveFormat => new WaveFormat();

        public long Position
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public long Length => throw new NotImplementedException();

        public int Read(byte[] buffer, int offset, int count)
        {
            if(Run)
                return _read(buffer, offset, count);

            Array.Clear(buffer, offset, count);

            return count;
        }

        public void Dispose() {}

        public void Start() => Run = true;

        public void Stop() => Run = false;
    }
}