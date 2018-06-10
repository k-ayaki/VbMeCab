'  MeCab -- Yet Another Part-of-Speech and Morphological Analyzer
'
'  Copyright(C) 2001-2006 Taku Kudo <taku@chasen.org>
'  Copyright(C) 2004-2006 Nippon Telegraph and Telephone Corporation
'  Copyright(C) 2018      Ken'ichiro Ayaki 
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports VbMeCab.Core
Imports System.Runtime.InteropServices

Public Class MeCabTagger : Implements IDisposable
    Private ReadOnly _viterbi As Viterbi = New Viterbi()
    Private ReadOnly _writer As Writer = New Writer()

#Region "Mode"
    '' <summary>
    '' 部分解析モード
    '' </summary>
    Public Property _Partial() As Boolean
        Get
            Me.ThrowIfDisposed()
            Return Me._viterbi._Partial
        End Get
        Set(ByVal Value As Boolean)
            Me.ThrowIfDisposed()
            Me._viterbi._Partial = Value
        End Set
    End Property

    '' <summary>
    '' ソフト分かち書きの温度パラメータ
    '' </summary>
    Public Property Theta() As Single
        Get
            Me.ThrowIfDisposed()
            Return Me._viterbi.Theta
        End Get
        Set(ByVal Value As Single)
            Me.ThrowIfDisposed()
            Me._viterbi.Theta = Value
        End Set
    End Property

    '' <summary>
    '' ラティスレベル(どの程度のラティス情報を解析時に構築するか)
    '' </summary>
    '' <value>
    '' 0: 最適解のみが出力可能なレベル (デフォルト, 高速) 
    '' 1: N-best 解が出力可能なレベル (中速) 
    '' 2: ソフトわかち書きが可能なレベル (低速) 
    '' </value>
    Public Property LatticeLevel() As MeCabLatticeLevel
        Get
            Me.ThrowIfDisposed()
            Return Me._viterbi.LatticeLevel
        End Get
        Set(ByVal Value As MeCabLatticeLevel)
            Me.ThrowIfDisposed()
            Me._viterbi.LatticeLevel = Value
        End Set
    End Property

    '' <summary>
    '' 全出力モード
    '' </summary>
    '' <value>
    '' true: 全出力
    '' false: ベスト解のみ
    '' </value>
    Public Property AllMorphs() As Boolean
        Get
            Me.ThrowIfDisposed()
            Return Me._viterbi.AllMorphs
        End Get
        Set(ByVal Value As Boolean)
            Me.ThrowIfDisposed()
            Me._viterbi.AllMorphs = Value
        End Set
    End Property

    '' <summary>
    '' 解析結果のフォーマット
    '' </summary>
    Public Property OutPutFormatType() As String
        Get
            Me.ThrowIfDisposed()
            Return Me._writer.OutputFormatType
        End Get
        Set(ByVal Value As String)
            Me.ThrowIfDisposed()
            Me._writer.OutputFormatType = Value
        End Set
    End Property
#End Region

#Region "Constractor"
    '' <summary>
    '' コンストラクタ
    '' </summary>
    Private Sub New()
    End Sub
#End Region

#Region "Open/Create"
    '' <summary>
    '' MeCabTaggerを開く
    '' </summary>
    '' <param name="param">初期化パラメーター</param>
    Private Sub Open(ByVal param As MeCabParam)
        Me._viterbi.Open(param)

        Me._writer.Open(param)
    End Sub

    '' <summary>
    '' MeCabTaggerのインスタンスを生成する
    '' </summary>
    '' <returns>MeCabTaggerのインスタンス</returns>
    Public Shared Function Create()
        Dim param As MeCabParam = New MeCabParam()
        param.LoadDicRC()
        Return MeCabTagger.Create(param)
    End Function

    '' <summary>
    '' MeCabTaggerのインスタンスを生成する
    '' </summary>
    '' <param name="param">初期化パラメーター</param>
    '' <returns>MeCabTaggerのインスタンス</returns>
    Public Shared Function Create(ByVal param As MeCabParam)
        Dim tagger As MeCabTagger = New MeCabTagger()
        tagger.Open(param)
        Return tagger
    End Function

#End Region

#Region "Parse"
    '' <summary>
    '' 解析を行う
    '' </summary>
    '' <param name="str">解析対象の文字列</param>
    '' <returns>解析結果の文字列</returns>
    Public Function Parse(str As String) As String
        Dim n As MeCabNode = Me.ParseToNode(str)
        If n Is Nothing Then
            Return Nothing
        End If
        Dim os As StringBuilder = New StringBuilder()
        Me._writer.Write(os, n)
        Return os.ToString()
    End Function


    '' <summary>
    '' 解析を行う
    '' </summary>
    '' <param name="str">解析対象の文字列</param>
    '' <returns>文頭の形態素</returns>
    Public Function ParseToNode(ByVal str As String) As MeCabNode
        If str.Length < 0 Then
            Throw New ArgumentOutOfRangeException("len", "Please set one or more to length of string.")
        End If
        Return Me._viterbi.Analyze(str)
    End Function

#End Region

#Region "NBest"
    '' <summary>
    '' 解析を行い結果を確からしいものから順番に取得する
    '' </summary>
    '' <param name="str">解析対象の文字列</param>
    '' <returns>文頭の形態素を、確からしい順に取得する列挙子</returns>
    Public Function ParseNBestToNode(ByVal str As String) As IEnumerable(Of MeCabNode)
        If Me.LatticeLevel = 0 Then
            Throw New InvalidOperationException("Please set one or more to LatticeLevel.")
        End If

        Dim n As MeCabNode = Me.ParseToNode(str)
        Dim nBest As NBestGenerator = New NBestGenerator()
        nBest._Set(n)
        Return nBest.GetEnumerator()
    End Function

    '' <summary>
    '' ParseのN-Best解出力version
    '' </summary>
    '' <param name="n">必要な解析結果の個数</param>
    '' <param name="str">解析対象の文字列</param>
    '' <returns>解析結果の文字列</returns>
    Public Function ParseNBest(ByVal n As Integer, ByVal str As String) As String
        If n <= 0 Then
            Throw New ArgumentOutOfRangeException("n", "")
        End If

        If n = 1 Then
            Return Me.Parse(str)
        End If

        Dim os As StringBuilder = New StringBuilder()

        Dim node As MeCabNode
        For Each node In Me.ParseNBestToNode(str)
            Me._writer.Write(os, node)
            n -= 1
            If n = 0 Then Exit For
        Next
        Return os.ToString()

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
            Me._viterbi.Dispose()
        End If
        Me.disposed = True
    End Sub

    Protected Overridable Overloads Sub Finalize()
        Dispose(False)
    End Sub

    Private Sub ThrowIfDisposed()
        If Me.disposed = True Then
            Throw New ObjectDisposedException(Me.GetType().FullName)
        End If
    End Sub
#End Region
End Class
