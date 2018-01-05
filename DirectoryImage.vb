Imports System.IO.File
Imports System.Runtime.InteropServices
Public Class DirectoryImage
    
    Dim configFilePath As String = ""
    Dim windowsCustomizeTab As String = "Customize"
    Dim Op As Char = "\" 'Operator character
    Sub LoadDirectoryImage() Handles MyBase.Load
        If System.Globalization.CultureInfo.CurrentCulture.EnglishName = "English (South Africa)" Then
            windowsCustomizeTab = "Customise"
        End If
        
        If Environment.GetEnvironmentVariable("OS") = "Windows_NT" Then
            Op = "\"
            If Not Directory.Exists(Environment.GetEnvironmentVariable("AppData") & "\WalkmanOSS") Then
                Directory.CreateDirectory(Environment.GetEnvironmentVariable("AppData") & "\WalkmanOSS")
            End If
            configFilePath = Environment.GetEnvironmentVariable("AppData") & "\WalkmanOSS\DirectoryImage.xml"
            
            If File.Exists(Application.StartupPath & "\DirectoryImage.xml") Then
                configFilePath = Application.StartupPath & "\DirectoryImage.xml"
            ElseIf File.Exists("DirectoryImage.xml") Then
                configFilePath = (New IO.FileInfo("DirectoryImage.xml")).FullName
            End If
            
            If File.Exists(configFilePath) Then
                ReadConfig(configFilePath)
            End If
        Else
            Op = "/"
            If Not Directory.Exists(Environment.GetEnvironmentVariable("HOME") & "/.walkmanoss") Then
                Directory.CreateDirectory(Environment.GetEnvironmentVariable("HOME") & "/.walkmanoss")
            End If
            configFilePath = Environment.GetEnvironmentVariable("HOME") & "/.walkmanoss/DirectoryImage.xml"
            
            If File.Exists(Application.StartupPath & "/DirectoryImage.xml") Then
                configFilePath = Application.StartupPath & "/DirectoryImage.xml"
            ElseIf File.Exists("DirectoryImage.xml") Then
                configFilePath = (New IO.FileInfo("DirectoryImage.xml")).FullName
            End If
            
            If File.Exists(configFilePath) Then
                ReadConfig(configFilePath)
            End If
            
            grpWindows.Location = New System.Drawing.Point(12, 210)
            grpLinux.Location = New System.Drawing.Point(12, 38)
            btnWindowsProperties.Visible = False
        End If
        
        If New WindowsPrincipal(WindowsIdentity.GetCurrent).IsInRole(WindowsBuiltInRole.Administrator) Then _
            Me.Text = "[Admin] Windows and Linux directory image setter"
        
        If My.Application.CommandLineArgs.Count = 0 Then
            timerDelayedBrowse.Start
        Else
            txtDirectoryPath.Text = My.Application.CommandLineArgs(0)
            grpLinux.Enabled = True
            grpWindows.Enabled = True
            
            If txtDirectoryPath.Text.EndsWith("""") Then
                txtDirectoryPath.Text = txtDirectoryPath.Text.Remove(txtDirectoryPath.Text.Length - 1) & "\"
            End If
            ParseFiles(txtDirectoryPath.Text)
        End If
    End Sub
    
    Sub timerDelayedBrowse_Tick() Handles timerDelayedBrowse.Tick
        timerDelayedBrowse.Stop
        btnDirectoryBrowse_Click
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
            imgWindowsCurrent.Image = My.Resources.Resources.ImageResIconNo3
            txtLinuxImagePath.Text = ""
            optLinuxAbsolute.Checked = False
            optLinuxRel.Checked = False
            optLinuxRelContained.Checked = True
            optLinuxRelExternal.Checked = False
            optLinuxSystemImage.Checked = False
            imgLinuxCurrent.Image = My.Resources.Resources.OxygenFolder128px
            
            ParseFiles(txtDirectoryPath.Text)
        End If
    End Sub
    
    Dim alreadyGotIcon, lookingForIconIndex As Boolean
    Sub ParseFiles(Directory As String)
        If directory.endswith(":\") Then
            btnWindowsOpenDataFile.Text = "Open Autorun.inf..."
            If Exists(Directory & "Autorun.inf") Then
                btnWindowsOpenDataFile.Enabled = True
                chkWindowsHidden.Enabled = True
                chkWindowsSystem.Enabled = True
                chkWindowsHidden.Checked = GetAttributes(Directory & "Autorun.inf").HasFlag(IO.FileAttributes.Hidden)
                chkWindowsSystem.Checked = GetAttributes(Directory & "Autorun.inf").HasFlag(IO.FileAttributes.System)
                For Each line In ReadLines(Directory & "Autorun.inf")
                    If line.StartsWith("Icon=", True, Nothing) Then
                        txtWindowsIconPath.Text = line.Substring(5)
                        ParseWindows()
                    End If
                Next
            Else
                btnWindowsOpenDataFile.Enabled = False
                chkWindowsHidden.Enabled = False
                chkWindowsSystem.Enabled = False
                chkWindowsHidden.Checked = False
                chkWindowsSystem.Checked = False
            End If
        Else
            btnWindowsOpenDataFile.Text = "Open desktop.ini..."
            If Exists(Directory & Op & "desktop.ini") Then
                btnWindowsOpenDataFile.Enabled = True
                chkWindowsHidden.Enabled = True
                chkWindowsSystem.Enabled = True
                chkWindowsHidden.Checked = GetAttributes(Directory & Op & "desktop.ini").HasFlag(IO.FileAttributes.Hidden)
                chkWindowsSystem.Checked = GetAttributes(Directory & Op & "desktop.ini").HasFlag(IO.FileAttributes.System)
                alreadyGotIcon = False
                lookingForIconIndex = False
                For Each line In ReadLines(Directory & Op & "desktop.ini")
                    If line.StartsWith("IconResource=", True, Nothing) Then
                        txtWindowsIconPath.Text = line.Substring(13)
                        alreadyGotIcon = True
                        ParseWindows()
                    ElseIf line.StartsWith("IconFile=", True, Nothing) And alreadyGotIcon = False Then
                        txtWindowsIconPath.Text = line.Substring(9)
                        lookingForIconIndex = True
                        ParseWindows()
                    ElseIf line.StartsWith("IconIndex=", True, Nothing) And lookingForIconIndex Then
                        txtWindowsIconPath.Text = txtWindowsIconPath.Text & "," & line.Substring(10)
                        lookingForIconIndex = False
                    End If
                Next
            Else
                btnWindowsOpenDataFile.Enabled = False
                chkWindowsHidden.Enabled = False
                chkWindowsSystem.Enabled = False
                chkWindowsHidden.Checked = False
                chkWindowsSystem.Checked = False
            End If
        End If
        
        If Exists(Directory & Op & ".directory") Then
            btnLinuxOpenDataFile.Enabled = True
            chkLinuxHidden.Enabled = True
            chkLinuxSystem.Enabled = True
            chkLinuxHidden.Checked = GetAttributes(txtDirectoryPath.Text & Op & ".directory").HasFlag(IO.FileAttributes.Hidden)
            chkLinuxSystem.Checked = GetAttributes(txtDirectoryPath.Text & Op & ".directory").HasFlag(IO.FileAttributes.System)
            For Each line In ReadLines(Directory & Op & ".directory")
                If line.StartsWith("Icon=", True, Nothing) Then
                    txtLinuxImagePath.Text = line.Substring(5)
                    ParseLinux()
                End If
            Next
        Else
            btnLinuxOpenDataFile.Enabled = False
            chkLinuxHidden.Enabled = False
            chkLinuxSystem.Enabled = False
            chkLinuxHidden.Checked = False
            chkLinuxSystem.Checked = False
        End If
    End Sub
    
    Sub ParseWindows
        Dim isAbsolute As New Boolean
        If txtWindowsIconPath.Text.StartsWith("%") Then
            isAbsolute = True
        Else
            For i = 1 To 26 ' The Chr() below will give all letters from A to Z
                If txtWindowsIconPath.Text.StartsWith( Chr(i+64) & ":\", True, Nothing) Then
                    isAbsolute = True
                    Exit For
                End If
            Next
        End If
        If isAbsolute Then
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
        
        If imgWindowsCurrent.ImageLocation.EndsWith(",0") Then
            imgWindowsCurrent.ImageLocation = imgWindowsCurrent.ImageLocation.Remove(imgWindowsCurrent.ImageLocation.Length-2)
        End If
        If imgWindowsCurrent.ImageLocation.StartsWith("%") Then
            imgWindowsCurrent.ImageLocation = imgWindowsCurrent.ImageLocation.Substring(1)
            If imgWindowsCurrent.ImageLocation.Contains("%") Then
                imgWindowsCurrent.ImageLocation = Environment.GetEnvironmentVariable(imgWindowsCurrent.ImageLocation.Remove(imgWindowsCurrent.ImageLocation.IndexOf("%"))) _
                  & imgWindowsCurrent.ImageLocation.Substring(imgWindowsCurrent.ImageLocation.IndexOf("%")+1)
            End If
        End If
    End Sub
    
    Sub imgWindowsCurrent_LoadCompleted(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs) Handles imgWindowsCurrent.LoadCompleted
        If Not IsNothing(e.Error) Then
            Try
                imgWindowsCurrent.Image = Icon.ExtractAssociatedIcon(imgWindowsCurrent.ImageLocation).ToBitmap
            Catch
            End Try
        End If
    End Sub
    
    Sub ParseLinux
        If txtLinuxImagePath.Text.StartsWith("/", True, Nothing) Then
            optLinuxAbsolute.Checked = True
            If op="\" Then
                LinuxPathToWindowsPath.ShowDialog '<- That sets the image
            Else
                imgLinuxCurrent.ImageLocation = txtLinuxImagePath.Text
            End If
        ElseIf txtLinuxImagePath.Text.StartsWith("./../", True, Nothing) Then
            optLinuxRel.Checked = True
            optLinuxRelExternal.Checked = True
            imgLinuxCurrent.ImageLocation = txtDirectoryPath.Text & txtLinuxImagePath.Text.Substring(1).Replace("/", "\")
        ElseIf txtLinuxImagePath.Text.StartsWith("./", True, Nothing) Then
            optLinuxRel.Checked = True
            optLinuxRelContained.Checked = True
            imgLinuxCurrent.ImageLocation = txtDirectoryPath.Text & txtLinuxImagePath.Text.Substring(1).Replace("/", "\")
        Else
            optLinuxSystemImage.Checked = True
            If op="/" Then
                imglinuxcurrent.imagelocation = inputbox("System images location:", "Enter Location", Environment.GetEnvironmentVariable("HOME") & "/.local/share/icons/hicolor/256x256/apps/") & txtlinuximagepath.text
            End If
        End If
    End Sub
    
    Sub optWindowsRel_CheckedChanged() Handles optWindowsRel.CheckedChanged
        grpWindowsRel.Enabled = optWindowsRel.Checked
    End Sub
    
    Sub optLinuxRel_CheckedChanged() Handles optLinuxRel.CheckedChanged
        grpLinuxRel.Enabled = optLinuxRel.Checked
    End Sub
    
    Sub WindowsOptionSelected() Handles optWindowsAbsolute.CheckedChanged, optWindowsRel.CheckedChanged, _
                                    optWindowsRelContained.CheckedChanged, optWindowsRelExternal.CheckedChanged
        btnWindowsSave.Enabled = False
        If optWindowsAbsolute.Checked Then
            btnWindowsIconSet.Enabled = True
        ElseIf optWindowsRel.Checked Then
            If optWindowsRelContained.Checked Then
                btnWindowsIconSet.Enabled = True
            ElseIf optWindowsRelExternal.Checked
                btnWindowsIconSet.Enabled = True
            Else
                btnWindowsIconSet.Enabled = False
            End If
        Else
            btnWindowsIconSet.Enabled = False
        End If
    End Sub
    
    Sub WindowsIconPathChanged() Handles txtWindowsIconPath.TextChanged
        If optWindowsAbsolute.Checked Then
            btnWindowsSave.Enabled = True
        ElseIf optWindowsRel.Checked Then
            If optWindowsRelContained.Checked Then
                btnWindowsSave.Enabled = True
            ElseIf optWindowsRelExternal.Checked
                btnWindowsSave.Enabled = True
            Else
                btnWindowsSave.Enabled = False
            End If
        Else
            btnWindowsSave.Enabled = False
        End If
    End Sub
    
    Sub btnWindowsIconSet_Click() Handles btnWindowsIconSet.Click
        SetInitialDirectories
        If OpenFileDialogWindows.ShowDialog = DialogResult.OK Then
            imgWindowsCurrent.ImageLocation = OpenFileDialogWindows.FileName
            btnWindowsSave.Enabled = True
            If optWindowsAbsolute.Checked Then
                If Op = "\" Then
                    txtWindowsIconPath.Text = OpenFileDialogWindows.FileName
                Else
                    txtwindowsiconpath.text = openfiledialogwindows.filename.replace("/", "\")
                    LinuxPathToWindowsDrive.ShowDialog
                End If
            ElseIf optWindowsRelContained.Checked Then
                If txtDirectorypath.text.endswith(":\") Then
                    txtWindowsIconPath.Text = OpenFileDialogWindows.FileName.Substring(txtDirectoryPath.Text.Length)
                Else
                    txtWindowsIconPath.Text = OpenFileDialogWindows.FileName.Substring(txtDirectoryPath.Text.Length + 1).Replace("/", "\")
                End If
            ElseIf optWindowsRelExternal.Checked Then
                txtWindowsIconPath.Text = ".."
                Dim scanText As String
                Try
                    scanText = txtDirectoryPath.Text.Substring(OpenFileDialogWindows.FileName.Replace("/", "\").LastIndexOf("\")+1).Replace("/", "\")
                    If scanText.Contains("\") Then
                        For Each character As Char In scanText
                            If character = "\" Then
                                txtWindowsIconPath.Text &= "\.."
                            End If
                        Next
                    End If
                Catch
                End Try
                txtWindowsIconPath.Text &= OpenFileDialogWindows.FileName.Substring(OpenFileDialogWindows.FileName.Replace("/", "\").LastIndexOf("\"))
            Else
                MsgBox("Please select an option!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub
    
    Dim lineNo, headerLine As Byte
    Dim SetIcon, HasHeader As Boolean
    Sub btnWindowsSave_Click() Handles btnWindowsSave.Click
        Try
            If txtDirectoryPath.Text.endswith(":\") Then
                If Exists(txtDirectoryPath.Text & "Autorun.inf") Then
                    lineno = 0
                    seticon = False
                    hasheader = False
                    Dim FileContents As String() = ReadAllLines(txtDirectoryPath.Text & "Autorun.inf")
                    For Each line As String In FileContents
                        If line.StartsWith("Icon=", True, Nothing) Then
                            FileContents(lineno) = "Icon=" & txtWindowsIconPath.Text
                            seticon = True
                        ElseIf line = "[autorun]" Then
                            hasheader = True
                            headerline = lineno
                        End If
                        lineno += 1
                    Next
                    setattributes(txtDirectoryPath.Text & "Autorun.inf", io.fileattributes.normal)
                    If seticon Then
                        WriteAllLines(txtDirectoryPath.Text & "Autorun.inf", FileContents)
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
                            FileContents(headerline+1) = "Icon=" & txtWindowsIconPath.Text
                            WriteAllLines(txtDirectoryPath.Text & "Autorun.inf", FileContents)
                        Else
                            appendalltext(txtDirectoryPath.Text & "Autorun.inf", vbnewline &"[.ShellClassInfo]"&vbNewLine &"Icon="& txtWindowsIconPath.Text &vbnewline)
                        End If
                    End If
                Else
                    WriteAllText(txtDirectoryPath.Text & "Autorun.inf", "[autorun]" & vbNewLine & "Icon=" & txtWindowsIconPath.Text)
                    btnWindowsOpenDataFile.Enabled = True
                End If
                SetAttributes(txtDirectoryPath.Text & "Autorun.inf", IO.FileAttributes.Hidden)
            Else
                If Exists(txtDirectoryPath.Text & Op & "desktop.ini") Then
                    lineno = 0
                    seticon = False
                    hasheader = False
                    Dim FileContents As String() = ReadAllLines(txtDirectoryPath.Text & Op & "desktop.ini")
                    For Each line In FileContents
                        If line.StartsWith("IconResource=", True, Nothing) Then
                            FileContents(lineno) = "IconResource=" & txtWindowsIconPath.Text
                            seticon = True
                        ElseIf line.StartsWith("IconFile=", True, Nothing) Then
                            FileContents(lineno) = Nothing
                        ElseIf line.StartsWith("IconIndex=", True, Nothing) Then
                            FileContents(lineno) = Nothing
                        ElseIf line = "[.ShellClassInfo]" Then
                            hasheader = True
                            headerline = lineno
                        End If
                        lineno += 1
                    Next
                    setattributes(txtDirectoryPath.Text & Op & "desktop.ini", io.fileattributes.normal)
                    If seticon Then
                        WriteAllLines(txtDirectoryPath.Text & Op & "desktop.ini", FileContents)
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
                            WriteAllLines(txtDirectoryPath.Text & Op & "desktop.ini", FileContents)
                        Else
                            AppendAllText(txtDirectoryPath.Text & Op & "desktop.ini", _
                                vbNewLine & "[.ShellClassInfo]" & vbNewLine & "IconResource=" & txtWindowsIconPath.Text & vbNewLine)
                        End If
                    End If
                Else
                    WriteAllText(txtDirectoryPath.Text & Op & "desktop.ini", "[.ShellClassInfo]" & vbNewLine & "IconResource=" & txtWindowsIconPath.Text)
                    btnWindowsOpenDataFile.Enabled = True
                End If
                SetAttributes(txtDirectoryPath.Text & Op & "desktop.ini", IO.FileAttributes.Hidden)
            End If
        Catch ex As exception
            ErrorParser(ex)
        End Try
        ParseFiles(txtDirectoryPath.Text)
    End Sub
    
    Sub btnWindowsOpenDataFile_Click() Handles btnWindowsOpenDataFile.Click
        If txtDirectoryPath.Text.endswith(":\") Then
            If Exists(txtDirectoryPath.Text & "Autorun.inf") Then
                If chkCustomEditor.Checked Then
                    Process.Start(txtEditorPath.Text, txtDirectoryPath.Text & "Autorun.inf")
                Else ' No Op check below because if the path ends with ":\", we're on Windows
                    If op="\" Then ' EXCEPT it is possible to make a random folder called ":\" because >linux
                        Process.Start(Environment.GetEnvironmentVariable("windir") & "\notepad.exe", txtDirectoryPath.Text & "Autorun.inf")
                    Else
                        Process.Start(txtDirectoryPath.Text & "/Autorun.inf") 'Environment.GetEnvironmentVariable("windir") & "\notepad.exe", 
                    End If
                End If
            Else
                btnWindowsOpenDataFile.Enabled = False
            End If
        Else
            If Exists(txtDirectoryPath.Text & Op & "desktop.ini") Then
                If chkCustomEditor.Checked Then
                    Process.Start(txtEditorPath.Text, txtDirectoryPath.Text & Op & "desktop.ini")
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
        End If
    End Sub
    
    Sub chkWindowsHidden_CheckedChanged() Handles chkWindowsHidden.Click 'CheckedChanged
        Try
            If txtDirectoryPath.Text.endswith(":\") Then
                If chkWindowsHidden.Checked Then
                    RemoveAttribute(txtDirectoryPath.Text & "Autorun.inf", FileAttribute.Hidden)
                Else
                    AddAttribute(txtDirectoryPath.Text & "Autorun.inf", FileAttributes.Hidden)
                End If
            Else
                If chkWindowsHidden.Checked Then
                    RemoveAttribute(txtDirectoryPath.Text & Op & "desktop.ini", FileAttribute.Hidden)
                Else
                    AddAttribute(txtDirectoryPath.Text & Op & "desktop.ini", FileAttributes.Hidden)
                End If
            End If
        Catch ex As exception
            ErrorParser(ex)
        End Try
        ParseFiles(txtDirectoryPath.Text)
    End Sub
    
    Sub chkWindowsSystem_CheckedChanged() Handles chkWindowsSystem.Click 'CheckedChanged
        Try
            If txtDirectoryPath.Text.endswith(":\") Then
                If chkWindowsSystem.Checked Then
                    RemoveAttribute(txtDirectoryPath.Text & "Autorun.inf", FileAttribute.System)
                Else
                    AddAttribute(txtDirectoryPath.Text & "Autorun.inf", FileAttribute.System)
                End If
            Else
                If chkWindowsSystem.Checked Then
                    RemoveAttribute(txtDirectoryPath.Text & Op & "desktop.ini", FileAttribute.System)
                Else
                    AddAttribute(txtDirectoryPath.Text & Op & "desktop.ini", FileAttribute.System)
                End If
            End If
        Catch ex As exception
            ErrorParser(ex)
        End Try
        ParseFiles(txtDirectoryPath.Text)
    End Sub
    
    Sub SetInitialDirectories()
        If optWindowsAbsolute.Checked Then
            If OpenFileDialogWindows.InitialDirectory = "" Then
                OpenFileDialogWindows.InitialDirectory = txtDirectoryPath.Text
            End If
        ElseIf optWindowsRel.Checked Then
            If optWindowsRelContained.Checked Then
                OpenFileDialogWindows.InitialDirectory = txtDirectoryPath.Text
            ElseIf optWindowsRelExternal.Checked
                OpenFileDialogWindows.InitialDirectory = txtDirectoryPath.Text.Remove(txtDirectoryPath.Text.LastIndexOf(Op))
            End If
        End If
        If imgWindowsCurrent.ImageLocation <> "" Then
            OpenFileDialogWindows.FileName = imgWindowsCurrent.ImageLocation
        End If
        
        If optLinuxAbsolute.Checked Then
            If OpenFileDialogLinux.InitialDirectory = "" Then
                OpenFileDialogLinux.InitialDirectory = txtDirectoryPath.Text
            End If
        ElseIf optLinuxRel.Checked Then
            If optLinuxRelContained.Checked Then
                OpenFileDialogLinux.InitialDirectory = txtDirectoryPath.Text
            ElseIf optLinuxRelExternal.Checked
                OpenFileDialogLinux.InitialDirectory = txtDirectoryPath.Text.Remove(txtDirectoryPath.Text.LastIndexOf(Op))
            End If
        End If
        If imgLinuxCurrent.ImageLocation <> "" Then
            OpenFileDialogLinux.FileName =  imgLinuxCurrent.ImageLocation
        End If
    End Sub
    
    Sub LinuxOptionSelected() Handles optLinuxAbsolute.CheckedChanged, optLinuxRel.CheckedChanged, _
                optLinuxRelContained.CheckedChanged, optLinuxRelExternal.CheckedChanged, optLinuxSystemImage.CheckedChanged
        btnLinuxSave.enabled = false
        If optLinuxAbsolute.Checked Then
            btnLinuxIconSet.Enabled = True
        ElseIf optLinuxRel.Checked Then
            If optLinuxRelContained.Checked Then
                btnLinuxIconSet.Enabled = True
            ElseIf optLinuxRelExternal.Checked
                btnLinuxIconSet.Enabled = True
            Else
                btnLinuxIconSet.Enabled = False
            End If
        ElseIf optLinuxSystemImage.Checked
            btnLinuxIconSet.Enabled = True
        Else
            btnLinuxIconSet.Enabled = False
        End If
    End Sub
    
    Sub LinuxImagePathChanged() Handles txtLinuxImagePath.TextChanged
        If optLinuxAbsolute.Checked Then
            btnLinuxSave.Enabled = True
        ElseIf optLinuxRel.Checked Then
            If optLinuxRelContained.Checked Then
                btnLinuxSave.Enabled = True
            ElseIf optLinuxRelExternal.Checked
                btnLinuxSave.Enabled = True
            Else
                btnLinuxSave.Enabled = False
            End If
        ElseIf optlinuxsystemimage.checked
            btnlinuxsave.enabled = True
        End If
    End Sub
    
    Sub btnLinuxIconSet_Click() Handles btnLinuxIconSet.Click
        SetInitialDirectories
        If OpenFileDialogLinux.ShowDialog = DialogResult.OK Then
            imgLinuxCurrent.ImageLocation = OpenFileDialogLinux.FileName
            btnLinuxSave.Enabled = True
            If optLinuxAbsolute.Checked Then
                txtLinuxImagePath.Text = OpenFileDialogLinux.FileName.Substring(2).Replace("\", "/")
                txtLinuxImagePath.Text = InputBox("Please enter the path in Linux where drive """& OpenFileDialogLinux.FileName.Remove(2)&""" is mounted:", _
                                                  "Linux Drive Mountpoint","/media/"&Environment.GetEnvironmentVariable("UserName")&"/MountPath") & txtLinuxImagePath.Text
            ElseIf optLinuxRelContained.Checked Then
                If txtDirectorypath.text.endswith(":\") Then
                    txtLinuxImagePath.Text = "./" & OpenFileDialogLinux.FileName.Substring(txtDirectoryPath.Text.Length).Replace("\", "/")
                Else
                    txtLinuxImagePath.Text = "./" & OpenFileDialogLinux.FileName.Substring(txtDirectoryPath.Text.Length + 1).Replace("\", "/")
                End If
            ElseIf optLinuxRelExternal.Checked Then
                txtLinuxImagePath.Text = "./.."
                Dim scanText As String
                Try
                    scanText = txtDirectoryPath.Text.Substring(OpenFileDialogLinux.FileName.Replace("\", "/").LastIndexOf("/")+1).Replace("\", "/")
                    If scanText.Contains("/") Then
                        For Each character As Char In scanText
                            If character = "/" Then
                                txtLinuxImagePath.Text &= "/.."
                            End If
                        Next
                    End If
                Catch
                End Try
                txtLinuxImagePath.Text &= OpenFileDialogLinux.FileName.Substring(OpenFileDialogLinux.FileName.Replace("/", "\").LastIndexOf("\")).Replace("\", "/")
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
                Dim FileContents As String() = ReadAllLines(txtDirectoryPath.Text & Op & ".directory")
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
                setattributes(txtDirectoryPath.Text & Op & ".directory", io.fileattributes.normal)
                If seticon Then
                    WriteAllLines(txtDirectoryPath.Text & Op & ".directory", FileContents)
                Else
                    If hasheader Then
                        ' account for index -> length conversion, and that less than will have to be less than or equal to without another addition
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
                        FileContents(headerline+1) = "Icon=" & txtLinuxImagePath.Text
                        WriteAllLines(txtDirectoryPath.Text & Op & ".directory", FileContents)
                    Else
                        appendalltext(txtDirectoryPath.Text & Op & ".directory", vbnewline &"[Desktop Entry]"&vbNewLine &"Icon="& txtLinuxImagePath.Text &vbnewline)
                    End If
                End If
            Else
                WriteAllText(txtDirectoryPath.Text & Op & ".directory", "[Desktop Entry]" & vbNewLine & "Icon=" & txtLinuxImagePath.Text)
                btnLinuxOpenDataFile.Enabled = True
            End If
            SetAttributes(txtDirectoryPath.Text & Op & ".directory", IO.FileAttributes.Hidden)
        Catch ex As exception
            ErrorParser(ex)
        End Try
        ParseFiles(txtDirectoryPath.Text)
    End Sub
    
    Sub btnLinuxOpenDataFile_Click() Handles btnLinuxOpenDataFile.Click
        If Exists(txtDirectoryPath.Text & Op & ".directory") Then
            If chkcustomeditor.checked Then
                Process.Start(txteditorpath.text, txtDirectoryPath.Text & Op & ".directory")
            Else
                If op="\" Then
                    Process.Start(Environment.GetEnvironmentVariable("windir") & "\notepad.exe", txtDirectoryPath.Text & "\.directory")
                Else
                    Process.Start(txtDirectoryPath.Text & "/.directory") 'Environment.GetEnvironmentVariable("windir") & "/notepad.exe", 
                End If
            End If
        Else
            btnLinuxOpenDataFile.Enabled = False
        End If
    End Sub
    
    Sub chkLinuxHidden_CheckedChanged() Handles chkLinuxHidden.Click 'CheckedChanged
        Try
            If chkLinuxHidden.Checked Then
                RemoveAttribute(txtDirectoryPath.Text & Op & ".directory", FileAttribute.Hidden)
            Else
                AddAttribute(txtDirectoryPath.Text & Op & ".directory", FileAttribute.Hidden)
            End If
        Catch ex As exception
            ErrorParser(ex)
        End Try
        ParseFiles(txtDirectoryPath.Text)
    End Sub
    
    Sub chkLinuxSystem_CheckedChanged() Handles chkLinuxSystem.Click 'CheckedChanged
        Try
            If chkLinuxSystem.Checked Then
                RemoveAttribute(txtDirectoryPath.Text & Op & ".directory", FileAttribute.System)
            Else
                AddAttribute(txtDirectoryPath.Text & Op & ".directory", FileAttribute.System)
            End If
        Catch ex As exception
            ErrorParser(ex)
        End Try
        ParseFiles(txtDirectoryPath.Text)
    End Sub
    
    Sub chkCustomEditor_Click() Handles chkCustomEditor.Click
        txtEditorPath.Enabled = False
        btnEditorPathCustom.Text = "Edit..."
        If chkCustomEditor.Checked Then
            If txtEditorPath.Text = "" Then btnEditorBrowse_Click()
        End If
        WriteConfig(configFilePath)
    End Sub
    
    Sub btnEditorBrowse_Click() Handles btnEditorBrowse.Click
        If Op = "\" Then
            OpenFileDialogEditor.InitialDirectory = Environment.GetEnvironmentVariable("ProgramFiles")
            OpenFileDialogEditor.Filter = "Applications|*.exe"
            OpenFileDialogEditor.DefaultExt = "exe"
        Else
            OpenFileDialogEditor.InitialDirectory = "\usr\bin"
            OpenFileDialogEditor.Filter = "Executables|*"
            OpenFileDialogEditor.AddExtension = false
        End If
        If OpenFileDialogEditor.ShowDialog = DialogResult.OK Then
            txtEditorPath.Text = OpenFileDialogEditor.FileName
            chkCustomEditor.Checked = True
        Else
            If txtEditorPath.Text = "" Then chkCustomEditor.Checked = False
        End If
        WriteConfig(configFilePath)
    End Sub
    
    Sub btnEditorPathCustom_Click() Handles btnEditorPathCustom.Click
        If btnEditorPathCustom.Text = "Edit..." Then
            txtEditorPath.Enabled = True
            btnEditorPathCustom.Text = "Save"
        Else
            If Exists(txtEditorPath.Text) Then
                WriteConfig(configFilePath)
                txtEditorPath.Enabled = False
                btnEditorPathCustom.Text = "Edit..."
                chkCustomEditor.Checked = True
            Else
                MsgBox("""" & txtEditorPath.Text & """ doesn't exist!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub
    
    ''' <summary>Adds the specified System.IO.FileAttributes to the file at the specified path</summary>
    ''' <param name="path">The path to the file.</param>
    ''' <param name="fileAttributes">The attributes to add.</param>
    Sub AddAttribute(path As String, fileAttribute As FileAttributes)
        SetAttributes(path, GetAttributes(path) + fileAttribute)
    End Sub
    
    ''' <summary>Removes the specified System.IO.FileAttributes from the file at the specified path</summary>
    ''' <param name="path">The path to the file.</param>
    ''' <param name="fileAttribute">The attributes to remove.</param>
    Sub RemoveAttribute(path As String, fileAttribute As FileAttributes)
        SetAttributes(path, GetAttributes(path) - fileAttribute)
    End Sub
    
    Sub ErrorParser(ex As Exception)
        If ex.GetType.ToString = "System.UnauthorizedAccessException" AndAlso _
          Not New WindowsPrincipal(WindowsIdentity.GetCurrent).IsInRole(WindowsBuiltInRole.Administrator) Then
            if op = "\" then
                If MsgBox(ex.message & vbnewline & vbnewline & "Try launching DirectoryImage As Administrator?", _
                        MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "Access denied!") = MsgBoxResult.Yes Then
                    CreateObject("Shell.Application").ShellExecute(Application.StartupPath & Op & Process.GetCurrentProcess.ProcessName & ".exe", _
                        """" & txtDirectoryPath.Text & """", "", "runas")
                    Application.Exit
                End If
            Else
                If MsgBox(ex.message & vbnewline & vbnewline & "Try launching DirectoryImage As root?", _
                        MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "Access denied!") = MsgBoxResult.Yes Then
                    Process.Start("kdesudo", "mono " & Application.StartupPath & Op & Process.GetCurrentProcess.ProcessName & ".exe """ & txtDirectoryPath.Text & """")
                    Application.Exit
                End If
            End If
        Else
            If MsgBox("There was an error! Error message: " & ex.Message & vbNewLine & "Show full stacktrace? (For sending to developer/making bugreport)", _
                MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "Error!") = MsgBoxresult.Yes Then
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
    
    Sub btnWindowsProperties_Click() Handles btnWindowsProperties.Click
        If Op = "\" Then
            Dim info As New ShellExecuteInfo
            info.cbSize = Marshal.SizeOf(info)
            info.fMask = 12
            info.lpVerb = "properties"
            info.lpParameters = windowsCustomizeTab
            info.lpFile = txtDirectoryPath.Text
            If ShellExecuteEx(info) = False Then
                MsgBox("Could not open properties window!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub
    
    Sub btnWindowsProperties_MouseUp(sender As Object, e As MouseEventArgs) Handles btnWindowsProperties.MouseUp
        If e.Button = MouseButtons.Right Then
            Dim tmpInput = InputBox("Enter customise tab name:", "Set customise tab name", windowsCustomizeTab)
            If tmpInput <> "" Then
                windowsCustomizeTab = tmpInput
            End If
        End If
    End Sub
    
    ' https://stackoverflow.com/a/1936957/2999220
    <DllImport("shell32.dll", CharSet := CharSet.Auto)> _
    Private Shared Function ShellExecuteEx(ByRef lpExecInfo As ShellExecuteInfo) As Boolean
    End Function
    <StructLayout(LayoutKind.Sequential, CharSet := CharSet.Auto)> _
    Public Structure ShellExecuteInfo
        Public cbSize As Integer
        Public fMask As UInteger
        Public hwnd As IntPtr
        <MarshalAs(UnmanagedType.LPTStr)> _
        Public lpVerb As String
        <MarshalAs(UnmanagedType.LPTStr)> _
        Public lpFile As String
        <MarshalAs(UnmanagedType.LPTStr)> _
        Public lpParameters As String
        <MarshalAs(UnmanagedType.LPTStr)> _
        Public lpDirectory As String
        Public nShow As Integer
        Public hInstApp As IntPtr
        Public lpIDList As IntPtr
        <MarshalAs(UnmanagedType.LPTStr)> _
        Public lpClass As String
        Public hkeyClass As IntPtr
        Public dwHotKey As UInteger
        Public hIcon As IntPtr
        Public hProcess As IntPtr
    End Structure
    
    Private Sub ReadConfig(path As String)
        Dim reader As XmlReader = XmlReader.Create(path)
        Try
            reader.Read()
        Catch ex As XmlException
            reader.Close
            Exit Sub
        End Try
        
        If reader.IsStartElement() AndAlso reader.Name = "DirectoryImage" Then
            If reader.Read AndAlso reader.IsStartElement() AndAlso reader.Name = "Settings" Then
                Dim attribute As String
                While reader.IsStartElement
                    If reader.Read AndAlso reader.IsStartElement() Then
                        If reader.Name = "CustomEditor" Then
                            attribute = reader("path")
                            If attribute IsNot Nothing Then
                                txtEditorPath.Text = attribute
                            End If
                            
                            attribute = reader("enabled")
                            If attribute IsNot Nothing Then
                                chkCustomEditor.Checked = attribute
                            End If
                        ElseIf reader.Name = "CustomizeTab" Then
                            attribute = reader("name")
                            If attribute IsNot Nothing Then
                                windowsCustomizeTab = attribute
                            End If
                        End If
                    End If
                End While
            End If
        End If
        
        reader.Close
    End Sub
    
    Private Sub WriteConfig(path As String)
        Dim XMLwSettings As New XmlWriterSettings()
        XMLwSettings.Indent = True
        Dim writer As XmlWriter = XmlWriter.Create(path, XMLwSettings)
        
        writer.WriteStartDocument()
        writer.WriteStartElement("DirectoryImage")
        
        writer.WriteStartElement("Settings")
            writer.WriteStartElement("CustomEditor")
                writer.WriteAttributeString("path", txtEditorPath.Text)
                writer.WriteAttributeString("enabled", chkCustomEditor.Checked)
            writer.WriteEndElement()
            writer.WriteStartElement("CustomizeTab")
                writer.WriteAttributeString("name", windowsCustomizeTab)
            writer.WriteEndElement()
        writer.WriteEndElement()
        
        writer.WriteEndElement()
        writer.WriteEndDocument()
        
        writer.Close
    End Sub
End Class
