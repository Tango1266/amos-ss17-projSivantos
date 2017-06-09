﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace RaspberryBackend
{
    /// <summary>
    /// Software representation of the RaspberryPi. It contains all component representations which are phyisical connected to the Rpi.
    /// </summary>
    public partial class RaspberryPi
    {
        //Single location for all Hardware Components
        private Dictionary<String, HWComponent> _hwComponents = new Dictionary<String, HWComponent>();

        private static readonly RaspberryPi _instance = new RaspberryPi();

        //flags for robustness and testing
        private bool _initialized = false;
        private bool testMode = true;

        public static RaspberryPi Instance { get => _instance; }

        private RaspberryPi() { }

        /// <summary>
        /// Default initialization of the Raspberry Pi. It initialize the preconfigured Hardware of the Raspberry Pi. To add aditional hardware, just put it as a new parameter.
        /// </summary>
        public void initialize()
        {
            testMode = false;
            initialize(
                new GPIOinterface(),
                new LCD(),
                new Potentiometer(),
                new Multiplexer(),
                new ADCDAC()
                );
        }

        /// <summary>
        /// Customized initialization of the Raspberry Pi. It initialize the desired Hardware configuration of the Raspberry Pi.
        /// </summary>
        /// <param name="hwComponents">
        /// Hardware Componens which shall be connected to the Raspi.
        /// Enter as many components as desired devided with ','. e.g. (HWComponent one, HWComponent two)
        /// </param>
        public void initialize(params HWComponent[] hwComponents)
        {
            foreach (HWComponent hwComponent in hwComponents)
            {
                System.Diagnostics.Debug.WriteLine("Add new Hardware to Pi: " + hwComponent.GetType().Name);


                addToRasPi(hwComponent);

                System.Diagnostics.Debug.WriteLine("Get Type : " + this.GetType().Name);
                System.Diagnostics.Debug.WriteLine("Get Property of type : " + this.GetType().GetProperty(hwComponent.GetType().AssemblyQualifiedName));

                var instanceVariable = this.GetType().GetProperty(hwComponent.GetType().Name);

                if (instanceVariable != null)
                {
                    instanceVariable.SetValue(this, Convert.ChangeType(hwComponent, hwComponent.GetType()));
                }
            }

            initializeHWComponents();

            _initialized = true;
        }

        private void addToRasPi(HWComponent hwComponent)
        {
            _hwComponents.Add(hwComponent.GetType().Name, hwComponent);
            _hwComponents.ToString();
        }

        /// <summary>
        /// Deletes thecurrent Hardware Configuration of the Raspberry Pi. For now it is used for Testing.
        /// </summary>
        public void reset()
        {
            _hwComponents = new Dictionary<String, HWComponent>();
            _initialized = false;

            //missing, resetting all individual Hardware Components
        }

        /// <summary>
        /// Return whether raspberrypi and it's hardware components are initialized
        /// </summary>
        /// <returns></returns>
        public Boolean isInitialized()
        {
            return _initialized & hwComponentsInitialized();
        }

        private bool hwComponentsInitialized()
        {
            foreach (var hwComponent in _hwComponents.Values)
            {
                if (!hwComponent.isInitialized())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
