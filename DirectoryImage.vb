Imports System.IO
Imports System.IO.File
Imports System.IO.Directory

Public Class DirectoryImage
    Dim ComponentResourcesManagerFromDesigner As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DirectoryImage))
    
    Sub LoadDirectoryImage() Handles MyBase.Load
        For Each s As String In My.Application.CommandLineArgs
            txtDirectoryPath.Text = s
            grpLinux.Enabled = True
            grpWindows.Enabled = True
            ParseFiles(txtDirectoryPath.Text)
        Next
    End Sub
    
    Sub txtDirectoryBrowse_Click(sender As Object, e As EventArgs) Handles txtDirectoryBrowse.Click
        If FolderBrowserDialog.ShowDialog = DialogResult.OK Then
            txtDirectoryPath.Text = FolderBrowserDialog.SelectedPath
            
            'GUI elements
            grpLinux.Enabled = True
            grpWindows.Enabled = True
            txtWindowsIconPath.Text = ""
            optWindowsAbsolute.Checked = False
            optWindowsRel.Checked = False
            optWindowsRelContained.Checked = False
            optWindowsRelExternal.Checked = False
            imgWindowsCurrent.Image = CType(ComponentResourcesManagerFromDesigner.GetObject("imgWindowsCurrent.Image"),System.Drawing.Image)
            txtLinuxImagePath.Text = ""
            optLinuxAbsolute.Checked = False
            optLinuxRel.Checked = False
            optLinuxRelContained.Checked = False
            optLinuxRelExternal.Checked = False
            'Me.imgLinuxCurrent.Image = CType(ComponentResourcesManagerFromDesigner.GetObject("imgLinuxCurrent.Image"),System.Drawing.Image)
            
            ParseFiles(txtDirectoryPath.Text)
        End If
    End Sub
    
    Dim alreadyGotIcon, lookingForIconIndex As Boolean
    
    Sub ParseFiles(Directory As String)
        If File.Exists(Directory & "\desktop.ini") Then
            btnWindowsOpenDataFile.Enabled = True
            alreadyGotIcon = False
            lookingForIconIndex = False
            For Each line In ReadLines(Directory & "\desktop.ini")
                If line.StartsWith("IconResource=", True, Nothing) Then
                    txtWindowsIconPath.Text = line.Remove(0, 13)
                    alreadyGotIcon = True
                ElseIf line.StartsWith("IconFile=", True, Nothing) And alreadyGotIcon = False Then
                    txtWindowsIconPath.Text = line.Remove(0, 9)
                    lookingForIconIndex = True
                ElseIf line.StartsWith("IconIndex=", True, Nothing) And lookingForIconIndex Then
                    txtWindowsIconPath.Text = txtWindowsIconPath.Text & "," & line.Remove(0, 10)
                End If
            Next
            ParseWindows()
        Else
            btnWindowsOpenDataFile.Enabled = False
        End If
        If File.Exists(Directory & "\.directory") Then
            btnLinuxOpenDataFile.Enabled = True
            For Each line In ReadLines(Directory & "\.directory")
                If line.StartsWith("Icon=", True, Nothing) Then
                    txtLinuxImagePath.Text = line.remove(0, 5)
                End If
            Next
            ParseLinux()
        Else
            btnLinuxOpenDataFile.Enabled = False
        End If
    End Sub
    
    Sub ParseWindows
        If txtWindowsIconPath.Text.StartsWith("%", True, Nothing) Or _
                txtWindowsIconPath.Text.StartsWith("A:\") Or _
                txtWindowsIconPath.Text.StartsWith("B:\") Or _
                txtWindowsIconPath.Text.StartsWith("C:\") Or _
                txtWindowsIconPath.Text.StartsWith("D:\") Or _
                txtWindowsIconPath.Text.StartsWith("E:\") Or _
                txtWindowsIconPath.Text.StartsWith("F:\") Or _
                txtWindowsIconPath.Text.StartsWith("G:\") Or _
                txtWindowsIconPath.Text.StartsWith("H:\") Or _
                txtWindowsIconPath.Text.StartsWith("I:\") Or _
                txtWindowsIconPath.Text.StartsWith("J:\") Or _
                txtWindowsIconPath.Text.StartsWith("K:\") Or _
                txtWindowsIconPath.Text.StartsWith("L:\") Or _
                txtWindowsIconPath.Text.StartsWith("M:\") Or _
                txtWindowsIconPath.Text.StartsWith("N:\") Or _
                txtWindowsIconPath.Text.StartsWith("O:\") Or _
                txtWindowsIconPath.Text.StartsWith("P:\") Then
            optWindowsAbsolute.Checked = True
        ElseIf txtWindowsIconPath.Text.StartsWith("..\", True, Nothing) Then
            optWindowsRel.Checked = True
            optWindowsRelExternal.Checked = True
        Else
            optWindowsRel.Checked = True
            optWindowsRelContained.Checked = True
        End If
    End Sub
    
    Sub ParseLinux
        If txtLinuxImagePath.Text.StartsWith("../", True, Nothing) Then
            optLinuxRel.Checked = True
            optLinuxRelExternal.Checked = True
        ElseIf txtLinuxImagePath.Text.StartsWith("./", True, Nothing) Then
            optLinuxRel.Checked = True
            optLinuxRelContained.Checked = True
        ElseIf txtLinuxImagePath.Text.StartsWith("/", True, Nothing) Then
            optLinuxAbsolute.Checked = True
        Else
            'optLinuxSystemIcon.Checked = True
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
