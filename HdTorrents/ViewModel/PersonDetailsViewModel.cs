using HdTorrents.Biz;
using HdTorrents.Types.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HdTorrents.ViewModel
{
    public partial class PersonDetailsViewModel : HdTorrentsBaseView
    {
        PersonDetails _details;
        public PersonDetailsViewModel(HdTorrentsSite site) : base(site)
        {
        }

        public PersonDetails Details
        {
            get => _details;
            set
            {
                _details = value;                
                OnPropertyChanged(nameof(Details));
            }
        }

        internal async Task LoadDetails(string url)
        {
            Details = await Site.LoadPersonDetails(url);
        }
    }
}
