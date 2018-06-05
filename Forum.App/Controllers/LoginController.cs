using System;
using Forum.App.UserInterface;
using Forum.App.Views;

namespace Forum.App.Controllers
{
    using Forum.App.Controllers.Contracts;
    using Forum.App.Services;
    using Forum.App.UserInterface.Contracts;

    public class LogInController : IController, IReadUserInfoController
    {
        private enum Command 
        {
            ReadUsername, ReadPassword,LogIn, Back
        }

        public string Username { get; private set; }
        private string  Password { get; set; }
        private bool Error { get; set; }

        public MenuState ExecuteCommand(int index)
        {
            switch ((Command)index)
            {
                case Command.ReadUsername:
                    this.ReadUsername();
                    return MenuState.Login;
                case Command.ReadPassword:
                    this.ReadPassword();
                    return MenuState.Login;
                case Command.LogIn:
                    bool userLoggedIn = UserServices.TryLogInUser(this.Username, this.Password);
                    if (userLoggedIn)
                    {
                        return MenuState.SuccessfulLogIn;
                    }
                    this.Error = true;
                    return MenuState.Error;
                case Command.Back:
                    this.ResetLogin();
                    return MenuState.Back;
                    default:
                    throw new InvalidOperationException();
            }
        }

        public IView GetView(string userName)
        {
           return new LogInView(Error,Username,Password.Length);
        }

        public void ReadPassword()
        {
            this.Password = ForumViewEngine.ReadRow();
            ForumViewEngine.HideCursor();
        }

        public void ReadUsername()
        {
            this.Username = ForumViewEngine.ReadRow();
            ForumViewEngine.HideCursor();
        }

        public LogInController()
        {
            this.ResetLogin();
        }
        private void ResetLogin()
        {
            this.Error = false;
            this.Username=String.Empty;
            this.Password=String.Empty;
        }
    }
}
