Namespace 基础

    ''' <summary>
    ''' 和Windows系统相关操作的模块
    ''' </summary>
    Public Module 系统

        ''' <summary>
        ''' 从剪贴板内获得字符串或图片
        ''' </summary>
        Public NotInheritable Class 剪贴板

            Protected Sub New()
            End Sub

            ''' <summary>
            ''' 清空剪贴板的内容
            ''' </summary>
            Public Shared Sub 清空()
                Try
                    My.Computer.Clipboard.Clear()
                Catch ex As Exception
                    出错(ex)
                End Try
            End Sub

            ''' <summary>
            ''' 检查剪贴板内是否是文本
            ''' </summary>
            Public Shared ReadOnly Property 有文本 As Boolean
                Get
                    Try
                        Return My.Computer.Clipboard.ContainsText
                    Catch ex As Exception
                        出错(ex)
                        Return False
                    End Try
                End Get
            End Property

            ''' <summary>
            ''' 检查剪贴板内是否是图片
            ''' </summary>
            Public Shared ReadOnly Property 有图片 As Boolean
                Get
                    Try
                        Return My.Computer.Clipboard.ContainsImage
                    Catch ex As Exception
                        出错(ex)
                        Return False
                    End Try
                End Get
            End Property

            ''' <summary>
            ''' 检查剪贴板内是否是文件列表
            ''' </summary>
            Public Shared ReadOnly Property 有文件列表 As Boolean
                Get
                    Try
                        Return My.Computer.Clipboard.ContainsFileDropList
                    Catch ex As Exception
                        出错(ex)
                        Return False
                    End Try
                End Get
            End Property

            ''' <summary>
            ''' 获取或设置剪贴板的字符串
            ''' </summary>
            Public Shared Property 文本 As String
                Get
                    Dim s As String = ""
                    Try
                        If My.Computer.Clipboard.ContainsText Then s = My.Computer.Clipboard.GetText
                        Return s
                    Catch ex As Exception
                        Return ""
                        出错(ex)
                    End Try
                End Get
                Set(内容 As String)
                    If 为空(内容) Then
                        清空()
                    Else
                        Try
                            My.Computer.Clipboard.SetText(内容)
                        Catch ex As Exception
                            出错(ex)
                        End Try
                    End If
                End Set
            End Property

            ''' <summary>
            ''' 获取或设置剪贴板的图片
            ''' </summary>
            Public Shared Property 图片 As Bitmap
                Get
                    Dim g As Image = Nothing
                    Try
                        If My.Computer.Clipboard.ContainsImage Then g = My.Computer.Clipboard.GetImage
                        Return g
                    Catch ex As Exception
                        出错(ex)
                        Return Nothing
                    End Try
                End Get
                Set(内容 As Bitmap)
                    If 为空(内容) Then
                        清空()
                    Else
                        Try
                            My.Computer.Clipboard.SetImage(内容)
                        Catch ex As Exception
                            出错(ex)
                        End Try
                    End If
                End Set
            End Property

            ''' <summary>
            ''' 获取剪贴板里存储的 Windows Explorer 的复制文件列表
            ''' </summary>
            Public Shared ReadOnly Property 文件列表 As List(Of String)
                Get
                    Dim g As New List(Of String)
                    Try
                        If My.Computer.Clipboard.ContainsFileDropList Then
                            For Each i As String In My.Computer.Clipboard.GetFileDropList()
                                If i.Length > 2 AndAlso g.Contains(i) = False Then g.Add(i)
                            Next
                        End If
                    Catch ex As Exception
                        出错(ex)
                    End Try
                    Return g
                End Get
            End Property

        End Class

        ''' <summary>
        ''' 获取Windows系统的一些软硬件信息
        ''' </summary>
        Public NotInheritable Class 系统信息

            Protected Sub New()
            End Sub

            ''' <summary>
            ''' 获取电脑CPU的名字字符串
            ''' </summary>
            Public Shared ReadOnly Property CPU型号 As String
                Get
                    Static s As String = 获取注册表("HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\CentralProcessor\0", "ProcessorNameString", "Unknown").Trim
                    Return s
                End Get
            End Property

            ''' <summary>
            ''' 返回电脑CPU的频率，单位为MHZ
            ''' </summary>
            Public Shared ReadOnly Property CPU频率 As UInteger
                Get
                    Static s As UInteger = Val(获取注册表("HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\CentralProcessor\0", "~MHz", "0"))
                    Return s
                End Get
            End Property

            ''' <summary>
            ''' 返回CPU的核心数量
            ''' </summary>
            Public Shared ReadOnly Property CPU核心数量 As UInteger
                Get
                    Static m As UInteger = Registry.LocalMachine.OpenSubKey("HARDWARE\DESCRIPTION\System\CentralProcessor", False).SubKeyCount
                    Return m
                End Get
            End Property

            ''' <summary>
            ''' 返回CPU的类型
            ''' </summary>
            Public Shared ReadOnly Property CPU类型 As String
                Get
                    Static s As String = 获取注册表("HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\CentralProcessor\0", "Identifier", "Unknown").Trim
                    Return s
                End Get
            End Property

            ''' <summary>
            ''' 获取电脑的显示器的分辨率，单位是像素
            ''' </summary>
            Public Shared ReadOnly Property 屏幕分辨率 As Size
                Get
                    Static m As Size = My.Computer.Screen.Bounds.Size
                    Return m
                End Get
            End Property

            ''' <summary>
            ''' 获取电脑的显示DPI，并强制转换为0.25的倍数
            ''' </summary>
            Public Shared ReadOnly Property DPI As Single
                Get
                    Static d As Single = 0
                    If d = 0 Then
                        Dim n As Integer = Val(获取注册表("HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "AppliedDPI", "100"))
                        If n < 100 Then n = 100
                        For i As Integer = 50 To 2000 Step 25
                            If Math.Abs(i - n) < 12.5 Then
                                n = i
                                Exit For
                            End If
                        Next
                        d = n / 100
                    End If
                    Return d
                End Get
            End Property

            ''' <summary>
            ''' 获取电脑的名字
            ''' </summary>
            Public Shared ReadOnly Property 电脑名 As String
                Get
                    Static m As String = My.Computer.Name
                    Return m
                End Get
            End Property

            ''' <summary>
            ''' 返回当前的系统用户的用户名
            ''' </summary>
            Public Shared ReadOnly Property 用户名 As String
                Get
                    Static m As String = 提取之后(My.User.Name, "\")
                    Return m
                End Get
            End Property

            ''' <summary>
            ''' 内存的大小，只包括物理内存，单位为MB
            ''' </summary>
            Public Shared ReadOnly Property 内存大小 As ULong
                Get
                    Static m As ULong = My.Computer.Info.TotalPhysicalMemory / 1024 / 1024
                    Return m
                End Get
            End Property

            Private Shared gpuRAM As ULong = 0

            ''' <summary>
            ''' 返回电脑的显卡的型号
            ''' </summary>
            Public Shared ReadOnly Property 显卡型号 As String
                Get
                    Static m As String = ""
                    If m.Length < 1 Then
                        Dim ra As RegistryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\Video\", False)
                        Dim r As RegistryKey = Nothing
                        If Not IsNothing(ra) Then
                            For Each i As String In ra.GetSubKeyNames
                                r = ra.OpenSubKey(i)
                                If IsNothing(r) = False Then
                                    r = r.OpenSubKey("0000")
                                    If IsNothing(r) = False Then
                                        Dim o As Object = r.GetValue("DriverDesc", Nothing)
                                        If IsNothing(o) = False AndAlso o.GetType = GetType(String) Then
                                            m = 文本标准化(o.ToString).Trim
                                        End If
                                        If m.Length > 0 Then
                                            gpuRAM = Math.Abs(Val(r.GetValue("HardwareInformation.MemorySize", 0).ToString)) / 1024 / 1024
                                            Exit For
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If
                    If m.Length < 1 Then m = "未知"
                    Return m
                End Get
            End Property

            ''' <summary>
            ''' 返回电脑的显卡的内存，单位为MB
            ''' </summary>
            Public Shared ReadOnly Property 显卡内存大小 As ULong
                Get
                    If gpuRAM < 1 Then
                        Dim a As String = 显卡型号
                    End If
                    Return gpuRAM
                End Get
            End Property

            ''' <summary>
            ''' 返回电脑的主板的型号
            ''' </summary>
            Public Shared ReadOnly Property 主板型号 As String
                Get
                    Static m As String = ""
                    If m.Length < 1 Then
                        m = 获取注册表("HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\BIOS", "BaseBoardProduct", "Unknown").Trim
                    End If
                    Return m
                End Get
            End Property

            ''' <summary>
            ''' 返回电脑的主板生产厂家的名字
            ''' </summary>
            Public Shared ReadOnly Property 主板品牌 As String
                Get
                    Static m As String = ""
                    If m.Length < 1 Then
                        m = 获取注册表("HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\BIOS", "BaseBoardManufacturer", "Unknown").Trim
                    End If
                    Return m
                End Get
            End Property

            ''' <summary>
            ''' 返回电脑的主板生产厂家的名字
            ''' </summary>
            Public Shared ReadOnly Property BIOS类型 As String
                Get
                    Static m As String = ""
                    If m.Length < 1 Then
                        m = 获取注册表("HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\BIOS", "BIOSVendor", "Unknown").Trim
                    End If
                    Return m
                End Get
            End Property

            ''' <summary>
            ''' 是否存在任意可用本地或互联网的网络连接
            ''' </summary>
            Public Shared ReadOnly Property 存在网络 As Boolean
                Get
                    Return My.Computer.Network.IsAvailable
                End Get
            End Property

            ''' <summary>
            ''' 获取本电脑的首选ipv4地址
            ''' </summary>
            Public Shared ReadOnly Property IPv4地址 As String
                Get
                    If 存在网络 Then
                        Dim s As String = PowerShell运行脚本("ipconfig")
                        If s.Length > 100 Then
                            s = s.ToLower()
                            s = 提取最之前(提取之后(s, "windows", "ipv4", ":"), vbCrLf).Trim
                        End If
                        Return s
                    End If
                    Return ""
                End Get
            End Property

            ''' <summary>
            ''' 获取本地的首选ipv4地址
            ''' </summary>
            Public Shared ReadOnly Property IPv6地址 As String
                Get
                    If 存在网络 Then
                        Dim s As String = PowerShell运行脚本("ipconfig")
                        If s.Length > 100 Then
                            s = s.ToLower()
                            s = 提取最之前(提取之后(s, "windows", "ipv6", ":"), vbCrLf).Trim
                        End If
                        Return s
                    End If
                    Return ""
                End Get
            End Property

            ''' <summary>
            ''' 应用程序的主题是否是暗色模式，只针对windows10，一般为false
            ''' </summary>
            ''' <returns></returns>
            Public Shared ReadOnly Property 暗色应用模式 As Boolean
                Get
                    Return Val(获取注册表("HKEY_CURRENT_USER\Software\Microsoft9\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", "1")) = 0
                End Get
            End Property

            ''' <summary>
            ''' 系统的主题是否是暗色模式，只针对windows10，一般为false
            ''' </summary>
            ''' <returns></returns>
            Public Shared ReadOnly Property 暗色系统模式 As Boolean
                Get
                    Return Val(获取注册表("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", "1")) = 0
                End Get
            End Property

            ''' <summary>
            ''' 从注册表里读取值
            ''' </summary>
            Public Shared ReadOnly Property 获取注册表(键 As String, 值名 As String, Optional 默认 As String = "") As String
                Get
                    Try
                        Return Registry.GetValue(键, 值名, 默认).ToString
                    Catch ex As Exception
                        出错(ex)
                        Return 默认
                    End Try
                End Get
            End Property

        End Class

    End Module

End Namespace