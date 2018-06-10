Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Core
    Public Class StrUtils
        Private Const Nul As Byte = CByte(0)

        '' <summary>
        '' バイト配列の中から終端が\0で表された文字列を取り出す。
        '' </summary>
        '' <remarks>
        '' バイト配列の長さはInt32.MaxValueを超えていても良い。
        '' </remarks>
        '' <param name="bytes">バイト配列</param>
        '' <param name="enc">文字エンコーディング</param>
        '' <returns>文字列（\0は含まない）</returns>
        Public Shared Function GetString(ByVal bytes() As Byte, ByVal enc As Encoding) As String
            'return StrUtils.GetString(bytes, 0L, enc);
            'バイト長のカウント
            Dim byteCount As Integer = 0
            While bytes(byteCount) <> Nul
                byteCount += 1
                '文字列のバイト長がInt32.MaxValueを超えたならエラー
            End While

            '生成されうる最大文字数のバッファを確保
            Dim maxCharCount As Integer = enc.GetMaxCharCount(byteCount)
            Dim buff() As Char = New Char(maxCharCount) {}
            'バイト配列を文字列にデコード
            Dim len As Integer = enc.GetChars(bytes, 0, byteCount, buff, 0)
            Return New String(buff, 0, len)
        End Function
        '' <summary>
        '' バイト配列の中から終端が\0で表された文字列を取り出す。
        '' </summary>
        '' <remarks>
        '' バイト配列の長さはInt32.MaxValueを超えていても良い。
        '' </remarks>
        '' <param name="bytes">バイト配列</param>
        '' <param name="offset">オフセット位置</param>
        '' <param name="enc">文字エンコーディング</param>
        '' <returns>文字列（\0は含まない）</returns>
        Public Shared Function GetString(ByVal bytes() As Byte, ByVal offset As Long, ByVal enc As Encoding) As String
            Dim idx As Long = offset
            Dim byteCount As Integer = 0
            While bytes(idx) <> Nul
                byteCount += 1
                '文字列のバイト長がInt32.MaxValueを超えたならエラー
                idx += 1
            End While

            '生成されうる最大文字数のバッファを確保
            Dim maxCharCount As Integer = enc.GetMaxCharCount(byteCount)
            Dim buff() As Char = New Char(maxCharCount) {}
            'バイト配列を文字列にデコード
            Dim len As Integer = enc.GetChars(bytes, offset, byteCount, buff, 0)
            Return New String(buff, 0, len)
        End Function

        '' <summary>
        '' 指定の名前に対応するエンコーディングを取得する（.NET FWが対応していない名前にもアドホックに対応）
        '' </summary>
        '' <param name="name"></param>
        '' <returns></returns>
        Public Shared Function GetEncoding(ByVal name As String) As Encoding
            Select Case name.ToUpper()
                Case "UTF8"
                    Return Encoding.UTF8
                Case Else
                    Return Encoding.GetEncoding(name)
            End Select
        End Function
    End Class
End Namespace
