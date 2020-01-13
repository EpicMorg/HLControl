Namespace 基础

    ''' <summary>
    ''' 引用其他的DLL的函数，并封装好一些常用的用法
    ''' </summary>
    Public Module 引用

        <DllImport("user32.dll")>
        Private Function ReleaseCapture() As Boolean
        End Function

        <DllImport("user32.dll")>
        Private Function SendMessage(HWnd As IntPtr, Msg As Integer, WParam As Integer, IParam As Integer) As Integer
        End Function

        ''' <summary>
        ''' 对窗口进行拖放，通常用于无边框的窗体，写在MouseDown事件里使用
        ''' </summary>
        Public Sub 拖动窗口(f As Form)
            If 为空(f) Then Exit Sub
            Try
                ReleaseCapture()
                SendMessage(f.Handle, &H112, &HF012, 0)
            Catch ex As Exception
                出错(ex)
            End Try
        End Sub

        ''' <summary>
        ''' 对控件发送滚动信号，使用 EM_LINESCROLL 
        ''' </summary>
        Public Sub 滚动(控件 As Control, 垂直 As Boolean, 数量 As Integer)
            If 为空(控件) Then Exit Sub
            Dim m As Integer = 0, n As Integer = 0
            If 垂直 Then
                m = 数量
            Else
                n = 数量
            End If
            Const EM_LINESCROLL As Integer = &HB6
            Try
                SendMessage(控件.Handle, EM_LINESCROLL, n, m)
            Catch ex As Exception
                出错(ex)
            End Try
        End Sub

        ''' <summary>
        ''' 获得文本框的行数，使用 EM_LINEINDEX 
        ''' </summary>
        Public Function 获得文本框行数(控件 As Control) As Integer
            If 为空(控件) OrElse 控件.Text.Length < 1 Then Return 0
            Const EM_GETLINECOUNT As Integer = &HBA
            Dim g As Integer = 0
            Try
                g = SendMessage(控件.Handle, EM_GETLINECOUNT, 0, 0)
                Return g
            Catch ex As Exception
                出错(ex)
            End Try
            Return 0
        End Function

        <DllImport("wininet.dll")>
        Private Function InternetGetCookieExA(lpszUrl As String, lpszCookieName As String, lpszCookieData As StringBuilder, ByRef lpdwSize As Integer, dwFlags As Integer, lpReserved As IntPtr) As Boolean
        End Function

        ''' <summary>
        ''' 从IE浏览器里获得指定网站的Cookie，网站应该是http://或者https://开头的完整链接
        ''' </summary>
        Public Function 获取IEcookie(网站 As String) As String
            Dim s As Integer = 512
            Dim m As New StringBuilder(s)
            Dim b As Boolean = InternetGetCookieExA(网站, Nothing, m, s, 8192, IntPtr.Zero)
            If b Then
                Return m.ToString
            End If
            Return ""
        End Function

    End Module

End Namespace
