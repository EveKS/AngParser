using Newtonsoft.Json;

namespace AngParser.JsonModels.Google
{
  public class Metatag
  {
    [JsonProperty("og:type")]
    public string OgType { get; set; }

    [JsonProperty("og:title")]
    public string OgTitle { get; set; }

    [JsonProperty("og:description")]
    public string OgDescription { get; set; }

    [JsonProperty("og:url")]
    public string OgUrl { get; set; }

    [JsonProperty("og:site_name")]
    public string OgSiteName { get; set; }

    [JsonProperty("og:image")]
    public string OgImage { get; set; }

    [JsonProperty("og:locale")]
    public string OgLocale { get; set; }

    [JsonProperty("twitter:site")]
    public string TwitterSite { get; set; }

    [JsonProperty("fb:app_id")]
    public string FbAppId { get; set; }

    [JsonProperty("application-name")]
    public string ApplicationName { get; set; }

    [JsonProperty("msapplication-window")]
    public string MsapplicationWindow { get; set; }

    [JsonProperty("msapplication-tooltip")]
    public string MsapplicationTooltip { get; set; }

    [JsonProperty("msapplication-task")]
    public string MsapplicationTask { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("twitter:card")]
    public string TwitterCard { get; set; }

    [JsonProperty("twitter:url")]
    public string TwitterUrl { get; set; }

    [JsonProperty("twitter:title")]
    public string TwitterTitle { get; set; }

    [JsonProperty("twitter:description")]
    public string TwitterDescription { get; set; }

    [JsonProperty("apple-mobile-web-app-capable")]
    public string AppleMobileWebAppCapable { get; set; }

    [JsonProperty("viewport")]
    public string Viewport { get; set; }

    [JsonProperty("x-npm:ga:id")]
    public string XNpmGaId { get; set; }

    [JsonProperty("x-npm:ga:dimensions")]
    public string XNpmGaDimensions { get; set; }

    [JsonProperty("msapplication-tilecolor")]
    public string MsapplicationTilecolor { get; set; }

    [JsonProperty("msapplication-tileimage")]
    public string MsapplicationTileimage { get; set; }

    [JsonProperty("msapplication-config")]
    public string MsapplicationConfig { get; set; }

    [JsonProperty("theme-color")]
    public string ThemeColor { get; set; }

    [JsonProperty("pjax-timeout")]
    public string PjaxTimeout { get; set; }

    [JsonProperty("request-id")]
    public string RequestId { get; set; }

    [JsonProperty("google-analytics")]
    public string GoogleAnalytics { get; set; }

    [JsonProperty("octolytics-host")]
    public string OctolyticsHost { get; set; }

    [JsonProperty("octolytics-app-id")]
    public string OctolyticsAppId { get; set; }

    [JsonProperty("octolytics-event-url")]
    public string OctolyticsEventUrl { get; set; }

    [JsonProperty("octolytics-dimension-request_id")]
    public string OctolyticsDimensionRequestId { get; set; }

    [JsonProperty("octolytics-dimension-region_edge")]
    public string OctolyticsDimensionRegionEdge { get; set; }

    [JsonProperty("octolytics-dimension-region_render")]
    public string OctolyticsDimensionRegionRender { get; set; }

    [JsonProperty("analytics-location")]
    public string AnalyticsLocation { get; set; }

    [JsonProperty("dimension1")]
    public string Dimension1 { get; set; }

    [JsonProperty("hostname")]
    public string Hostname { get; set; }

    [JsonProperty("expected-hostname")]
    public string ExpectedHostname { get; set; }

    [JsonProperty("js-proxy-site-detection-payload")]
    public string JsProxySiteDetectionPayload { get; set; }

    [JsonProperty("html-safe-nonce")]
    public string HtmlSafeNonce { get; set; }

    [JsonProperty("go-import")]
    public string GoImport { get; set; }

    [JsonProperty("octolytics-dimension-user_id")]
    public string OctolyticsDimensionUserId { get; set; }

    [JsonProperty("octolytics-dimension-user_login")]
    public string OctolyticsDimensionUserLogin { get; set; }

    [JsonProperty("octolytics-dimension-repository_id")]
    public string OctolyticsDimensionRepositoryId { get; set; }

    [JsonProperty("octolytics-dimension-repository_nwo")]
    public string OctolyticsDimensionRepositoryNwo { get; set; }

    [JsonProperty("octolytics-dimension-repository_public")]
    public string OctolyticsDimensionRepositoryPublic { get; set; }

    [JsonProperty("octolytics-dimension-repository_is_fork")]
    public string OctolyticsDimensionRepositoryIsFork { get; set; }

    [JsonProperty("octolytics-dimension-repository_network_root_id")]
    public string OctolyticsDimensionRepositoryNetworkRootId { get; set; }

