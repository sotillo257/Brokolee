using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Concrete
{
    public class EFPublicRepository
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();

        public async Task<int> SendAlertEmail(string fromEmail, string toEmail,
            string fromname, string subject, string content, string emailtemplate)
        {
            try
            {
                // Send verify code to Email
                var message = new MailMessage();
                string fromemail = fromEmail.Trim();
                message.Body = emailtemplate;

                message.To.Add(new MailAddress(toEmail));
                message.From = new MailAddress(fromemail);
                message.Subject = subject;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "940048b11a3b8d335205460215ebeaff",  // replace with valid value
                        Password = "88b385667a9bcfe3ccead76e789fd465"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "in-v3.mailjet.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public string GetStripeSecretKey()
        {
            return "sk_test_6eZ9D9NkaFA0Qe7U5pnpGHGQ";
        }

        public string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public string[] GetCommunityCoInfo(long userId)
        {
            string[] list = new string[2];
            string communityName = "No community";
            string communityApart = "No apartment";
            user selfUser = entities.users.Find(userId);
            if (selfUser.create_userid != null)
            {
                long coadminUserId = (long)selfUser.create_userid;
                communuser communuserResult = entities.communusers.Where(m => m.user_id == coadminUserId).FirstOrDefault();
                if (communuserResult != null)
                {
                    long communityID = communuserResult.commun_id;
                    community community = entities.communities.Find(communityID);
                    communityName = community.first_name;
                    communityApart = community.apart;
                }
                list[0] = communityName;
                list[1] = communityApart;
                return list;
            }
            else
            {
                return list;
            }
           
        }

        public string[] GetCommunityInfo(long userId)
        {
            string[] list = new string[2];
            string communityName = "No community";
            string communityApart = "No apartment";        
            communuser communuserResult = entities.communusers.Where(m => m.user_id == userId).FirstOrDefault();
            
            if (communuserResult != null)
            {
                long communityID = communuserResult.commun_id;
                community community = entities.communities.Find(communityID);
                communityName = community.first_name;
                communityApart = community.apart;
            }
            list[0] = communityName;
            list[1] = communityApart;            
            return list;
        }

        public List<task> GetNotifiTaskList(long userID)
        {
            List<task> taskList = new List<task>();
            taskuser taskuser = entities.taskusers.Where(m => m.user_id == userID).FirstOrDefault();
            if (taskuser == null)
            {
                taskuser = new taskuser();
                taskuser.user_id = userID;
                taskuser.task_list = null;
                entities.taskusers.Add(taskuser);
                entities.SaveChanges();
            } else
            {
                string taskListStr = taskuser.task_list;

                if (taskListStr != null)
                {
                    string[] taskIDList = taskListStr.Split(',');
                    for (int i = 0; i < taskIDList.Length; i++)
                    {
                        long taskID = Convert.ToInt64(taskIDList[i]);
                        task taskItem = entities.tasks.Find(taskID);
                        taskList.Add(taskItem);
                    }
                }
            }
            
            return taskList;
        }

        public string GetLogoutUrl()
        {
            return "http://70.35.195.31/iniciar/iniciar";
        }

        public string GetSuscribeLogout()
        {
            return "http://70.35.195.31/suscribe/Index?packageId=";
        }

        public int GetUnreadMessageCount(List<ShowMessage> pubMessageList)
        {
            int messageCount = 0;
            foreach (var item in pubMessageList)
            {
                messageCount = messageCount + item.count_message;
            }
            return messageCount;
        }

        public List<ShowMessage> GetChatMessages(long userId)
        {
            List<ShowMessage> messageList = new List<ShowMessage>();
            List<ChatMessage> chatmessageList = entities.chatmessages
                .Where(m => m.to_user_id == userId && m.is_read != true).OrderBy(m=>m.created_at).Take(5)
                .GroupBy(x => x.from_user_id).Select(x => new ChatMessage {
                    userID = x.Key,
                    count_message = x.Count()
                }).ToList();
            foreach (var item in chatmessageList)
            {
                ShowMessage message = new ShowMessage();
                long fromUserId = item.userID;
                user fromUser = entities.users.Find(fromUserId);
                message.username = fromUser.first_name1 + " " + fromUser.last_name1;
                message.count_message = item.count_message;
                message.image_path = fromUser.user_img;
                messageList.Add(message);

            }
            return messageList;
        }
    }

    public class ChatMessage
    {
        public long userID { get; set; }
        public int count_message { get; set; }
    }

    public class ShowMessage
    {
        public string username { get; set; }
        public int count_message { get; set; }
        public string image_path { get; set; }
    }
}