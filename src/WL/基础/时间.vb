Namespace 基础

    ''' <summary>
    ''' 和时间日期有关的模块
    ''' </summary>
    Public Module 时间

        ''' <summary>
        ''' 把时间转换为 UNIX 时间，默认不包括毫秒（毫秒是伪造的）
        ''' </summary>
        Public Function 转时间戳(时间 As Date, Optional 包括毫秒 As Boolean = False) As ULong
            Dim n As ULong = DateDiff(DateInterval.Second, #1970-1-1 0:0:0#, 时间.ToUniversalTime)
            If 包括毫秒 Then
                Dim g As Single = Microsoft.VisualBasic.Timer
                n = (n + g - CLng(g)) * 1000
            End If
            Return n
        End Function

        ''' <summary>
        ''' 返回 Microsoft.VisualBasic.Timer 自午夜以来经过的值，该值表示的秒数。
        ''' </summary>
        Public Function 当日时间戳() As Double
            Return Microsoft.VisualBasic.Timer
        End Function

        ''' <summary>
        ''' 把当前时间转换为 UNIX 时间，精确到秒
        ''' </summary>
        Public Function 当前时间戳(Optional 包括毫秒 As Boolean = False) As ULong
            Return 转时间戳(Now, 包括毫秒)
        End Function

        ''' <summary>
        ''' 把时间戳变成 DateTime，只支持秒级别的时间戳
        ''' </summary>
        Public Function 时间戳转出(时间戳 As ULong) As Date
            Dim t As Date = #1970-1-1 00:00:00#
            If 时间戳 > 1000484608441 Then 时间戳 /= 1000
            Return t.AddSeconds(时间戳).ToLocalTime
        End Function

        ''' <summary>
        ''' 把时间格式化后输出字符串，用Y年，M月，D天，h时，m分钟，s秒，并且年强制为4位数字，其他强制为2位数字
        ''' </summary>
        Public Function 时间格式化(时间 As Date, Optional 格式 As String = "Y-M-D h-m-s") As String
            With 时间
                Return 正则.替换(替换(格式, "m", "f"), "Y+", 补左(.Year.ToString, 4, "0"),
                             "M+", 补左(.Month.ToString, 2, "0"),
                             "D+", 补左(.Day.ToString, 2, "0"),
                             "h+", 补左(.Hour.ToString, 2, "0"),
                             "f+", 补左(.Minute.ToString, 2, "0"),
                             "s+", 补左(.Second.ToString, 2, "0"))
            End With
        End Function

        ''' <summary>
        ''' 把秒数变成时间文字，默认从秒起步
        ''' </summary>
        Public Function 时间文字(秒数 As Long, Optional 一天起步 As Boolean = False) As String
            秒数 = Math.Abs(秒数)
            If 一天起步 AndAlso 秒数 < 60 * 60 * 25 Then Return "1天"
            Select Case 秒数
                Case < 1
                    Return "瞬间"
                Case < 61
                    Return 秒数 & "秒"
                Case < 60 * 70
                    Return Fix(秒数 / 60) & "分钟"
                Case < 60 * 60 * 25
                    Return Fix(秒数 / 60 / 60) & "小时"
                Case < 60 * 60 * 24 * 80
                    Return Fix(秒数 / 60 / 60 / 24) & "天"
                Case < 60 * 60 * 24 * 380
                    Return Fix(秒数 / 60 / 60 / 24 / 30) & "个月"
                Case Else
                    Return Fix(秒数 / 60 / 60 / 24 / 365) & "年"
            End Select
        End Function

        ''' <summary>
        ''' 如果同一个ID访问这个函数的时间间隔过小，就返回true
        ''' </summary>
        Public Function 过频(id As Integer, 时间 As Double) As Boolean
            Static d As New Dictionary(Of Integer, Double)
            If d.ContainsKey(id) = False Then
                d.Add(id, 当日时间戳)
            Else
                Dim m As Double = d.Item(id), n As Double = 当日时间戳()
                If n - m < 时间 Then Return True
                d.Item(id) = n
            End If
            Return False
        End Function

    End Module

End Namespace