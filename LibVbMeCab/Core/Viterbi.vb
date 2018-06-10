'  MeCab -- Yet Another Part-of-Speech and Morphological Analyzer
'
'  Copyright(C) 2001-2006 Taku Kudo <taku@chasen.org>
'  Copyright(C) 2004-2006 Nippon Telegraph and Telephone Corporation
'  Copyright(C) 2018      Ken'ichiro Ayaki 
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Runtime.InteropServices

Namespace Core
    Public Class Viterbi : Implements IDisposable
#Region "InnerClass"
        Private Class ThreadData
            Public EosNode As MeCabNode
            Public BosNode As MeCabNode
            Public EndNodeList() As MeCabNode
            Public BeginNodeList() As MeCabNode
            Public Z As Single
        End Class
#End Region

#Region "Field/Property"
        Private ReadOnly tokenizer As Tokenizer = New Tokenizer()
        Private ReadOnly connector As Connector = New Connector()

        Private level As MeCabLatticeLevel
        Private _theta As Single
        Private costFactor As Integer

        Public Property Theta() As Single
            Get
                Return Me._theta * Me.costFactor
            End Get
            Set(ByVal Value As Single)
                Me._theta = Value / Me.costFactor
            End Set
        End Property

        Public Property LatticeLevel() As MeCabLatticeLevel
            Get
                Return Me.level
            End Get
            Set(ByVal Value As MeCabLatticeLevel)
                Me.level = Value
                Me.connect = AddressOf Me.ConnectNomal
                Me._analyze = AddressOf Me.DoViterbi
                If Value >= MeCabLatticeLevel.One Then
                    Me.connect = AddressOf Me.ConnectWithAllPath
                End If
                If Value >= MeCabLatticeLevel.Two Then
                    Me._analyze = AddressOf Me.ForwardBackward
                End If
            End Set
        End Property

        Public Property _Partial As Boolean

        Public Property AllMorphs() As Boolean
            Get
                If System.Delegate.Equals(CType(AddressOf Me.BuildAllLattice, BuildLatticeFunc), Me.buildLattice) = True Then
                    Return True
                End If
                Return False
            End Get
            Set(ByVal Value As Boolean)
                If Value = True Then
                    Me.buildLattice = AddressOf Me.BuildAllLattice
                Else
                    Me.buildLattice = AddressOf Me.BuildBestLattice
                End If
            End Set
        End Property
#End Region
#Region "Open/Clear"
        Public Sub Open(ByVal param As MeCabParam)
            tokenizer.Open(param)
            connector.Open(param)

            Me.costFactor = param.CostFactor
            Me.Theta = param.Theta
            Me.LatticeLevel = param.LatticeLevel
            Me._Partial = param._Partial
            Me.AllMorphs = param.AllMorphs
        End Sub

#End Region

#Region "AnalyzeStart"
        ' string版
        Public Function Analyze(ByVal str As String) As MeCabNode
            Dim work As ThreadData = New ThreadData()
            With work
                .EndNodeList = New MeCabNode(str.Length + 3) {}
                .BeginNodeList = New MeCabNode(str.Length + 3) {}
            End With

            If Me._Partial = True Then
                Dim NewStr As String = Me.InitConstraints(str, work)
                Me._analyze(NewStr, work)
                Return Me.buildLattice(work)
            End If
            Me._analyze(str, work)
            Return Me.buildLattice(work)
        End Function
#End Region

