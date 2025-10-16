using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.Models;

namespace QuizApp.Controllers;

public class MultipleChoiceController : Controller
{
    private readonly IMultipleChoiceRepository _multipleChoiceRepository;
    private readonly ILogger<MultipleChoiceController> _logger;

    public MultipleChoiceController(IMultipleChoiceRepository multipleChoiceRepository, ILogger<MultipleChoiceController> logger)
    {
        _multipleChoiceRepository = multipleChoiceRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var questions = await _multipleChoiceRepository.GetAll();
        if (questions == null)
        {
            _logger.LogError("[MultipleChoiceController] Could not fetch MultipleChoice questions from repository.");
            return NotFound("Questions not found");
        }

        return View(questions);
    }

    [HttpGet]
    public IActionResult CreateMultipleChoiceQuestion()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateMultipleChoiceQuestion(MultipleChoice question)
    {
        if (!ModelState.IsValid)
        {
            return View(question);
        }

        var ok = await _multipleChoiceRepository.Create(question);
        if (!ok)
        {
            _logger.LogError("[MultipleChoiceController] Failed to create question {@Question}", question);
            return View(question);
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> EditMultipleChoiceQuestion(int id)
    {
        var question = await _multipleChoiceRepository.GetById(id);
        if (question == null)
        {
            _logger.LogError("[MultipleChoiceController] Question not found, Id={Id:0000}", id);
            return NotFound("Question not found");
        }

        return View(question);
    }

    [HttpPost]
    public async Task<IActionResult> EditMultipleChoiceQuestion(MultipleChoice question)
    {
        if (!ModelState.IsValid)
        {
            return View(question);
        }

        var ok = await _multipleChoiceRepository.Update(question);
        if (!ok)
        {
            _logger.LogError("[MultipleChoiceController] Failed to update question {@Question}", question);
            return View(question);
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> DeleteMultipleChoiceQuestion(int id)
    {
        var question = await _multipleChoiceRepository.GetById(id);
        if (question == null)
        {
            _logger.LogError("[MultipleChoiceController] Question not found for deletion, Id={Id:0000}", id);
            return NotFound();
        }

        return View(question);
    }

    [HttpPost, ActionName("DeleteMultipleChoiceQuestion")]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        var ok = await _multipleChoiceRepository.Delete(id);
        if (!ok)
        {
            _logger.LogError("[MultipleChoiceController] Failed to delete question Id={Id:0000}", id);
        }

        return RedirectToAction("Index");
    }
}
