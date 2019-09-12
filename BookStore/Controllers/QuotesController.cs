using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStore.Data;
using BookStore.DTO;
using BookStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public QuotesController(QuotesDbContext quotesDbContext, IMapper mapper)
        {
            _quoteDbContext = quotesDbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all quotes
        /// </summary>
        /// <param name="sort">Sort by Title</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get(string sort)
        {
            IEnumerable<Quote> quotes;

            //var quoteDTO = _mapper.Map<QuoteDTO>(quote);
            var quoteDTO = _quoteDbContext.Quotes.ProjectTo<QuoteDTO>(_mapper.ConfigurationProvider).ToList();
            switch (sort)
            {
                case "desc":
                    quotes = _quoteDbContext.Quotes.OrderByDescending(q => q.Title);
                    break;

                case "asc":
                    quotes = _quoteDbContext.Quotes.OrderBy(q => q.Title);
                    break;

                default:
                    quotes = _quoteDbContext.Quotes;
                    break;
            }
            return Ok(quoteDTO);
        }

        /// <summary>
        /// Get quote by id
        /// </summary>
        /// <param name="id">The unique id quote</param>
        /// <returns></returns>
        // GET: api/Quotes/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var quote = _quoteDbContext.Quotes.Find(id);
            if (quote == null)
            {
                return NotFound("Not found id...");
            }
            else
            {
                return Ok(quote);
            }
        }

        /// <summary>
        /// pagination quotes
        /// </summary>
        /// <param name="pageNumber">The number page</param>
        /// <param name="pageSize">The record in a page</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IActionResult PagingQuote(int? pageNumber, int? pageSize)
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
        [HttpGet]
        [Route("[action]")]
        public IActionResult SearchQuote(string type)
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
            if (quotes.ToString() != type)
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
        [HttpPost]
        public IActionResult Post([FromBody] Quote quote)
        {
            //string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            //quote.UserId = userId;
            quote.CreateAt = DateTime.Now;
            _quoteDbContext.Quotes.Add(quote);
            _quoteDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        /// update quote
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quote"></param>
        /// <returns></returns>
        // PUT: api/Quotes/5
        [HttpPut("{id}")]
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
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var quote = _quoteDbContext.Quotes.Find(id);
            if (quote == null)
            {
                return NotFound("No record found against this id");
            }
            else
            {
                _quoteDbContext.Remove(quote);
                _quoteDbContext.SaveChanges();
                return Ok("Delete record successfully");
            }
        }
    }
}