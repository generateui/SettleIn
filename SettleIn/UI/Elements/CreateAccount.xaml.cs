using System;
using System.Collections.Generic;
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
using System.Windows.Media.Animation;

using SettleInCommon;
using SettleInCommon.Actions;
using SettleInCommon.Board;
using SettleInCommon.User;

using SettleIn.DataServerInstance;

namespace SettleIn.UI.Elements
{
	/// <summary>
	/// Interaction logic for CreateAccount.xaml
	/// </summary>
	public partial class CreateAccount : UserControl
	{
        private delegate void ProxySingletonProxyEventDelegate(object sender, ProxyCallBackEventArgs e);

        private Dictionary<string, bool> _UsersChecked = 
            new Dictionary<string, bool>();

        public CreateAccount()
		{
			this.InitializeComponent();
        }

        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            IsUserNameTakenRequest req = new IsUserNameTakenRequest();
            req.name = txtName.Text;
            DataServerInstance.Service1Client ds = new Service1Client();
            ds.IsUserNameTakenCompleted += new EventHandler<IsUserNameTakenCompletedEventArgs>(ds_IsUserNameTakenCompleted);
            ds.IsUserNameTakenAsync(req);
        }

        void ds_IsUserNameTakenCompleted(object sender, SettleIn.DataServerInstance.IsUserNameTakenCompletedEventArgs e)
        {
            IsUserNameTakenResponse resp = e.Result;
            if (!resp.IsUserNameTakenResult)
                lblCheckName.Content = "User name is free!";
            else
                lblCheckName.Content = "User name is taken :(";
        }

        private void pwdConfirmPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!CheckPasswordIsSame())
            {
                lblCheckPassword.Content = "Entered passwords are not equal.";
            }
            else
            {
                lblCheckPassword.Content = "Passwords are equal";
            }
            CheckInput();
        }

        private void btnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            spRegisterstatus.Visibility = Visibility.Visible;
            lblRegisterStatus.Content = String.Format("Trying to register \"{0}\"...", txtName.Text);
            
            SettleIn.DataServerInstance.RegisterRequest req = new SettleIn.DataServerInstance.RegisterRequest();
            req.email = txtEmail.Text;
            req.name = txtName.Text;
            req.pasword = pwdPassword.Text;
            Core.Instance.DataClient.RegisterCompleted += new EventHandler<RegisterCompletedEventArgs>(ds_RegisterCompleted);
            Core.Instance.DataClient.RegisterAsync(req);
        }

        void ds_RegisterCompleted(object sender, RegisterCompletedEventArgs e)
        {
            spinnerrr.Visibility = Visibility.Hidden;
            if (e.Result.RegisterResult != null)
            {
                lblRegisterStatus.Content = String.Format("Registering \"{0}\" succeeded!", txtName.Text);
                if (Properties.Settings.Default.Credentials == null)
                {
                    Properties.Settings.Default.Credentials = new XmlUserCredentials();
                }
                Properties.Settings.Default.Credentials.Name = txtName.Text;
                Properties.Settings.Default.Credentials.Password = pwdConfirmPassword.Text;
                Properties.Settings.Default.Save();

                string nextMessage = String.Format("{0} saved in settings as default player.", txtName.Text);
            }
            else
            {
                lblRegisterStatus.Content = "Registering failed.";
            }
        }

        private void CheckInput()
        {
            if (!CheckPasswordIsSame())
            {
                DisableCreateAccountButton();
                return;
            }
            if (_UsersChecked.ContainsKey(txtName.Text))
            {
                if (_UsersChecked[txtName.Text])
                {
                    DisableCreateAccountButton();
                    return;
                }
            }
            EnableCreateAccountButton();
        }
        private bool CheckPasswordIsSame()
        {
            return true; // pwdConfirmPassword.SecurePassword.Equals(pwdPassword.SecurePassword);
        }
        private void DisableCreateAccountButton()
        {
            btnCreateAccount.IsEnabled = false;
        }
        private void EnableCreateAccountButton()
        {
            btnCreateAccount.IsEnabled = true;
        }
    }
}