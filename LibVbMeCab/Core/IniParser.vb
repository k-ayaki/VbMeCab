Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO

Namespace Core
    Public Class IniParser
        Public Property SplitChar As Char
        Public Property SkipChars As Char()
        Public Property TrimChars As Char()
        Public Property IsRewrites As Boolean

        Private ReadOnly dic As Dictionary(Of String, String) = New Dictionary(Of String, String)()

        Public Sub New()
            Me.SplitChar = "="c
            Me.SkipChars = {";"c, "#"c}
            Me.TrimChars = {" "c, vbTab}
        End Sub

        Default Public Property Item(key As String) As String
            Get
                Return Me.dic(key)
            End Get
            Set(ByVal Value As String)
                Me.dic(key) = Value
            End Set
        End Property

        Public Sub Load(ByVal fileName As String, ByVal encoding As Encoding)
            Using reader As TextReader = New StreamReader(fileName, encoding)
                Me.Load(reader, fileName)
            End Using
        End Sub

        Public Sub Load(ByVal reader As TextReader, Optional ByVal fileName As String = vbNullString)
            Dim lineNo As Integer = 0
            Dim line As String
            Do
                line = reader.ReadLine()
                If Len(line) = 0 Then Exit Do
                lineNo += 1
                line = line.Trim(Me.TrimChars)

                If Len(line) = 0 Or Left(line, 1) = ";" Or Left(line, 1) = "#" Then Continue Do

                Dim eqPos As Integer = line.IndexOf(Me.SplitChar)
                If eqPos <= 0 Then
                    Throw New MeCabFileFormatException("Format error.", fileName, lineNo, line)
                End If

                Dim key As String = line.Substring(0, eqPos).TrimEnd(Me.TrimChars)
                If Me.IsRewrites = False And Me.dic.ContainsKey(key) Then
                    Continue Do
                End If
                Dim value As String = line.Substring(eqPos + 1).TrimStart(Me.TrimChars)
                Me.dic(key) = value
            Loop
        End Sub
        Public Sub Clear()
            Me.dic.Clear()
        End Sub

    End Class
End Namespace

