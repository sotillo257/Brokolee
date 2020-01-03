using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Concrete
{
    public class EFUserRepository
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();

        public void SaveUserOnlineStatus(onlineuser obj)
        {
            var query = entities.onlineusers.Where(m => m.user_id == obj.user_id &&
            m.is_active == true && m.connectionID == obj.connectionID).FirstOrDefault();

            if (obj != null)
            {
                query.is_online = obj.is_online;
                query.updated_at = DateTime.Now;
                query.connectionID = obj.connectionID;
            } else
            {
                obj.created_at = DateTime.Now;
                obj.updated_at = DateTime.Now;
                obj.is_active = true;
                entities.onlineusers.Add(obj);
            }
            entities.SaveChanges();
        }

        public List<string> GetUserConnectionID(long userID)
        {
            var obj = entities.onlineusers.Where(m => m.user_id == userID &&
            m.is_active == true && m.is_online == true)
            .Select(m => m.connectionID).ToList();
            return obj;
        }

        public List<string> GetUserConnectionID(long[] userIDs)
        {
            var obj = entities.onlineusers.Where(m => userIDs.Contains(m.user_id) == true &&
            m.is_active == true && m.is_online == true)
            .Select(m => m.connectionID).ToList();
            return obj;
        }

        public List<user> GetAllUsers()
        {
            var objList = entities.users.ToList();
            return objList;
        }

        public List<onlineUserDetailsViewModel> GetOnlineFriends(long userID)
        {
            long[] friends = GetFriendUserIds(userID);
            var friendOnlineDetails = entities.onlineusers.Where(m => friends.Contains(m.user_id) &&
            m.is_active == true && m.is_online == true).ToList();
            var obj = (from v in entities.users
                       where friends.Contains(v.id)
                       select new onlineUserDetailsViewModel
                       {
                           userID = v.id,
                           name = v.first_name1 + " " + v.last_name1,
                           profilePicture = v.user_img,
                       }).OrderBy(m => m.name).ToList();
            var onlineUserIds = friendOnlineDetails.Select(m => m.user_id).ToArray();
            obj = obj.Where(m => onlineUserIds.Contains(m.userID)).ToList();
            obj.ForEach(m =>
            {
                m.connectionID = friendOnlineDetails.Where(x => x.user_id == m.userID).Select(x => x.connectionID).ToList();
            });
            return obj;
        }

        public long[] GetFriendUserIds(long userID)
        {
            var arr = entities.friendmappings.Where(m => (m.request_userid == userID ||
            m.end_userid == userID) && m.request_status == "Accepted" && m.is_active == true).
            Select(m => m.request_userid == userID ? m.end_userid : m.request_userid).ToArray();
            return arr;
        }

        public user GetUserById(long userID)
        {
            var obj = entities.users.Where(m => m.id == userID).FirstOrDefault();
            return obj;
        }

        public List<friendRequestViewModel> GetSentFriendRequests(long userID)
        {
            var list = (from u in entities.friendmappings
                        join v in entities.users on u.end_userid equals v.id
                        where u.request_userid == userID && u.request_status == "Sent" && u.is_active == true
                        select new friendRequestViewModel()
                        {
                            userInfo = v,
                            requestStatus = u.request_status,
                            endUserID = u.end_userid,
                            requestorUserID = u.request_userid
                        }).ToList();
            return list;
        }
        public List<friendRequestViewModel> GetReceivedFriendRequests(long userID)
        {
            var list = (from u in entities.friendmappings
                        join v in entities.users on u.request_userid equals v.id
                        where u.end_userid == userID && u.request_status == "Sent" && u.is_active == true
                        select new friendRequestViewModel()
                        {
                            userInfo = v,
                            requestStatus = u.request_status,
                            endUserID = u.end_userid,
                            requestorUserID = u.request_userid
                        }).ToList();
            return list;
        }

        public List<friendRequestViewModel> GetAllSentFriendRequests()
        {
            var list = (from u in entities.friendmappings
                        join v in entities.users on u.end_userid equals v.id
                        where u.request_status == "Sent" && u.is_active == true
                        select new friendRequestViewModel()
                        {
                            userInfo = v,
                            requestStatus = u.request_status,
                            endUserID = u.end_userid,
                            requestorUserID = u.request_userid
                        }).ToList();
            return list;
        }
        public List<userSearchResultViewModel> SearchUsers(string name, long userID)
        {
            long[] friendIds = GetFriendUserIds(userID);
            var objList = entities.users.Where(m => (m.first_name1.ToLower().Contains(name.ToLower()) ||
            m.last_name1.ToLower().Contains(name.ToLower())) 
            && m.id != userID && !friendIds.Contains(m.id)).ToList();
            var receivedRequests = GetReceivedFriendRequests(userID);
            var sentRequests = GetSentFriendRequests(userID);
            List<userSearchResultViewModel> list = new List<userSearchResultViewModel>();
            foreach (var item in objList)
            {
                bool isReceived = false;
                var receivedRequest = receivedRequests.Where(x => x.userInfo.id == item.id).FirstOrDefault();
                if (receivedRequest != null)
                {
                    isReceived = true;
                }
                var userInfo = new userSearchResultViewModel();
                userInfo.isRequestReceived = isReceived;
                userInfo.userInfo = item;
                var sentRequest = sentRequests.Where(m => m.userInfo.id == item.id).FirstOrDefault();
                if (sentRequest != null)
                {
                    userInfo.friendRequestStatus = sentRequest.requestStatus; ;
                }
                list.Add(userInfo);
            }
            return list;
        }
        public void SendFriendRequest(long endUserID, long loggedInUserID)
        {
            friendmapping objentity = new friendmapping();
            objentity.created_at = System.DateTime.Now;
            objentity.end_userid = endUserID;
            objentity.is_active = true;
            objentity.request_userid = loggedInUserID;
            objentity.request_status = "Sent";
            objentity.updated_at = System.DateTime.Now;
            entities.friendmappings.Add(objentity);
            entities.SaveChanges();
        }
        public long SaveUserNotification(string notificationType, long fromUserID, long toUserID)
        {
            notification notification = new notification();
            notification.created_at = System.DateTime.Now;
            notification.is_active = true;
            notification.noti_type = notificationType;
            notification.from_userid = fromUserID;
            notification.status = "New";
            notification.updated_at = System.DateTime.Now;
            notification.to_userid = toUserID;
            entities.notifications.Add(notification);
            entities.SaveChanges();
            return notification.id;
        }
        public friendmapping GetFriendRequestStatus(long userID)
        {
            var obj = entities.friendmappings.Where(m => (m.end_userid == userID 
            || m.request_userid == userID) && m.is_active == true).FirstOrDefault();
            return obj;
        }
        public long ResponseToFriendRequest(long requestorID, string requestResponse, long endUserID)
        {
            var request = entities.friendmappings.Where(m => m.end_userid == endUserID 
            && m.request_userid == requestorID && m.is_active == true).FirstOrDefault();
            if (request != null)
            {
                request.request_status = requestResponse;
                request.updated_at = System.DateTime.Now;
                entities.SaveChanges();
            }
            var notification = entities.notifications.Where(m => m.to_userid == endUserID 
            && m.from_userid == requestorID && m.is_active == true 
            && m.noti_type == "FriendRequest").FirstOrDefault();

            if (notification != null)
            {
                notification.is_active = false;
                notification.updated_at = System.DateTime.Now;
                entities.SaveChanges();
                return notification.id;
            }
            return 0;
        }
        public List<userNotificationListViewModel> GetUserNotifications(int toUserID)
        {
            var listQuery = (from u in entities.notifications
                             join v in entities.users on u.from_userid equals v.id
                             where u.to_userid == toUserID && u.is_active == true
                             select new userNotificationListViewModel()
                             {
                                 notificationID = u.id,
                                 notificationType = u.noti_type,
                                 User = v,
                                 notificationStatus = u.status,
                                 createdOn = u.created_at
                             }).OrderByDescending(m => m.createdOn);
            long totalNotifications = listQuery.Count();
            var list = listQuery.Take(3).ToList();
            list.ForEach(m => m.totalNotifications = totalNotifications);
            return list;
        }
        public int GetUserNotificationCounts(long toUserID)
        {
            int count = entities.notifications.Where(m => m.status == "New" 
            && m.to_userid == toUserID && m.is_active == true).Count();
            return count;
        }
        public void ChangeNotificationStatus(long[] notificationIDs)
        {
            entities.notifications.Where(m => notificationIDs.Contains(m.id)).ToList().ForEach(m => m.status = "Read");
            entities.SaveChanges();
        }
        public friendmapping RemoveFriendMapping(long friendMappingID)
        {
            var obj = entities.friendmappings.Where(m => m.id == friendMappingID).FirstOrDefault();
            if (obj != null)
            {
                obj.is_active = false;
                entities.SaveChanges();
            }
            return obj;
        }
        public void UpdateProfilePicture(int userID, string profilePicturePath)
        {

        }
        public List<user> GetUsersByLinqQuery(Expression<Func<user, bool>> where)
        {
            var objList = entities.users.Where(where).ToList();
            return objList;
        }
        public List<onlineUserDetailsViewModel> GetRecentChats(long currentUserID)
        {
            long[] friends = GetFriendUserIds(currentUserID);
            var recentMessages = entities.chatmessages.Where(m => m.is_active == true && (m.to_user_id == currentUserID 
            || m.from_user_id == currentUserID)).OrderByDescending(m => m.created_at).ToList();
            var userIds = recentMessages.Select(m => (m.to_user_id == currentUserID ? m.from_user_id : m.to_user_id)).Distinct().ToArray();
            var userIdsList = userIds.ToList();
            var messagesByUserId = recentMessages.Where(m => m.to_user_id == currentUserID && m.status == "Sent").ToList();
            var newMessagesCount = (from p in messagesByUserId
                                    group p by p.from_user_id into g
                                    select new { FromUserID = g.Key, Count = g.Count() }).ToList();
            var onlineUserIDs = entities.onlineusers.Where(m => friends.Contains(m.user_id) && userIds.Contains(m.user_id) 
            && m.is_active == true && m.is_online == true).Select(m => m.user_id).ToArray();
            var users = (from m in entities.users
                         join v in userIdsList on m.id equals v
                         select new onlineUserDetailsViewModel
                         {
                             userID = m.id,
                             name = m.first_name1 + " " + m.last_name1,
                             profilePicture = m.user_img,
                             isOnline = onlineUserIDs.Contains(m.id) ? true : false
                         }).ToList();
            users.ForEach(m =>
            {
                m.UnReadMessageCount = newMessagesCount.Where(x => x.FromUserID == m.userID).Select(x => x.Count).FirstOrDefault();
            });
            users = users.OrderBy(d => userIdsList.IndexOf(d.userID)).ToList();
            return users;
        }
        public onlineUserDetailsViewModel GetUserOnlineStatus(long userID)
        {
            onlineUserDetailsViewModel obj = new onlineUserDetailsViewModel();
            obj.userID = userID;
            var objList = entities.onlineusers.Where(m => m.user_id == userID 
            && m.is_active == true).ToList();

            if (objList != null && objList.Count > 0)
            {
                obj.isOnline = false;
                var onlineConnections = objList.Where(m => m.is_online).ToList();
                var offlineConnections = objList.Where(m => !m.is_online).ToList();
                if (onlineConnections != null && onlineConnections.Count > 0)
                {
                    obj.isOnline = true;
                }
                else if (offlineConnections != null && offlineConnections.Count > 0)
                {
                    obj.isOnline = false;
                    obj.LastUpdationTime = offlineConnections.OrderByDescending(m => m.updated_at)
                        .Select(m => m.updated_at).FirstOrDefault();
                }
            }
            return obj;
        }
       
      
        public List<onlineUserDetailsViewModel> GetFriends(int userID)
        {
            var friendIds = GetFriendUserIds(userID);
            var onlineUserIDs = entities.onlineusers.Where(m => friendIds.Contains(m.user_id) 
            && m.is_active == true && m.is_online == true).Select(m => m.user_id).ToArray();
            var users = entities.users.Where(m => friendIds.Contains(m.id)).Select(m => new onlineUserDetailsViewModel
            {
                userID = m.id,
                name = m.first_name1 + " " + m.last_name1,
                profilePicture = m.user_img,
                isOnline = onlineUserIDs.Contains(m.id) ? true : false
            }).ToList();
            return users;
        }
    }
}