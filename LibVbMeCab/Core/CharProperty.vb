'  MeCab -- Yet Another Part-of-Speech and Morphological Analyzer
'
'  Copyright(C) 2001-2006 Taku Kudo <taku@chasen.org>
'  Copyright(C) 2004-2006 Nippon Telegraph and Telephone Corporation
'  Copyright(C) 2018      Ken'ichiro Ayaki 
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Runtime.InteropServices

Namespace Core
    Public Class CharProperty
#Region "Const/Field/Property"

        Private Const CharPropertyFile As String = "char.bin"

        Private cList() As String

        Private ReadOnly charInfoList() As CharInfo = New CharInfo(&HFFFE) {}

        Public ReadOnly Property Size() As Integer
            Get
                Return Me.cList.Length
            End Get
        End Property
#End Region

#Region "Open"

        Public Sub Open(ByVal dicDir As String)
            Dim fileName As String = Path.Combine(dicDir, CharPropertyFile)

            Using stream As FileStream = New FileStream(fileName, FileMode.Open, FileAccess.Read)
                Using reader As BinaryReader = New BinaryReader(stream)
                    Me.Open(reader, fileName)
                End Using
            End Using
        End Sub

        Public Sub Open(ByVal reader As BinaryReader, Optional ByVal fileName As String = vbNullString)
            Dim cSize As System.UInt32 = reader.ReadUInt32()

            If reader.BaseStream.CanSeek = True Then
                Dim fSize As Long = Marshal.SizeOf(Of System.UInt32) + 32 * cSize + Marshal.SizeOf(Of System.UInt32) * charInfoList.Length
                If reader.BaseStream.Length <> fSize Then
                    Throw New MeCabInvalidFileException("invalid file size", fileName)
                End If
            End If

            Me.cList = New String(cSize - 1) {}
            Dim i As Integer
            For i = 0 To Me.cList.Length - 1
                Me.cList(i) = StrUtils.GetString(reader.ReadBytes(32), Encoding.ASCII)
            Next

            For i = 0 To Me.charInfoList.Length - 1
                Me.charInfoList(i) = New CharInfo(reader.ReadUInt32())
            Next
        End Sub
#End Region

#Region "Get Infometion"
        Public Function Name(ByVal i As Integer) As String
            Return Me.cList(i)
        End Function

        Public Function SeekToOtherType(ByVal sentence As String, ByVal iBegin As Integer, ByVal c As CharInfo, ByRef fail As CharInfo, ByRef cLen As Integer) As Integer
            Dim p As Integer = iBegin
            cLen = 0
            Dim str As Char() = sentence.ToCharArray()
            If sentence.Length <= iBegin Then Return iBegin
            fail = Me.GetCharInfo(str(p))

            While p < (str.Length - 1) AndAlso c.IsKindOf(fail)
                p += 1
                cLen += 1
                c = fail
                fail = Me.GetCharInfo(str(p))
            End While

            Return p
        End Function
        Public Function GetCharInfo(ByVal c As Char) As CharInfo
            Dim i As Long = Microsoft.VisualBasic.AscW(c)
            Return Me.charInfoList(i)
        End Function
#End Region

    End Class
End Namespace

