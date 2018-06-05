using System.ComponentModel;
using Forum.App.Services;
using Forum.App.UserInterface;
using Forum.App.UserInterface.ViewModels;
using Forum.App.Views;

namespace Forum.App.Controllers
{
    using Forum.App.Controllers.Contracts;
    using Forum.App.UserInterface.Contracts;


    public class PostDetailsController : IController, IUserRestrictedController
    {
        private enum Command
        {
            Back,AddReply
        }

        public int PostId { get; private set; }
        public bool LoggedInUser { get; set; }

        public MenuState ExecuteCommand(int index)
        {
            switch ((Command)index)
            {
                case Command.AddReply:
                    return MenuState.AddReplyToPost;
                case Command.Back:
                    ForumViewEngine.ResetBuffer();
                    return MenuState.Back;
            }
            throw new InvalidEnumArgumentException();
        }

        public IView GetView(string userName)
        {
            PostViewModel pvm = PostService.GetPostViewModel(this.PostId);
            return new PostDetailsView(pvm,this.LoggedInUser);
        }

        public void UserLogIn()
        {
            this.LoggedInUser = true;
        }

        public void UserLogOut()
        {
            this.LoggedInUser = false;
        }

        public void SetPostId(int postId)
        {
            this.PostId = postId;
        }
    }
}
