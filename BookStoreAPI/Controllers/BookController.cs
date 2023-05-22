﻿using BookStoreAPI.Dto;
using BookStoreAPI.Dto.AuthorDto;
using BookStoreAPI.Dto.BookDto;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork<Author> author;
        private readonly IUnitOfWork<Book> book;

        public BookController(IWebHostEnvironment webHostEnvironment, IUnitOfWork<Author> author, IUnitOfWork<Book> book)
        {
            _webHostEnvironment = webHostEnvironment;
            this.author = author;
            this.book = book;
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
                Price = book.Price,
                Rate = book.Rate,
                EditionDate = book.EditionDate,
                ImageUrl = book.ImageUrl,
                AuthorName = book.Author.Name
            });
            return Ok(booksDto);
        }


        [HttpGet("{id}"), Authorize(AuthenticationSchemes = "Bearer")]
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
                Price = book.Price,
                Rate = book.Rate,
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
                Price = _book.Price,
                Rate = _book.Rate,
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
        public ActionResult AddRateToBook([FromBody][Required] Guid bookId)
        {
            var _book = book.entity.GetById(bookId);
            if (_book is null)
            {
                return NotFound("The Book not found.");
            }
            _book.Rate += 1;
            book.entity.Update(_book);
            book.save();
            return Ok("Rate added to Book successfully.");
        }


        private async Task UpdateFile(string filePath,IFormFile newFile) 
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await newFile.CopyToAsync(stream);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteBook(Guid id)
        {
            book.entity.Delete(id);
            book.save();
            return Ok("Book was deleted successfully.");
        }
    }
}