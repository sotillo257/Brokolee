using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Concrete
{
    public class EFMessageRepository
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();

        public chatmessage SaveChatMessage(chatmessage objentity)
        {
            entities.chatmessages.Add(objentity);
            entities.SaveChanges();
            return objentity;
        }
        public messageRecordViewModel GetChatMessagesByUserID(long currentUserID, long toUserID, long lastMessageID = 0)
        {
            messageRecordViewModel obj = new messageRecordViewModel();
            var messages = entities.chatmessages.Where(m => m.is_active == true && (m.to_user_id == toUserID || m.from_user_id == toUserID)
            && (m.to_user_id == currentUserID || m.from_user_id == currentUserID)).OrderByDescending(m => m.created_at);
            if (lastMessageID > 0)
            {
                obj.messages = messages.Where(m => m.id < lastMessageID).Take(20).ToList().OrderBy(m => m.created_at).ToList();
            }
            else
            {
                obj.messages = messages.Take(20).ToList().OrderBy(m => m.created_at).ToList();
            }
            obj.lastChatMessageId = obj.messages.OrderBy(m => m.id).Select(m => m.id).FirstOrDefault();
            return obj;
        }
        public void UpdateMessageStatusByUserID(long fromUserID, long currentUserID)
        {
            var unreadMessages = entities.chatmessages.Where(m => m.status == "Sent" && m.to_user_id == currentUserID && m.from_user_id == fromUserID && m.is_active == true).ToList();
            unreadMessages.ForEach(m =>
            {
                m.status = "Viewed";
                m.viewed_at = System.DateTime.Now;
            });
            entities.SaveChanges();
        }
        public void UpdateMessageStatusByMessageID(long messageID)
        {
            var unreadMessages = entities.chatmessages.Where(m => m.id == messageID).FirstOrDefault();
            if (unreadMessages != null)
            {
                unreadMessages.status = "Viewed";
                unreadMessages.viewed_at = System.DateTime.Now;
                entities.SaveChanges();
            }
        }
    }
}