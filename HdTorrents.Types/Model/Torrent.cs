using AngleSharp.Dom;
using HdTorrents.Types.Attributes;
using HdTorrents.Types.CustomProcessor;
using HdTorrents.Types.Helpers;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HdTorrents.Types.Models
{
    public class PagedTorrentDetailsView
    {
        [CollectionAttributeSelector("[class='torrent-search--list__no-poster-row']")]
        [CollectionAttributeSelector("[class='torrent-search--poster__result']")]
        [CollectionAttributeSelector("[class='torrent-card']")]
        public List<Torrent> Torrents { get; set; }
        [CustomProcessingAttributeSelector("[class='pagination']:first-of-type", typeof(PaginatorProcessor))]
        public Paginator Pages { get; set; }
    }
    public class Torrent
    {
        [TextContentSelector("[class='torrent-search--list__name']")]
        [TextContentSelector("[class='torrent-search--poster__title']")]
        [TextContentSelector("[class='torrent-card__link']")]
        public string Title { get; set; }
        [TextContentSelector("[class='torrent-search--list__resolution']")]
        [StringSplitFormatter(' ')]
        public string Resolution { get; set; }
        [TextContentSelector("[class='torrent-search--list__type']")]
        public string Type { get; set; }
        [TorrentTypeCssConverter("[class='torrent-search--list__category'] > i")]
        public string Category { get; set; }
        [TextContentSelector("[class*='user-tag__link']")]
        public string Owner { get; set; }
        [IntContentSelector("[class*='torrent-icons__thanks']")]
        public Int32 HeaetBeat { get; set; }
        [IntContentSelector("[class*='torrent-icons__comments']")]
        public Int32 Comments { get; set; }
        [BoolContentSelector("[class*='torrent-icons__highspeed']")]
        public bool HighSpeed { get; set; }
        [TextContentSelector("[class='torrent-search--list__age'] > span")]
        public string Age { get; set; }
        [TextContentSelector("[class='torrent-search--list__size'] > span")]
        public string Size { get; set; }
        [TextContentSelector("[class='torrent-search--list__rating'] > span")]
        [TextContentSelector("[class='torrent-card__rating']")]
        public string Rating { get; set; }
        [IntContentSelector("[class='torrent-search--list__seeders'] > a")]
        public Int32 Seeders { get; set; }
        [IntContentSelector("[class='torrent-search--list__leechers'] > a")]
        public Int32 Leechers { get; set; }
        [IntContentSelector("[class='torrent-search--list__completed'] > a")]
        public Int32 Completed { get; set; }
        [AttributeContentSelector("[class='torrent-search--list__name']", "href")]
        [AttributeContentSelector("[class='torrent-search--poster__poster']", "href")]
        [AttributeContentSelector("[class='torrent-card__link']", "href")]
        public string? DetailsUrl { get; set; }
        [AttributeContentSelector("[class='torrent-search--poster__poster'] img", "src")]
        [AttributeContentSelector("[class='torrent-card__image']", "src", "https://via.placeholder.com/90x135")]
        public string? PosterUrl { get; set; }
        [TextContentSelector("[class='torrent-search--poster__release-date']")]
        public string? ReleaseData { get; set; }
        [TextContentSelector("[class='torrent-card__plot']")]
        public string? Description { get; set; }
    }

    public class TorrentDetails
    {
        string _description;
        string _description1;
        [AttributeContentSelector("img[class='meta__backdrop']", "src")]
        [FallBackValue("https://cdn.dday.it/system/uploads/news/main_image/28801/main_photo-cinema.jpg")]
        public string BackgroundUrl { get; set; }
        [AttributeContentSelector("img[class='meta__poster']", "src")]
        public string PosterUrl { get; set; }
        [TextContentSelector("[class='meta__title']")]
        public string Title { get; set; }

        [CollectionAttributeSelector("li[class='form__group form__group--short-horizontal']:has( i[class='fas fa-download']) a")]
        [CollectionAttributeSelector("a[class='torrent-search--list__file form__standard-icon-button']")]
        public List<TorrentDownloadLink> TorrenDownloadUrls { get; set; }


        public string Url { get; set; }

        [TextContentSelector("[class='meta__description']")]
        public string Description { get => !string.IsNullOrEmpty(_description1) ? _description1 : _description; set => _description = value; }
        [TextContentSelector("[class='panel__body bbcode-rendered']")]
        public string Description1 { get => _description1; set => _description1 = value; }
        [CollectionAttributeSelector("[class='meta__chips'] > section:first-child > article")]
        public List<Chip> Cast { get; set; }
        [CollectionAttributeSelector("[class='meta__chips'] > section:nth-child(2) > article")]
        public List<Chip> Crew { get; set; }
        [CollectionAttributeSelector("[class='meta__chips'] > section:nth-child(3) > article")]
        public List<Chip> Extras { get; set; }

        [CollectionAttributeSelector("section[class='recommendations'] > article")]
        public List<Recommendation> Recommendations { get; set; }
    }

    public class PersonDetails
    {
        [AttributeContentSelector("article[class='sidebar2'] img", "src")]
        public string ImageUrl { get; set; }

        public string Url { get; set; }

        [TextContentSelector("aside [class='panelV2'] h2[class='panel__heading']")]
        public string FullName { get; set; }

        [TextContentSelector("aside [class='panelV2'] dd:nth-of-type(1)")]
        public string Born { get; set; }

        [TextContentSelector("aside [class='panelV2'] dd:nth-of-type(2)")]
        public string PlaceOfBirth { get; set; }

        [TextContentSelector("aside div[class='panel__body']")]
        public string Memo { get; set; }

        [CollectionAttributeSelector("div[class='panel__body'] article")]
        public List<Reference> Movies { get; set; }
    }

    public class Chip
    {
        [AttributeContentSelector("img[class='meta-chip__image']", "src")]
        public string ImageUrl { get; set; }
        [TextContentSelector("[class='meta-chip__name']")]
        public string Name { get; set; }
        [TextContentSelector("[class='meta-chip__value']")]
        public string Value { get; set; }

        [AttributeContentSelector("a[class='meta-chip']","href" )]
        public string DetailslUrl { get; set; }
        public string FA
        {
            get => Name switch
            {
                "Valutazione" => FontAwesomeIcons.Star,
                "Runtime" => FontAwesomeIcons.Clock,
                "Trailer" => FontAwesomeIcons.ArrowUpRightFromSquare,
                "Company" => FontAwesomeIcons.CameraMovie,
                "Keywords" => FontAwesomeIcons.Tag,
                "Genres" => FontAwesomeIcons.MasksTheater,
                _ => FontAwesomeIcons.User
            };
        }
    }

    public class Recommendation
    {
        [AttributeContentSelector("[class='torrent-search--poster__result'] img", "src")]
        public string PosterUrl { get; set; }
        [TextContentSelector("[class='torrent-search--poster__title']")]
        public string Title { get; set; }
        [TextContentSelector("[class='torrent-search--poster__release-date']")]
        public string ReleaseYear { get; set; }
        [AttributeContentSelector("article > figure > a", "href")]
        public string DetailsUrl { get; set; }
    }

    public class TorrentDownloadLink
    {
        [AttributeSelector("href")]
        public string Url { get; set; }
    }

    public class Reference
    {
        [TextContentSelector("header[class='torrent-search--grouped__header'] a")]
        public string Title { get; set; }

        [TextContentSelector("header[class='torrent-search--grouped__header'] address a")]
        public string Directory { get; set; }
        
        [TextContentSelector("header[class='torrent-search--grouped__header'] p")]
        public string Description { get; set; }
    }

    public class Paginator
    {
        public Paginator()
        {
            AvailablePages = new();
        }
        public int Current { get; set; }

        public List<BasePage> AvailablePages { get; private set; }
    }

    public abstract class BasePage
    {
        public string DisplayText { get; set; }
        public Uri? Link { get; set; }

        public int? PageNumber { get; set; } 
        public bool IsCurrent => this.GetType() == typeof(CurrentPage);
    }

    public class CurrentPage : BasePage 
    {         
    }

    public class PreviousPage :BasePage 
    {
        public PreviousPage()
        {
            DisplayText = "<<";
        }
    }

    public class Page : BasePage
    {
    }

    public class DummyPage : BasePage 
    {
        public DummyPage()
        {
            DisplayText = "?";
        }
    }

    public class NextPage : BasePage
    { 
        public NextPage()
        {
            DisplayText = ">>";
        }
    }

    public enum LayoutMode
    {
        Details,
        Poster,
        Card
    }

    public class SearchResults
    {
        [JsonPropertyName("results")]
        public List<SearchResultItem> Results { get; set; } = [];
    }

    public class SearchResultItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("year")]
        [JsonConverter(typeof(YearJsonConverter))]
        public string? Year { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
    
    public class YearJsonConverter : JsonConverter<string?>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.String => reader.GetString(),
                JsonTokenType.Number => reader.GetInt32().ToString(),
                JsonTokenType.Null => null,
                _ => throw new JsonException()
            };
        }

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        {
            if (value == null)
                writer.WriteNullValue();
            else
                writer.WriteStringValue(value);
        }
    }
}
