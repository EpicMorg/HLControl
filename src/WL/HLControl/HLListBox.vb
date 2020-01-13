Namespace HLControl

    <DefaultEvent("SelectedIndexChanged")>
    Public Class HLListBox
        Inherits HLControlBase

        Private 滚动条 As HLVScrollBar, 物品 As List(Of String), 最高栏 As Integer, 行高 As Integer, 选中 As Integer
        Private 边缘 As Single

        Public Sub New()
            DoubleBuffered = True
            边缘 = 4 * DPI
            物品 = New List(Of String)
            滚动条 = New HLVScrollBar
            最高栏 = 0
            选中 = -1
            NoAllowNoSelectedItem = True
            With 滚动条
                .Width = 25 * DPI
                .Height = Height
                .Top = 0
                .Left = 0
            End With
            Controls.Add(滚动条)
            AddHandler 滚动条.ValueChanged, Sub()
                                             最高栏 = 滚动条.Value
                                             Invalidate()
                                         End Sub
            行高 = Font.GetHeight + 3 * DPI
        End Sub

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> <Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", GetType(UITypeEditor))>
        Public ReadOnly Property Items As List(Of String)
            Get
                Invalidate()
                Return 物品
            End Get
        End Property

        Private Sub _MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
            If ShowScrollBar = False OrElse e.X < 滚动条.Left Then
                Dim h As Integer = e.Y - 边缘
                h = Int(h / 行高) + 最高栏
                Dim old As Integer = 选中
                选中 = IIf(h < 物品.Count, h, -1)
                HandleDontAllow()
                If old <> 选中 Then
                    RaiseEvent SelectedIndexChanged(Me, New HLValueEventArgs(old, 选中))
                    Invalidate()
                End If
            End If
        End Sub

        Public Sub PerformMouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel
            滚动条.PerformMouseWheel(sender, e)
        End Sub

        <Browsable(False)>
        Public Property SelectedIndex As Integer
            Get
                Return 选中
            End Get
            Set(v As Integer)
                If v < 0 Then v = -1
                If v >= 物品.Count Then
                    v = -1
                End If
                If 选中 <> v Then
                    Dim old As Integer = 选中
                    选中 = v
                    HandleDontAllow()
                    滚动条.Value = 选中
                    RaiseEvent SelectedIndexChanged(Me, New HLValueEventArgs(old, 选中))
                    Invalidate()
                End If
            End Set
        End Property

        <Browsable(False)>
        Public Property SelectedItem As String
            Get
                If 选中 > -1 Then
                    Return 物品.Item(选中)
                End If
                Return ""
            End Get
            Set(v As String)
                If 选中 > -1 AndAlso 物品.Item(选中) <> v Then
                    物品.Item(选中) = v
                    Invalidate()
                End If
            End Set
        End Property

        <DefaultValue(1)>
        Public Property SmallChange As Integer
            Get
                Return 滚动条.SmallChange
            End Get
            Set(v As Integer)
                滚动条.SmallChange = v
            End Set
        End Property

        Public Event SelectedIndexChanged(sender As HLListBox, e As HLValueEventArgs)

        <DefaultValue(True)>
        Public Property ShowScrollBar As Boolean
            Get
                Return 滚动条.Visible
            End Get
            Set(v As Boolean)
                If 滚动条.Visible <> v Then
                    滚动条.Visible = v
                    Invalidate()
                End If
            End Set
        End Property

        <DefaultValue(True)>
        Public Property NoAllowNoSelectedItem As Boolean

        Public Function FullHeight() As Integer
            行高 = Font.GetHeight + 3 * DPI
            Return 行高 * 物品.Count + 边缘
        End Function

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            行高 = Font.GetHeight + 3 * DPI
            设最小值(Width, 30 * DPI)
            设最小值(Height, 50 * DPI)
            Dim shown As Integer = Int((Height - 边缘) / 行高 - 0.5)
            Dim f As Integer = 物品.Count - shown, y As Integer
            With 滚动条
                .Left = Width - .Width
                .Height = Height
                .Enabled = f > 0
            End With
            Dim g As Graphics = e.Graphics, c As Rectangle = ClientRectangle
            With g
                绘制基础矩形(g, c)
                If f < 1 Then f = 1
                滚动条.Maximum = f
                y = 边缘
                For i As Integer = 最高栏 To 最高栏 + shown
                    If 物品.Count <= i Then Exit For
                    If 选中 = i Then
                        .FillRectangle(块黄笔刷, New Rectangle(0, y, Width - 边缘, 行高))
                    End If
                    绘制文本(g, 物品(i), Font, 边缘, y, 获取文本状态(Enabled))
                    y += 行高
                Next
            End With
        End Sub

        Private Sub HandleDontAllow()
            If NoAllowNoSelectedItem AndAlso 选中 < 0 AndAlso Items.Count > 0 Then
                选中 = 0
            End If
        End Sub

    End Class

End Namespace
