using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace DvMod.Paperwork
{
    public static class EmbeddedAudioClips
    {
        public static AudioClip LoadWavFromEmbeddedResource(string resourceName)
        {
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
            int channels = BitConverter.ToInt16(wavFile, 22);
            int sampleRate = BitConverter.ToInt32(wavFile, 24);
            var pos = 44;

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
