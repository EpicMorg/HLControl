Imports WL.基础

Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim i As Integer
        With HlListView1.Columns
            .Add(New HLListViewColumn("Server Name", 450))
            .Add(New HLListViewColumn("Map", 150))
            .Add(New HLListViewColumn("Players", 100))
            .Add(New HLListViewColumn("Ping", 100))
        End With
        For i = 100 To 200
            HlComboBox1.Items.Add(i.ToString + "cm")
            HlListBox1.Items.Add(随机.西文(5))
            HlListView1.Items.Add(New HLListViewItem(i.ToString + "  " + 随机.西文(随机.整数(0, 6)), 随机.当中一个("de_dust", "cs_" + 随机.西文(随机.整数(2, 16)), "cs_office"), 随机.整数(1, 32) & "/32", 随机.整数(29, 232).ToString))
            Dim g As New HLGroup(i.ToString + "  " + 随机.西文(5))
            For i2 As Integer = 1 To 15
                g.Items.Add(New HLGroupItem(随机.小数().ToString, 随机.当中一个(SystemIcons.Application, SystemIcons.Error, SystemIcons.Information, Nothing)))
            Next
            HlGroupList1.Groups.Add(g)
            HlGroupList1.SortAll()
        Next
        HlListBox1.SelectedIndex = 0
        HlComboBox1.SelectedIndex = 0
        HlLabel8.Text += (系统信息.DPI * 100) & "%"
    End Sub

    Private Sub HlButton1_Click(sender As Object, e As EventArgs) Handles HlButton1.Click
        反转(HlTextBox2.ScrollBar)
    End Sub

    Private Sub HlButton2_Click(sender As Object, e As EventArgs) Handles HlButton2.Click
        For Each i As Control In HlButton2.Parent.Controls
            If i.Name <> HlButton2.Name Then
                反转(i.Enabled)
            End If
        Next
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        HlProgressBar1.Value += 1
    End Sub

    Private Sub HlButton3_Click(sender As Object, e As EventArgs) Handles HlButton3.Click
        反转(Timer1.Enabled)
    End Sub

    Private Sub HlTrackBar1_ValueChanged() Handles HlTrackBar1.ValueChanged
        HlLabel6.Text = HlTrackBar1.Value.ToString
    End Sub

    Private Sub HlCheckBox7_CheckedChanged() Handles HlCheckBox7.CheckedChanged
        HlProgressBar1.AutoReset = HlCheckBox7.Checked
    End Sub

    Private Sub HlCheckBox8_CheckedChanged() Handles HlCheckBox8.CheckedChanged
        ShowSteamIcon = HlCheckBox8.Checked
        Refresh()
    End Sub

    Private Sub HlTrackBar2_ValueChanged(sender As HLTrackBar, e As HLValueEventArgs) Handles HlTrackBar2.ValueChanged
        HlLabel7.Text = HlTrackBar2.Value.ToString
        With HlProgressBar2
            .Maximum = HlTrackBar2.Maximum
            .Minimum = HlTrackBar2.Minimum
            .Value = HlTrackBar2.Value
        End With
    End Sub

    Private Sub HlButton4_Click(sender As Object, e As EventArgs) Handles HlButton4.Click
        Form2.Show()
    End Sub

End Class
