using Microsoft.AspNetCore.Mvc;
using WebApplication1.Core.ApplicationDatabaseContext;
using WebApplication1.Core.BaseModels;
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
    // Controller for Words operations

    // Get all words
    [HttpGet("get")]
    public IActionResult GetWords()
    {
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
    [HttpGet("get/{id}")]
    public IActionResult GetWordById(int id)
    {
        // Return a word by id from the database

        var word = _context.words.Find(id);
        if (word == null)
            return NotFound(new WordTransactionResponse
            {
                IsSuccess = false,
                Message = "Word not found",
                Words = _context.words.ToList(),
                Word_Count = _context.words.ToList().Count
            });

        return Ok(word);
    }
    // Add a new word
    [HttpPost]
    [Route("add")]
    public IActionResult AddWord([FromBody] Words word)
    {
        // Add a new word to the database if not existing already in database , return WordTransactionResponse
        if (_context.words.Any(x => x.word_english == word.word_english))
            return BadRequest(new WordTransactionResponse
            {
                IsSuccess = false,
                Message = "Word already exists",
                Words = _context.words.ToList(),
                Word_Count = _context.words.ToList().Count
            });
        _context.words.Add(word);
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
    [HttpPost("update/{id}")]
    public IActionResult UpdateWord(int id , [FromBody] Words word)
    {
        // Update an existing word in the database if it exists , return WordTransactionResponse
        var dbWord = _context.words.Find(id);
        if (dbWord == null)
            return NotFound(new WordTransactionResponse
            {
                IsSuccess = false,
                Message = "Word not found",
                Words = _context.words.ToList(),
                Word_Count = _context.words.ToList().Count
            });

        dbWord.word_english = word.word_english;
        dbWord.word_turkish = word.word_turkish;
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
    [HttpPost("delete/{id}")]
    public IActionResult DeleteWord(int id)
    {
        // Delete a word from the database if it exists , return WordTransactionResponse
        var dbWord = _context.words.Find(id);
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