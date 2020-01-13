Imports System.Drawing.Imaging
Imports System.Windows.Media.Imaging

Namespace 基础

    ''' <summary>
    ''' 图片处理模块
    ''' </summary>
    Public Module 图片

        ''' <summary>
        ''' 把图片的指定范围裁剪出来成新的图片
        ''' </summary>
        Public Function 裁剪(图片 As Bitmap, 范围 As Rectangle) As Bitmap
            If 为空(图片) Then Return Nothing
            Return 图片.Clone(范围, 图片.PixelFormat)
        End Function

        ''' <summary>
        ''' 获取Bitmap的帧数，普通图片帧数为1
        ''' </summary>
        Public Function 获取帧数(图片 As Bitmap) As UInteger
            If 为空(图片) Then Return 0
            Dim fd As New FrameDimension(图片.FrameDimensionsList(0))
            Return 图片.GetFrameCount(fd)
        End Function

        ''' <summary>
        ''' 根据图片格式，获取 ImageCodecInfo 的 Encoder
        ''' </summary>
        Public Function 获取图片编码器(格式 As ImageFormat) As ImageCodecInfo
            If 为空(格式) Then Return Nothing
            Try
                For Each i As ImageCodecInfo In ImageCodecInfo.GetImageEncoders
                    If i.FormatID = 格式.Guid Then
                        Return i
                    End If
                Next
            Catch ex As Exception
                出错(ex)
            End Try
            Return Nothing
        End Function

        ''' <summary>
        ''' 把图片变成字节数组，只保存PNG
        ''' </summary>
        Public Function 图片转字节数组(图片 As Bitmap) As Byte()
            If 为空(图片) Then Return Nothing
            Dim m As New MemoryStream
            Dim c As ImageFormat = ImageFormat.Png
            图片.Save(m, c)
            Return m.ToArray
        End Function

        ''' <summary>
        ''' 把字节数组变成图片，只支持单帧
        ''' </summary>
        Public Function 字节数组转图片(字节数组 As Byte()) As Bitmap
            If 非空(字节数组) Then
                Try
                    Dim m As New MemoryStream
                    m.Write(字节数组, 0, 字节数组.Length)
                    Dim b As Bitmap = Bitmap.FromStream(m)
                    m.Dispose()
                    Return b
                Catch ex As Exception
                    出错(ex)
                End Try
            End If
            Return Nothing
        End Function

        ''' <summary>
        ''' 把图片文件进行读取，并且不占用实际的本地文件 
        ''' </summary>
        Public Function 读图片文件(文件 As String) As Bitmap
            Dim g As Byte() = 读文件为字节数组(文件)
            If 为空(g) Then Return Nothing
            Try
                Dim m As New MemoryStream
                m.Write(g, 0, g.Length)
                Dim b As Bitmap = Bitmap.FromStream(m)
                Return b
            Catch ex As Exception
                出错(ex)
            End Try
            Return Nothing
        End Function

    End Module

End Namespace
