Namespace HLControl
    <DefaultEvent("ValueChanged")>
    Public Class HLHScrollBar
        Inherits HLControlBase

        Private 按住上 As Boolean, 按住 As Boolean, 按住下 As Boolean, 值 As Integer, 滚动一次 As Integer, 最大 As Integer, 最小 As Integer
        Private 上一个值 As Integer

        Public Sub New()
            DoubleBuffered = True
            按住上 = False
            按住下 = False
            按住 = False
            值 = 0
            滚动一次 = 1
            最大 = 100
            最小 = 0
            上一个值 = 0
        End Sub

        Private Sub FixValue()
            If 最大 = 最小 Then
                最大 += 1
            ElseIf 最大 < 最小 Then
                互换(最大, 最小)
            End If
            If 值 < 最小 Then
                值 = 最小
            ElseIf 值 > 最大 Then
                值 = 最大
            End If
            If 滚动一次 >= 最大 Then 滚动一次 = 最大 - 1
            If 滚动一次 <= 最小 Then 滚动一次 = 最小 + 1
            Invalidate()
            If 上一个值 <> Value Then
                RaiseEvent ValueChanged(Me, New HLValueEventArgs(上一个值, Value))
                上一个值 = Value
            End If
        End Sub

        <DefaultValue(100)>
        Public Property Maximum As Integer
            Get
                Return 最大
            End Get
            Set(v As Integer)
                If v <> 最大 Then
                    最大 = v
                    FixValue()
                End If
            End Set
        End Property

        <DefaultValue(0)>
        Public Property Minimum As Integer
            Get
                Return 最小
            End Get
            Set(v As Integer)
                最小 = v
                FixValue()
            End Set
        End Property

        <DefaultValue(0)>
        Public Property Value As Integer
            Get
                Return 值
            End Get
            Set(v As Integer)
                值 = v
                FixValue()
            End Set
        End Property

        <DefaultValue(1)>
        Public Property SmallChange As Integer
            Get
                Return 滚动一次
            End Get
            Set(v As Integer)
                滚动一次 = v
                FixValue()
            End Set
        End Property

        Public Sub ChangeValueWithoutRaiseEvent(v As Integer)
            设最大值(v, 最大)
            设最小值(v, 最小)
            If v <> 值 Then
                值 = v
                Invalidate()
            End If
        End Sub

        Public Event ValueChanged(sender As HLHScrollBar, e As HLValueEventArgs)

        Private Sub _MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
            Dim y As Integer = e.X, h As Integer = Height
            If y <= h Then
                按住上 = True
                Value -= 滚动一次
            ElseIf y >= Width - h Then
                按住下 = True
                Value += 滚动一次
            Else
                _MouseMove(sender, e)
            End If
        End Sub

        Private Sub _MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel
            If e.Delta > 0 Then
                Value -= 滚动一次
            Else
                Value += 滚动一次
            End If
        End Sub

        Private Sub _MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
            If e.Button <> MouseButtons.None AndAlso 按住上 = False AndAlso 按住下 = False Then
                Dim y As Integer = e.X, h As Integer = Height
                If 按住 = False AndAlso y > h AndAlso y < Width - h Then
                    按住 = True
                End If
                If 按住 Then Value = (y - h) / (Width - 2 * h) * (Maximum - Minimum) + Minimum
            End If
        End Sub

        Private Sub _MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
            If 按住 OrElse 按住上 OrElse 按住下 Then
                按住上 = False
                按住 = False
                按住下 = False
                Invalidate()
            End If
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            修正Dock(Me, True, False)
            设最小值(Width, 50 * DPI)
            设最小值(Height, 10 * DPI)
            设最大值(Height, Width / 5)
            MyBase.OnPaint(e)
            Dim g As Graphics = e.Graphics, w1 As Integer = Height
            Dim h As Integer = Width - 2 * w1, h2 As Integer = 2.2 * w1
            With g
                绘制基础矩形(g, New Rectangle(0, 0, w1, w1), 按住上, False)
                绘制基础矩形(g, New Rectangle(Width - w1, 0, w1, w1), 按住下, False)
                Dim f As New Font("Segoe UI", 设最大值(0.4 * Height, 20))
                Dim sz As SizeF = .MeasureString("◀", f)
                Dim sw As Integer = (Height - sz.Width) * 0.5
                Dim sh As Integer = (Height - sz.Height) * 0.5
                绘制文本(g, "◀", f, sw, sh, 获取文本状态(Enabled))
                绘制文本(g, "▶", f, Width - w1 + sw, sh, 获取文本状态(Enabled))
                .FillRectangle(滚动绿笔刷, New Rectangle(w1, 0, h, w1))
                If Enabled Then
                    Dim v As Single = (Value - Minimum) / (Maximum - Minimum)
                    h = v * (h - h2 - 4 * DPI) + w1 + 1 * DPI
                    绘制基础矩形(g, New Rectangle(h, 0, h2, w1))
                End If
            End With
        End Sub

    End Class

End Namespace

