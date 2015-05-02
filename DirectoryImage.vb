Imports System.IO.File

Public Class DirectoryImage
    Dim Op As Char = "\" 'Operator character
    Sub LoadDirectoryImage() Handles MyBase.Load
        If My.settings.customeditor <> "" Then
            txtEditorPath.Text = My.settings.customeditor
            chkCustomEditor.Checked = True
        End If
        If Environment.GetEnvironmentVariable("OS") = "Windows_NT" Then
            Op = "\"
        Else
            op = "/"
            grpWindows.Location = New System.Drawing.Point(12, 210)
            grpLinux.Location = New System.Drawing.Point(12, 38)
        End If
        For Each s As String In My.Application.CommandLineArgs
            txtDirectoryPath.Text = s
            grpLinux.Enabled = True
            grpWindows.Enabled = True
            ParseFiles(txtDirectoryPath.Text)
        Next
        If My.Application.CommandLineArgs.Count = 0 Then
            btnDirectoryBrowse_Click
        End If
    End Sub
    
    Sub btnDirectoryBrowse_Click() Handles btnDirectoryBrowse.Click
        If txtDirectoryPath.Text <> "" Then
            FolderBrowserDialog.SelectedPath = txtDirectoryPath.Text
        End If
        If FolderBrowserDialog.ShowDialog = DialogResult.OK Then
            txtDirectoryPath.Text = FolderBrowserDialog.SelectedPath
            
            'GUI elements
            grpLinux.Enabled = True
            grpWindows.Enabled = True
            txtWindowsIconPath.Text = ""
            optWindowsAbsolute.Checked = False
            optWindowsRel.Checked = False
            optWindowsRelContained.Checked = True
            optWindowsRelExternal.Checked = False
            imgWindowsCurrent.Image = Global.DirectoryImage.My.Resources.Resources.ImageResIconNo3
            txtLinuxImagePath.Text = ""
            optLinuxAbsolute.Checked = False
            optLinuxRel.Checked = False
            optLinuxRelContained.Checked = True
            optLinuxRelExternal.Checked = False
            optLinuxSystemImage.Checked = False
            imgLinuxCurrent.Image = Global.DirectoryImage.My.Resources.Resources.OxygenFolder128px
            
            ParseFiles(txtDirectoryPath.Text)
        End If
    End Sub
    
    Dim alreadyGotIcon, lookingForIconIndex As Boolean
    Sub ParseFiles(Directory As String)
        If Exists(Directory & Op &"desktop.ini") Then
            btnWindowsOpenDataFile.Enabled = True
            If GetAttributes(Directory & Op &"desktop.ini").HasFlag(IO.FileAttributes.Hidden) Then
                btnWindowsSetHidden.Enabled = False
            Else
                btnWindowsSetHidden.Enabled = True
            End If
            If GetAttributes(Directory & Op &"desktop.ini").HasFlag(IO.FileAttributes.System) Then
                btnWindowsSetSystem.Enabled = False
            Else
                btnWindowsSetSystem.Enabled = True
            End If
            alreadyGotIcon = False
            lookingForIconIndex = False
            For Each line In ReadLines(Directory & Op &"desktop.ini")
                If line.StartsWith("IconResource=", True, Nothing) Then
                    txtWindowsIconPath.Text = line.Remove(0, 13)
                    alreadyGotIcon = True
                    ParseWindows()
                ElseIf line.StartsWith("IconFile=", True, Nothing) And alreadyGotIcon = False Then
                    txtWindowsIconPath.Text = line.Remove(0, 9)
                    lookingForIconIndex = True
                    ParseWindows()
                ElseIf line.StartsWith("IconIndex=", True, Nothing) And lookingForIconIndex Then
                    txtWindowsIconPath.Text = txtWindowsIconPath.Text & "," & line.Remove(0, 10)
                End If
            Next
        Else
            btnWindowsOpenDataFile.Enabled = False
            btnWindowsSetHidden.Enabled = False
            btnWindowsSetSystem.Enabled = False
        End If
        
        If Exists(Directory & Op &".directory") Then
            btnLinuxOpenDataFile.Enabled = True
            If GetAttributes(txtDirectoryPath.Text & Op &".directory").HasFlag(IO.FileAttributes.Hidden) Then
                btnLinuxSetHidden.Enabled = False
            Else
                btnLinuxSetHidden.Enabled = True
            End If
            If GetAttributes(txtDirectoryPath.Text & Op &".directory").HasFlag(IO.FileAttributes.System) Then
                btnLinuxSetSystem.Enabled = False
            Else
                btnLinuxSetSystem.Enabled = True
            End If
            For Each line In ReadLines(Directory & "\.directory")
                If line.StartsWith("Icon=", True, Nothing) Then
                    txtLinuxImagePath.Text = line.remove(0, 5)
                    ParseLinux()
                End If
            Next
        Else
            btnLinuxOpenDataFile.Enabled = False
            btnLinuxSetHidden.Enabled = False
            btnLinuxSetSystem.Enabled = False
        End If
    End Sub
    
    Sub ParseWindows
        If txtWindowsIconPath.Text.StartsWith("%") Or _
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
            imgWindowsCurrent.ImageLocation = txtWindowsIconPath.Text
        ElseIf txtWindowsIconPath.Text.StartsWith("..\", True, Nothing) Then
            optWindowsRel.Checked = True
            optWindowsRelExternal.Checked = True
            imgWindowsCurrent.ImageLocation = txtDirectoryPath.Text & op & txtWindowsIconPath.Text.Replace("\", "/")
        Else
            optWindowsRel.Checked = True
            optWindowsRelContained.Checked = True
            imgWindowsCurrent.ImageLocation = txtDirectoryPath.Text & op & txtWindowsIconPath.Text.Replace("\", "/")
        End If
    End Sub
    
    Sub ParseLinux
        If txtLinuxImagePath.Text.StartsWith("../", True, Nothing) Then
            optLinuxRel.Checked = True
            optLinuxRelExternal.Checked = True
            imgLinuxCurrent.ImageLocation = txtDirectoryPath.Text & Op & txtLinuxImagePath.Text.Replace("/", "\")
        ElseIf txtLinuxImagePath.Text.StartsWith("./", True, Nothing) Then
            optLinuxRel.Checked = True
            optLinuxRelContained.Checked = True
            imgLinuxCurrent.ImageLocation = txtDirectoryPath.Text & txtLinuxImagePath.Text.Remove(0,1).Replace("/", "\")
        ElseIf txtLinuxImagePath.Text.StartsWith("/", True, Nothing) Then
            optLinuxAbsolute.Checked = True
            If op="\" Then
                LinuxPathToWindowsPath.Show '<- That sets the image
            Else
                imglinuxcurrent.imagelocation = txtlinuximagepath.text
            End If
        Else
            optLinuxSystemImage.Checked = True
            If op="/" Then
                imglinuxcurrent.imagelocation = inputbox("System images location:", "/usr/share/icons/oxygen/256x256/places/") & txtlinuximagepath.text
            End If
        End If
    End Sub
    
    Sub optWindowsRel_CheckedChanged(sender As Object, e As EventArgs) Handles optWindowsRel.CheckedChanged
        grpWindowsRel.Enabled = optWindowsRel.Checked
    End Sub
    
    Sub optLinuxRel_CheckedChanged(sender As Object, e As EventArgs) Handles optLinuxRel.CheckedChanged
        grpLinuxRel.Enabled = optLinuxRel.Checked
    End Sub
    
    Sub WindowsOptionSelected() Handles optWindowsAbsolute.CheckedChanged, optWindowsRel.CheckedChanged, optWindowsRelContained.CheckedChanged, optWindowsRelExternal.CheckedChanged
        btnWindowsSave.Enabled = False
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
    
    Sub WindowsIconPathChanged() Handles txtWindowsIconPath.TextChanged
        If optWindowsAbsolute.Checked = True Then
            btnWindowsSave.Enabled = True
        ElseIf optWindowsRel.Checked = True Then
            If optWindowsRelContained.Checked = True Then
                btnWindowsSave.Enabled = True
            ElseIf optWindowsRelExternal.Checked = True
                btnWindowsSave.Enabled = True
            Else
                btnWindowsSave.Enabled = False
            End If
        Else
            btnWindowsSave.Enabled = False
        End If
    End Sub
    
    Sub btnWindowsIconSet_Click(sender As Object, e As EventArgs) Handles btnWindowsIconSet.Click
        SetInitialDirectories
        If OpenFileDialogWindows.ShowDialog = DialogResult.OK Then
            imgWindowsCurrent.ImageLocation = OpenFileDialogWindows.FileName
            btnWindowsSave.Enabled = True
            If optWindowsAbsolute.Checked = True Then
                If op="\" Then
                    txtWindowsIconPath.Text = OpenFileDialogWindows.FileName
                Else
                    txtwindowsiconpath.text = openfiledialogwindows.filename.replace("/", "\")
                    txtwindowsiconpath.text = inputbox("Please enter the Windows drive letter where the path in linux ""/media/"&Environment.GetEnvironmentVariable("UserName")&"/MountPath"" is mounted:", _
                                                       "Windows Drive Letter","& OpenFileDialogLinux.FileName.Remove(2)&") & txtWindowsIconPath.Text
                End If
            ElseIf optWindowsRelContained.Checked = True Then
                If txtDirectorypath.text.endswith(":\") Then
                    txtWindowsIconPath.Text = OpenFileDialogWindows.FileName.Remove(0, txtDirectoryPath.Text.Length).Replace("/", "\")
                Else
                    txtWindowsIconPath.Text = OpenFileDialogWindows.FileName.Remove(0, txtDirectoryPath.Text.Length + 1).Replace("/", "\")
                End If
            ElseIf optWindowsRelExternal.Checked = True Then
                txtWindowsIconPath.Text = ".." & OpenFileDialogWindows.FileName.Remove(0, OpenFileDialogWindows.FileName.Replace("/", "\").LastIndexOf("\"))
            Else
                MsgBox("Please select an option!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub
    
    Dim lineNo, headerLine As Byte
    Dim SetIcon, HasHeader As Boolean
    Sub btnWindowsSave_Click() Handles btnWindowsSave.Click
        Try
            If Exists(txtDirectoryPath.Text & Op &"desktop.ini") Then
                lineno = 0
                seticon = False
                hasheader = false
                Dim FileContents As String() = ReadAllLines(txtDirectoryPath.Text & Op &"desktop.ini")
                For Each line As String In FileContents
                    If line.StartsWith("IconResource=", True, Nothing) Then
                        FileContents(lineno) = "IconResource=" & txtWindowsIconPath.Text
                        seticon = true
                    ElseIf line.StartsWith("IconFile=", True, Nothing) Then
                        FileContents(lineno) = nothing
                    ElseIf line.StartsWith("IconIndex=", True, Nothing) Then
                        FileContents(lineno) = Nothing
                    ElseIf line = "[.ShellClassInfo]" Then
                        hasheader = True
                        headerline = lineno
                    End If
                    lineno += 1
                Next
                setattributes(txtDirectoryPath.Text & Op &"desktop.ini", io.fileattributes.normal)
                If seticon Then
                    WriteAllLines(txtDirectoryPath.Text & Op &"desktop.ini", FileContents)
                Else
                    If hasheader Then
                        If FileContents.length<headerline+2 Then
                            Array.Resize(FileContents, FileContents.Length+1)
                        Else
                            Do Until filecontents(headerline+1) = ""
                                headerline+=1
                                If FileContents.length<headerline+2 Then
                                    Array.Resize(FileContents, FileContents.Length+1)
                                End If
                            Loop
                        End If
                        FileContents(headerline+1) = "IconResource=" & txtWindowsIconPath.Text
                        WriteAllLines(txtDirectoryPath.Text & Op &"desktop.ini", FileContents)
                    Else
                        appendalltext(txtDirectoryPath.Text & Op &"desktop.ini", vbnewline &"[.ShellClassInfo]"&vbNewLine &"IconResource="& txtWindowsIconPath.Text &vbnewline)
                    End If
                End If
            Else
                WriteAllText(txtDirectoryPath.Text & Op &"desktop.ini", "[.ShellClassInfo]" & vbNewLine & "IconResource=" & txtWindowsIconPath.Text)
                btnWindowsOpenDataFile.Enabled = True
            End If
            SetAttributes(txtDirectoryPath.Text & Op &"desktop.ini", IO.FileAttributes.Hidden)
        Catch ex As exception
            ErrorParser(ex)
        End Try
        ParseFiles(txtDirectoryPath.Text)
    End Sub
    
    Sub btnWindowsOpenDataFile_Click(sender As Object, e As EventArgs) Handles btnWindowsOpenDataFile.Click
        If Exists(txtDirectoryPath.Text & Op &"desktop.ini") Then
            If chkcustomeditor.checked Then
                Process.Start(txteditorpath.text, txtDirectoryPath.Text & Op &"desktop.ini")
            Else
                If op="\" Then
                    Process.Start(Environment.GetEnvironmentVariable("windir") & "\notepad.exe", txtDirectoryPath.Text & "\desktop.ini")
                Else
                    Process.Start(txtDirectoryPath.Text & "/desktop.ini") 'Environment.GetEnvironmentVariable("windir") & "/notepad.exe", 
                End If
            End If
        Else
            btnWindowsOpenDataFile.Enabled = False
        End If
    End Sub
    
    Sub btnWindowsSetHidden_Click() Handles btnWindowsSetHidden.Click
        'If Not GetAttributes(txtDirectoryPath.Text & Op &"desktop.ini").HasFlag(IO.FileAttributes.Hidden) Then
        SetAttributes(txtDirectoryPath.Text & Op &"desktop.ini", FileAttribute.Hidden)
        ParseFiles(txtDirectoryPath.Text)
        'btnWindowsSetHidden.Enabled = False '<= no need since ParseFiles will detect if the flag is set
        'End If
    End Sub
    
    Sub btnWindowsSetSystem_Click() Handles btnWindowsSetSystem.Click
        SetAttributes(txtDirectoryPath.Text & Op &"desktop.ini", FileAttribute.System)
        ParseFiles(txtDirectoryPath.Text)
    End Sub
    
    Sub SetInitialDirectories()
        If optWindowsAbsolute.Checked = True Then
            If OpenFileDialogWindows.InitialDirectory = "" Then
                OpenFileDialogWindows.InitialDirectory = txtDirectoryPath.Text
            End If
        ElseIf optWindowsRel.Checked = True Then
            If optWindowsRelContained.Checked = True Then
                OpenFileDialogWindows.InitialDirectory = txtDirectoryPath.Text
            ElseIf optWindowsRelExternal.Checked = True
                OpenFileDialogWindows.InitialDirectory = txtDirectoryPath.Text.Remove(txtDirectoryPath.Text.LastIndexOf("\"))
            End If
            If txtWindowsIconPath.Text <> "" Then
                OpenFileDialogWindows.FileName = txtWindowsIconPath.Text
            End If
        End If
        
        If optLinuxAbsolute.Checked = True Then
            If OpenFileDialogLinux.InitialDirectory = "" Then
                OpenFileDialogLinux.InitialDirectory = txtDirectoryPath.Text
            End If
        ElseIf optLinuxRel.Checked = True Then
            If optLinuxRelContained.Checked = True Then
                OpenFileDialogLinux.InitialDirectory = txtDirectoryPath.Text
            ElseIf optLinuxRelExternal.Checked = True
                OpenFileDialogLinux.InitialDirectory = txtDirectoryPath.Text.Remove(txtDirectoryPath.Text.LastIndexOf("\"))
            End If
            If txtLinuxImagePath.Text <> "" Then
                OpenFileDialogLinux.FileName = txtLinuxImagePath.Text
            End If
        End If
    End Sub
    
    Sub LinuxOptionSelected() Handles optLinuxAbsolute.CheckedChanged, optLinuxRel.CheckedChanged, optLinuxRelContained.CheckedChanged, optLinuxRelExternal.CheckedChanged, optLinuxSystemImage.CheckedChanged
        btnLinuxSave.enabled = false
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
        ElseIf optLinuxSystemImage.Checked = True
            btnLinuxIconSet.Enabled = True
        Else
            btnLinuxIconSet.Enabled = False
        End If
    End Sub
    
    Sub LinuxImagePathChanged() Handles txtLinuxImagePath.TextChanged
        If optLinuxAbsolute.Checked = True Then
            btnLinuxSave.Enabled = True
        ElseIf optLinuxRel.Checked = True Then
            If optLinuxRelContained.Checked = True Then
                btnLinuxSave.Enabled = True
            ElseIf optLinuxRelExternal.Checked = True
                btnLinuxSave.Enabled = True
            Else
                btnLinuxSave.Enabled = False
            End If
        ElseIf optlinuxsystemimage.checked = True
            btnlinuxsave.enabled = True
        End If
    End Sub
    
    Sub btnLinuxIconSet_Click(sender As Object, e As EventArgs) Handles btnLinuxIconSet.Click
        SetInitialDirectories
        If OpenFileDialogLinux.ShowDialog = DialogResult.OK Then
            imgLinuxCurrent.ImageLocation = OpenFileDialogLinux.FileName
            btnWindowsSave.Enabled = True
            If optLinuxAbsolute.Checked = True Then
                txtLinuxImagePath.Text = OpenFileDialogLinux.FileName.Remove(0,2).Replace("\", "/")
                txtLinuxImagePath.Text = InputBox("Please enter the path in linux where drive """& OpenFileDialogLinux.FileName.Remove(2)&""" is mounted:", _
                                                  "Linux Drive Mountpoint","/media/"&Environment.GetEnvironmentVariable("UserName")&"/MountPath") & txtLinuxImagePath.Text
            ElseIf optLinuxRelContained.Checked = True Then
                If txtDirectorypath.text.endswith(":\") Then
                    txtLinuxImagePath.Text = "./" & OpenFileDialogLinux.FileName.Remove(0, txtDirectoryPath.Text.Length).Replace("\", "/")
                Else
                    txtLinuxImagePath.Text = "./" & OpenFileDialogLinux.FileName.Remove(0, txtDirectoryPath.Text.Length + 1).Replace("\", "/")
                End If
            ElseIf optLinuxRelExternal.Checked = True Then
                txtLinuxImagePath.Text = ".." & OpenFileDialogLinux.FileName.Remove(0, OpenFileDialogLinux.FileName.LastIndexOf("\")).Replace("\", "/")
            Else
                MsgBox("Please select an option!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub
    
    Sub btnLinuxSave_Click() Handles btnLinuxSave.Click
        Try
            If Exists(txtDirectoryPath.Text & "\.directory") Then
                lineno = 0 ' Actually the index of the line
                seticon = False  ' Index starts at 0, count/length/number starts at 1
                hasheader = false
                Dim FileContents As String() = ReadAllLines(txtDirectoryPath.Text & "\.directory")
                For Each line As String In FileContents
                    If line.StartsWith("Icon=", True, Nothing) Then
                        FileContents(lineno) = "Icon=" & txtLinuxImagePath.Text
                        seticon = true
                    ElseIf line = "[Desktop Entry]" Then
                        hasheader = True
                        headerline = lineno ' therefore it is the header index not line
                    End If
                    lineno += 1
                Next
                setattributes(txtDirectoryPath.Text & "\.directory", io.fileattributes.normal)
                If seticon Then
                    WriteAllLines(txtDirectoryPath.Text & "\.directory", FileContents)
                Else
                    If hasheader Then
                        If FileContents.length<headerline+2 Then
                        ' account for index -> length conversion, and that less than will have to be less than or equal to without another addition
                            Array.Resize(FileContents, FileContents.Length+1)
                        Else
                            Do Until filecontents(headerline+1) = ""
                                headerline+=1
                                If FileContents.length<headerline+2 Then
                                    Array.Resize(FileContents, FileContents.Length+1)
                                End If
                            Loop
                        End If
                        FileContents(headerline+1) = "Icon=" & txtLinuxImagePath.Text
                        WriteAllLines(txtDirectoryPath.Text & "\.directory", FileContents)
                    Else
                        appendalltext(txtDirectoryPath.Text & "\.directory", vbnewline &"[Desktop Entry]"&vbNewLine &"Icon="& txtLinuxImagePath.Text &vbnewline)
                    End If
                End If
            Else
                WriteAllText(txtDirectoryPath.Text & "\.directory", "[Desktop Entry]" & vbNewLine & "Icon=" & txtLinuxImagePath.Text)
                btnLinuxOpenDataFile.Enabled = True
            End If
            SetAttributes(txtDirectoryPath.Text & "\.directory", IO.FileAttributes.Hidden)
        Catch ex As exception
            ErrorParser(ex)
        End Try
        ParseFiles(txtDirectoryPath.Text)
    End Sub
    
    Sub btnLinuxOpenDataFile_Click(sender As Object, e As EventArgs) Handles btnLinuxOpenDataFile.Click
        If Exists(txtDirectoryPath.Text & "\.directory") Then
            If chkcustomeditor.checked Then
                Process.Start(txteditorpath.text, txtDirectoryPath.Text & "\.directory")
            Else
                Process.Start(Environment.GetEnvironmentVariable("windir") & "\notepad.exe", txtDirectoryPath.Text & "\.directory")
            End If
        Else
            btnLinuxOpenDataFile.Enabled = False
        End If
    End Sub
    
    Sub btnLinuxSetHidden_Click() Handles btnLinuxSetHidden.Click
        SetAttributes(txtDirectoryPath.Text & "\.directory", FileAttribute.Hidden)
        ParseFiles(txtDirectoryPath.Text)
    End Sub
    
    Sub btnLinuxSetSystem_Click() Handles btnLinuxSetSystem.Click
        SetAttributes(txtDirectoryPath.Text & "\.directory", FileAttribute.System)
        ParseFiles(txtDirectoryPath.Text)
    End Sub
    
    Sub chkCustomEditor_CheckedChanged(sender As Object, e As EventArgs) handles chkcustomeditor.checkedchanged
        btneditorbrowse.enabled = chkcustomeditor.checked
        txteditorpath.enabled = false
        btneditorpathcustom.enabled = chkcustomeditor.checked
        btneditorpathcustom.text = "Edit..."
        If chkcustomeditor.checked Then
            if txteditorpath.text = "" then btneditorbrowse_click() else my.settings.customeditor = txteditorpath.text
        Else
            My.settings.customeditor = ""
        End If
    End Sub
    
    Sub btnEditorBrowse_Click() handles btneditorbrowse.click
        openfiledialogeditor.initialdirectory = environment.GetEnvironmentVariable("ProgramFiles")
        If openfiledialogeditor.showdialog = dialogresult.ok Then
            txteditorpath.text = openfiledialogeditor.filename
            my.settings.customeditor = openfiledialogeditor.filename
        End If
    End Sub
    
    Sub btnEditorPathCustom_Click(sender As Object, e As EventArgs) handles btneditorpathcustom.click
        If btneditorpathcustom.text = "Edit..." Then
            txteditorpath.enabled = true
            btneditorpathcustom.text = "Save"
        Else
            My.Settings.CustomEditor = txteditorpath.text
            txteditorpath.enabled = false
            btneditorpathcustom.text = "Edit..."
        End If
    End Sub
    
    Sub ErrorParser(ex As Exception)
        'Access Denied:
        'HResult of -2147024891
        'GetHashCode of 44223604
        'GetType: System.UnauthorizedAccessException
        'Message: Access to the path 'filepath' is denied.
        If ex.GetType.ToString = "System.UnauthorizedAccessException" Then
        'Neither
        'If ex.GetType.Equals(System.UnauthorizedAccessException) Then
        'nor
        'If System.UnauthorizedAccessException.Equals(ex.GetType) Then
        'work so i just compared them as strings.
            If MsgBox(ex.message & vbnewline & vbnewline & "Try launching DirectoryImage As Administrator?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "Access denied!") = MsgBoxResult.Yes Then
                Dim objShell As Object = CreateObject("Shell.Application")
                objShell.ShellExecute(Environment.CurrentDirectory & Op & Process.GetCurrentProcess.ProcessName & ".exe", """" & txtDirectoryPath.Text & """", "", "runas")
                Application.Exit
            End If
        Else
            If MsgBox("There was an error! Error message: " & ex.Message & vbNewLine & "Show full stacktrace? (For sending to developer/making bugreport)", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "Error!") = MsgBoxresult.Yes Then
                Dim frmBugReport As New Form()
                frmBugReport.Width = 600
                frmBugReport.Height = 525
                frmBugReport.StartPosition = FormStartPosition.CenterParent
                frmBugReport.WindowState = Me.WindowState
                frmBugReport.ShowIcon = False
                frmBugReport.ShowInTaskbar = True
                frmBugReport.Text = "Full error trace"
                Dim txtBugReport As New TextBox()
                txtBugReport.Multiline = True
                txtBugReport.ScrollBars = ScrollBars.Vertical
                frmBugReport.Controls.Add(txtBugReport)
                txtBugReport.Dock = DockStyle.Fill
                txtBugReport.Text = "ToString:" & vbNewLine & ex.ToString & vbNewLine & vbNewLine & _
                                    "Data:" & vbNewLine & ex.Data.ToString & vbNewLine & vbNewLine & _
                                    "BaseException:" & vbNewLine & ex.GetBaseException.ToString & vbNewLine & vbNewLine & _
                                    "HashCode:" & vbNewLine & ex.GetHashCode.ToString & vbNewLine & vbNewLine & _
                                    "Type:" & vbNewLine & ex.GetType.ToString & vbNewLine & vbNewLine & _
                                    "HResult:" & vbNewLine & ex.HResult.ToString & vbNewLine & vbNewLine & _
                                    "Message:" & vbNewLine & ex.Message.ToString & vbNewLine & vbNewLine & _
                                    "Source:" & vbNewLine & ex.Source.ToString & vbNewLine & vbNewLine & _
                                    "StackTrace:" & vbNewLine & ex.StackTrace.ToString & vbNewLine & vbNewLine & _
                                    "TargetSite:" & vbNewLine & ex.TargetSite.ToString
                                    '"InnerException:" & vbNewLine & ex.InnerException.ToString & vbNewLine & vbNewLine & _
                frmBugReport.Show()
            End If
        End If
    End Sub
End Class
