using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.Models;

namespace QuizApp.Controllers;

public class FillInTheBlankController : Controller
{
    private readonly IFillInTheBlankRepository _fillInTheBlankRepository;
    private readonly ILogger<FillInTheBlankController> _logger;

    public FillInTheBlankController(IFillInTheBlankRepository fillInTheBlankRepository, ILogger<FillInTheBlankController> logger)
    {
        _fillInTheBlankRepository = fillInTheBlankRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Question(int id)
    {
        var fillInTheBlank = await _fillInTheBlankRepository.GetQuestionById(id);
        if (fillInTheBlank == null)
        {
            _logger.LogError("[FillInTheBlankController] FillInTheBlank question not found for the FillInTheBlankId {FillInTheBlankId: 0000}", id);
            return NotFound("FillInTheBlank question not found.");
        }
        return View(fillInTheBlank);
    }
}