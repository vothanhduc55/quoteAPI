using System.Collections.Generic;

namespace BookStore.Hateoas
{
    public abstract class LinkResourceBase
    {
        public List<LinkResource> Links { get; set; } = new List<LinkResource>();
    }
}