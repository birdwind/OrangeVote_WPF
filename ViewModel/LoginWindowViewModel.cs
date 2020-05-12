using System;
using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OrangeVote.Model;
using OrangeVote.Utils;

namespace OrangeVote.ViewModel
{
    public class LoginWindowViewModel : ViewModelBase
    {
        private readonly HttpHelper _helper;
        public LoginWindowViewModel()
        {
            _helper = HttpHelper.GetInstance();
        }

        private readonly LoginModel _loginModel = new LoginModel();

        public string Username
        {
            get => _loginModel.UserName;
            set
            {
                _loginModel.UserName = value;
                this.RaisePropertyChanged("UserName");
            }
        }

        public string Password
        {
            get => _loginModel.Password;
            set
            {
                _loginModel.Password = value;
                this.RaisePropertyChanged("Passwrod");
            }
        }

        public ICommand ClickLogin => new RelayCommand(() =>
        {
            const string url = "http://220.135.26.9:5278/login/";
            var paramList = new List<KeyValuePair<string, string>>();
            paramList.Add(new KeyValuePair<string, string>("username", Username));
            paramList.Add(new KeyValuePair<string, string>("password", Password));
            Console.Write("test");
            _helper.Post(url, paramList);
        });
    }
}