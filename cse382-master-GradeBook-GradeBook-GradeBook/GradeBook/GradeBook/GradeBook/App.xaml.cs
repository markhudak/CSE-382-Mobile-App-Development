using GradeBook.Services;
using GradeBook.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GradeBook
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            Page mainPage = new MainPage();

            //Setting navigation page
            MainPage = new NavigationPage(mainPage);
            // Opening DataBase connection - see DB.cs
            DB.OpenConnection();

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
