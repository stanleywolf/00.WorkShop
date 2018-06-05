using System;
using System.Collections.Generic;
using System.Text;
using Forum.Models;

namespace Forum.Data
{
    class Program
    {
        static void Main(string[] args)
        {
            var forumData = new ForumData();

            forumData.Users.Add(new User(1,"gg","fff",new List<int>(){1,2,3}));
            forumData.SaveChenges();
        }
    }
}
