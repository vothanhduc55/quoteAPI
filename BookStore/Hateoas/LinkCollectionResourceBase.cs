using System.Collections.Generic;

namespace BookStore.Hateoas
{
    public class LinkCollectionResourceBase<T> : LinkResourceBase where T : LinkResourceBase
    {
        public IEnumerable<T> Value { get; set; }

        public LinkCollectionResourceBase(IEnumerable<T> value)
        {
            Value = value;
        }
    }
}