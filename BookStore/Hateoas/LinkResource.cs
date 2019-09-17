namespace BookStore.Hateoas
{
    public class LinkResource
    {
        public string Href { get; set; }
        public string Method { get; set; }
        public string Rel { get; set; }

        public LinkResource(string href, string method, string rel)
        {
            Href = href;
            Method = method;
            Rel = rel;
        }
    }
}