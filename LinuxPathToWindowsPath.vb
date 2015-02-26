Public Class LinuxPathToWindowsPath
    Public Sub New()
        Me.InitializeComponent()
    End Sub
    
    Sub LinuxPathToWindowsPath_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtMntLen.Maximum = DirectoryImage.txtLinuxImagePath.Text.Length-1
        txtMntLen_ValueChanged()
    End Sub
    
    Sub txtMntLen_ValueChanged() Handles txtMntLen.ValueChanged
        txtMnt.Text = DirectoryImage.txtLinuxImagePath.Text.Remove(txtMntLen.Value)
    End Sub
    
    Sub cbxWinLetter_SelectedIndexChanged() Handles cbxWinLetter.SelectedIndexChanged
        If cbxWinLetter.SelectedIndex <> -1 Then
            cbxWinLetter.DropDownStyle = ComboBoxStyle.DropDown
            btnSave.Enabled = True
        End If
    End Sub
    
    Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        DirectoryImage.imgLinuxCurrent.ImageLocation = cbxWinLetter.Text & DirectoryImage.txtLinuxImagePath.Text.Remove(0,txtMntLen.Value).Replace("/", "\")
        Me.Close
    End Sub
    
    Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Me.Close
        DirectoryImage.txtDirectoryBrowse_Click()
    End Sub
    
    Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close
    End Sub
End Class
