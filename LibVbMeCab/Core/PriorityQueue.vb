Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Core
    '' <summary>
    '' 優先度付き先入れ先出しコレクション（実装アルゴリズムはPairing Heap）
    '' </summary>
    '' <typeparam name="T"></typeparam>
    Public Class PriorityQueue(Of T As IComparable(Of T))

        Private Class HeapNode
            Public Property Value As T
            Public Property ChildsCount As Integer
            Private firstChild As HeapNode
            Private lastChild As HeapNode
            Private _next As HeapNode

            Public Sub AddFirstChild(ByVal newNode As HeapNode)
                Me.ChildsCount += 1
                If Me.ChildsCount = 1 Then
                    Me.lastChild = newNode
                Else
                    newNode._next = Me.firstChild
                End If
                Me.firstChild = newNode
            End Sub

            Public Sub AddLastChild(ByVal newNode As HeapNode)
                Me.ChildsCount += 1
                If Me.ChildsCount = 1 Then
                    Me.firstChild = newNode
                Else
                    Me.lastChild._next = newNode
                End If
                Me.lastChild = newNode
            End Sub

            Public Function PollFirstChild() As HeapNode
                Dim ret As HeapNode = Me.firstChild
                Me.ChildsCount -= 1
                If Me.ChildsCount = 0 Then
                    Me.firstChild = Nothing
                    Me.lastChild = Nothing
                Else
                    Me.firstChild = ret._next
                    ret._next = Nothing
                End If
                Return ret
            End Function

            Public Sub New(ByVal _val As T)
                Me.Value = _val
            End Sub
        End Class

        Private rootNode As HeapNode

        Public Property Count As Integer

        Public Sub Clear()
            Me.Count = 0
            Me.rootNode = Nothing
        End Sub

        Public Sub Push(ByVal item As T)
            Me.Count += 1
            Me.rootNode = Me.MergeNodes(Me.rootNode, New HeapNode(item))
        End Sub

        Public Function Pop() As T
            If Me.Count = 0 Then
                Throw New InvalidOperationException("Empty")
            End If

            Me.Count -= 1
            Dim ret As T = Me.rootNode.Value
            Me.rootNode = Me.UnifyChildNodes(Me.rootNode)
            Return ret
        End Function

        Private Function MergeNodes(ByVal l As HeapNode, ByVal r As HeapNode) As HeapNode
            If l Is Nothing Then
                Return r
            End If
            If r Is Nothing Then
                Return l
            End If

            If l.Value.CompareTo(r.Value) > 0 Then
                r.AddFirstChild(l)
                Return r
            Else
                l.AddLastChild(r)
                Return l
            End If
        End Function

        Private Function UnifyChildNodes(ByVal node As HeapNode) As HeapNode
            Dim j As Integer = node.ChildsCount \ 2
            Dim tmp(j - 1) As HeapNode  '必要な要素数が明らかなのでStackではなく配列

            Dim i As Integer
            For i = 0 To tmp.Length - 1
                Dim x As HeapNode = node.PollFirstChild()
                Dim y As HeapNode = node.PollFirstChild()
                tmp(i) = Me.MergeNodes(x, y)
            Next

            Dim z As HeapNode
            If node.ChildsCount = 1 Then
                z = node.PollFirstChild()
            Else
                z = Nothing
            End If

            For i = tmp.Length - 1 To 0 Step -1
                z = Me.MergeNodes(tmp(i), z)
            Next

            Return z
        End Function
    End Class
End Namespace

