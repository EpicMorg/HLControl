
Namespace 基础

    ''' <summary>
    ''' 与本程序还有其他程序的相关操作的模块
    ''' </summary>
    Public Module 程序

        ''' <summary>
        ''' 本程序的相关信息
        ''' </summary>
        Public NotInheritable Class 本程序

            Protected Sub New()
            End Sub

            ''' <summary>
            ''' 获取本程序的Process类
            ''' </summary>
            Public Shared ReadOnly Property 进程() As Process
                Get
                    Static p As Process = Process.GetCurrentProcess
                    Return p
                End Get
            End Property

            ''' <summary>
            ''' 获得本程序的PID
            ''' </summary>
            ''' <returns></returns>
            Public Shared ReadOnly Property PID As UInteger
                Get
                    Return 进程.Id
                End Get
            End Property

            ''' <summary>
            ''' 获取本程序的文件路径
            ''' </summary>
            ''' <returns></returns>
            Public Shared ReadOnly Property 路径 As String
                Get
                    Static m As String = 路径标准化(My.Application.Info.DirectoryPath)
                    Return m
                End Get
            End Property

            ''' <summary>
            ''' 获得本程序的文件名，不包含.exe
            ''' </summary>
            ''' <returns></returns>
            Public Shared ReadOnly Property 文件名() As String
                Get
                    Return 进程.ProcessName
                End Get
            End Property

            ''' <summary>
            ''' 重启本程序
            ''' </summary>
            Public Shared Sub 重启()
                Directory.SetCurrentDirectory(路径())
                打开程序("C:\Windows\System32\cmd.exe", "/c taskkill /f /pid " & PID() & " & start " & 引号 & 引号 & " " & 引(文件名() & ".exe"), ProcessWindowStyle.Hidden)
            End Sub

            ''' <summary>
            ''' 退出本程序
            ''' </summary>
            Public Shared Sub 退出()
                进程.Kill()
            End Sub

            ''' <summary>
            ''' 是否以 Console.Writeline 来操作输出 sub
            ''' </summary>
            ''' <returns></returns>
            Public Shared Property 控制台输出 As Boolean = False

            ''' <summary>
            ''' 检查本程序是否真的在运行中
            ''' </summary>
            Public Shared Function 真的运行中() As Boolean
                Return 非空(进程) AndAlso 进程.ProcessName <> "devenv"
            End Function

            ''' <summary>
            ''' 获得本程序的主文件图标
            ''' </summary>
            Public Shared ReadOnly Property 图标 As Icon
                Get
                    Static m As Icon = Nothing
                    Try
                        If 为空(m) Then m = Icon.ExtractAssociatedIcon(本程序.路径 + 本程序.文件名 + ".exe")
                    Catch ex As Exception
                        m = SystemIcons.Application
                        出错(ex)
                    End Try
                    Return m
                End Get
            End Property

        End Class

        ''' <summary>
        ''' Debug.Print 输出信息，也可以 Console.Writeline
        ''' </summary>
        Public Sub 输出(ParamArray 内容() As Object)
            Dim s As String = "", m As String, e As String = "<empty string>"
            For Each i As Object In 内容
                If IsNothing(i) Then
                    m = "<nothing>"
                Else
                    m = i.ToString
                    If m.Length < 1 Then m = e
                End If
                s += m + "    "
            Next
            s = 去右(s, 4)
            If s.Length < 1 Then s = e
            Debug.Print(s)
            If 本程序.控制台输出 Then Console.WriteLine(s)
        End Sub

        ''' <summary>
        ''' 把线程强制中断
        ''' </summary>
        Public Sub 中断线程(ParamArray 线程() As Thread)
            For Each t As Thread In 线程
                If (Not IsNothing(t)) AndAlso t.IsAlive Then t.Abort()
            Next
        End Sub

        ''' <summary>
        ''' 打开一个程序，超时选项只有在等到运行结束为True的时候才会工作
        ''' </summary>
        Public Sub 打开程序(文件 As String, Optional 参数 As String = "", Optional 窗口样式 As ProcessWindowStyle = ProcessWindowStyle.Normal, Optional 管理员权限运行 As Boolean = False, Optional 等到运行结束 As Boolean = False, Optional 超时 As UInteger = 0)
            If 文件.Length < 4 Then
                出错("打开程序，文件名不对", 文件)
                Exit Sub
            End If
            If 文件夹存在(文件) OrElse 头(文件.ToLower, "https://", "http://") Then
                Try
                    Process.Start(文件)
                Catch ex As Exception
                    出错(ex)
                End Try
            Else
                If 文件存在(文件) Then
                    Dim pi As New ProcessStartInfo
                    With pi
                        .FileName = 文件
                        .Arguments = 参数
                        .WindowStyle = 窗口样式
                        If 管理员权限运行 Then .Verb = "runas"
                    End With
                    Try
                        Dim p As Process = Process.Start(pi)
                        If 等到运行结束 Then
                            Dim n As Single = 0
                            Do Until p.HasExited
                                Thread.Sleep(100)
                                n += 0.1
                                If 超时 > 0 AndAlso n > 超时 Then p.Kill()
                            Loop
                        Else
                            p.Dispose()
                        End If
                    Catch ex As Exception
                        出错(ex)
                    End Try
                Else
                    出错("打开程序，文件不存在", 文件)
                End If
            End If
        End Sub

        ''' <summary>
        ''' 判断该名字的程序是否在运行中，无需.exe
        ''' </summary>
        Public Function 程序运行中(程序名 As String) As Boolean
            If 程序名.Length < 1 Then Return False
            Return Process.GetProcessesByName(程序名).Length > 0
        End Function

        ''' <summary>
        ''' 出错的时候会呼叫这个event
        ''' </summary>
        Public Event WL出错(内容 As String)

        ''' <summary>
        ''' 出错的时候会call这个sub
        ''' </summary>
        Public Sub 出错(ParamArray 内容() As Object)
            Dim s As String = "WL出错："
            For Each i As Object In 内容
                Dim m As String
                If i.GetType.ToString.ToLower.Contains("exception") Then
                    Dim e As Exception = i
                    m = e.TargetSite.Name + vbCrLf + e.Message + vbCrLf + e.StackTrace
                Else
                    If IsNothing(i) Then i = "<nothing>"
                    m = 替换(i.ToString, vbCr, "[CR]", vbLf, "[LF]")
                    Dim m2 As String = 文本标准化(m)
                    If m2 <> m Then m = "标准化后：" + m2
                End If
                If m.Length < 1 Then m = "<empty string>"
                s += m + vbCrLf
            Next
            RaiseEvent WL出错(s)
            输出(s)
        End Sub

        ''' <summary>
        ''' 输出一条计时，包括距离上次计时的间距
        ''' </summary>
        Public Sub 计时()
            Static m As Double = Microsoft.VisualBasic.Timer
            Dim n As Double = Microsoft.VisualBasic.Timer
            输出(Math.Round(n - m, 3), 时间格式化(Now, "h:m:s"))
            m = n
        End Sub

        ''' <summary>
        ''' 运行Powershell脚本，并返回运行结果，只能是全自动脚本，不然会卡住
        ''' </summary>
        Public Function PowerShell运行脚本(脚本 As String) As String
            Dim s As String = ""
            If 脚本.Length > 0 Then
                Try
                    Dim p As PowerShell = PowerShell.Create()
                    p.AddScript(脚本)
                    Dim c As Collection(Of PSObject) = p.Invoke
                    For Each i As PSObject In c
                        If Not IsNothing(i) Then
                            s += i.ToString + vbCrLf
                        End If
                    Next
                    p.Dispose()
                Catch ex As Exception
                    出错(ex)
                End Try
            End If
            Return 文本标准化(s)
        End Function

        ''' <summary>
        ''' 一个用窗体线程的 Timer
        ''' </summary>
        Public Class 计时器
            Implements IDisposable

            Private tm As System.Windows.Forms.Timer, ts As UInteger, worktimes As UInteger

            ''' <summary>
            ''' 新建一个用窗体线程的 Timer，不会自动启动
            ''' </summary>
            Public Sub New(间隔 As Integer, Optional 事件 As EventHandler = Nothing)
                tm = New Windows.Forms.Timer
                tm.Enabled = False
                Me.间隔 = 间隔
                新增事件(事件)
                ts = 0
                worktimes = 0
            End Sub

            ''' <summary>
            ''' 释放 Timer 的资源
            ''' </summary>
            Public Sub Dispose() Implements IDisposable.Dispose
                tm.Dispose()
                Finalize()
            End Sub

            ''' <summary>
            ''' 间隔，也就是 Interval，单位为毫秒，如果设置的比1还小就会直接停止工作，并且需要手动重启
            ''' </summary>
            Public Property 间隔 As Integer
                Get
                    Return tm.Interval
                End Get
                Set(v As Integer)
                    If v < 1 Then
                        tm.Enabled = False
                    End If
                    tm.Interval = v
                End Set
            End Property

            ''' <summary>
            ''' 也就是 Enabled
            ''' </summary>
            Public Property 启用 As Boolean
                Get
                    Return tm.Enabled
                End Get
                Set(v As Boolean)
                    tm.Enabled = v
                End Set
            End Property

            ''' <summary>
            ''' 新增一个计时器时间到的时候会进行的事件，无法删除老事件，只能删除本计时器后重新做一个
            ''' </summary>
            Public Sub 新增事件(事件 As EventHandler)
                If 非空(事件) Then AddHandler tm.Tick, Sub()
                                                       事件.Invoke(Nothing, Nothing)
                                                       If 工作次数 > 0 Then
                                                           ts += 1
                                                           If ts >= 工作次数 Then
                                                               启用 = False
                                                               ts = 0
                                                           End If
                                                       End If
                                                   End Sub
            End Sub

            ''' <summary>
            ''' 工作几次后会自动停止
            ''' </summary>
            Public Property 工作次数 As UInteger
                Get
                    Return worktimes
                End Get
                Set(v As UInteger)
                    If worktimes = v Then Exit Property
                    worktimes = v
                    ts = 0
                End Set
            End Property

        End Class

        ''' <summary>
        ''' 使用系统的文件对话框选取文件文件夹
        ''' </summary>
        Public NotInheritable Class 系统文件对话框

            Private Shared Sub SetFolder(x As Windows.Forms.FileDialog)
                With x
                    .AddExtension = True
                    .CheckPathExists = True
                    .SupportMultiDottedExtensions = True
                    .DereferenceLinks = True
                    .SupportMultiDottedExtensions = True
                    .ValidateNames = True
                End With
            End Sub

            ''' <summary>
            ''' 打开窗口并选择一个文件，过滤写法是 Text files (*.txt)|*.txt|All files (*.*)|*.*
            ''' </summary>
            Public Shared Function 打开文件(Optional 过滤 As String = "") As String
                Dim t As New Windows.Forms.OpenFileDialog
                With t
                    SetFolder(t)
                    .CheckFileExists = True
                    .Multiselect = False
                    If 非空(过滤) Then .Filter = 过滤
                    .ShowDialog()
                    Return .FileName
                End With
            End Function

            ''' <summary>
            ''' 打开窗口并选择多个文件，过滤写法是 Text files (*.txt)|*.txt|All files (*.*)|*.*
            ''' </summary>
            Public Shared Function 打开多个文件(Optional 过滤 As String = "") As String()
                Dim t As New Windows.Forms.OpenFileDialog
                With t
                    SetFolder(t)
                    .CheckFileExists = True
                    .Multiselect = True
                    If 非空(过滤) Then .Filter = 过滤
                    .ShowDialog()
                    Return .FileNames
                End With
            End Function

            ''' <summary>
            ''' 打开窗口并选择单个文件夹
            ''' </summary>
            Public Shared Function 打开文件夹() As String
                Dim t As New FolderBrowserDialog
                With t
                    .ShowDialog()
                    Return 路径标准化(.SelectedPath)
                End With
            End Function

            ''' <summary>
            ''' 打开窗口并保存单个文件，过滤写法是 Text files (*.txt)|*.txt|All files (*.*)|*.*
            ''' </summary>
            Public Shared Function 保存文件(Optional 过滤 As String = "") As String
                Dim t As New Windows.Forms.SaveFileDialog
                With t
                    SetFolder(t)
                    .CheckFileExists = False
                    If 非空(过滤) Then .Filter = 过滤
                    .ShowDialog()
                    Return .FileName
                End With
            End Function

        End Class

    End Module

End Namespace