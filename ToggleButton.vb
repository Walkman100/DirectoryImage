' 
'Copyright (c) 2011 BinaryConstruct
' 
'This source is subject to the Microsoft Public License.
'See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
'All other rights reserved.
'
'THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
'EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
'WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
' 


Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Text
Imports System.Windows.Forms
Imports System.IO

Namespace BCCL.UI.WinForms
    Public Class ToggleButton
        Inherits Button
        #Region "Events"
        <Browsable(True)> _
        <EditorBrowsable(EditorBrowsableState.Always)> _
        <Category("Action")> _
        <Description("Triggers the CheckState is changed.")> _
        Public Event CheckStateChanged As EventHandler
        Protected Overridable Sub OnCheckStateChanged(e As EventArgs)
            RaiseEvent CheckStateChanged(Me, e)

            Select Case _CheckState
                Case CheckState.Unchecked
                    If Me.Checked Then
                        Me.Checked = False
                    End If
                    Exit Select
                Case CheckState.Checked
                    If Not Me.Checked Then
                        Me.Checked = True
                    End If
                    Exit Select
                Case CheckState.Indeterminate
                    If Not Me.Checked Then
                        Me.Checked = True
                    End If
                    Exit Select
            End Select

        End Sub

        <Browsable(True)> _
        <EditorBrowsable(EditorBrowsableState.Always)> _
        <Category("Action")> _
        <Description("Triggers the CheckState is changed.")> _
        Public Event CheckedChanged As EventHandler
        Protected Overridable Sub OnCheckedChanged(e As EventArgs)
            RaiseEvent CheckedChanged(Me, e)

            If _Checked Then
                If Me.CheckState <> CheckState.Checked Then
                    Me.CheckState = CheckState.Checked
                End If
            Else
                If Me.CheckState <> CheckState.Unchecked Then
                    Me.CheckState = CheckState.Unchecked
                End If
            End If
        End Sub

        #End Region

        #Region "Properties"
        Private _Checked As Boolean = False
        <Browsable(True)> _
        <EditorBrowsable(EditorBrowsableState.Always)> _
        <Category("Appearance")> _
        <Description("Gets or Sets the if the ToggleButton is Checked.")> _
        <DefaultValue(False)> _
        Public Property Checked() As Boolean
            Get
                Return _Checked
            End Get
            Set
                _Checked = value
                OnCheckedChanged(New EventArgs())
                SetBG()
                Me.Invalidate()
            End Set
        End Property

        Private _CheckState As CheckState = CheckState.Unchecked
        <Browsable(True)> _
        <EditorBrowsable(EditorBrowsableState.Always)> _
        <Category("Appearance")> _
        <Description("Gets or sets the CheckState of the ToggleButton")> _
        <DefaultValue(GetType(CheckState), "Unchecked")> _
        Public Property CheckState() As CheckState
            Get
                Return _CheckState
            End Get
            Set
                _CheckState = value
                OnCheckStateChanged(New EventArgs())
                SetBG()
                Me.Invalidate()
            End Set
        End Property

        Private _ThreeState As Boolean = False
        <Browsable(True)> _
        <EditorBrowsable(EditorBrowsableState.Always)> _
        <Category("Behavior")> _
        <Description("Gets or Sets the if the ToggleButton is three state.")> _
        <DefaultValue(False)> _
        Public Property ThreeState() As Boolean
            Get
                Return _ThreeState
            End Get
            Set
                _ThreeState = value
                SetBG()
                Me.Invalidate()
            End Set
        End Property

        #Region "Appearance"

        Private _ColorChecked As Color
        <Browsable(True)> _
        <EditorBrowsable(EditorBrowsableState.Always)> _
        <Category("Appearance")> _
        <Description("Gets or Sets the Checked Color")> _
        <DefaultValue(GetType(Color), "Gold")> _
        Public Property ColorChecked() As Color
            Get
                Return _ColorChecked
            End Get
            Set
                _ColorChecked = value
                SetBG()
                Me.Invalidate()
            End Set
        End Property
        Private _ColorIntermediate As Color
        <Browsable(True)> _
        <EditorBrowsable(EditorBrowsableState.Always)> _
        <Category("Appearance")> _
        <Description("Gets or Sets the Intermediate Color")> _
        <DefaultValue(GetType(Color), "Gray")> _
        Public Property ColorIntermediate() As Color
            Get
                Return _ColorIntermediate
            End Get
            Set
                _ColorIntermediate = value
                SetBG()
                Me.Invalidate()
            End Set
        End Property

        Private _BackColor As Color
        <Browsable(True)> _
        <EditorBrowsable(EditorBrowsableState.Always)> _
        <Category("Appearance")> _
        <Description("Gets or Sets the Unchecked Color")> _
        <DefaultValue(GetType(Color), "Transparent")> _
        Public Overrides Property BackColor() As Color
            Get
                Return _BackColor
            End Get
            Set
                _BackColor = value
                SetBG()
                Me.Invalidate()
            End Set
        End Property
        #End Region
        #End Region

        Public Sub New()
            SetBG()
        End Sub


        Protected Overridable Sub SetBG()
            Me.BackgroundImageLayout = ImageLayout.Stretch

            Dim drawArea As New Rectangle(0, 0, Me.ClientRectangle.Width, Me.ClientRectangle.Height)

            Dim bg As New Bitmap(Me.ClientRectangle.Width, Me.ClientRectangle.Height)
            Dim g As Graphics = Graphics.FromImage(bg)
            g.Clear(Color.Transparent)
            Select Case Me.CheckState
                Case CheckState.Checked
                    g.FillRectangle(New LinearGradientBrush(drawArea, SystemColors.ButtonHighlight, _ColorChecked, 90), drawArea)

                    Exit Select
                Case CheckState.Indeterminate
                    g.FillRectangle(New LinearGradientBrush(drawArea, SystemColors.ButtonHighlight, _ColorIntermediate, 90), drawArea)
                    Exit Select
                Case Else
                    g.FillRectangle(New LinearGradientBrush(drawArea, SystemColors.ButtonHighlight, Me.BackColor, 90), drawArea)
                    Exit Select
            End Select
            g.Dispose()

            Me.BackgroundImage = bg

            'if (this.Checked)
            '    this.BackgroundImage = bg;
            'else
            '    this.BackgroundImage = null;

        End Sub

        Protected Overrides Sub OnClick(e As EventArgs)
            If _ThreeState Then
                Select Case _CheckState
                    Case CheckState.Unchecked
                        Me.CheckState = CheckState.Checked
                        Exit Select
                    Case CheckState.Checked
                        Me.CheckState = CheckState.Indeterminate
                        Exit Select
                    Case CheckState.Indeterminate
                        Me.CheckState = CheckState.Unchecked
                        Exit Select
                    Case Else
                        Me.CheckState = CheckState.Unchecked
                        Exit Select
                End Select
            Else
                Select Case _CheckState
                    Case CheckState.Unchecked
                        Me.CheckState = CheckState.Checked
                        Exit Select
                    Case CheckState.Checked
                        Me.CheckState = CheckState.Unchecked
                        Exit Select
                    Case Else
                        Me.CheckState = CheckState.Unchecked
                        Exit Select
                End Select
            End If

            MyBase.OnClick(e)
        End Sub
    End Class
End Namespace
