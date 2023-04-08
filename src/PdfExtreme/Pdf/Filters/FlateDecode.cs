//
// Authors:
//   Stefan Lange
//
// Copyright (c) 2005-2019 empira Software GmbH, Cologne Area (Germany)
//
// http://www.pdfsharp.com
// http://sourceforge.net/projects/pdfsharp
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

using System;
using System.IO;
using PdfExtreme.Internal;
using System.IO.Compression;
using System.Text;
using PdfExtreme.SharpZipLib.Zip.Compression;
using PdfExtreme.SharpZipLib.Zip.Compression.Streams;

namespace PdfExtreme.Pdf.Filters
{
    /// <summary>
    /// Implements the FlateDecode filter by wrapping SharpZipLib.
    /// </summary>
    public class FlateDecode : Filter
    {
#if false
        // Reference: 3.3.3  LZWDecode and FlateDecode Filters / Page 71

        /// <summary>
        /// Encodes the specified data.
        /// </summary>
        public override byte[] Encode(byte[] data)
        {
            return Encode(data, PdfFlateEncodeMode.Default);
        }

        /// <summary>
        /// Encodes the specified data.
        /// </summary>
        public byte[] Encode(byte[] data, PdfFlateEncodeMode mode)
        {
            MemoryStream ms = new MemoryStream();
      
#if false
            
            ms.WriteByte(0x78);
            ms.WriteByte(0x49);

            DeflateStream zip = new DeflateStream(ms, CompressionMode.Compress, true);
            zip.Write(data, 0, data.Length);
            zip.Close();
#else
            int level = Deflater.DEFAULT_COMPRESSION;
            switch (mode)
            {
                case PdfFlateEncodeMode.BestCompression:
                    level = Deflater.BEST_COMPRESSION;
                    break;
                case PdfFlateEncodeMode.BestSpeed:
                    level = Deflater.BEST_SPEED;
                    break;
            }
            DeflaterOutputStream zip = new DeflaterOutputStream(ms, new Deflater(level, false));
            zip.Write(data, 0, data.Length);
            zip.Finish();
#endif
#if !NETFX_CORE && !UWP
            ms.Capacity = (int)ms.Length;
            return ms.GetBuffer();
#else
            return ms.ToArray();
#endif
        }

        /// <summary>
        /// Decodes the specified data.
        /// </summary>
        public override byte[] Decode(byte[] data, FilterParms parms)
        {
            data[0] = 0x78;
            data[1] = 0x9C;
            var dstr = Encoding.ASCII.GetString(data);
            MemoryStream msInput = new MemoryStream(data);
            MemoryStream msOutput = new MemoryStream();
#if false
            byte header;   // 0x30 0x59
            header = (byte)msInput.ReadByte();
            //Debug.Assert(header == 48);
            header = (byte)msInput.ReadByte();
            //Debug.Assert(header == 89);
            DeflateStream zip = new DeflateStream(msInput, CompressionMode.Decompress, true);
            int cbRead;
            byte[] abResult = new byte[1024];
            do
            {
                cbRead = zip.Read(abResult, 0, abResult.Length);
                if (cbRead > 0)
                    msOutput.Write(abResult, 0, cbRead);
            }
            while (cbRead > 0);
            zip.Close();
            msOutput.Flush();
            if (msOutput.Length >= 0)
            {
                msOutput.Capacity = (int)msOutput.Length;
                return msOutput.GetBuffer();
            }
            return null;
#else
            InflaterInputStream iis = new InflaterInputStream(msInput, new Inflater(false));
            int cbRead;
            byte[] abResult = new byte[32768];
            do
            {
                cbRead = iis.Read(abResult, 0, abResult.Length);
                if (cbRead > 0)
                    msOutput.Write(abResult, 0, cbRead);
            }
            while (cbRead > 0);
#if UWP
            iis.Dispose();
#else
            iis.Close();
#endif
            msOutput.Flush();
            if (msOutput.Length >= 0)
            {
#if NETFX_CORE || UWP || DNC10
                return msOutput.ToArray();
#else
                msOutput.Capacity = (int)msOutput.Length;
                var buf = msOutput.GetBuffer();
                var debugstr = System.Text.Encoding.UTF8.GetString(buf);
                return buf;
#endif
            }
            return null;
#endif
        }
#else
        // New implementation of deflate filter not using SharpZipLib
        public override byte[] Encode(byte[] data)
        {
            return Encode(data, PdfFlateEncodeMode.Default);
        }

        /// <summary>
        /// Encodes the specified data.
        /// </summary>
        public byte[] Encode(byte[] data, PdfFlateEncodeMode mode)
        {
            throw new NotImplementedException();
        }

        public override byte[] Decode(byte[] data, FilterParms parms)
        {
            byte[] decompressedArray = null;
            
            using (var decompressedStream = new MemoryStream())
            {
                using (var compressStream = new MemoryStream(data))
                {
                    compressStream.Position += 2; // skip the first two bytes (header)
                    using (var deflateStream1 = new DeflateStream(compressStream, CompressionMode.Decompress))
                    {
                        deflateStream1.CopyTo(decompressedStream);
                    }
                }
                decompressedArray = decompressedStream.ToArray();
            }

#if DEBUG
            var str = System.Text.Encoding.UTF8.GetString(decompressedArray);
#endif

            return decompressedArray;
        }
#endif
    }
}
