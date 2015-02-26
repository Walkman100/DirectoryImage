Public Class DirectoryImage
    Sub txtDirectoryBrowse_Click(sender As Object, e As EventArgs) Handles txtDirectoryBrowse.Click
        If FolderBrowserDialog.ShowDialog = DialogResult.OK Then
            txtDirectoryPath.Text = FolderBrowserDialog.SelectedPath
            grpLinux.Enabled = True
            grpWindows.Enabled = True
            ParseFiles()
        End If
    End Sub
    
    Sub ParseFiles(Optional Directors As String = Nothing)
        
    End Sub
    
    Sub optWindowsRel_CheckedChanged(sender As Object, e As EventArgs) Handles optWindowsRel.CheckedChanged
        grpWindowsRel.Enabled = optWindowsRel.Checked
    End Sub
    
    Sub optLinuxRel_CheckedChanged(sender As Object, e As EventArgs) Handles optLinuxRel.CheckedChanged
        grpLinuxRel.Enabled = optLinuxRel.Checked
    End Sub
    
    Sub btnWindowsIconSet_Click(sender As Object, e As EventArgs) Handles btnWindowsIconSet.Click
        If OpenFileDialogWindows.ShowDialog = DialogResult.OK Then
            If optWindowsAbsolute.Checked = True Then
                txtWindowsIconPath.Text = OpenFileDialogWindows.FileName
            ElseIf optWindowsRelContained.Checked = True Then
                
            ElseIf optWindowsRelExternal.Checked = True Then
                
            Else
                MsgBox("Please select an option!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub
    
    Sub btnWindowsOpenDataFile_Click(sender As Object, e As EventArgs) Handles btnWindowsOpenDataFile.Click
        Process.Start(Environment.GetEnvironmentVariable("windir") & "\notepad " & txtDirectoryPath.Text & "\desktop.ini")
    End Sub
    
    Sub btnLinuxIconSet_Click(sender As Object, e As EventArgs) Handles btnLinuxIconSet.Click
        If OpenFileDialogLinux.ShowDialog = DialogResult.OK Then
            If optLinuxAbsolute.Checked = True Then
                txtLinuxImagePath.Text = OpenFileDialogLinux.FileName
            ElseIf optLinuxRelContained.Checked = True Then
                txtLinuxImagePath.Text = "./"
            ElseIf optLinuxRelExternal.Checked = True Then
                txtLinuxImagePath.Text = "../"
            Else
                MsgBox("Please select an option!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub
    
    Sub btnLinuxOpenDataFile_Click(sender As Object, e As EventArgs) Handles btnLinuxOpenDataFile.Click
        Process.Start(Environment.GetEnvironmentVariable("windir") & "\notepad " & txtDirectoryPath.Text & "\.directory")
    End Sub
End Class
