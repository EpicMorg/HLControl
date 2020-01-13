Namespace 基础

    ''' <summary>
    ''' WSave配置文件的模块
    ''' </summary>
    Public Module WSave配置文件

        ''' <summary>
        ''' WSave配置文件
        ''' </summary>
        Public Class WSave

            Private En As 走過去加密, key As String = "戈登走過去只是个平凡的人"
            Private Di As Dictionary(Of String, String)

            ''' <summary>
            ''' 本地文件的位置，通常为 .wsave 后缀
            ''' </summary>
            Public Property 本地文件 As String

            ''' <summary>
            ''' 新建一个WSave配置文件类
            ''' </summary>
            Public Sub New(Optional 本地文件 As String = "")
                En = New 走過去加密(key)
                Di = New Dictionary(Of String, String)
                If 本地文件.Length > 4 Then
                    Me.本地文件 = 本地文件
                    从文件读取()
                End If
            End Sub

            ''' <summary>
            ''' 测试该外部密钥是否和内部密钥一样
            ''' </summary>
            Public Function 密钥冲突(测试 As String) As Boolean
                Dim m As New 走過去加密(测试)
                Return m.密钥.Contains(En.密钥) OrElse En.密钥.StartsWith(m.密钥) OrElse En.密钥.EndsWith(m.密钥)
            End Function

            ''' <summary>
            ''' 清空，并从本地读取文件
            ''' </summary>
            Public Sub 从文件读取()
                Di.Clear()
                If 文件大小Byte(本地文件) > 5 Then
                    Dim m As StreamReader = Nothing
                    Try
                        m = New StreamReader(File.OpenRead(本地文件))
                        m.ReadLine()
                        Dim s1 As String, s2 As String
                        Do While True
                            s1 = En.解密为字符串(m.ReadLine).Trim.ToLower
                            If m.EndOfStream Then Exit Do
                            s2 = En.解密为字符串(m.ReadLine)
                            If 非空全部(s1, s2) Then
                                If Not Di.ContainsKey(s1) Then
                                    Di.Add(s1, s2)
                                End If
                            End If
                            If m.EndOfStream Then Exit Do
                        Loop
                        m.Dispose()
                    Catch ex As Exception
                        If 类型.非空(m) Then m.Dispose()
                        出错(ex)
                    End Try
                End If
            End Sub

            ''' <summary>
            ''' 保存内容到本地文件
            ''' </summary>
            Public Sub 保存到本地()
                If 删除文件(本地文件) Then
                    Dim m As StreamWriter = Nothing
                    Try
                        m = New StreamWriter(File.OpenWrite(本地文件))
                        m.WriteLine("戈登走過去的配置文件，请勿乱修改。 " + 本程序.文件名 + ".exe")
                        For Each i As String In Di.Keys
                            Dim s1 As String = Di.Item(i)
                            If 非空全部(i, s1) Then
                                m.WriteLine(En.加密(i))
                                m.WriteLine(En.加密(s1))
                            End If
                        Next
                        m.Dispose()
                    Catch ex As Exception
                        If 类型.非空(m) Then m.Dispose()
                        出错(ex)
                    End Try
                End If
            End Sub

            ''' <summary>
            ''' 从配置文件里读取或写入字符串
            ''' </summary>
            Public Property 字符串(名字 As String, Optional 默认 As String = "") As String
                Get
                    文本标准化(默认)
                    名字 = 名字.ToLower.Trim
                    If 名字.Length > 0 AndAlso Di.ContainsKey(名字) Then Return Di.Item(名字)
                    Return 默认
                End Get
                Set(值 As String)
                    If 名字.Length < 1 Then Exit Property
                    名字 = 名字.ToLower.Trim
                    If Di.ContainsKey(名字) Then
                        Di.Remove(名字)
                    End If
                    If 值.Length > 0 Then
                        Di.Add(名字, 值)
                    End If
                End Set
            End Property

            ''' <summary>
            ''' 从配置文件里检查是否有这个名字的值
            ''' </summary>
            Public ReadOnly Property 非空(名字 As String) As Boolean
                Get
                    Return 字符串(名字).Length > 0
                End Get
            End Property

            ''' <summary>
            ''' 从配置文件里读取或写入数字
            ''' </summary>
            Public Property 数字(名字 As String, Optional 默认 As Double = 0) As Double
                Get
                    Dim s As String = 字符串(名字)
                    If s.Length > 0 Then Return Val(s)
                    Return 默认
                End Get
                Set(值 As Double)
                    If 值 <> 0 Then
                        字符串(名字) = 值.ToString
                    Else
                        字符串(名字) = ""
                    End If
                End Set
            End Property

            ''' <summary>
            ''' 从配置文件里读取或写入真假
            ''' </summary>
            Public Property 真假(名字 As String, Optional 默认 As Boolean = False) As Boolean
                Get
                    Dim s As String = 字符串(名字)
                    If s.Length = 4 Then Return True
                    Return 默认
                End Get
                Set(值 As Boolean)
                    字符串(名字) = IIf(值, "True", "")
                End Set
            End Property

            ''' <summary>
            ''' 从配置文件里读取或写入日期
            ''' </summary>
            Public Property 日期(名字 As String, Optional 默认 As Date = #2000-01-01 00:00:00#) As Date
                Get
                    Dim s As String = 仅数字(字符串(名字))
                    If s.Length > 1 Then Return 时间戳转出(Val(s))
                    Return 默认
                End Get
                Set(值 As Date)
                    字符串(名字) = 转时间戳(值)
                End Set
            End Property

            ''' <summary>
            ''' 把值与控件绑定，一个控件只能有一个值，绑定的时候读取值，窗口关闭的时候保存值
            ''' </summary>
            Public Sub 绑定控件(控件 As Control, 值 As 控件值类型, Optional 默认 As Object = Nothing)
                Dim s As String = 生成控件读取(控件)
                If s.Length < 1 Then Exit Sub
                Try
                    Dim g As Object = Nothing, tp As Integer = 0
                    Select Case 值
                        Case 控件值类型.Text
                            g = 字符串(s, 默认)
                        Case 控件值类型.Checked
                            g = 真假(s, 默认)
                            tp = 1
                        Case Else
                            g = 数字(s, 默认)
                            tp = 2
                    End Select
                    CallByName(控件, 值.ToString, CallType.Set, g)
                    AddHandler 控件.FindForm.FormClosing, Sub()
                                                            Dim c As String = CallByName(控件, 值.ToString, CallType.Get).ToString
                                                            Select Case tp
                                                                Case 0
                                                                    字符串(s) = c
                                                                Case 1
                                                                    真假(s) = c.Length = 4
                                                                Case 2
                                                                    数字(s) = Val(c)
                                                            End Select
                                                        End Sub
                Catch ex As Exception
                    出错(ex, 控件, 值.ToString)
                End Try
            End Sub

            ''' <summary>
            ''' 绑定控件的时候的内部ID
            ''' </summary>
            Public Function 生成控件读取(控件 As Control) As String
                If 为空(控件, 控件.FindForm) Then Return ""
                Return 控件.FindForm.Name + 控件.GetType.ToString + 控件.Name
            End Function

        End Class

        ''' <summary>
        ''' Wsave控件绑定的时候用的控件值类型，注意Value视作为数字类型
        ''' </summary>
        Public Enum 控件值类型
            Text
            Checked
            Value
            Maximum
            Minimum
            Left
            Top
            Width
            Height
            SelectedIndex
            Interval
        End Enum

    End Module

End Namespace
