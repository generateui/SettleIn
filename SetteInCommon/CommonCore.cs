using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettleInCommon
{
    public class CommonCore
    {
        private static CommonCore _Instance = new CommonCore();

        public static CommonCore Instance
        {
            get { return _Instance; }
        }

        public CommonCore()
        {
            _Random = new Random();
        }

        /// <summary>
        /// Random field to minimize new creation of Random objects,
        /// which according to the documentation can yield same results
        /// when instantiated more then once
        /// </summary>
        private Random _Random;

        public Random Random
        {
            get { return _Random; }
        }
    }
}
