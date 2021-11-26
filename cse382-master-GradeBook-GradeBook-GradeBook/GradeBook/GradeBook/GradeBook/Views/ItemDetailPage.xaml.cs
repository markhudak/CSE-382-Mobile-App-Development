using GradeBook.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace GradeBook.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}