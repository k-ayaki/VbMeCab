'  MeCab -- Yet Another Part-of-Speech and Morphological Analyzer
'
'  Copyright(C) 2001-2006 Taku Kudo <taku@chasen.org>
'  Copyright(C) 2004-2006 Nippon Telegraph and Telephone Corporation
'  Copyright(C) 2018      Ken'ichiro Ayaki 
Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Core
    Public Structure CharInfo
#Region "Const/Field/Property"

        Private ReadOnly bits As System.UInt32

        '' <summary>
        '' 互換カテゴリ
        '' </summary>
        Public ReadOnly Property Type() As System.UInt32
            Get
                Return BitUtils.GetBitField(Me.bits, 0, 18)
            End Get
        End Property

        '' <summary>
        '' デフォルトカテゴリ
        '' </summary>
        Public ReadOnly Property DefaultType() As System.UInt32
            Get
                Return BitUtils.GetBitField(Me.bits, 18, 8)
            End Get
        End Property

        '' <summary>
        '' 長さ: 未知語の候補生成方法
        '' </summary>
        '' <value>
        '' 1: 1文字までの文字列を未知語とする
        '' 2: 2文字までの文字列を未知語とする
        '' ...
        '' n: n文字までの文字列を未知語とする
        '' </value>
        Public ReadOnly Property Length() As System.UInt32
            Get
                Return BitUtils.GetBitField(Me.bits, 18 + 8, 4)
            End Get
        End Property

        '' <summary>
        '' グルーピング: 未知語の候補生成方法
        '' </summary>
        '' <value>
        '' true: 同じ字種でまとめる
        '' false: 同じ字種でまとめない
        '' </value>
        Public ReadOnly Property Group() As Boolean
            Get
                Return BitUtils.GetFlag(Me.bits, 18 + 8 + 4)
            End Get
        End Property

        '' <summary>
        '' 動作タイミング
        '' そのカテゴリにおいて, いつ未知語処理を動かすか
        '' </summary>
        '' <value>
        '' true: 常に未知語処理を動かす
        '' false: 既知語がある場合は, 未知語処理を動作させない
        '' </value>
        Public ReadOnly Property Invoke() As Boolean
            Get
                Return BitUtils.GetFlag(Me.bits, 18 + 8 + 4 + 1)
            End Get
        End Property

#End Region

#Region "Constractor"

        Public Sub New(ByVal bits As System.UInt32)
            Me.bits = bits
        End Sub
#End Region

#Region "Method"


        '' <summary>
        '' 互換カテゴリ判定
        '' </summary>
        '' <param name="c"></param>
        '' <returns></returns>
        Public Function IsKindOf(ByVal c As CharInfo) As Boolean
            Return BitUtils.CompareAnd(Me.bits, c.bits, 0, 18)
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("[Type:{0}][DefaultType:{1}][Length:{2}][Group:{3}][Invoke:{4}]",
                                 Me.Type,
                                 Me.DefaultType,
                                 Me.Length,
                                 Me.Group,
                                 Me.Invoke)
        End Function
#End Region

    End Structure
End Namespace

