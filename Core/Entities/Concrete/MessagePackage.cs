using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Concrete
{
    public class MessagePackage
    {
        public List<Attachment> Attachments { get; set; }
        public List<UserAddress> To { get; set; }
        public UserAddress From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string LogoPath { get; set; }
        public MailTemplateType? MailTemplateType { get; set; }
        public Dictionary<string, string> ParamsToBeChanged { get; set; }

        public MessagePackage(string subject, string body, List<UserAddress> to, UserAddress from, List<Attachment> attachment, Dictionary<string, string> paramsToBeChanged = null)
        {
            this.Subject = subject;
            this.Body = body;
            this.To = to;
            this.From = from;
            this.ParamsToBeChanged = paramsToBeChanged;
            this.Attachments = attachment;
        }
        public MessagePackage()
        {
            this.To = new List<UserAddress>();
            this.ParamsToBeChanged = new Dictionary<string, string>();
            this.Attachments = new List<Attachment>();
        }
    }

    public class UserAddress
    {
        public UserAddress() { }
        public UserAddress(string Address, string DisplayName)
        {
            this.Address = Address;
            this.DisplayName = DisplayName;
        }
        public string Address { get; set; }
        public string DisplayName { get; set; }
    }
    public class Attachment
    {
        public Attachment() { }

        public Attachment(string type, string name, string content, string filePath)
        {
            this.Type = type;
            this.Name = name;
            this.Content = content;
            this.FilePath = filePath;
        }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        public string FilePath { get; set; }
    }
    public enum MailTemplateType : int
    {
        ForgotPassword = 0
    }
}
