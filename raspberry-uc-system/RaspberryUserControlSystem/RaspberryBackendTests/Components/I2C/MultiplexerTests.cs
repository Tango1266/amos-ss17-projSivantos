﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaspberryBackend;

namespace RaspberryBackendTests
{
    [TestClass]
    public class MultiplexerTests
    {
        Multiplexer mux;
        
        [TestInitialize]
        public void setUp()
        {
            mux = new Multiplexer();
        }

        //Tests if unknown requests creates the corresponding exception
        [TestMethod]
        public void testMethod()
        {
            
        }


        [TestCleanup]
        public void tearDown()
        {
           
        }
    }
}