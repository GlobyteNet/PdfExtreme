// Copyright(c) 2023 Kirill Oskolkov

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.IO;
using System.IO.Compression;

namespace PdfExtreme.Pdf.Filters
{
    /// <summary>
    /// Implements the FlateDecode filter
    /// </summary>
    public class FlateDecode : Filter
    {
        // Reference: 3.3.3  LZWDecode and FlateDecode Filters / Page 71
#if true // New implementation of the deflate filter that does not use SharpZipLib. Set to false, add reference to library, to test SharpZipLib implementation
        
        /// <summary>
        /// Encodes the specified data using flate compression.
        /// </summary>
        /// <param name="data">Data to encode</param>
        /// <returns>Encoded data</returns>
        public override byte[] Encode(byte[] data)
        {
            return Encode(data, PdfFlateEncodeMode.Default);
        }

        /// <summary>
        /// Encodes the specified data using flate compression.
        /// </summary>
        /// <param name="data">Data to encode</param>
        /// <param name="mode">Compression mode to use</param>
        /// <returns>Encoded data</returns>
        public byte[] Encode(byte[] data, PdfFlateEncodeMode mode)
        {
            byte[] compressedArray = null;

            var compressionLevel = mode switch {
                // for now, we only support the default and best speed modes
                PdfFlateEncodeMode.Default or PdfFlateEncodeMode.BestCompression => CompressionLevel.Optimal,
                PdfFlateEncodeMode.BestSpeed => CompressionLevel.Fastest,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };

            using (var compressedStream = new MemoryStream())
            {
                using (var decompressStream = new MemoryStream(data))
                {
                    // Write the standard header
                    compressedStream.WriteByte(0x78);
                    compressedStream.WriteByte(0x9c);

                    using (var deflateStream = new DeflateStream(compressedStream, compressionLevel, leaveOpen: true))
                    {
                        decompressStream.CopyTo(deflateStream);
                    }

                    // Append the Adler32 checksum
                    var adler = Adler32.Calculate(data);
                    compressedStream.WriteByte((byte)(adler >> 24));
                    compressedStream.WriteByte((byte)(adler >> 16));
                    compressedStream.WriteByte((byte)(adler >> 8));
                    compressedStream.WriteByte((byte)adler);
                }
                compressedArray = compressedStream.ToArray();
            }

            return compressedArray;
        }

        /// <summary>
        /// Decodes the specified data using flate compression.
        /// </summary>
        /// <param name="data">Data to decode</param>
        /// <param name="parms">Filter parameters</param>
        /// <returns>Decoded data</returns>
        public override byte[] Decode(byte[] data, FilterParms parms)
        {
            byte[] decompressedArray = null;
            
            using (var decompressedStream = new MemoryStream())
            {
                using (var compressStream = new MemoryStream(data))
                {
                    compressStream.Position += 2; // skip the first two bytes (header)

                    if ((data[1] & 0x20) != 0)
                    {
                        compressStream.Position += 4; // skip the Adler32 checksum (preset dict) see RFC 1950
                    }

                    using (var deflateStream = new DeflateStream(compressStream, CompressionMode.Decompress))
                    {
                        deflateStream.CopyTo(decompressedStream);
                    }
                }
                decompressedArray = decompressedStream.ToArray();
            }

#if false
            var str = System.Text.Encoding.UTF8.GetString(decompressedArray);
#endif

            return decompressedArray;
        }

#else
    public override byte[] Encode(byte[] data)
        {
            return Encode(data, PdfFlateEncodeMode.Default);
        }

        public byte[] Encode(byte[] data, PdfFlateEncodeMode mode)
        {
            MemoryStream ms = new MemoryStream();

            var level = mode switch
            {
                PdfFlateEncodeMode.BestCompression => ICSharpCode.SharpZipLib.Zip.Compression.Deflater.BEST_COMPRESSION,
                PdfFlateEncodeMode.BestSpeed => ICSharpCode.SharpZipLib.Zip.Compression.Deflater.BEST_SPEED,
                _ => ICSharpCode.SharpZipLib.Zip.Compression.Deflater.DEFAULT_COMPRESSION,
            };

            var deflater = new ICSharpCode.SharpZipLib.Zip.Compression.Streams.DeflaterOutputStream(ms, new ICSharpCode.SharpZipLib.Zip.Compression.Deflater(level, false));
            deflater.Write(data, 0, data.Length);
            deflater.Finish();

            return ms.ToArray();
        }

        public override byte[] Decode(byte[] data, FilterParms parms)
        {
            MemoryStream msInput = new MemoryStream(data);
            MemoryStream msOutput = new MemoryStream();
            var inflater = new ICSharpCode.SharpZipLib.Zip.Compression.Streams.InflaterInputStream(msInput, new ICSharpCode.SharpZipLib.Zip.Compression.Inflater(false));
            int cbRead;
            byte[] abResult = new byte[32768];

            do
            {
                cbRead = inflater.Read(abResult, 0, abResult.Length);
                if (cbRead > 0)
                    msOutput.Write(abResult, 0, cbRead);
            }
            while (cbRead > 0);

            inflater.Close();
            msOutput.Flush();

            return msOutput.ToArray();
        }
#endif
    }

    /// <summary>
    /// Adler32 checksum algorithm.
    /// </summary>
    public class Adler32
    {
        /// <summary>
        /// Calculates the Adler32 checksum of the specified data.
        /// </summary>
        /// <returns>Adler32 checksum</returns>
        public static uint Calculate(Span<byte> data)
        {
            uint a = 1, b = 0;
            uint mod = 65521;

            for (int i = 0; i < data.Length; i++)
            {
                a = (a + data[i]) % mod;
                b = (b + a) % mod;
            }

            return (b << 16) | a;
        }
    }
}
