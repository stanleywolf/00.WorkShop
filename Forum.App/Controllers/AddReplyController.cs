using System.Linq;
using Forum.App.Services;
using Forum.App.UserInterface;
using Forum.App.UserInterface.Input;
using Forum.App.UserInterface.ViewModels;
using Forum.App.Views;

namespace Forum.App.Controllers
{
    using Forum.App.Controllers.Contracts;
    using Forum.App.UserInterface.Contracts;

    public class AddReplyController : IController
    {
        private const int TEXT_AREA_WIDTH = 37;
        private const int TEXT_AREA_HEIGHT = 6;
        private const int POST_MAX_LENGTH = 220;
        private enum Command
        {
             Write, Post
        }
        public ReplyViewModel Reply { get; set; }
        private PostViewModel postViewModel;
        public TextArea TextArea { get; set; }
        public bool Error { get; set; }

        public AddReplyController()
        {
            ResetReply();
        }
        private static int centerTop = Position.ConsoleCenter().Top;
        private static int centerleft = Position.ConsoleCenter().Left;

        public MenuState ExecuteCommand(int index)
        {
            switch ((Command)index)
            {
               
                case Command.Write:
                    this.TextArea.Write();
                    this.Reply.Content = this.TextArea.Lines.ToList();
                    return MenuState.AddPost;
                case Command.Post:
                    var replyAdded = PostService.TrySaveReply(this.Reply, postViewModel.PostId);
                    if (!replyAdded)
                    {
                        Error = true;
                        return MenuState.Rerender;
                    }
                    return MenuState.ReplyAdded;
            }
            throw new InvalidCommandException();
        }

        public IView GetView(string userName)
        {
            this.Reply.Author = userName;
            return new AddReplyView(postViewModel,this.Reply ,this.TextArea, this.Error);
        }
        public void ResetReply()
        {
            this.Error = false;
            this.Reply = new ReplyViewModel();
            int contentLenght = postViewModel?.Content.Count ?? 0;
            this.TextArea = new TextArea(centerleft - 18, centerTop + contentLenght - 6, TEXT_AREA_WIDTH, TEXT_AREA_HEIGHT, POST_MAX_LENGTH);
        }

        public void SetPostId(int postId)
        {
            postViewModel = PostService.GetPostViewModel(postId);
            ResetReply();
        }
    }
}
