; DirectoryImage Installer NSIS Script
; get NSIS at http://nsis.sourceforge.net/Download
; As a program that all Power PC users should have, Notepad++ is recommended to edit this file

Icon "My Project\Shell32(#326).ico"
Caption "DirectoryImage Installer"
Name "DirectoryImage"
AutoCloseWindow true
ShowInstDetails show

LicenseBkColor /windows
LicenseData "LICENSE.md"
LicenseForceSelection checkbox "I have read and understand this notice"
LicenseText "Please read the notice below before installing DirectoryImage. If you understand the notice, click the checkbox below and click Next."

InstallDir $PROGRAMFILES\WalkmanOSS

OutFile "bin\Release\DirectoryImage-Installer.exe"

; Pages

Page license
Page components
Page directory
Page instfiles
UninstPage uninstConfirm
UninstPage instfiles

; Sections

Section "Executable & Uninstaller"
  SectionIn RO
  SetOutPath $INSTDIR
  File "bin\Release\DirectoryImage.exe"
  WriteUninstaller "DirectoryImage-Uninst.exe"
SectionEnd

Section "Start Menu Shortcuts"
  CreateDirectory "$SMPROGRAMS\WalkmanOSS"
  CreateShortCut "$SMPROGRAMS\WalkmanOSS\DirectoryImage.lnk" "$INSTDIR\DirectoryImage.exe" "" "$INSTDIR\DirectoryImage.exe" "" "" "" "DirectoryImage"
  CreateShortCut "$SMPROGRAMS\WalkmanOSS\Uninstall DirectoryImage.lnk" "$INSTDIR\DirectoryImage-Uninst.exe" "" "" "" "" "" "Uninstall DirectoryImage"
  ;Syntax for CreateShortCut: link.lnk target.file [parameters [icon.file [icon_index_number [start_options [keyboard_shortcut [description]]]]]]
SectionEnd

Section "Desktop Shortcut"
  CreateShortCut "$DESKTOP\DirectoryImage.lnk" "$INSTDIR\DirectoryImage.exe" "" "$INSTDIR\DirectoryImage.exe" "" "" "" "DirectoryImage"
SectionEnd

Section "Quick Launch Shortcut"
  CreateShortCut "$QUICKLAUNCH\DirectoryImage.lnk" "$INSTDIR\DirectoryImage.exe" "" "$INSTDIR\DirectoryImage.exe" "" "" "" "DirectoryImage"
SectionEnd

Section "Add DirectoryImage to context menu"
  DeleteRegKey HKCR "Directory\shell\DirImage" ; Remove old context menu item, 'Folder' also covers drives
  
  WriteRegStr HKCR "Folder\shell\DirImage" "" "Set Directory Image..."
  WriteRegStr HKCR "Folder\shell\DirImage" "Icon" "$INSTDIR\DirectoryImage.exe"
  WriteRegStr HKCR "Folder\shell\DirImage\command" "" "$\"$INSTDIR\DirectoryImage.exe$\" $\"%1$\""
SectionEnd

; Functions

Function .onInit
  MessageBox MB_YESNO "This will install DirectoryImage. Do you wish to continue?" IDYES gogogo
    Abort
  gogogo:
  SetShellVarContext all
  SetAutoClose true
FunctionEnd

Function .onInstSuccess
    MessageBox MB_YESNO "Install Succeeded! Open ReadMe?" IDNO NoReadme
      ExecShell "open" "https://github.com/Walkman100/DirectoryImage/blob/master/README.md#directoryimage-"
    NoReadme:
FunctionEnd

; Uninstaller

Section "Uninstall"
  Delete "$INSTDIR\DirectoryImage-Uninst.exe"   ; Remove Application Files
  Delete "$INSTDIR\DirectoryImage.exe"
  RMDir "$INSTDIR"
  
  Delete "$SMPROGRAMS\WalkmanOSS\DirectoryImage.lnk"   ; Remove Start Menu Shortcuts & Folder
  Delete "$SMPROGRAMS\WalkmanOSS\Uninstall DirectoryImage.lnk"
  RMDir "$SMPROGRAMS\WalkmanOSS"
  
  Delete "$DESKTOP\DirectoryImage.lnk"   ; Remove Desktop Shortcut
  Delete "$QUICKLAUNCH\DirectoryImage.lnk"   ; Remove Quick Launch Shortcut
  
  DeleteRegKey HKCR "Directory\shell\DirImage" ; Remove old context menu item
  DeleteRegKey HKCR "Folder\shell\DirImage" ; Remove context menu item
SectionEnd

; Uninstaller Functions

Function un.onInit
    MessageBox MB_YESNO "This will uninstall DirectoryImage. Continue?" IDYES NoAbort
      Abort ; causes uninstaller to quit.
    NoAbort:
    SetShellVarContext all
    SetAutoClose true
FunctionEnd

Function un.onUninstFailed
    MessageBox MB_OK "Uninstall Cancelled"
FunctionEnd

Function un.onUninstSuccess
    MessageBox MB_OK "Uninstall Completed"
FunctionEnd
