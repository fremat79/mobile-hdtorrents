using HdTorrents.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace HdTorrents.Views
{
    public partial class PersonDetailsView : ContentPage , IQueryAttributable
    {
        public PersonDetailsView(PersonDetailsViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        string Url { get; set; }

        bool LoadEnd { get; set; }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count > 0)
            {
                var url = (string)query["Url"];
                if (url != Url)
                {
                    Url = url;
                    LoadEnd = false;
                }
            }
        }

        PersonDetailsViewModel ViewModel => BindingContext as PersonDetailsViewModel;

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (!LoadEnd)
            {
                await ViewModel.LoadDetails(Url);
                LoadEnd = true;
            }
        }
    }
}