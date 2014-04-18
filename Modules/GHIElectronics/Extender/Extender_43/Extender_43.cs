﻿using GTM = Gadgeteer.Modules;
using GTI = Gadgeteer.SocketInterfaces;

namespace Gadgeteer.Modules.GHIElectronics
{
    /// <summary>
    /// An Extender module for Microsoft .NET Gadgeteer.
    /// </summary>
    public class Extender : GTM.Module
    {
        private Socket socketA;
        private Socket socketB;

        /// <summary>Constructs a new instance.</summary>
        /// <param name="socketNumber">The mainboard socket that has the module plugged into it.</param>
        public Extender(int socketNumber)
        {
            this.socketA = Socket.GetSocket(socketNumber, true, this, null);

            this.socketB = Socket.SocketInterfaces.CreateUnnumberedSocket(socketNumber.ToString() + "-" + " Extender");
            this.socketB.SupportedTypes = this.socketA.SupportedTypes;
            this.socketB.CpuPins[3] = this.socketA.CpuPins[3];
            this.socketB.CpuPins[4] = this.socketA.CpuPins[5];
            this.socketB.CpuPins[5] = this.socketA.CpuPins[4];
            this.socketB.CpuPins[6] = this.socketA.CpuPins[7];
            this.socketB.CpuPins[7] = this.socketA.CpuPins[6];
            this.socketB.SerialPortName = this.socketA.SerialPortName;
            this.socketB.SPIModule = this.socketA.SPIModule;
            this.socketB.AnalogOutput5 = this.socketA.AnalogOutput5;
            this.socketB.AnalogInput3 = this.socketA.AnalogInput3;
            this.socketB.AnalogInput4 = this.socketA.AnalogInput4;
            this.socketB.AnalogInput5 = this.socketA.AnalogInput5;
            this.socketB.PWM7 = this.socketA.PWM7;
            this.socketB.PWM8 = this.socketA.PWM8;
            this.socketB.PWM9 = this.socketA.PWM9;
            this.socketB.AnalogInputIndirector = this.socketA.AnalogInputIndirector;
            this.socketB.AnalogOutputIndirector = this.socketA.AnalogOutputIndirector;
            this.socketB.DigitalInputIndirector = this.socketA.DigitalInputIndirector;
            this.socketB.DigitalIOIndirector = this.socketA.DigitalIOIndirector;
            this.socketB.DigitalOutputIndirector = this.socketA.DigitalOutputIndirector;
            this.socketB.I2CBusIndirector = this.socketA.I2CBusIndirector;
            this.socketB.InterruptIndirector = this.socketA.InterruptIndirector;
            this.socketB.PwmOutputIndirector = this.socketA.PwmOutputIndirector;
            this.socketB.SpiIndirector = this.socketA.SpiIndirector;
            this.socketB.SerialIndirector = this.socketA.SerialIndirector;

            Socket.SocketInterfaces.RegisterSocket(this.socketB);
        }

        /// <summary>
        /// Returns the socket number for socket on the module.
        /// </summary>
        public int ExtenderSocketB { get { return this.socketB.SocketNumber; } }

        /// <summary>
        /// Creates a digital input on the given pin.
        /// </summary>
        /// <param name="pin">The pin to create the interface on.</param>
        /// <param name="glitchFilterMode">The glitch filter mode for the interface.</param>
        /// <param name="resistorMode">The resistor mode for the interface.</param>
        /// <returns>The new interface.</returns>
        public GTI.DigitalInput CreateDigitalInput(Socket.Pin pin, GTI.GlitchFilterMode glitchFilterMode, GTI.ResistorMode resistorMode)
        {
            return GTI.DigitalInputFactory.Create(socketA, pin, glitchFilterMode, resistorMode, this);
        }

        /// <summary>
        /// Creates a digital output on the given pin.
        /// </summary>
        /// <param name="pin">The pin to create the interface on.</param>
        /// <param name="initialState">The initial state for the interface.</param>
        /// <returns>The new interface.</returns>
        public GTI.DigitalOutput CreateDigitalOutput(Socket.Pin pin, bool initialState)
        {
            return GTI.DigitalOutputFactory.Create(socketA, pin, initialState, this);
        }

        /// <summary>
        /// Creates a digital input/output on the given pin.
        /// </summary>
        /// <param name="pin">The pin to create the interface on.</param>
        /// <param name="initialState">The initial state for the interface.</param>
        /// <param name="glitchFilterMode">The glitch filter mode for the interface.</param>
        /// <param name="resistorMode">The resistor mode for the interface.</param>
        /// <returns>The new interface.</returns>
        public GTI.DigitalIO CreateDigitalIO(Socket.Pin pin, bool initialState, GTI.GlitchFilterMode glitchFilterMode, GTI.ResistorMode resistorMode)
        {
            return GTI.DigitalIOFactory.Create(socketA, pin, initialState, glitchFilterMode, resistorMode, this);
        }

        /// <summary>
        /// Creates an interrupt input on the given pin.
        /// </summary>
        /// <param name="pin">The pin to create the interface on.</param>
        /// <param name="glitchFilterMode">The glitch filter mode for the interface.</param>
        /// <param name="resistorMode">The resistor mode for the interface.</param>
        /// <param name="interruptMode">The interrupt mode for the interface.</param>
        /// <returns>The new interface.</returns>
        public GTI.InterruptInput CreateInterruptInput(Socket.Pin pin, GTI.GlitchFilterMode glitchFilterMode, GTI.ResistorMode resistorMode, GTI.InterruptMode interruptMode)
        {
            return GTI.InterruptInputFactory.Create(socketA, pin, glitchFilterMode, resistorMode, interruptMode, this);
        }

        /// <summary>
        /// Creates an analog input on the given pin.
        /// </summary>
        /// <param name="pin">The pin to create the interface on.</param>
        /// <returns>The new interface.</returns>
        public GTI.AnalogInput CreateAnalogInput(Socket.Pin pin)
        {
            return GTI.AnalogInputFactory.Create(socketA, pin, this);
        }

        /// <summary>
        /// Creates an analog output on the given pin.
        /// </summary>
        /// <param name="pin">The pin to create the interface on.</param>
        /// <returns>The new interface.</returns>
        public GTI.AnalogOutput CreateAnalogOutput(Socket.Pin pin)
        {
            return GTI.AnalogOutputFactory.Create(socketA, pin, this);
        }

        /// <summary>
        /// Creates a pwm output on the given pin.
        /// </summary>
        /// <param name="pin">The pin to create the interface on.</param>
        /// <returns>The new interface.</returns>
        public GTI.PwmOutput CreatePwmOutput(Socket.Pin pin)
        {
            return GTI.PwmOutputFactory.Create(socketA, pin, false, this);
        }
    }
}