#Region "Analyze"
        Private Delegate Sub AnalyzeAction(ByVal sentence As String, ByRef work As ThreadData)

        Private _analyze As AnalyzeAction

        ' string版
        Private Sub ForwardBackward(ByVal sentence As String, ByRef work As ThreadData)
            Me.DoViterbi(sentence, work)

            work.EndNodeList(0).Alpha = 0.0
            Dim pos As Integer
            For pos = 0 To sentence.Length - 1
                Dim node As MeCabNode = work.BeginNodeList(pos)
                Do Until (node Is Nothing)
                    Me.CalcAlpha(node, Me.Theta)
                    node = node.BNext
                Loop
            Next

            work.BeginNodeList(sentence.Length).Beta = 0
            For pos = sentence.Length - 1 To 0 Step -1
                Dim node As MeCabNode = work.EndNodeList(pos)
                Do Until (node Is Nothing)
                    Me.CalcBeta(node, Me.Theta)
                    node = node.ENext
                Loop
            Next
            work.Z = work.BeginNodeList(sentence.Length).Alpha ' alpha of EOS

            For pos = 0 To sentence.Length - 1
                Dim node As MeCabNode = work.BeginNodeList(pos)
                Do Until (node Is Nothing)
                    node.Prob = CType(Math.Exp(node.Alpha + node.Beta - work.Z), Single)
                    node = node.BNext
                Loop
            Next

        End Sub

        Private Sub CalcAlpha(ByRef n As MeCabNode, ByRef beta As Double)
            n.Alpha = 0.0
            Dim path As MeCabPath
            path = n.LPath
            Do Until (path Is Nothing)
                n.Alpha = CType(Utils.LogSumExp(n.Alpha,
                                         -beta * path.Cost + path.LNode.Alpha,
                                         If(path Is n.LPath, True, False)), Single)
                path = path.LNext
            Loop
        End Sub

        Private Sub CalcBeta(ByRef n As MeCabNode, ByRef beta As Double)
            n.Beta = 0.0
            Dim path As MeCabPath
            path = n.RPath
            Do Until (path Is Nothing)
                n.Beta = CType(Utils.LogSumExp(n.Beta,
                                                -beta * path.Cost + path.RNode.Beta,
                                                If(path Is n.RPath, True, False)), Single)
                path = path.RNext
            Loop
        End Sub

        ' string版
        Private Sub DoViterbi(ByVal sentence As String, ByRef work As ThreadData)
            work.BosNode = Me.tokenizer.GetBosNode()
            work.BosNode.Length = sentence.Length

            work.BosNode.Surface = sentence
            work.EndNodeList(0) = work.BosNode

            Dim pos As Integer
            For pos = 0 To sentence.Length - 1
                If Not work.EndNodeList(pos) Is Nothing Then
                    Dim rNode As MeCabNode = tokenizer.Lookup(sentence.Substring(pos))
                    rNode = Me.FilterNode(rNode, pos, work)
                    rNode.BPos = pos
                    rNode.EPos = pos + rNode.RLength
                    work.BeginNodeList(pos) = rNode
                    Me.connect(pos, rNode, work)
                End If
            Next

            work.EosNode = tokenizer.GetEosNode()
            work.EosNode.Surface = vbNullString
            work.BeginNodeList(sentence.Length) = work.EosNode
            For pos = sentence.Length To 0 Step -1
                If Not work.EndNodeList(pos) Is Nothing Then
                    Me.connect(pos, work.EosNode, work)
                    Exit For
                End If
            Next
        End Sub

#End Region

#Region "Connect"

        Private Delegate Sub ConnectAction(ByVal pos As Integer, ByVal rNode As MeCabNode, ByVal work As ThreadData)

        Private connect As ConnectAction

        Private Sub ConnectWithAllPath(ByVal pos As Integer, ByVal rNode As MeCabNode, ByVal work As ThreadData)
            Do Until rNode Is Nothing
                Dim bestCost As Long = Integer.MaxValue  ' 2147483647

                Dim bestNode As MeCabNode = Nothing

                Dim lNode As MeCabNode
                lNode = work.EndNodeList(pos)
                Do Until (lNode Is Nothing)
                    Dim lCost As Integer = Me.connector.Cost(lNode, rNode)  ' local cost
                    Dim cost As Long = lNode.Cost + lCost

                    If cost < bestCost Then
                        bestNode = lNode
                        bestCost = cost
                    End If

                    Dim path As MeCabPath = New MeCabPath
                    With path
                        .Cost = lCost
                        .RNode = rNode
                        .LNode = lNode
                        .LNext = rNode.LPath
                        .RNext = lNode.RPath
                    End With

                    rNode.LPath = path
                    lNode.RPath = path
                    lNode = lNode.ENext
                Loop

                If bestNode Is Nothing Then
                    Throw New ArgumentException("too long sentence.")
                End If

                rNode.Prev = bestNode
                rNode._Next = Nothing
                rNode.Cost = bestCost
                Dim x As Integer = rNode.RLength + pos
                rNode.ENext = work.EndNodeList(x)
                work.EndNodeList(x) = rNode
                rNode = rNode.BNext
            Loop
        End Sub

        Private Sub ConnectNomal(ByVal pos As Integer, ByVal rNode As MeCabNode, ByVal work As ThreadData)
            Do Until rNode Is Nothing
                Dim bestCost As Long = Integer.MaxValue  ' 2147483647

                Dim bestNode As MeCabNode = Nothing

                Dim lNode As MeCabNode

                lNode = work.EndNodeList(pos)
                Do Until (lNode Is Nothing)
                    Dim cost As Long = lNode.Cost + Me.connector.Cost(lNode, rNode)

                    If cost < bestCost Then
                        bestNode = lNode
                        bestCost = cost
                    End If
                    lNode = lNode.ENext
                Loop

                If bestNode Is Nothing Then
                    Throw New MeCabException("too long sentence.")
                End If

                rNode.Prev = bestNode
                rNode._Next = Nothing
                rNode.Cost = bestCost
                Dim x As Integer = rNode.RLength + pos
                rNode.ENext = work.EndNodeList(x)
                work.EndNodeList(x) = rNode
                rNode = rNode.BNext
            Loop
        End Sub
