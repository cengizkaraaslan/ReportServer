using Core.Entities.Concrete;
using Core.Utilities.Security.Encyption;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Core.Mail
{
    public class SmtpEmail
    {
    
        readonly IConfiguration configuration;
        public SmtpEmail(IConfiguration _configuration)
        {
            this.configuration = _configuration;
        }
        public bool Send(MessagePackage messagePackage)
        {
            
                EncrpytDecrypt se = new EncrpytDecrypt();

                string fromUserName = configuration.GetSection("ServiceEmail:fromUserName").Value.ToString();
                string fromName = configuration.GetSection("ServiceEmail:fromName").Value.ToString();
                string smtpPassword = EncrpytDecrypt.DecryptString(configuration.GetSection("ServiceEmail:smtpPassword").Value.ToString(), "@1B2c3D4e5F6g7H8");
                string smtpPort = configuration.GetSection("ServiceEmail:smtpPort").Value.ToString();
                string smtpServer = configuration.GetSection("ServiceEmail:smtpServer").Value.ToString();
                string smtpUseSSL = configuration.GetSection("ServiceEmail:smtpUseSSL").Value.ToString();

                MailMessage ms = new MailMessage();
                ms.From = new MailAddress(fromUserName, fromName);
                Dictionary<int, List<UserAddress>> BccList = new Dictionary<int, List<UserAddress>>();
                int howManyMessagesToSend = 50;
                if (messagePackage.To.Count() > howManyMessagesToSend)
                {
                    List<UserAddress> bCCToList = messagePackage.To;
                    int TotalCount50 = messagePackage.To.Count() / howManyMessagesToSend;
                    int LastCount = messagePackage.To.Count() % howManyMessagesToSend;
                    int tempCount = 50;
                    int j = 0;
                    if (LastCount != 0)
                    {
                        TotalCount50 += 1;
                    }
                    for (int i = 1; i <= TotalCount50; i++)
                    {
                        j = ((i - 1) * 50);
                        if (i == TotalCount50)
                            tempCount = LastCount;
                        BccList.Add(i, bCCToList.GetRange(j, tempCount));
                    }
                }
                else
                {
                    messagePackage.To.ForEach(e =>
                    ms.Bcc.Add(e.Address)
                    );
                }
                ms.IsBodyHtml = true;
                ms.Subject = messagePackage.Subject;
                ms.Body = (messagePackage.Body.Replace("{OrganizationLogo}", "http://" + messagePackage.LogoPath).Trim('"'));
               
                foreach (var item in messagePackage.ParamsToBeChanged)
                {
                    ms.Body = ms.Body.Replace(item.Key, item.Value);
                }
                NetworkCredential auth = new NetworkCredential(fromUserName, smtpPassword);
                int smtpPortInt = 0;
                bool CheckInt = int.TryParse(smtpPort, out smtpPortInt);
                SmtpClient smtp = new SmtpClient(smtpServer, smtpPortInt);
                smtp.EnableSsl = Convert.ToBoolean(smtpUseSSL);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = auth;
                if (BccList.Count() > 0)
                {
                    foreach (KeyValuePair<int, List<UserAddress>> item in BccList)
                    {
                        item.Value.ForEach(e =>
                        ms.Bcc.Add(e.Address)
                        );
                        smtp.Send(ms);
                        item.Value.ForEach(e => ms.Bcc.RemoveAt(0));
                    }
                }
                else
                {
                    smtp.Send(ms);
                }
                return true;
           
            
        }
    }
}
