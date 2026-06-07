using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace DvMod.Paperwork
{
    public static class EmbeddedResources
    {
        public static class Assets
        {
            public static readonly Lazy<Texture2D> CheckPng = new Lazy<Texture2D>(() => LoadPng("DvMod.Paperwork.check.png", 128, 128));

            public static readonly Lazy<AudioClip> CheckWav = new Lazy<AudioClip>(() => LoadWav("DvMod.Paperwork.check.wav"));
        }

        public static Texture2D LoadPng(string resourceName, int width, int height)
        {
            Paperwork.LogTrace($"{nameof(EmbeddedResources)}.{nameof(LoadPng)}()");

            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new Exception($"Resource not found: {resourceName}");

            using var reader = new BinaryReader(stream);

            var png = reader.ReadBytes((int)stream.Length);
            Texture2D texture = new Texture2D(width, height);

            ImageConversion.LoadImage(texture, png);
            return texture;
        }

        public static AudioClip LoadWav(string resourceName)
        {
            Paperwork.LogTrace($"{nameof(EmbeddedResources)}.{nameof(LoadWav)}()");

            var assembly = Assembly.GetExecutingAssembly();

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new Exception($"Resource not found: {resourceName}");

            using var reader = new BinaryReader(stream);

            var wav = reader.ReadBytes((int)stream.Length);

            return WavToAudioClip(wav, resourceName);
        }

        private static AudioClip WavToAudioClip(byte[] wavFile, string name)
        {
            Paperwork.LogTrace($"{nameof(EmbeddedResources)}.{nameof(WavToAudioClip)}()");

            const int wavHeaderOffsetChannels = 22;
            const int wavHeaderOffsetSampleRate = 24;
            const int waveHeaderSize = 44;

            int channels = BitConverter.ToInt16(wavFile, wavHeaderOffsetChannels);
            int sampleRate = BitConverter.ToInt32(wavFile, wavHeaderOffsetSampleRate);
            var pos = waveHeaderSize;

            int samples = (wavFile.Length - pos) / 2;
            float[] data = new float[samples];

            int i = 0;
            while (pos < wavFile.Length)
            {
                short sample = BitConverter.ToInt16(wavFile, pos);
                data[i] = sample / 32768f;
                pos += 2;
                i++;
            }

            var clip = AudioClip.Create(name, samples / channels, channels, sampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }
    }
}
