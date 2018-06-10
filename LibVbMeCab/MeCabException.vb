'  MeCab -- Yet Another Part-of-Speech and Morphological Analyzer
'
'  Copyright(C) 2001-2006 Taku Kudo <taku@chasen.org>
'  Copyright(C) 2004-2006 Nippon Telegraph and Telephone Corporation
'  Copyright(C) 2018      Ken'ichiro Ayaki 
Imports System
Imports System.Runtime.Serialization

<Serializable>
Public Class MeCabException : Inherits Exception
    Public Sub New()
    End Sub
    Public Sub New(ByVal message As String)
    End Sub

    Public Sub New(ByVal message As String, ByVal ex As Exception)
    End Sub

    Public Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
    End Sub
End Class
