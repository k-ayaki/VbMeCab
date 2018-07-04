Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
Imports System.Text
Imports VbMeCab

Module Module1
    Sub Main()
        Dim targetFile As String = My.Settings.TargetFile
        Dim encoding As Encoding = Encoding.GetEncoding(My.Settings.TargetEncoding)
        Dim sw As Stopwatch = New Stopwatch()

        '開始指示を待機
        Console.WriteLine("Press Enter key to start.")
        Console.ReadLine()

        Console.WriteLine(vbTab + vbTab + vbTab + "ProcessTime" + vbTab + "TotalMemory")

        '解析準備処理
        GC.Collect()
        sw.Start()
        Dim tagger As MeCabTagger = MeCabTagger.Create()
        sw.Stop()
        Console.WriteLine("OpenTagger:" + vbTab + vbTab + "{0:0.000}sec" + vbTab + "{1:#,000}byte",
                          sw.Elapsed.TotalSeconds, GC.GetTotalMemory(False))

        'ファイル読込だけの場合
        Using reader As StreamReader = New StreamReader(targetFile, encoding)
            sw.Reset()
            GC.Collect()
            sw.Start()
            Do
                Dim line As String = reader.ReadLine()
                If line Is Nothing Then Exit Do
                line = reader.ReadLine()
            Loop
            sw.Stop()
        End Using

        Console.WriteLine("ReadLine:" + vbTab + vbTab + "{0:0.000}sec" + vbTab + "{1:#,000}byte",
                          sw.Elapsed.TotalSeconds, GC.GetTotalMemory(False))

        '解析処理（Nodeの出力）

        Using reader = New StreamReader(targetFile, encoding)
            sw.Reset()
            GC.Collect()
            sw.Start()
            Dim node As MeCabNode
            Do
                Dim line As String = reader.ReadLine()
                If line Is Nothing Then Exit Do
                node = tagger.ParseToNode(line)
            Loop
            sw.Stop()
        End Using

        Console.WriteLine("ParseToNode:" + vbTab + vbTab + "{0:0.000}sec" + vbTab + "{1:#,000}byte",
                          sw.Elapsed.TotalSeconds, GC.GetTotalMemory(False))

        '解析処理（latticeモードの文字列出力）
        tagger.OutPutFormatType = "lattice"
        Using reader = New StreamReader(targetFile, encoding)

            sw.Reset()
            GC.Collect()
            sw.Start()
            Do
                Dim line As String = reader.ReadLine()
                If line Is Nothing Then Exit Do
                Dim ret As String = tagger.Parse(line)
            Loop
            sw.Stop()
            Console.WriteLine("Parse(lattice):" + vbTab + vbTab + "{0:0.000}sec" + vbTab + "{1:#,000}byte",
                              sw.Elapsed.TotalSeconds, GC.GetTotalMemory(False))
        End Using

        '解析処理（Best解5件のNodeの出力）
        tagger.LatticeLevel = MeCabLatticeLevel.One
        Using reader = New StreamReader(targetFile, encoding)
            sw.Reset()
            GC.Collect()
            sw.Start()
            Dim i As Integer = 0
            Do
                Dim line As String = reader.ReadLine()
                If line Is Nothing Then Exit Do
                For Each node In tagger.ParseNBestToNode(line)
                    i += 1
                    If 5 <= i Then Exit For
                Next
            Loop
            sw.Stop()
            Console.WriteLine("ParseNBestToNode:" + vbTab + "{0:0.000}sec" + vbTab + "{1:#,000}byte",
                                  sw.Elapsed.TotalSeconds, GC.GetTotalMemory(False))
        End Using

        '対象の情報
        Using reader = New StreamReader(targetFile, encoding)
            Dim charCount As Long = 0
            Dim lineCount As Long = 0
            Dim wordCount As Long = 0
            Do
                Dim line As String = reader.ReadLine()
                If line Is Nothing Then Exit Do
                charCount += line.Length
                lineCount += 1
                Dim node As MeCabNode = tagger.ParseToNode(line)
                node = node._Next
                Do
                    If node._Next Is Nothing Then Exit Do
                    wordCount += 1
                    node = node._Next
                Loop
            Loop

            Console.WriteLine("Target: {0} {1:#,000}byte {2:#,000}char {3:#,000}line ({4:#,000}word)",
                              targetFile, reader.BaseStream.Position, charCount, lineCount, wordCount)

        End Using
        tagger.Dispose()

        '終了したことを通知
        Console.WriteLine()
        Console.WriteLine("Finish!")
        Console.WriteLine("Press Enter key to close.")
        Console.ReadLine()
    End Sub
End Module
