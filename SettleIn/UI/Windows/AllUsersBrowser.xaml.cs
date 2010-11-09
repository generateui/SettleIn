using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

using SettleInCommon.Actions;
using SettleInCommon.Gaming;
using SettleInCommon.Board;
using SettleInCommon.User;
using SettleIn.DataServerInstance;

namespace SettleIn.UI
{
    /// <summary>
    /// Interaction logic for AllUsersBrowser.xaml
    /// </summary>
    public partial class AllUsersBrowser : UserControl
    {
        private delegate void ProxySingletonProxyEventDelegate(object sender, ProxyCallBackEventArgs e);
        public AllUsersBrowser()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(AllUsersBrowser_Loaded);
        }

        void AllUsersBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            Core.Instance.DataClient.GetAllUsersCompleted += new EventHandler<SettleIn.DataServerInstance.GetAllUsersCompletedEventArgs>(DataServiceClient_GetAllUsersCompleted);
            
            DataServerInstance.GetAllUsersRequest req = new GetAllUsersRequest();

            Core.Instance.DataClient.GetAllUsersAsync(req);
            /*
            Server.Instance.Connect(new XmlUserCredentials("piet", "test"));
            RequestAllPlayersAction getPlayers = new RequestAllPlayersAction();
            getPlayers.Sender = new XmlUser("piet");
            Server.Instance.SayAndClear("TestUser", getPlayers, false);
            */
            // Update UI request has been sent
            // do that here

        }

        void DataServiceClient_GetAllUsersCompleted(object sender, SettleIn.DataServerInstance.GetAllUsersCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                lbxGames.DataContext = e.Result.GetAllUsersResult;
            }
        }

        private void lbxGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
