Imports System.IO
Imports System.IO.File
Imports System.IO.Directory

Public Class DirectoryImage
    Sub txtDirectoryBrowse_Click(sender As Object, e As EventArgs) Handles txtDirectoryBrowse.Click
        If FolderBrowserDialog.ShowDialog = DialogResult.OK Then
            txtDirectoryPath.Text = FolderBrowserDialog.SelectedPath
            grpLinux.Enabled = True
            grpWindows.Enabled = True
            ParseFiles()
        End If
    End Sub
    
    Sub ParseFiles(Optional Directory As String = Nothing)
        If File.Exists(txtDirectoryPath.Text & "\desktop.ini") Then
            btnWindowsOpenDataFile.Enabled = True
        Else
            btnWindowsOpenDataFile.Enabled = False
        End If
        
        If File.Exists(txtDirectoryPath.Text & "\.directory") Then
            btnLinuxOpenDataFile.Enabled = True
        Else
            btnLinuxOpenDataFile.Enabled = False
        End If
    End Sub
    
    Sub optWindowsRel_CheckedChanged(sender As Object, e As EventArgs) Handles optWindowsRel.CheckedChanged
        grpWindowsRel.Enabled = optWindowsRel.Checked
    End Sub
    
    Sub optLinuxRel_CheckedChanged(sender As Object, e As EventArgs) Handles optLinuxRel.CheckedChanged
        grpLinuxRel.Enabled = optLinuxRel.Checked
    End Sub
    
    Sub WindowsOptionSelected() Handles optWindowsAbsolute.CheckedChanged, optWindowsRel.CheckedChanged, optWindowsRelContained.CheckedChanged, optWindowsRelExternal.CheckedChanged
        If optWindowsAbsolute.Checked = True Then
            btnWindowsIconSet.Enabled = True
        ElseIf optWindowsRel.Checked = True Then
            If optWindowsRelContained.Checked = True Then
                btnWindowsIconSet.Enabled = True
            ElseIf optWindowsRelExternal.Checked = True
                btnWindowsIconSet.Enabled = True
            Else
                btnWindowsIconSet.Enabled = False
            End If
        Else
            btnWindowsIconSet.Enabled = False
        End If
    End Sub
    
    Sub LinuxOptionSelected() Handles optLinuxAbsolute.CheckedChanged, optLinuxRel.CheckedChanged, optLinuxRelContained.CheckedChanged, optLinuxRelExternal.CheckedChanged
        If optLinuxAbsolute.Checked = True Then
            btnLinuxIconSet.Enabled = True
        ElseIf optLinuxRel.Checked = True Then
            If optLinuxRelContained.Checked = True Then
                btnLinuxIconSet.Enabled = True
            ElseIf optLinuxRelExternal.Checked = True
                btnLinuxIconSet.Enabled = True
            Else
                btnLinuxIconSet.Enabled = False
            End If
        Else
            btnLinuxIconSet.Enabled = False
        End If
    End Sub
    
    Sub btnWindowsIconSet_Click(sender As Object, e As EventArgs) Handles btnWindowsIconSet.Click
        If OpenFileDialogWindows.InitialDirectory = "" Then
            OpenFileDialogWindows.InitialDirectory = txtDirectoryPath.Text
        End If
        If OpenFileDialogWindows.ShowDialog = DialogResult.OK Then
            If optWindowsAbsolute.Checked = True Then
                txtWindowsIconPath.Text = OpenFileDialogWindows.FileName
            ElseIf optWindowsRelContained.Checked = True Then
                
            ElseIf optWindowsRelExternal.Checked = True Then
                
            Else
                MsgBox("Please select an option!", MsgBoxStyle.Exclamation)
            End If
            imgWindowsCurrent.ImageLocation = OpenFileDialogWindows.FileName
        End If
    End Sub
    
    Sub btnWindowsOpenDataFile_Click(sender As Object, e As EventArgs) Handles btnWindowsOpenDataFile.Click
        If File.Exists(txtDirectoryPath.Text & "\desktop.ini") Then
            Process.Start(Environment.GetEnvironmentVariable("windir") & "\notepad.exe", txtDirectoryPath.Text & "\desktop.ini")
        Else
            btnWindowsOpenDataFile.Enabled = False
        End If
    End Sub
    
    Sub btnLinuxIconSet_Click(sender As Object, e As EventArgs) Handles btnLinuxIconSet.Click
        If OpenFileDialogLinux.InitialDirectory = "" Then
            OpenFileDialogLinux.InitialDirectory = txtDirectoryPath.Text
        End If
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
            imgLinuxCurrent.ImageLocation = OpenFileDialogLinux.FileName
        End If
    End Sub
    
    Sub btnLinuxOpenDataFile_Click(sender As Object, e As EventArgs) Handles btnLinuxOpenDataFile.Click
        If File.Exists(txtDirectoryPath.Text & "\.directory") Then
            Process.Start(Environment.GetEnvironmentVariable("windir") & "\notepad.exe", txtDirectoryPath.Text & "\.directory")
        Else
            btnLinuxOpenDataFile.Enabled = False
        End If
    End Sub
End Class
