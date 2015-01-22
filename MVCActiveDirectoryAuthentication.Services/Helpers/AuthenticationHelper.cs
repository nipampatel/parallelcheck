using MvcActiveDirectoryAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Linq;
using System.Web;

namespace MVCActiveDirectoryAuthentication.Services
{
    public class AuthenticationHelper
    {
        public static readonly string LDAP_CONNECTION_STRING = ConfigurationManager.AppSettings["LDAPConnection"];
        public static bool AuthenticateUser(LoginModel model)
        {
            DirectoryEntry directoryEntry = new DirectoryEntry(AuthenticationHelper.LDAP_CONNECTION_STRING);
            directoryEntry.Username = model.UserName;
            directoryEntry.Password = model.Password;

            try
            {
                var directorySearcher = new DirectorySearcher(directoryEntry);
                SearchResult foundRecord = directorySearcher.FindOne();
            }
            catch(DirectoryServicesCOMException authException)
            {
                return false;    
            }            
            return true;
        }
    }
}