using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using SE160271.BookManagementApplicationRepo.Models;
using SE160271.BookManagementApplicationRepo.UnitOfWork;

namespace SE160271.BookManagementApplicationAPI.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IUnitOfWork unitOfWork, ILogger<BooksController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: odata/Books
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetBooks()
        {
            try
            {
                var books = await _unitOfWork.Books.GetAllAsync();
                return Ok(books.AsQueryable());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetBooks)}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: odata/Books(5)
        [HttpGet("{key}")]
        [EnableQuery]
        public async Task<IActionResult> GetBookById([FromODataUri] int key)
        {
            try
            {
                var book = await _unitOfWork.Books.GetByIdAsync(key);
                if (book == null)
                {
                    return NotFound();
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetBookById)}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: odata/Books
        [HttpPost]
        public async Task<IActionResult> PostBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _unitOfWork.Books.AddAsync(book);
                await _unitOfWork.CompleteAsync();

                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(PostBook)}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: odata/Books(5)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook([FromODataUri] int id, [FromBody] Book update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingBook = await _unitOfWork.Books.GetByIdAsync(id);
                if (existingBook == null)
                {
                    return NotFound();
                }

                existingBook.Isbn = update.Isbn;
                existingBook.Title = update.Title;
                existingBook.Author = update.Author;
                existingBook.Price = update.Price;
                existingBook.PressId = update.PressId;

                await _unitOfWork.Books.UpdateAsync(existingBook);
                await _unitOfWork.CompleteAsync();

                return Ok(existingBook);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(PutBook)}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PATCH: odata/Books(5)
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchBook([FromODataUri] int id, [FromBody] Delta<Book> delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingBook = await _unitOfWork.Books.GetByIdAsync(id);
                if (existingBook == null)
                {
                    return NotFound();
                }

                delta.Patch(existingBook);

                await _unitOfWork.Books.UpdateAsync(existingBook);
                await _unitOfWork.CompleteAsync();

                return Ok(existingBook);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(PatchBook)}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: odata/Books(5)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook([FromODataUri] int id)
        {
            try
            {
                var existingBook = await _unitOfWork.Books.GetByIdAsync(id);
                if (existingBook == null)
                {
                    return NotFound();
                }

                await _unitOfWork.Books.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(DeleteBook)}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
