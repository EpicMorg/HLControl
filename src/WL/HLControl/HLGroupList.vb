Namespace HLControl

    <DefaultEvent("SelectedIndexChanged")>
    Public Class HLGroupList
        Inherits HLControlBase

        Private 滚动条 As HLVScrollBar, 物品 As List(Of HLGroup), 最高栏 As Integer, 行高 As Integer, 选中 As Integer
        Private 边缘 As Single, 展示图标 As Boolean

        Public Sub New()
            DoubleBuffered = True
            物品 = New List(Of HLGroup)
            滚动条 = New HLVScrollBar
            最高栏 = 0
            选中 = -1
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
            边缘 = 5 * DPI
            展示图标 = True
        End Sub

        Public Sub PerformMouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel
            滚动条.PerformMouseWheel(sender, e)
        End Sub

        Private Sub _MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
            If ShowScrollBar = False OrElse e.X < 滚动条.Left Then
                Dim h As Integer = e.Y - 边缘
                h = Int(h / 行高) + 最高栏
                Dim old As Integer = 选中
                选中 = IIf(h < 总物品数(), h, -1)
                If 选中 <> old Then
                    RaiseEvent SelectedIndexChanged(Me, New HLValueEventArgs(转外部序号(old), 转外部序号(选中)))
                    Invalidate()
                End If
            End If
        End Sub

        <DefaultValue(1)>
        Public Property SmallChange As Integer
            Get
                Return 滚动条.SmallChange
            End Get
            Set(v As Integer)
                滚动条.SmallChange = v
            End Set
        End Property

        Public Event SelectedIndexChanged(sender As HLGroupList, e As HLValueEventArgs)

        <Browsable(False)>
        Public ReadOnly Property Groups As List(Of HLGroup)
            Get
                Invalidate()
                Return 物品
            End Get
        End Property

        Public Function GetGroup(title As String) As HLGroup
            For Each i As HLGroup In 物品
                If i.Title.ToLower = title.ToLower Then
                    Invalidate()
                    Return i
                End If
            Next
            Return Nothing
        End Function

        <DefaultValue(True)>
        Public Property ShowIcon As Boolean
            Get
                Return 展示图标
            End Get
            Set(v As Boolean)
                If 展示图标 <> v Then
                    展示图标 = v
                    Invalidate()
                End If
            End Set
        End Property

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

        <Browsable(False)>
        Public Property SelectedIndex As Integer
            Get
                Return 转外部序号(选中)
            End Get
            Set(v As Integer)
                Dim c As Integer = 转内部序号(v)
                If 选中 <> c Then
                    Dim old As Integer = 选中
                    If c = -1 Then v = -1
                    滚动条.Value = c
                    选中 = c
                    RaiseEvent SelectedIndexChanged(Me, New HLValueEventArgs(转外部序号(old), 选中))
                    Invalidate()
                End If
            End Set
        End Property

        <Browsable(False)>
        Public Property SelectedItem As HLGroupItem
            Get
                Dim o As Object = 序号物品(选中)
                If 非空(o) AndAlso o.GetType = GetType(HLGroupItem) Then
                    Return o
                End If
                Return Nothing
            End Get
            Set(v As HLGroupItem)
                If 非空(v) AndAlso 选中 > 0 Then
                    Dim g As Integer = -1, i As HLGroup = Nothing, g2 As Integer = 0, ok As Boolean = False
                    For Each i In 物品
                        g += 1
                        g2 = -1
                        For Each i2 As HLGroupItem In i.Items
                            g += 1
                            g2 += 1
                            If g = 选中 Then
                                ok = True
                                Exit For
                            End If
                        Next
                        If ok Then Exit For
                    Next
                    If ok Then
                        输出(g2)
                        i.Items(g2) = v
                        Invalidate()
                    End If
                End If
            End Set
        End Property

        Private Function 转外部序号(c As Integer) As Integer
            If c < 0 Then Return -1
            Dim c2 As Integer = -1, c3 As Integer = -1
            For Each i As HLGroup In 物品
                c2 += 1
                If c2 = c Then Return -1
                For Each i2 As HLGroupItem In i.Items
                    c2 += 1
                    c3 += 1
                    If c2 = c Then Return c3
                Next
            Next
            Return -1
        End Function

        Private Function 转内部序号(c As Integer) As Integer
            If c < 0 Then Return -1
            Dim c2 As Integer = -1, c3 As Integer = -1
            For Each i As HLGroup In 物品
                c2 += 1
                For Each i2 As HLGroupItem In i.Items
                    c2 += 1
                    c3 += 1
                    If c3 = c Then Return c2
                Next
            Next
            Return -1
        End Function

        Private Function 序号物品(v As Integer) As Object
            If v < 0 Then Return Nothing
            Dim g As Integer = 0
            For Each i As HLGroup In 物品
                If g = v Then Return i
                For Each i2 As HLGroupItem In i.Items
                    g += 1
                    If g = v Then Return i2
                Next
                g += 1
            Next
            Return Nothing
        End Function

        Private Function 总物品数() As Integer
            Dim c As Integer = 0
            For Each i As HLGroup In 物品
                c += 1 + i.Items.Count
            Next
            滚动条.Maximum = IIf(c > 0, c, 1)
            Return c
        End Function

        Public Sub SortAll(Optional desc As Boolean = False)
            Groups.Sort(New HLGroupComparer(desc))
            For Each i As HLGroup In Groups
                i.Sort(desc)
            Next
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            行高 = Font.GetHeight + 3 * DPI
            设最小值(Width, 30 * DPI)
            设最小值(Height, 50 * DPI)
            Dim shown As Integer = Int((Height - 边缘) / 行高 - 0.5)
            Dim f As Integer = 总物品数() - shown, y As Integer
            With 滚动条
                .Left = Width - .Width
                .Height = Height
                .Enabled = f > 0
            End With
            Dim g As Graphics = e.Graphics, c As Rectangle = ClientRectangle
            Dim iw As Integer = Font.GetHeight
            Dim ft As New Font(Font.Name, Font.Size * 0.8)
            With g
                绘制基础矩形(g, c, True, False, 内容绿)
                If f < 1 Then f = 1
                滚动条.Maximum = f
                y = 边缘
                Dim x As Integer = 边缘
                If ShowIcon Then x += iw
                For i As Integer = 最高栏 To 最高栏 + shown
                    Dim o As Object = 序号物品(i)
                    If 为空(o) Then Exit For
                    Dim hl As Boolean = o.GetType = GetType(HLGroup)
                    If 选中 = i AndAlso Not hl Then
                        .FillRectangle(块黄笔刷, New Rectangle(0, y, Width - 边缘, 行高))
                    End If
                    绘制文本(g, IIf(hl, o.ToString.ToUpper, o.ToString), IIf(hl, ft, Font), x, IIf(hl, y + iw * 0.2, y), 获取文本状态(Enabled, hl))
                    If ShowIcon AndAlso Not hl Then
                        Dim gi As HLGroupItem = o
                        If gi.HasIco Then

                            .DrawIcon(gi.Ico, New Rectangle(边缘, y + iw * 0.1, iw * 0.8, iw * 0.8))
                        End If
                    ElseIf hl Then
                        .DrawLine(黑边框, 点(边缘, y + 行高), 点(IIf(滚动条.Visible, 滚动条.Left, Width) - 边缘, y + 行高))
                    End If
                    y += 行高
                Next
            End With
        End Sub

    End Class

    Public Class HLGroup

        Public Sub New(title As String, ParamArray str() As String)
            Me.Title = title
            Items = New List(Of HLGroupItem)
            For Each i As String In str
                Items.AddRange(New HLGroupItem(i))
            Next
        End Sub

        Public Property Title As String

        Public Property Items As List(Of HLGroupItem)

        Public Function GetItem(title As String) As HLGroupItem
            For Each i As HLGroupItem In Items
                If i.Title.ToLower = title.ToLower Then
                    Return i
                End If
            Next
            Return Nothing
        End Function

        Public Shared Widening Operator CType(m As String) As HLGroup
            Return New HLGroup(m)
        End Operator

        Public Shared Widening Operator CType(m As HLGroup) As String
            Return m.Title
        End Operator

        Public Overrides Function ToString() As String
            Return Title
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            If obj.GetType = Me.GetType Then
                Return Title = obj.title
            End If
            Return False
        End Function

        Public Sub Sort(Optional desc As Boolean = False)
            Items.Sort(New HLGroupItemComparer(desc))
        End Sub

    End Class

    Friend Class HLGroupComparer
        Implements IComparer(Of HLGroup)

        Private desc As Boolean

        Public Sub New(desc As Boolean)
            Me.desc = desc
        End Sub

        Public Function Compare(x As HLGroup, y As HLGroup) As Integer Implements IComparer(Of HLGroup).Compare
            Dim g As Integer = 比较文本(x.Title, x.Title)
            If desc Then g = -g
            Return g
        End Function

    End Class

    Friend Class HLGroupItemComparer
        Implements IComparer(Of HLGroupItem)

        Private desc As Boolean

        Public Sub New(desc As Boolean)
            Me.desc = desc
        End Sub

        Public Function Compare(x As HLGroupItem, y As HLGroupItem) As Integer Implements IComparer(Of HLGroupItem).Compare
            Dim g As Integer = 比较文本(x.Title, x.Title)
            If desc Then g = -g
            Return g
        End Function

    End Class

    Public Class HLGroupItem

        Public Sub New(title As String, Optional ico As Icon = Nothing)
            Me.Title = title
            Me.Ico = ico
        End Sub

        Public Property Title As String

        Public Property Ico As Icon

        Public ReadOnly Property HasIco As Boolean
            Get
                Return 非空(Ico)
            End Get
        End Property

        Public Shared Widening Operator CType(m As String) As HLGroupItem
            Return New HLGroupItem(m)
        End Operator

        Public Shared Widening Operator CType(m As HLGroupItem) As String
            Return m.Title
        End Operator

        Public Overrides Function ToString() As String
            Return Title
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            If obj.GetType = Me.GetType Then
                Return Title = obj.title
            End If
            Return False
        End Function

    End Class

End Namespace
