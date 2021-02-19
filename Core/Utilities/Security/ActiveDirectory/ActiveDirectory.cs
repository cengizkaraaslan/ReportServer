using Core.Utilities.ExceptionInfo;
using Core.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;

namespace Core.Utilities.Security.ActiveDirectory
{
    public class ActiveDirectory
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public ActiveDirectory Login(string userName, string password)
        {

#if (DEBUG || TEST)

            return GetUser(userName);
#else
            try
            {
                DirectoryEntry rootDSE = new DirectoryEntry("LDAP://tedas.gov.tr", userName, password);
                if (rootDSE.Properties.Count > 0)
                {
                    var defaultNamingContext = rootDSE.Properties["defaultNamingContext"].Value;
                    DirectorySearcher dssearch = new DirectorySearcher("LDAP://tedas.gov.tr");
                    dssearch.Filter = "(sAMAccountName=" + userName + ")";
                    SearchResult sresult = dssearch.FindOne();
                    DirectoryEntry dsresult = sresult.GetDirectoryEntry();
                    this.Ad = Helper.StringParse.UpperCaseFirstCharacter(dsresult.Properties["givenName"][0].ToString());
                    this.Soyad = dsresult.Properties["sn"][0].ToString().ToLower();
                    this.Email = dsresult.Properties["mail"][0].ToString().ToLower();
                    return new ActiveDirectory { Ad = this.Ad, Soyad = this.Soyad, Email = this.Email, UserName = userName };
                }
            }
            catch (Exception ex)
            {
                throw new ActiveDirectoryAccount(AspectMessages.ActiveDirectoryConnection, ex);
            }

#endif

#if !DEBUG
            return null;
#endif
        }
        public ActiveDirectory GetUser(string filter = null)
        {

            DirectoryEntry rootDSE = new DirectoryEntry("LDAP://tedas.gov.tr", "egitimportalldap", "Gfsj8@!6");

            using (var directorySearch = new DirectorySearcher(rootDSE))
            {
                directorySearch.Filter = "(&(objectCategory=person)(objectClass=user)(sAMAccountName=*" + filter + "*))";
                directorySearch.SearchScope = SearchScope.Subtree;
                var userResult = directorySearch.FindAll();
                foreach (SearchResult searchEntry in userResult)
                {
                    var entry = new DirectoryEntry(searchEntry.GetDirectoryEntry().Path);

                    if (entry.Properties["sAMAccountName"].Value != null)
                    {
                        return new ActiveDirectory
                        {
                            UserName = entry.Properties["sAMAccountName"].Value.ToString(),
                            Ad = Helper.StringParse.UpperCaseFirstCharacter(entry.Properties["givenName"].Value == null ? null : entry.Properties["givenName"].Value.ToString()),
                            Soyad = entry.Properties["sn"].Value == null ? null : entry.Properties["sn"].Value.ToString().ToUpper(),
                            Email = entry.Properties["mail"].Value == null ? null : entry.Properties["mail"].Value.ToString().ToLower()
                        };
                    }
                }

            }
            throw new ActiveDirectoryAccount(AspectMessages.ActiveDirectoryAccount, new Exception(AspectMessages.ActiveDirectoryAccount));

        }

    }
}
