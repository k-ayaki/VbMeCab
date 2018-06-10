'  MeCab -- Yet Another Part-of-Speech and Morphological Analyzer
'
'  Copyright(C) 2001-2006 Taku Kudo <taku@chasen.org>
'  Copyright(C) 2004-2006 Nippon Telegraph and Telephone Corporation
'  Copyright(C) 2018      Ken'ichiro Ayaki 
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO

Namespace Core
    Public Structure Token
#Region "Const/Field/Property"
        '' <summary>
        '' 右文脈 id
        '' </summary>
        Public Property LcAttr As UShort

        '' <summary>
        '' 左文脈 id
        '' </summary>
        Public Property RcAttr As UShort

        '' <summary>
        '' 形態素 ID
        '' </summary>
        Public Property PosId As UShort

        '' <summary>
        '' 単語生起コスト
        '' </summary>
        Public Property WCost As Short

        '' <summary>
        '' 素性情報の位置
        '' </summary>
        Public Property Feature As UInteger

        '' <summary>
        '' reserved for noun compound
        '' </summary>
        Public Property Compound As UInteger

#End Region

#Region "Method"

        Public Shared Function Create(ByVal reader As BinaryReader) As Token
            Dim ret As New Token
            With ret
                .LcAttr = reader.ReadUInt16()
                .RcAttr = reader.ReadUInt16()
                .PosId = reader.ReadUInt16()
                .WCost = reader.ReadInt16()
                .Feature = reader.ReadUInt32()
                .Compound = reader.ReadUInt32()

            End With
            Return ret
        End Function


        Public Overrides Function ToString() As String
            Return String.Format("[LcAttr:{0}][RcAttr:{1}][PosId:{2}][WCost:{3}][Feature:{4}][Compound:{5}]",
                  Me.LcAttr,
                  Me.RcAttr,
                  Me.PosId,
                  Me.WCost,
                  Me.Feature,
                  Me.Compound)
        End Function
#End Region
    End Structure
End Namespace
