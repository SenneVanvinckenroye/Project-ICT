//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.IO;
using System.IO.IsolatedStorage;
using TailSpin.Phone.Adapters;

namespace TailSpin.PhoneClient.Infrastructure
{
    public class WaveFormatter : IDisposable
    {
        private BinaryWriter outputWriter;
        private IIsolatedStorageFacade isolatedStorageFacade;

        public WaveFormatter(string outFile, uint sampleRate, ushort bitsPerSample, ushort channels, IIsolatedStorageFacade isolatedStorageFacade)
        {
            this.isolatedStorageFacade = isolatedStorageFacade;
            this.outputWriter = isolatedStorageFacade.OpenBinaryWriter(outFile);
            this.WriteHeaderAndFormat(sampleRate, bitsPerSample, channels);
        }

        public void WriteDataChunk(byte[] data)
        {
            var bufferSize = (uint)data.Length;
            this.outputWriter.Write("data".ToCharArray());
            this.outputWriter.Write(bufferSize);
            this.outputWriter.Write(data);
        }

        public void Dispose()
        {
            if (this.outputWriter != null)
            {
                this.Close();
            }
        }

        private void Close()
        {
            // write out the final length to the header.
            this.outputWriter.Seek(4, SeekOrigin.Begin);
            var filesize = (uint)this.outputWriter.BaseStream.Length;
            this.outputWriter.Write(filesize - 8);
            this.outputWriter.Close();
            this.outputWriter = null;
            this.isolatedStorageFacade.Dispose();
            this.isolatedStorageFacade = null;
        }

        private void WriteHeaderAndFormat(uint sampleRate, ushort bitsPerSample, ushort channels)
        {
            var header = new WaveHeader();
            var wavFormat = new WaveFormatChunk
            {
                SamplesPerSec = sampleRate,
                Channels = channels,
                BitsPerSample = bitsPerSample
            };

            wavFormat.BlockAlign = (ushort)(wavFormat.Channels * (wavFormat.BitsPerSample / 8));
            wavFormat.AvgBytesPerSec = wavFormat.SamplesPerSec * wavFormat.BlockAlign;

            // Write the header  
            this.outputWriter.Write(header.GroupId.ToCharArray());
            this.outputWriter.Write(header.FileLength);
            this.outputWriter.Write(header.RiffType.ToCharArray());

            // Write the format chunk  
            this.outputWriter.Write(wavFormat.ChunkId.ToCharArray());
            this.outputWriter.Write(wavFormat.ChunkSize);
            this.outputWriter.Write(wavFormat.FormatTag);
            this.outputWriter.Write(wavFormat.Channels);
            this.outputWriter.Write(wavFormat.SamplesPerSec);
            this.outputWriter.Write(wavFormat.AvgBytesPerSec);
            this.outputWriter.Write(wavFormat.BlockAlign);
            this.outputWriter.Write(wavFormat.BitsPerSample);
        }
    }
}
