Public Class LinuxPathToWindowsPath
    Public Sub New()
        Me.InitializeComponent()
    End Sub
    
    Sub LinuxPathToWindowsPath_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtMntLen.Maximum = DirectoryImage.txtLinuxImagePath.Text.Length
        txtMntLen_ValueChanged()
    End Sub
    
    Sub txtMntLen_ValueChanged() Handles txtMntLen.ValueChanged
        txtMnt.Text = DirectoryImage.txtLinuxImagePath.Text.Remove(txtMntLen.Value)
    End Sub
    
    Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Me.Close
        DirectoryImage.imgLinuxCurrent.ImageLocation = cbxWinLetter.SelectedItem & DirectoryImage.txtLinuxImagePath.Text.Remove(0,txtMntLen.Value).Replace("/", "\")
    End Sub
    
    Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Me.Close
        DirectoryImage.txtDirectoryBrowse_Click()
    End Sub
    
    Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close
    End Sub
End Class
