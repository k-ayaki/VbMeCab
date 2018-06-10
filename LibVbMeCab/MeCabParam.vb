'  MeCab -- Yet Another Part-of-Speech and Morphological Analyzer
'
'  Copyright(C) 2001-2006 Taku Kudo <taku@chasen.org>
'  Copyright(C) 2004-2006 Nippon Telegraph and Telephone Corporation
'  Copyright(C) 2018      Ken'ichiro Ayaki 
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports VbMeCab.Core

Public Class MeCabParam
    Public Property DicDir As String

    Public Property UserDic As String()

    Public Property MaxGroupingSize As Integer

    '' <summary>
    '' 文頭, 文末の素性
    '' </summary>
    Public Property BosFeature As String

    Public Property UnkFeature As String

    Public Property AlloCateSentence As Boolean

    '' <summary>
    '' コスト値に変換するときのスケーリングファクター
    '' </summary>
    Public Property CostFactor As Integer

    Public Const DefaultTheta As Single = 0.75F

    '' <summary>
    '' ソフト分かち書きの温度パラメータ
    '' </summary>
    Public Property Theta As Single

    '' <summary>
    '' ラティスレベル(どの程度のラティス情報を解析時に構築するか)
    '' </summary>
    Public Property LatticeLevel As MeCabLatticeLevel

    '' <summary>
    '' 部分解析
    '' </summary>
    Public Property _Partial As Boolean

    '' <summary>
    '' 出力モード
    '' </summary>
    '' <value>
    '' true: 全出力
    '' false: ベスト解のみ
    '' </value>
    Public Property AllMorphs As Boolean

    Public Property OutputFormatType As String

    Public Const DefaultRcFile As String = "dicrc"

    Public Property RcFile As String

    '' <summary>
    '' コンストラクタ
    '' </summary>
    Public Sub New()
        Me.Theta = MeCabParam.DefaultTheta
        Me.RcFile = MeCabParam.DefaultRcFile

        Me.DicDir = My.Settings.DicDir
        Me.UserDic = Me.SplitStringArray(My.Settings.UserDic, ","c)
        Me.OutputFormatType = My.Settings.OutputFormatType
    End Sub

    Public Sub LoadDicRC()
        Dim rc As String = Path.Combine(Me.DicDir, Me.RcFile)
        Me.Load(rc)
    End Sub

    Public Sub Load(ByVal path As String)
        Dim ini As IniParser = New IniParser()
        ini.Load(path, Encoding.ASCII)

        Dim cf As String = ini("cost-factor")
        If cf Is Nothing Or cf.Length = 0 Then
            cf = "0"
        End If
        Me.CostFactor = Integer.Parse(cf)
        Me.BosFeature = ini("bos-feature")
    End Sub

    Private Function SplitStringArray(ByVal configStr As String, ByVal separator As Char) As String()
        If String.IsNullOrEmpty(configStr) = True Then
            Return {}
        End If

        Dim ret() As String = configStr.Split(separator)

        Dim i As Integer
        For i = 0 To ret.Length - 1
            ret(i) = ret(i).Trim()
        Next
        Return ret
    End Function
End Class
