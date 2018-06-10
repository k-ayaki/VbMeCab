'  MeCab -- Yet Another Part-of-Speech and Morphological Analyzer
'
'  Copyright(C) 2001-2006 Taku Kudo <taku@chasen.org>
'  Copyright(C) 2004-2006 Nippon Telegraph and Telephone Corporation
'  Copyright(C) 2018      Ken'ichiro Ayaki 
Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Core
    Public Class MeCabPath
#Region "Const/Field/Property"
        Public Property RNode As MeCabNode

        Public Property RNext As MeCabPath

        Public Property LNode As MeCabNode

        Public Property LNext As MeCabPath

        Public Property Cost As Integer

        Public Property Prob As Single
#End Region

#Region "Method"
        Public Overrides Function ToString() As String
            Return String.Format("[Cost:{0}][Prob:{1}][LNode:{2}][RNode;{3}]",
                                 Me.Cost,
                                 Me.Prob,
                                 Me.LNode,
                                 Me.RNode)

        End Function
#End Region
    End Class
End Namespace
