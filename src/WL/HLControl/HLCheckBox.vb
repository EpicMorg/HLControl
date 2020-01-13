Namespace HLControl

    <DefaultEvent("CheckedChanged")>
    Public Class HLCheckBox
        Inherits HLControlBase

        Private 值 As Boolean, 行高 As Integer

        Public Sub New()
            DoubleBuffered = True
            值 = False
        End Sub

        Private Sub _MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
            反转(Checked)
        End Sub

        <DefaultValue(False)>
        Public Property Checked As Boolean
            Get
                Return 值
            End Get
            Set(v As Boolean)
                If v <> 值 Then
                    值 = v
                    Dim old As Boolean = 值
                    RaiseEvent CheckedChanged(Me, New HLValueEventArgs(值, v))
                    Invalidate()
                End If
            End Set
        End Property

        Public Event CheckedChanged(sender As HLCheckBox, e As HLValueEventArgs)

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            修正Dock(Me, True, False)
            行高 = Font.GetHeight
            Height = 行高 + 2 * DPI
            MyBase.OnPaint(e)
            Dim g As Graphics = e.Graphics, c As Rectangle = ClientRectangle
            With g
                Width = .MeasureString(Text, Font).Width + Height
                Dim h As Single = 行高 * 0.8, p As Single
                p = (行高 - h) / 2 + 2 * DPI
                If Enabled AndAlso Checked Then
                    h -= 4 * DPI
                    Dim pe As New Pen(白色, 3 * DPI), pt As Point = 点(p + h * 0.4, p + h)
                    .DrawLine(pe, 点(p + 2 * DPI, p + h * 0.5), pt)
                    .DrawLine(pe, pt, 点(p + h, p + h * 0.2))
                    h += 4 * DPI
                End If
                绘制圆角矩形(g, 边缘灰笔, New Rectangle(p, p, h, h), 5 * DPI)
                h *= 0.6
                p = (行高 - h) / 2
                绘制文本(g, Text, Font, 行高, 0, 获取文本状态(Enabled, Checked))
            End With
        End Sub

    End Class

End Namespace
