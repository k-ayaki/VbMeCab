Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Core
    '' <summary>
    '' ビット値操作のユーティリティ
    '' </summary>
    '' <remarks>
    '' BitVector32構造体より実行速度に重点を置き、シンプルな実装にする。
    '' </remarks>
    Public Class BitUtils
        Private Const One As System.UInt32 = &H1
        Private Const AllZero As System.UInt32 = &H0
        Private Const AllOne As System.UInt32 = &HFFFFFFFFUL

        '' <summary>
        '' 指定範囲のビットフィールド値を取り出す
        '' </summary>
        '' <param name="bits">ビット列を表すUInt32値</param>
        '' <param name="offset">開始ビット位置</param>
        '' <param name="len">ビット長</param>
        '' <returns>ビットフィールド値</returns>
        Public Shared Function GetBitField(ByVal bits As System.UInt32, ByVal offset As Integer, ByVal len As Integer) As System.UInt32
            Dim mask As System.UInt32 = Not (AllOne << len)
            Return (bits >> offset) And mask
        End Function

        '' <summary>
        '' 指定位置のビット値を取り出す
        '' </summary>
        '' <param name="bits">ビット列を表すUInt32値</param>
        '' <param name="offset">ビット位置</param>
        '' <returns>ビット値</returns>
        Public Shared Function GetFlag(ByVal bits As System.UInt32, ByVal offset As Integer) As Boolean
            Dim mask As System.UInt32 = One << offset
            Return If((bits And mask) <> AllZero, True, False)
        End Function

        '' <summary>
        '' 指定範囲のビット値のAND比較
        '' </summary>
        '' <param name="bits1">ビット列1を表すUInt32値</param>
        '' <param name="bits2">ビット列2を表すUInt32値</param>
        '' <param name="offset">開始ビット位置</param>
        '' <param name="len">ビット長</param>
        '' <returns>比較結果</returns>
        Public Shared Function CompareAnd(ByVal bits1 As System.UInt32, ByVal bits2 As System.UInt32, ByVal offset As Integer, ByVal len As Integer) As Boolean
            Dim mask As System.UInt32 = Not (AllOne << len) << offset
            Return If(((bits1 And bits2) And mask) <> AllZero, True, False)
        End Function
    End Class
End Namespace
