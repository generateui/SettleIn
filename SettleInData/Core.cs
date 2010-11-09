using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SettleInData;

namespace SettleInData
{
    public class Core
    {
        private static Core _Instance = new Core();
        private static object singletonLock = new object();
        private SIEntities _Entities;
        private UserAdministration _UserAdministration = new UserAdministration();

        public UserAdministration UserAdministration
        {
            get { return _UserAdministration; }
        }

        public SIEntities Entities
        {
            get { return _Entities; }
        }

        public static Core Instance
        {
            get
            {
                lock (singletonLock)
                {
                    return _Instance;
                }
            }
        }

        private Core()
        {
            _Entities = new SIEntities();
            _Entities.Connection.Open();
        }
    }
}
