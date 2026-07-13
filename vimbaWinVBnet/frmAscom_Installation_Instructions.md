# frmAscom Installation Instructions

The frmAscom form has been created with the following files:
- vimbaWinVBnet\frmAscom.vb
- vimbaWinVBnet\frmAscom.Designer.vb  
- vimbaWinVBnet\frmAscom.resx

## Manual Steps Required (to be done in Visual Studio):

### 1. Add ASCOM.Platform NuGet Package
The project requires ASCOM.Platform package to work with ASCOM cameras.

**Steps:**
1. Right-click on the `vimbaWinVBnet` project in Solution Explorer
2. Select "Manage NuGet Packages..."
3. Search for "ASCOM.Platform"
4. Install version 6.5.2 (or latest stable version)

This will add the following assemblies:
- ASCOM.Astrometry
- ASCOM.Attributes
- ASCOM.DeviceInterfaces
- ASCOM.DriverAccess
- ASCOM.Exceptions
- ASCOM.Utilities

### 2. Add the Form Files to the Project
Since the solution is currently open, the project file cannot be modified programmatically.

**Steps:**
1. In Visual Studio Solution Explorer, right-click on the `vimbaWinVBnet` project
2. Select "Add" > "Existing Item..."
3. Navigate to the vimbaWinVBnet folder
4. Select these three files (hold Ctrl to select multiple):
   - frmAscom.vb
   - frmAscom.Designer.vb
   - frmAscom.resx
5. Click "Add"

Visual Studio should automatically detect that frmAscom.Designer.vb and frmAscom.resx are dependent on frmAscom.vb and nest them appropriately.

### 3. Build the Project
After completing steps 1 and 2:
1. Build the solution (Ctrl+Shift+B)
2. Verify there are no compilation errors

## Form Features

The frmAscom form provides the following capabilities:

### ASCOM Camera Integration
- **Camera Selection**: Dropdown populated with all registered ASCOM cameras on the system
- **Refresh Button**: Reload the list of available ASCOM cameras
- **Connection**: Automatic connection when Start is clicked

### Inherited from frmMaster
All standard camera control features are available:
- Day/Night exposure settings
- Gain control
- Image path configuration
- Meteor detection integration
- Image saving (JPEG with quality control)
- Web server for remote viewing
- FPS monitoring
- Dark frame support (via UI controls)

### Image Acquisition
- Continuous acquisition mode
- Automatic image format conversion (ASCOM arrays to bitmaps)
- Automatic exposure and gain adjustment based on day/night mode
- Integration with detection queue for meteor processing

## Usage

1. Launch the application
2. The form will automatically enumerate available ASCOM cameras
3. Select your camera from the dropdown
4. Configure exposure and gain settings
5. Click "Start" to begin acquisition
6. The form will connect to the camera and start displaying images
7. Click "Stop" to end acquisition and disconnect

## Troubleshooting

### "No ASCOM cameras found"
- Ensure ASCOM camera drivers are installed
- Verify cameras are registered in ASCOM (run ASCOM Diagnostics)
- Click "Refresh" button to reload the camera list

### "Error connecting to camera"
- Check that the camera is powered on and connected
- Verify no other application is using the camera
- Some ASCOM cameras require additional driver-specific setup

### Compilation Errors
- Verify ASCOM.Platform NuGet package is installed
- Check that all three form files are added to the project
- Clean and rebuild the solution

## Technical Details

### ASCOM Interface Used
- **Camera Enumeration**: ASCOM.Utilities.Profile.RegisteredDevices()
- **Camera Control**: ASCOM.DriverAccess.Camera
- **Image Acquisition**: StartExposure(), ImageReady, ImageArray

### Threading
- Image acquisition runs in a separate thread to prevent UI blocking
- Detection processing runs in a separate thread (inherited from frmMaster)

### Image Format Conversion
- ASCOM ImageArray (2D integer array) is converted to Bitmap
- Automatic scaling from camera bit depth to 8-bit display
- Maintains original dynamic range information for processing
