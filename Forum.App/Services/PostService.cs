using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Forum.App.UserInterface.ViewModels;
using Forum.Data;
using Forum.Models;

namespace Forum.App.Services
{
    internal static class PostService
    {
        internal static Category GetCategory(int categoryId)
        {
            ForumData forumData = new ForumData();
            var category = forumData.Categories.SingleOrDefault(c => c.Id == categoryId);

            return category;
        }

        public static IList<ReplyViewModel> GetPostReplies(int postId)
        {
            ForumData forumData = new ForumData();
            Post post = forumData.Posts.Find(p => p.Id == postId);

            IList<ReplyViewModel> replies = new List<ReplyViewModel>();

            foreach (var replyId in post.ReplyIds)
            {
                var reply = forumData.Replies.Single(r => r.Id == replyId);
                replies.Add(new ReplyViewModel(reply));
            }
            return replies;
        }

        public static string[] GetAllCategoryNames()
        {
            ForumData forumData = new ForumData();
            var allCategories = forumData.Categories.Select(c => c.Name).ToArray();

            return allCategories;
        }

        public static IEnumerable<Post> GetPostsByCategory(int categoryId)
        {
            var forumData = new ForumData();
            var postIds = forumData.Categories.First(c => c.Id == categoryId).PostsIds;

            IEnumerable<Post> posts = forumData.Posts.Where(p => postIds.Contains(p.Id));
            return posts;
        }

        public static PostViewModel GetPostViewModel(int postId)
        {
            var forumData = new ForumData();
            var post = forumData.Posts.Find(p => p.Id == postId);
            PostViewModel pvm = new PostViewModel(post);
            return pvm;
        }

        private static Category EnsureCategory(PostViewModel postView, ForumData forumData)
        {
            var categoryName = postView.Category;
            Category category = forumData.Categories.FirstOrDefault(x => x.Name == categoryName);
            if (category == null)
            {
                var categories = forumData.Categories;
                int categoryid = categories.Any() ? categories.Last().Id + 1 : 1;
                category = new Category(categoryid, categoryName, new List<int>());
                forumData.Categories.Add(category);
            }
            return category;
        }

        public static bool TrysavePost(PostViewModel postView)
        {
            bool emptyCategory = string.IsNullOrWhiteSpace(postView.Category);
            bool emptytitle = string.IsNullOrWhiteSpace(postView.Title);
            bool emptyContent = !postView.Content.Any();

            if (emptyCategory || emptyContent || emptytitle)
            {
                return false;
            }
            ForumData forumData = new ForumData();
            Category category = EnsureCategory(postView, forumData);

            var postId = forumData.Posts.LastOrDefault()?.Id + 1 ?? 1;
            var author = UserServices.GetUser (postView.Author, forumData);
                var content = string.Join("",postView.Content);
            var post = new Post(postId, postView.Title,content,category.Id,author.Id, new List<int>());

            forumData.Posts.Add(post);
            category.PostsIds.Add(postId);
            author.PostIds.Add(postId);
            forumData.SaveChenges();

            postView.PostId = postId;

            return true;
        }
        public static bool TrySaveReply(ReplyViewModel replyView ,int postId)
        {
            if (!replyView.Content.Any())
            {
            ForumData forumData = new ForumData();

                var author = UserServices.GetUser(replyView.Author, forumData);
                var post = forumData.Posts.Single(p => p.Id == postId);
                var replyId = forumData.Replies.LastOrDefault()?.Id + 1 ?? 1;
                var content = string.Join("", replyView.Content);
                var reply = new Reply(replyId, content, author.Id, postId);

                forumData.Replies.Add(reply);
                post.ReplyIds.Add(replyId);
                forumData.SaveChenges();
                
                return true;
            }
            return false;
        }
    }
}
