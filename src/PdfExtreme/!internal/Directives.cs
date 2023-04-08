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

//
// Documentation of conditional compilation symbols used in PdfExtreme.
// Checks correct settings and obsolete conditional compilation symbols.
//

#if GDI && WPF
// PdfExtreme based on both System.Drawing and System.Windows classes
// This is for developing and cross testing only
#elif GDI
// PdfExtreme based on System.Drawing classes
#if UseGdiObjects
// PdfExtreme X graphics classes have implicit cast operators for GDI+ objects.
// Define this to make it easier to use older code with PdfExtreme.
// Undefine this to prevent dependencies to GDI+
#endif

#elif WPF
// PdfExtreme based on Windows Presentation Foundation.
#elif SILVERLIGHT
// PdfExtreme based on 'Silverlight'.
#if !WPF
#error 'SILVERLIGHT' must be defined together with 'WPF'
#endif

#elif WINDOWS_PHONE
// PdfExtreme based on 'Windows Phone'.
#if !WPF
#error 'WINDOWS_PHONE' must be defined together with 'WPF'.
#endif
#if !SILVERLIGHT
#error 'WINDOWS_PHONE' must be defined together with 'SILVERLIGHT'.
#endif

#elif CORE
// PdfExtreme independent of any particular .NET library.
#elif NETFX_CORE
// PdfExtreme based on 'WinRT'.
#elif UWP
// PdfExtreme based on 'Windows Universal Platform'.
#elif DNC10
#else
#error Either 'CORE', 'GDI', 'WPF', 'SILVERLIGHT', 'WINDOWS_PHONE', or 'NETFX_CORE' must be defined. Or UWP. Or DNC10.
#endif
