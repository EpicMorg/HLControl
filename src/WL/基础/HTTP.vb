Namespace 基础

    ''' <summary>
    ''' HTTP相关操作模块
    ''' </summary>
    Public Module HTTP

        ''' <summary>
        ''' HTTP请求用的常用的浏览器UA
        ''' </summary>
        Public NotInheritable Class 常用UserAgent

            Public Const Chrome64 As String = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.167 Safari/537.36"
            Public Const Firefox63 As String = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:63.0) Gecko/20100101 Firefox/63.0"
            Public Const Steam As String = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; Valve Steam Client/default/1522709999; ) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36"
            Public Const IE11 As String = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; rv:11.0) like Gecko "
            Public Const IE8 As String = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)"
            Public Const iPhone As String = "Mozilla/5.0 (iPhone; CPU iPhone OS 10_3_3 like Mac OS X) AppleWebKit/603.3.8 (KHTML, like Gecko) Version/10.0 Mobile/14G60 Safari/602.1"
            Public Const Edge As String = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.140 Safari/537.36 Edge/18.17763"

            Protected Sub New()
            End Sub

        End Class

        ''' <summary>
        ''' 简单的发送HTTP请求并获得回应
        ''' </summary>
        Public Class 发送HTTP

            Private headers As WebHeaderCollection, write As List(Of Byte), err As String

            ''' <summary>
            ''' 新建一个HTTP请求
            ''' </summary>
            Public Sub New(链接 As String, Optional 方法 As String = "GET")
                headers = New WebHeaderCollection
                Me.链接 = 链接
                Me.方法 = 方法
                UA = 常用UserAgent.iPhone
                Accept = ""
                Content_Type = "application/x-www-form-urlencoded; charset=UTF-8"
                Cookie = ""
                Origin = ""
                Referer = ""
                write = New List(Of Byte)
                超时 = 8
                重试次数 = 0
                重试间隔时间 = 3
                err = ""
            End Sub

            ''' <summary>
            ''' 请求用的链接
            ''' </summary>
            Public Property 链接 As String

            ''' <summary>
            ''' Accept 请求头用来告知客户端可以处理的内容类型
            ''' 默认为空
            ''' </summary>
            Public Property Accept As String

            ''' <summary>
            ''' 请求用的方法 GET POST DELETE PUT 等
            ''' 默认为 GET
            ''' </summary>
            Public Property 方法 As String

            ''' <summary>
            ''' Content-Type 实体头部用于指示资源的MIME类型
            ''' 默认为 application/x-www-form-urlencoded; charset=UTF-8
            ''' </summary>
            Public Property Content_Type As String

            ''' <summary>
            ''' 请求的 User-Agent
            ''' 默认为 Mozilla/5.0 (iPhone; CPU iPhone OS 10_3_3 like Mac OS X) AppleWebKit/603.3.8 (KHTML, like Gecko) Version/10.0 Mobile/14G60 Safari/602.1
            ''' </summary>
            Public Property UA As String

            ''' <summary>
            ''' 请求用的 cookie
            ''' 默认为空
            ''' </summary>
            Public Property Cookie As String
                Get
                    Return 自定义头("Cookie")
                End Get
                Set(值 As String)
                    自定义头("Cookie") = 值
                End Set
            End Property

            ''' <summary>
            ''' Origin 首部字段表明预检请求或实际请求的源站
            ''' 默认为空
            ''' </summary>
            Public Property Origin As String
                Get
                    Return 自定义头("Origin")
                End Get
                Set(值 As String)
                    自定义头("Origin") = 值
                End Set
            End Property

            ''' <summary>
            ''' Referer 首部包含了当前请求页面的来源页面的地址，即表示当前页面是通过此来源页面里的链接进入的。
            ''' 默认为空
            ''' </summary>
            Public Property Referer As String

            ''' <summary>
            ''' 设置几秒后超时
            ''' 默认为8秒
            ''' </summary>
            Public Property 超时 As UInteger

            ''' <summary>
            ''' 自定义请求头的集合
            ''' </summary>
            Public Property 自定义头(名字 As String) As String
                Get
                    If 名字.Length < 1 Then Return ""
                    Return 文本标准化(headers.Get(名字))
                End Get
                Set(值 As String)
                    If 名字.Length > 0 Then
                        headers.Remove(名字)
                        If 值.Length > 0 Then headers.Add(名字, 值)
                    End If
                End Set
            End Property

            ''' <summary>
            ''' 清空已经向请求内写入的各种内容
            ''' </summary>
            Public Sub 清空已写入内容()
                write.Clear()
            End Sub

            ''' <summary>
            ''' 向请求内写入字节数组
            ''' </summary>
            Public Sub 写入字节数组(字节数组 As Byte())
                If 非空(字节数组) Then write.AddRange(字节数组)
            End Sub

            ''' <summary>
            ''' 向请求内写入文本
            ''' </summary>
            Public Sub 写入文本(文本 As String, Optional 编码 As Encoding = Nothing)
                If 文本.Length > 0 Then 写入字节数组(文本转字节数组(文本, 编码))
            End Sub

            ''' <summary>
            ''' 写入FormData
            ''' </summary>
            Public Sub 写入FormData(生成器 As 生成FormData)
                清空已写入内容()
                方法 = "POST"
                写入文本(生成器.ToString)
            End Sub

            ''' <summary>
            ''' 写入 multipart/form-data
            ''' </summary>
            Public Sub 写入multipartformdata(生成器 As 生成multipartformdata)
                清空已写入内容()
                方法 = "POST"
                Content_Type = "multipart/form-data; boundary=" + 生成器.分隔符
                写入字节数组(生成器.实际生成内容)
            End Sub

            ''' <summary>
            ''' 如果请求失败，重试的次数，默认不重试
            ''' </summary>
            Public Property 重试次数 As UInteger

            ''' <summary>
            ''' 重试请求的时候的时间间隔，单位为秒，默认是3秒
            ''' </summary>
            Public Property 重试间隔时间 As UInteger

            ''' <summary>
            ''' 对HTTP请求内容进行预览，默认不输出请求流的内容
            ''' </summary>
            Public ReadOnly Property 预览内容(Optional 输出请求流的内容 As Boolean = False) As String
                Get
                    Dim s As New 数据登记表()
                    With s
                        .增加("Url", 链接)
                        .增加("Method", 方法)
                        .增加("Timeout", 超时.ToString)
                        If 重试次数 > 0 Then
                            .增加("重试次数", 重试次数.ToString)
                            .增加("重试间隔时间", 重试间隔时间.ToString)
                        End If
                        .增加("Accept", Accept)
                        .增加("Content-Type", Content_Type)
                        Dim bs As Byte() = write.ToArray
                        .增加("Content-Length", bs.Length)
                        .增加("User-Agent", UA)
                        .增加("Referer", Referer)
                        For Each i As String In headers.AllKeys
                            .增加(i, headers.Get(i))
                        Next
                        If 输出请求流的内容 Then
                            .增加(字节数组转文本(bs))
                        End If
                    End With
                    Return s.ToString
                End Get
            End Property

            ''' <summary>
            ''' 发送请求，获取回应并读取为字节数组，如果出错会返回nothing
            ''' </summary>
            Public Function 获取回应为字节数组() As Byte()
                Dim a As Integer = 重试次数
                For retry As Integer = 0 To a
                    Try
                        Dim b() As Byte = Nothing
                        Dim h As HttpWebRequest = WebRequest.Create(链接)
                        With h
                            .Timeout = 超时 * 1000
                            .ReadWriteTimeout = .Timeout
                            .ContinueTimeout = .Timeout
                            .Method = 方法
                            If Accept.Length > 0 Then .Accept = Accept
                            If Content_Type.Length > 0 Then .ContentType = Content_Type
                            For Each i As String In headers.AllKeys
                                .Headers.Remove(i)
                                .Headers.Add(i, headers.Get(i))
                            Next
                            If Referer.Length > 0 Then .Referer = Referer
                            If UA.Length > 0 Then .UserAgent = UA
                            If write.Count > 0 Then
                                Dim w() As Byte = write.ToArray
                                .ContentLength = w.Length
                                .GetRequestStream.Write(w, 0, w.Length)
                            Else
                                .ContentLength = 0
                            End If
                            Dim s As Stream = .GetResponse.GetResponseStream()
                            b = 读取完整流(s)
                            s.Close()
                            Return b
                        End With
                        Thread.Sleep(重试间隔时间 * 1000)
                    Catch ex As Exception
                        出错(ex, IIf(retry > 0, "第" & retry & "次重试。", "首次失败。"))
                        err = ex.Message
                    End Try
                Next
                Return Nothing
            End Function

            ''' <summary>
            ''' 发送请求，获取回应并读取为字符串，如果出错会返回错误信息的字符串
            ''' 默认不去除引号，但默认反转义
            ''' </summary>
            Public Function 获取回应为字符串(Optional 编码 As Encoding = Nothing, Optional 反转义 As Boolean = True, Optional 去除引号 As Boolean = False) As String
                Dim b() As Byte = 获取回应为字节数组()
                If 为空(b) Then Return err
                Dim s As String = 字节数组转文本(b, 编码)
                If 去除引号 Then s = 去除(s, 引号)
                If 反转义 Then s = 文本.反转义(s)
                Return s
            End Function

        End Class

        ''' <summary>
        ''' HTTP请求用的 FormData 生成器
        ''' </summary>
        Public Class 生成FormData

            Private m As String

            ''' <summary>
            ''' 新建一个生成器并写入内容
            ''' </summary>
            Public Sub New(ParamArray 内容() As String)
                m = ""
                写入(内容)
            End Sub

            ''' <summary>
            ''' 写入内容
            ''' </summary>
            Public Sub 写入(ParamArray 内容() As String)
                Dim c As Integer = 内容.Length - 1
                If c > 0 Then
                    If 是偶数(c) Then c -= 1
                    If m.Length > 0 Then m += "&"
                    For i As Integer = 0 To c Step 2
                        m += 内容(i) + "=" + 内容(i + 1)
                        If i + 1 <> c Then m += "&"
                    Next
                End If
            End Sub

            ''' <summary>
            ''' 输出 FormData 原文
            ''' </summary>
            Public Overrides Function ToString() As String
                Return m
            End Function

        End Class

        ''' <summary>
        ''' HTTP请求用的 multipart/form-data 生成器
        ''' </summary>
        Public Class 生成multipartformdata

            Private m As List(Of Byte), pv As String, ec As Encoding

            ''' <summary>
            ''' 新建一个 multipart/form-data 生成器，支持自定义分隔符和自定义编码
            ''' </summary>
            Public Sub New(Optional 自定义分隔符 As String = "", Optional 自定义编码 As Encoding = Nothing)
                pv = ""
                m = New List(Of Byte)
                If 自定义分隔符.Length > 0 Then
                    分隔符 = 自定义分隔符
                Else
                    分隔符 = "--------WebKitFormBoundary" + 随机.当中字符(小写英文字母, 10)
                End If
                If 为空(自定义编码) Then
                    ec = 无BOM的UTF8编码()
                Else
                    ec = 自定义编码
                End If
            End Sub

            ''' <summary>
            ''' 返回随机生成的 boundary
            ''' </summary>
            Public ReadOnly Property 分隔符 As String

            Private Sub AddHead()
                Dim s As String = "--" + 分隔符
                If m.Count > 5 Then s = vbCrLf + s
                s += vbCrLf
                m.AddRange(文本转字节数组(s, ec))
                pv += s
            End Sub

            ''' <summary>
            ''' 写入一个参数，默认不写类型，无需对写入内容进行URL编码
            ''' </summary>
            Public Sub 写入参数(名字 As String, 内容 As String, Optional 类型 As String = "")
                If 名字.Length < 1 Then Exit Sub
                AddHead()
                Dim s As String = "Content-Disposition: form-data; name=" + 引(URL编码(名字)) + vbCrLf
                If 类型.Length > 0 Then s += "Content-Type: " + 类型 + vbCrLf
                s += vbCrLf + URL编码(内容)
                m.AddRange(文本转字节数组(s, ec))
                pv += s
            End Sub

            ''' <summary>
            ''' 写入字节数组
            ''' </summary>
            Public Sub 写入字节数组(名字 As String, 文件名 As String, 类型 As String, 字节数组 As Byte())
                If Not (字节数组.Length > 0 AndAlso 文件名.Length > 0 AndAlso 类型.Length > 0) Then Exit Sub
                AddHead()
                Dim s As String = "Content-Disposition: form-data; name=" + 引(URL编码(名字)) + "; filename=" + 引(URL编码(文件名)) + vbCrLf
                s += "Content-Type: " + 类型 + vbCrLf + vbCrLf
                m.AddRange(文本转字节数组(s, ec))
                m.AddRange(字节数组)
                pv += s + "[二进制内容 长度：" & 字节数组.Length & "]"
            End Sub

            ''' <summary>
            ''' 读取文件的二进制内容并写入
            ''' </summary>
            Public Sub 写入文件(名字 As String, 类型 As String, 文件 As String)
                Dim b() As Byte = 读文件为字节数组(文件)
                If 非空(b) Then 写入字节数组(名字, 文件名(文件), 类型, b)
            End Sub

            ''' <summary>
            ''' 返回实际生成的内容，HTTP请求用
            ''' </summary>
            Public ReadOnly Property 实际生成内容 As Byte()
                Get
                    Dim l As New List(Of Byte)
                    l.AddRange(m.ToArray)
                    l.AddRange(文本转字节数组(vbCrLf + "--" + 分隔符 + "--", ec))
                    Return l.ToArray
                End Get
            End Property

            ''' <summary>
            ''' 返回生成内容的预览文本
            ''' </summary>
            Public Overrides Function ToString() As String
                Return pv + vbCrLf + "--" + 分隔符 + "--"
            End Function

        End Class

    End Module

End Namespace
