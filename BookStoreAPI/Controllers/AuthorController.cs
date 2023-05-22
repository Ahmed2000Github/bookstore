using BookStoreAPI.Dto;
using BookStoreAPI.Dto.AuthorDto;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BookStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthorController : ControllerBase
    {
        private readonly IUnitOfWork<Author> author;

        public AuthorController(IUnitOfWork<Author> author)
        {
            this.author = author;
        }

        [HttpGet, Authorize(AuthenticationSchemes = "Bearer")]
        public ActionResult<List<Author>> GetAuthors()
        {
            var authorsList = author.entity.GetAll();
            var authorsDto = authorsList.Select(author => new AuthorResponseDto
            {
                Id = author.Id,
                Name = author.Name,
                Description = author.Description
            });
            return Ok(authorsDto);
        }
        
        [HttpPost, Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<string> AddAuthor(AuthorParamsDto parameters)
        {
            author.entity.Add(new Author
            {
                Id = new Guid(),
                Name = parameters.Name,
                Description = parameters.Description
            });
            author.save();
            return Ok("Author was added successfully.");
        }

        [HttpPut("{id}"), Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<string> UpdateAuthor(Guid id,AuthorParamsDto parameters)
        {
            var authorToUpdate = author.entity.GetById(id);
            if (authorToUpdate == null){
                return BadRequest("The Author with the provided id not Found.");
            }
            authorToUpdate.Name = parameters.Name;
            authorToUpdate.Description = parameters.Description;
            author.entity.Update(authorToUpdate);
            author.save();
            return Ok("Author was updated successfully.");
        }



        [HttpDelete("{id}"), Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<string> DeleteAuthor(Guid id)
        {
            author.entity.Delete(id);
            author.save();
            return Ok("Author was deleted successfully.");
        }
    }
}
