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

using PdfExtreme.Drawing;
using PdfExtreme.Pdf.Internal;

namespace PdfExtreme.Pdf.Advanced
{
    /// <summary>
    /// Creates the named destination parameters.
    /// </summary>
    public class PdfNamedDestinationParameters
    {
        private readonly string _parameters;

        private PdfNamedDestinationParameters(string parameters)
        {
            _parameters = parameters;
        }

        private static PdfNamedDestinationParameters CreateXYZ(double? left, double? top, double? zoom)
        {
            return new PdfNamedDestinationParameters(Format("/XYZ {0} {1} {2}", left, top, zoom));
        }

        /// <summary>
        /// Creates a PdfNamedDestinationParameters object for a named destination.
        /// Moving to this destination will only move to the destination page, without changing the left, top and zoom values for the displayed area.
        /// </summary>
        public static PdfNamedDestinationParameters CreateUnchangedPosition()
        {
            return CreateXYZ(null, null, null);
        }

        /// <summary>
        /// Creates a PdfNamedDestinationParameters object for a named destination.
        /// Moving to this destination will move to the desired top value and the optional zoom value on the destination page. The left value for the displayed area and null values are retained unchanged.
        /// </summary>
        /// <param name="top">The top value of the displayed area in PDF world space units.</param>
        /// <param name="zoom">Optional: The zoom value for the displayed area. 1 = 100%, 2 = 200% etc.</param>
        public static PdfNamedDestinationParameters CreateVerticalPosition(double? top, double? zoom = null)
        {
            return CreateXYZ(null, top, zoom);
        }

        /// <summary>
        /// Creates a PdfNamedDestinationParameters object for a named destination.
        /// Moving to this destination will move to the desired left and top value and the optional zoom value on the destination page. Null values are retained unchanged.
        /// </summary>
        /// <param name="left">The left value of the displayed area in PDF world space units.</param>
        /// <param name="top">The top value of the displayed area in PDF world space units.</param>
        /// <param name="zoom">Optional: The zoom value for the displayed area. 1 = 100%, 2 = 200% etc.</param>
        public static PdfNamedDestinationParameters CreatePosition(double? left, double? top, double? zoom = null)
        {
            return CreateXYZ(left, top, zoom);
        }

        /// <summary>
        /// Creates a PdfNamedDestinationParameters object for a named destination.
        /// Moving to this destination will move to the desired left and top value and the optional zoom value on the destination page. Null values are retained unchanged.
        /// </summary>
        /// <param name="position">An Xpoint defining the left and top value of the displayed area in PDF world space units.</param>
        /// <param name="zoom">Optional: The zoom value for the displayed area. 1 = 100%, 2 = 200% etc.</param>
        public static PdfNamedDestinationParameters CreatePosition(XPoint position, double? zoom = null)
        {
            return CreateXYZ(position.X, position.Y, zoom);
        }

        /// <summary>
        /// Creates a PdfNamedDestinationParameters object for a named destination.
        /// Moving to this destination will move to the destination page, displaying the whole page.
        /// </summary>
        public static PdfNamedDestinationParameters CreateFit()
        {
            return new PdfNamedDestinationParameters("/Fit");
        }

        /// <summary>
        /// Creates a PdfNamedDestinationParameters object for a named destination.
        /// Moving to this destination will move to the desired top value on the destination page. The page width ist fitted to the window. Null values are retained unchanged.
        /// </summary>
        /// <param name="top">The top value of the displayed area in PDF world space units.</param>
        public static PdfNamedDestinationParameters CreateFitHorizontally(double? top)
        {
            return new PdfNamedDestinationParameters(Format("/FitH {0}", top));
        }

        /// <summary>
        /// Creates a PdfNamedDestinationParameters object for a named destination.
        /// Moving to this destination will move to the desired left value on the destination page. The page height ist fitted to the window. Null values are retained unchanged.
        /// </summary>
        /// <param name="left">The left value of the displayed area in PDF world space units.</param>
        public static PdfNamedDestinationParameters CreateFitVertically(double? left)
        {
            return new PdfNamedDestinationParameters(Format("/FitV {0}", left));
        }

        /// <summary>
        /// Creates a PdfNamedDestinationParameters object for a named destination.
        /// Moving to this destination will move to the destination page. The given rectangle ist fitted to the window.
        /// </summary>
        /// <param name="left">The left value of the rectangle to display in PDF world space units.</param>
        /// <param name="top">The top value of the rectangle to display in PDF world space units.</param>
        /// <param name="right">The right value of the rectangle to display in PDF world space units.</param>
        /// <param name="bottom">The bottom value of the rectangle to display in PDF world space units.</param>
        public static PdfNamedDestinationParameters CreateFitRectangle(double left, double top, double right, double bottom)
        {
            return new PdfNamedDestinationParameters(Format("/FitR {0} {1} {2} {3}", left, bottom, right, top));
        }

        /// <summary>
        /// Creates a PdfNamedDestinationParameters object for a named destination.
        /// Moving to this destination will move to the destination page. The given rectangle ist fitted to the window.
        /// </summary>
        /// <param name="rect">The XRect representing the rectangle to display in PDF world space units.</param>
        public static PdfNamedDestinationParameters CreateFitRectangle(XRect rect)
        {
            return CreateFitRectangle(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        /// <summary>
        /// Creates a PdfNamedDestinationParameters object for a named destination.
        /// Moving to this destination will move to the destination page. The given rectangle ist fitted to the window.
        /// </summary>
        /// <param name="point1">The first XPoint representing the rectangle to display in PDF world space units.</param>
        /// <param name="point2">The second XPoint representing the rectangle to display in PDF world space units.</param>
        public static PdfNamedDestinationParameters CreateFitRectangle(XPoint point1, XPoint point2)
        {
            return CreateFitRectangle(new XRect(point1, point2));
        }

        /// <summary>
        /// Creates a PdfNamedDestinationParameters object for a named destination.
        /// Moving to this destination will move to the destination page. The page's bounding box is fitted to the window.
        /// </summary>
        public static PdfNamedDestinationParameters CreateFitBoundingBox()
        {
            return new PdfNamedDestinationParameters("/FitB");
        }

        /// <summary>
        /// Creates a PdfNamedDestinationParameters object for a named destination.
        /// Moving to this destination will move to the desired top value on the destination page. The page's bounding box width ist fitted to the window. Null values are retained unchanged.
        /// </summary>
        /// <param name="top">The top value of the displayed area in PDF world space units.</param>
        public static PdfNamedDestinationParameters CreateFitBoundingBoxHorizontally(double? top)
        {
            return new PdfNamedDestinationParameters(Format("/FitBH {0}", top));
        }

        /// <summary>
        /// Creates a PdfNamedDestinationParameters object for a named destination.
        /// Moving to this destination will move to the desired left value on the destination page. The page's bounding box height ist fitted to the window. Null values are retained unchanged.
        /// </summary>
        /// <param name="left">The left value of the displayed area in PDF world space units.</param>
        public static PdfNamedDestinationParameters CreateFitBoundingBoxVertically(double? left)
        {
            return new PdfNamedDestinationParameters(Format("/FitBV {0}", left));
        }

        private static string Format(string format, params double?[] values)
        {
            int length = values.Length;
            object[] objValues = new object[length];
            for (int i = 0; i < length; i++)
            {
                objValues[i] = values[i] ?? (object)"null";
            }

            return PdfEncoders.Format(format, objValues);
        }

        /// <summary>
        /// Returns the parameters string for the named destination.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _parameters;
        }
    }
}
