﻿#I "../bin"
#r "FSharp.Markdown.dll"
#r "PdfSharp.dll"
#r "MigraDoc.DocumentObjectModel.dll"
#r "FSharp.Markdown.Pdf.dll"

open System.IO

(****
    To use the Markdown to PDF formatting, you need to reference these three DLLs:
        // for the MarkdownParser
        FSharp.Markdown.dll
        
        // for the PDF formatter that formats the tokenized Markdown doc
        FSharp.Markdown.Pdf.dll
        
        // to construct the Document value that is passed into the 'formatMarkdown' function
        MigraDoc.DocumentObjectModel.dll
***)
open FSharp.Markdown
open FSharp.Markdown.Pdf
open MigraDoc.DocumentObjectModel

let source = __SOURCE_DIRECTORY__
let outputPath = Path.Combine(source, "output")
let testFilesPath = Path.Combine(source, "testfiles")

// the 'testfiles' folder contains a suite of Markdown document (the .text files) as well as the 
// HTML output (the .html files)
let testFiles = Directory.GetFiles(testFilesPath) |> Array.filter (fun path -> path.EndsWith ".text")

(***
    The 'formatMarkdown' function takes a Document value which you need to construct, you have the
    option to set the styles for various style names (heading 1-6, table, list, etc.) which will
    be used as overrides in the generated PDF document rather than those that are specified as
    default in the 'formatMarkdown' function.
    
    If you are happy to use the default styles, then simply call the 'formatMarkdown' function with
    a new Document value, as well as the links and paragraphs from the tokenized Markdown document
    generated by the Markdown parser from the FSharp.Formatting library.
***)
let outputFiles = 
    for testFile in testFiles do
        let filename = Path.GetFileNameWithoutExtension testFile
        let markdownDoc = File.ReadAllText testFile
        let parsed = Markdown.Parse(markdownDoc)
        let pdfDoc = formatMarkdown (new Document()) parsed.DefinedLinks parsed.Paragraphs
        
        let outputFile = Path.Combine(outputPath, filename + ".pdf")
        pdfDoc.Save(outputFile)