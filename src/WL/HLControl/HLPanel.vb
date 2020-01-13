Namespace HLControl

    Public Class HLPanel
        Inherits Panel

        Private 边缘 As UInteger, 滚动条 As HLVScrollBar, 自动滚动 As Boolean

        Public Sub New()
            DoubleBuffered = True
            边缘 = 线宽
            Border = True
            自动滚动 = False
            滚动条 = New HLVScrollBar
            Controls.Add(滚动条)
            With 滚动条
                .Visible = False
                .Left = 0
                .Top = 0
                .Width = 25 * DPI
                .Height = Height
                .SmallChange = 15 * DPI
                .Dock = DockStyle.Right
                AddHandler .ValueChanged, Sub(sender As HLVScrollBar, e As HLValueEventArgs)
                                              Dim c As Integer = e.NewValue - e.OldValue
                                              For Each i As Control In Controls
                                                  If i.GetHashCode <> .GetHashCode Then
                                                      i.Top -= c
                                                  End If
                                              Next
                                          End Sub
            End With
        End Sub

        Private Sub _MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel
            If 滚动条.Visible Then 滚动条.PerformMouseWheel(sender, e)
        End Sub

        Public Sub ResetScroll()
            滚动条.Value = 滚动条.Minimum
        End Sub

        Public Sub PerformScroll(value As Integer)
            滚动条.Value += value
        End Sub

        <DefaultValue(True)>
        Public Property Border As Boolean

        <DefaultValue(False)>
        Public Overloads Property AutoScroll As Boolean
            Get
                Return 自动滚动
            End Get
            Set(v As Boolean)
                If v <> 自动滚动 Then
                    自动滚动 = v
                    Invalidate()
                End If
            End Set
        End Property

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            AutoSize = False
            BorderStyle = BorderStyle.None
            Padding = New Padding(边缘)
            BackColor = 基础绿
            If AutoScroll Then
                Dim ht As Integer = 0, st As Integer = 0, i As Control
                For Each i In Controls
                    If i.GetHashCode <> 滚动条.GetHashCode Then
                        If i.Bottom > ht Then ht = i.Bottom
                        If i.Top < st Then st = i.Top
                    End If
                Next
                ht += -st - Height + 20 * DPI
                With 滚动条
                    .Visible = ht > 0
                    .Minimum = 0
                    .Maximum = ht
                End With
            Else
                滚动条.Visible = False
            End If
            Dim g As Graphics = e.Graphics, c As Rectangle = ClientRectangle
            With g
                c.Height -= 边缘
                c.Width -= 边缘
                If Border Then 绘制基础矩形(g, c,,, 基础绿)
            End With

        End Sub

    End Class

End Namespace

