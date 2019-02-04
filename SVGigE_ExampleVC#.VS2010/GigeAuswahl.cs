using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace GigEApi
{
	/// <summary>
	/// Zusammenfassung für GigeAuswahl.
	/// </summary>
	public class GigeAuswahl : System.Windows.Forms.Form
	{
		int selectedIndex = -1;

		private GigeApi myApi;
		private int hCameraContainer;
		private int hCamera;
	
		private string modelname; 
		private int camWidth, camHeight, gigeBitcount;
		private GigeApi.SVSGigeApiReturn errorflag;
        
		private Form1 camDispl;
        private Button refreshbutton;
        private ListBox GigeAuswahllistBox;

        private ComboBox GigeComboBox;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GigeAuswahl(Form1 cD, ComboBox GigeComboBox)
		{
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//

            this.GigeComboBox = GigeComboBox;

			InitializeComponent();
			camDispl = cD;



			//
			// TODO: Fügen Sie den Konstruktorcode nach dem Aufruf von InitializeComponent hinzu
			//
           
		}

		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
            this.refreshbutton = new System.Windows.Forms.Button();
            this.GigeAuswahllistBox = new System.Windows.Forms.ListBox();

            //this.GigeComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // refreshbutton
            // 
            this.refreshbutton.Location = new System.Drawing.Point(4, 8);
            this.refreshbutton.Name = "refreshbutton";
            this.refreshbutton.Size = new System.Drawing.Size(144, 21);
            this.refreshbutton.TabIndex = 1;
            this.refreshbutton.Text = "Refresh";
            this.refreshbutton.UseVisualStyleBackColor = true;
            this.refreshbutton.Click += new System.EventHandler(this.refreshbutton_Click);
            // 
            // GigeAuswahllistBox
            // 
            this.GigeAuswahllistBox.FormattingEnabled = true;
            this.GigeAuswahllistBox.Location = new System.Drawing.Point(4, 35);
            this.GigeAuswahllistBox.Name = "GigeAuswahllistBox";
            this.GigeAuswahllistBox.Size = new System.Drawing.Size(369, 264);
            this.GigeAuswahllistBox.TabIndex = 2;
            this.GigeAuswahllistBox.SelectedIndexChanged += new System.EventHandler(this.GigeAuswahlcomboBox_SelectedIndexChanged);
            this.GigeAuswahllistBox.Click += new System.EventHandler(this.GigeAuswahlcomboBox_Click);
            // 
            // GigeAuswahl
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(408, 341);
            this.Controls.Add(this.GigeAuswahllistBox);
            this.Controls.Add(this.refreshbutton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GigeAuswahl";
            this.Text = "GigeAuswahl";
            this.Load += new System.EventHandler(this.GigeAuswahl_Load);
            this.ResumeLayout(false);

		}
		#endregion

		public void addEntry(string entry)
		{
			GigeAuswahllistBox.Items.Add(entry);
		}

		private void GigeAuswahlcomboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int n;
            //GigeApi.SvGigePixelDepth depth;
			selectedIndex = GigeAuswahllistBox.SelectedIndex;
            
			n=getSelectedIndex();


			hCamera = myApi.Gige_CameraContainer_getCamera(hCameraContainer,n);	
			modelname = myApi.Gige_Camera_getModelName(hCamera);
			
			MessageBox.Show(modelname);
			errorflag=myApi.Gige_Camera_openConnection(hCamera, 15.0f);
						
			if(errorflag!=GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
			{
				MessageBox.Show("Errorflag: "+ errorflag.ToString());
				this.Close();
			}
			
			{
												
				myApi.Gige_Camera_setAcquisitionMode(hCamera,GigeApi.SVSGige_CameraCodes.ACQUISITION_MODE_FREE_RUNNING);

				/***
				if(myApi.Gige_Camera_isCameraFeature(hCamera,GigeApi.CameraFeature.CAMERA_FEATURE_COLORDEPTH_16BPP)==true)
				{
					errorflag = myApi.Gige_Camera_setPixelDepth(hCamera,GigeApi.SvGigePixelDepth.SVGIGE_PIXEL_DEPTH_16);
					if(errorflag!=GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
					{
						MessageBox.Show("Errorflag 16bit: "+ errorflag.ToString());
						this.Close();
					}
					gigeBitcount=16;
				}
				else if(myApi.Gige_Camera_isCameraFeature(hCamera,GigeApi.CameraFeature.CAMERA_FEATURE_COLORDEPTH_12BPP)==true)
				{
                    
					errorflag = myApi.Gige_Camera_setPixelDepth(hCamera,GigeApi.SvGigePixelDepth.SVGIGE_PIXEL_DEPTH_12);
					if(errorflag!=GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
					{
						MessageBox.Show("Errorflag 12bit: "+ errorflag.ToString());
						this.Close();
					}
                 
					gigeBitcount=12;
				}
				else
				{
					errorflag = myApi.Gige_Camera_setPixelDepth(hCamera,GigeApi.SvGigePixelDepth.SVGIGE_PIXEL_DEPTH_8);
					if(errorflag!=GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
					{
						MessageBox.Show("Errorflag 8bit: "+ errorflag.ToString());
						this.Close();
					}
					gigeBitcount=8;
				}
                ***/
                errorflag = myApi.Gige_Camera_setPixelDepth(hCamera, GigeApi.SvGigePixelDepth.SVGIGE_PIXEL_DEPTH_8);
                if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                {
                    MessageBox.Show("Errorflag 8bit: " + errorflag.ToString());
                    this.Close();
                }
                gigeBitcount = 8;

				
				//MessageBox.Show("myCamController initialised bitcount= " + bitcount+ " GigE-bitcount= " + gigeBitcount);
			}
			camWidth = 0;
			errorflag = myApi.Gige_Camera_getSizeX(hCamera,  ref(camWidth));
			if(errorflag!=GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
			{
				MessageBox.Show("Errorflag: "+ errorflag.ToString());
				this.Close();
			}

			camHeight = 0;

			errorflag = myApi.Gige_Camera_getSizeY(hCamera, ref(camHeight));

			this.camDispl.setGigeHandles(myApi,hCamera,hCameraContainer,camWidth,camHeight,modelname,gigeBitcount);

			//MessageBox.Show("CamWidth: "+ camWidth+ "; CamHeight:"+camHeight);
			
		}

		public int getSelectedIndex()
		{
			return selectedIndex;
		}

		
		private bool ChooseMyCamera()
		
		{
			bool ret = false;

			myApi = new GigeApi();
            ret = this.getCameras();

            return ret; 
			
		
		}

		private void GigeAuswahl_Load(object sender, System.EventArgs e)
		{
			this.ChooseMyCamera();
		}

        //---------------------------------------------------------------------------
        // sucht nach allen Kameras im Netzwerk und fügt sie in die Listbox hinzu
        //---------------------------------------------------------------------------

        private bool getCameras()
        {
            
            int camAnz;
            int n;
            string SN;
            if (hCameraContainer != 0)
            {
                myApi.Gige_CameraContainer_delete(hCameraContainer); 
            }
            hCameraContainer = myApi.Gige_CameraContainer_create(GigeApi.SVGigETL_Type.SVGigETL_TypeFilter);
			//hCameraContainer = myApi.Gige_CameraContainer_create(GigeApi.SVGigETL_Type.SVGigETL_TypeWinSock);


            if (hCameraContainer >= 0)
            {
                errorflag = myApi.Gige_CameraContainer_discovery(hCameraContainer);

                if (errorflag == GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                {
                    camAnz = myApi.Gige_CameraContainer_getNumberOfCameras(hCameraContainer);

                    if (camAnz > 0)
                    {

                        //MessageBox.Show("Camera Anzahl: " + camAnz);

                        for (n = 0; n < camAnz; n++)
                        {
                            hCamera = myApi.Gige_CameraContainer_getCamera(hCameraContainer, n);
                            modelname = myApi.Gige_Camera_getModelName(hCamera);
                            SN = myApi.Gige_Camera_getSerialNumber(hCamera);
                            //GigeAuswahllistBox.Items.Add(modelname + " SN: " + SN);

                            GigeComboBox.Items.Add(modelname + " SN: " + SN);
                        }                     

                    }
                    return true;

                }
                else
                {
                    MessageBox.Show("Error Containerdiscovery:");
                    this.Close();
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Error ContainerCreate:");
                this.Close();
                return false;
            }


            return false;

        }

        private void refreshbutton_Click(object sender, EventArgs e)
        {
            
            
            GigeAuswahllistBox.Items.Clear();
            
            this.getCameras();
        }

        private void GigeAuswahlcomboBox_Click(object sender, EventArgs e)
        {

        }

		
	}
}
