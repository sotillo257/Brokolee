using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Concrete;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Common
{
    public static class CommonFunctions
    {
        public static string GetProfilePicture(string profilePicture)
        {
            string profilePicturePath = "";
            if (string.IsNullOrEmpty(profilePicture))
            {
                
                 profilePicturePath = "~/assets/images/user.svg";
             
            }
            else
            {
                profilePicturePath = profilePicture;
            }
            return profilePicturePath;
        }
        public static UserModel GetUserModel(long id, user objentity = null, string friendRequestStatus = "", bool isRequestReceived = false)
        {
            var user = new user();
            if (objentity != null)
            {
                user = objentity;
            }
            else
            {
                EFUserRepository _UserRepo = new EFUserRepository();
                user = _UserRepo.GetUserById(id);
            }
            UserModel objmodel = new UserModel();
            if (user != null)
            {
                objmodel.IsRequestReceived = isRequestReceived;
                objmodel.FriendRequestStatus = friendRequestStatus;
                objmodel.UserID = user.id;
                objmodel.Name = user.first_name1 + " " + user.last_name1;
                objmodel.ProfilePicture = CommonFunctions.GetProfilePicture(user.user_img);
                objmodel.Gender = "Male";
                objmodel.DOB = user.acq_date.ToShortDateString();
                if (user.acq_date != null)
                {
                    objmodel.Age = Convert.ToString(Math.Floor(DateTime.Now.Subtract(Convert.ToDateTime(user.acq_date)).TotalDays / 365.0)) + " Years";
                }
                else
                {
                    objmodel.Age = "NaN";
                }
                objmodel.Bio = "N";
            }
            return objmodel;
        }
        public static MessageModel GetMessageModel(chatmessage objentity)
        {
            MessageModel objmodel = new MessageModel();
            objmodel.ChatMessageID = objentity.id;
            objmodel.FromUserID = objentity.from_user_id;
            objmodel.ToUserID = objentity.to_user_id;
            objmodel.Message = objentity.message;
            objmodel.Status = objentity.status;
            objmodel.CreatedOn = Convert.ToString(objentity.created_at);
            objmodel.UpdatedOn = Convert.ToString(objentity.updated_at);
            objmodel.ViewedOn = Convert.ToString(objentity.viewed_at);
            objmodel.IsActive = objentity.is_active;
            return objmodel;
        }
    }
}