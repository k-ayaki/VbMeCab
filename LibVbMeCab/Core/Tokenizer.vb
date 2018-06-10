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
    Public Class Tokenizer : Implements IDisposable
#Region "Const"
        Private Const SysDicFile As String = "sys.dic"
        Private Const UnkDicFile As String = "unk.dic"
        Private Const DAResultSize As Integer = 512
        Private Const DefaltMaxGroupingSize As Integer = 24
        Private Const BosKey As String = "BOS/EOS"

#End Region

#Region "Field"
        Private dic() As MeCabDictionary
        Private ReadOnly unkDic As MeCabDictionary = New MeCabDictionary()
        Private bosFeature As String
        Private unkFeature As String
        Private unkTokens()() As Token
        Private space As CharInfo
        Private ReadOnly _property As CharProperty = New CharProperty()
        Private maxGroupingSize As Integer

        Public Sub Open(ByVal param As MeCabParam)
            Me.dic = New MeCabDictionary(param.UserDic.Length) {}

            Dim prefix As String = param.DicDir

            Me._property.Open(prefix)

            Me.unkDic.Open(Path.Combine(prefix, UnkDicFile))
            If Me.unkDic.Type <> DictionaryType.Unk Then
                Throw New MeCabInvalidFileException("not a unk dictionary", Me.unkDic.FileName)
            End If

            Dim sysDic As MeCabDictionary = New MeCabDictionary()
            sysDic.Open(Path.Combine(prefix, SysDicFile))
            If sysDic.Type <> DictionaryType.Sys Then
                Throw New MeCabInvalidFileException("not a system dictionary", sysDic.FileName)
            End If
            Me.dic(0) = sysDic

            Dim i As Integer
            For i = 0 To param.UserDic.Length - 1
                Dim d As MeCabDictionary = New MeCabDictionary()
                d.Open(Path.Combine(prefix, param.UserDic(i)))
                If d.Type <> DictionaryType.Usr Then
                    Throw New MeCabInvalidFileException("not a user dictionary", d.FileName)
                End If
                If Not sysDic.IsCompatible(d) Then
                    Throw New MeCabInvalidFileException("incompatible dictionary", d.FileName)
                End If
                Me.dic(i + 1) = d
            Next

            Me.unkTokens = New Token(Me._property.Size - 1)() {}
            For i = 0 To Me.unkTokens.Length - 1
                Dim key As String = Me._property.Name(i)
                Dim n As DoubleArray.ResultPair = Me.unkDic.ExactMatchSearch(key)
                If n.Value = -1 Then
                    Throw New MeCabInvalidFileException("cannot find UNK category: " + key, Me.unkDic.FileName)
                End If
                Me.unkTokens(i) = Me.unkDic.GetToken(n)
            Next

            Me.space = Me._property.GetCharInfo(" "c)

            Me.bosFeature = param.BosFeature
            Me.unkFeature = param.UnkFeature

            Me.maxGroupingSize = param.MaxGroupingSize
            If Me.maxGroupingSize <= 0 Then
                Me.maxGroupingSize = DefaltMaxGroupingSize
            End If
        End Sub

#End Region

