﻿Namespace HLControl

    <DefaultEvent("ValueChanged")>
    Public Class HLTrackBar
        Inherits Control

        Private 值 As Integer, 最大 As Integer, 最小 As Integer, 可触 As Integer, 按住 As Boolean
        Private 边缘 As Single, 上一个值 As Integer

        Public Sub New()
            DoubleBuffered = True
            最大 = 100
            最小 = 0
            值 = 0
            可触 = 0
            按住 = False
            边缘 = 6 * DPI
            上一个值 = 0
        End Sub

        Private Sub _NeedRePaint() Handles Me.SizeChanged, Me.Resize, Me.AutoSizeChanged, Me.TextChanged, Me.FontChanged, Me.EnabledChanged
            Invalidate()
        End Sub

        Private Sub FixValue()
            If 过频(GetHashCode, 0.04) Then Exit Sub
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
            Invalidate()
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
                If v <> 最小 Then
                    最小 = v
                    FixValue()
                End If
            End Set
        End Property

        <DefaultValue(0)>
        Public Property Value As Integer
            Get
                Return 值
            End Get
            Set(v As Integer)
                If v <> 值 Then
                    值 = v
                    If 非空(HighLightLabel) Then HighLightLabel.HighLight = True
                    FixValue()
                End If
            End Set
        End Property

        Public Event ValueChanged(LastValue As Integer, NewValue As Integer)

        Public Property HighLightLabel As HLLabel

        Private Sub _MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
            If e.Button <> MouseButtons.None Then
                If 按住 = False AndAlso e.Y <= Height * 0.6 AndAlso e.X - 可触 < 边缘 * 2.5 Then
                    按住 = True
                End If
                If 按住 Then
                    Dim x As Integer = e.X
                    设最小值(x, 0)
                    设最大值(x, Width - 边缘)
                    Value = (x / (Width - 边缘)) * (Maximum - Minimum)
                End If
            End If
        End Sub

        Private Sub _MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
            Dim x As Integer = e.X
            If x > 0 AndAlso x <= Width - 边缘 Then
                Value = (x / (Width - 边缘)) * (Maximum - Minimum)
            End If
        End Sub

        Private Sub _MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
            If 按住 Then
                按住 = False
                Invalidate()
            End If
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            修正Dock(Me, True, False)
            MyBase.OnPaint(e)
            Height = 30 * DPI
            设最小值(Width, Height)
            Dim g As Graphics = e.Graphics
            With g
                Dim h As Integer = 20 * DPI, h2 As Integer = 边缘, y As Integer = (h - h2) * 0.5
                Dim r As New Rectangle(0, y, Width, h2)
                y += h2 * 2
                绘制基础矩形(g, r, True,, Color.Black)
                Dim x As Integer = 4 * DPI
                Do While x < Width
                    .DrawLine(细线灰笔, 点(x, y), 点(x, y + h2))
                    x += 15 * DPI
                Loop
                If 上一个值 <> Value Then
                    RaiseEvent ValueChanged(上一个值, Value)
                    上一个值 = Value
                End If
                Dim v As Single = (Value - Minimum) / (Maximum - Minimum)
                r = New Rectangle(v * (Width - 边缘 * 1.7), 0, h2 * 1.5, h)
                可触 = r.Left
                绘制基础矩形(g, r,,, 基础绿)
            End With
        End Sub

    End Class

End Namespace
