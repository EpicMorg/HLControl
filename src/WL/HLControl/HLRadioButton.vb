Namespace HLControl

    <DefaultEvent("CheckedChanged")>
    Public Class HLRadioButton
        Inherits HLControlBase

        Private 值 As Boolean, 行高 As Integer

        Public Sub New()
            DoubleBuffered = True
            值 = False
        End Sub

        Private Sub _MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
            Checked = True
        End Sub

        <DefaultValue(False)>
        Public Property Checked As Boolean
            Get
                Return 值
            End Get
            Set(v As Boolean)
                If v <> 值 Then
                    If 非空(Parent) Then
                        For Each i As Control In Parent.Controls
                            If i.GetType = [GetType]() AndAlso i.GetHashCode <> GetHashCode() Then
                                Dim r As HLRadioButton = i
                                r.Checked = False
                            End If
                        Next
                    End If
                    Dim old As Integer = 值
                    值 = v
                    RaiseEvent CheckedChanged(Me, New HLValueEventArgs(old, 值))
                    Invalidate()
                End If
            End Set
        End Property

        Public Event CheckedChanged(sender As HLRadioButton, e As HLValueEventArgs)

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            修正Dock(Me, True, False)
            行高 = Font.GetHeight
            Height = 行高
            MyBase.OnPaint(e)
            Dim g As Graphics = e.Graphics, c As Rectangle = ClientRectangle
            With g
                Width = .MeasureString(Text, Font).Width + Height
                Dim h As Single = 行高 * 0.6
                Dim p As Single = (行高 - h) / 2
                .DrawEllipse(边缘灰笔, New RectangleF(p, p, h, h))
                h *= 0.6
                p = (行高 - h) / 2
                If Checked AndAlso Enabled Then .FillEllipse(白色笔刷, New RectangleF(p, p, h, h))
                绘制文本(g, Text, Font, 行高, 0, 获取文本状态(Enabled, Checked))
            End With
        End Sub

    End Class

End Namespace
