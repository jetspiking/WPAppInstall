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
        public enum USBActions
        {
            Connect,
            Disconnect
        }

        public class USBDevice
        {
            public enum RegexResults
            {
                Ok,
                Error
            }

            public readonly String vendorId;
            public readonly String productId;
            public readonly String guid;
            public readonly RegexResults result;

            private static readonly String REGEX_CHECK = ".*((VID)|(PID))+.*{.*}";
            private static readonly String REGEX_GUID = "{.+?}";
            private static readonly String REGEX_VID = "VID_.+?(&|#)";
            private static readonly String REGEX_PID = "PID_.+?(&|#)";

            public USBDevice(String usbPropertyString)
            {
                Regex regex = new Regex(REGEX_CHECK);
                Match match = regex.Match(usbPropertyString);
                result = match.Success ? RegexResults.Ok : RegexResults.Error;

                Match vendorIdRegex = new Regex(REGEX_VID).Match(usbPropertyString);
                if (vendorIdRegex.Success)
                {
                    String vendorId = vendorIdRegex.Value;
                    this.vendorId = vendorId.Substring(4, vendorId.Length - 4 - 1);
                }

                Match productIdRegex = new Regex(REGEX_PID).Match(usbPropertyString);
                if (productIdRegex.Success)
                {
                    String productId = productIdRegex.Value;
                    this.productId = productId.Substring(4, productId.Length - 4 - 1);
                }

                Match guidRegex = new Regex(REGEX_GUID).Match(usbPropertyString);
                if (guidRegex.Success)
                {
                    String guid = guidRegex.Value;
                    this.guid = guid.Substring(1, guid.Length - 1 - 1);
                }
            }
        }

        private IUSBSubsriber _subscriberUSB;
        private NativeMethods.CallBack _onUSBConnectedCallback;
        private NativeMethods.CallBack _onUSBDisconnectedCallback;
        private System.Timers.Timer _timer;
        private bool _timeoutUSB = false;
        private USBActions _lastUSBAction = USBActions.Disconnect;
        private Object _syncLock = new object();
        private Object _valueLock = new object();

        public USBConnectionHandler(IUSBSubsriber subscriberUSB)
        {
            _subscriberUSB = subscriberUSB;

            _timer = new System.Timers.Timer(Utils.Constants.MIN_USB_TIME_MILLIS);
            _timer.AutoReset = false;
            _timer.Elapsed += _timer_Elapsed;

            _onUSBConnectedCallback = new NativeMethods.CallBack(this.OnUSBConnected);
            _onUSBDisconnectedCallback = new NativeMethods.CallBack(this.OnUSBDisconnected);
            NativeMethods.USBMonitorStart(_onUSBConnectedCallback, _onUSBDisconnectedCallback);
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (_syncLock)
            {
                _timeoutUSB = false;
                _timer.Stop();
            }
        }

        private bool VerifyTimer()
        {
            lock (_syncLock)
                if (!_timeoutUSB)
                {
                    _timer.Start();
                    _timeoutUSB = true;
                    return true;
                }
            return false;
        }

        private bool VerifyLastAction(USBActions calledFrom)
        {
            lock (_valueLock)
            {
                lock (_syncLock)
                {
                    _timer.Stop();
                    _timer.Start();
                }

                if (calledFrom == USBActions.Connect)
                    return _lastUSBAction == USBActions.Disconnect;
                else return _lastUSBAction == USBActions.Connect;
            }
        }

        private void OnUSBConnected(String name)
        {
            if (VerifyTimer() || VerifyLastAction(USBActions.Connect))
                lock (_valueLock)
                {
                    _subscriberUSB.NotifyConnected(name);
                    _lastUSBAction = USBActions.Connect;
                }
        }

        private void OnUSBDisconnected(String name)
        {
            if (VerifyTimer() || VerifyLastAction(USBActions.Disconnect))
                lock (_valueLock)
                {
                    _subscriberUSB.NotifyDisconnected(name);
                    _lastUSBAction = USBActions.Disconnect;
                }
        }
    }
}
