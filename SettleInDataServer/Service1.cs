using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.Entity;

//using SettleInData;
using SettleInCommon.User;
using SettleInCommon.Gaming;

namespace SettleInDataServer
{
    
    public class DataService : IDataService
    {
        private SIEntities _Model;
        private UserAdministration _UserAdministration = new UserAdministration();

        public DataService()
        {
            _Model = new SIEntities();
            _Model.Connection.Open();
        }

        #region IDataService Members

        public List<XmlUser> GetAllUsers()
        {
            List<XmlUser> result = new List<XmlUser>();

            foreach (User user in (from u in _Model.User select u))
                result.Add(user.BuildXmlUser());

            return result;
        }

        public bool IsUserNameTaken(string name)
        {
            return _UserAdministration.CheckUserNameExists(name);
        }

        public XmlUser NewestUser()
        {
            //var user = from u in _Model.User 
            //           where u.DateTimeRegistered
            return null;
        }

        public XmlGameResult MostRecentGame()
        {
            throw new NotImplementedException();
        }

        public XmlUser Register(string name, string password, string email)
        {
            /*
            // If the user exists, fail
            if (Core.Instance.UserAdministration.CheckUserNameExists(action.Name))
            {
                MessageFromServerAction response = new MessageFromServerAction();
                response.Message = "The name you are trying to register is already taken. Try again with a different name";
                response.Sender = _ServerUser;
                Whisper(action.Sender.Name, response);
                return;
            }

            //if the password is empty, fail
            if (String.IsNullOrEmpty(action.Password))
            {
                MessageFromServerAction response = new MessageFromServerAction();
                response.Message = "Empty password! Try again with a decent password ;)";
                response.Sender = _ServerUser;
                Whisper(action.Sender.Name, response);
                return;
            }
             
            if (IsUserNameTaken(name)) return null;
            return _UserAdministration.AddUser(name, password, email, null);
             */
            return null;
        }

        #endregion
    }

}
