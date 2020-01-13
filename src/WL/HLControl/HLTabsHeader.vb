Imports Microsoft.VisualBasic.ApplicationServices

Namespace HLControl

    Public Class HLTabsHeader
        Inherits HLControlBase

        Private 开始 As Boolean, 边缘 As Integer, 标签宽 As UShort, tabs As TabControl, 计时器 As 计时器

        Public Sub New()
            DoubleBuffered = True
            开始 = False
            标签宽 = 100 * DPI
            边缘 = 3 * DPI
            tabs = Nothing
            计时器 = New 计时器(10, AddressOf FixTabs)
            计时器.启用 = False
            计时器.工作次数 = 4
        End Sub

        Public Property TabHeaderWidth As UShort
            Get
                Return 标签宽
            End Get
            Set(v As UShort)
                设最小值(v, 30 * DPI)
                设最大值(v, Width / 3)
                If v <> 标签宽 Then
                    标签宽 = v
                    Invalidate()
                End If
            End Set
        End Property

        Public Property RealTabControl As TabControl
            Get
                Return tabs
            End Get
            Set(v As TabControl)
                If Not 开始 Then
                    tabs = v
                    If Not IsNothing(v) Then
                        With tabs
                            AddHandler .SelectedIndexChanged, Sub()
                                                                  Invalidate()
                                                              End Sub
                        End With
                    End If
                End If
            End Set
        End Property

        Private Sub _MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
            If 为空(tabs) Then Exit Sub
            Dim x As Integer = Fix(e.X / (TabHeaderWidth * DPI))
            If x < tabs.TabCount Then
                tabs.SelectedIndex = x
            End If
        End Sub

        Private Sub FixTabs()
            If 开始 AndAlso 非空(tabs) AndAlso 非空(FindForm) AndAlso tabs.Visible AndAlso FindForm.WindowState <> FormWindowState.Minimized Then
                With tabs
                    Dim g As Graphics = .CreateGraphics()
                    Dim r As Rectangle = .ClientRectangle
                    g.Clear(基础绿)
                    绘制基础矩形(g, r)
                    If 非空(.SelectedTab) Then .SelectedTab.BackColor = 基础绿
                End With
            End If
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            If 为空(FindForm) Then Exit Sub
            设最小值(Width, 90 * DPI)
            Dim h As Integer = Font.GetHeight + 12 * DPI
            设最小值(h, 30 * DPI)
            Height = h
            If 为空(tabs) OrElse tabs.TabPages.Count < 1 Then
                Dim n As Single = 当日时间戳()
                绘制基础矩形(e.Graphics, ClientRectangle,,, Color.Red)
                Exit Sub
            End If
            If 开始 = False AndAlso 本程序.真的运行中 Then
                开始 = True
                With tabs
                    .Visible = Visible
                    .DrawMode = TabDrawMode.OwnerDrawFixed
                    .Top = Bottom - 30 * DPI
                    .Left = Left
                    .Width = Width
                End With
                BringToFront()
            End If
            If 开始 Then
                计时器.启用 = True
            End If
            Dim g As Graphics = e.Graphics, x As Integer = 0, s As Integer = tabs.SelectedIndex
            Dim tb As TabPage
            With g
                Dim r As Rectangle, th As Single = TabHeaderWidth * DPI
                For i As Integer = 0 To tabs.TabPages.Count - 1
                    tb = tabs.TabPages.Item(i)
                    Try
                        tb.BackColor = 基础绿
                    Catch ex As Exception
                    End Try
                    r = New Rectangle(x, IIf(i = s, 0, 边缘 * 1.5), th, Height)
                    .FillRectangle(基础绿笔刷, r)
                    .DrawLine(边缘白笔, 左上角(r), 左下角(r))
                    .DrawLine(边缘白笔, 左上角(r), 右上角(r))
                    .DrawLine(暗色笔, 右上角(r), 右下角(r))
                    绘制文本(g, tb.Text, Font, x + 边缘, Height * 0.2, 获取文本状态(Enabled, i = s))
                    x += th + 线宽
                    If x > Width Then Exit For
                Next
                th += 线宽
                r = ClientRectangle
                .DrawLine(暗色笔, 点((s + 1) * th, Height), 右下角(r))
                If s > 0 Then
                    .DrawLine(暗色笔, 左下角(r), 点(s * th, Height))
                End If
            End With
        End Sub

    End Class

End Namespace

