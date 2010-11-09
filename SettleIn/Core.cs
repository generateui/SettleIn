using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

using SettleIn.DataServerInstance;
using SettleInCommon.User;
using SettleInCommon.Board;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using SettleIn.DataServerInstance;

namespace SettleIn
{
    /// <summary>
    /// Holds all instance information of the software
    /// </summary>
    public class Core
    {
        /// <summary>
        /// Singleton declaration
        /// </summary>
        private static Core _Instance = new Core();
        /// <summary>
        /// All textures for the 3D models used
        /// </summary>
        private ResourceDictionary _Textures;
        /// <summary>
        /// All icons used
        /// </summary>
        private ResourceDictionary _Icons;
        /// <summary>
        /// 3D models of All pieces
        /// </summary>
        private ResourceDictionary _Models;
        private ResourceDictionary _DataTemplates;

        public ResourceDictionary DataTemplates
        {
            get 
            {
                if (_DataTemplates == null)
                {
                    _DataTemplates = new ResourceDictionary();
                    _DataTemplates.Source = new Uri("Resources/DataTemplates.xaml", UriKind.Relative);
                }
                return _DataTemplates;
            }
        }

        private XmlUser _ServerUser = new XmlUser() { ID = 0, Name = "Server" };

        public XmlUser ServerUser
        {
            get { return _ServerUser; }
        }

        private ObservableCollection<XmlBoard> _Boards = new ObservableCollection<XmlBoard>();

        public ObservableCollection<XmlBoard> Boards
        {
            get { return _Boards; }
            set { _Boards = value; }
        }

        // Client to the data server instance
        DataServerInstance.Service1Client _DataServiceClient = new Service1Client();

        private XmlUser _CurrentPlayer = new XmlUser() { ID = 5, Name = "WieuwHost" };

        /// <summary>
        /// Random field to minimize new creation of Random objects,
        /// which according to the documentation can yield same results
        /// when instantiated more then once
        /// </summary>
        private Random _Random;

        public static Core Instance
        {
            get { return _Instance; }
        }
        private Core()
        {
            _Random = new Random();
            LoadBoards();
        }

        public DataServerInstance.Service1Client DataClient
        {
            get { return _DataServiceClient; }
        }

        public ResourceDictionary Textures
        {
            get
            {
                if (_Textures == null)
                {
                    _Textures = new ResourceDictionary();
                    _Textures.Source = new Uri("Resources/Textures.xaml", UriKind.Relative);
                }
                return _Textures;
            }
        }

        public ResourceDictionary Icons
        {
            get
            {
                if (_Icons == null)
                {
                    _Icons = new ResourceDictionary();
                    _Icons.Source = new Uri("Resources/Images.xaml", UriKind.Relative);
                }
                return _Icons;
            }
        }
        public ResourceDictionary Models
        {
            get
            {
                if (_Models == null)
                {
                    _Models = new ResourceDictionary();
                    _Models.Source = new Uri("Resources/Models.xaml", UriKind.Relative);
                }
                return _Models;
            }
        }

        public Random Random
        { get { return _Random; } }

        public XmlUser CurrentPlayer
        { 
            get { return _CurrentPlayer; }
            set { _CurrentPlayer = value; }
        }

        private void LoadBoards()
        {
            DirectoryInfo di = new DirectoryInfo(@"E:\Documents\SettleIn\Boards");
            foreach (FileInfo file in di.GetFiles("*.xml.sib"))
            {
                try
                {
                    DataContractSerializer xmlSerializer = new DataContractSerializer(typeof(XmlBoard));
                    XmlTextReader xmlreader = new XmlTextReader(di.ToString() + "\\" + file.ToString());
                    XmlBoard xmlBoard = (XmlBoard)xmlSerializer.ReadObject(xmlreader);
                    _Boards.Add(xmlBoard);
                }
                catch (Exception ex)
                {
                    // something went wrong, return a new empty list
                    //return new BoardLists();
                }
                finally
                {
                }
            }
        }

    }
}
