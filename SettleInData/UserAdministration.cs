using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

using SettleInData;
using SettleInCommon;
using SettleInCommon.User;

namespace SettleInData
{
    public class UserAdministration
    {
        /// <summary>
        /// Checks if a specified name already exists in the user database
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool CheckUserNameExists(string name)
        {
            return (from u in Core.Instance.Entities.User
                    where u.Name.ToLower().Trim() == name.ToLower()
                    select u)
                    .Count() > 0;
        }

        public XmlUser AddUser(string name, string password, string email, UserSettings settings)
        {
            User u = new User();
            u.Name = name.Trim();
            u.PasswordHash = GetSHA1(password);
            u.Email = email;
            
            UserSettings userSettings = settings;
            if (userSettings == null)
            {
                userSettings = new UserSettings();
                userSettings.FirstColor = new XmlColor(255, 0, 0);  //red
                userSettings.FirstColor = new XmlColor(255, 255, 255);   //white
            }

            Core.Instance.Entities.AddToUser(u);
            Core.Instance.Entities.SaveChanges();
            Console.WriteLine(String.Format("New user added: \"{0}\"", name));

            return u.BuildXmlUser();
        }

        public XmlUser Authenticate(XmlUserCredentials credentials)
        {
            string password = GetSHA1(credentials.Password);

            var users = (from u in Core.Instance.Entities.User 
                     where (u.Name.ToLower() == credentials.Name.ToLower() && 
                     password.Equals(u.PasswordHash))
                     select u);

            if (users.Count() == 0) return null;

            if (users.Count() == 1)
            {
                return users.First().BuildXmlUser();
            }
            else
            {
                return null;
            }

        }

        private string GetSHA1(string strPlain)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] HashValue, MessageBytes = UE.GetBytes(strPlain);
            SHA1Managed SHhash = new SHA1Managed();
            StringBuilder strHex = new StringBuilder();

            HashValue = SHhash.ComputeHash(MessageBytes);
            foreach (byte b in HashValue)
            {
                strHex.Append(String.Format("{0:x2}", b));
            }
            return strHex.ToString();
        } 
    }
}
