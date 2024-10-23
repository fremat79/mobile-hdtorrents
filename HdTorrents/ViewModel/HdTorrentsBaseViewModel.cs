using CommunityToolkit.Mvvm.ComponentModel;
using HdTorrents.Biz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HdTorrents.ViewModel
{
    public class HdTorrentsBaseView : ObservableObject
    {
        protected HdTorrentsSite Site { get; private set; }
        public HdTorrentsBaseView(HdTorrentsSite site)
        {
            Site = site;
        }
    }
}
