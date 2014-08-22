﻿using GTM = Gadgeteer.Modules;

namespace Gadgeteer.Modules.GHIElectronics
{
    /// <summary>
    /// A Generi cDaisylink module used for Microsoft .NET Gadgeteer
    /// </summary>
    /// /// <example>
    /// <para>The following example uses a <see cref="DL40"/> object to turn an LED on. 
    /// </para>
    /// <code>
    /// using System;
    /// using System.Collections;
    /// using System.Threading;
    /// using Microsoft.SPOT;
    /// using Microsoft.SPOT.Presentation;
    /// using Microsoft.SPOT.Presentation.Controls;
    /// using Microsoft.SPOT.Presentation.Media;
    /// using Microsoft.SPOT.Touch;
    ///
    /// using Gadgeteer.Networking;
    /// using GT = Gadgeteer;
    /// using GTM = Gadgeteer.Modules;
    /// using Gadgeteer.Modules.GHIElectronics;
    ///
    /// namespace TestApp
    /// {
    ///     public partial class Program
    ///     {
    ///         void ProgramStarted()
    ///         {
    ///             // Keep in mind that the first argument is the register that you wish to
    ///             //      write to, and the second argument is the value you wish to write 
    ///             //      to that register
    ///
    ///             // The port that you want to write to
    ///             genericDaisylink.WriteRegister(1, 3);
    ///
    ///             // The pin on the specified port that you want to write to
    ///             genericDaisylink.WriteRegister(2, 0);
    ///
    ///             // The direction that you want the pin to be:
    ///             // 1 == output
    ///             // 0 == input
    ///             genericDaisylink.WriteRegister(3, 1);
    ///
    ///             // The value that you want to write
    ///             // 1 == High
    ///             // 0 == Low
    ///             genericDaisylink.WriteRegister(4, 1);
    ///
    ///             // Enables or disables the pin
    ///             // 1 == Enable
    ///             // 0 == Disable
    ///             genericDaisylink.WriteRegister(0, 1);
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    public class DL40 : GTM.DaisyLinkModule
    {
        // -- CHANGE FOR MICRO FRAMEWORK 4.2 --
        // If you want to use Serial, SPI, or DaisyLink (which includes GTI.SoftwareI2C), you must do a few more steps
        // since these have been moved to separate assemblies for NETMF 4.2 (to reduce the minimum memory footprint of Gadgeteer)
        // 1) add a reference to the assembly (named Gadgeteer.[interfacename])
        // 2) in GadgeteerHardware.xml, uncomment the lines under <Assemblies> so that end user apps using this module also add a reference.

        private const byte GHI_DAISYLINK_MANUFACTURER = 0x10;
        private const byte GHI_DAISYLINK_TYPE_GENERIC = 0x01;
        private const byte GHI_DAISYLINK_VERSION_GENERIC = 0x01;

        // Note: A constructor summary is auto-generated by the doc builder.
        /// <summary></summary>
        /// <param name="socketNumber">The socket that this module is plugged in to.</param>
        public DL40(int socketNumber)
            : base(socketNumber, GHI_DAISYLINK_MANUFACTURER, GHI_DAISYLINK_TYPE_GENERIC, GHI_DAISYLINK_VERSION_GENERIC, GHI_DAISYLINK_VERSION_GENERIC, 50, "Generic")
        {
            // This finds the Socket instance from the user-specified socket number.  
            // This will generate user-friendly error messages if the socket is invalid.
            // If there is more than one socket on this module, then instead of "null" for the last parameter, 
            // put text that identifies the socket to the user (e.g. "S" if there is a socket type S)
            Socket socket = Socket.GetSocket(socketNumber, true, this, null);
        }

        /// <summary>
        /// Writes to the daisylink register specified by the address. Does not allow writing to the reserved registers.
        /// </summary>
        /// <param name="address">Address of the register.</param>
        /// <param name="writebuffer">Byte to write.</param>
        public void WriteRegister(byte address, byte writebuffer)
        {
            WriteParams((byte)(DaisyLinkOffset + address), (byte)writebuffer);
        }

        /// <summary>
        /// Reads a byte from the specified register. Allows reading of reserved registers.
        /// </summary>
        /// <param name="memoryaddress">Address of the register.</param>
        /// <returns></returns>
        public byte ReadRegister(byte memoryaddress)
        {
            return Read(memoryaddress);
        }
    }
}