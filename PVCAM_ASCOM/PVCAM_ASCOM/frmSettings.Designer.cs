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
            this.btnOk = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbRoiYtrim = new System.Windows.Forms.TextBox();
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
            this.label1.Location = new System.Drawing.Point(10, 654);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Error message:";
            // 
            // lblErrMsg
            // 
            this.lblErrMsg.AutoSize = true;
            this.lblErrMsg.Location = new System.Drawing.Point(116, 654);
            this.lblErrMsg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblErrMsg.Name = "lblErrMsg";
            this.lblErrMsg.Size = new System.Drawing.Size(40, 17);
            this.lblErrMsg.TabIndex = 2;
            this.lblErrMsg.Text = "none";
            // 
            // lbxStatusList
            // 
            this.lbxStatusList.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbxStatusList.FormattingEnabled = true;
            this.lbxStatusList.HorizontalScrollbar = true;
            this.lbxStatusList.ItemHeight = 17;
            this.lbxStatusList.Location = new System.Drawing.Point(13, 442);
            this.lbxStatusList.Margin = new System.Windows.Forms.Padding(4);
            this.lbxStatusList.Name = "lbxStatusList";
            this.lbxStatusList.Size = new System.Drawing.Size(504, 208);
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
            this.gboxSettings.Location = new System.Drawing.Point(8, 7);
            this.gboxSettings.Margin = new System.Windows.Forms.Padding(4);
            this.gboxSettings.Name = "gboxSettings";
            this.gboxSettings.Padding = new System.Windows.Forms.Padding(4);
            this.gboxSettings.Size = new System.Drawing.Size(308, 357);
            this.gboxSettings.TabIndex = 13;
            this.gboxSettings.TabStop = false;
            this.gboxSettings.Text = "Settings";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(21, 279);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(66, 17);
            this.label11.TabIndex = 36;
            this.label11.Text = "EM Gain:";
            // 
            // tbxEMGain
            // 
            this.tbxEMGain.Location = new System.Drawing.Point(25, 299);
            this.tbxEMGain.Margin = new System.Windows.Forms.Padding(4);
            this.tbxEMGain.Name = "tbxEMGain";
            this.tbxEMGain.Size = new System.Drawing.Size(61, 22);
            this.tbxEMGain.TabIndex = 35;
            this.tbxEMGain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxEMGain_KeyDown);
            // 
            // cbxGainStates
            // 
            this.cbxGainStates.FormattingEnabled = true;
            this.cbxGainStates.Location = new System.Drawing.Point(25, 101);
            this.cbxGainStates.Margin = new System.Windows.Forms.Padding(4);
            this.cbxGainStates.Name = "cbxGainStates";
            this.cbxGainStates.Size = new System.Drawing.Size(145, 24);
            this.cbxGainStates.TabIndex = 33;
            this.cbxGainStates.SelectedIndexChanged += new System.EventHandler(this.cbxGainStates_SelectedIndexChanged);
            // 
            // tbrEMGain
            // 
            this.tbrEMGain.Location = new System.Drawing.Point(96, 294);
            this.tbrEMGain.Margin = new System.Windows.Forms.Padding(4);
            this.tbrEMGain.Maximum = 1000;
            this.tbrEMGain.Name = "tbrEMGain";
            this.tbrEMGain.Size = new System.Drawing.Size(159, 56);
            this.tbrEMGain.TabIndex = 34;
            this.tbrEMGain.TickFrequency = 100;
            this.tbrEMGain.Scroll += new System.EventHandler(this.tbrEMGain_Scroll);
            this.tbrEMGain.ValueChanged += new System.EventHandler(this.tbrEMGain_ValueChanged);
            this.tbrEMGain.MouseCaptureChanged += new System.EventHandler(this.tbrEMGain_MouseCaptureChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 81);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 17);
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
            this.cbxClearCycles.Location = new System.Drawing.Point(25, 201);
            this.cbxClearCycles.Margin = new System.Windows.Forms.Padding(4);
            this.cbxClearCycles.Name = "cbxClearCycles";
            this.cbxClearCycles.Size = new System.Drawing.Size(145, 24);
            this.cbxClearCycles.TabIndex = 29;
            this.cbxClearCycles.SelectedIndexChanged += new System.EventHandler(this.cbxClearCycles_SelectedIndexChanged);
            // 
            // cbxRdOutSpd
            // 
            this.cbxRdOutSpd.DropDownWidth = 480;
            this.cbxRdOutSpd.FormattingEnabled = true;
            this.cbxRdOutSpd.Location = new System.Drawing.Point(25, 49);
            this.cbxRdOutSpd.Margin = new System.Windows.Forms.Padding(4);
            this.cbxRdOutSpd.Name = "cbxRdOutSpd";
            this.cbxRdOutSpd.Size = new System.Drawing.Size(237, 24);
            this.cbxRdOutSpd.TabIndex = 28;
            this.cbxRdOutSpd.SelectedIndexChanged += new System.EventHandler(this.cbxRdOutSpd_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(21, 30);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(109, 17);
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
            this.cbxClkMode.Location = new System.Drawing.Point(25, 250);
            this.cbxClkMode.Margin = new System.Windows.Forms.Padding(4);
            this.cbxClkMode.Name = "cbxClkMode";
            this.cbxClkMode.Size = new System.Drawing.Size(145, 24);
            this.cbxClkMode.TabIndex = 26;
            this.cbxClkMode.SelectedIndexChanged += new System.EventHandler(this.cbxClkMode_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 230);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 17);
            this.label9.TabIndex = 25;
            this.label9.Text = "Clocking mode:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 132);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 17);
            this.label5.TabIndex = 18;
            this.label5.Text = "Clear mode:";
            // 
            // cbxClearMode
            // 
            this.cbxClearMode.FormattingEnabled = true;
            this.cbxClearMode.Location = new System.Drawing.Point(25, 151);
            this.cbxClearMode.Margin = new System.Windows.Forms.Padding(4);
            this.cbxClearMode.Name = "cbxClearMode";
            this.cbxClearMode.Size = new System.Drawing.Size(145, 24);
            this.cbxClearMode.TabIndex = 16;
            this.cbxClearMode.SelectedIndexChanged += new System.EventHandler(this.cbxClearMode_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 181);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 17);
            this.label4.TabIndex = 15;
            this.label4.Text = "Clear cycles:";
            // 
            // cbxBinning
            // 
            this.cbxBinning.FormattingEnabled = true;
            this.cbxBinning.Location = new System.Drawing.Point(80, 30);
            this.cbxBinning.Margin = new System.Windows.Forms.Padding(4);
            this.cbxBinning.Name = "cbxBinning";
            this.cbxBinning.Size = new System.Drawing.Size(145, 24);
            this.cbxBinning.TabIndex = 30;
            this.cbxBinning.SelectedIndexChanged += new System.EventHandler(this.cbxBinning_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 33);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 17);
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
            this.gboxROI.Location = new System.Drawing.Point(8, 7);
            this.gboxROI.Margin = new System.Windows.Forms.Padding(4);
            this.gboxROI.Name = "gboxROI";
            this.gboxROI.Padding = new System.Windows.Forms.Padding(4);
            this.gboxROI.Size = new System.Drawing.Size(484, 357);
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
            this.gbxCentroid.Location = new System.Drawing.Point(16, 284);
            this.gbxCentroid.Margin = new System.Windows.Forms.Padding(4);
            this.gbxCentroid.Name = "gbxCentroid";
            this.gbxCentroid.Padding = new System.Windows.Forms.Padding(4);
            this.gbxCentroid.Size = new System.Drawing.Size(407, 66);
            this.gbxCentroid.TabIndex = 49;
            this.gbxCentroid.TabStop = false;
            this.gbxCentroid.Text = "Centroid";
            // 
            // lblCentroidRadiusRange
            // 
            this.lblCentroidRadiusRange.AutoSize = true;
            this.lblCentroidRadiusRange.Location = new System.Drawing.Point(313, 16);
            this.lblCentroidRadiusRange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCentroidRadiusRange.Name = "lblCentroidRadiusRange";
            this.lblCentroidRadiusRange.Size = new System.Drawing.Size(29, 17);
            this.lblCentroidRadiusRange.TabIndex = 6;
            this.lblCentroidRadiusRange.Text = "0-0";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(244, 15);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(60, 17);
            this.label24.TabIndex = 5;
            this.label24.Text = "Radius: ";
            // 
            // tbxCentroidRadius
            // 
            this.tbxCentroidRadius.Location = new System.Drawing.Point(248, 36);
            this.tbxCentroidRadius.Margin = new System.Windows.Forms.Padding(4);
            this.tbxCentroidRadius.Name = "tbxCentroidRadius";
            this.tbxCentroidRadius.Size = new System.Drawing.Size(140, 22);
            this.tbxCentroidRadius.TabIndex = 4;
            // 
            // lblCentrodCountRange
            // 
            this.lblCentrodCountRange.AutoSize = true;
            this.lblCentrodCountRange.Location = new System.Drawing.Point(159, 15);
            this.lblCentrodCountRange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCentrodCountRange.Name = "lblCentrodCountRange";
            this.lblCentrodCountRange.Size = new System.Drawing.Size(29, 17);
            this.lblCentrodCountRange.TabIndex = 3;
            this.lblCentrodCountRange.Text = "0-0";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(96, 15);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(53, 17);
            this.label23.TabIndex = 2;
            this.label23.Text = "Count: ";
            // 
            // tbxCentroidCount
            // 
            this.tbxCentroidCount.Location = new System.Drawing.Point(99, 34);
            this.tbxCentroidCount.Margin = new System.Windows.Forms.Padding(4);
            this.tbxCentroidCount.Name = "tbxCentroidCount";
            this.tbxCentroidCount.Size = new System.Drawing.Size(140, 22);
            this.tbxCentroidCount.TabIndex = 1;
            // 
            // cboxCentroidEnable
            // 
            this.cboxCentroidEnable.AutoSize = true;
            this.cboxCentroidEnable.Location = new System.Drawing.Point(9, 34);
            this.cboxCentroidEnable.Margin = new System.Windows.Forms.Padding(4);
            this.cboxCentroidEnable.Name = "cboxCentroidEnable";
            this.cboxCentroidEnable.Size = new System.Drawing.Size(74, 21);
            this.cboxCentroidEnable.TabIndex = 0;
            this.cboxCentroidEnable.Text = "Enable";
            this.cboxCentroidEnable.UseVisualStyleBackColor = true;
            this.cboxCentroidEnable.CheckedChanged += new System.EventHandler(this.cboxCentroidEnable_CheckedChanged);
            // 
            // cboxMetadata
            // 
            this.cboxMetadata.AutoSize = true;
            this.cboxMetadata.Location = new System.Drawing.Point(247, 32);
            this.cboxMetadata.Margin = new System.Windows.Forms.Padding(4);
            this.cboxMetadata.Name = "cboxMetadata";
            this.cboxMetadata.Size = new System.Drawing.Size(137, 21);
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
            this.gboxMultiROI.Location = new System.Drawing.Point(16, 63);
            this.gboxMultiROI.Margin = new System.Windows.Forms.Padding(4);
            this.gboxMultiROI.Name = "gboxMultiROI";
            this.gboxMultiROI.Padding = new System.Windows.Forms.Padding(4);
            this.gboxMultiROI.Size = new System.Drawing.Size(407, 222);
            this.gboxMultiROI.TabIndex = 47;
            this.gboxMultiROI.TabStop = false;
            this.gboxMultiROI.Text = "Regions";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(221, 23);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(49, 17);
            this.label15.TabIndex = 57;
            this.label15.Text = "Y-Size";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(155, 23);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(49, 17);
            this.label14.TabIndex = 56;
            this.label14.Text = "X-Size";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(88, 23);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(60, 17);
            this.label13.TabIndex = 55;
            this.label13.Text = "Y-Origin";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(21, 23);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(60, 17);
            this.label12.TabIndex = 54;
            this.label12.Text = "X-Origin";
            // 
            // btnRemoveROI
            // 
            this.btnRemoveROI.Location = new System.Drawing.Point(303, 82);
            this.btnRemoveROI.Margin = new System.Windows.Forms.Padding(4);
            this.btnRemoveROI.Name = "btnRemoveROI";
            this.btnRemoveROI.Size = new System.Drawing.Size(81, 32);
            this.btnRemoveROI.TabIndex = 53;
            this.btnRemoveROI.Text = "Remove";
            this.btnRemoveROI.UseVisualStyleBackColor = true;
            this.btnRemoveROI.Click += new System.EventHandler(this.btnRemoveROI_Click);
            // 
            // btnAddROI
            // 
            this.btnAddROI.Location = new System.Drawing.Point(304, 43);
            this.btnAddROI.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddROI.Name = "btnAddROI";
            this.btnAddROI.Size = new System.Drawing.Size(69, 32);
            this.btnAddROI.TabIndex = 51;
            this.btnAddROI.Text = "Add";
            this.btnAddROI.UseVisualStyleBackColor = true;
            this.btnAddROI.Click += new System.EventHandler(this.btnAddROI_Click);
            // 
            // tbxMySize
            // 
            this.tbxMySize.Location = new System.Drawing.Point(219, 43);
            this.tbxMySize.Margin = new System.Windows.Forms.Padding(4);
            this.tbxMySize.Name = "tbxMySize";
            this.tbxMySize.Size = new System.Drawing.Size(76, 22);
            this.tbxMySize.TabIndex = 50;
            // 
            // tbxMxSize
            // 
            this.tbxMxSize.Location = new System.Drawing.Point(153, 43);
            this.tbxMxSize.Margin = new System.Windows.Forms.Padding(4);
            this.tbxMxSize.Name = "tbxMxSize";
            this.tbxMxSize.Size = new System.Drawing.Size(68, 22);
            this.tbxMxSize.TabIndex = 49;
            // 
            // tbxMyOrigin
            // 
            this.tbxMyOrigin.Location = new System.Drawing.Point(88, 43);
            this.tbxMyOrigin.Margin = new System.Windows.Forms.Padding(4);
            this.tbxMyOrigin.Name = "tbxMyOrigin";
            this.tbxMyOrigin.Size = new System.Drawing.Size(68, 22);
            this.tbxMyOrigin.TabIndex = 48;
            // 
            // tbxMxOrigin
            // 
            this.tbxMxOrigin.Location = new System.Drawing.Point(23, 43);
            this.tbxMxOrigin.Margin = new System.Windows.Forms.Padding(4);
            this.tbxMxOrigin.Name = "tbxMxOrigin";
            this.tbxMxOrigin.Size = new System.Drawing.Size(68, 22);
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
            this.lstViewMultiROI.LabelEdit = true;
            this.lstViewMultiROI.Location = new System.Drawing.Point(21, 75);
            this.lstViewMultiROI.Margin = new System.Windows.Forms.Padding(4);
            this.lstViewMultiROI.Name = "lstViewMultiROI";
            this.lstViewMultiROI.Size = new System.Drawing.Size(272, 138);
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
            this.gboxCooling.Location = new System.Drawing.Point(324, 7);
            this.gboxCooling.Margin = new System.Windows.Forms.Padding(4);
            this.gboxCooling.Name = "gboxCooling";
            this.gboxCooling.Padding = new System.Windows.Forms.Padding(4);
            this.gboxCooling.Size = new System.Drawing.Size(155, 233);
            this.gboxCooling.TabIndex = 17;
            this.gboxCooling.TabStop = false;
            this.gboxCooling.Text = "Cooling";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(8, 180);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(85, 17);
            this.label20.TabIndex = 7;
            this.label20.Text = "Fan Speed: ";
            // 
            // cboxFanSpeed
            // 
            this.cboxFanSpeed.FormattingEnabled = true;
            this.cboxFanSpeed.Location = new System.Drawing.Point(8, 199);
            this.cboxFanSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.cboxFanSpeed.Name = "cboxFanSpeed";
            this.cboxFanSpeed.Size = new System.Drawing.Size(131, 24);
            this.cboxFanSpeed.TabIndex = 0;
            this.cboxFanSpeed.SelectedIndexChanged += new System.EventHandler(this.cboxFanSpeed_SelectedIndexChanged);
            // 
            // chboxTempUpdate
            // 
            this.chboxTempUpdate.AutoSize = true;
            this.chboxTempUpdate.Location = new System.Drawing.Point(8, 139);
            this.chboxTempUpdate.Margin = new System.Windows.Forms.Padding(4);
            this.chboxTempUpdate.Name = "chboxTempUpdate";
            this.chboxTempUpdate.Size = new System.Drawing.Size(77, 21);
            this.chboxTempUpdate.TabIndex = 6;
            this.chboxTempUpdate.Text = "Monitor";
            this.chboxTempUpdate.UseVisualStyleBackColor = true;
            this.chboxTempUpdate.CheckedChanged += new System.EventHandler(this.chboxTempUpdate_CheckedChanged);
            // 
            // btnApplyTemp
            // 
            this.btnApplyTemp.Location = new System.Drawing.Point(93, 36);
            this.btnApplyTemp.Margin = new System.Windows.Forms.Padding(4);
            this.btnApplyTemp.Name = "btnApplyTemp";
            this.btnApplyTemp.Size = new System.Drawing.Size(58, 27);
            this.btnApplyTemp.TabIndex = 5;
            this.btnApplyTemp.Text = "Apply";
            this.btnApplyTemp.UseVisualStyleBackColor = true;
            this.btnApplyTemp.Click += new System.EventHandler(this.btnApplyTemp_Click);
            // 
            // lblTempRange
            // 
            this.lblTempRange.AutoSize = true;
            this.lblTempRange.Location = new System.Drawing.Point(8, 65);
            this.lblTempRange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTempRange.Name = "lblTempRange";
            this.lblTempRange.Size = new System.Drawing.Size(34, 17);
            this.lblTempRange.TabIndex = 4;
            this.lblTempRange.Text = "0--0";
            // 
            // lboxCurTemp
            // 
            this.lboxCurTemp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lboxCurTemp.Location = new System.Drawing.Point(8, 105);
            this.lboxCurTemp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lboxCurTemp.Name = "lboxCurTemp";
            this.lboxCurTemp.Size = new System.Drawing.Size(82, 27);
            this.lboxCurTemp.TabIndex = 3;
            this.lboxCurTemp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(8, 89);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(63, 17);
            this.label19.TabIndex = 2;
            this.label19.Text = "Current :";
            // 
            // tbxSetpoint
            // 
            this.tbxSetpoint.Location = new System.Drawing.Point(8, 38);
            this.tbxSetpoint.Margin = new System.Windows.Forms.Padding(4);
            this.tbxSetpoint.Name = "tbxSetpoint";
            this.tbxSetpoint.Size = new System.Drawing.Size(81, 22);
            this.tbxSetpoint.TabIndex = 1;
            this.tbxSetpoint.TextChanged += new System.EventHandler(this.tbxSetpoint_TextChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(4, 22);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(68, 17);
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
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(511, 433);
            this.tabControl1.TabIndex = 19;
            // 
            // tabSettings
            // 
            this.tabSettings.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabSettings.Controls.Add(this.gboxSettings);
            this.tabSettings.Controls.Add(this.gboxCooling);
            this.tabSettings.Location = new System.Drawing.Point(4, 25);
            this.tabSettings.Margin = new System.Windows.Forms.Padding(4);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(4);
            this.tabSettings.Size = new System.Drawing.Size(503, 375);
            this.tabSettings.TabIndex = 1;
            this.tabSettings.Text = "Basic Settings";
            // 
            // tabROI
            // 
            this.tabROI.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabROI.Controls.Add(this.label3);
            this.tabROI.Controls.Add(this.tbRoiYtrim);
            this.tabROI.Controls.Add(this.gboxROI);
            this.tabROI.Location = new System.Drawing.Point(4, 25);
            this.tabROI.Margin = new System.Windows.Forms.Padding(4);
            this.tabROI.Name = "tabROI";
            this.tabROI.Padding = new System.Windows.Forms.Padding(4);
            this.tabROI.Size = new System.Drawing.Size(503, 404);
            this.tabROI.TabIndex = 2;
            this.tabROI.Text = "ROI Control";
           // this.tabROI.Click += new System.EventHandler(this.tabROI_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(207, 657);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(126, 31);
            this.btnOk.TabIndex = 20;
            this.btnOk.Text = "ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 368);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 17);
            this.label3.TabIndex = 42;
            this.label3.Text = "Trim Pixels off y:";
            // 
            // tbRoiYtrim
            // 
            this.tbRoiYtrim.Location = new System.Drawing.Point(127, 368);
            this.tbRoiYtrim.Margin = new System.Windows.Forms.Padding(4);
            this.tbRoiYtrim.Name = "tbRoiYtrim";
            this.tbRoiYtrim.Size = new System.Drawing.Size(61, 22);
            this.tbRoiYtrim.TabIndex = 41;
            // 
            // FrmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(533, 698);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lbxStatusList);
            this.Controls.Add(this.lblErrMsg);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmSettings";
            this.Text = "PVCam .NET Sample";
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

