﻿using Microsoft.SPOT;

using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using GTI = Gadgeteer.Interfaces;

using Microsoft.SPOT.Hardware;
using System;

namespace Gadgeteer.Modules.GHIElectronics
{
    // -- CHANGE FOR MICRO FRAMEWORK 4.2 --
    // If you want to use Serial, SPI, or DaisyLink (which includes GTI.SoftwareI2C), you must do a few more steps
    // since these have been moved to separate assemblies for NETMF 4.2 (to reduce the minimum memory footprint of Gadgeteer)
    // 1) add a reference to the assembly (named Gadgeteer.[interfacename])
    // 2) in GadgeteerHardware.xml, uncomment the lines under <Assemblies> so that end user apps using this module also add a reference.

    /// <summary>
    /// A 3.5 inch TFT display module with resistive touch for Microsoft .NET Gadgeteer.
    /// </summary>
    /// <example>
    /// <para>The following example uses a <see cref="DisplayT35"/> object to display the picture taken by a camera module. 
    /// First the code initializes a camera object and the button pressed event delegate in which the camera takes a picture.
    /// Then, another delegate is initialized to handle the asynchronous PictureCaptured event.  In this method the display module uses 
    /// the SimpleGraphics class to display the picture captured by the camera.
    /// </para>
    /// <code>
    /// using System;
    /// using Microsoft.SPOT;
    /// using Microsoft.SPOT.Presentation;
    /// using Microsoft.SPOT.Presentation.Controls;
    /// using Microsoft.SPOT.Presentation.Media;
    ///
    /// using GT = Gadgeteer;
    /// using GTM = Gadgeteer.Modules;
    ///
    /// using Gadgeteer.Modules.GHIElectronics;
    ///
    /// namespace TestApp
    /// {
    ///     public partial class Program
    ///     {
    ///         // This template uses the FEZ Spider mainboard from GHI Electronics
    ///
    ///         // Define and initialize GTM.Modules here, specifying their socket numbers.        
    ///         GTM.GHIElectronics.UsbClientDP usbClient = new UsbClientDP(1);
    ///         GTM.GHIElectronics.Button button = new Button(4);
    ///         GTM.GHIElectronics.Camera camera = new Camera(3);
    ///         GTM.GHIElectronics.Display_T35 display = new Display_T35(12, 13, 14);
    ///
    ///         void ProgramStarted()
    ///         {
    ///             // Initialize event handlers here.
    ///             button.ButtonPressed += new Button.ButtonEventHandler(button_ButtonPressed);
    ///             camera.PictureCaptured += new Camera.PictureCapturedEventHandler(camera_PictureCaptured);
    ///
    ///             // Do one-time tasks here
    ///             Debug.Print("Program Started");
    ///         }
    ///
    ///         void camera_PictureCaptured(Camera sender, GT.Picture picture)
    ///         {
    ///             Debug.Print("Picture Captured event.");
    ///             display.SimpleGraphics.DisplayImage(picture, 5, 5);
    ///         }
    ///
    ///         void button_ButtonPressed(Button sender, Button.ButtonState state)
    ///         {
    ///             camera.TakePicture();
    ///         }
    ///     }
    /// }
    /// 
    /// </code>
    /// </example>
    [Obsolete]
    public class DisplayT35 : GTM.Module.DisplayModule
    {
        private bool _bBackLightOn = true;

        /// <summary>
        /// Accessor for the state of the backlight
        /// </summary>
        public bool BBackLightOn
        {
            get { return _bBackLightOn; }
            //set { _bBackLightOn = value; }
        }

        private static GTI.DigitalOutput backlightPin;// = new OutputPort(greenSocket.CpuPins[9], true);

        /// <summary>
        /// Sets the backlight to the passed in value.
        /// </summary>
        /// <param name="bOn">Backlight state.</param>
        public void SetBacklight(bool bOn)
        {
            if (greenSocket != null)
            {
                backlightPin.Write(bOn);
                _bBackLightOn = bOn;
            }
            else
            {
                ErrorPrint("Cannot set backlight yet. RGB sockets not yet initialized");
            }
        }

        private bool _touchPanelEnabled;
        // This example implements  a driver in managed code for a simple Gadgeteer module.  The module uses a 
        // single GTI.InterruptInput to interact with a sensor that can be in either of two states: low or high.
        // The example code shows the recommended code pattern for exposing the property (IsHigh). 
        // The example also uses the recommended code pattern for exposing two events: Display_T35High, Display_T35Low. 
        // The triple-slash "///" comments shown will be used in the build process to create an XML file named
        // GTM.GHIElectronics.Display_T35. This file will provide Intellisense and documention for the
        // interface and make it easier for developers to use the Display T35 module.        

        // Note: A constructor summary is auto-generated by the doc builder.
        /// <summary></summary>
        /// <remarks>The ordering of the RGB socket numbers does not matter (socket numbers are autodetected).</remarks>
        /// <param name="rgbSocketNumber1">The mainboard socket that has the display's R, G, or B socket connected to it.</param>
        /// <param name="rgbSocketNumber2">The mainboard socket that has the display's R, G, or B socket connected to it.</param>
        /// <param name="rgbSocketNumber3">The mainboard socket that has the display's R, G, or B socket connected to it.</param>
        public DisplayT35(int rgbSocketNumber1, int rgbSocketNumber2, int rgbSocketNumber3)
            : this(rgbSocketNumber1, rgbSocketNumber2, rgbSocketNumber3, Socket.Unused)
        {
        }

