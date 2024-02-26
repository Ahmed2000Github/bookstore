using BookStoreAPI.Dto;
using BookStoreAPI.Dto.AuthorDto;
using BookStoreAPI.Dto.BookDto;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork<Author> author;
        private readonly IUnitOfWork<Book> book;
        private readonly IUnitOfWork<Rating> rating;
        private readonly IUnitOfWork<Sell> sell;

        public BookController(IWebHostEnvironment webHostEnvironment, IUnitOfWork<Author> author, IUnitOfWork<Book> book, IUnitOfWork<Rating> rating, IUnitOfWork<Sell> sell)
        {
            _webHostEnvironment = webHostEnvironment;
            this.author = author;
            this.book = book;
            this.rating = rating;
            this.sell = sell;
        }

        [HttpGet, Authorize(AuthenticationSchemes = "Bearer")]
        public ActionResult GetBooks()
        {
            var booksList = book.entity.GetFull(b => b.Author);
            var booksDto = booksList.Select(book => new BooksResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Quantity = book.stockQuantity,
                Price = book.Price,
                EditionDate = book.EditionDate,
                ImageUrl = book.ImageUrl,
                AuthorName = book.Author.Name ?? ""
            });
            return Ok(booksDto);
        }

        [HttpPost, Authorize(AuthenticationSchemes = "Bearer")]
        public ActionResult SearchBook([FromBody] BookSearchParamsDto parameters)
        {
            var booksList = book.entity.GetFull(b => b.Author);
            var filter = (Book _book) =>
            {
                DateTime date;
                try
                {
                    date = DateTime.Parse(parameters.EditionDate);
                }
                catch (Exception)
                {
                    date = DateTime.Parse("2000-01-01T00:00:00");
                }
                parameters.EditionDate = parameters.EditionDate.Trim().IsNullOrEmpty() ? "2000-01-01T00:00:00" : parameters.EditionDate;
                return _book.Title == parameters.Title ||
                _book.Price == parameters.Price ||
                _book.EditionDate == date  ||
                _book.Author.Name == parameters.AuthorName;
            };
            var filterBookList = booksList.Where(filter);
            var booksDto = filterBookList.Select(book => new BooksResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Quantity = book.stockQuantity,
                Price = book.Price,
                EditionDate = book.EditionDate,
                ImageUrl = book.ImageUrl,
                AuthorName = book.Author.Name ?? ""
            });
            return Ok(booksDto);
        }

        [HttpGet("{id}"), Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult GetAuthorBooks(Guid id)
        {
            var books = book.entity.Where(x => x.Author.Id == id);
            if (books == null)
            {
                return NotFound();
            }
            var booksDto = books.Select(book => new BooksResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Quantity = book.stockQuantity,
                Price = book.Price,
                EditionDate = book.EditionDate,
                ImageUrl = book.ImageUrl
            });
            var authorBooksDto = new BooksOfAuthorResponseDto
            {
                Total = books.Count(),
                Books = booksDto.ToList()
            };
            return Ok(authorBooksDto);
        }
        [HttpGet("{id}"), Authorize(AuthenticationSchemes = "Bearer")]
        public ActionResult GetBookDetails(Guid id)
        {
            var _book = book.entity.GetFull(b => b.Author).Where(x => x.Id == id).FirstOrDefault();
            if (_book is null) { 
                return NotFound("Book not found.");
            }
            var bookDto = new BookResponseDto
            {
                Id = _book.Id,
                Title = _book.Title,
                Description = _book.Description,
                Quantity = _book.stockQuantity,
                Price = _book.Price,
                EditionDate = _book.EditionDate,
                ImageUrl = _book.ImageUrl,
                Author = new Author
                {
                    Id = _book.Author.Id,
                    Name = _book.Author.Name,
                    Description = _book.Author.Description
                },
                DocUrl = _book.DocUrl
            };
            return Ok(bookDto);
        }



        [HttpPost, Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public async Task<ActionResult> AddBookAsync([FromForm] BookParamsDto parameters)
        {
            var _author = author.entity.GetById(parameters.AuthorId);
            if (_author is null)
            {
                return NotFound("Author Not Found.");
            }
            string wwwrootPath = _webHostEnvironment.WebRootPath;
            string blanketFileName = Guid.NewGuid().ToString() + Path.GetExtension(parameters.Blanket.FileName);
            string documentFileName = Guid.NewGuid().ToString() + Path.GetExtension(parameters.Document.FileName);
            string blanketFilePath = Path.Combine(wwwrootPath+ "\\Blankets\\", blanketFileName);
            string documentFilePath = Path.Combine(wwwrootPath+ "\\Documents\\", documentFileName);
            using (var stream = new FileStream(blanketFilePath, FileMode.Create))
            {
                await parameters.Blanket.CopyToAsync(stream);
            }
            using (var stream = new FileStream(documentFilePath, FileMode.Create))
            {
                await parameters.Document.CopyToAsync(stream);
            }
            var newBook = new Book
            {
                Id = new Guid(),
                Title = parameters.Title,
                Description = parameters.Description,
                Price = parameters.Price,
                Rate = 0,
                EditionDate = DateTime.Parse(parameters.EditionDate),
                ImageUrl = "\\Blankets\\" + blanketFileName,
                DocUrl = "\\Documents\\" + documentFileName,
                Author = _author
            };
            book.entity.Add(newBook);
            book.save();
            return Ok("Book was added successfully.");
        }

        [HttpPut, Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public async Task<ActionResult> UpdateBook( [FromForm] BookUpdateParamsDto parameters)
        {

            var _book = book.entity.GetById(parameters.BookId);
            if (_book is null)
            {
                return NotFound("The Book not found.");
            }
            string wwwrootPath = _webHostEnvironment.WebRootPath;
            if (parameters.Blanket is not null)
            {
                var filePath = wwwrootPath + _book!.ImageUrl;
                await UpdateFile(filePath, parameters.Blanket);
            }
            if (parameters.Document is not null)
            {
                var filePath = wwwrootPath + _book!.DocUrl;
                await UpdateFile(filePath, parameters.Document);
            }
            _book.Title = parameters.Title ?? _book.Title;
            _book.Description = parameters.Description ?? _book.Description;
            _book.Price = parameters.Price ?? _book.Price;
            if (parameters.EditionDate is not null)
            {
                _book.EditionDate = DateTime.Parse(parameters.EditionDate);
            }
            
            book.entity.Update(_book);
            book.save();
            
            return Ok("The  Book information was updated successfully.");
        }

        [HttpPost, Authorize(AuthenticationSchemes = "Bearer", Roles = "USER")]
        public ActionResult AddRateToBook([FromBody][Required] RatingParamsDto parameters)
        {
            var _book = book.entity.GetById(parameters.BookId);
            if (_book is null)
            {
                return NotFound("The Book not found.");
            }
            var newRate = new Rating()
            {
                UserId = (new Guid()).ToString(),
                BookId = parameters.BookId,
                Rate = parameters.Rate,
                Date = DateTime.Now
            };
            rating.entity.Add(newRate);
            rating.save();
            return Ok("Rate added to Book successfully.");
        }

        [HttpPost, Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult AddSell([FromBody][Required] SellParamsDto parameters)
        {
            var _book = book.entity.GetById(parameters.BookId);
            if (_book is null)
            {
                return NotFound("The Book not found.");
            }
            if (parameters.Quantity <= 0)
            {
                return BadRequest("The provided qunatity is inacceptable.");
            }
            if (_book.stockQuantity<parameters.Quantity)
            {
                return Unauthorized("The requested quantity not available now.");
            }
            var newSell = new Sell()
            {
                UserId = new Guid(User.FindFirstValue(ClaimTypes.UserData)).ToString(),
                BookId = parameters.BookId,
                Quantity = parameters.Quantity,
                Date = DateTime.Now
            };
            sell.entity.Add(newSell);
            sell.save();
            return Ok("Rate added to Book successfully.");
        }

        private async Task UpdateFile(string filePath,IFormFile newFile) 
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await newFile.CopyToAsync(stream);
            }
        }
        private void DeleteFile(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
        [HttpDelete("{id}"), Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public async Task<ActionResult> DeleteBookAsync(Guid id)
        {
            
            var _book = book.entity.GetById(id);
            if (_book is null)
            {
                return NotFound("The Book not found.");
            }
            string wwwrootPath = _webHostEnvironment.WebRootPath;
            DeleteFile(wwwrootPath + _book!.ImageUrl);
            DeleteFile(wwwrootPath + _book!.DocUrl);
            book.entity.Delete(id);
            book.save();
            return Ok("Book was deleted successfully.");
        }
      

    }
}
