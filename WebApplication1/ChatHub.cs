using Microsoft.AspNet.SignalR.Transports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication1.Concrete;
using WebApplication1.Models;
using Microsoft.AspNet.SignalR;

namespace WebApplication1.Common
{
    public class ChatHub : Hub
    {
        public static string emailIDLoaded = "";
        //EFUserRepository _UserRepo = new EFUserRepository();
        //EFMessageRepository _MessageRepo = new EFMessageRepository();
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        public void Connect(string userID)//string userName, string email
        {
            string userName = "", email = "";
            var connectionId = Context.ConnectionId;
            long uID = Convert.ToInt64(userID);
            user curUser = entities.users.Find(uID);
            userName = curUser.first_name1 + " " + curUser.last_name1;
            email = curUser.email;
            using (pjrdev_condominiosEntities dc = new pjrdev_condominiosEntities())
            {
                var item = dc.onlineusers.FirstOrDefault(x => x.user_id == uID);
                if (item != null)
                {
                    item.connectionID = connectionId;
                    item.updated_at = DateTime.Now;
                    item.first_name = curUser.first_name1;
                    item.last_name = curUser.last_name1;
                    item.user_img = null;
                    //curUser.user_img;
                    item.is_active = true;
                    item.is_online = true;
                    dc.SaveChanges();

                    // Disconnect
                    //Clients.All.onUserDisconnectedExisting(item.ConnectionId, item.UserName);

                    // send to caller
                    var connectedUsers = dc.onlineusers.Where(m => m.user_id != uID && m.is_online == true).ToList();
                    var CurrentMessage = dc.chatmessages.Where(m => m.from_user_id == uID || m.to_user_id == uID).ToList();
                    Clients.Caller.onConnected(connectionId, userName);
                }

                var users = dc.onlineusers.ToList();
                if (users.Where(x => x.user_id == uID).ToList().Count == 0)
                {
                    var userdetails = new onlineuser
                    {
                        connectionID = connectionId,
                        user_id = uID,
                        is_online = true,
                        is_active = true,
                        created_at = DateTime.Now,
                        updated_at = DateTime.Now
                    };
                    dc.onlineusers.Add(userdetails);
                    dc.SaveChanges();

                    // send to caller
                    var connectedUsers = dc.onlineusers.Where(m => m.user_id != uID && m.is_online == true).ToList();
                    var CurrentMessage = dc.chatmessages.Where(m => m.from_user_id == uID || m.to_user_id == uID).ToList();
                    Clients.Caller.onConnected(connectionId, userName);
                }

                // send to all except caller client
                Clients.AllExcept(connectionId).onNewUserConnected(connectionId, userName, email);
            }
        }



        public override Task OnDisconnected(bool stopCalled)
        {
            using (pjrdev_condominiosEntities dc = new pjrdev_condominiosEntities())
            {
                string userName = "";
                var connectionID = Context.ConnectionId;
                var item = dc.onlineusers.FirstOrDefault(x => x.connectionID == connectionID);
                if (item != null)
                {
                    long uID = item.user_id;
                    user curUser = entities.users.Find(uID);
                    userName = curUser.first_name1 + " " + curUser.last_name1;
                    item.is_online = false;
                    onlineuser onlineuserItem = dc.onlineusers.Where(m => m.user_id == uID).FirstOrDefault();
                    if (onlineuserItem != null)
                    {
                        onlineuserItem.is_online = false;
                        onlineuserItem.connectionID = connectionID;
                        onlineuserItem.updated_at = DateTime.Now;
                    }
                    else
                    {
                        onlineuserItem = new onlineuser();
                        onlineuserItem.user_id = uID;
                        onlineuserItem.connectionID = connectionID;
                        onlineuserItem.is_online = false;
                        onlineuserItem.created_at = DateTime.Now;
                        onlineuserItem.updated_at = DateTime.Now;
                        dc.onlineusers.Add(onlineuserItem);
                    }
                    dc.SaveChanges();
                    Clients.All.onUserDisconnected(connectionID, userName);
                }
            }
            return base.OnDisconnected(stopCalled);
        }



        public void SendPrivateMessage(string touserID, string message, string status, string email)
        {
            try
            {
                string fromUserName = "";
                bool isEmail = false;
                if (email == "Email")
                {
                    isEmail = true;
                }
                string fromConnectionId = Context.ConnectionId;
                using (pjrdev_condominiosEntities dc = new pjrdev_condominiosEntities())
                {
                    long messageId = 0;
                    var fromOnlineUser = dc.onlineusers.FirstOrDefault(x => x.connectionID == fromConnectionId);
                    if (fromOnlineUser != null)
                    {
                        long fromUserId = fromOnlineUser.user_id;
                        user fromUser = entities.users.Find(fromUserId);
                        fromUserName = fromUser.first_name1 + " " + fromUser.last_name1;
                        long toUserId = Convert.ToInt64(touserID);
                        var toOnlineUser = dc.onlineusers.FirstOrDefault(x => x.user_id == toUserId);
                        user toUser = entities.users.Find(toUserId);

                        if (message.Contains("#&&#"))
                        {
                            string[] strList = message.Split(':');
                            long delMessageId = Convert.ToInt64(strList[0]);
                            DeletePrivateMessage(delMessageId);
                            Clients.Client(fromConnectionId).sendPrivateMessage(delMessageId, fromUserName, message, email);
                            if (toOnlineUser != null)
                            {
                                if (toOnlineUser.is_online == true)
                                {
                                    string toConnectionId = toOnlineUser.connectionID;
                                    Clients.Client(toConnectionId).sendPrivateMessage(delMessageId, fromUserName, message, email);
                                }
                            }
                        }
                        else
                        {
                            if (status == "Click")
                            {
                                if (toOnlineUser == null)
                                {
                                    messageId = AddPrivateMessageinCache(fromUserId, toUserId, message, isEmail, false);
                                }
                                else
                                {
                                    messageId = AddPrivateMessageinCache(fromUserId, toUserId, message, isEmail, toOnlineUser.is_online);
                                }
                            }

                            Clients.Client(fromConnectionId).sendPrivateMessage(messageId, fromUserName, message, email);
                            if (toOnlineUser != null)
                            {
                                if (toOnlineUser.is_online == true)
                                {
                                    messageId = 0;
                                    string toConnectionId = toOnlineUser.connectionID;
                                    Clients.Client(toConnectionId).sendPrivateMessage(messageId, fromUserName, message, email);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void DeletePrivateMessage(long delMessageId)
        {
            try
            {
                using (pjrdev_condominiosEntities dc = new pjrdev_condominiosEntities())
                {
                    chatmessage chatmessage = dc.chatmessages.Find(delMessageId);
                    dc.chatmessages.Remove(chatmessage);
                    dc.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private long AddPrivateMessageinCache(long fromUserId, long toUserId, string message, bool isEmail, bool isRead)
        {
            using (pjrdev_condominiosEntities dc = new pjrdev_condominiosEntities())
            {
                // Save details
                var chatmessage = new chatmessage
                {
                    from_user_id = fromUserId,
                    to_user_id = toUserId,
                    message = message,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                    viewed_at = DateTime.Now,
                    is_active = true,
                    status = "Sent",
                    is_email = isEmail,
                    is_read = isRead
                };
                dc.chatmessages.Add(chatmessage);
                dc.SaveChanges();
                return chatmessage.id;
            }
        }
    }

    public class PrivateChatMessage
    {
        public string userName { get; set; }
        public string message { get; set; }
    }
}