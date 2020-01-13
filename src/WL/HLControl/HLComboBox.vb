Namespace HLControl

    <DefaultEvent("SelectedIndexChanged")>
    Public Class HLComboBox
        Inherits HLControlBase

        Private 列表 As HLListBox, 原高度 As Integer

        Public Sub New()
            DoubleBuffered = True
            列表 = New HLListBox
            Controls.Add(列表)
            NoAllowNoSelectedItem = True
            With 列表
                .Visible = False
                .Top = 0
                .Width = 1
                .Left = 0
                .ShowScrollBar = True
                AddHandler .Click, Sub()
                                       HideListBox()
                                   End Sub
                AddHandler .SelectedIndexChanged, Sub(sender As HLListBox, e As HLValueEventArgs)
                                                      RaiseEvent SelectedIndexChanged(Me, e)
                                                  End Sub
            End With
            原高度 = Height
        End Sub

        Public Property HighLightLabel As HLLabel

        <DefaultValue(True)>
        Public Property NoAllowNoSelectedItem As Boolean

        <DefaultValue(1)>
        Public Property SmallChange As Integer
            Get
                Return 列表.SmallChange
            End Get
            Set(v As Integer)
                列表.SmallChange = v
            End Set
        End Property

        Private Sub _MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel
            If Enabled AndAlso e.Y < 原高度 Then
                SelectedIndex += IIf(e.Delta < 0, 1, -1)
            End If
        End Sub

        Private Sub ShowListbox()
            If 为空(Parent) OrElse Visible = False OrElse 列表.Visible OrElse 列表.Items.Count < 1 Then Exit Sub
            With 列表
                Dim c As Integer = .Items.Count
                If c < 1 Then Exit Sub
                .Width = Width
                .Top = Height
                .Left = 0
                c = .FullHeight
                设最大值(c, 350 * DPI)
                .Height = c
                .Visible = True
                Height += .Height
                If 非空(HighLightLabel) Then
                    HighLightLabel.HighLight = True
                End If
                .BringToFront()
                BringToFront()
            End With
            Invalidate()
        End Sub

        Private Sub HideListBox()
            With 列表
                If .Visible = False Then Exit Sub
                .Visible = False
                Height -= .Height
            End With
            Invalidate()
        End Sub

        Private Sub _MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
            If 列表.Visible = False Then
                ShowListbox()
            Else
                HideListBox()
            End If
        End Sub

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> <Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", GetType(UITypeEditor))>
        Public ReadOnly Property Items As List(Of String)
            Get
                Return 列表.Items
            End Get
        End Property

        <Browsable(False)>
        Public Property SelectedIndex As Integer
            Get
                Return 列表.SelectedIndex
            End Get
            Set(v As Integer)
                If 列表.SelectedIndex <> v Then
                    列表.SelectedIndex = v
                    Invalidate()
                End If
            End Set
        End Property

        <Browsable(False)>
        Public Property SelectedItem As String
            Get
                Return 列表.SelectedItem
            End Get
            Set(v As String)
                If 列表.SelectedItem <> v Then
                    列表.SelectedItem = v
                    Invalidate()
                End If
            End Set
        End Property

        Public Event SelectedIndexChanged(sender As HLComboBox, e As HLValueEventArgs)

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            修正Dock(Me, True, False)
            MyBase.OnPaint(e)
            If Not 列表.Visible Then
                Height = Font.GetHeight + 6 * DPI
                原高度 = Height
            End If
            If Not Enabled Then
                列表.Visible = False
            End If
            Dim g As Graphics = e.Graphics, c As Rectangle = ClientRectangle
            With g
                绘制基础矩形(g, c, True)
                Dim m As Single = 原高度 * 0.15
                Dim f As New Font("Segoe UI", 设最大值(0.4 * 原高度, 20))
                Dim sz As SizeF = .MeasureString("▼", f)
                Dim sw As Integer = (原高度 - sz.Width) * 0.5
                Dim sh As Integer = (原高度 - sz.Height) * 0.5
                绘制文本(g, "▼", f, Width - 原高度 + sw, sh, 获取文本状态(Enabled))
                m = 3 * DPI
                绘制文本(g, 列表.SelectedItem, Font, m, m, 获取文本状态(Enabled))
            End With
        End Sub

    End Class

End Namespace
