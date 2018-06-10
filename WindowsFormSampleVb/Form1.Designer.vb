<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.toolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ResultTextBox = New System.Windows.Forms.TextBox()
        Me.AllMorphsCheckBox = New System.Windows.Forms.CheckBox()
        Me.PartialCheckBox = New System.Windows.Forms.CheckBox()
        Me.NBestCheckBox = New System.Windows.Forms.CheckBox()
        Me.NBestNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.NBestFlowLayoutPanel = New System.Windows.Forms.FlowLayoutPanel()
        Me.NBestGroupBox = New System.Windows.Forms.GroupBox()
        Me.OutputFormatLatticeRadioButton = New System.Windows.Forms.RadioButton()
        Me.OutputFormatWakatiRadioButton = New System.Windows.Forms.RadioButton()
        Me.OutputFormatDumpRadioButton = New System.Windows.Forms.RadioButton()
        Me.OutputFormatflowLayoutPanel = New System.Windows.Forms.FlowLayoutPanel()
        Me.OutputFormatGroupBox = New System.Windows.Forms.GroupBox()
        Me.LatticeLevel0RadioButton = New System.Windows.Forms.RadioButton()
        Me.LatticeLevel1RadioButton = New System.Windows.Forms.RadioButton()
        Me.LatticeLevel2RadioButton = New System.Windows.Forms.RadioButton()
        Me.LatticeLevelFlowLayoutPanel = New System.Windows.Forms.FlowLayoutPanel()
        Me.LatticeLevelGroupBox = New System.Windows.Forms.GroupBox()
        Me.TargetTextBox = New System.Windows.Forms.TextBox()
        Me.DoAnalyzeButton = New System.Windows.Forms.Button()
        Me.tableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.flowLayoutPanel3 = New System.Windows.Forms.FlowLayoutPanel()
        Me.splitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.statusStrip1 = New System.Windows.Forms.StatusStrip()
        CType(Me.NBestNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.NBestFlowLayoutPanel.SuspendLayout()
        Me.NBestGroupBox.SuspendLayout()
        Me.OutputFormatflowLayoutPanel.SuspendLayout()
        Me.OutputFormatGroupBox.SuspendLayout()
        Me.LatticeLevelFlowLayoutPanel.SuspendLayout()
        Me.LatticeLevelGroupBox.SuspendLayout()
        Me.tableLayoutPanel1.SuspendLayout()
        Me.flowLayoutPanel3.SuspendLayout()
        CType(Me.splitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContainer1.Panel1.SuspendLayout()
        Me.splitContainer1.Panel2.SuspendLayout()
        Me.splitContainer1.SuspendLayout()
        Me.statusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'toolStripStatusLabel1
        '
        Me.toolStripStatusLabel1.Name = "toolStripStatusLabel1"
        Me.toolStripStatusLabel1.Size = New System.Drawing.Size(57, 17)
        Me.toolStripStatusLabel1.Text = "Startup ..."
        '
        'ResultTextBox
        '
        Me.ResultTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ResultTextBox.Location = New System.Drawing.Point(0, 0)
        Me.ResultTextBox.Multiline = True
        Me.ResultTextBox.Name = "ResultTextBox"
        Me.ResultTextBox.ReadOnly = True
        Me.ResultTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.ResultTextBox.Size = New System.Drawing.Size(617, 223)
        Me.ResultTextBox.TabIndex = 0
        Me.ResultTextBox.TabStop = False
        '
        'AllMorphsCheckBox
        '
        Me.AllMorphsCheckBox.AutoSize = True
        Me.AllMorphsCheckBox.Location = New System.Drawing.Point(459, 3)
        Me.AllMorphsCheckBox.Name = "AllMorphsCheckBox"
        Me.AllMorphsCheckBox.Size = New System.Drawing.Size(75, 16)
        Me.AllMorphsCheckBox.TabIndex = 3
        Me.AllMorphsCheckBox.Text = "AllMorphs"
        Me.AllMorphsCheckBox.UseVisualStyleBackColor = True
        '
        'PartialCheckBox
        '
        Me.PartialCheckBox.AutoSize = True
        Me.PartialCheckBox.Location = New System.Drawing.Point(540, 3)
        Me.PartialCheckBox.Name = "PartialCheckBox"
        Me.PartialCheckBox.Size = New System.Drawing.Size(57, 16)
        Me.PartialCheckBox.TabIndex = 4
        Me.PartialCheckBox.Text = "Partial"
        Me.PartialCheckBox.UseVisualStyleBackColor = True
        '
        'NBestCheckBox
        '
        Me.NBestCheckBox.AutoSize = True
        Me.NBestCheckBox.Location = New System.Drawing.Point(3, 3)
        Me.NBestCheckBox.Name = "NBestCheckBox"
        Me.NBestCheckBox.Size = New System.Drawing.Size(15, 14)
        Me.NBestCheckBox.TabIndex = 0
        Me.NBestCheckBox.UseVisualStyleBackColor = True
        '
        'NBestNumericUpDown
        '
        Me.NBestNumericUpDown.AutoSize = True
        Me.NBestNumericUpDown.Enabled = False
        Me.NBestNumericUpDown.Location = New System.Drawing.Point(24, 3)
        Me.NBestNumericUpDown.Maximum = New Decimal(New Integer() {256, 0, 0, 0})
        Me.NBestNumericUpDown.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.NBestNumericUpDown.Name = "NBestNumericUpDown"
        Me.NBestNumericUpDown.Size = New System.Drawing.Size(39, 19)
        Me.NBestNumericUpDown.TabIndex = 1
        Me.NBestNumericUpDown.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'NBestFlowLayoutPanel
        '
        Me.NBestFlowLayoutPanel.AutoSize = True
        Me.NBestFlowLayoutPanel.Controls.Add(Me.NBestCheckBox)
        Me.NBestFlowLayoutPanel.Controls.Add(Me.NBestNumericUpDown)
        Me.NBestFlowLayoutPanel.Location = New System.Drawing.Point(6, 18)
        Me.NBestFlowLayoutPanel.Name = "NBestFlowLayoutPanel"
        Me.NBestFlowLayoutPanel.Size = New System.Drawing.Size(66, 25)
        Me.NBestFlowLayoutPanel.TabIndex = 0
        '
        'NBestGroupBox
        '
        Me.NBestGroupBox.AutoSize = True
        Me.NBestGroupBox.Controls.Add(Me.NBestFlowLayoutPanel)
        Me.NBestGroupBox.Location = New System.Drawing.Point(375, 3)
        Me.NBestGroupBox.Name = "NBestGroupBox"
        Me.NBestGroupBox.Size = New System.Drawing.Size(78, 61)
        Me.NBestGroupBox.TabIndex = 2
        Me.NBestGroupBox.TabStop = False
        Me.NBestGroupBox.Text = "NBest"
        '
        'OutputFormatLatticeRadioButton
        '
        Me.OutputFormatLatticeRadioButton.AutoSize = True
        Me.OutputFormatLatticeRadioButton.Checked = True
        Me.OutputFormatLatticeRadioButton.Location = New System.Drawing.Point(3, 3)
        Me.OutputFormatLatticeRadioButton.Name = "OutputFormatLatticeRadioButton"
        Me.OutputFormatLatticeRadioButton.Size = New System.Drawing.Size(55, 16)
        Me.OutputFormatLatticeRadioButton.TabIndex = 0
        Me.OutputFormatLatticeRadioButton.TabStop = True
        Me.OutputFormatLatticeRadioButton.Text = "lattice"
        Me.OutputFormatLatticeRadioButton.UseVisualStyleBackColor = True
        '
        'OutputFormatWakatiRadioButton
        '
        Me.OutputFormatWakatiRadioButton.AutoSize = True
        Me.OutputFormatWakatiRadioButton.Location = New System.Drawing.Point(64, 3)
        Me.OutputFormatWakatiRadioButton.Name = "OutputFormatWakatiRadioButton"
        Me.OutputFormatWakatiRadioButton.Size = New System.Drawing.Size(56, 16)
        Me.OutputFormatWakatiRadioButton.TabIndex = 1
        Me.OutputFormatWakatiRadioButton.TabStop = True
        Me.OutputFormatWakatiRadioButton.Text = "wakati"
        Me.OutputFormatWakatiRadioButton.UseVisualStyleBackColor = True
        '
        'OutputFormatDumpRadioButton
        '
        Me.OutputFormatDumpRadioButton.AutoSize = True
        Me.OutputFormatDumpRadioButton.Location = New System.Drawing.Point(126, 3)
        Me.OutputFormatDumpRadioButton.Name = "OutputFormatDumpRadioButton"
        Me.OutputFormatDumpRadioButton.Size = New System.Drawing.Size(50, 16)
        Me.OutputFormatDumpRadioButton.TabIndex = 2
        Me.OutputFormatDumpRadioButton.TabStop = True
        Me.OutputFormatDumpRadioButton.Text = "dump"
        Me.OutputFormatDumpRadioButton.UseVisualStyleBackColor = True
        '
        'OutputFormatflowLayoutPanel
        '
        Me.OutputFormatflowLayoutPanel.Controls.Add(Me.OutputFormatLatticeRadioButton)
        Me.OutputFormatflowLayoutPanel.Controls.Add(Me.OutputFormatWakatiRadioButton)
        Me.OutputFormatflowLayoutPanel.Controls.Add(Me.OutputFormatDumpRadioButton)
        Me.OutputFormatflowLayoutPanel.Location = New System.Drawing.Point(9, 18)
        Me.OutputFormatflowLayoutPanel.Name = "OutputFormatflowLayoutPanel"
        Me.OutputFormatflowLayoutPanel.Size = New System.Drawing.Size(179, 22)
        Me.OutputFormatflowLayoutPanel.TabIndex = 0
        '
        'OutputFormatGroupBox
        '
        Me.OutputFormatGroupBox.Controls.Add(Me.OutputFormatflowLayoutPanel)
        Me.OutputFormatGroupBox.Location = New System.Drawing.Point(175, 3)
        Me.OutputFormatGroupBox.Name = "OutputFormatGroupBox"
        Me.OutputFormatGroupBox.Size = New System.Drawing.Size(194, 58)
        Me.OutputFormatGroupBox.TabIndex = 1
        Me.OutputFormatGroupBox.TabStop = False
        Me.OutputFormatGroupBox.Text = "OutputFormat"
        '
        'LatticeLevel0RadioButton
        '
        Me.LatticeLevel0RadioButton.AutoSize = True
        Me.LatticeLevel0RadioButton.Checked = True
        Me.LatticeLevel0RadioButton.Location = New System.Drawing.Point(3, 3)
        Me.LatticeLevel0RadioButton.Name = "LatticeLevel0RadioButton"
        Me.LatticeLevel0RadioButton.Size = New System.Drawing.Size(46, 16)
        Me.LatticeLevel0RadioButton.TabIndex = 0
        Me.LatticeLevel0RadioButton.TabStop = True
        Me.LatticeLevel0RadioButton.Text = "Zero"
        Me.LatticeLevel0RadioButton.UseVisualStyleBackColor = True
        '
        'LatticeLevel1RadioButton
        '
        Me.LatticeLevel1RadioButton.AutoSize = True
        Me.LatticeLevel1RadioButton.Location = New System.Drawing.Point(55, 3)
        Me.LatticeLevel1RadioButton.Name = "LatticeLevel1RadioButton"
        Me.LatticeLevel1RadioButton.Size = New System.Drawing.Size(43, 16)
        Me.LatticeLevel1RadioButton.TabIndex = 1
        Me.LatticeLevel1RadioButton.TabStop = True
        Me.LatticeLevel1RadioButton.Text = "One"
        Me.LatticeLevel1RadioButton.UseVisualStyleBackColor = True
        '
        'LatticeLevel2RadioButton
        '
        Me.LatticeLevel2RadioButton.AutoSize = True
        Me.LatticeLevel2RadioButton.Location = New System.Drawing.Point(104, 3)
        Me.LatticeLevel2RadioButton.Name = "LatticeLevel2RadioButton"
        Me.LatticeLevel2RadioButton.Size = New System.Drawing.Size(44, 16)
        Me.LatticeLevel2RadioButton.TabIndex = 2
        Me.LatticeLevel2RadioButton.TabStop = True
        Me.LatticeLevel2RadioButton.Text = "Two"
        Me.LatticeLevel2RadioButton.UseVisualStyleBackColor = True
        '
        'LatticeLevelFlowLayoutPanel
        '
        Me.LatticeLevelFlowLayoutPanel.Controls.Add(Me.LatticeLevel0RadioButton)
        Me.LatticeLevelFlowLayoutPanel.Controls.Add(Me.LatticeLevel1RadioButton)
        Me.LatticeLevelFlowLayoutPanel.Controls.Add(Me.LatticeLevel2RadioButton)
        Me.LatticeLevelFlowLayoutPanel.Location = New System.Drawing.Point(9, 18)
        Me.LatticeLevelFlowLayoutPanel.Name = "LatticeLevelFlowLayoutPanel"
        Me.LatticeLevelFlowLayoutPanel.Size = New System.Drawing.Size(151, 22)
        Me.LatticeLevelFlowLayoutPanel.TabIndex = 0
        '
        'LatticeLevelGroupBox
        '
        Me.LatticeLevelGroupBox.Controls.Add(Me.LatticeLevelFlowLayoutPanel)
        Me.LatticeLevelGroupBox.Location = New System.Drawing.Point(3, 3)
        Me.LatticeLevelGroupBox.Name = "LatticeLevelGroupBox"
        Me.LatticeLevelGroupBox.Size = New System.Drawing.Size(166, 58)
        Me.LatticeLevelGroupBox.TabIndex = 0
        Me.LatticeLevelGroupBox.TabStop = False
        Me.LatticeLevelGroupBox.Text = "LatticeLevel"
        '
        'TargetTextBox
        '
        Me.TargetTextBox.AcceptsReturn = True
        Me.TargetTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TargetTextBox.Location = New System.Drawing.Point(3, 76)
        Me.TargetTextBox.Multiline = True
        Me.TargetTextBox.Name = "TargetTextBox"
        Me.TargetTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TargetTextBox.Size = New System.Drawing.Size(611, 93)
        Me.TargetTextBox.TabIndex = 0
        '
        'DoAnalyzeButton
        '
        Me.DoAnalyzeButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DoAnalyzeButton.Location = New System.Drawing.Point(3, 175)
        Me.DoAnalyzeButton.Name = "DoAnalyzeButton"
        Me.DoAnalyzeButton.Size = New System.Drawing.Size(75, 23)
        Me.DoAnalyzeButton.TabIndex = 1
        Me.DoAnalyzeButton.Text = "Analyze"
        Me.DoAnalyzeButton.UseVisualStyleBackColor = True
        '
        'tableLayoutPanel1
        '
        Me.tableLayoutPanel1.ColumnCount = 1
        Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tableLayoutPanel1.Controls.Add(Me.TargetTextBox, 0, 1)
        Me.tableLayoutPanel1.Controls.Add(Me.DoAnalyzeButton, 0, 2)
        Me.tableLayoutPanel1.Controls.Add(Me.flowLayoutPanel3, 0, 0)
        Me.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.tableLayoutPanel1.Name = "tableLayoutPanel1"
        Me.tableLayoutPanel1.RowCount = 3
        Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tableLayoutPanel1.Size = New System.Drawing.Size(617, 201)
        Me.tableLayoutPanel1.TabIndex = 0
        '
        'flowLayoutPanel3
        '
        Me.flowLayoutPanel3.AutoSize = True
        Me.flowLayoutPanel3.Controls.Add(Me.LatticeLevelGroupBox)
        Me.flowLayoutPanel3.Controls.Add(Me.OutputFormatGroupBox)
        Me.flowLayoutPanel3.Controls.Add(Me.NBestGroupBox)
        Me.flowLayoutPanel3.Controls.Add(Me.AllMorphsCheckBox)
        Me.flowLayoutPanel3.Controls.Add(Me.PartialCheckBox)
        Me.flowLayoutPanel3.Location = New System.Drawing.Point(3, 3)
        Me.flowLayoutPanel3.Name = "flowLayoutPanel3"
        Me.flowLayoutPanel3.Size = New System.Drawing.Size(600, 67)
        Me.flowLayoutPanel3.TabIndex = 2
        '
        'splitContainer1
        '
        Me.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.splitContainer1.Name = "splitContainer1"
        Me.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'splitContainer1.Panel1
        '
        Me.splitContainer1.Panel1.Controls.Add(Me.tableLayoutPanel1)
        '
        'splitContainer1.Panel2
        '
        Me.splitContainer1.Panel2.Controls.Add(Me.ResultTextBox)
        Me.splitContainer1.Panel2.Controls.Add(Me.statusStrip1)
        Me.splitContainer1.Size = New System.Drawing.Size(617, 450)
        Me.splitContainer1.SplitterDistance = 201
        Me.splitContainer1.TabIndex = 1
        Me.splitContainer1.TabStop = False
        '
        'statusStrip1
        '
        Me.statusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.toolStripStatusLabel1})
        Me.statusStrip1.Location = New System.Drawing.Point(0, 223)
        Me.statusStrip1.Name = "statusStrip1"
        Me.statusStrip1.Size = New System.Drawing.Size(617, 22)
        Me.statusStrip1.TabIndex = 1
        Me.statusStrip1.Text = "statusStrip1"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(617, 450)
        Me.Controls.Add(Me.splitContainer1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.NBestNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.NBestFlowLayoutPanel.ResumeLayout(False)
        Me.NBestFlowLayoutPanel.PerformLayout()
        Me.NBestGroupBox.ResumeLayout(False)
        Me.NBestGroupBox.PerformLayout()
        Me.OutputFormatflowLayoutPanel.ResumeLayout(False)
        Me.OutputFormatflowLayoutPanel.PerformLayout()
        Me.OutputFormatGroupBox.ResumeLayout(False)
        Me.LatticeLevelFlowLayoutPanel.ResumeLayout(False)
        Me.LatticeLevelFlowLayoutPanel.PerformLayout()
        Me.LatticeLevelGroupBox.ResumeLayout(False)
        Me.tableLayoutPanel1.ResumeLayout(False)
        Me.tableLayoutPanel1.PerformLayout()
        Me.flowLayoutPanel3.ResumeLayout(False)
        Me.flowLayoutPanel3.PerformLayout()
        Me.splitContainer1.Panel1.ResumeLayout(False)
        Me.splitContainer1.Panel2.ResumeLayout(False)
        Me.splitContainer1.Panel2.PerformLayout()
        CType(Me.splitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContainer1.ResumeLayout(False)
        Me.statusStrip1.ResumeLayout(False)
        Me.statusStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Private WithEvents toolStripStatusLabel1 As ToolStripStatusLabel
    Private WithEvents ResultTextBox As TextBox
    Private WithEvents AllMorphsCheckBox As CheckBox
    Private WithEvents PartialCheckBox As CheckBox
    Private WithEvents NBestCheckBox As CheckBox
    Private WithEvents NBestNumericUpDown As NumericUpDown
    Private WithEvents NBestFlowLayoutPanel As FlowLayoutPanel
    Private WithEvents NBestGroupBox As GroupBox
    Private WithEvents OutputFormatLatticeRadioButton As RadioButton
    Private WithEvents OutputFormatWakatiRadioButton As RadioButton
    Private WithEvents OutputFormatDumpRadioButton As RadioButton
    Private WithEvents OutputFormatflowLayoutPanel As FlowLayoutPanel
    Private WithEvents OutputFormatGroupBox As GroupBox
    Private WithEvents LatticeLevel0RadioButton As RadioButton
    Private WithEvents LatticeLevel1RadioButton As RadioButton
    Private WithEvents LatticeLevel2RadioButton As RadioButton
    Private WithEvents LatticeLevelFlowLayoutPanel As FlowLayoutPanel
    Private WithEvents LatticeLevelGroupBox As GroupBox
    Private WithEvents TargetTextBox As TextBox
    Private WithEvents DoAnalyzeButton As Button
    Private WithEvents tableLayoutPanel1 As TableLayoutPanel
    Private WithEvents flowLayoutPanel3 As FlowLayoutPanel
    Private WithEvents splitContainer1 As SplitContainer
    Private WithEvents statusStrip1 As StatusStrip
End Class
