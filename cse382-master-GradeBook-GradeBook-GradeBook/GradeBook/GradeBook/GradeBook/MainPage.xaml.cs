using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.SimpleAudioPlayer;
using Xamarin.Essentials;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using GradeBook.Models;
using SQLite;

[assembly: ExportFont("ChalkboyRegular.otf", Alias = "Chalkboy")]

namespace GradeBook
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {

        // Uri uri = new Uri("https://clipground.com/images/clipart-teacher-apple-2.jpg");
        Image apple = new Image { Source = "apple.png" };
        CoursePage coursePage;
        CharityPage charPage;
        DataPage dp;
        ISimpleAudioPlayer player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();

        public MainPage()
        {
            InitializeComponent();

            Load("sfx.mp3");
            player.Play();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            coursePage = new CoursePage();
            await Navigation.PushAsync(coursePage, true);
        }

        private void Load(string file)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            String ns = "GradeBook";
            ns += ".";
            ns += file;
            Stream audioStream = assembly.GetManifestResourceStream(ns);
            player.Load(audioStream);
        }
       
    } 
}

    
