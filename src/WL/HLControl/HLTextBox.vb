Namespace HLControl
    <DefaultEvent("TextChanged")>
    Public Class HLTextBox
        Inherits HLControlBase

        Private 文本框 As TextBox, 滚动条 As Boolean, 竖条 As HLVScrollBar
        Private 边缘 As Single, 滚动条大小 As Integer

        Public Sub New()
            DoubleBuffered = True
            HighLightLabel = Nothing
            边缘 = 3 * DPI
            滚动条 = False
            滚动条大小 = 25 * DPI
            竖条 = New HLVScrollBar
            文本框 = New TextBox
            Controls.Add(竖条)
            Controls.Add(文本框)
            With 竖条
                .Visible = False
                .Top = 0
                .Left = 0
                .Width = 滚动条大小
                .Height = Height
                AddHandler .ValueChanged, Sub(sender As HLVScrollBar, e As HLValueEventArgs)
                                              滚动(文本框, True, e.NewValue - e.OldValue)
                                          End Sub
            End With
            With 文本框
                .BackColor = 内容绿
                .TextAlign = HorizontalAlignment.Left
                .ImeMode = ImeMode
                .BorderStyle = BorderStyle.None
                AddHandler .TextChanged, Sub()
                                             MyBase.OnTextChanged(Nothing)
                                         End Sub
                AddHandler .KeyDown, Sub(sender As Object, e As KeyEventArgs)
                                         MyBase.OnKeyDown(e)
                                         If e.Control Then
                                             Select Case e.KeyCode
                                                 Case Keys.A
                                                     .SelectAll()
                                             End Select
                                         End If
                                         FixScrollPos()
                                     End Sub
                AddHandler .MouseDown, Sub(sender As Object, e As MouseEventArgs)
                                           MyBase.OnMouseDown(e)
                                           If 非空(HighLightLabel) Then HighLightLabel.HighLight = True
                                           FixScrollPos()
                                       End Sub
                AddHandler .MouseWheel, Sub(sender As Object, e As MouseEventArgs)
                                            MyBase.OnMouseWheel(e)
                                            If Multiline Then
                                                竖条.PerformMouseWheel(sender, e)
                                            End If
                                        End Sub
                AddHandler .DragDrop, Sub(sender As Object, e As EventArgs)
                                          MyBase.OnDragDrop(e)
                                      End Sub
                AddHandler .DragEnter, Sub(sender As Object, e As EventArgs)
                                           MyBase.OnDragEnter(e)
                                       End Sub
                AddHandler .DragOver, Sub(sender As Object, e As EventArgs)
                                          MyBase.OnDragOver(e)
                                      End Sub
                AddHandler .DragLeave, Sub(sender As Object, e As EventArgs)
                                           MyBase.OnDragLeave(e)
                                       End Sub
                AddHandler .GiveFeedback, Sub(sender As Object, e As EventArgs)
                                              MyBase.OnGiveFeedback(e)
                                          End Sub
                AddHandler .HandleCreated, Sub(sender As Object, e As EventArgs)
                                               MyBase.OnHandleCreated(e)
                                           End Sub
                AddHandler .HandleDestroyed, Sub(sender As Object, e As EventArgs)
                                                 MyBase.OnHandleDestroyed(e)
                                             End Sub
                AddHandler .DoubleClick, Sub(sender As Object, e As EventArgs)
                                             MyBase.OnDoubleClick(e)
                                         End Sub
                AddHandler .Enter, Sub(sender As Object, e As EventArgs)
                                       MyBase.OnEnter(e)
                                   End Sub
                AddHandler .GotFocus, Sub(sender As Object, e As EventArgs)
                                          MyBase.OnGotFocus(e)
                                      End Sub
                AddHandler .KeyPress, Sub(sender As Object, e As EventArgs)
                                          MyBase.OnKeyPress(e)
                                      End Sub
                AddHandler .KeyUp, Sub(sender As Object, e As EventArgs)
                                       MyBase.OnKeyUp(e)
                                   End Sub
                AddHandler .Leave, Sub(sender As Object, e As EventArgs)
                                       MyBase.OnLeave(e)
                                   End Sub
                AddHandler .Click, Sub(sender As Object, e As EventArgs)
                                       MyBase.OnClick(e)
                                   End Sub
                AddHandler .LostFocus, Sub(sender As Object, e As EventArgs)
                                           MyBase.OnLostFocus(e)
                                       End Sub
                AddHandler .MouseClick, Sub(sender As Object, e As EventArgs)
                                            MyBase.OnMouseClick(e)
                                        End Sub
                AddHandler .MouseDoubleClick, Sub(sender As Object, e As EventArgs)
                                                  MyBase.OnMouseDoubleClick(e)
                                              End Sub
                AddHandler .MouseCaptureChanged, Sub(sender As Object, e As EventArgs)
                                                     MyBase.OnMouseCaptureChanged(e)
                                                 End Sub
                AddHandler .MouseEnter, Sub(sender As Object, e As EventArgs)
                                            MyBase.OnMouseEnter(e)
                                        End Sub
                AddHandler .MouseLeave, Sub(sender As Object, e As EventArgs)
                                            MyBase.OnMouseLeave(e)
                                        End Sub
                AddHandler .MouseHover, Sub(sender As Object, e As EventArgs)
                                            MyBase.OnMouseHover(e)
                                        End Sub
                AddHandler .MouseMove, Sub(sender As Object, e As EventArgs)
                                           MyBase.OnMouseMove(e)
                                       End Sub
                AddHandler .MouseUp, Sub(sender As Object, e As EventArgs)
                                         MyBase.OnMouseUp(e)
                                     End Sub
                AddHandler .Move, Sub(sender As Object, e As EventArgs)
                                      MyBase.OnMove(e)
                                  End Sub
                AddHandler .PreviewKeyDown, Sub(sender As Object, e As EventArgs)
                                                MyBase.OnPreviewKeyDown(e)
                                            End Sub
            End With
        End Sub

        Private Sub FixScrollPos()
            Dim m As Integer = 文本框.SelectionStart
            Dim s As String = 左(文本框.Text, m)
            竖条.ChangeValueWithoutRaiseEvent(正则.检索(s, vbCrLf).Count)
        End Sub

        Private Sub FixSize()
            Dim h As Integer = 边缘 * 2, x1 As Integer = 0, x2 As Integer = 0
            With 竖条
                .Top = 边缘
                .Width = 滚动条大小
                .Left = Width - 边缘 - .Width
                .Height = Height - h
                .Visible = Multiline AndAlso 滚动条
                If .Visible Then
                    x2 = 滚动条大小
                    .Maximum = 获得文本框行数(文本框)
                    .Enabled = .Maximum > 1
                End If
            End With
            With 文本框
                .ForeColor = IIf([ReadOnly], 淡色, 内容白)
                .Font = Font
                Dim i As Single = 边缘
                .Left = 边缘
                .Top = 边缘
                .Width = Width - h - x2
                .Height = Height - h - x1
                .ScrollBars = ScrollBars.None
                .WordWrap = True
            End With
        End Sub

        Public Property HighLightLabel As HLLabel

        <DefaultValue(False)>
        Public Property ScrollBar As Boolean
            Get
                Return 滚动条
            End Get
            Set(v As Boolean)
                If 滚动条 <> v Then
                    滚动条 = v
                    Invalidate()
                End If
            End Set
        End Property

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            修正Dock(Me, True, Multiline)
            If Not Multiline Then Height = 文本框.Height + 6 * DPI
            FixSize()
            MyBase.OnPaint(e)
            With e.Graphics
                绘制基础矩形(e.Graphics, ClientRectangle, True, False, 内容绿)
            End With
        End Sub

