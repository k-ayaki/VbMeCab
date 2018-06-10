'  MeCab -- Yet Another Part-of-Speech and Morphological Analyzer
'
'  Copyright(C) 2001-2006 Taku Kudo <taku@chasen.org>
'  Copyright(C) 2004-2006 Nippon Telegraph and Telephone Corporation
'  Copyright(C) 2018      Ken'ichiro Ayaki 
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Globalization
Imports VbMeCab.Core

Public Class MeCabNode
    '' <summary>
    '' 一つ前の形態素
    '' </summary>
    Public Property Prev As MeCabNode

    '' <summary>
    '' 一つ先の形態素
    '' </summary>
    Public Property _Next As MeCabNode

    '' <summary>
    '' 同じ位置で終わる形態素
    '' </summary>
    Public Property ENext As MeCabNode

    '' <summary>
    '' 同じ開始位置で始まる形態素
    '' </summary>
    Public Property BNext As MeCabNode

    Friend Property RPath As MeCabPath

    Friend Property LPath As MeCabPath

    '' <summary>
    '' 形態素の文字列情報
    '' </summary>
    Public Property Surface As String

    Private _feature As String

    '' <summary>
    '' CSV で表記された素性情報
    '' </summary>
    Public Property Feature() As String
        Get
            If Me._feature = Nothing And (Not Me.Dictionary Is Nothing) Then
                Me._feature = Me.Dictionary.GetFeature(Me.featurePos)
            End If
            Return Me._feature
        End Get
        Set(ByVal Value As String)
            Me._feature = Value
        End Set
    End Property

    Private featurePos As System.UInt32

    Private Property Dictionary As MeCabDictionary

    '' <summary>
    '' 素性情報を遅延読込するための値設定
    '' </summary>
    '' <param name="featurePos">辞書内の素性情報の位置</param>
    '' <param name="dic">検索元の辞書</param>
    Friend Sub SetFeature(ByVal _featurePos As UInteger, ByVal dic As MeCabDictionary)
        Me.Feature = Nothing
        Me.featurePos = _featurePos
        Me.Dictionary = dic
    End Sub

    '' <summary>
    '' 形態素の長さ
    '' </summary>
    Public Property Length As Integer

    '' <summary>
    '' 形態素の長さ(先頭のスペースを含む)
    '' </summary>
    Public Property RLength As Integer

    '' <summary>
    '' 右文脈 id
    '' </summary>
    Public Property RCAttr As System.UInt16

    '' <summary>
    '' 左文脈 id
    '' </summary>
    Public Property LCAttr As System.UInt16

    '' <summary>
    '' 形態素 ID
    '' </summary>
    Public Property PosId As System.UInt16

    '' <summary>
    '' 文字種情報
    '' </summary>
    Public Property CharType As System.UInt32

    '' <summary>
    '' 形態素の種類
    '' </summary>
    Public Property Stat As MeCabNodeStat

    '' <summary>
    '' ベスト解
    '' </summary>
    Public Property IsBest As Boolean

    '' <summary>
    '' forward backward の foward log 確率
    '' </summary>
    Public Property Alpha As Single

    '' <summary>
    '' forward backward の backward log 確率
    '' </summary>
    Public Property Beta As Single

    '' <summary>
    '' 周辺確率
    '' </summary>
    Public Property Prob As Single

    '' <summary>
    '' 単語生起コスト
    '' </summary>
    Public Property WCost As Short

    '' <summary>
    '' 累積コスト
    '' </summary>
    Public Property Cost As Long

    Public Property BPos As Integer

    Public Property EPos As Integer

    Public Overrides Function ToString() As String
        Dim os As StringBuilder = New StringBuilder()
        os.Append("[Surface:")
        If Me.Stat = MeCabNodeStat.Bos Then
            os.Append("BOS")
        ElseIf Me.Stat = MeCabNodeStat.Eos Then
            os.Append("EOS")
        Else
            os.Append(Me.Surface)
        End If
        os.Append("]")

        os.Append("[Feature:").Append(Me.Feature).Append("]")
        os.Append("[BPos:").Append(Me.BPos).Append("]")
        os.Append("[EPos:").Append(Me.EPos).Append("]")
        os.Append("[RCAttr:").Append(Me.RCAttr).Append("]")
        os.Append("[LCAttr:").Append(Me.LCAttr).Append("]")
        os.Append("[PosId:").Append(Me.PosId).Append("]")
        os.Append("[CharType:").Append(Me.CharType).Append("]")
        os.Append("[Stat:").Append(CType(Me.Stat, Integer)).Append("]")
        os.Append("[IsBest:").Append(Me.IsBest).Append("]")
        os.Append("[Alpha:").Append(Me.Alpha).Append("]")
        os.Append("[Beta:").Append(Me.Beta).Append("]")
        os.Append("[Prob:").Append(Me.Prob).Append("]")
        os.Append("[Cost:").Append(Me.Cost).Append("]")

        Dim path As MeCabPath
        path = Me.LPath
        Do Until path Is Nothing
            os.Append("[Path:")
            os.Append("(Cost:").Append(path.Cost).Append(")")
            os.Append("(Prob:").Append(path.Prob).Append(")")
            os.Append("]")
            path = path.LNext
        Loop

        Return os.ToString()
    End Function
End Class
