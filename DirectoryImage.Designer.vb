<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DirectoryImage
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtDirectoryPath = New System.Windows.Forms.TextBox()
        Me.txtDirectoryBrowse = New System.Windows.Forms.Button()
        Me.grpWindows = New System.Windows.Forms.GroupBox()
        Me.btnWindowsOpenDataFile = New System.Windows.Forms.Button()
        Me.optWindowsRel = New System.Windows.Forms.RadioButton()
        Me.grpWindowsRel = New System.Windows.Forms.GroupBox()
        Me.optWindowsRelContained = New System.Windows.Forms.RadioButton()
        Me.optWindowsRelExternal = New System.Windows.Forms.RadioButton()
        Me.btnWindowsIconSet = New System.Windows.Forms.Button()
        Me.txtWindowsIconPath = New System.Windows.Forms.TextBox()
        Me.optWindowsAbsolute = New System.Windows.Forms.RadioButton()
        Me.imgWindowsCurrent = New System.Windows.Forms.PictureBox()
        Me.lblWindowsCurrent = New System.Windows.Forms.Label()
        Me.grpLinux = New System.Windows.Forms.GroupBox()
        Me.btnLinuxOpenDataFile = New System.Windows.Forms.Button()
        Me.optLinuxRel = New System.Windows.Forms.RadioButton()
        Me.grpLinuxRel = New System.Windows.Forms.GroupBox()
        Me.optLinuxRelContained = New System.Windows.Forms.RadioButton()
        Me.optLinuxRelExternal = New System.Windows.Forms.RadioButton()
        Me.btnLinuxIconSet = New System.Windows.Forms.Button()
        Me.txtLinuxImagePath = New System.Windows.Forms.TextBox()
        Me.optLinuxAbsolute = New System.Windows.Forms.RadioButton()
        Me.imgLinuxCurrent = New System.Windows.Forms.PictureBox()
        Me.lblLinuxCurrent = New System.Windows.Forms.Label()
        Me.FolderBrowserDialog = New System.Windows.Forms.FolderBrowserDialog()
        Me.OpenFileDialogWindows = New System.Windows.Forms.OpenFileDialog()
        Me.OpenFileDialogLinux = New System.Windows.Forms.OpenFileDialog()
        Me.grpWindows.SuspendLayout
        Me.grpWindowsRel.SuspendLayout
        CType(Me.imgWindowsCurrent,System.ComponentModel.ISupportInitialize).BeginInit
        Me.grpLinux.SuspendLayout
        Me.grpLinuxRel.SuspendLayout
        CType(Me.imgLinuxCurrent,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SuspendLayout
        '
        'txtDirectoryPath
        '
        Me.txtDirectoryPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
                        Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtDirectoryPath.Location = New System.Drawing.Point(12, 12)
        Me.txtDirectoryPath.Name = "txtDirectoryPath"
        Me.txtDirectoryPath.Size = New System.Drawing.Size(431, 20)
        Me.txtDirectoryPath.TabIndex = 0
        '
        'txtDirectoryBrowse
        '
        Me.txtDirectoryBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtDirectoryBrowse.Location = New System.Drawing.Point(449, 10)
        Me.txtDirectoryBrowse.Name = "txtDirectoryBrowse"
        Me.txtDirectoryBrowse.Size = New System.Drawing.Size(75, 23)
        Me.txtDirectoryBrowse.TabIndex = 1
        Me.txtDirectoryBrowse.Text = "Browse..."
        Me.txtDirectoryBrowse.UseVisualStyleBackColor = true
        '
        'grpWindows
        '
        Me.grpWindows.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
                        Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.grpWindows.Controls.Add(Me.btnWindowsOpenDataFile)
        Me.grpWindows.Controls.Add(Me.optWindowsRel)
        Me.grpWindows.Controls.Add(Me.grpWindowsRel)
        Me.grpWindows.Controls.Add(Me.btnWindowsIconSet)
        Me.grpWindows.Controls.Add(Me.txtWindowsIconPath)
        Me.grpWindows.Controls.Add(Me.optWindowsAbsolute)
        Me.grpWindows.Controls.Add(Me.imgWindowsCurrent)
        Me.grpWindows.Controls.Add(Me.lblWindowsCurrent)
        Me.grpWindows.Enabled = false
        Me.grpWindows.Location = New System.Drawing.Point(12, 38)
        Me.grpWindows.Name = "grpWindows"
        Me.grpWindows.Size = New System.Drawing.Size(512, 166)
        Me.grpWindows.TabIndex = 2
        Me.grpWindows.TabStop = false
        Me.grpWindows.Text = "Windows Icon"
        '
        'btnWindowsOpenDataFile
        '
        Me.btnWindowsOpenDataFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnWindowsOpenDataFile.AutoSize = true
        Me.btnWindowsOpenDataFile.Location = New System.Drawing.Point(400, 137)
        Me.btnWindowsOpenDataFile.Name = "btnWindowsOpenDataFile"
        Me.btnWindowsOpenDataFile.Size = New System.Drawing.Size(106, 23)
        Me.btnWindowsOpenDataFile.TabIndex = 10
        Me.btnWindowsOpenDataFile.Text = "Open desktop.ini..."
        Me.btnWindowsOpenDataFile.UseVisualStyleBackColor = true
        '
        'optWindowsRel
        '
        Me.optWindowsRel.AutoSize = true
        Me.optWindowsRel.Location = New System.Drawing.Point(140, 68)
        Me.optWindowsRel.Name = "optWindowsRel"
        Me.optWindowsRel.Size = New System.Drawing.Size(67, 17)
        Me.optWindowsRel.TabIndex = 8
        Me.optWindowsRel.TabStop = true
        Me.optWindowsRel.Text = "Relative:"
        Me.optWindowsRel.UseVisualStyleBackColor = true
        '
        'grpWindowsRel
        '
        Me.grpWindowsRel.Controls.Add(Me.optWindowsRelContained)
        Me.grpWindowsRel.Controls.Add(Me.optWindowsRelExternal)
        Me.grpWindowsRel.Enabled = false
        Me.grpWindowsRel.Location = New System.Drawing.Point(140, 70)
        Me.grpWindowsRel.Name = "grpWindowsRel"
        Me.grpWindowsRel.Size = New System.Drawing.Size(150, 65)
        Me.grpWindowsRel.TabIndex = 9
        Me.grpWindowsRel.TabStop = false
        '
        'optWindowsRelContained
        '
        Me.optWindowsRelContained.AutoSize = true
        Me.optWindowsRelContained.Location = New System.Drawing.Point(6, 19)
        Me.optWindowsRelContained.Name = "optWindowsRelContained"
        Me.optWindowsRelContained.Size = New System.Drawing.Size(113, 17)
        Me.optWindowsRelContained.TabIndex = 2
        Me.optWindowsRelContained.TabStop = true
        Me.optWindowsRelContained.Text = "Contained in folder"
        Me.optWindowsRelContained.UseVisualStyleBackColor = true
        '
        'optWindowsRelExternal
        '
        Me.optWindowsRelExternal.AutoSize = true
        Me.optWindowsRelExternal.Location = New System.Drawing.Point(6, 42)
        Me.optWindowsRelExternal.Name = "optWindowsRelExternal"
        Me.optWindowsRelExternal.Size = New System.Drawing.Size(142, 17)
        Me.optWindowsRelExternal.TabIndex = 7
        Me.optWindowsRelExternal.TabStop = true
        Me.optWindowsRelExternal.Text = "Relative outside of folder"
        Me.optWindowsRelExternal.UseVisualStyleBackColor = true
        '
        'btnWindowsIconSet
        '
        Me.btnWindowsIconSet.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnWindowsIconSet.AutoSize = true
        Me.btnWindowsIconSet.Enabled = false
        Me.btnWindowsIconSet.Location = New System.Drawing.Point(431, 42)
        Me.btnWindowsIconSet.Name = "btnWindowsIconSet"
        Me.btnWindowsIconSet.Size = New System.Drawing.Size(75, 23)
        Me.btnWindowsIconSet.TabIndex = 5
        Me.btnWindowsIconSet.Text = "Set Icon..."
        Me.btnWindowsIconSet.UseVisualStyleBackColor = true
        '
        'txtWindowsIconPath
        '
        Me.txtWindowsIconPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
                        Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtWindowsIconPath.Location = New System.Drawing.Point(140, 19)
        Me.txtWindowsIconPath.Name = "txtWindowsIconPath"
        Me.txtWindowsIconPath.Size = New System.Drawing.Size(366, 20)
        Me.txtWindowsIconPath.TabIndex = 4
        '
        'optWindowsAbsolute
        '
        Me.optWindowsAbsolute.AutoSize = true
        Me.optWindowsAbsolute.Location = New System.Drawing.Point(140, 45)
        Me.optWindowsAbsolute.Name = "optWindowsAbsolute"
        Me.optWindowsAbsolute.Size = New System.Drawing.Size(66, 17)
        Me.optWindowsAbsolute.TabIndex = 3
        Me.optWindowsAbsolute.TabStop = true
        Me.optWindowsAbsolute.Text = "Absolute"
        Me.optWindowsAbsolute.UseVisualStyleBackColor = true
        '
        'imgWindowsCurrent
        '
        Me.imgWindowsCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.imgWindowsCurrent.Location = New System.Drawing.Point(6, 32)
        Me.imgWindowsCurrent.Name = "imgWindowsCurrent"
        Me.imgWindowsCurrent.Size = New System.Drawing.Size(128, 128)
        Me.imgWindowsCurrent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.imgWindowsCurrent.TabIndex = 1
        Me.imgWindowsCurrent.TabStop = false
        '
        'lblWindowsCurrent
        '
        Me.lblWindowsCurrent.AutoSize = true
        Me.lblWindowsCurrent.Location = New System.Drawing.Point(6, 16)
        Me.lblWindowsCurrent.Name = "lblWindowsCurrent"
        Me.lblWindowsCurrent.Size = New System.Drawing.Size(68, 13)
        Me.lblWindowsCurrent.TabIndex = 0
        Me.lblWindowsCurrent.Text = "Current Icon:"
        '
        'grpLinux
        '
        Me.grpLinux.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
                        Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.grpLinux.Controls.Add(Me.btnLinuxOpenDataFile)
        Me.grpLinux.Controls.Add(Me.optLinuxRel)
        Me.grpLinux.Controls.Add(Me.grpLinuxRel)
        Me.grpLinux.Controls.Add(Me.btnLinuxIconSet)
        Me.grpLinux.Controls.Add(Me.txtLinuxImagePath)
        Me.grpLinux.Controls.Add(Me.optLinuxAbsolute)
        Me.grpLinux.Controls.Add(Me.imgLinuxCurrent)
        Me.grpLinux.Controls.Add(Me.lblLinuxCurrent)
        Me.grpLinux.Enabled = false
        Me.grpLinux.Location = New System.Drawing.Point(12, 210)
        Me.grpLinux.Name = "grpLinux"
        Me.grpLinux.Size = New System.Drawing.Size(512, 166)
        Me.grpLinux.TabIndex = 3
        Me.grpLinux.TabStop = false
        Me.grpLinux.Text = "Linux Image"
        '
        'btnLinuxOpenDataFile
        '
        Me.btnLinuxOpenDataFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnLinuxOpenDataFile.AutoSize = true
        Me.btnLinuxOpenDataFile.Location = New System.Drawing.Point(408, 137)
        Me.btnLinuxOpenDataFile.Name = "btnLinuxOpenDataFile"
        Me.btnLinuxOpenDataFile.Size = New System.Drawing.Size(98, 23)
        Me.btnLinuxOpenDataFile.TabIndex = 14
        Me.btnLinuxOpenDataFile.Text = "Open .directory..."
        Me.btnLinuxOpenDataFile.UseVisualStyleBackColor = true
        '
        'optLinuxRel
        '
        Me.optLinuxRel.AutoSize = true
        Me.optLinuxRel.Location = New System.Drawing.Point(140, 68)
        Me.optLinuxRel.Name = "optLinuxRel"
        Me.optLinuxRel.Size = New System.Drawing.Size(67, 17)
        Me.optLinuxRel.TabIndex = 8
        Me.optLinuxRel.TabStop = true
        Me.optLinuxRel.Text = "Relative:"
        Me.optLinuxRel.UseVisualStyleBackColor = true
        '
        'grpLinuxRel
        '
        Me.grpLinuxRel.Controls.Add(Me.optLinuxRelContained)
        Me.grpLinuxRel.Controls.Add(Me.optLinuxRelExternal)
        Me.grpLinuxRel.Enabled = false
        Me.grpLinuxRel.Location = New System.Drawing.Point(140, 70)
        Me.grpLinuxRel.Name = "grpLinuxRel"
        Me.grpLinuxRel.Size = New System.Drawing.Size(150, 65)
        Me.grpLinuxRel.TabIndex = 13
        Me.grpLinuxRel.TabStop = false
        '
        'optLinuxRelContained
        '
        Me.optLinuxRelContained.AutoSize = true
        Me.optLinuxRelContained.Location = New System.Drawing.Point(6, 19)
        Me.optLinuxRelContained.Name = "optLinuxRelContained"
        Me.optLinuxRelContained.Size = New System.Drawing.Size(113, 17)
        Me.optLinuxRelContained.TabIndex = 2
        Me.optLinuxRelContained.TabStop = true
        Me.optLinuxRelContained.Text = "Contained in folder"
        Me.optLinuxRelContained.UseVisualStyleBackColor = true
        '
        'optLinuxRelExternal
        '
        Me.optLinuxRelExternal.AutoSize = true
        Me.optLinuxRelExternal.Location = New System.Drawing.Point(6, 42)
        Me.optLinuxRelExternal.Name = "optLinuxRelExternal"
        Me.optLinuxRelExternal.Size = New System.Drawing.Size(142, 17)
        Me.optLinuxRelExternal.TabIndex = 7
        Me.optLinuxRelExternal.TabStop = true
        Me.optLinuxRelExternal.Text = "Relative outside of folder"
        Me.optLinuxRelExternal.UseVisualStyleBackColor = true
        '
        'btnLinuxIconSet
        '
        Me.btnLinuxIconSet.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnLinuxIconSet.AutoSize = true
        Me.btnLinuxIconSet.Enabled = false
        Me.btnLinuxIconSet.Location = New System.Drawing.Point(431, 42)
        Me.btnLinuxIconSet.Name = "btnLinuxIconSet"
        Me.btnLinuxIconSet.Size = New System.Drawing.Size(75, 23)
        Me.btnLinuxIconSet.TabIndex = 12
        Me.btnLinuxIconSet.Text = "Set Image..."
        Me.btnLinuxIconSet.UseVisualStyleBackColor = true
        '
        'txtLinuxImagePath
        '
        Me.txtLinuxImagePath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
                        Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtLinuxImagePath.Location = New System.Drawing.Point(140, 19)
        Me.txtLinuxImagePath.Name = "txtLinuxImagePath"
        Me.txtLinuxImagePath.Size = New System.Drawing.Size(366, 20)
        Me.txtLinuxImagePath.TabIndex = 11
        '
        'optLinuxAbsolute
        '
        Me.optLinuxAbsolute.AutoSize = true
        Me.optLinuxAbsolute.Location = New System.Drawing.Point(140, 45)
        Me.optLinuxAbsolute.Name = "optLinuxAbsolute"
        Me.optLinuxAbsolute.Size = New System.Drawing.Size(66, 17)
        Me.optLinuxAbsolute.TabIndex = 10
        Me.optLinuxAbsolute.TabStop = true
        Me.optLinuxAbsolute.Text = "Absolute"
        Me.optLinuxAbsolute.UseVisualStyleBackColor = true
        '
        'imgLinuxCurrent
        '
        Me.imgLinuxCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.imgLinuxCurrent.Location = New System.Drawing.Point(6, 32)
        Me.imgLinuxCurrent.Name = "imgLinuxCurrent"
        Me.imgLinuxCurrent.Size = New System.Drawing.Size(128, 128)
        Me.imgLinuxCurrent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.imgLinuxCurrent.TabIndex = 3
        Me.imgLinuxCurrent.TabStop = false
        '
        'lblLinuxCurrent
        '
        Me.lblLinuxCurrent.AutoSize = true
        Me.lblLinuxCurrent.Location = New System.Drawing.Point(6, 16)
        Me.lblLinuxCurrent.Name = "lblLinuxCurrent"
        Me.lblLinuxCurrent.Size = New System.Drawing.Size(76, 13)
        Me.lblLinuxCurrent.TabIndex = 2
        Me.lblLinuxCurrent.Text = "Current Image:"
        '
        'FolderBrowserDialog
        '
        Me.FolderBrowserDialog.Description = "Select a folder to set or view icons for"
        '
        'OpenFileDialogWindows
        '
        Me.OpenFileDialogWindows.DefaultExt = "ico"
        Me.OpenFileDialogWindows.Filter = "Icon Files|*.ico"
        Me.OpenFileDialogWindows.ReadOnlyChecked = true
        Me.OpenFileDialogWindows.Title = "Select an icon to be displayed on Windows"
        '
        'OpenFileDialogLinux
        '
        Me.OpenFileDialogLinux.AddExtension = false
        Me.OpenFileDialogLinux.DefaultExt = "png"
        Me.OpenFileDialogLinux.Filter = "Image files|*.png; *.bmp; *.jpg; *.jpeg; *.ico"
        Me.OpenFileDialogLinux.ReadOnlyChecked = true
        Me.OpenFileDialogLinux.Title = "Select on icon to be displayed on Linux"
        '
        'DirectoryImage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(536, 389)
        Me.Controls.Add(Me.grpLinux)
        Me.Controls.Add(Me.grpWindows)
        Me.Controls.Add(Me.txtDirectoryBrowse)
        Me.Controls.Add(Me.txtDirectoryPath)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "DirectoryImage"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Windows and Linux directory image setter"
        Me.grpWindows.ResumeLayout(false)
        Me.grpWindows.PerformLayout
        Me.grpWindowsRel.ResumeLayout(false)
        Me.grpWindowsRel.PerformLayout
        CType(Me.imgWindowsCurrent,System.ComponentModel.ISupportInitialize).EndInit
        Me.grpLinux.ResumeLayout(false)
        Me.grpLinux.PerformLayout
        Me.grpLinuxRel.ResumeLayout(false)
        Me.grpLinuxRel.PerformLayout
        CType(Me.imgLinuxCurrent,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)
        Me.PerformLayout
    End Sub
    Friend WithEvents OpenFileDialogLinux As System.Windows.Forms.OpenFileDialog
    Friend WithEvents OpenFileDialogWindows As System.Windows.Forms.OpenFileDialog
    Friend WithEvents FolderBrowserDialog As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents btnLinuxOpenDataFile As System.Windows.Forms.Button
    Friend WithEvents btnWindowsOpenDataFile As System.Windows.Forms.Button
    Friend WithEvents lblLinuxCurrent As System.Windows.Forms.Label
    Friend WithEvents imgLinuxCurrent As System.Windows.Forms.PictureBox
    Friend WithEvents optLinuxAbsolute As System.Windows.Forms.RadioButton
    Friend WithEvents txtLinuxImagePath As System.Windows.Forms.TextBox
    Friend WithEvents btnLinuxIconSet As System.Windows.Forms.Button
    Friend WithEvents optLinuxRelExternal As System.Windows.Forms.RadioButton
    Friend WithEvents optLinuxRel As System.Windows.Forms.RadioButton
    Friend WithEvents optLinuxRelContained As System.Windows.Forms.RadioButton
    Friend WithEvents grpLinuxRel As System.Windows.Forms.GroupBox
    Friend WithEvents lblWindowsCurrent As System.Windows.Forms.Label
    Friend WithEvents imgWindowsCurrent As System.Windows.Forms.PictureBox
    Friend WithEvents optWindowsAbsolute As System.Windows.Forms.RadioButton
    Friend WithEvents txtWindowsIconPath As System.Windows.Forms.TextBox
    Friend WithEvents btnWindowsIconSet As System.Windows.Forms.Button
    Friend WithEvents optWindowsRelExternal As System.Windows.Forms.RadioButton
    Friend WithEvents optWindowsRel As System.Windows.Forms.RadioButton
    Friend WithEvents optWindowsRelContained As System.Windows.Forms.RadioButton
    Friend WithEvents grpWindowsRel As System.Windows.Forms.GroupBox
    Friend WithEvents grpLinux As System.Windows.Forms.GroupBox
    Friend WithEvents grpWindows As System.Windows.Forms.GroupBox
    Friend WithEvents txtDirectoryBrowse As System.Windows.Forms.Button
    Friend WithEvents txtDirectoryPath As System.Windows.Forms.TextBox

End Class
