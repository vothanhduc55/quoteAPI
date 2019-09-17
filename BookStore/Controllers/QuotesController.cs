using AutoMapper;
using BookStore.Data;
using BookStore.DTO;
using BookStore.Hateoas;
using BookStore.Interfaces;
using BookStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class QuotesController : ControllerBase
    {
        private QuotesDbContext _quoteDbContext;
        private IMapper _mapper;
        private IUrlHelper _urlHelper;
        private IQuoteRepository _quoteRepository;
        private IUnitofWork _UnitOfWork;

        public QuotesController(QuotesDbContext quotesDbContext, IMapper mapper, IUrlHelper urlHelper, IQuoteRepository quoteRepository)
        {
            _quoteDbContext = quotesDbContext;
            _mapper = mapper;
            _urlHelper = urlHelper;
            _quoteRepository = quoteRepository;
        }

        /// <summary>
        /// Get all quotes
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetQuotes")]
        public ActionResult<Quote> GetQuotes(string sort)
        {
            //var quoteDTO = _mapper.Map<IEnumerable<QuoteDTO>, IEnumerable<Quote>>(quotes);
            var quotes = _quoteRepository.GetQuotes();

            switch (sort)
            {
                case "desc":
                    quotes = _quoteDbContext.Quotes.OrderByDescending(q => q.Type);
                    break;

                case "asc":
                    quotes = _quoteDbContext.Quotes.OrderBy(q => q.Type);
                    break;

                default:
                    quotes = _quoteDbContext.Quotes;
                    break;
            }
            var quotesDto = _mapper.Map<IEnumerable<QuoteDTO>>(quotes);
            var wrapper = new LinkCollectionResourceBase<QuoteDTO>(quotesDto);
            return Ok(CreateLinkforQuotes(wrapper));
            //return Ok(quotesDto);
        }

        /// <summary>
        /// Get quote by id
        /// </summary>
        /// <param name="id">The unique id quote</param>
        /// <returns></returns>
        // GET: api/Quotes/5
        [HttpGet("{id}", Name = "GetQuoteById")]
        public IActionResult Get(int id)
        {
            var quote = _quoteRepository.GetQuoteById(id);
            var quoteDto = _mapper.Map<QuoteDTO>(quote);
            if (quoteDto == null)
            {
                return NotFound("Not found id...");
            }
            else
            {
                return Ok(CreateLinkforQuote(quoteDto));
            }
        }

        /// <summary>
        /// pagination quotes
        /// </summary>
        /// <param name="pageNumber">The number page</param>
        /// <param name="pageSize">The record in a page</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IActionResult paging(int? pageNumber, int? pageSize)
        {
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5;
            var quotes = _quoteDbContext.Quotes.OrderBy(q => q.Id);
            return Ok(quotes.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        /// <summary>
        /// search quote by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns>the quote is searching</returns>
       // GET: api/Quotes/search?type=th
        [HttpGet]
        [Route("[action]")]
        public IActionResult search(string type)
        {
            var quotes = _quoteDbContext.Quotes.Where(q => q.Type.Contains(type));
            if (quotes == null)
            {
                return NotFound();
            }
            if (type == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(quotes);
            }
        }

        /// <summary>
        /// Create a quote
        /// </summary>
        /// <param name="quote"></param>
        /// <remarks>
        /// Id is a database-generated identity so it should never be include in INSERT
        ///
        /// Example :
        /// {
        ///     "title": "some title",
        ///     "decsription": "some decsciption",
        ///      "author": "some author",
        ///     "type": "some type"
        /// }
        /// </remarks>
        /// <returns></returns>
        // POST: api/Quotes
        [HttpPost(Name = "PostQuote")]
        public IActionResult Post([FromBody] Quote quote)
        {
            //string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            //quote.UserId = userId;

            _quoteRepository.addQuote(quote);

            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        /// update quote
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quote"></param>
        /// <returns></returns>
        // PUT: api/Quotes/5
        [HttpPut("{id}", Name = "Update_Quote")]
        public IActionResult Put(int id, [FromBody] Quote quote)
        {
            //string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var entity = _quoteDbContext.Quotes.Find(id);

            if (entity == null)
            {
                return NotFound();
            }
            //if (userId != entity.UserId)
            //{
            //    return BadRequest("Can't update this record...");
            //}
            else
            {
                entity.Title = quote.Title;
                entity.Decsription = quote.Decsription;
                entity.Author = quote.Author;
                entity.Type = quote.Type;
                entity.CreateAt = quote.CreateAt;
                _quoteDbContext.SaveChanges();
                return Ok("Update record successfully...");
            }
            //return Ok();
        }

        /// <summary>
        /// Detele a quote
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Quotes/5
        [HttpDelete("{id}", Name = "Delete_Quote")]
        public IActionResult Delete(int id)
        {
            var quote = _quoteRepository.GetQuoteById(id);
            if (quote == null)
            {
                return NotFound("No record found against this id");
            }
            else
            {
                _quoteRepository.deleteQuote(quote);
                _quoteDbContext.SaveChanges();
                return Ok("Delete record successfully");
            }
        }

        private QuoteDTO CreateLinkforQuote(QuoteDTO quoteDto)
        {
            //var t = new {
            //    id = quoteDto.Id
            //};
            //var a = _urlHelper.Link("GetQuoteById", t);

            //if(quoteDto.Links.Count <0)
            quoteDto.Links.Add(new LinkResource(
                href: _urlHelper.Link("GetquoteById", new { id = quoteDto.Id }),
                rel: "Self",
                method: "GET"));
            quoteDto.Links.Add(new LinkResource(
                href: _urlHelper.Link("Update_Quote", new { id = quoteDto.Id }),
                rel: "Change a quote",
                method: "PUT"));
            quoteDto.Links.Add(new LinkResource(
                href: _urlHelper.Link("Delete_Quote", new { id = quoteDto.Id }),
                rel: "Delete a quote",
                method: "DELETE"));
            return quoteDto;
        }

        private LinkCollectionResourceBase<QuoteDTO> CreateLinkforQuotes(
           LinkCollectionResourceBase<QuoteDTO> quoteWrapper)
        {
            quoteWrapper.Links.Add(
                new LinkResource(
                    href: _urlHelper.Link("GetQuoteById", null),
                    rel: "self",
                    method: "GET"));

            return quoteWrapper;
        }
    }
}