Imports WL.基础

Public Class Form2

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        Static l As Integer = 0, l2 As Integer = 0
        Dim lb As HLLabel = TabPage1.Controls(l)
        lb.HighLight = True
        Dim o As Object
        For Each o In TabPage2.Controls
            o.value = 随机.整数(100, 0)
        Next
        For Each o In HlPanel1.Controls
            If o.GetType <> GetType(HLVScrollBar) Then o.SelectedIndex = 随机.整数(490, 0)
        Next
        l += 1
        l2 += 1
        If l >= TabPage1.Controls.Count Then l = 0
        If l2 >= TabPage2.Controls.Count Then l2 = 0
        HlPanel1.PerformScroll(随机.整数(30, -30))
        Timer1.Enabled = True
    End Sub

    Private Sub HlTrackBar6_ValueChanged(sender As HLTrackBar, e As HLValueEventArgs) Handles HlTrackBar6.ValueChanged
        HlTrackBar6.HighLightLabel.Text = "FPS:" & e.NewValue
        Timer1.Interval = 1000 / e.NewValue
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each i As Object In HlPanel1.Controls
            Dim t As Type = i.GetType
            If t <> GetType(HLVScrollBar) Then
                If t = GetType(HLGroupList) Then
                    For n As Integer = 1 To 50
                        Dim l As HLGroupList = i
                        Dim g As New HLGroup(随机.繁体汉字)
                        For p As Integer = 0 To 10
                            g.Items.Add(New HLGroupItem(p.ToString + "__" + 随机.西文, 随机.当中一个(SystemIcons.Application, SystemIcons.Error, SystemIcons.Information, Nothing)))
                        Next
                        l.Groups.Add(g)
                    Next
                Else
                    If t = GetType(HLListView) Then
                        i.Columns.add("ffwwwwwww")
                    End If
                    For n As Integer = 1 To 500
                        i.items.add(随机.西文(8))
                    Next
                End If
            End If
        Next

    End Sub

End Class