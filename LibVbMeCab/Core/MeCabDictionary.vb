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
    Public Class MeCabDictionary : Implements IDisposable
#Region "Const/Field/Property"
        Private Const DictionaryMagicID As System.UInt32 = &HEF718F77UL
        Private Const DicVersion As System.UInt32 = 102UL

        Private tokens As Token()
        Private features As Byte()

        Private da As DoubleArray = New DoubleArray()

        Private encoding As Encoding

        '' <summary>
        '' 辞書の文字コード
        '' </summary>
        Public ReadOnly Property CharSet() As String
            Get
                Return Me.encoding.WebName
            End Get
        End Property

        '' <summary>
        '' バージョン
        '' </summary>
        Public Property Version As System.UInt32

        '' <summary>
        '' 辞書のタイプ
        '' </summary>
        Public Property Type As DictionaryType

        Public Property LexSize As System.UInt32

        '' <summary>
        '' 左文脈 ID のサイズ
        '' </summary>
        Public Property LSize As System.UInt32

        '' <summary>
        '' 右文脈 ID のサイズ
        '' </summary>
        Public Property RSize As System.UInt32

        '' <summary>
        '' 辞書のファイル名
        '' </summary>
        Public Property FileName As String
#End Region

#Region "Open"
        Public Sub Open(ByVal filePath As String)
            Me.FileName = filePath

            Using fileStream As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)
                Using reader As BinaryReader = New BinaryReader(fileStream)
                    Me.Open(reader)
                End Using
            End Using
        End Sub

        Public Sub Open(ByVal reader As BinaryReader)
            Dim magic As System.UInt32 = reader.ReadUInt32()
            'CanSeekの時のみストリーム長のチェック
            If reader.BaseStream.CanSeek = True And reader.BaseStream.Length <> (magic Xor DictionaryMagicID) Then
                Throw New MeCabInvalidFileException("dictionary file is broken", Me.FileName)
            End If

            Me.Version = reader.ReadUInt32()
            If Me.Version <> DicVersion Then
                Throw New MeCabInvalidFileException("incompatible version", Me.FileName)
            End If

            Me.Type = CType(reader.ReadUInt32(), DictionaryType)
            Me.LexSize = reader.ReadUInt32()
            Me.LSize = reader.ReadUInt32()
            Me.RSize = reader.ReadUInt32()
            Dim dSize As System.UInt32 = reader.ReadUInt32()
            Dim tSize As System.UInt32 = reader.ReadUInt32()
            Dim fSize As System.UInt32 = reader.ReadUInt32()
            Dim dummy As System.UInt32 = reader.ReadUInt32()

            Dim charSet As String = StrUtils.GetString(reader.ReadBytes(32), Encoding.ASCII)
            Me.encoding = StrUtils.GetEncoding(charSet)

            Me.da.Open(reader, dSize)

            Me.tokens = New Token(tSize / Marshal.SizeOf(Of Token)() - 1) {}
            Dim i As Integer
            For i = 0 To Me.tokens.Length - 1
                Me.tokens(i) = Token.Create(reader)
            Next

            Me.features = reader.ReadBytes(CType(fSize, Integer))

            If reader.BaseStream.ReadByte() <> -1 Then
                Throw New MeCabInvalidFileException("dictionary file is broken", Me.FileName)
            End If
        End Sub
#End Region

#Region "Search"
        Public Function ExactMatchSearch(ByVal key As String) As DoubleArray.ResultPair
            Dim maxByteCount As Integer = Me.encoding.GetMaxByteCount(key.Length)
            Dim bytes As Byte() = New Byte(maxByteCount - 1) {}
            Dim bytesLen As Integer = Me.encoding.GetBytes(key, 0, key.Length, bytes, 0)

            Dim result As DoubleArray.ResultPair = Me.da.ExactMatchSearch(bytes, bytesLen, 0)

            '文字数をデコードしたものに変換
            result.Length = Me.encoding.GetCharCount(bytes, 0, result.Length)
            Return result
        End Function

        Public Function CommonPrefixSearch(ByVal str As String, ByVal ikey As Integer, ByRef result As DoubleArray.ResultPair(), ByVal rLen As Integer) As Integer
            Dim len As Integer = str.Length
            Dim maxByteLen As Integer = Me.encoding.GetMaxByteCount(len)
            Dim bytes As Byte() = New Byte(maxByteLen - 1) {}
            Dim bytesLen As Integer = Me.encoding.GetBytes(str, 0, len, bytes, 0)
            Dim n As Integer = Me.da.CommonPrefixSearch(bytes, result, rLen, bytesLen)

            '文字数をデコードしたものに変換
            Dim i As Integer
            For i = 0 To n - 1
                result(i).Length = Me.encoding.GetCharCount(bytes, 0, result(i).Length)
            Next

            Return n
        End Function
#End Region

#Region "Get Infomation"
        Public Function GetToken(ByVal n As DoubleArray.ResultPair) As Token()
            Dim dist As Token() = New Token((&HFF And n.Value) - 1) {}
            Dim tokenPos As Integer = n.Value >> 8
            Array.Copy(Me.tokens, tokenPos, dist, 0, dist.Length)
            Return dist
        End Function

        Public Function GetFeature(ByVal featurePos As System.UInt32) As String
            Return StrUtils.GetString(Me.features, CType(featurePos, Long), Me.encoding)
        End Function

#End Region

#Region "etc."
        Public Function IsCompatible(ByVal d As MeCabDictionary) As Boolean
            IsCompatible = If(Me.Version = d.Version And
                    Me.LSize = d.LSize And
                    Me.RSize = d.RSize And
                    Me.CharSet = d.CharSet, True, False)
        End Function
#End Region

#Region "Dispose"
        Private disposed As Boolean

        '' <summary>
        '' 使用されているリソースを開放する
        '' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            Me.Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub


        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If disposed = True Then
                Return
            End If

            If disposing = True Then
                If Not Me.da Is Nothing Then
                    Me.da.Dispose()
                End If
            End If

            Me.disposed = True
        End Sub
        Protected Overridable Overloads Sub Finalize()
            Me.Dispose(False)
        End Sub
#End Region
    End Class
End Namespace

