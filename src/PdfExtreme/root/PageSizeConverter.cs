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
using PdfExtreme.Drawing;

namespace PdfExtreme
{
    /// <summary>
    /// Converter from <see cref="PageSize"/> to <see cref="XSize"/>.
    /// </summary>
    public static class PageSizeConverter
    {
        /// <summary>
        /// Converts the specified page size enumeration to a pair of values in point.
        /// </summary>
        /// <param name="value">The page size enum.</param>
        /// <returns>The size of a page.</returns>
        /// <exception cref="ArgumentException">If PageSize was undefined</exception>
        public static XSize ToSize(PageSize value)
        {
            // The international definitions are:
            //   1 inch == 25.4 mm
            //   1 inch == 72 point
            return value switch
            {
                // Source http://www.din-formate.de/reihe-a-din-groessen-mm-pixel-dpi.html
                PageSize.A0 => new XSize(2384, 3370),
                PageSize.A1 => new XSize(1684, 2384),
                PageSize.A2 => new XSize(1191, 1684),
                PageSize.A3 => new XSize(842, 1191),
                PageSize.A4 => new XSize(595, 842),
                PageSize.A5 => new XSize(420, 595),
                PageSize.RA0 => new XSize(2438, 3458),
                PageSize.RA1 => new XSize(1729, 2438),
                PageSize.RA2 => new XSize(1219, 1729),
                PageSize.RA3 => new XSize(865, 1219),
                PageSize.RA4 => new XSize(609, 865),
                PageSize.RA5 => new XSize(433, 609),
                PageSize.B0 => new XSize(2835, 4008),
                PageSize.B1 => new XSize(2004, 2835),
                PageSize.B2 => new XSize(1417, 2004),
                PageSize.B3 => new XSize(1001, 1417),
                PageSize.B4 => new XSize(709, 1001),
                PageSize.B5 => new XSize(499, 709),
                // The non-ISO sizes
                // 8 x 10 inch²
                PageSize.Quarto => new XSize(576, 720),
                // 8 x 13 inch²
                PageSize.Foolscap => new XSize(576, 936),
                // 7.5 x 10 inch²
                PageSize.Executive => new XSize(540, 720),
                // 8 x 10.5 inch²
                PageSize.GovernmentLetter => new XSize(576, 756),
                // 8.5 x 11 inch²
                PageSize.Letter => new XSize(612, 792),
                // 8.5 x 14 inch²
                PageSize.Legal => new XSize(612, 1008),
                // 17 x 11 inch²
                PageSize.Ledger => new XSize(1224, 792),
                // 11 x 17 inch²
                PageSize.Tabloid => new XSize(792, 1224),
                // 15.5 x 19.25 inch²
                PageSize.Post => new XSize(1126, 1386),
                // 20 x 15 inch²
                PageSize.Crown => new XSize(1440, 1080),
                // 16.5 x 21 inch²
                PageSize.LargePost => new XSize(1188, 1512),
                // 17.5 x 22 inch²
                PageSize.Demy => new XSize(1260, 1584),
                // 18 x 23 inch²
                PageSize.Medium => new XSize(1296, 1656),
                // 20 x 25 inch²
                PageSize.Royal => new XSize(1440, 1800),
                // 23 x 28 inch²
                PageSize.Elephant => new XSize(1565, 2016),
                // 23.5 x 35 inch²
                PageSize.DoubleDemy => new XSize(1692, 2520),
                // 35 x 45 inch²
                PageSize.QuadDemy => new XSize(2520, 3240),
                // 5.5 x 8.5 inch²
                PageSize.STMT => new XSize(396, 612),
                // 8.5 x 13 inch²
                PageSize.Folio => new XSize(612, 936),
                // 5.5 x 8.5 inch²
                PageSize.Statement => new XSize(396, 612),
                // 10 x 14 inch²
                PageSize.Size10x14 => new XSize(720, 1008),
                _ => throw new ArgumentException($"Invalid PageSize {value}", nameof(value)),
            };
        }
    }
}