Namespace HLControl

    Public Class HLLabel
        Inherits Label

        Private 高亮 As Boolean, 黯淡 As Boolean, 文本 As String

        Public Sub New()
            DoubleBuffered = True
            高亮 = False
            黯淡 = False
            AutoSize = True
        End Sub

        <DefaultValue(False)>
        Public Property HighLight As Boolean
            Get
                Return 高亮
            End Get
            Set(v As Boolean)
                高亮 = v
                Invalidate()
                If v Then 黯淡 = False
                If v = True AndAlso 非空(Parent) Then
                    For Each i As Control In Parent.Controls
                        If i.GetType = [GetType]() AndAlso i.Name <> Name Then
                            Dim c As HLLabel = i
                            c.HighLight = False
                        End If
                    Next
                End If
            End Set
        End Property

        <DefaultValue(False)>
        Public Property LowLight As Boolean
            Get
                Return 黯淡
            End Get
            Set(v As Boolean)
                If v Then 高亮 = False
                黯淡 = v
                Invalidate()
            End Set
        End Property

        Private Sub HLLabel_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
            Dim c As Color = 内容白
            If HighLight Then c = 内容黄
            If LowLight Then c = 淡色
            If Not Enabled Then
                c = 暗色
            End If
            ForeColor = c
        End Sub

    End Class

End Namespace