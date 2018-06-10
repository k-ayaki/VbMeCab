Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports VbMeCab
Imports System.Diagnostics


Partial Public Class Form1
    Inherits Form
    Dim tagger As MeCabTagger

    Private Property LatticeLevel() As MeCabLatticeLevel
        Get
            If Me.LatticeLevel0RadioButton.Checked = True Then
                Return MeCabLatticeLevel.Zero
            ElseIf Me.LatticeLevel1RadioButton.Checked = True Then
                Return MeCabLatticeLevel.One
            ElseIf Me.LatticeLevel2RadioButton.Checked = True Then
                Return MeCabLatticeLevel.Two
            Else
                Return MeCabLatticeLevel.Two
            End If
        End Get
        Set(ByVal Value As MeCabLatticeLevel)
            Select Case Value
                Case MeCabLatticeLevel.Zero
                    Me.LatticeLevel0RadioButton.Checked = True
                    Exit Property
                Case MeCabLatticeLevel.One
                    Me.LatticeLevel1RadioButton.Checked = True
                    Exit Property
                Case MeCabLatticeLevel.Two
                    Me.LatticeLevel2RadioButton.Checked = True
                    Exit Property
                Case Else
                    Throw New ArgumentOutOfRangeException()
            End Select
        End Set
    End Property

    Private Property OutputFormat() As String
        Get
            If Me.OutputFormatLatticeRadioButton.Checked = True Then
                Return Me.OutputFormatLatticeRadioButton.Text
            ElseIf Me.OutputFormatWakatiRadioButton.Checked = True Then
                Return Me.OutputFormatWakatiRadioButton.Text
            ElseIf Me.OutputFormatDumpRadioButton.Checked = True Then
                Return Me.OutputFormatDumpRadioButton.Text
            Else
                Return Me.OutputFormatDumpRadioButton.Text
            End If
        End Get
        Set(ByVal Value As String)
            Select Case Value
                Case "lattice"
                    Me.OutputFormatLatticeRadioButton.Checked = True
                    Exit Property
                Case "wakati"
                    Me.OutputFormatWakatiRadioButton.Checked = True
                    Exit Property
                Case "dump"
                    Me.OutputFormatDumpRadioButton.Checked = True
                    Exit Property
                Case Else
                    Throw New NotImplementedException()
            End Select
        End Set
    End Property

    Public Sub New()
        InitializeComponent()
        Me.Text = Application.ProductName
    End Sub


    Private Sub NBestCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    'Private Sub PartialCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
    '
    'End Sub

    Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs)
        If Not Me.tagger Is Nothing Then
            Me.tagger.Dispose()
        End If
    End Sub

    Private Sub DoAnalyzeButton_Click_1(sender As Object, e As EventArgs) Handles DoAnalyzeButton.Click
        Try
            Me.tagger.LatticeLevel = Me.LatticeLevel
            Me.tagger.OutPutFormatType = Me.OutputFormat
            Me.tagger.AllMorphs = Me.AllMorphsCheckBox.Checked
            Me.tagger._Partial = Me.PartialCheckBox.Checked

            Me.toolStripStatusLabel1.Text = "Analyzing ..."
            Dim sw As Stopwatch = Stopwatch.StartNew()

            If NBestCheckBox.Checked = True Then
                Me.ResultTextBox.Text = Me.tagger.ParseNBest(CType(NBestNumericUpDown.Value, Integer), Me.TargetTextBox.Text)
            Else
                Me.ResultTextBox.Text = Me.tagger.Parse(Me.TargetTextBox.Text)
            End If

            sw.Stop()
            Me.toolStripStatusLabel1.Text = String.Format("Finish ({0:0.000}sec)",
                                                                sw.Elapsed.TotalSeconds)
        Catch ex As Exception
            Me.toolStripStatusLabel1.Text = "ERROR"
            MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Form1_Load_1(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim sw As Stopwatch = Stopwatch.StartNew()

            Me.tagger = MeCabTagger.Create()

            sw.Stop()
            Me.toolStripStatusLabel1.Text = String.Format("startup end ({0:0.000}sec)",
                                                                sw.Elapsed.TotalSeconds)

            Me.LatticeLevel = Me.tagger.LatticeLevel
            Me.OutputFormat = Me.tagger.OutPutFormatType
            Me.AllMorphsCheckBox.Checked = Me.tagger.AllMorphs
            Me.PartialCheckBox.Checked = Me.tagger._Partial
        Catch ex As Exception
            MessageBox.Show(ex.ToString(), "Startup ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End Try
    End Sub

    Private Sub PartialCheckBox_CheckedChanged_1(sender As Object, e As EventArgs) Handles PartialCheckBox.CheckedChanged
        If PartialCheckBox.Checked = True Then
            Me.TargetTextBox.AcceptsTab = True
            Me.toolStripStatusLabel1.Text = "Accepts Tab for Partial mode."
        Else
            Me.TargetTextBox.AcceptsTab = False
            Me.toolStripStatusLabel1.Text = ""
        End If
    End Sub

    Private Sub NBestCheckBox_CheckedChanged_1(sender As Object, e As EventArgs) Handles NBestCheckBox.CheckedChanged
        If Me.NBestCheckBox.Checked = True Then
            Me.NBestNumericUpDown.Enabled = True
            If Me.LatticeLevel = MeCabLatticeLevel.Zero Then
                Me.LatticeLevel = MeCabLatticeLevel.One
            End If
            Me.LatticeLevel0RadioButton.Enabled = False
            Me.AllMorphsCheckBox.Checked = False
            Me.AllMorphsCheckBox.Enabled = False
        Else
            Me.NBestNumericUpDown.Enabled = False
            Me.LatticeLevel0RadioButton.Enabled = True
            Me.AllMorphsCheckBox.Enabled = True
        End If
    End Sub
End Class
