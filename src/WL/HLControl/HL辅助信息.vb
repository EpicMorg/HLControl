Namespace HLControl

    Friend Module HL辅助信息

        Public ReadOnly DPI As Single = 系统信息.DPI

        Public ReadOnly 线宽 As Single = DPI * 3
        Public ReadOnly 细线宽 As Single = DPI * 2
        Public ReadOnly 虚线宽 As Single = DPI * 1

        Public ReadOnly 基础绿 As Color = Color.FromArgb(76, 88, 68)
        Public ReadOnly 基础绿笔刷 As New SolidBrush(基础绿)

        Public ReadOnly 内容绿 As Color = Color.FromArgb(62, 70, 55)
        Public ReadOnly 内容绿笔刷 As New SolidBrush(内容绿)

        Public ReadOnly 白色 As Color = Color.FromArgb(255, 255, 255)
        Public ReadOnly 白色笔 As New Pen(白色, 线宽)
        Public ReadOnly 白色笔刷 As New SolidBrush(白色)

        Public ReadOnly 淡色 As Color = Color.FromArgb(160, 170, 149)
        Public ReadOnly 淡色笔刷 As New SolidBrush(淡色)

        Public ReadOnly 暗色 As Color = Color.FromArgb(40, 46, 34)
        Public ReadOnly 暗色笔 As New Pen(暗色, 线宽)
        Public ReadOnly 暗色笔刷 As New SolidBrush(暗色)

        Public ReadOnly 边缘白 As Color = Color.FromArgb(134, 145, 128)
        Public ReadOnly 边缘白笔 As New Pen(边缘白, 线宽)
        Public ReadOnly 边缘白笔刷 As New SolidBrush(边缘白)

        Public ReadOnly 黑边框 As New Pen(Color.Black, 虚线宽)
        Public ReadOnly 黑虚线边框 As New Pen(Color.Black, 虚线宽) With {.DashStyle = Drawing2D.DashStyle.Dot}

        Public ReadOnly 内容黄 As Color = Color.FromArgb(196, 181, 80)
        Public ReadOnly 内容黄笔刷 As New SolidBrush(内容黄)

        Public ReadOnly 内容白 As Color = Color.FromArgb(216, 222, 211)
        Public ReadOnly 内容白笔 As New Pen(内容白, 线宽)
        Public ReadOnly 内容白笔刷 As New SolidBrush(内容白)

        Public ReadOnly 块黄 As Color = Color.FromArgb(149, 136, 49)
        Public ReadOnly 块黄笔刷 As New SolidBrush(块黄)

        Public ReadOnly 滚动绿 As Color = Color.FromArgb(90, 106, 80)
        Public ReadOnly 滚动绿笔刷 As New SolidBrush(滚动绿)

        Public ReadOnly 禁用底色 As Color = Color.FromArgb(117, 128, 111)
        Public ReadOnly 禁用底色笔刷 As New SolidBrush(禁用底色)

        Public ReadOnly 边缘灰 As Color = Color.FromArgb(162, 156, 154)
        Public ReadOnly 边缘灰笔 As New Pen(边缘灰, 细线宽)
        Public ReadOnly 边缘灰笔刷 As New SolidBrush(边缘灰)

        Public ReadOnly 细线灰 As Color = Color.FromArgb(127, 140, 127)
        Public ReadOnly 细线灰笔 As New Pen(细线灰, 细线宽)

        Public ReadOnly 按钮灰 As Color = Color.FromArgb(193, 191, 189)
        Public ReadOnly 按钮灰笔 As New Pen(按钮灰, 细线宽)

        Public Function 点(x As Integer, y As Integer) As Point
            Return New Point(x, y)
        End Function

        Public Function 点F(x As Integer, y As Integer) As PointF
            Return New PointF(x, y)
        End Function

        Public Function 笔(颜色 As Color, Optional 宽度 As Single = 1) As Pen
            Return New Pen(颜色, 宽度)
        End Function

        Public Function 笔刷(颜色 As Color) As SolidBrush
            Return New SolidBrush(颜色)
        End Function

        Public Function 左上角(c As Rectangle, Optional 差 As Integer = 0) As Point
            Return 点(c.Left + 差, c.Top + 差)
        End Function

        Public Function 左下角(c As Rectangle, Optional 差 As Integer = 0) As Point
            Return 点(c.Left + 差, c.Bottom - 差)
        End Function

        Public Function 右上角(c As Rectangle, Optional 差 As Integer = 0) As Point
            Return 点(c.Right - 差, c.Top + 差)
        End Function

        Public Function 右下角(c As Rectangle, Optional 差 As Integer = 0) As Point
            Return 点(c.Right - 差, c.Bottom - 差)
        End Function

        Public Sub 绘制基础矩形(g As Graphics, c As Rectangle, Optional 按下 As Boolean = False, Optional 黑框 As Boolean = False, Optional 内容颜色 As Color = Nothing)
            If 为空(内容颜色) Then 内容颜色 = 基础绿
            With g
                .FillRectangle(New SolidBrush(内容颜色), c)
                Dim s1 As Pen = IIf(按下, 暗色笔, 边缘白笔)
                Dim s2 As Pen = IIf(按下, 边缘白笔, 暗色笔)
                .DrawLine(s1, 左上角(c), 左下角(c))
                .DrawLine(s1, 左上角(c), 右上角(c))
                .DrawLine(s2, 右上角(c), 右下角(c))
                .DrawLine(s2, 左下角(c), 右下角(c))
                If 黑框 Then
                    .DrawRectangle(黑边框, c)
                    Dim d As Single = DPI * 4
                    .DrawRectangle(黑虚线边框, d, d, c.Width - d * 2, c.Height - d * 2)
                End If
            End With
        End Sub

        Public Function 文本宽度(文本 As String, 字体 As Font) As Single
            If 为空(文本, 字体) Then Return 0
            Static b As New Bitmap(1, 1)
            Static g As Graphics = Graphics.FromImage(b)
            Dim s As Single = g.MeasureString(文本, 字体).Width - 2
            If s < 0 Then s = 0
            Return s
        End Function

        Public Sub 修正Dock(c As Control, Optional 允许横向 As Boolean = True, Optional 允许竖向 As Boolean = False)
            Dim d As DockStyle = c.Dock, ok As Boolean = True
            Select Case d
                Case DockStyle.None
                    Exit Sub
                Case DockStyle.Top
                    ok = 允许横向
                Case DockStyle.Bottom
                    ok = 允许横向
                Case DockStyle.Left
                    ok = 允许竖向
                Case DockStyle.Right
                    ok = 允许竖向
                Case DockStyle.Fill
                    ok = 允许竖向 AndAlso 允许横向
            End Select
            If Not ok Then c.Dock = DockStyle.None
        End Sub

        Public Enum HL文本状态
            正常白
            黄色高亮
            副文本黯淡
            禁用
        End Enum

        Public Function 获取文本状态(启用 As Boolean, Optional 高光 As Boolean = False, Optional 黯淡 As Boolean = False) As HL文本状态
            If Not 启用 Then Return HL文本状态.禁用
            If 高光 Then Return HL文本状态.黄色高亮
            If 黯淡 Then Return HL文本状态.副文本黯淡
            Return HL文本状态.正常白
        End Function

        ''' <summary>
        ''' 状态： 0正常白 1黄色高亮 2副文本黯淡 3禁用
        ''' </summary>
        Public Sub 绘制文本(g As Graphics, t As String, f As Font, x As Single, y As Single, Optional 状态 As HL文本状态 = 0)
            If 为空(g, t, f) Then Exit Sub
            With g
                Dim c As Color = 内容白
                Select Case 状态
                    Case HL文本状态.黄色高亮
                        c = 内容黄
                    Case HL文本状态.副文本黯淡
                        c = 淡色
                    Case HL文本状态.禁用
                        c = 暗色
                        .DrawString(t, f, 禁用底色笔刷, 点F(x + DPI, y + DPI))
                End Select
                .DrawString(t, f, New SolidBrush(c), 点F(x, y))
            End With
        End Sub

        Public Sub 绘制圆角矩形(g As Graphics, p As Pen, 矩形 As Rectangle, 半径 As Integer)
            矩形.Offset(-1, -1)
            Dim RoundRect As New Rectangle(矩形.Location, New Size(半径 - 1, 半径 - 1))
            Dim path As New Drawing2D.GraphicsPath
            path.AddArc(RoundRect, 180, 90)
            RoundRect.X = 矩形.Right - 半径
            path.AddArc(RoundRect, 270, 90)
            RoundRect.Y = 矩形.Bottom - 半径
            path.AddArc(RoundRect, 0, 90)
            RoundRect.X = 矩形.Left
            path.AddArc(RoundRect, 90, 90)
            path.CloseFigure()
            g.DrawPath(p, path)
        End Sub

        Public Function 是HL控件(c As Control) As Boolean
            If 为空(c) Then Return False
            Dim s As String = c.GetType.ToString
            If s.StartsWith("WL.HLControl") AndAlso 包含(s.ToLower, "panel") = False Then
                Return True
            End If
            Return False
        End Function

    End Module

    Public Class HLValueEventArgs
        Inherits EventArgs

        Public Sub New(old As Object, [new] As Object)
            OldValue = old
            NewValue = [new]
        End Sub

        Public Property OldValue As Object

        Public Property NewValue As Object

        Public Overrides Function ToString() As String
            Return "{old:" + OldValue.ToString + ", new:" + NewValue.ToString + "}"
        End Function

    End Class

End Namespace