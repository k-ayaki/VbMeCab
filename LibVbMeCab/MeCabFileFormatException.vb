'  MeCab -- Yet Another Part-of-Speech and Morphological Analyzer
'
'  Copyright(C) 2001-2006 Taku Kudo <taku@chasen.org>
'  Copyright(C) 2004-2006 Nippon Telegraph and Telephone Corporation
'  Copyright(C) 2018      Ken'ichiro Ayaki 
Imports System
Imports System.Runtime.Serialization
Imports System.Security.Permissions
Imports System.Text

<Serializable>
Public Class MeCabFileFormatException : Inherits MeCabInvalidFileException
    Public Property LineNo As Integer

    Public Property Line As String

    Public Overrides ReadOnly Property Message As String
        Get
            Dim os As StringBuilder = New StringBuilder()
            os.Append(MyBase.Message)
            If Me.LineNo > 0 Then
                os.AppendFormat("[LineNo:{0}]", Me.LineNo)
            End If
            If Not Me.Line Is Nothing Then
                os.AppendFormat("[Line:{0}]", Me.Line)
            End If
            Return os.ToString()
        End Get
    End Property

    Public Sub New(ByVal message As String, Optional ByVal fileName As String = Nothing, Optional ByVal _lineNo As Integer = -1, Optional ByVal _line As String = Nothing)
        Me.LineNo = _lineNo
        Me.Line = _line
    End Sub


    Public Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        Me.LineNo = info.GetInt32("LineNo")
        Me.Line = info.GetString("Line")
    End Sub

    <SecurityPermission(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
    Public Overrides Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.GetObjectData(info, context)
        info.AddValue("LineNo", Me.LineNo)
        info.AddValue("Line", Me.Line)
    End Sub
End Class
