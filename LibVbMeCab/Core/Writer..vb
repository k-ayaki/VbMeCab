'  MeCab -- Yet Another Part-of-Speech and Morphological Analyzer
'
'  Copyright(C) 2001-2006 Taku Kudo <taku@chasen.org>
'  Copyright(C) 2004-2006 Nippon Telegraph and Telephone Corporation
'  Copyright(C) 2018      Ken'ichiro Ayaki 
Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Core
    Public Class Writer
        Private Const FloatFormat As String = "f6"

        Private Delegate Sub WriteAction(ByVal os As StringBuilder, ByVal bosNode As MeCabNode)
        Private _write As WriteAction

        Private _outputFormatType As String

        Public Property OutputFormatType() As String
            Get
                Return Me._outputFormatType
            End Get
            Set(ByVal Value As String)
                Me._outputFormatType = Value
                Select Case Value
                    Case "lattice"
                        Me._write = AddressOf Me.WriteLattice
                        Exit Property
                    Case "wakati"
                        Me._write = AddressOf Me.WriteWakati
                        Exit Property
                    Case "none"
                        Me._write = AddressOf Me.WriteNone
                        Exit Property
                    Case "dump"
                        Me._write = AddressOf Me.WriteDump
                        Exit Property
                    Case "em"
                        Me._write = AddressOf Me.WriteEM
                        Exit Property
                    Case Else
                        Throw New ArgumentOutOfRangeException(Value + " is not supported Format")
                End Select
            End Set
        End Property

        Public Sub Open(ByVal param As MeCabParam)
            Me.OutputFormatType = param.OutputFormatType
        End Sub

        Public Sub Write(ByVal os As StringBuilder, ByVal bosNode As MeCabNode)
            Me._write(os, bosNode)
        End Sub

        Public Sub WriteLattice(ByVal os As StringBuilder, ByVal bosNode As MeCabNode)
            Dim node As MeCabNode
            node = bosNode._Next
            Do Until (node._Next Is Nothing)
                os.Append(node.Surface)
                os.Append(vbTab)
                os.Append(node.Feature)
                os.AppendLine()
                node = node._Next
            Loop
            os.AppendLine("EOS")
        End Sub

        Public Sub WriteWakati(ByVal os As StringBuilder, ByVal bosNode As MeCabNode)
            Dim node As MeCabNode = bosNode._Next
            If Not node._Next Is Nothing Then
                os.Append(node.Surface)
                node = node._Next
                Do Until node._Next Is Nothing
                    os.Append(" ")
                    os.Append(node.Surface)
                    node = node._Next
                Loop
            End If
            os.AppendLine()
        End Sub

        Public Sub WriteNone(ByVal os As StringBuilder, ByVal bosNode As MeCabNode)
            ' do nothing
        End Sub

        Public Sub WriteUser(ByVal os As StringBuilder, ByVal bosNode As MeCabNode)
            Throw New NotImplementedException()
        End Sub

        Public Sub WriteEM(ByVal os As StringBuilder, ByVal bosNode As MeCabNode)
            Const MinProb As Single = 0.0001F
            Dim node As MeCabNode
            node = bosNode
            Do Until node Is Nothing
                If node.Prob >= MinProb Then
                    os.Append("U" + vbTab)
                    If node.Stat = MeCabNodeStat.Bos Then
                        os.Append("BOS")
                    ElseIf node.Stat = MeCabNodeStat.Eos Then
                        os.Append("EOS")
                    Else
                        os.Append(node.Surface)
                    End If
                    os.Append(vbTab).Append(node.Feature)
                    os.Append(vbTab).Append(node.Prob.ToString(FloatFormat))
                    os.AppendLine()
                End If
                Dim path As MeCabPath
                path = node.LPath
                Do Until path Is Nothing
                    If path.Prob >= MinProb Then
                        os.Append("B" + vbTab).Append(path.LNode.Feature)
                        os.Append(vbTab).Append(node.Feature)
                        os.Append(vbTab).Append(path.Prob.ToString(FloatFormat))
                        os.AppendLine()
                    End If
                    path = path.LNext
                Loop
                node = node._Next
            Loop
            os.AppendLine("EOS")
        End Sub

        Public Sub WriteDump(ByVal os As StringBuilder, ByVal bosNode As MeCabNode)
            Dim node As MeCabNode
            node = bosNode
            Do Until node Is Nothing
                If node.Stat = MeCabNodeStat.Bos Then
                    os.Append("BOS")
                ElseIf node.Stat = MeCabNodeStat.Eos Then
                    os.Append("EOS")
                Else
                    os.Append(node.Surface)
                End If

                os.Append(" ").Append(node.Feature)
                os.Append(" ").Append(node.BPos)
                os.Append(" ").Append(node.EPos)
                os.Append(" ").Append(node.RCAttr)
                os.Append(" ").Append(node.LCAttr)
                os.Append(" ").Append(node.PosId)
                os.Append(" ").Append(node.CharType)
                os.Append(" ").Append(CType(node.Stat, Integer))
                os.Append(" ").Append(If(node.IsBest = True, "1", "0"))
                os.Append(" ").Append(node.Alpha.ToString(FloatFormat))
                os.Append(" ").Append(node.Beta.ToString(FloatFormat))
                os.Append(" ").Append(node.Prob.ToString(FloatFormat))
                os.Append(" ").Append(node.Cost)

                Dim path As MeCabPath
                path = node.LPath
                Do Until path Is Nothing
                    os.Append(" ")
                    os.Append(":").Append(path.Cost)
                    os.Append(":").Append(path.Prob.ToString(FloatFormat))
                    path = path.LNext
                Loop

                os.AppendLine()
                node = node._Next
            Loop
        End Sub
        Private Function GetEscapedChar(ByVal p As Char) As Char
            Select Case p
                Case "0"c
                    Return vbNullChar
                Case "a"c
                    Return Chr(7)
                Case "b"c
                    Return vbBack
                Case "t"c
                    Return vbTab
                Case "n"c
                    Return vbCr
                Case "v"c
                    Return vbVerticalTab
                Case "f"c
                    Return vbFormFeed
                Case "r"c
                    Return vbLf
                Case "s"c
                    Return " "c
                Case "\"c
                    Return "\"c
                Case Else
                    Return vbNullChar
            End Select
        End Function
    End Class
End Namespace
