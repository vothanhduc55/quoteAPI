using AutoMapper;
using BookStore.DTO;
using BookStore.Models;

namespace BookStore.Mappings
{
    public class SimpleMapping : Profile
    {
        public SimpleMapping()
        {
            CreateMap<Quote, QuoteDTO>().ReverseMap();
            //CreateMap<QuoteDTO, Quote>();
        }
    }
}