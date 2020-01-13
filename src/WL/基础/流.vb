Namespace 基础

    ''' <summary>
    ''' 针对 stream 进行处理的模块
    ''' </summary>
    Public Module 流

        ''' <summary>
        ''' 读取该stream一直到末尾，输出字节数组，这并不会关闭流
        ''' </summary>
        Public Function 读取完整流(流 As Stream) As Byte()
            If IsNothing(流) Then Return Nothing
            Try
                Dim b As New List(Of Byte), m As Integer = 1
                Do While True
                    m = 流.ReadByte()
                    If m < Byte.MinValue OrElse m > Byte.MaxValue Then Exit Do
                    b.Add(m)
                Loop
                Return b.ToArray
            Catch ex As Exception
                出错(ex)
            End Try
            Return Nothing
        End Function

        ''' <summary>
        ''' 读取流直到0字节或者流的末尾，然后输出该部分的字符串，这并不会关闭流
        ''' </summary>
        Public Function 读至零返回字符串(流 As Stream, Optional 编码 As Encoding = Nothing) As String
            If IsNothing(流) Then Return ""
            Dim b As New List(Of Byte), m As Integer = 1
            Do While True
                m = 流.ReadByte()
                If m < 1 OrElse m > Byte.MaxValue Then Exit Do
                b.Add(m)
            Loop
            Return 字节数组转文本(b.ToArray, 编码)
        End Function

    End Module

End Namespace