#End Region

#Region "Lattice"
        Private Delegate Function BuildLatticeFunc(work As ThreadData) As MeCabNode

        Private buildLattice As BuildLatticeFunc

        Private Function BuildAllLattice(ByVal work As ThreadData) As MeCabNode
            If Me.BuildBestLattice(work) Is Nothing Then
                Return Nothing
            End If

            Dim prev As MeCabNode = work.BosNode

            Dim pos As Integer
            For pos = 0 To work.BeginNodeList.Length - 1
                Dim node As MeCabNode
                node = work.BeginNodeList(pos)
                Do Until (node Is Nothing)
                    prev._Next = node
                    node.Prev = prev
                    prev = node
                    Dim path As MeCabPath
                    path = node.LPath
                    Do Until (path Is Nothing)
                        path.Prob = CType((path.LNode.Alpha -
                            Me.Theta * path.Cost +
                            path.RNode.Beta - work.Z), Single)
                        path = path.LNext
                    Loop
                    node = node.BNext
                Loop
            Next

            Return work.BosNode
        End Function

        Private Function BuildBestLattice(ByVal work As ThreadData) As MeCabNode
            Dim node As MeCabNode = work.EosNode
            Dim prevNode As MeCabNode
            Do Until (node.Prev Is Nothing)
                node.IsBest = True
                prevNode = node.Prev
                prevNode._Next = node
                node = prevNode
            Loop
            Return work.BosNode
        End Function

#End Region

#Region "Partial"
        ' string版
        Private Function InitConstraints(ByVal str As String, ByVal work As ThreadData) As String
            Dim os As StringBuilder = New StringBuilder()
            os.Append(" "c)
            Dim pos As Integer = 0

            Dim line As String
            Dim sep() As Char = {vbCr, vbLf}
            For Each line In str.Split(sep)
                If line = "" Then Continue For
                If line = "EOS" Then Exit For

                Dim column() As String = line.Split(vbTab)
                os.Append(column(0)).Append(" "c)
                Dim len As Integer = column(0).Length

                If column.Length = 2 Then
                    If column(1) = vbNullChar Then
                        Throw New ArgumentException("use tab as separator")
                    End If
                    Dim c As MeCabNode = Me.tokenizer.GetNewNode()
                    c.Surface = column(0)
                    c.Feature = column(1)
                    c.Length = len
                    c.RLength = len + 1
                    c.BNext = Nothing
                    c.WCost = 0
                    work.BeginNodeList(pos) = c
                End If

                pos += len + 1
            Next
            Return os.ToString()
        End Function


        Private Function FilterNode(ByVal node As MeCabNode, ByVal pos As Integer, ByVal work As ThreadData) As MeCabNode
            If Not Me._Partial Then
                Return node
            End If

            Dim c As MeCabNode = work.BeginNodeList(pos)
            If c Is Nothing Then
                Return node
            End If
            Dim wild As Boolean = If(c.Feature = "*", True, False)

            Dim prev As MeCabNode = Nothing
            Dim result As MeCabNode = Nothing

            Dim n As MeCabNode
            n = node
            Do Until (n Is Nothing)
                If c.Surface = n.Surface And (wild Or Me.PartialMatch(c.Feature, n.Feature)) Then
                    If Not (prev Is Nothing) Then
                        prev.BNext = n
                        prev = n
                    Else
                        result = n
                        prev = result
                    End If
                End If
                n = n.BNext
            Loop
            If result Is Nothing Then
                result = c
            End If
            If Not (prev Is Nothing) Then
                prev.BNext = Nothing
            End If

            Return result
        End Function


        Private Function PartialMatch(ByVal f1 As String, ByVal f2 As String) As Boolean
            Dim c1() As String = f1.Split(","c)
            Dim c2() As String = f2.Split(","c)

            Dim n As Integer = Math.Min(c1.Length, c2.Length)

            Dim i As Integer
            For i = 0 To n - 1
                If c1(i) <> "*" And c2(i) <> "*" And c1(i) <> c2(i) Then
                    Return False
                End If
            Next

            Return True
        End Function
#End Region

#Region "Dispose"
        Private disposed As Boolean

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
                Me.tokenizer.Dispose()
                Me.connector.Dispose()
            End If
            Me.disposed = True
        End Sub

        Protected Overridable Overloads Sub Finalize()
            Dispose(False)
        End Sub
#End Region
    End Class
End Namespace

