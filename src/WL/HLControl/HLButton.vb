Namespace HLControl

    Public Class HLButton
        Inherits HLControlBase

        Private 激活 As Boolean, 按住 As Boolean

        Public Sub New()
            DoubleBuffered = True
            激活 = False
            按住 = False
        End Sub

        Private Sub _TextChanged() Handles Me.TextChanged
            If 包含(Text, vbCr, vbLf) Then Text = 替换(Text, vbCrLf, " ", vbLf, " ", vbCr, " ")
        End Sub

        Private Sub _KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            If e.KeyCode = Keys.Enter Then
                MyBase.OnClick(Nothing)
            End If
        End Sub

        Private Sub _GotFocus() Handles Me.GotFocus
            激活 = True
            Invalidate()
        End Sub

        Private Sub _LostFocus() Handles Me.LostFocus
            激活 = False
            Invalidate()
        End Sub

        Private Sub _MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
            按住 = True
            If 激活 Then
                Invalidate()
            Else
                Focus()
            End If
        End Sub

        Private Sub _MouseUp() Handles Me.MouseUp
            按住 = False
            Invalidate()
        End Sub

        Public Sub PerformClick()
            MyBase.OnClick(Nothing)
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            修正Dock(Me, True, False)
            Height = 10 * DPI + Font.GetHeight
            MyBase.OnPaint(e)
            Dim g As Graphics = e.Graphics
            With g
                绘制基础矩形(g, ClientRectangle, 按住, 激活)
                绘制文本(g, Text, Font, 6 * DPI, 4 * DPI, 获取文本状态(Enabled))
            End With
        End Sub

    End Class

End Namespace