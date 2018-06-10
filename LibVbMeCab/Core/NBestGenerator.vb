'  MeCab -- Yet Another Part-of-Speech and Morphological Analyzer
'
'  Copyright(C) 2001-2006 Taku Kudo <taku@chasen.org>
'  Copyright(C) 2004-2006 Nippon Telegraph and Telephone Corporation
'  Copyright(C) 2018      Ken'ichiro Ayaki 
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports VbMeCab

Namespace Core
    Public Class NBestGenerator
        Private Class QueueElement
            Implements IComparable(Of QueueElement)

            Public Property Node As MeCabNode

            Public Property _Next As QueueElement
            Public Property Fx As Long
            Public Property Gx As Long

            Public Overloads Function CompareTo(ByVal other As QueueElement) As Integer Implements IComparable(Of QueueElement).CompareTo
                Return Me.Fx.CompareTo(other.Fx)
            End Function

            Public Overrides Function ToString() As String
                Return Me.Node.ToString()
            End Function
        End Class
        Private agenda As PriorityQueue(Of QueueElement) = New PriorityQueue(Of QueueElement)()


        Public Sub _Set(ByVal node As MeCabNode)
            Do Until node._Next Is Nothing
                node = node._Next
            Loop
            ' seek to EOS;
            Me.agenda.Clear()
            Dim eos As QueueElement = New QueueElement()
            With eos
                .Node = node
                ._Next = Nothing
                .Fx = 0
                .Gx = 0
            End With

            Me.agenda.Push(eos)
        End Sub

        Public Function _Next() As MeCabNode
            While Me.agenda.Count <> 0
                Dim top As QueueElement = Me.agenda.Pop()
                Dim rNode As MeCabNode = top.Node

                If rNode.Stat = MeCabNodeStat.Bos Then
                    Dim n As QueueElement
                    n = top
                    Do Until (n._Next Is Nothing)
                        n.Node._Next = n._Next.Node ' change next & prev
                        n._Next.Node.Prev = n.Node
                        n = n._Next
                    Loop
                    Return rNode
                End If

                Dim path As MeCabPath
                path = rNode.LPath
                Do Until (path Is Nothing)
                    Dim n As QueueElement = New QueueElement()
                    With n
                        .Node = path.LNode
                        .Gx = path.Cost + top.Gx
                        .Fx = path.LNode.Cost + path.Cost + top.Gx
                        ._Next = top
                    End With
                    Me.agenda.Push(n)
                    path = path.LNext
                Loop
            End While
            Return Nothing
        End Function

        Public Iterator Function GetEnumerator() As IEnumerable(Of MeCabNode)
            Dim rNode As MeCabNode
            Do
                rNode = Me._Next()
                If rNode Is Nothing Then Exit Do
                Yield rNode
            Loop
        End Function
    End Class
End Namespace