    [JsonProperty("octolytics-dimension-repository_network_root_nwo")]
    public string OctolyticsDimensionRepositoryNetworkRootNwo { get; set; }

    [JsonProperty("octolytics-dimension-repository_explore_github_marketplace_ci_cta_shown")]
    public string OctolyticsDimensionRepositoryExploreGithubMarketplaceCiCtaShown { get; set; }

    [JsonProperty("browser-stats-url")]
    public string BrowserStatsUrl { get; set; }

    [JsonProperty("browser-errors-url")]
    public string BrowserErrorsUrl { get; set; }

    [JsonProperty("norton-safeweb-site-verification")]
    public string NortonSafewebSiteVerification { get; set; }

    [JsonProperty("medium")]
    public string Medium { get; set; }

    [JsonProperty("video_height")]
    public string VideoHeight { get; set; }

    [JsonProperty("video_width")]
    public string VideoWidth { get; set; }

    [JsonProperty("video_type")]
    public string VideoType { get; set; }

    [JsonProperty("og:video")]
    public string OgVideo { get; set; }

    [JsonProperty("og:video:secure_url")]
    public string OgVideoSecureUrl { get; set; }

    [JsonProperty("og:video:type")]
    public string OgVideoType { get; set; }

    [JsonProperty("og:video:height")]
    public string OgVideoHeight { get; set; }

    [JsonProperty("og:video:width")]
    public string OgVideoWidth { get; set; }

    [JsonProperty("twitter:player")]
    public string TwitterPlayer { get; set; }

    [JsonProperty("twitter:player:height")]
    public string TwitterPlayerHeight { get; set; }

    [JsonProperty("twitter:player:width")]
    public string TwitterPlayerWidth { get; set; }

    [JsonProperty("swift-page-name")]
    public string SwiftPageName { get; set; }

    [JsonProperty("swift-page-section")]
    public string SwiftPageSection { get; set; }

    [JsonProperty("al:ios:url")]
    public string AlIosUrl { get; set; }

    [JsonProperty("al:ios:app_store_id")]
    public string AlIosAppStoreId { get; set; }

    [JsonProperty("al:ios:app_name")]
    public string AlIosAppName { get; set; }

    [JsonProperty("al:android:url")]
    public string AlAndroidUrl { get; set; }

    [JsonProperty("al:android:package")]
    public string AlAndroidPackage { get; set; }

    [JsonProperty("al:android:app_name")]
    public string AlAndroidAppName { get; set; }

    [JsonProperty("msvalidate.01")]
    public string Msvalidate01 { get; set; }

    [JsonProperty("csrf-param")]
    public string CsrfParam { get; set; }

    [JsonProperty("csrf-token")]
    public string CsrfToken { get; set; }

    [JsonProperty("image")]
    public string Image { get; set; }

    [JsonProperty("twitter:image")]
    public string TwitterImage { get; set; }

    [JsonProperty("fb:admins")]
    public string FbAdmins { get; set; }

    [JsonProperty("og:profile:username")]
    public string OgProfileUsername { get; set; }

    [JsonProperty("author")]
    public string Author { get; set; }

    [JsonProperty("copyright")]
    public string Copyright { get; set; }

    [JsonProperty("y_key")]
    public string YKey { get; set; }

    [JsonProperty("apple-itunes-app")]
    public string AppleItunesApp { get; set; }

    [JsonProperty("apple-mobile-web-app-title")]
    public string AppleMobileWebAppTitle { get; set; }

    [JsonProperty("handheldfriendly")]
    public string Handheldfriendly { get; set; }

    [JsonProperty("al:web:url")]
    public string AlWebUrl { get; set; }

    [JsonProperty("og:video:url")]
    public string OgVideoUrl { get; set; }

    [JsonProperty("og:video:tag")]
    public string OgVideoTag { get; set; }

    [JsonProperty("twitter:app:name:iphone")]
    public string TwitterAppNameIphone { get; set; }

    [JsonProperty("twitter:app:id:iphone")]
    public string TwitterAppIdIphone { get; set; }

    [JsonProperty("twitter:app:name:ipad")]
    public string TwitterAppNameIpad { get; set; }

    [JsonProperty("twitter:app:id:ipad")]
    public string TwitterAppIdIpad { get; set; }

    [JsonProperty("twitter:app:url:iphone")]
    public string TwitterAppUrlIphone { get; set; }

    [JsonProperty("twitter:app:url:ipad")]
    public string TwitterAppUrlIpad { get; set; }

    [JsonProperty("twitter:app:name:googleplay")]
    public string TwitterAppNameGoogleplay { get; set; }

    [JsonProperty("twitter:app:id:googleplay")]
    public string TwitterAppIdGoogleplay { get; set; }

    [JsonProperty("twitter:app:url:googleplay")]
    public string TwitterAppUrlGoogleplay { get; set; }
  }
}
