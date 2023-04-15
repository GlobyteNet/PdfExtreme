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

namespace PdfExtreme
{
    /// <summary>
    /// Version info base for all PdfExtreme related assemblies.
    /// </summary>
    public static class ProductVersionInfo
    {
        /// <summary>
        /// The title of the product.
        /// </summary>
        public const string Title = "PdfExtreme";

        /// <summary>
        /// A characteristic description of the product.
        /// </summary>
        public const string Description = "A .NET library for processing PDF.";

        /// <summary>
        /// The PDF producer information string.
        /// </summary>
        public const string Producer = Title + " " + VersionMajor + "." + VersionMinor + "." + VersionBuild + Technology;

        /// <summary>
        /// The PDF producer information string including VersionPatch.
        /// </summary>
        public const string Producer2 = Title + " " + VersionMajor + "." + VersionMinor + "." + VersionBuild + "." + VersionPatch + Technology;

        /// <summary>
        /// The full version number.
        /// </summary>
        public const string Version = VersionMajor + "." + VersionMinor + "." + VersionBuild + "." + VersionPatch;

        /// <summary>
        /// The full version string.
        /// </summary>
        public const string Version2 = VersionMajor + "." + VersionMinor + "." + VersionBuild + "." + VersionPatch + Technology;

        /// <summary>
        /// The major version number of the product.
        /// </summary>
        public const string VersionMajor = "1";

        /// <summary>
        /// The minor version number of the product.
        /// </summary>
        public const string VersionMinor = "51";

        /// <summary>
        /// The build number of the product.
        /// </summary>
        public const string VersionBuild = "5185";  // V16G // Build = days since 2005-01-01  -  change this values ONLY HERE

        /// <summary>
        /// The patch number of the product.
        /// </summary>
        public const string VersionPatch = "0";

        /// <summary>
        /// The Version Prerelease String for NuGet.
        /// </summary>
        public const string VersionPrerelease = "beta"; // "" for stable Release, e.g. "beta" or "rc.1.2" for Prerelease. // Also used for NuGet Version.

#if DEBUG
        /// <summary>
        /// The calculated build number.
        /// </summary>
// ReSharper disable RedundantNameQualifier
        public static int BuildNumber = (System.DateTime.Now - new System.DateTime(2005, 1, 1)).Days;
        // ReSharper restore RedundantNameQualifier
#endif

        /// <summary>
        /// E.g. "2005-01-01", for use in NuGet Script.
        /// </summary>
        public const string VersionReferenceDate = "2005-01-01";

        /// <summary>
        /// Nuspec Doc: A Boolean value that specifies whether the client needs to ensure that the package license (described by licenseUrl) is accepted before the package is installed.
        /// </summary>                  
        public const bool NuGetRequireLicenseAcceptance = false;

        /// <summary>
        /// The technology tag of the product:
        /// (none) Pure .NET
        /// -gdi : GDI+,
        /// -wpf : WPF,
        /// -hybrid : Both GDI+ and WPF (hybrid).
        /// -sl : Silverlight
        /// -wp : Windows Phone
        /// -wrt : Windows RunTime
        /// </summary>
#if GDI && !WPF
        // GDI+ (System.Drawing)
        public const string Technology = "-gdi";
#endif
#if WPF && !GDI && !SILVERLIGHT
        // Windows Presentation Foundation
        public const string Technology = "-wpf";
#endif
#if WPF && GDI
        // Hybrid - for testing only
        public const string Technology = "-h";
#endif
#if SILVERLIGHT && !WINDOWS_PHONE
        // Silverlight 5
        public const string Technology = "-sl";
#endif
#if WINDOWS_PHONE
        // Windows Phone
        public const string Technology = "-wp";
#endif
#if NETFX_CORE
        // WinRT
        public const string Technology = "-wrt";
#endif
#if UWP
        // Windows Universal App
        public const string Technology = "-uwp";
#endif
#if DNC10
        // .net Core
        public const string Technology = "-dnc";
#endif
#if CORE
        // .net classic without GDI+ and WPF
        public const string Technology = "";  // no extension
#endif
    }
}
