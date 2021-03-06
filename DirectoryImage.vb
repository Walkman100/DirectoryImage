Imports System.IO.File
Imports System.Runtime.InteropServices
Public Class DirectoryImage
    Dim configFileName As String = "DirectoryImage.xml"
    Dim configFilePath As String = ""
    Dim windowsCustomizeTab As String = "Customize"
    
    Sub LoadDirectoryImage() Handles MyBase.Load
        lblVersion.Text = My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Build
        If Environment.GetEnvironmentVariable("OS") = "Windows_NT" Then
            If Not       Directory.Exists(Path.Combine(Environment.GetEnvironmentVariable("AppData"), "WalkmanOSS")) Then
                Directory.CreateDirectory(Path.Combine(Environment.GetEnvironmentVariable("AppData"), "WalkmanOSS"))
            End If
            configFilePath =              Path.Combine(Environment.GetEnvironmentVariable("AppData"), "WalkmanOSS", configFileName)
            
            If WalkmanLib.GetWindowsVersion >= WindowsVersion.Windows8 Then
                windowsCustomizeTab = "Customise"
            End If
        Else
            If Not       Directory.Exists(Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".config", "WalkmanOSS")) Then
                Directory.CreateDirectory(Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".config", "WalkmanOSS"))
            End If
            configFilePath =              Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".config", "WalkmanOSS", configFileName)
            
            grpWindows.Location = New System.Drawing.Point(12, 210)
            grpLinux.Location = New System.Drawing.Point(12, 38)
            btnWindowsProperties.Visible = False
            btnWindowsPickIcon.Visible = False
        End If
        
        If       File.Exists(Path.Combine(Application.StartupPath, configFileName)) Then
            configFilePath = Path.Combine(Application.StartupPath, configFileName)
        ElseIf File.Exists(configFileName) Then
            configFilePath = (New FileInfo(configFileName)).FullName
        End If
        
        If File.Exists(configFilePath) Then
            ReadConfig(configFilePath)
        End If
        
        If WalkmanLib.IsAdmin Then Me.Text = "[Admin] Windows and Linux directory image setter"
        
        If My.Application.CommandLineArgs.Count = 0 Then
            timerDelayedBrowse.Start
        Else
            txtDirectoryPath.Text = My.Application.CommandLineArgs(0)
            grpLinux.Enabled = True
            grpWindows.Enabled = True
            
            If txtDirectoryPath.Text.EndsWith("""") Then
                txtDirectoryPath.Text = txtDirectoryPath.Text.Remove(txtDirectoryPath.Text.Length - 1) & IO.Path.DirectorySeparatorChar
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
    
    ' ==================== Reading Files ====================
    
    Dim alreadyGotIcon, lookingForIconIndex As Boolean
    Sub ParseFiles(Directory As String)
        If Directory.EndsWith(":\") Then
            btnWindowsOpenDataFile.Text = "Open Autorun.inf..."
            If Exists(Path.Combine(Directory, "Autorun.inf")) Then
                btnWindowsOpenDataFile.Enabled = True
                chkWindowsHidden.Enabled = True
                chkWindowsSystem.Enabled = True
                chkWindowsHidden.Checked = GetAttributes(Path.Combine(Directory, "Autorun.inf")).HasFlag(IO.FileAttributes.Hidden)
                chkWindowsSystem.Checked = GetAttributes(Path.Combine(Directory, "Autorun.inf")).HasFlag(IO.FileAttributes.System)
                For Each line In ReadLines(Path.Combine(Directory, "Autorun.inf"))
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
            If Exists(Path.Combine(Directory, "desktop.ini")) Then
                btnWindowsOpenDataFile.Enabled = True
                chkWindowsHidden.Enabled = True
                chkWindowsSystem.Enabled = True
                chkWindowsHidden.Checked = GetAttributes(Path.Combine(Directory, "desktop.ini")).HasFlag(IO.FileAttributes.Hidden)
                chkWindowsSystem.Checked = GetAttributes(Path.Combine(Directory, "desktop.ini")).HasFlag(IO.FileAttributes.System)
                alreadyGotIcon = False
                lookingForIconIndex = False
                For Each line In ReadLines(Path.Combine(Directory, "desktop.ini"))
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
        
        If Exists(Path.Combine(Directory, ".directory")) Then
            btnLinuxOpenDataFile.Enabled = True
            chkLinuxHidden.Enabled = True
            chkLinuxSystem.Enabled = True
            chkLinuxHidden.Checked = GetAttributes(Path.Combine(Directory, ".directory")).HasFlag(IO.FileAttributes.Hidden)
            chkLinuxSystem.Checked = GetAttributes(Path.Combine(Directory, ".directory")).HasFlag(IO.FileAttributes.System)
            For Each line In ReadLines(Path.Combine(Directory, ".directory"))
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
    
    ' ==================== Parsing paths for absolute/relative ====================
    
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
            imgWindowsCurrent.ImageLocation = Path.Combine(txtDirectoryPath.Text, txtWindowsIconPath.Text.Replace("\", "/"))
        Else
            optWindowsRel.Checked = True
            optWindowsRelContained.Checked = True
            imgWindowsCurrent.ImageLocation = Path.Combine(txtDirectoryPath.Text, txtWindowsIconPath.Text.Replace("\", "/"))
        End If
        
        If imgWindowsCurrent.ImageLocation.EndsWith(",0") Then
            If Environment.GetEnvironmentVariable("OS") <> "Windows_NT" Then Threading.Thread.Sleep(100) ' otherwise it crashes with "System.NotSupportedException: WebClient does not support concurrent I/O operations." at this point
            imgWindowsCurrent.ImageLocation = imgWindowsCurrent.ImageLocation.Remove(imgWindowsCurrent.ImageLocation.Length-2)
        End If
        
        If Environment.GetEnvironmentVariable("OS") = "Windows_NT" AndAlso imgWindowsCurrent.ImageLocation.StartsWith("%") Then
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
                imgWindowsCurrent.Image = Drawing.Icon.ExtractAssociatedIcon(imgWindowsCurrent.ImageLocation).ToBitmap
            Catch
                Dim filePath As String = imgWindowsCurrent.ImageLocation
                Dim iconIndex As Integer = 0
                
                If filePath.Contains(",") Then
                    If IsNumeric(filePath.Substring( filePath.LastIndexOf(",") +1 )) Then
                        iconIndex = filePath.Substring( filePath.LastIndexOf(",") +1 )
                        
                        filePath = filePath.Remove(filePath.LastIndexOf(","))
                    End If
                End If
                
                Try
                    imgWindowsCurrent.Image = WalkmanLib.ExtractIconByIndex(filePath, iconIndex, imgWindowsCurrent.Width).ToBitmap
                Catch
                End Try
            End Try
        End If
    End Sub
    
    Sub ParseLinux
        If txtLinuxImagePath.Text.StartsWith("/", True, Nothing) Then
            optLinuxAbsolute.Checked = True
            If Environment.GetEnvironmentVariable("OS") = "Windows_NT" Then
                imgLinuxCurrent.ImageLocation = txtLinuxImagePath.Text
            Else
                LinuxPathToWindowsPath.ShowDialog '<- That sets the image
            End If
        ElseIf txtLinuxImagePath.Text.StartsWith("./../", True, Nothing) Then
            optLinuxRel.Checked = True
            optLinuxRelExternal.Checked = True ' no Path.Combine below because the image path will begin with \, and that causes Path.Combine to just use the second part
            imgLinuxCurrent.ImageLocation = txtDirectoryPath.Text & txtLinuxImagePath.Text.Substring(1).Replace("/", "\")
        ElseIf txtLinuxImagePath.Text.StartsWith("./", True, Nothing) Then
            optLinuxRel.Checked = True
            optLinuxRelContained.Checked = True ' no Path.Combine below because the image path will begin with \, and that causes Path.Combine to just use the second part
            imgLinuxCurrent.ImageLocation = txtDirectoryPath.Text & txtLinuxImagePath.Text.Substring(1).Replace("/", "\")
        Else
            optLinuxSystemImage.Checked = True
            
            If Environment.GetEnvironmentVariable("OS") <> "Windows_NT" Then
                Dim tmpString As String = Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".local/share/icons/hicolor/256x256/apps")
                
                If OokiiDialogsLoaded() Then
                    OokiiInputBox(tmpString, "Enter Location", "System images location:")
                                ' tmpString above is ByRef, so OokiiInputBox() updates it
                Else
                    tmpString = InputBox("System images location:", "Enter Location", tmpString)
                End If
                
                imgLinuxCurrent.ImageLocation = Path.Combine(tmpString, txtLinuxImagePath.Text)
            End If
        End If
    End Sub
    
    ' ==================== GUI actions - Windows ====================
    
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
    
    Sub optWindowsRel_CheckedChanged() Handles optWindowsRel.CheckedChanged
        grpWindowsRel.Enabled = optWindowsRel.Checked
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
    
    ' selecting icon
    Sub btnWindowsIconSet_Click() Handles btnWindowsIconSet.Click
        SetInitialDirectories
        If OpenFileDialogWindows.ShowDialog = DialogResult.OK Then
            imgWindowsCurrent.ImageLocation = OpenFileDialogWindows.FileName
            btnWindowsSave.Enabled = True
            If optWindowsAbsolute.Checked Then
                If Environment.GetEnvironmentVariable("OS") = "Windows_NT" Then
                    txtWindowsIconPath.Text = OpenFileDialogWindows.FileName
                Else
                    txtWindowsIconPath.Text = OpenFileDialogWindows.FileName.Replace("/", "\")
                    
                    LinuxPathToWindowsDrive.ShowDialog
                End If
            ElseIf optWindowsRelContained.Checked Then
                If txtDirectoryPath.Text.EndsWith(":\") Then
                    txtWindowsIconPath.Text = OpenFileDialogWindows.FileName.Substring(txtDirectoryPath.Text.Length)
                Else
                    txtWindowsIconPath.Text = OpenFileDialogWindows.FileName.Substring(txtDirectoryPath.Text.Length + 1).Replace("/", "\")
                End If
            ElseIf optWindowsRelExternal.Checked Then
                txtWindowsIconPath.Text = ".."
                Dim scanText As String
                Try
                    scanText = txtDirectoryPath.Text.Substring(OpenFileDialogWindows.FileName.Replace("/", "\").LastIndexOf("\") +1).Replace("/", "\")
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
    
    ' Writing windows icon file
    Dim lineNo, headerLine As Byte
    Dim SetIcon, HasHeader As Boolean
    Sub btnWindowsSave_Click() Handles btnWindowsSave.Click
        Try
            If txtDirectoryPath.Text.EndsWith(":\") Then
                If Exists(Path.Combine(txtDirectoryPath.Text, "Autorun.inf")) Then
                    lineNo = 0
                    SetIcon = False
                    HasHeader = False
                    Dim FileContents As String() = ReadAllLines(Path.Combine(txtDirectoryPath.Text, "Autorun.inf"))
                    For Each line As String In FileContents
                        If line.StartsWith("Icon=", True, Nothing) Then
                            FileContents(lineNo) = "Icon=" & txtWindowsIconPath.Text
                            SetIcon = True
                        ElseIf line = "[autorun]" Then
                            HasHeader = True
                            headerLine = lineNo
                        End If
                        lineNo += 1
                    Next
                    WalkmanLib.SetAttribute(Path.Combine(txtDirectoryPath.Text, "Autorun.inf"), FileAttributes.Normal, AddressOf ErrorParser)
                    If SetIcon Then
                        WriteAllLines(Path.Combine(txtDirectoryPath.Text, "Autorun.inf"), FileContents)
                    Else
                        If HasHeader Then
                            If FileContents.Length < headerLine +2 Then
                                Array.Resize(FileContents, FileContents.Length+1)
                            Else
                                Do Until FileContents(headerLine + 1) = ""
                                    headerLine += 1
                                    If FileContents.Length < headerLine +2 Then
                                        Array.Resize(FileContents, FileContents.Length +1)
                                    End If
                                Loop
                            End If
                            FileContents(headerLine +1) = "Icon=" & txtWindowsIconPath.Text
                            WriteAllLines(Path.Combine(txtDirectoryPath.Text, "Autorun.inf"), FileContents)
                        Else
                            AppendAllText(Path.Combine(txtDirectoryPath.Text, "Autorun.inf"), _
                                vbNewLine & "[.ShellClassInfo]" & vbNewLine & "Icon=" & txtWindowsIconPath.Text & vbNewLine)
                        End If
                    End If
                Else
                    WriteAllText(Path.Combine(txtDirectoryPath.Text, "Autorun.inf"), "[autorun]" & vbNewLine & "Icon=" & txtWindowsIconPath.Text)
                    btnWindowsOpenDataFile.Enabled = True
                End If
                WalkmanLib.AddAttribute(Path.Combine(txtDirectoryPath.Text, "Autorun.inf"), IO.FileAttributes.Hidden, AddressOf ErrorParser)
            Else
                If Exists(Path.Combine(txtDirectoryPath.Text, "desktop.ini")) Then
                    lineNo = 0
                    SetIcon = False
                    HasHeader = False
                    Dim FileContents As String() = ReadAllLines(Path.Combine(txtDirectoryPath.Text, "desktop.ini"))
                    For Each line In FileContents
                        If line.StartsWith("IconResource=", True, Nothing) Then
                            FileContents(lineNo) = "IconResource=" & txtWindowsIconPath.Text
                            SetIcon = True
                        ElseIf line.StartsWith("IconFile=", True, Nothing) Then
                            FileContents(lineNo) = Nothing
                        ElseIf line.StartsWith("IconIndex=", True, Nothing) Then
                            FileContents(lineNo) = Nothing
                        ElseIf line = "[.ShellClassInfo]" Then
                            HasHeader = True
                            headerLine = lineNo
                        End If
                        lineNo += 1
                    Next
                    WalkmanLib.SetAttribute(Path.Combine(txtDirectoryPath.Text, "desktop.ini"), FileAttributes.Normal, AddressOf ErrorParser)
                    If SetIcon Then
                        WriteAllLines(Path.Combine(txtDirectoryPath.Text, "desktop.ini"), FileContents)
                    Else
                        If HasHeader Then
                            If FileContents.Length < headerLine +2 Then
                                Array.Resize(FileContents, FileContents.Length +1)
                            Else
                                Do Until FileContents(headerLine +1) = ""
                                    headerLine += 1
                                    If FileContents.Length < headerLine +2 Then
                                        Array.Resize(FileContents, FileContents.Length +1)
                                    End If
                                Loop
                            End If
                            FileContents(headerLine +1) = "IconResource=" & txtWindowsIconPath.Text
                            WriteAllLines(Path.Combine(txtDirectoryPath.Text, "desktop.ini"), FileContents)
                        Else
                            AppendAllText(Path.Combine(txtDirectoryPath.Text, "desktop.ini"), _
                                vbNewLine & "[.ShellClassInfo]" & vbNewLine & "IconResource=" & txtWindowsIconPath.Text & vbNewLine)
                        End If
                    End If
                Else
                    WriteAllText(Path.Combine(txtDirectoryPath.Text, "desktop.ini"), "[.ShellClassInfo]" & vbNewLine & "IconResource=" & txtWindowsIconPath.Text)
                    btnWindowsOpenDataFile.Enabled = True
                End If
                WalkmanLib.AddAttribute(Path.Combine(txtDirectoryPath.Text, "desktop.ini"), IO.FileAttributes.Hidden, AddressOf ErrorParser)
            End If
        Catch ex As Exception
            ErrorParser(ex)
        End Try
        ParseFiles(txtDirectoryPath.Text)
    End Sub
    
    Sub btnWindowsOpenDataFile_Click() Handles btnWindowsOpenDataFile.Click
        If txtDirectoryPath.Text.EndsWith(":\") Then
            If Exists(Path.Combine(txtDirectoryPath.Text, "Autorun.inf")) Then
                If chkCustomEditor.Checked Then
                    Process.Start(txtEditorPath.Text, """" & Path.Combine(txtDirectoryPath.Text, "Autorun.inf") & """")
                Else
                    If Environment.GetEnvironmentVariable("OS") = "Windows_NT" Then
                        Process.Start(Path.Combine(Environment.GetEnvironmentVariable("windir"), "notepad.exe"), """" & Path.Combine(txtDirectoryPath.Text, "Autorun.inf") & """")
                    Else
                        Process.Start("xdg-open", """" & Path.Combine(txtDirectoryPath.Text & "Autorun.inf") & """")
                    End If
                End If
            Else
                btnWindowsOpenDataFile.Enabled = False
            End If
        Else
            If Exists(Path.Combine(txtDirectoryPath.Text, "desktop.ini")) Then
                If chkCustomEditor.Checked Then
                    Process.Start(txtEditorPath.Text, """" & Path.Combine(txtDirectoryPath.Text, "desktop.ini") & """")
                Else
                    If Environment.GetEnvironmentVariable("OS") = "Windows_NT" Then
                        Process.Start(Path.Combine(Environment.GetEnvironmentVariable("windir"), "notepad.exe"), """" & Path.Combine(txtDirectoryPath.Text, "desktop.ini") & """")
                    Else
                        Process.Start("xdg-open", """" & Path.Combine(txtDirectoryPath.Text, "desktop.ini") & """")
                    End If
                End If
            Else
                btnWindowsOpenDataFile.Enabled = False
            End If
        End If
    End Sub
    
    Sub chkWindowsHidden_CheckedChanged() Handles chkWindowsHidden.Click 'CheckedChanged
        If txtDirectoryPath.Text.EndsWith(":\") Then
            WalkmanLib.ChangeAttribute(Path.Combine(txtDirectoryPath.Text, "Autorun.inf"), FileAttributes.Hidden, chkWindowsHidden.Checked, AddressOf ErrorParser)
        Else
            WalkmanLib.ChangeAttribute(Path.Combine(txtDirectoryPath.Text, "desktop.ini"), FileAttributes.Hidden, chkWindowsHidden.Checked, AddressOf ErrorParser)
        End If
        ParseFiles(txtDirectoryPath.Text)
    End Sub
    
    Sub chkWindowsSystem_CheckedChanged() Handles chkWindowsSystem.Click 'CheckedChanged
        If txtDirectoryPath.Text.EndsWith(":\") Then
            WalkmanLib.ChangeAttribute(Path.Combine(txtDirectoryPath.Text, "Autorun.inf"), FileAttributes.System, chkWindowsSystem.Checked, AddressOf ErrorParser)
        Else
            WalkmanLib.ChangeAttribute(Path.Combine(txtDirectoryPath.Text, "desktop.ini"), FileAttributes.System, chkWindowsSystem.Checked, AddressOf ErrorParser)
        End If
        ParseFiles(txtDirectoryPath.Text)
    End Sub
    
    ' ==================== Called before selecting icon for both types ====================
    Sub SetInitialDirectories()
        If optWindowsAbsolute.Checked Then
            If OpenFileDialogWindows.InitialDirectory = "" Then
                OpenFileDialogWindows.InitialDirectory = txtDirectoryPath.Text
            End If
        ElseIf optWindowsRel.Checked Then
            If optWindowsRelContained.Checked Then
                OpenFileDialogWindows.InitialDirectory = txtDirectoryPath.Text
            ElseIf optWindowsRelExternal.Checked
                OpenFileDialogWindows.InitialDirectory = txtDirectoryPath.Text.Remove(txtDirectoryPath.Text.LastIndexOf(Path.DirectorySeparatorChar))
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
                OpenFileDialogLinux.InitialDirectory = txtDirectoryPath.Text.Remove(txtDirectoryPath.Text.LastIndexOf(Path.DirectorySeparatorChar))
            End If
        End If
        If imgLinuxCurrent.ImageLocation <> "" Then
            OpenFileDialogLinux.FileName = imgLinuxCurrent.ImageLocation
        End If
    End Sub
    
    ' ==================== GUI actions - Linux ====================
    
    Sub LinuxOptionSelected() Handles optLinuxAbsolute.CheckedChanged, optLinuxRel.CheckedChanged, _
                optLinuxRelContained.CheckedChanged, optLinuxRelExternal.CheckedChanged, optLinuxSystemImage.CheckedChanged
        btnLinuxSave.Enabled = False
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
    
    Sub optLinuxRel_CheckedChanged() Handles optLinuxRel.CheckedChanged
        grpLinuxRel.Enabled = optLinuxRel.Checked
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
    
    ' selecting icon
    Sub btnLinuxIconSet_Click() Handles btnLinuxIconSet.Click
        SetInitialDirectories
        If OpenFileDialogLinux.ShowDialog = DialogResult.OK Then
            imgLinuxCurrent.ImageLocation = OpenFileDialogLinux.FileName
            btnLinuxSave.Enabled = True
            If optLinuxAbsolute.Checked Then
                If Environment.GetEnvironmentVariable("OS") = "Windows_NT" Then
                    txtLinuxImagePath.Text = OpenFileDialogLinux.FileName.Substring(2).Replace("\", "/")
                    
                    Dim tmpString As String = "/media/" & Environment.GetEnvironmentVariable("UserName") & "/MountPath"
                    If OokiiDialogsLoaded() Then
                        OokiiInputBox(tmpString, "Linux Drive Mountpoint", "Please enter the path in Linux where drive """& OpenFileDialogLinux.FileName.Remove(2)&""" is mounted:")
                                    ' tmpString above is ByRef, so OokiiInputBox() updates it
                    Else
                        tmpString = InputBox("Please enter the path in Linux where drive """& OpenFileDialogLinux.FileName.Remove(2)&""" is mounted:", "Linux Drive Mountpoint", tmpString)
                    End If
                    
                    txtLinuxImagePath.Text = tmpString & txtLinuxImagePath.Text
                Else
                    txtLinuxImagePath.Text = OpenFileDialogLinux.FileName
                End If
            ElseIf optLinuxRelContained.Checked Then
                If txtDirectoryPath.Text.EndsWith(":\") Then
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
    
    ' Writing linux icon file
    Sub btnLinuxSave_Click() Handles btnLinuxSave.Click
        Try
            If Exists(Path.Combine(txtDirectoryPath.Text, ".directory")) Then
                lineNo = 0 ' Actually the index of the line
                SetIcon = False  ' Index starts at 0, count/length/number starts at 1
                HasHeader = false
                Dim FileContents As String() = ReadAllLines(Path.Combine(txtDirectoryPath.Text, ".directory"))
                For Each line As String In FileContents
                    If line.StartsWith("Icon=", True, Nothing) Then
                        FileContents(lineNo) = "Icon=" & txtLinuxImagePath.Text
                        SetIcon = true
                    ElseIf line = "[Desktop Entry]" Then
                        HasHeader = True
                        headerLine = lineNo ' therefore it is the header index not line
                    End If
                    lineNo += 1
                Next
                WalkmanLib.SetAttribute(Path.Combine(txtDirectoryPath.Text, ".directory"), FileAttributes.Normal, AddressOf ErrorParser)
                If SetIcon Then
                    WriteAllLines(Path.Combine(txtDirectoryPath.Text, ".directory"), FileContents)
                Else
                    If HasHeader Then
                        ' account for index -> length conversion, and that less than will have to be less than or equal to without another addition
                        If FileContents.Length < headerLine +2 Then
                            Array.Resize(FileContents, FileContents.Length +1)
                        Else
                            Do Until FileContents(headerLine +1) = ""
                                headerLine += 1
                                If FileContents.Length < headerLine +2 Then
                                    Array.Resize(FileContents, FileContents.Length +1)
                                End If
                            Loop
                        End If
                        FileContents(headerLine +1) = "Icon=" & txtLinuxImagePath.Text
                        WriteAllLines(Path.Combine(txtDirectoryPath.Text, ".directory"), FileContents)
                    Else
                        AppendAllText(Path.Combine(txtDirectoryPath.Text, ".directory"), _
                            vbNewLine & "[Desktop Entry]" & vbNewLine & "Icon=" & txtLinuxImagePath.Text & vbNewLine)
                    End If
                End If
            Else
                WriteAllText(Path.Combine(txtDirectoryPath.Text, ".directory"), "[Desktop Entry]" & vbNewLine & "Icon=" & txtLinuxImagePath.Text)
                btnLinuxOpenDataFile.Enabled = True
            End If
            WalkmanLib.AddAttribute(Path.Combine(txtDirectoryPath.Text, ".directory"), FileAttributes.Hidden, AddressOf ErrorParser)
        Catch ex As Exception
            ErrorParser(ex)
        End Try
        ParseFiles(txtDirectoryPath.Text)
    End Sub
    
    Sub btnLinuxOpenDataFile_Click() Handles btnLinuxOpenDataFile.Click
        If Exists(Path.Combine(txtDirectoryPath.Text, ".directory")) Then
            If chkCustomEditor.Checked Then
                Process.Start(txtEditorPath.Text, """" & Path.Combine(txtDirectoryPath.Text, ".directory") & """")
            Else
                If Environment.GetEnvironmentVariable("OS") = "Windows_NT" Then
                    Process.Start(Path.Combine(Environment.GetEnvironmentVariable("windir"), "notepad.exe"), """" & Path.Combine(txtDirectoryPath.Text, ".directory") & """")
                Else
                    Process.Start("xdg-open", """" & Path.Combine(txtDirectoryPath.Text, ".directory") & """")
                End If
            End If
        Else
            btnLinuxOpenDataFile.Enabled = False
        End If
    End Sub
    
    Sub chkLinuxHidden_CheckedChanged() Handles chkLinuxHidden.Click 'CheckedChanged
        WalkmanLib.ChangeAttribute(Path.Combine(txtDirectoryPath.Text, ".directory"), FileAttributes.Hidden, chkLinuxHidden.Checked, AddressOf ErrorParser)
        ParseFiles(txtDirectoryPath.Text)
    End Sub
    
    Sub chkLinuxSystem_CheckedChanged() Handles chkLinuxSystem.Click 'CheckedChanged
        WalkmanLib.ChangeAttribute(Path.Combine(txtDirectoryPath.Text, ".directory"), FileAttributes.System, chkLinuxSystem.Checked, AddressOf ErrorParser)
        ParseFiles(txtDirectoryPath.Text)
    End Sub
    
    ' ==================== Custom Editor managing ====================
    
    Sub chkCustomEditor_Click() Handles chkCustomEditor.Click
        txtEditorPath.Enabled = False
        btnEditorPathCustom.Text = "Edit..."
        If chkCustomEditor.Checked Then
            If txtEditorPath.Text = "" Then btnEditorBrowse_Click()
        End If
        WriteConfig(configFilePath)
    End Sub
    
    Sub btnEditorBrowse_Click() Handles btnEditorBrowse.Click
        If Environment.GetEnvironmentVariable("OS") = "Windows_NT" Then
            OpenFileDialogEditor.InitialDirectory = Environment.GetEnvironmentVariable("ProgramFiles")
            OpenFileDialogEditor.Filter = "Applications|*.exe"
            OpenFileDialogEditor.DefaultExt = "exe"
        Else
            OpenFileDialogEditor.InitialDirectory = "/usr/bin"
            OpenFileDialogEditor.Filter = "Executables|*"
            OpenFileDialogEditor.AddExtension = False
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
    
    ' ==================== Extra Windows buttons ====================
    
    Sub btnWindowsProperties_Click() Handles btnWindowsProperties.Click
        If Environment.GetEnvironmentVariable("OS") = "Windows_NT" Then
            If WalkmanLib.ShowProperties(txtDirectoryPath.Text, windowsCustomizeTab) = False Then
                MsgBox("Could not open properties window!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub
    
    Sub btnWindowsProperties_MouseUp(sender As Object, e As MouseEventArgs) Handles btnWindowsProperties.MouseUp
        If e.Button = MouseButtons.Right Then
            Dim tmpInput As String = windowsCustomizeTab
            
            If OokiiDialogsLoaded() Then
                If OokiiInputBox(tmpInput, "Set customise tab name", "Enter customise tab name:") <> DialogResult.OK Then
                    tmpInput = "" ' tmpInput above is ByRef, so OokiiInputBox() updates it
                End If
            Else
                tmpInput = InputBox("Enter customise tab name:", "Set customise tab name", tmpInput)
            End If
            
            If tmpInput <> "" Then
                windowsCustomizeTab = tmpInput
                WriteConfig(configFilePath)
            End If
        End If
    End Sub
    
    Sub btnWindowsPickIcon_Click() Handles btnWindowsPickIcon.Click
        Dim selectedFilePath As String = txtWindowsIconPath.Text
        Dim selectedIconIndex As Integer
        
        If selectedFilePath.Contains(",") Then
            If IsNumeric(selectedFilePath.Substring( selectedFilePath.LastIndexOf(",") +1 )) Then
                selectedIconIndex = selectedFilePath.Substring( selectedFilePath.LastIndexOf(",") +1 )
                
                selectedFilePath = selectedFilePath.Remove(selectedFilePath.LastIndexOf(","))
            End If
        End If
        
        If optWindowsRel.Checked Then
            If optWindowsRelContained.Checked Then
                selectedFilePath = txtDirectoryPath.Text & Path.DirectorySeparatorChar & selectedFilePath
            ElseIf optWindowsRelExternal.Checked
                selectedFilePath = txtDirectoryPath.Text.Remove(txtDirectoryPath.Text.LastIndexOf(Path.DirectorySeparatorChar)) & Path.DirectorySeparatorChar & selectedFilePath
            End If
        End If
        
        If WalkmanLib.PickIconDialogShow(selectedFilePath, selectedIconIndex, Me.Handle) Then
            Dim combinedIconPath As String = selectedFilePath & "," & selectedIconIndex
            
            imgWindowsCurrent.ImageLocation = Environment.ExpandEnvironmentVariables(combinedIconPath)
            btnWindowsSave.Enabled = True
            If optWindowsAbsolute.Checked Then
                If Environment.GetEnvironmentVariable("OS") = "Windows_NT" Then
                    txtWindowsIconPath.Text = combinedIconPath
                End If
            ElseIf optWindowsRelContained.Checked Then
                If txtDirectoryPath.Text.EndsWith(":\") Then
                    txtWindowsIconPath.Text = combinedIconPath.Substring(txtDirectoryPath.Text.Length)
                Else
                    txtWindowsIconPath.Text = combinedIconPath.Substring(txtDirectoryPath.Text.Length + 1).Replace("/", "\")
                End If
            ElseIf optWindowsRelExternal.Checked Then
                txtWindowsIconPath.Text = ".."
                Dim scanText As String
                Try
                    scanText = txtDirectoryPath.Text.Substring(combinedIconPath.Replace("/", "\").LastIndexOf("\") +1).Replace("/", "\")
                    If scanText.Contains("\") Then
                        For Each character As Char In scanText
                            If character = "\" Then
                                txtWindowsIconPath.Text &= "\.."
                            End If
                        Next
                    End If
                Catch
                End Try
                txtWindowsIconPath.Text &= combinedIconPath.Substring(combinedIconPath.Replace("/", "\").LastIndexOf("\"))
            Else
                MsgBox("Please select an option!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub
    
    Sub btnExit_Click() Handles btnExit.Click
        Application.Exit()
    End Sub
    
    ' ==================== Helper methods ====================
    
    Sub ErrorParser(ex As Exception)
        If ex.GetType.ToString = "System.UnauthorizedAccessException" AndAlso Not WalkmanLib.IsAdmin Then
            If Environment.GetEnvironmentVariable("OS") = "Windows_NT" Then
                If MsgBox(ex.Message & vbNewLine & vbNewLine & "Try launching DirectoryImage As Administrator?", _
                        MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "Access denied!") = MsgBoxResult.Yes Then
                    WalkmanLib.RunAsAdmin(Path.Combine(Application.StartupPath, Process.GetCurrentProcess.ProcessName & ".exe"), """" & txtDirectoryPath.Text & """")
                    Application.Exit
                End If
            Else
                If MsgBox(ex.message & vbnewline & vbnewline & "Try launching DirectoryImage As root?", _
                        MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "Access denied!") = MsgBoxResult.Yes Then
                    Process.Start("kdesudo", "mono " & Path.Combine(Application.StartupPath, Process.GetCurrentProcess.ProcessName & ".exe") & " """ & txtDirectoryPath.Text & """")
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
    
    Function OokiiInputBox(ByRef input As String, Optional windowTitle As String = Nothing, Optional header As String = Nothing, Optional content As String = Nothing) As DialogResult
        Dim ooInput = New Ookii.Dialogs.InputDialog
        ooInput.WindowTitle = windowTitle
        ooInput.MainInstruction = header
        ooInput.Content = content
        ooInput.Input = input
        
        Dim returnResult = ooInput.ShowDialog()
        input = ooInput.Input
        Return returnResult
    End Function
    Function OokiiDialogsLoaded() As Boolean
        Try
            OokiiDialogsLoadedDelegate()
            Return True
        Catch ex As FileNotFoundException
            If ex.Message.StartsWith("Could not load file or assembly 'PropertiesDotNet-Ookii.Dialogs") = False Then
                MsgBox("Unexpected error loading Ookii.Dialogs.dll!" & vbNewLine & vbNewLine & ex.Message, MsgBoxStyle.Exclamation)
            End If
            Return False
        Catch ex As Exception
            MsgBox("Unexpected error loading Ookii.Dialogs.dll!" & vbNewLine & vbNewLine & ex.Message, MsgBoxStyle.Exclamation)
            Return False
        End Try
    End Function
    Sub OokiiDialogsLoadedDelegate() ' because calling a not found class will fail the caller of the method not directly in the method
        Dim test = Ookii.Dialogs.TaskDialogIcon.Information
    End Sub
    
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