#Region "Lookup"
        Public Function Lookup(ByVal str As String) As MeCabNode
            Dim iBegin As Integer = 0
            Dim iEnd As Integer = str.Length
            Dim cInfo As CharInfo = Me._property.GetCharInfo(" "c)
            Dim resultNode As MeCabNode = Nothing
            Dim cLen As Integer = 0

            If iEnd - iBegin > UShort.MaxValue Then
                iEnd = iBegin + UShort.MaxValue
            End If
            Dim iBegin2 As Integer = Me._property.SeekToOtherType(str, iBegin, Me.space, cInfo, cLen)
            Dim daResults() As DoubleArray.ResultPair = New DoubleArray.ResultPair(DAResultSize - 1) {}

            Dim i As Integer
            Dim it As MeCabDictionary
            For Each it In Me.dic
                Dim n As Integer = it.CommonPrefixSearch(str, iBegin2, daResults, DAResultSize)

                For i = 0 To n - 1
                    Dim token() As Token = it.GetToken(daResults(i))
                    Dim j As Integer
                    For j = 0 To token.Length - 1
                        Dim NewNode As MeCabNode = Me.GetNewNode()
                        Me.ReadNodeInfo(it, token(j), NewNode)
                        'newNode.Token = token(j);
                        NewNode.Length = daResults(i).Length
                        NewNode.RLength = CType((iBegin2 - iBegin), Integer) + daResults(i).Length
                        NewNode.Surface = str.Substring(iBegin2, daResults(i).Length)
                        NewNode.Stat = MeCabNodeStat.Nor
                        NewNode.CharType = cInfo.DefaultType
                        NewNode.BNext = resultNode
                        resultNode = NewNode
                    Next
                Next
            Next

            If (Not (resultNode Is Nothing)) And cInfo.Invoke = False Then
                Return resultNode
            End If

            Dim iBegin3 As Integer = iBegin2 + 1
            Dim igroupBegin3 As Integer = -1

            If iBegin3 >= iEnd Then
                Me.AddUnknown(resultNode, cInfo, str, iBegin, iBegin2, iBegin3)
                Return resultNode
            End If

            If cInfo.Group = True Then
                Dim tmp As Integer = iBegin3
                Dim fail As CharInfo = Me._property.GetCharInfo(" "c)
                iBegin3 = Me._property.SeekToOtherType(str, iBegin3, cInfo, fail, cLen)
                If cLen <= maxGroupingSize Then
                    Me.AddUnknown(resultNode, cInfo, str, iBegin, iBegin2, iBegin3)
                End If
                igroupBegin3 = iBegin3
                iBegin3 = tmp
            End If

            For i = 1 To cInfo.Length
                If iBegin3 >= iEnd Then
                    Exit For
                End If
                If iBegin3 = igroupBegin3 Then
                    Continue For
                End If
                cLen = i
                Me.AddUnknown(resultNode, cInfo, str, iBegin, iBegin2, iBegin3)
                If Not cInfo.IsKindOf(Me._property.GetCharInfo(str(iBegin3))) Then
                    Exit For
                End If
                iBegin3 += 1
            Next

            If resultNode Is Nothing Then
                Me.AddUnknown(resultNode, cInfo, str, iBegin, iBegin2, iBegin3)
            End If

            Return resultNode
        End Function

        Private Sub ReadNodeInfo(ByVal dic As MeCabDictionary, ByRef token As Token, ByRef node As MeCabNode)
            node.LCAttr = token.LcAttr
            node.RCAttr = token.RcAttr
            node.PosId = token.PosId
            node.WCost = token.WCost
            'node.Token = token;
            'node.Feature = dic.GetFeature(token);  'この段階では素性情報を取得しない
            node.SetFeature(token.Feature, dic)     'そのかわり遅延取得を可能にする 
        End Sub

        ' string版
        Private Sub AddUnknown(ByRef resultNode As MeCabNode, ByVal cInfo As CharInfo, ByVal str As String, ByVal ibegin As Integer, ByVal ibegin2 As Integer, ByVal ibegin3 As Integer)
            Dim token As Token() = Me.unkTokens(cInfo.DefaultType)

            For i As Integer = 0 To token.Length - 1
                Dim newNode As MeCabNode = Me.GetNewNode()
                Me.ReadNodeInfo(Me.unkDic, token(i), newNode)
                newNode.CharType = cInfo.DefaultType
                newNode.Surface = str.Substring(ibegin2, CInt(ibegin3 - ibegin2))
                newNode.Length = CInt(ibegin3 - ibegin2)
                newNode.RLength = CInt(ibegin3 - ibegin)
                newNode.BNext = resultNode
                newNode.Stat = MeCabNodeStat.Unk
                If Me.unkFeature IsNot Nothing Then newNode.Feature = Me.unkFeature
                resultNode = newNode
            Next
        End Sub
#End Region

#Region "Get Node"
        Public Function GetBosNode() As MeCabNode
            Dim bosNode As MeCabNode = Me.GetNewNode()
            bosNode.Surface = BosKey ' dummy
            bosNode.Feature = Me.bosFeature
            bosNode.IsBest = True
            bosNode.Stat = MeCabNodeStat.Bos
            Return bosNode
        End Function

        Public Function GetEosNode() As MeCabNode
            Dim eosNode As MeCabNode = Me.GetBosNode()  ' same
            eosNode.Stat = MeCabNodeStat.Eos
            Return eosNode
        End Function

        Public Function GetNewNode() As MeCabNode
            Dim node As MeCabNode = New MeCabNode()
            Return node
        End Function
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
                If Not Me.dic Is Nothing Then
                    Dim d As MeCabDictionary
                    For Each d In Me.dic
                        If Not d Is Nothing Then
                            d.Dispose()
                        End If
                    Next
                End If

                If Not Me.unkDic Is Nothing Then
                    Me.unkDic.Dispose()
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

