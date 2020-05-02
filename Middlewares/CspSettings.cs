namespace KarthikTechBlog.SecurityFeatures.API
{
    public class CspSettings
    {
        public string Default { get; set; }
        public string Image { get; set; }
        public string Style { get; set; }
        public string Font { get; set; }
        public string Script { get; set; }
        public bool UseHttps { get; set; }
        public bool BlockMixedContent { get; set; }
    }
}
