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

using System.Diagnostics;
using System.IO;
using PdfExtreme.Internal;
using PdfExtreme.Pdf.Advanced;
using PdfExtreme.Pdf.Content.Objects;

namespace PdfExtreme.Pdf.Content
{
    /// <summary>
    /// Provides the functionality to parse PDF content streams.
    /// </summary>
    public sealed class CParser
    {
        /// <summary>
        /// Creates new instance of CParser.
        /// </summary>
        public CParser(PdfPage page)
        {
            _page = page;
            PdfContent content = page.Contents.CreateSingleContent();
            byte[] bytes = content.Stream.Value;
            _lexer = new CLexer(bytes);
        }

        /// <summary>
        /// Creates new instance of CParser.
        /// </summary>
        public CParser(byte[] content)
        {
            _lexer = new CLexer(content);
        }

        /// <summary>
        /// Creates new instance of CParser.
        /// </summary>
        public CParser(MemoryStream content)
        {
            _lexer = new CLexer(content.ToArray());
        }

        /// <summary>
        /// Creates new instance of CParser.
        /// </summary>
        public CParser(CLexer lexer)
        {
            _lexer = lexer;
        }

        /// <summary>
        /// Current symbol.
        /// </summary>
        public CSymbol Symbol
        {
            get { return _lexer.Symbol; }
        }

        /// <summary>
        /// Reads content from provided source and returns a sequence of content objects.
        /// </summary>
        /// <returns>Parsed sequence</returns>
        public CSequence ReadContent()
        {
            CSequence sequence = new CSequence();
            ParseObject(sequence, CSymbol.Eof);

            return sequence;
        }

        /// <summary>
        /// Parses whatever comes until the specified stop symbol is reached.
        /// </summary>
        void ParseObject(CSequence sequence, CSymbol stop)
        {
            CSymbol symbol;
            while ((symbol = ScanNextToken()) != CSymbol.Eof)
            {
                if (symbol == stop)
                    return;

                CString s;
                COperator op;
                switch (symbol)
                {
                    case CSymbol.Comment:
                        // ignore comments
                        break;

                    case CSymbol.Integer:
                        CInteger n = new CInteger();
                        n.Value = _lexer.TokenToInteger;
                        _operands.Add(n);
                        break;

                    case CSymbol.Real:
                        CReal r = new CReal();
                        r.Value = _lexer.TokenToReal;
                        _operands.Add(r);
                        break;

                    case CSymbol.String:
                    case CSymbol.HexString:
                    case CSymbol.UnicodeString:
                    case CSymbol.UnicodeHexString:
                        s = new CString();
                        s.Value = _lexer.Token;
                        _operands.Add(s);
                        break;

                    case CSymbol.Dictionary:
                        s = new CString();
                        s.Value = _lexer.Token;
                        s.CStringType = CStringType.Dictionary;
                        _operands.Add(s);
                        op = CreateOperator(OpCodeName.Dictionary);
                        //_operands.Clear();
                        sequence.Add(op);

                        break;

                    case CSymbol.Name:
                        CName name = new CName();
                        name.Name = _lexer.Token;
                        _operands.Add(name);
                        break;

                    case CSymbol.Operator:
                        op = CreateOperator();
                        //_operands.Clear();
                        sequence.Add(op);
                        break;

                    case CSymbol.BeginArray:
                        CArray array = new CArray();
                        if (_operands.Count != 0)
                            ContentReaderDiagnostics.ThrowContentReaderException("Array within array...");

                        ParseObject(array, CSymbol.EndArray);
                        array.Add(_operands);
                        _operands.Clear();
                        _operands.Add((CObject)array);
                        break;

                    case CSymbol.EndArray:
                        ContentReaderDiagnostics.HandleUnexpectedCharacter(']');
                        break;

#if DEBUG
                    default:
                        Debug.Assert(false);
                        break;
#endif
                }
            }
        }

        COperator CreateOperator()
        {
            string name = _lexer.Token;
            COperator op = OpCodes.OperatorFromName(name);
            return CreateOperator(op);
        }

        COperator CreateOperator(OpCodeName nameop)
        {
            string name = nameop.ToString();
            COperator op = OpCodes.OperatorFromName(name);
            return CreateOperator(op);
        }

        COperator CreateOperator(COperator op)
        {
            if (op.OpCode.OpCodeName == OpCodeName.BI)
            {
                _lexer.ScanInlineImage();
            }
#if DEBUG
            if (op.OpCode.Operands != -1 && op.OpCode.Operands != _operands.Count)
            {
                if (op.OpCode.OpCodeName != OpCodeName.ID)
                {
                    GetType();
                    Debug.Assert(false, "Invalid number of operands.");
                }
            }
#endif
            op.Operands.Add(_operands);
            _operands.Clear();
            return op;
        }

        CSymbol ScanNextToken()
        {
            return _lexer.ScanNextToken();
        }

        CSymbol ScanNextToken(out string token)
        {
            CSymbol symbol = _lexer.ScanNextToken();
            token = _lexer.Token;
            return symbol;
        }

        /// <summary>
        /// Reads the next symbol that must be the specified one.
        /// </summary>
        CSymbol ReadSymbol(CSymbol symbol)
        {
            CSymbol current = _lexer.ScanNextToken();
            if (symbol != current)
                ContentReaderDiagnostics.ThrowContentReaderException(PSSR.UnexpectedToken(_lexer.Token));
            return current;
        }

        readonly CSequence _operands = new CSequence();
        PdfPage _page;
        readonly CLexer _lexer;
    }
}
