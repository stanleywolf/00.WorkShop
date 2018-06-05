using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Forum.Data;
using Forum.Models;
using static Forum.App.Controllers.SignUpController;

namespace Forum.App.Services
{
    public static class UserServices
    {
        public static bool TryLogInUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }
            ForumData forumData = new ForumData();

            bool userExist = forumData.Users.Any(u => u.Username == username && u.Password == password);
            return userExist;
        }

        public static SignUpStatus TrySignUpUser(string username, string password)
        {
            var validUsername = !string.IsNullOrWhiteSpace(username) && username.Length > 3;
            var validPassword = !string.IsNullOrWhiteSpace(password) && password.Length > 3;

            if (!validPassword || !validUsername)
            {
                return SignUpStatus.DetailsError;
            }
            var forumData = new ForumData();

            var userAlreadyExist = forumData.Users.Any(u => u.Username == username);

            if (!userAlreadyExist)
            {
                //Create user
                var id = forumData.Users.LastOrDefault()?.Id + 1 ?? 1;
                var user = new User(id,username,password,new List<int>());
                forumData.Users.Add(user);
                forumData.SaveChenges();

                return SignUpStatus.Success;
            }
            return SignUpStatus.UsernameTakenError;
        }

        public static User GetUser(int userId)
        {
            ForumData forumData =new ForumData();
            User user = forumData.Users.Find(u => u.Id == userId);
            return user;
        }
        public static User GetUser(string userName, ForumData forumData)
        {          
            User user = forumData.Users.Find(u => u.Username == userName);
            return user;
        }
    }
}
