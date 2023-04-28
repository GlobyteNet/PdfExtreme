# PdfExtreme
C# library that allows you to create, edit and save PDF files.

## Usage
Here is a simple example of how to create a PDF file with a single page and a text on it.
```csharp
var pdf = new PdfDocument();
var page = pdf.Pages.Add();
var graphics = XGraphics.FromPdfPage(page);
var font = new XFont("Verdana", 24);
graphics.DrawString("Hello, World!", font, XBrushes.Black, 100, 50);
pdf.Save("file.pdf");
```

## Contributing
Contributions are welcome. Please submit a pull request.

## Acknowledgements
This library was based on PDFsharp: A .NET library for processing PDF
