Namespace HLControl

    <DefaultEvent("SelectedIndexChanged")>
    Public Class HLListView
        Inherits HLControlBase

        Private 列 As List(Of HLListViewColumn), 物品 As List(Of HLListViewItem), 最高栏 As Integer, 行高 As Integer, 选中 As Integer
        Private 滚动条 As HLVScrollBar, 边缘 As Integer, 选中列 As Integer, 拖动 As Boolean

        Public Sub New()
            DoubleBuffered = True
            列 = New List(Of HLListViewColumn)
            物品 = New List(Of HLListViewItem)
            滚动条 = New HLVScrollBar
            With 滚动条
                .Left = 0
                .Top = 0
                .Width = 10
                .Height = 10
                Controls.Add(滚动条)
                .Dock = DockStyle.Right
            End With
            最高栏 = 0
            选中 = -1
            拖动 = False
            ShowCount = True
            选中列 = -1
            AddHandler 滚动条.ValueChanged, Sub()
                                             最高栏 = 滚动条.Value
                                             Invalidate()
                                         End Sub
        End Sub

        Private Sub _MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
            If 为空(物品.Count, 列.Count) Then Exit Sub
            Dim old As Integer = 选中
            选中列 = -1
            选中 = -1
            Dim ok As Boolean = False
            If e.X < 滚动条.Left Then
                Dim y As Integer = e.Y, x As Integer = 0, m As Integer = 0
                If y <= 边缘 Then
                    For Each i As HLListViewColumn In 列
                        x += i.Rwidth
                        If (m = 列.Count - 1 AndAlso e.X >= x - i.Rwidth) OrElse (e.X <= x - 边缘 AndAlso e.X >= x - i.Rwidth + 边缘) Then
                            选中列 = m
                            ok = True
                            Exit For
                        End If
                        m += 1
                    Next
                Else
                    Dim h As Integer = y - 边缘
                    h = Int(h / 行高) + 最高栏
                    选中 = IIf(h < 物品.Count, h, -1)
                    ok = True
                End If
            End If
            If ok Then
                If 选中列 >= 0 Then
                    Static d As New Dictionary(Of Integer, Boolean)
                    If Not d.ContainsKey(选中列) Then
                        d.Add(选中列, True)
                    End If
                    物品.Sort(New HLListViewItemSort(选中列, 反转(d.Item(选中列))))
                End If
                RaiseEvent SelectedIndexChanged(Me, New HLValueEventArgs(old, 选中))
                Invalidate()
            End If
        End Sub

        Private Sub _MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
            选中列 = -1
            拖动 = False
            Cursor = Cursors.Default
            Invalidate()
        End Sub

        Private Sub _MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel
            滚动条.PerformMouseWheel(sender, e)
        End Sub

        Private Sub _MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
            If e.Y > 边缘 Then Exit Sub
            Dim x As Integer = 0, dg As Boolean = False
            For Each i As HLListViewColumn In 列
                If Math.Abs(e.X - (x + i.Rwidth)) <= 10 * DPI Then
                    dg = True
                    If e.Button <> MouseButtons.None Then
                        拖动 = True
                        i.Width = 设最大值((e.X - x) / DPI, Width - 边缘 - 30 * DPI - x)
                        Invalidate()
                    End If
                    Exit For
                End If
                x += i.Rwidth
            Next
            Cursor = IIf(dg, Cursors.SizeWE, Cursors.Default)
        End Sub

        <Browsable(False)>
        Public ReadOnly Property Columns As List(Of HLListViewColumn)
            Get
                Invalidate()
                Return 列
            End Get
        End Property

        <Browsable(False)>
        Public ReadOnly Property Items As List(Of HLListViewItem)
            Get
                Invalidate()
                Return 物品
            End Get
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

        Public Sub AddColumn(name As String)
            Columns.Add(New HLListViewColumn(name))
        End Sub

        Public Sub AddColumn(name As String, width As UInteger)
            Columns.Add(New HLListViewColumn(name, width))
        End Sub

        Public Sub AddItem(title As String, ParamArray str() As String)
            Items.Add(New HLListViewItem(title, str))
        End Sub

        <Browsable(False)>
        Public Property SelectedItem As HLListViewItem
            Get
                If 选中 >= 物品.Count Then 选中 = -1
                If 选中 < 0 Then Return Nothing
                Return 物品.Item(选中)
            End Get
            Set(v As HLListViewItem)
                If 选中 >= 物品.Count Then 选中 = -1
                If 选中 >= 0 Then
                    物品.Item(选中) = v
                    Invalidate()
                End If
            End Set
        End Property

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
                    滚动条.Value = v
                    RaiseEvent SelectedIndexChanged(Me, New HLValueEventArgs(old, 选中))
                    Invalidate()
                End If
            End Set
        End Property

        <DefaultValue(True)>
        Public Property ShowCount As Boolean

        Public Event SelectedIndexChanged(sender As HLListView, e As HLValueEventArgs)

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            行高 = Font.GetHeight + 3 * DPI
            边缘 = 行高 + 3 * DPI
            设最小值(Width, 30 * DPI)
            设最小值(Height, 50 * DPI)
            If 选中 >= 物品.Count Then 选中 = -1
            Dim g As Graphics = e.Graphics, c As Rectangle = ClientRectangle
            Dim x As Integer = 0, y As Integer = 0, r As Rectangle, b As Boolean
            Dim shown As Integer = Int((Height - 边缘 * 2) / 行高 - 0.5), p As Integer = 2 * DPI
            滚动条.Width = 边缘
            滚动条.Maximum = 设最小值(物品.Count - shown, 3)
            With g
                绘制基础矩形(g, c,,, 内容绿)
                For Each i As HLListViewColumn In 列
                    b = y = 列.Count - 1
                    r = New Rectangle(x, 0, IIf(b, Width - x, i.Rwidth), 边缘 - p)
                    绘制基础矩形(g, r, 选中列 = y)
                    Dim st As String = i.Name.Trim
                    If y = 0 AndAlso ShowCount Then
                        st += " (" + 物品.Count.ToString + ")"
                    End If
                    绘制文本(g, st, Font, x + p, p, 获取文本状态(Enabled))
                    y += 1
                    x += i.Rwidth
                    If x >= 滚动条.Left Then Exit For
                Next
                y = 边缘
                For i As Integer = 最高栏 To 最高栏 + shown + 1
                    If 物品.Count <= i Then Exit For
                    Dim firstC As Boolean = True, t As HLListViewItem = 物品(i), it As Integer = 0
                    x = 0
                    If 选中 = i Then
                        .FillRectangle(块黄笔刷, New Rectangle(0, y, Width, 行高))
                    End If
                    For Each i2 As HLListViewColumn In 列
                        If Not firstC Then Call .FillRectangle(IIf(选中 = i, 块黄笔刷, 内容绿笔刷), New Rectangle(x - 2 * p, y, Width, 行高))
                        Dim st As String = ""
                        If firstC Then
                            st = t.Title
                        Else
                            st = t.Items.Item(it)
                        End If
                        绘制文本(g, st, Font, x + p, y, 获取文本状态(Enabled))
                        If Not firstC Then it += 1
                        If it >= t.Items.Count Then Exit For
                        firstC = False
                        x += i2.Width * DPI
                    Next
                    y += 行高
                Next
            End With
        End Sub

    End Class

    Public Class HLListViewColumn

        Private n As String, w As Integer

        Public Sub New(name As String, width As UInteger)
            With Me
                .Name = name
                .Width = width
            End With
        End Sub

        Public Sub New(name As String)
            With Me
                .Name = name
                .Width = name.Length * 25
            End With
        End Sub

        Public Property Name As String
            Get
                Return n
            End Get
            Set(v As String)
                n = 去除(v, vbCr, vbLf)
            End Set
        End Property

        Public Property Width As UInteger
            Get
                Return w
            End Get
            Set(v As UInteger)
                w = 设最小值(v, 20)
            End Set
        End Property

        Public ReadOnly Property Rwidth As UInteger
            Get
                Return w * DPI
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return Name
        End Function

        Public Shared Widening Operator CType(s As String) As HLListViewColumn
            Return New HLListViewColumn(s)
        End Operator

        Public Shared Widening Operator CType(s As HLListViewColumn) As String
            Return s.Name
        End Operator

        Public Shared Operator =(a As HLListViewColumn, b As HLListViewColumn) As Boolean
            Return a.Name = b.Name
        End Operator

        Public Shared Operator <>(a As HLListViewColumn, b As HLListViewColumn) As Boolean
            Return a.Name <> b.Name
        End Operator

        Public Overrides Function Equals(obj As Object) As Boolean
            If obj.GetType = Me.GetType Then
                Return Name = obj.Name
            End If
            Return False
        End Function

    End Class

    Public Class HLListViewItem

        Public Sub New(Title As String, ParamArray str() As String)
            With Me
                .Title = 去除(Title, vbCr, vbLf)
                .Items = New List(Of String)
                .Items.AddRange(str)
            End With
        End Sub

        Public Property Items As List(Of String)

        Public Property Title As String

        Public Shared Widening Operator CType(s As String) As HLListViewItem
            Return New HLListViewItem(s)
        End Operator

        Public Shared Widening Operator CType(s As HLListViewItem) As String
            Return s.Title
        End Operator

        Public Overrides Function ToString() As String
            Return Title
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            If obj.GetType = [GetType]() Then
                Dim o As HLListViewItem = obj
                If o.Title = Title AndAlso o.Items.Count = Items.Count Then
                    Dim ok As Boolean = True
                    For Each i As String In Items
                        If Not o.Items.Contains(i) Then
                            ok = False
                        End If
                    Next
                    If ok Then Return True
                End If
            End If
            Return False
        End Function

    End Class

    Friend Class HLListViewItemSort
        Implements IComparer(Of HLListViewItem)

        Private L As UInteger, op As Boolean

        Public Sub New(L As UInteger, 反向 As Boolean)
            Me.L = L
            op = 反向
        End Sub

        Public Function Compare(a As HLListViewItem, b As HLListViewItem) As Integer Implements IComparer(Of HLListViewItem).Compare
            Dim s1 As String = "", s2 As String = ""
            If L = 0 Then
                s1 = a.Title
                s2 = b.Title
            Else
                L -= 1
                If a.Items.Count > L Then
                    s1 = a.Items.Item(L)
                End If
                If b.Items.Count > L Then
                    s2 = b.Items.Item(L)
                End If
                L += 1
            End If
            Dim g As Integer = 比较文本(s1, s2)
            If op Then g = -g
            Return g
        End Function

    End Class

End Namespace

