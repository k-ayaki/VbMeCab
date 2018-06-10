'  MeCab -- Yet Another Part-of-Speech and Morphological Analyzer
'
'  Copyright(C) 2001-2006 Taku Kudo <taku@chasen.org>
'  Copyright(C) 2004-2006 Nippon Telegraph and Telephone Corporation
'  Copyright(C) 2018      Ken'ichiro Ayaki 
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.IO.MemoryMappedFiles


Namespace Core
    Public Class Connector : Implements IDisposable
#Region "Const/Field/Property"
        Private Const MatrixFile As String = "matrix.bin"

        Private matrix() As Short

        Public Property LSize As ULong

        Public Property RSize As ULong

#End Region

#Region "Open"
        Public Sub Open(ByVal param As MeCabParam)
            Dim fileName As String = Path.Combine(param.DicDir, MatrixFile)
            Me.Open(fileName)
        End Sub

        Public Sub Open(ByVal fileName As String)
            Using stream As FileStream = New FileStream(fileName, FileMode.Open, FileAccess.Read)
                Using reader As BinaryReader = New BinaryReader(stream)
                    Me.Open(reader, fileName)
                End Using
            End Using
        End Sub

        Public Sub Open(ByVal reader As BinaryReader, Optional ByVal fileName As String = vbNullString)
            Me.LSize = reader.ReadUInt16()
            Me.RSize = reader.ReadUInt16()
            Dim msize As ULong = Me.LSize * Me.RSize

            Me.matrix = New Short(msize - 1) {}
            Dim i As Integer
            For i = 0 To Me.matrix.Length - 1
                Me.matrix(i) = reader.ReadInt16()
            Next

            If reader.BaseStream.ReadByte() <> -1 Then
                Throw New MeCabInvalidFileException("file size is invalid", fileName)
            End If
        End Sub
#End Region

#Region "Cost"
        Public Function Cost(ByVal lNode As MeCabNode, ByVal rNode As MeCabNode) As Integer
            Dim pos As Integer = lNode.RCAttr + Me.LSize * rNode.LCAttr

            Return Me.matrix(pos) + rNode.WCost
        End Function
#End Region

#Region "Dispose"
        Private disposed As Boolean = False

        '' <summary>
        '' 使用中のリソースを開放する
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

            End If

            Me.disposed = True
        End Sub

        Protected Overridable Overloads Sub Finalize()
            Dispose(False)
        End Sub
#End Region
    End Class
End Namespace

