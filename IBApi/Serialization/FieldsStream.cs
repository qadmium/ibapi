using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IBApi.Serialization
{
    internal sealed class FieldsStream : IDisposable
    {
        private const int prefetchSize = 1024;
        private const int bufferSize = prefetchSize*8;
        private readonly Stream stream;
        private byte[] buffer = new byte[bufferSize];
        private int currentBufferIndex;
        private int dataEndIndex;

        private int startOfFieldIndex;

        public FieldsStream(Stream stream)
        {
            this.stream = stream;
        }

        public void Dispose()
        {
            this.stream.Dispose();
        }

        public async Task<string> ReadNextField(CancellationToken cancellationToken)
        {
            var result = this.TryReadField();

            while (result == null)
            {
                await this.ReadFromStreamAndAppendToBuffer(cancellationToken);
                result = this.TryReadField();
            }

            return result;
        }

        public async Task Write(byte[] field, CancellationToken cancellationToken)
        {
            await this.stream.WriteAsync(field, 0, field.Length, cancellationToken);
        }

        private string TryReadField()
        {
            while (this.currentBufferIndex < this.dataEndIndex)
            {
                if (this.buffer[this.currentBufferIndex] == char.MinValue)
                {
                    var result = Encoding.ASCII.GetString(this.buffer, this.startOfFieldIndex, this.ReadedBytes());
                    this.currentBufferIndex++;
                    this.startOfFieldIndex = this.currentBufferIndex;
                    return result;
                }

                ++this.currentBufferIndex;
            }
            return null;
        }

        private async Task ReadFromStreamAndAppendToBuffer(CancellationToken cancellationToken)
        {
            Contract.Assert(this.dataEndIndex == this.currentBufferIndex);
            if (this.AvailableSpaceInBuffer() < prefetchSize)
            {
                var oldBuffer = this.buffer;
                this.buffer = new byte[bufferSize];
                Buffer.BlockCopy(oldBuffer, this.startOfFieldIndex, this.buffer, 0, this.ReadedBytes());
                this.currentBufferIndex = this.ReadedBytes();
                this.startOfFieldIndex = 0;
                this.dataEndIndex = this.currentBufferIndex;
            }

            var readedBytes = await this.stream.ReadAsync(this.buffer, this.dataEndIndex, prefetchSize, cancellationToken);
            this.dataEndIndex += readedBytes;
        }

        private int AvailableSpaceInBuffer()
        {
            return this.buffer.Length - this.dataEndIndex;
        }

        private int ReadedBytes()
        {
            return this.currentBufferIndex - this.startOfFieldIndex;
        }
    }
}