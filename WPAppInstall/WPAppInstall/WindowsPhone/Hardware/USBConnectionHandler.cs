using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;

namespace WPAppInstall.WindowsPhone.Hardware
{
    /// <summary>
    /// Handle USB connections.
    /// </summary>
    public class USBConnectionHandler
    {
        /// <summary>
        /// Devices can be connected or disconnected using USB.
        /// </summary>
        public enum USBActions
        {
            Connect,
            Disconnect
        }

        /// <summary>
        /// This class contains actions specific for USB devices.
        /// </summary>
        public class USBDevice
        {
            /// <summary>
            /// Ok or error are possible results for matching the regex.
            /// </summary>
            public enum RegexResults
            {
                Ok,
                Error
            }

            public readonly String VendorId;
            public readonly String ProductId;
            public readonly String Guid;
            public readonly RegexResults RegexResult;

            private static readonly String regexCheck = ".*((VID)|(PID))+.*{.*}";
            private static readonly String regexGuid = "{.+?}";
            private static readonly String regexVid = "VID_.+?(&|#)";
            private static readonly String regexPid = "PID_.+?(&|#)";

            /// <summary>
            /// Create a usb device and it's corresponding properties by attempting to match regex patterns.
            /// </summary>
            /// <param name="usbPropertyString">Property string of the usb device.</param>
            public USBDevice(String usbPropertyString)
            {
                Regex regex = new Regex(regexCheck);
                Match match = regex.Match(usbPropertyString);
                RegexResult = match.Success ? RegexResults.Ok : RegexResults.Error;

                Match vendorIdRegex = new Regex(regexVid).Match(usbPropertyString);
                if (vendorIdRegex.Success)
                {
                    String vendorId = vendorIdRegex.Value;
                    this.VendorId = vendorId.Substring(4, vendorId.Length - 4 - 1);
                }

                Match productIdRegex = new Regex(regexPid).Match(usbPropertyString);
                if (productIdRegex.Success)
                {
                    String productId = productIdRegex.Value;
                    this.ProductId = productId.Substring(4, productId.Length - 4 - 1);
                }

                Match guidRegex = new Regex(regexGuid).Match(usbPropertyString);
                if (guidRegex.Success)
                {
                    String guid = guidRegex.Value;
                    this.Guid = guid.Substring(1, guid.Length - 1 - 1);
                }
            }
        }

        private IUSBSubsriber subscriberUSB;
        private NativeMethods.CallBack onUSBConnectedCallback;
        private NativeMethods.CallBack onUSBDisconnectedCallback;
        private System.Timers.Timer timer;
        private Boolean timeoutUSB = false;
        private USBActions lastUSBAction = USBActions.Disconnect;
        private Object syncLock = new Object();
        private Object valueLock = new Object();

        /// <summary>
        /// Create a usb connection handler.
        /// </summary>
        /// <param name="subscriberUSB">Callback for the usb connect or disconnect action.</param>
        public USBConnectionHandler(IUSBSubsriber subscriberUSB)
        {
            this.subscriberUSB = subscriberUSB;

            timer = new System.Timers.Timer(Utils.Constants.MinUsbTimeMillis);
            timer.AutoReset = false;
            timer.Elapsed += _timer_Elapsed;

            onUSBConnectedCallback = new NativeMethods.CallBack(this.OnUSBConnected);
            onUSBDisconnectedCallback = new NativeMethods.CallBack(this.OnUSBDisconnected);

            NativeMethods.USBMonitorStart(onUSBConnectedCallback, onUSBDisconnectedCallback);
        }

        /// <summary>
        /// If the timer elapsed, stop the timer and disable the usb timeout.
        /// </summary>
        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (syncLock)
            {
                timeoutUSB = false;
                timer.Stop();
            }
        }

        /// <summary>
        /// Enable the usb timeout if previously set to false.
        /// </summary>
        /// <returns>Describes whether the usb timeout has been applied.</returns>
        private Boolean VerifyTimer()
        {
            lock (syncLock)
                if (!timeoutUSB)
                {
                    timer.Start();
                    timeoutUSB = true;
                    return true;
                }
            return false;
        }

        /// <summary>
        /// Verify the last usb action.
        /// </summary>
        /// <param name="calledFrom">USBAction should be swapped.</param>
        /// <returns>Updates the last action if a lock can be acquired.</returns>
        private Boolean VerifyLastAction(USBActions calledFrom)
        {
            lock (valueLock)
            {
                lock (syncLock)
                {
                    timer.Stop();
                    timer.Start();
                }

                if (calledFrom == USBActions.Connect)
                    return lastUSBAction == USBActions.Disconnect;
                else return lastUSBAction == USBActions.Connect;
            }
        }

        /// <summary>
        /// Notify the usb subscriber on a connect action.
        /// </summary>
        /// <param name="name">Name of the usb device connected</param>
        private void OnUSBConnected(String name)
        {
            if (VerifyTimer() || VerifyLastAction(USBActions.Connect))
                lock (valueLock)
                {
                    subscriberUSB.NotifyConnected(name);
                    lastUSBAction = USBActions.Connect;
                }
        }

        /// <summary>
        /// Notify the usb subscriber on a disconnect action.
        /// </summary>
        /// <param name="name">Name of the usb device disconnected</param>
        private void OnUSBDisconnected(String name)
        {
            if (VerifyTimer() || VerifyLastAction(USBActions.Disconnect))
                lock (valueLock)
                {
                    subscriberUSB.NotifyDisconnected(name);
                    lastUSBAction = USBActions.Disconnect;
                }
        }
    }
}
