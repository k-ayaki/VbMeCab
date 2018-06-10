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
    '' <summary>
    '' Double-Array Trie の実装
    '' </summary>
    Public Class DoubleArray : Implements IDisposable

#Region "Array"
        Private Structure Unit
            Public ReadOnly Base As Integer
            Public ReadOnly Check As System.UInt32

            Public Sub New(ByVal b As Integer, ByVal c As System.UInt32)
                Me.Base = b
                Me.Check = c
            End Sub
        End Structure

        Public UnitSize As Integer = Marshal.SizeOf(Of Integer) + Marshal.SizeOf(Of System.UInt32)

        Private array() As Unit

        Public ReadOnly Property Size() As Integer
            Get
                Return Me.array.Length
            End Get
        End Property

        Public ReadOnly Property TotalSize() As Integer
            Get
                Return Me.Size * UnitSize
            End Get
        End Property
#End Region

#Region "Open"
        Public Sub Open(ByVal reader As BinaryReader, ByVal size As System.UInt32)
            Me.array = New Unit(size / UnitSize - 1) {}

            Dim i As Integer
            For i = 0 To array.Length - 1
                Me.array(i) = New Unit(reader.ReadInt32(), reader.ReadUInt32())
            Next
        End Sub
#End Region

#Region "Search"
        Public Structure ResultPair
            Public Value As Integer

            Public Length As Integer

            Public Sub New(ByVal r As Integer, ByVal t As Integer)
                Me.Value = r
                Me.Length = t
            End Sub
        End Structure

        Public Sub ExactMatchSearch(ByVal key() As Byte, ByRef result() As ResultPair, ByVal len As Integer, ByVal nodePos As Integer)
            result(0) = Me.ExactMatchSearch(key, len, nodePos)
        End Sub

        Public Function ExactMatchSearch(ByVal key() As Byte, ByVal len As Integer, ByVal nodePos As Integer) As ResultPair
            Dim b As Integer = Me.ReadBase(nodePos)
            Dim p As Unit

            Dim i As Integer
            For i = 0 To len - 1
                Me.ReadUnit(b + key(i) + 1, p)
                If b = p.Check Then
                    b = p.Base
                Else
                    Return New ResultPair(-1, 0)
                End If
            Next

            Me.ReadUnit(b, p)
            Dim n As Integer = p.Base
            If b = p.Check And n < 0 Then
                Return New ResultPair(-n - 1, len)
            End If

            Return New ResultPair(-1, 0)
        End Function

        Public Function CommonPrefixSearch(ByVal key() As Byte, ByRef result() As ResultPair, ByRef resultLen As Integer, ByVal len As Integer, Optional ByVal nodePos As Integer = 0) As Integer
            Dim b As Integer = Me.ReadBase(nodePos)
            Dim num As Integer = 0
            Dim n As Integer
            Dim p As Unit

            Dim i As Integer
            For i = 0 To len - 1
                Me.ReadUnit(b, p)
                n = p.Base

                If b = p.Check And n < 0 Then
                    If num < resultLen Then
                        result(num) = New ResultPair(-n - 1, i)
                    End If
                    num += 1
                End If

                Me.ReadUnit(b + key(i) + 1, p)
                If b = p.Check Then
                    b = p.Base
                Else
                    Return num
                End If
            Next

            Me.ReadUnit(b, p)
            n = p.Base

            If b = p.Check And n < 0 Then
                If num < resultLen Then
                    result(num) = New ResultPair(-n - 1, len)
                End If
                num += 1
            End If

            Return num
        End Function

        Private Function ReadBase(ByVal pos As Integer) As Integer
            Return Me.array(pos).Base
        End Function

        Private Sub ReadUnit(ByVal pos As Integer, ByRef unit As Unit)
            unit = Me.array(pos)
        End Sub
#End Region

#Region "Dispose"
        Private disposed As Boolean

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
            Me.Dispose(False)
        End Sub
#End Region
    End Class
End Namespace