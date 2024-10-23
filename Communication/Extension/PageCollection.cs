using HdTorrents.Types.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HdTorrents.Biz.Extension
{
    public static class PageCollectionExtension
    {
        public static void FillSkeleton(this ObservableCollection<BasePage> pages)
        {
            pages.Clear();
            pages.Add(new DummyPage());
            pages.Add(new DummyPage());
            pages.Add(new DummyPage());
            pages.Add(new DummyPage());
            pages.Add(new DummyPage());
            pages.Add(new DummyPage());
        }

        public static void FillSkeleton(this ObservableCollection<Torrent> torrents)
        {
            torrents.Clear();
            
            for (int i = 0; i < 11; i++) {
                torrents.Add(new Torrent() { Title = "?????????????????", PosterUrl = "loading.gif", Owner = "?????", Resolution = "???", Type = "???" });
            }
        }
    }
}
