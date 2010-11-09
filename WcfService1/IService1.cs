using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using SettleInCommon.User;
using SettleInCommon.Gaming;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        List<XmlUser> GetAllUsers();

        [OperationContract]
        bool IsUserNameTaken(string name);

        [OperationContract]
        XmlUser NewestUser();

        [OperationContract]
        XmlGameResult MostRecentGame();

        [OperationContract]
        XmlUser Register(string name, string pasword, string email);
    }
}