        /// <summary>
        /// </summary>
        /// <remarks>The ordering of the RGB socket numbers does not matter (socket numbers are autodetected).</remarks>
        /// <param name="rgbSocketNumber1">The mainboard socket that has the display's R socket connected to it.</param>
        /// <param name="rgbSocketNumber2">The mainboard socket that has the display's G socket connected to it.</param>
        /// <param name="rgbSocketNumber3">The mainboard socket that has the display's B socket connected to it.</param>
        /// <param name="touchSocketNumber">Optional: the mainboard socket that has the display's T socket connected to it. 
        /// This enables the touch panel capabilities.</param>
        public DisplayT35(int rgbSocketNumber1, int rgbSocketNumber2, int rgbSocketNumber3, int touchSocketNumber)
            : base(WPFRenderOptions.Ignore)
        {
            ReserveLCDPins(rgbSocketNumber1, rgbSocketNumber2, rgbSocketNumber3);
            ConfigureLCD();

            if (touchSocketNumber == Socket.Unused) return;

            ReserveTouchPins(touchSocketNumber);
            GT.Program.BeginInvoke(new NullParamsDelegate(EnableTouchPanel), null);
        }

        private delegate void NullParamsDelegate();

        private static Socket greenSocket;
        private void ReserveLCDPins(int rgbSocketNumber1, int rgbSocketNumber2, int rgbSocketNumber3)
        {
            bool gotR = false, gotG = false, gotB = false;
            Socket[] rgbSockets = new Socket[3] { Socket.GetSocket(rgbSocketNumber1, true, this, "rgbSocket1"), Socket.GetSocket(rgbSocketNumber2, true, this, "rgbSocket2"), Socket.GetSocket(rgbSocketNumber3, true, this, "rgbSocket3") };

            foreach (var rgbSocket in rgbSockets)
            {
                if (!gotR && rgbSocket.SupportsType('R'))
                {
                    gotR = true;
                }
                else if (!gotG && rgbSocket.SupportsType('G'))
                {
                    gotG = true;
                    greenSocket = rgbSocket;
                    backlightPin = new GTI.DigitalOutput(greenSocket, Socket.Pin.Nine, true, this);
                }
                else if (!gotB && rgbSocket.SupportsType('B'))
                {
                    gotB = true;
                }
                else
                {
                    throw new GT.Socket.InvalidSocketException("Socket " + rgbSocket + " is not an R, G or B socket, as required for the LCD module.");
                }

                rgbSocket.ReservePin(Socket.Pin.Three, this);
                rgbSocket.ReservePin(Socket.Pin.Four, this);
                rgbSocket.ReservePin(Socket.Pin.Five, this);
                rgbSocket.ReservePin(Socket.Pin.Six, this);
                rgbSocket.ReservePin(Socket.Pin.Seven, this);
                rgbSocket.ReservePin(Socket.Pin.Eight, this);
    
                if (!rgbSocket.SupportsType('G'))
                    rgbSocket.ReservePin(Socket.Pin.Nine, this);
            }
        }

        private void ReserveTouchPins(int touchSocketNumber)
        {
            Socket tsocket = Socket.GetSocket(touchSocketNumber, true, this, "T");

            tsocket.EnsureTypeIsSupported('T', this);
            tsocket.ReservePin(Socket.Pin.Four, this);
            tsocket.ReservePin(Socket.Pin.Five, this);
            tsocket.ReservePin(Socket.Pin.Six, this);
            tsocket.ReservePin(Socket.Pin.Seven, this);
        }

        private void ConfigureLCD()
        {
            Mainboard.LCDConfiguration lcdConfig = new Mainboard.LCDConfiguration();

            lcdConfig.LCDControllerEnabled = true;

            lcdConfig.Width = Width;
            lcdConfig.Height = Height;

            // Only use if needed, see documentation.
            lcdConfig.PriorityEnable = false;

            lcdConfig.OutputEnableIsFixed = true;
            lcdConfig.OutputEnablePolarity = true;

            lcdConfig.HorizontalSyncPolarity = false;
            lcdConfig.VerticalSyncPolarity = false;
            lcdConfig.PixelPolarity = true;

            lcdConfig.HorizontalSyncPulseWidth = 41;
            lcdConfig.HorizontalBackPorch = 27;
            lcdConfig.HorizontalFrontPorch = 51;
            lcdConfig.VerticalSyncPulseWidth = 10;
            lcdConfig.VerticalBackPorch = 8;
            lcdConfig.VerticalFrontPorch = 16;
            
            lcdConfig.PixelClockDivider = 8;

            // Set configs
            DisplayModule.SetLCDConfig(lcdConfig);
        }

        private void EnableTouchPanel()
        {
            if (!_touchPanelEnabled)
            {
                // Initialize touch input                
                Microsoft.SPOT.Touch.Touch.Initialize(Application.Current);
                _touchPanelEnabled = true;
            }
        }

        /// <summary>
        /// Gets the width of the display.
        /// </summary>
        /// <remarks>
        /// This property always returns 320.
        /// </remarks>
        public override uint Width { get { return 320; } }

        /// <summary>
        /// Gets the height of the display.
        /// </summary>
        /// <remarks>
        /// This property always returns 240.
        /// </remarks>
        public override uint Height { get { return 240; } }

        /// <summary>
        /// Renders display data on the display device. 
        /// </summary>
        /// <param name="bitmap">The <see cref="T:Microsoft.SPOT.Bitmap"/> object to render on the display.</param>
        protected override void Paint(Bitmap bitmap)
        {
            try
            {
                bitmap.Flush();
            }
            catch
            {
                ErrorPrint("Painting error");
            }
        }

    }
}