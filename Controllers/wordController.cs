using HT.Core;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Core.ApplicationDatabaseContext;
using WebApplication1.Core.BaseModels;
using WebApplication1.Core.RequestModels;
using WebApplication1.Core.ResponseModels;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class wordController : ControllerBase
{

    public readonly dbContext _context;

    public wordController(dbContext db)
    {
        // Constructor for WordController
        _context = db;
    }

    //Authentication
    public bool IsValidRequest(string token)
    {
        try
        {
            if (token == null)
            {
                return false;

            }
            string openKey = token.Decrypt();
            string loginTime = openKey.Split('+')[1];
            DateTime time = Convert.ToDateTime(loginTime);
            if (DateTime.Now.Subtract(time).Minutes > 5)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            return false;
        }

    }
    // Controller for Words operations

    // Get all words
    [HttpPost("get")]
    public IActionResult GetWords(WordRequest request)
    {
        if (IsValidRequest(request.Token) == false)
        {
            return Unauthorized(new WordTransactionResponse
            {
                IsSuccess = false,
                Message = "Token is invalid",
            });
        }
        // Return all words from the database return WordTransactionResponse
        if (_context.words.ToList() != null)
        {
            return Ok(new WordTransactionResponse
            {
                IsSuccess = true,
                Message = "Words retrieved successfully",
                Words = _context.words.ToList(),
                Word_Count = _context.words.ToList().Count
            });
        }
        return NotFound(new WordTransactionResponse
        {
            IsSuccess = false,
            Message = "No words found",
            Words = null,
            Word_Count = 0
        });

    }
    // Get a word by id
    [HttpPost("getbyid")]
    public IActionResult GetWordById(WordRequest request)
    {

        // Return a word by id from the database
        if (IsValidRequest(request.Token) == false)
        {
            return Unauthorized(new WordTransactionResponse
            {
                IsSuccess = false,
                Message = "Token is invalid",
            });
        }
        var word = _context.words.Find(request.id);
        if (word == null)
            return NotFound(new WordTransactionResponse
            {
                IsSuccess = false,
                Message = "Word not found",
                Words = _context.words.ToList(),
                Word_Count = _context.words.ToList().Count
            });

        return Ok(new WordTransactionResponse
        {
            IsSuccess = true,
            Message = "Word retrieved successfully",
            Words = new List<Words> { word },
            Word_Count = 1
        });
    }
    // Add a new word
    [HttpPost("add")]
    public IActionResult AddWord([FromBody] WordRequest request)
    {
        // Add a new word to the database if not existing already in database , return WordTransactionResponse
        if (IsValidRequest(request.Token) == false)
        {
            return Unauthorized(new WordTransactionResponse
            {
                IsSuccess = false,
                Message = "Token is invalid",
            });
        }
        if (_context.words.Any(x => x.word_english == request.Word.word_english))
            return BadRequest(new WordTransactionResponse
            {
                IsSuccess = false,
                Message = "Word already exists",
                Words = _context.words.ToList(),
                Word_Count = _context.words.ToList().Count
            });
        _context.words.Add(request.Word);
        _context.SaveChanges();
        return Ok(new WordTransactionResponse
        {
            IsSuccess = true,
            Message = "Word added successfully",
            Transaction = "Add",
            Words = _context.words.ToList(),
            Word_Count = _context.words.ToList().Count
        });


    }
    // Update an existing word
    [HttpPost("update")]
    public IActionResult UpdateWord([FromBody] WordRequest request)
    {
        // Update an existing word in the database if it exists , return WordTransactionResponse
        if (IsValidRequest(request.Token) == false)
        {
            return Unauthorized(new WordTransactionResponse
            {
                IsSuccess = false,
                Message = "Token is invalid",
            });
        }
        var dbWord = _context.words.Find(request.id);
        if (dbWord == null)
            return NotFound(new WordTransactionResponse
            {
                IsSuccess = false,
                Message = "Word not found",
                Words = _context.words.ToList(),
                Word_Count = _context.words.ToList().Count
            });

        dbWord.word_english = request.Word.word_english;
        dbWord.word_turkish = request.Word.word_turkish;
        _context.SaveChanges();
        return Ok(new WordTransactionResponse
        {
            IsSuccess = true,
            Message = "Word updated successfully",
            Transaction = "Update",
            Words = _context.words.ToList(),
            Word_Count = _context.words.ToList().Count
        });
    }
    // Delete a word by id
    [HttpPost("delete")]
    public IActionResult DeleteWord(WordRequest request)
    {
        // Delete a word from the database if it exists , return WordTransactionResponse
        if (IsValidRequest(request.Token) == false)
        {
            return Unauthorized(new WordTransactionResponse
            {
                IsSuccess = false,
                Message = "Token is invalid",
            });
        }
        var dbWord = _context.words.Find(request.id);
        if (dbWord == null)
            return NotFound(new WordTransactionResponse
            {
                IsSuccess = false,
                Message = "Word not found",
                Words = _context.words.ToList(),
                Word_Count = _context.words.ToList().Count
            });

        _context.words.Remove(dbWord);
        _context.SaveChanges();
        return Ok(new WordTransactionResponse
        {
            IsSuccess = true,
            Message = "Word deleted successfully",
            Transaction = "Delete",
            Words = _context.words.ToList(),
            Word_Count = _context.words.ToList().Count
        });
    }




}