#Region "Textbox 复刻内容"

        <Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", GetType(UITypeEditor))> <Localizable(True)>
        Public Overrides Property Text As String
            Get
                Return 文本框.Text
            End Get
            Set(value As String)
                文本框.Text = value
            End Set
        End Property

        <Browsable(False)>
        Public ReadOnly Property TextLength As Integer
            Get
                Return 文本框.TextLength
            End Get
        End Property

        <DefaultValue(False)>
        Public Property AcceptsTab As Boolean
            Get
                Return 文本框.AcceptsTab
            End Get
            Set(v As Boolean)
                文本框.AcceptsTab = v
            End Set
        End Property

        <DefaultValue(True)>
        Public Property HideSelection As Boolean
            Get
                Return 文本框.HideSelection
            End Get
            Set(v As Boolean)
                文本框.HideSelection = v
            End Set
        End Property

        Public Property Lines As String()
            Get
                Return 文本框.Lines
            End Get
            Set(v As String())
                文本框.Lines = v
            End Set
        End Property

        Public Overloads ReadOnly Property Handle As IntPtr
            Get
                Return 文本框.Handle
            End Get
        End Property

        <DefaultValue(32767)>
        Public Overridable Property MaxLength As Integer
            Get
                Return 文本框.MaxLength
            End Get
            Set(v As Integer)
                文本框.MaxLength = v
            End Set
        End Property

        <Browsable(False)>
        Public Property Modified As Boolean
            Get
                Return 文本框.Modified
            End Get
            Set(v As Boolean)
                文本框.Modified = v
            End Set
        End Property

        <DefaultValue(False)>
        Public Overridable Property Multiline As Boolean
            Get
                Return 文本框.Multiline
            End Get
            Set(v As Boolean)
                文本框.Multiline = v
                AcceptsReturn = v
            End Set
        End Property

        <DefaultValue(False)>
        Public Property [ReadOnly] As Boolean
            Get
                Return 文本框.[ReadOnly]
            End Get
            Set(v As Boolean)
                文本框.[ReadOnly] = v
                FixSize()
            End Set
        End Property

        <Browsable(False)>
        Public Overridable Property SelectedText As String
            Get
                Return 文本框.SelectedText
            End Get
            Set(v As String)
                文本框.SelectedText = v
            End Set
        End Property

        <Browsable(False)>
        Public Property SelectionStart As Integer
            Get
                Return 文本框.SelectionStart
            End Get
            Set(v As Integer)
                文本框.SelectionStart = v
            End Set
        End Property

        <Browsable(False)>
        Public Overridable Property SelectionLength As Integer
            Get
                Return 文本框.SelectionLength
            End Get
            Set(v As Integer)
                文本框.SelectionLength = v
            End Set
        End Property

        <DefaultValue(CharacterCasing.Normal)>
        Public Property CharacterCasing As CharacterCasing
            Get
                Return 文本框.CharacterCasing
            End Get
            Set(v As CharacterCasing)
                文本框.CharacterCasing = v
            End Set
        End Property

        <Browsable(True)> <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> <Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", GetType(UITypeEditor))> <EditorBrowsable(EditorBrowsableState.Always)>
        Public Property AutoCompleteCustomSource As AutoCompleteStringCollection
            Get
                Return 文本框.AutoCompleteCustomSource
            End Get
            Set(v As AutoCompleteStringCollection)
                文本框.AutoCompleteCustomSource = v
            End Set
        End Property

        <Browsable(True)> <DefaultValue(AutoCompleteSource.None)> <EditorBrowsable(EditorBrowsableState.Always)>
        Public Property AutoCompleteSource As AutoCompleteSource
            Get
                Return 文本框.AutoCompleteSource
            End Get
            Set(v As AutoCompleteSource)
                文本框.AutoCompleteSource = v
            End Set
        End Property

        <Browsable(True)> <DefaultValue(AutoCompleteMode.None)> <EditorBrowsable(EditorBrowsableState.Always)>
        Public Property AutoCompleteMode As AutoCompleteMode
            Get
                Return 文本框.AutoCompleteMode
            End Get
            Set(v As AutoCompleteMode)
                文本框.AutoCompleteMode = v
            End Set
        End Property

        <DefaultValue(False)> <RefreshProperties(RefreshProperties.Repaint)>
        Public Property UseSystemPasswordChar As Boolean
            Get
                Return 文本框.UseSystemPasswordChar
            End Get
            Set(v As Boolean)
                文本框.UseSystemPasswordChar = v
                文本框.PasswordChar = IIf(v, "*", "")
            End Set
        End Property

        <DefaultValue(False)>
        Public Property AcceptsReturn As Boolean
            Get
                Return 文本框.AcceptsReturn
            End Get
            Set(v As Boolean)
                文本框.AcceptsReturn = v
            End Set
        End Property

        Public Sub Copy()
            文本框.Copy()
        End Sub

        Public Sub Undo()
            文本框.Undo()
        End Sub

        Public Sub AppendText(text As String)
            文本框.AppendText(text)
        End Sub

        Public Sub Clear()
            文本框.Clear()
        End Sub

        Public Sub SelectAll()
            文本框.SelectAll()
        End Sub

        Public Overloads Sub [Select](start As Integer, length As Integer)
            文本框.[Select](start, length)
        End Sub

        Public Sub DeselectAll()
            文本框.DeselectAll()
        End Sub

        Public Sub ScrollToCaret()
            文本框.ScrollToCaret()
        End Sub

        Public Sub ClearUndo()
            文本框.ClearUndo()
        End Sub

        Public Sub Cut()
            文本框.Cut()
        End Sub

        Public Sub Paste()
            文本框.Paste()
        End Sub

#End Region

    End Class

End Namespace