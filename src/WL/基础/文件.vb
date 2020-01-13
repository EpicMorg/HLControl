Namespace 基础

    ''' <summary>
    ''' 文件信息处理模块
    ''' </summary>
    Public Module 文件

        ''' <summary>
        ''' 把路径进行标准化处理，最后带一个\
        ''' </summary>
        Public Function 路径标准化(路径 As String) As String
            If 路径.Length > 2 Then
                路径 = 文本标准化(路径).Trim
                Dim f As String = "\"
                路径 = 替换(路径, "/", f)
                If Not 尾(路径, f) Then 路径 += f
            End If
            Return 路径
        End Function

        ''' <summary>
        ''' 判断这个文件名或者文件夹名当中是否包含非法字符
        ''' </summary>
        Public Function 是合法文件名(名字 As String) As Boolean
            名字 = 名字.Trim
            Dim l As Integer = 名字.Length
            If l < 1 OrElse l > 250 Then Return False
            For Each i As Char In "/\:*?<>|" + 引号
                If 名字.Contains(i) Then Return False
            Next
            Return True
        End Function

        ''' <summary>
        ''' 返回文本当中的小写盘符
        ''' </summary>
        Public Function 盘符(文件 As String) As String
            If 文件.Length > 2 Then Return 去除(Regex.Match(文件.ToLower, "[a-z]*?:").ToString, ":")
            Return ""
        End Function

        ''' <summary>
        ''' 如果是文件夹就返回上层路径，如果是文件就返回路径
        ''' </summary>
        Public Function 文件路径(文件 As String) As String
            If 文件.Length > 2 Then
                If 文件.EndsWith("\") Then 文件 = 去右(文件, 1)
                Return 路径标准化(Regex.Match(文件, "(\S|\s)+\\").ToString)
            End If
            Return ""
        End Function

        ''' <summary>
        ''' 如果是文件夹就返回文件夹名，如果是文件就返回文件名，默认包括后缀
        ''' </summary>
        Public Function 文件名(文件 As String, Optional 包含后缀 As Boolean = True) As String
            If 文件.Length > 2 Then
                If 文件.EndsWith("\") Then 文件 = 去右(文件, 1)
                文件 = 去除(文件, 文件路径(文件)).Trim
                If 包含后缀 = False AndAlso 文件.Length > 0 Then
                    文件 = Regex.Match(文件, ".*?\.").ToString
                    If 文件.EndsWith(".") Then 文件 = 去右(文件, 1)
                End If
                Return 文件
            End If
            Return ""
        End Function

        ''' <summary>
        ''' 返回文件的小写后缀，包括第一个点
        ''' </summary>
        Public Function 文件后缀(文件 As String) As String
            If 文件.Length > 2 Then
                If 文件.EndsWith("\") Then 文件 = 去右(文件, 1)
                文件 = 去除(文件, 文件路径(文件)).Trim
                If 文件.Length > 2 Then Return Regex.Match(去除(文件, 文件路径(文件)).Trim, "\.[\S]+").ToString.ToLower
            End If
            Return ""
        End Function

        ''' <summary>
        ''' 检查这个文件是否存在 
        ''' </summary>
        Public Function 文件存在(文件 As String) As Boolean
            If 文件.Length < 3 Then Return False
            Return File.Exists(文件)
        End Function

        ''' <summary>
        ''' 检查这个文件夹是否存在 
        ''' </summary>
        Public Function 文件夹存在(文件夹 As String) As Boolean
            If 文件夹.Length < 3 Then Return False
            Return Directory.Exists(文件夹)
        End Function

        ''' <summary>
        ''' 返回这个文件的大小，单位为Byte，如果文件不存在则返回0
        ''' </summary>
        Public Function 文件大小Byte(文件 As String) As UInteger
            If 文件存在(文件) Then Return FileLen(文件)
            Return 0
        End Function

        ''' <summary>
        ''' 返回这个文件的大小，单位为KB，如果文件不存在则返回0
        ''' </summary>
        Public Function 文件大小KB(文件 As String) As Single
            Return 文件大小Byte(文件) / 1024
        End Function

        ''' <summary>
        ''' 返回这个文件的大小，单位为MB，如果文件不存在则返回0
        ''' </summary>
        Public Function 文件大小MB(文件 As String) As Single
            Return 文件大小Byte(文件) / 1024 / 1024
        End Function

        ''' <summary>
        ''' 读取文件大小，并自动选择合适的单位返回一个字符串，只保留整数
        ''' </summary>
        Public Function 文件大小文字(文件 As String) As String
            Return 文件大小文字(文件大小Byte(文件))
        End Function

        ''' <summary>
        ''' 根据long值，单位应该为B，自动选择合适的单位返回一个字符串，只保留整数
        ''' </summary>
        Public Function 文件大小文字(大小 As Long) As String
            Select Case 大小
                Case < 1024
                    Return 大小 & "B"
                Case < 1024 * 1024
                    Return Int(大小 / 1024) & "KB"
                Case < 1024 * 1024 * 1024
                    Return Int(大小 / 1024 / 1024) & "MB"
                Case Else
                    Return Int(大小 / 1024 / 1024 / 1024) & "GB"
            End Select
        End Function

        ''' <summary>
        ''' 读取文件的内容为字节数组，如果无法读取，则返回nothing
        ''' </summary>
        Public Function 读文件为字节数组(文件 As String) As Byte()
            If 文件存在(文件) Then
                Dim b() As Byte = Nothing
                Try
                    Dim s As Stream = File.OpenRead(文件), c As Integer = s.Length - 1
                    ReDim b(c)
                    For i As Integer = 0 To c
                        b(i) = s.ReadByte
                    Next
                    s.Close()
                    Return b
                Catch ex As Exception
                    出错(ex)
                End Try
            End If
            Return Nothing
        End Function

        ''' <summary>
        ''' 读取文件的内容为字符串，并且会进行标准化处理
        ''' </summary>
        Public Function 读文件为文本(文件 As String, Optional 编码 As Encoding = Nothing) As String
            Dim b() As Byte = 读文件为字节数组(文件)
            If 为空(b) Then Return ""
            Return 字节数组转文本(b, 编码)
        End Function

        ''' <summary>
        ''' 删除文件或文件夹，返回删除是否成功，如果不存在也算删除成功
        ''' </summary>
        Public Function 删除文件(文件 As String) As Boolean
            Try
                If 文件存在(文件) Then File.Delete(文件)
                If 文件夹存在(文件) Then Directory.Delete(文件, True)
            Catch ex As Exception
                出错(ex)
            End Try
            Return 文件存在(文件) = False AndAlso 文件夹存在(文件) = False
        End Function

        ''' <summary>
        ''' 尝试创建文件夹，返回文件夹是否已经成功存在
        ''' </summary>
        Public Function 创建文件夹(路径 As String) As Boolean
            If 文件夹存在(路径) Then Return True
            Dim ok As Boolean = False
            Try
                Directory.CreateDirectory(路径)
            Catch ex As Exception
                出错(ex)
            End Try
            Return 文件夹存在(路径)
        End Function

        ''' <summary>
        ''' 删除文件，然后重新写入对应的字节数组，返回是否写入成功
        ''' </summary>
        Public Function 写字节数组到文件(文件 As String, 字节数组() As Byte) As Boolean
            If 删除文件(文件) AndAlso 创建文件夹(文件路径(文件)) Then
                Dim ok As Boolean = False
                Try
                    Dim s As Stream = File.OpenWrite(文件)
                    Dim c As Integer = 字节数组.Length
                    s.Write(字节数组, 0, c)
                    s.Close()
                    Return 文件大小Byte(文件) = c
                Catch ex As Exception
                    出错(ex)
                End Try
            End If
            Return False
        End Function

        ''' <summary>
        ''' 删除文件，然后重新写入对应的文本，返回是否写入成功
        ''' </summary>
        Public Function 写文本到文件(文件 As String, 文本 As String, Optional 编码 As Encoding = Nothing) As Boolean
            Return 写字节数组到文件(文件, 文本转字节数组(文本, 编码))
        End Function

        ''' <summary>
        ''' 把路径拆分开来
        ''' </summary>
        Public Function 路径拆分(路径 As String) As List(Of String)
            Return 分割(路径标准化(路径), "\")
        End Function

        ''' <summary>
        ''' 复制文件到新文件夹并重命名，并返回是否复制成功
        ''' </summary>
        Public Function 文件复制重命名(文件 As String, 新文件 As String) As Boolean
            Dim 路径 As String = 文件路径(新文件)
            If 文件存在(文件) = False OrElse 创建文件夹(路径) = False Then Return False
            If 删除文件(新文件) Then
                Try
                    File.Copy(文件, 新文件, True)
                    Return 文件大小Byte(新文件) = 文件大小Byte(文件)
                Catch ex As Exception
                    出错(ex)
                End Try
            End If
            Return False
        End Function

        ''' <summary>
        ''' 复制文件到新文件夹，并返回是否复制成功
        ''' </summary>
        Public Function 文件复制(文件 As String, 路径 As String) As Boolean
            Return 文件复制重命名(文件, 路径 + 文件名(文件))
        End Function

    End Module

End Namespace