namespace pvcam_helper
{
    partial class FrmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSettings));
            this.label1 = new System.Windows.Forms.Label();
            this.lblErrMsg = new System.Windows.Forms.Label();
            this.lbxStatusList = new System.Windows.Forms.ListBox();
            this.gboxSettings = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbxEMGain = new System.Windows.Forms.TextBox();
            this.cbxGainStates = new System.Windows.Forms.ComboBox();
            this.tbrEMGain = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxClearCycles = new System.Windows.Forms.ComboBox();
            this.cbxRdOutSpd = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbxClkMode = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbxClearMode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbxBinning = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.gboxROI = new System.Windows.Forms.GroupBox();
            this.gbxCentroid = new System.Windows.Forms.GroupBox();
            this.lblCentroidRadiusRange = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.tbxCentroidRadius = new System.Windows.Forms.TextBox();
            this.lblCentrodCountRange = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.tbxCentroidCount = new System.Windows.Forms.TextBox();
            this.cboxCentroidEnable = new System.Windows.Forms.CheckBox();
            this.cboxMetadata = new System.Windows.Forms.CheckBox();
            this.gboxMultiROI = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnRemoveROI = new System.Windows.Forms.Button();
            this.btnAddROI = new System.Windows.Forms.Button();
            this.tbxMySize = new System.Windows.Forms.TextBox();
            this.tbxMxSize = new System.Windows.Forms.TextBox();
            this.tbxMyOrigin = new System.Windows.Forms.TextBox();
            this.tbxMxOrigin = new System.Windows.Forms.TextBox();
            this.lstViewMultiROI = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gboxCooling = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.cboxFanSpeed = new System.Windows.Forms.ComboBox();
            this.chboxTempUpdate = new System.Windows.Forms.CheckBox();
            this.btnApplyTemp = new System.Windows.Forms.Button();
            this.lblTempRange = new System.Windows.Forms.Label();
            this.lboxCurTemp = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.tbxSetpoint = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.tmrTempUpdate = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.tabROI = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.tbRoiYtrim = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.gboxSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbrEMGain)).BeginInit();
            this.gboxROI.SuspendLayout();
            this.gbxCentroid.SuspendLayout();
            this.gboxMultiROI.SuspendLayout();
            this.gboxCooling.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.tabROI.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 531);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Error message:";
            // 
            // lblErrMsg
            // 
            this.lblErrMsg.AutoSize = true;
            this.lblErrMsg.Location = new System.Drawing.Point(87, 531);
            this.lblErrMsg.Name = "lblErrMsg";
            this.lblErrMsg.Size = new System.Drawing.Size(31, 13);
            this.lblErrMsg.TabIndex = 2;
            this.lblErrMsg.Text = "none";
            // 
            // lbxStatusList
            // 
            this.lbxStatusList.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbxStatusList.FormattingEnabled = true;
            this.lbxStatusList.HorizontalScrollbar = true;
            this.lbxStatusList.ItemHeight = 14;
            this.lbxStatusList.Location = new System.Drawing.Point(10, 359);
            this.lbxStatusList.Name = "lbxStatusList";
            this.lbxStatusList.Size = new System.Drawing.Size(379, 158);
            this.lbxStatusList.TabIndex = 5;
            // 
            // gboxSettings
            // 
            this.gboxSettings.Controls.Add(this.label11);
            this.gboxSettings.Controls.Add(this.tbxEMGain);
            this.gboxSettings.Controls.Add(this.cbxGainStates);
            this.gboxSettings.Controls.Add(this.tbrEMGain);
            this.gboxSettings.Controls.Add(this.label2);
            this.gboxSettings.Controls.Add(this.cbxClearCycles);
            this.gboxSettings.Controls.Add(this.cbxRdOutSpd);
            this.gboxSettings.Controls.Add(this.label10);
            this.gboxSettings.Controls.Add(this.cbxClkMode);
            this.gboxSettings.Controls.Add(this.label9);
            this.gboxSettings.Controls.Add(this.label5);
            this.gboxSettings.Controls.Add(this.cbxClearMode);
            this.gboxSettings.Controls.Add(this.label4);
            this.gboxSettings.Location = new System.Drawing.Point(6, 6);
            this.gboxSettings.Name = "gboxSettings";
            this.gboxSettings.Size = new System.Drawing.Size(231, 290);
            this.gboxSettings.TabIndex = 13;
            this.gboxSettings.TabStop = false;
            this.gboxSettings.Text = "Settings";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(16, 227);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 13);
            this.label11.TabIndex = 36;
            this.label11.Text = "EM Gain:";
            // 
            // tbxEMGain
            // 
            this.tbxEMGain.Location = new System.Drawing.Point(19, 243);
            this.tbxEMGain.Name = "tbxEMGain";
            this.tbxEMGain.Size = new System.Drawing.Size(47, 20);
            this.tbxEMGain.TabIndex = 35;
            this.tbxEMGain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxEMGain_KeyDown);
            // 
            // cbxGainStates
            // 
            this.cbxGainStates.FormattingEnabled = true;
            this.cbxGainStates.Location = new System.Drawing.Point(19, 82);
            this.cbxGainStates.Name = "cbxGainStates";
            this.cbxGainStates.Size = new System.Drawing.Size(110, 21);
            this.cbxGainStates.TabIndex = 33;
            this.cbxGainStates.SelectedIndexChanged += new System.EventHandler(this.cbxGainStates_SelectedIndexChanged);
            // 
            // tbrEMGain
            // 
            this.tbrEMGain.Location = new System.Drawing.Point(72, 239);
            this.tbrEMGain.Maximum = 1000;
            this.tbrEMGain.Name = "tbrEMGain";
            this.tbrEMGain.Size = new System.Drawing.Size(119, 45);
            this.tbrEMGain.TabIndex = 34;
            this.tbrEMGain.TickFrequency = 100;
            this.tbrEMGain.Scroll += new System.EventHandler(this.tbrEMGain_Scroll);
            this.tbrEMGain.ValueChanged += new System.EventHandler(this.tbrEMGain_ValueChanged);
            this.tbrEMGain.MouseCaptureChanged += new System.EventHandler(this.tbrEMGain_MouseCaptureChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Gain state:";
            // 
            // cbxClearCycles
            // 
            this.cbxClearCycles.FormattingEnabled = true;
            this.cbxClearCycles.Items.AddRange(new object[] {
            "1",
            "2",
            "4",
            "8",
            "10",
            "20"});
            this.cbxClearCycles.Location = new System.Drawing.Point(19, 163);
            this.cbxClearCycles.Name = "cbxClearCycles";
            this.cbxClearCycles.Size = new System.Drawing.Size(110, 21);
            this.cbxClearCycles.TabIndex = 29;
            this.cbxClearCycles.SelectedIndexChanged += new System.EventHandler(this.cbxClearCycles_SelectedIndexChanged);
            // 
            // cbxRdOutSpd
            // 
            this.cbxRdOutSpd.DropDownWidth = 480;
            this.cbxRdOutSpd.FormattingEnabled = true;
            this.cbxRdOutSpd.Location = new System.Drawing.Point(19, 40);
            this.cbxRdOutSpd.Name = "cbxRdOutSpd";
            this.cbxRdOutSpd.Size = new System.Drawing.Size(179, 21);
            this.cbxRdOutSpd.TabIndex = 28;
            this.cbxRdOutSpd.SelectedIndexChanged += new System.EventHandler(this.cbxRdOutSpd_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(83, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "Readout speed:";
            // 
            // cbxClkMode
            // 
            this.cbxClkMode.FormattingEnabled = true;
            this.cbxClkMode.Items.AddRange(new object[] {
            "Normal",
            "FT",
            "MPP",
            "FT MPP",
            "Alt Normal",
            "Alt FT",
            "Alt MPP",
            "Alt FT MPP"});
            this.cbxClkMode.Location = new System.Drawing.Point(19, 203);
            this.cbxClkMode.Name = "cbxClkMode";
            this.cbxClkMode.Size = new System.Drawing.Size(110, 21);
            this.cbxClkMode.TabIndex = 26;
            this.cbxClkMode.SelectedIndexChanged += new System.EventHandler(this.cbxClkMode_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 187);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "Clocking mode:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Clear mode:";
            // 
            // cbxClearMode
            // 
            this.cbxClearMode.FormattingEnabled = true;
            this.cbxClearMode.Location = new System.Drawing.Point(19, 123);
            this.cbxClearMode.Name = "cbxClearMode";
            this.cbxClearMode.Size = new System.Drawing.Size(110, 21);
            this.cbxClearMode.TabIndex = 16;
            this.cbxClearMode.SelectedIndexChanged += new System.EventHandler(this.cbxClearMode_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Clear cycles:";
            // 
            // cbxBinning
            // 
            this.cbxBinning.FormattingEnabled = true;
            this.cbxBinning.Location = new System.Drawing.Point(60, 24);
            this.cbxBinning.Name = "cbxBinning";
            this.cbxBinning.Size = new System.Drawing.Size(110, 21);
            this.cbxBinning.TabIndex = 30;
            this.cbxBinning.SelectedIndexChanged += new System.EventHandler(this.cbxBinning_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Binning:";
            // 
            // gboxROI
            // 
            this.gboxROI.Controls.Add(this.gbxCentroid);
            this.gboxROI.Controls.Add(this.cboxMetadata);
            this.gboxROI.Controls.Add(this.gboxMultiROI);
            this.gboxROI.Controls.Add(this.label7);
            this.gboxROI.Controls.Add(this.cbxBinning);
            this.gboxROI.Location = new System.Drawing.Point(6, 6);
            this.gboxROI.Name = "gboxROI";
            this.gboxROI.Size = new System.Drawing.Size(363, 290);
            this.gboxROI.TabIndex = 34;
            this.gboxROI.TabStop = false;
            this.gboxROI.Text = "ROI control";
            // 
            // gbxCentroid
            // 
            this.gbxCentroid.Controls.Add(this.lblCentroidRadiusRange);
            this.gbxCentroid.Controls.Add(this.label24);
            this.gbxCentroid.Controls.Add(this.tbxCentroidRadius);
            this.gbxCentroid.Controls.Add(this.lblCentrodCountRange);
            this.gbxCentroid.Controls.Add(this.label23);
            this.gbxCentroid.Controls.Add(this.tbxCentroidCount);
            this.gbxCentroid.Controls.Add(this.cboxCentroidEnable);
            this.gbxCentroid.Location = new System.Drawing.Point(12, 231);
            this.gbxCentroid.Name = "gbxCentroid";
            this.gbxCentroid.Size = new System.Drawing.Size(305, 54);
            this.gbxCentroid.TabIndex = 49;
            this.gbxCentroid.TabStop = false;
            this.gbxCentroid.Text = "Centroid";
            // 
            // lblCentroidRadiusRange
            // 
            this.lblCentroidRadiusRange.AutoSize = true;
            this.lblCentroidRadiusRange.Location = new System.Drawing.Point(235, 13);
            this.lblCentroidRadiusRange.Name = "lblCentroidRadiusRange";
            this.lblCentroidRadiusRange.Size = new System.Drawing.Size(22, 13);
            this.lblCentroidRadiusRange.TabIndex = 6;
            this.lblCentroidRadiusRange.Text = "0-0";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(183, 12);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(46, 13);
            this.label24.TabIndex = 5;
            this.label24.Text = "Radius: ";
            // 
            // tbxCentroidRadius
            // 
            this.tbxCentroidRadius.Location = new System.Drawing.Point(186, 29);
            this.tbxCentroidRadius.Name = "tbxCentroidRadius";
            this.tbxCentroidRadius.Size = new System.Drawing.Size(106, 20);
            this.tbxCentroidRadius.TabIndex = 4;
            // 
            // lblCentrodCountRange
            // 
            this.lblCentrodCountRange.AutoSize = true;
            this.lblCentrodCountRange.Location = new System.Drawing.Point(119, 12);
            this.lblCentrodCountRange.Name = "lblCentrodCountRange";
            this.lblCentrodCountRange.Size = new System.Drawing.Size(22, 13);
            this.lblCentrodCountRange.TabIndex = 3;
            this.lblCentrodCountRange.Text = "0-0";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(72, 12);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(41, 13);
            this.label23.TabIndex = 2;
            this.label23.Text = "Count: ";
            // 
            // tbxCentroidCount
            // 
            this.tbxCentroidCount.Location = new System.Drawing.Point(74, 28);
            this.tbxCentroidCount.Name = "tbxCentroidCount";
            this.tbxCentroidCount.Size = new System.Drawing.Size(106, 20);
            this.tbxCentroidCount.TabIndex = 1;
            // 
            // cboxCentroidEnable
            // 
            this.cboxCentroidEnable.AutoSize = true;
            this.cboxCentroidEnable.Location = new System.Drawing.Point(7, 28);
            this.cboxCentroidEnable.Name = "cboxCentroidEnable";
            this.cboxCentroidEnable.Size = new System.Drawing.Size(59, 17);
            this.cboxCentroidEnable.TabIndex = 0;
            this.cboxCentroidEnable.Text = "Enable";
            this.cboxCentroidEnable.UseVisualStyleBackColor = true;
            this.cboxCentroidEnable.CheckedChanged += new System.EventHandler(this.cboxCentroidEnable_CheckedChanged);
            // 
            // cboxMetadata
            // 
            this.cboxMetadata.AutoSize = true;
            this.cboxMetadata.Location = new System.Drawing.Point(185, 26);
            this.cboxMetadata.Name = "cboxMetadata";
            this.cboxMetadata.Size = new System.Drawing.Size(107, 17);
            this.cboxMetadata.TabIndex = 48;
            this.cboxMetadata.Text = "Enable Metadata";
            this.cboxMetadata.UseVisualStyleBackColor = true;
            this.cboxMetadata.CheckedChanged += new System.EventHandler(this.cboxMetadata_CheckedChanged);
            // 
            // gboxMultiROI
            // 
            this.gboxMultiROI.Controls.Add(this.label15);
            this.gboxMultiROI.Controls.Add(this.label14);
            this.gboxMultiROI.Controls.Add(this.label13);
            this.gboxMultiROI.Controls.Add(this.label12);
            this.gboxMultiROI.Controls.Add(this.btnRemoveROI);
            this.gboxMultiROI.Controls.Add(this.btnAddROI);
            this.gboxMultiROI.Controls.Add(this.tbxMySize);
            this.gboxMultiROI.Controls.Add(this.tbxMxSize);
            this.gboxMultiROI.Controls.Add(this.tbxMyOrigin);
            this.gboxMultiROI.Controls.Add(this.tbxMxOrigin);
            this.gboxMultiROI.Controls.Add(this.lstViewMultiROI);
            this.gboxMultiROI.Location = new System.Drawing.Point(12, 51);
            this.gboxMultiROI.Name = "gboxMultiROI";
            this.gboxMultiROI.Size = new System.Drawing.Size(305, 180);
            this.gboxMultiROI.TabIndex = 47;
            this.gboxMultiROI.TabStop = false;
            this.gboxMultiROI.Text = "Regions";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(166, 19);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(37, 13);
            this.label15.TabIndex = 57;
            this.label15.Text = "Y-Size";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(116, 19);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(37, 13);
            this.label14.TabIndex = 56;
            this.label14.Text = "X-Size";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(66, 19);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(44, 13);
            this.label13.TabIndex = 55;
            this.label13.Text = "Y-Origin";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(16, 19);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 13);
            this.label12.TabIndex = 54;
            this.label12.Text = "X-Origin";
            // 
            // btnRemoveROI
            // 
            this.btnRemoveROI.Location = new System.Drawing.Point(227, 67);
            this.btnRemoveROI.Name = "btnRemoveROI";
            this.btnRemoveROI.Size = new System.Drawing.Size(61, 26);
            this.btnRemoveROI.TabIndex = 53;
            this.btnRemoveROI.Text = "Remove";
            this.btnRemoveROI.UseVisualStyleBackColor = true;
            this.btnRemoveROI.Click += new System.EventHandler(this.btnRemoveROI_Click);
            // 
            // btnAddROI
            // 
            this.btnAddROI.Location = new System.Drawing.Point(228, 35);
            this.btnAddROI.Name = "btnAddROI";
            this.btnAddROI.Size = new System.Drawing.Size(52, 26);
            this.btnAddROI.TabIndex = 51;
            this.btnAddROI.Text = "Add";
            this.btnAddROI.UseVisualStyleBackColor = true;
            this.btnAddROI.Click += new System.EventHandler(this.btnAddROI_Click);
            // 
            // tbxMySize
            // 
            this.tbxMySize.Location = new System.Drawing.Point(164, 35);
            this.tbxMySize.Name = "tbxMySize";
            this.tbxMySize.Size = new System.Drawing.Size(58, 20);
            this.tbxMySize.TabIndex = 50;
            // 
            // tbxMxSize
            // 
            this.tbxMxSize.Location = new System.Drawing.Point(115, 35);
            this.tbxMxSize.Name = "tbxMxSize";
            this.tbxMxSize.Size = new System.Drawing.Size(52, 20);
            this.tbxMxSize.TabIndex = 49;
            // 
            // tbxMyOrigin
            // 
            this.tbxMyOrigin.Location = new System.Drawing.Point(66, 35);
            this.tbxMyOrigin.Name = "tbxMyOrigin";
            this.tbxMyOrigin.Size = new System.Drawing.Size(52, 20);
            this.tbxMyOrigin.TabIndex = 48;
            // 
            // tbxMxOrigin
            // 
            this.tbxMxOrigin.Location = new System.Drawing.Point(17, 35);
            this.tbxMxOrigin.Name = "tbxMxOrigin";
            this.tbxMxOrigin.Size = new System.Drawing.Size(52, 20);
            this.tbxMxOrigin.TabIndex = 47;
            // 
            // lstViewMultiROI
            // 
            this.lstViewMultiROI.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lstViewMultiROI.FullRowSelect = true;
            this.lstViewMultiROI.GridLines = true;
            this.lstViewMultiROI.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstViewMultiROI.HideSelection = false;
            this.lstViewMultiROI.LabelEdit = true;
            this.lstViewMultiROI.Location = new System.Drawing.Point(16, 61);
            this.lstViewMultiROI.Name = "lstViewMultiROI";
            this.lstViewMultiROI.Size = new System.Drawing.Size(205, 113);
            this.lstViewMultiROI.TabIndex = 46;
            this.lstViewMultiROI.UseCompatibleStateImageBehavior = false;
            this.lstViewMultiROI.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "X-Origin";
            this.columnHeader1.Width = 48;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 48;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Width = 48;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Width = 48;
            // 
            // gboxCooling
            // 
            this.gboxCooling.Controls.Add(this.label20);
            this.gboxCooling.Controls.Add(this.cboxFanSpeed);
            this.gboxCooling.Controls.Add(this.chboxTempUpdate);
            this.gboxCooling.Controls.Add(this.btnApplyTemp);
            this.gboxCooling.Controls.Add(this.lblTempRange);
            this.gboxCooling.Controls.Add(this.lboxCurTemp);
            this.gboxCooling.Controls.Add(this.label19);
            this.gboxCooling.Controls.Add(this.tbxSetpoint);
            this.gboxCooling.Controls.Add(this.label18);
            this.gboxCooling.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gboxCooling.Location = new System.Drawing.Point(243, 6);
            this.gboxCooling.Name = "gboxCooling";
            this.gboxCooling.Size = new System.Drawing.Size(116, 189);
            this.gboxCooling.TabIndex = 17;
            this.gboxCooling.TabStop = false;
            this.gboxCooling.Text = "Cooling";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 146);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(65, 13);
            this.label20.TabIndex = 7;
            this.label20.Text = "Fan Speed: ";
            // 
            // cboxFanSpeed
            // 
            this.cboxFanSpeed.FormattingEnabled = true;
            this.cboxFanSpeed.Location = new System.Drawing.Point(6, 162);
            this.cboxFanSpeed.Name = "cboxFanSpeed";
            this.cboxFanSpeed.Size = new System.Drawing.Size(99, 21);
            this.cboxFanSpeed.TabIndex = 0;
            this.cboxFanSpeed.SelectedIndexChanged += new System.EventHandler(this.cboxFanSpeed_SelectedIndexChanged);
            // 
            // chboxTempUpdate
            // 
            this.chboxTempUpdate.AutoSize = true;
            this.chboxTempUpdate.Location = new System.Drawing.Point(6, 113);
            this.chboxTempUpdate.Name = "chboxTempUpdate";
            this.chboxTempUpdate.Size = new System.Drawing.Size(61, 17);
            this.chboxTempUpdate.TabIndex = 6;
            this.chboxTempUpdate.Text = "Monitor";
            this.chboxTempUpdate.UseVisualStyleBackColor = true;
            this.chboxTempUpdate.CheckedChanged += new System.EventHandler(this.chboxTempUpdate_CheckedChanged);
            // 
            // btnApplyTemp
            // 
            this.btnApplyTemp.Location = new System.Drawing.Point(70, 29);
            this.btnApplyTemp.Name = "btnApplyTemp";
            this.btnApplyTemp.Size = new System.Drawing.Size(44, 22);
            this.btnApplyTemp.TabIndex = 5;
            this.btnApplyTemp.Text = "Apply";
            this.btnApplyTemp.UseVisualStyleBackColor = true;
            this.btnApplyTemp.Click += new System.EventHandler(this.btnApplyTemp_Click);
            // 
            // lblTempRange
            // 
            this.lblTempRange.AutoSize = true;
            this.lblTempRange.Location = new System.Drawing.Point(6, 53);
            this.lblTempRange.Name = "lblTempRange";
            this.lblTempRange.Size = new System.Drawing.Size(25, 13);
            this.lblTempRange.TabIndex = 4;
            this.lblTempRange.Text = "0--0";
            // 
            // lboxCurTemp
            // 
            this.lboxCurTemp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lboxCurTemp.Location = new System.Drawing.Point(6, 85);
            this.lboxCurTemp.Name = "lboxCurTemp";
            this.lboxCurTemp.Size = new System.Drawing.Size(62, 22);
            this.lboxCurTemp.TabIndex = 3;
            this.lboxCurTemp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 72);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(47, 13);
            this.label19.TabIndex = 2;
            this.label19.Text = "Current :";
            // 
            // tbxSetpoint
            // 
            this.tbxSetpoint.Location = new System.Drawing.Point(6, 31);
            this.tbxSetpoint.Name = "tbxSetpoint";
            this.tbxSetpoint.Size = new System.Drawing.Size(62, 20);
            this.tbxSetpoint.TabIndex = 1;
            this.tbxSetpoint.TextChanged += new System.EventHandler(this.tbxSetpoint_TextChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(3, 18);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(52, 13);
            this.label18.TabIndex = 0;
            this.label18.Text = "Setpoint :";
            // 
            // tmrTempUpdate
            // 
            this.tmrTempUpdate.Interval = 1500;
            this.tmrTempUpdate.Tick += new System.EventHandler(this.tmrTempUpdate_Tick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabSettings);
            this.tabControl1.Controls.Add(this.tabROI);
            this.tabControl1.Location = new System.Drawing.Point(10, 11);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(383, 352);
            this.tabControl1.TabIndex = 19;
            // 
            // tabSettings
            // 
            this.tabSettings.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabSettings.Controls.Add(this.gboxSettings);
            this.tabSettings.Controls.Add(this.gboxCooling);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabSettings.Size = new System.Drawing.Size(375, 326);
            this.tabSettings.TabIndex = 1;
            this.tabSettings.Text = "Basic Settings";
            // 
            // tabROI
            // 
            this.tabROI.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabROI.Controls.Add(this.label3);
            this.tabROI.Controls.Add(this.tbRoiYtrim);
            this.tabROI.Controls.Add(this.gboxROI);
            this.tabROI.Location = new System.Drawing.Point(4, 22);
            this.tabROI.Name = "tabROI";
            this.tabROI.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabROI.Size = new System.Drawing.Size(375, 326);
            this.tabROI.TabIndex = 2;
            this.tabROI.Text = "ROI Control";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 299);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 42;
            this.label3.Text = "Trim Pixels off y:";
            // 
            // tbRoiYtrim
            // 
            this.tbRoiYtrim.Location = new System.Drawing.Point(95, 299);
            this.tbRoiYtrim.Name = "tbRoiYtrim";
            this.tbRoiYtrim.Size = new System.Drawing.Size(47, 20);
            this.tbRoiYtrim.TabIndex = 41;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(155, 534);
            this.btnOk.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(94, 25);
            this.btnOk.TabIndex = 20;
            this.btnOk.Text = "ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FrmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(400, 567);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lbxStatusList);
            this.Controls.Add(this.lblErrMsg);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmSettings";
            this.Text = "Photometrics settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmTester_FormClosing);
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.gboxSettings.ResumeLayout(false);
            this.gboxSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbrEMGain)).EndInit();
            this.gboxROI.ResumeLayout(false);
            this.gboxROI.PerformLayout();
            this.gbxCentroid.ResumeLayout(false);
            this.gbxCentroid.PerformLayout();
            this.gboxMultiROI.ResumeLayout(false);
            this.gboxMultiROI.PerformLayout();
            this.gboxCooling.ResumeLayout(false);
            this.gboxCooling.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.tabROI.ResumeLayout(false);
            this.tabROI.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblErrMsg;
        private System.Windows.Forms.ListBox lbxStatusList;

        public System.Windows.Forms.ListBox LbxStatusList
        {
            get { return lbxStatusList; }
            set { lbxStatusList = value; }
        }
        private System.Windows.Forms.GroupBox gboxSettings;
        private System.Windows.Forms.ComboBox cbxClearMode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbxRdOutSpd;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbxClkMode;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbxBinning;
        private System.Windows.Forms.ComboBox cbxClearCycles;
        private System.Windows.Forms.TrackBar tbrEMGain;
        private System.Windows.Forms.ComboBox cbxGainStates;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxEMGain;
        private System.Windows.Forms.GroupBox gboxROI;
        private System.Windows.Forms.GroupBox gboxCooling;
        private System.Windows.Forms.CheckBox chboxTempUpdate;
        private System.Windows.Forms.Button btnApplyTemp;
        private System.Windows.Forms.Label lblTempRange;
        private System.Windows.Forms.Label lboxCurTemp;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox tbxSetpoint;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Timer tmrTempUpdate;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.TabPage tabROI;
        private System.Windows.Forms.ComboBox cboxFanSpeed;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ListView lstViewMultiROI;
        private System.Windows.Forms.GroupBox gboxMultiROI;
        private System.Windows.Forms.Button btnAddROI;
        private System.Windows.Forms.TextBox tbxMySize;
        private System.Windows.Forms.TextBox tbxMxSize;
        private System.Windows.Forms.TextBox tbxMyOrigin;
        private System.Windows.Forms.TextBox tbxMxOrigin;
        private System.Windows.Forms.Button btnRemoveROI;
        private System.Windows.Forms.CheckBox cboxMetadata;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.GroupBox gbxCentroid;
        private System.Windows.Forms.Label lblCentroidRadiusRange;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox tbxCentroidRadius;
        private System.Windows.Forms.Label lblCentrodCountRange;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox tbxCentroidCount;
        private System.Windows.Forms.CheckBox cboxCentroidEnable;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbRoiYtrim;

        public System.Windows.Forms.Label LblErrMsg
        {
            get { return lblErrMsg; }
            set { lblErrMsg = value; }
        }
    }
}

