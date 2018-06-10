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
Public Class MeCabInvalidFileException : Inherits MeCabException
    Public Property FileName As String

    Public Overrides ReadOnly Property Message() As String
        Get
            Dim os As StringBuilder = New StringBuilder()
            os.Append(MyBase.Message)
            If Not Me.FileName Is Nothing Then
                os.AppendFormat("[FileName:{0}]", Me.FileName)
            End If
            Return os.ToString()
        End Get
    End Property

    Public Sub New()

    End Sub
    Public Sub New(ByVal message As String, ByVal _fileName As String)
        Me.FileName = _fileName
    End Sub

    Public Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        Me.FileName = info.GetString("FileName")
    End Sub

    <SecurityPermission(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
    Public Overrides Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.GetObjectData(info, context)
        info.AddValue("FileName", Me.FileName)
    End Sub
End Class

