Namespace HLControl

    Public MustInherit Class HLControlBase
        Inherits Control

        Public Sub New()
            DoubleBuffered = True
        End Sub

        Private Sub NeedRePaint() Handles Me.SizeChanged, Me.Resize, Me.AutoSizeChanged, Me.FontChanged, Me.EnabledChanged, Me.TextChanged, MyBase.TextChanged
            Invalidate()
        End Sub

    End Class

End Namespace
