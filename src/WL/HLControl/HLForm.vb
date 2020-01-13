Namespace HLControl

    <DefaultEvent("Load")> <Designer("System.Windows.Forms.Design.FormDocumentDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", GetType(System.ComponentModel.Design.IRootDesigner))> <DesignerCategory("Form")> <DesignTimeVisible(False)> <InitializationEvent("Load")> <ToolboxItem(False)> <ToolboxItemFilter("System.Windows.Forms.Control.TopLevel")>
    Public Class HLForm
        Inherits Form

        Public Shared ReadOnly SteamIcon As Icon = My.Resources.SteamLogo

        Private 关闭按钮区域 As Rectangle, 最小化按钮区域 As Rectangle
        Private LastX As Integer = -1, LastY As Integer = -1

        Public Sub New()
            DoubleBuffered = True
            _SetDefault()
            Font = New Font("Microsoft Yahei", 12)
            ShowSteamIcon = True
        End Sub

        <Browsable(False)>
        Public Overrides Property AutoScroll As Boolean

        <Browsable(False)>
        Public Overrides Property AutoSize As Boolean

        Private Sub _SetDefault()
            BackColor = 基础绿
            Dim w As Integer = 10 * DPI
            Padding = New Padding(w, w * 5, w, w)
            MinimumSize = New Size(200, 80)
            AutoScaleMode = AutoScaleMode.None
            FormBorderStyle = FormBorderStyle.None
            StartPosition = FormStartPosition.CenterScreen
            MaximizeBox = False
        End Sub

        <DefaultValue(True)>
        Public Property ShowSteamIcon As Boolean

        Private Sub _Load(sender As Object, e As EventArgs) Handles Me.Load
            _SetDefault()
            Dim d As Single = DPI
            If d > 1 Then
                Height *= d
                Width *= d
                For Each i As Control In Controls
                    SetDPI(i)
                Next
                Left = (系统信息.屏幕分辨率.Width - Width) * 0.5
                Top = (系统信息.屏幕分辨率.Height - Height) * 0.5
            End If
        End Sub

        Private Sub _MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
            If 关闭按钮区域.Contains(e.Location) Then
                If Me.IsMdiChild Then
                    Hide()
                Else
                    Close()
                End If
            ElseIf 最小化按钮区域.Contains(e.Location) Then
                WindowState = FormWindowState.Minimized
            End If
            LastX = -1
            LastY = -1
        End Sub

        Private Sub _MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
            If e.Y < 最小化按钮区域.Bottom AndAlso e.X < 最小化按钮区域.X Then
                拖动窗口(Me)
            End If
        End Sub

        Private Sub SetDPI(c As Control)
            Dim d As Single = DPI
            With c
                .Left *= d
                .Top *= d
                .Height *= d
                .Width *= d
                If 是HL控件(c) = False AndAlso c.HasChildren Then
                    For Each i As Control In c.Controls
                        SetDPI(i)
                    Next
                End If
            End With
        End Sub

        Protected Overrides ReadOnly Property CreateParams As CreateParams
            Get
                Dim cp As CreateParams = MyBase.CreateParams
                If 本程序.真的运行中 Then
                    cp.Style = &H20000
                End If
                Return cp
            End Get
        End Property

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            Dim g As Graphics = e.Graphics
            With g
                绘制基础矩形(g, ClientRectangle)
                Dim w As Integer = 24 * DPI, p As Integer = 8 * DPI, y As Integer
                Dim r As New Rectangle(Width - p - w, p * 1.5, w, w)
                关闭按钮区域 = r
                绘制基础矩形(g, r)
                y = 9 * DPI
                Dim ft As New Font(Font.Name, 设最大值(10 * DPI, 13))
                绘制文本(g, Text, ft, 31 * DPI, y, 获取文本状态(Enabled))
                If ShowIcon Then
                    Dim c As Icon
                    If ShowSteamIcon Then
                        c = SteamIcon
                    Else
                        c = Icon
                    End If
                    Dim ih As Integer = 16
                    If ft.GetHeight > 120 Then
                        ih = 128
                    ElseIf ft.GetHeight > 60 Then
                        ih = 64
                    ElseIf ft.GetHeight > 30 Then
                        ih = 32
                    End If
                    .DrawIcon(c, New Rectangle(y, y + 2 * DPI, ih, ih))
                End If
                y = 7 * DPI
                Dim pe As Pen = 按钮灰笔
                .DrawLine(pe, 左上角(r, y), 右下角(r, y))
                .DrawLine(pe, 左下角(r, y), 右上角(r, y))
                r.X -= w + p
                最小化按钮区域 = r
                绘制基础矩形(g, r)
                y = r.Y + w * 0.5
                .DrawLine(pe, 点(r.X + w * 0.3, y), 点(r.X + w * 0.7, y))
            End With
        End Sub

        Private Sub _Move(sender As Object, e As EventArgs) Handles Me.Move
            If Visible = False OrElse WindowState = FormWindowState.Minimized Then Exit Sub
            If Top < 0 Then Top = 0
            Dim g As Integer = 150
            If Right < g Then Left = -Width + g
            g = 系统信息.屏幕分辨率.Width - g
            If Left > g Then Left = g
            g = 系统信息.屏幕分辨率.Height - 50
            If Top > g Then Top = g
        End Sub

    End Class

End Namespace
