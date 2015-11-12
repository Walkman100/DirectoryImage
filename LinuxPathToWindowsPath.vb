<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class LinuxPathToWindowsPath
    Inherits System.Windows.Forms.Form
    Private components As System.ComponentModel.IContainer
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If components IsNot Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.cbxWinLetter = New System.Windows.Forms.ComboBox()
        Me.txtMntLen = New System.Windows.Forms.NumericUpDown()
        Me.txtMnt = New System.Windows.Forms.TextBox()
        Me.lblMntLen = New System.Windows.Forms.Label()
        Me.lblMnt = New System.Windows.Forms.Label()
        Me.lblWinLetter = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        CType(Me.txtMntLen,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SuspendLayout
        'cbxWinLetter
        Me.cbxWinLetter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.cbxWinLetter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxWinLetter.FormattingEnabled = true
        Me.cbxWinLetter.Items.AddRange(New Object() {"A:", "B:", "C:", "D:", "E:", "F:", "G:", "H:", "I:", "J:", "K:", "L:", "M:", "N:", "O:", "P:"})
        Me.cbxWinLetter.Location = New System.Drawing.Point(12, 103)
        Me.cbxWinLetter.Name = "cbxWinLetter"
        Me.cbxWinLetter.Size = New System.Drawing.Size(255, 21)
        Me.cbxWinLetter.TabIndex = 0
        'txtMntLen
        Me.txtMntLen.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtMntLen.Location = New System.Drawing.Point(12, 25)
        Me.txtMntLen.Name = "txtMntLen"
        Me.txtMntLen.Size = New System.Drawing.Size(255, 20)
        Me.txtMntLen.TabIndex = 1
        Me.txtMntLen.Value = New Decimal(New Integer() {24, 0, 0, 0})
        'txtMnt
        Me.txtMnt.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtMnt.Location = New System.Drawing.Point(12, 64)
        Me.txtMnt.Name = "txtMnt"
        Me.txtMnt.ReadOnly = true
        Me.txtMnt.Size = New System.Drawing.Size(255, 20)
        Me.txtMnt.TabIndex = 2
        'lblMntLen
        Me.lblMntLen.AutoSize = true
        Me.lblMntLen.Location = New System.Drawing.Point(12, 9)
        Me.lblMntLen.Name = "lblMntLen"
        Me.lblMntLen.Size = New System.Drawing.Size(111, 13)
        Me.lblMntLen.TabIndex = 3
        Me.lblMntLen.Text = "Length of mount path:"
        'lblMnt
        Me.lblMnt.AutoSize = true
        Me.lblMnt.Location = New System.Drawing.Point(12, 48)
        Me.lblMnt.Name = "lblMnt"
        Me.lblMnt.Size = New System.Drawing.Size(40, 13)
        Me.lblMnt.TabIndex = 4
        Me.lblMnt.Text = "Result:"
        'lblWinLetter
        Me.lblWinLetter.AutoSize = true
        Me.lblWinLetter.Location = New System.Drawing.Point(12, 87)
        Me.lblWinLetter.Name = "lblWinLetter"
        Me.lblWinLetter.Size = New System.Drawing.Size(108, 13)
        Me.lblWinLetter.TabIndex = 5
        Me.lblWinLetter.Text = "Windows Drive letter:"
        'btnSave
        Me.btnSave.Enabled = false
        Me.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnSave.Location = New System.Drawing.Point(12, 130)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 6
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = true
        'btnBrowse
        Me.btnBrowse.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnBrowse.Location = New System.Drawing.Point(93, 130)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(93, 23)
        Me.btnBrowse.TabIndex = 7
        Me.btnBrowse.Text = "Re-Browse..."
        Me.btnBrowse.UseVisualStyleBackColor = true
        'btnCancel
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(192, 130)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = true
        'LinuxPathToWindowsPath
        Me.AcceptButton = Me.btnSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(279, 165)
        Me.ControlBox = false
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.lblWinLetter)
        Me.Controls.Add(Me.lblMnt)
        Me.Controls.Add(Me.lblMntLen)
        Me.Controls.Add(Me.txtMnt)
        Me.Controls.Add(Me.txtMntLen)
        Me.Controls.Add(Me.cbxWinLetter)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = false
        Me.MinimizeBox = false
        Me.Name = "LinuxPathToWindowsPath"
        Me.ShowIcon = false
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Set Windows drive letter for Linux mount point:"
        CType(Me.txtMntLen,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)
        Me.PerformLayout
    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents lblWinLetter As System.Windows.Forms.Label
    Friend WithEvents lblMnt As System.Windows.Forms.Label
    Friend WithEvents lblMntLen As System.Windows.Forms.Label
    Friend WithEvents txtMnt As System.Windows.Forms.TextBox
    Friend WithEvents txtMntLen As System.Windows.Forms.NumericUpDown
    Friend WithEvents cbxWinLetter As System.Windows.Forms.ComboBox
    
    Sub LinuxPathToWindowsPath_Load() Handles MyBase.Load
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
    
    Sub btnSave_Click() Handles btnSave.Click
        DirectoryImage.imgLinuxCurrent.ImageLocation = cbxWinLetter.Text & DirectoryImage.txtLinuxImagePath.Text.Substring(txtMntLen.Value).Replace("/", "\")
        Me.Close
    End Sub
    
    Sub btnBrowse_Click() Handles btnBrowse.Click
        Me.Close
        DirectoryImage.btnDirectoryBrowse_Click()
    End Sub
    
    Sub btnCancel_Click() Handles btnCancel.Click
        Me.Close
    End Sub
End Class
