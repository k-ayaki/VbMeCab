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
    Public Class Utils
        Public Shared Function LogSumExp(ByVal x As Double, ByVal y As Double, ByVal flg As Boolean) As Double
            Const MinusLogEpsilon As Double = 50.0

            If flg Then
            End If
            Dim vMin As Double = Math.Min(x, y)
            Dim vMax As Double = Math.Max(x, y)
            If vMax > vMin + MinusLogEpsilon Then
                Return vMax
            Else
                Return vMax + Math.Log(Math.Exp(vMin - vMax) + 1.0)
            End If
        End Function
    End Class
End Namespace

