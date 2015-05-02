Public Class LinuxPathToWindowsDrive
    Public Sub New()
        Me.InitializeComponent()
    End Sub
    
    Sub LinuxPathToWindowsPath_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtMntLen.Maximum = DirectoryImage.txtwindowsiconpath.Text.Length-1
        txtMntLen_ValueChanged()
    End Sub
    
    Sub txtMntLen_ValueChanged() Handles txtMntLen.ValueChanged
        txtMnt.Text = DirectoryImage.txtwindowsiconpath.Text.Remove(txtMntLen.Value)
    End Sub
    
    Sub cbxWinLetter_SelectedIndexChanged() Handles cbxWinLetter.SelectedIndexChanged
        If cbxWinLetter.SelectedIndex <> -1 Then
            cbxWinLetter.DropDownStyle = ComboBoxStyle.DropDown
            btnSave.Enabled = True
        End If
    End Sub
    'txtwindowsiconpath.text = inputbox("Please enter the Windows drive letter where the path in linux ""/media/"&Environment.GetEnvironmentVariable("UserName")&"/MountPath"" is mounted:", _
    '                                                   "Windows Drive Letter") & txtWindowsIconPath.Text
    Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        DirectoryImage.txtwindowsiconpath.text = cbxWinLetter.Text & DirectoryImage.txtwindowsiconpath.Text.Remove(0,txtMntLen.Value).Replace("/", "\")
        Me.Close
    End Sub
    
    Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close
    End Sub
End Class
