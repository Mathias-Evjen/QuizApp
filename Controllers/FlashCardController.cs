using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using QuizApp.ViewModels;
using QuizApp.DAL;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;
using Serilog;
using QuizApp.Services;

namespace QuizApp.Controllers;

public class FlashCardController : Controller
{
    private readonly IFlashCardRepository _flashCardRepository;
    private readonly IFlashCardQuizService _flashCardQuizService;
    private readonly ILogger<FlashCardController> _logger;

    public FlashCardController(IFlashCardRepository flashCardRepository, IFlashCardQuizService flashCardQuizService, ILogger<FlashCardController> logger)
    {
        _flashCardRepository = flashCardRepository;
        _flashCardQuizService = flashCardQuizService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> FlashCards(int quizId, string quizName)
    {
        var flashCards = await _flashCardRepository.GetAll(quizId);
        if (flashCards == null)
        {
            _logger.LogError("[FlashCardController] FlashCards list not found while executing _flashCardRepository.GetAll()");
            return NotFound("FlashCards not found");
        }
        var flashCardsViewModel = new FlashCardsViewModel(flashCards, quizName);
        return View(flashCardsViewModel);
    }


    // Switches the ShowAnswer value of the current flash card and returns the model
    [HttpPost]
    public IActionResult RevealFlashCardAnswer(FlashCardsViewModel model) {
        model.FlashCards.ElementAt(model.CurrentFlashCardNum).ShowAnswer = !model.FlashCards.ElementAt(model.CurrentFlashCardNum).ShowAnswer;

        return View("FlashCards", model);
    }

    // TODO: This will not be needed when we move over to React
    [HttpPost]
    public IActionResult NextFlashCard(FlashCardsViewModel model)
    {
        if (model.CurrentFlashCardNum + 1 < model.FlashCards.Count())
            model.CurrentFlashCardNum += 1;
        return View("FlashCards", model);
    }

    [HttpPost]
    public IActionResult PrevFlashCard(FlashCardsViewModel model)
    {
        if (model.CurrentFlashCardNum - 1 >= 0)
            model.CurrentFlashCardNum -= 1;
        return View("FlashCards", model);
    }

    [HttpGet]
    public IActionResult Create(int quizId, int numOfQuestions)
    {
        var flashCard = new FlashCard
        {
            QuizId = quizId,
            QuizQuestionNum = numOfQuestions + 1
        };
        return View(flashCard);
    }

    [HttpPost]
    public async Task<IActionResult> Create(FlashCard flashCard)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _flashCardRepository.CreateFlashCard(flashCard);
            if (returnOk)
                await _flashCardQuizService.IncQuestionCounter(flashCard.QuizId);
                return RedirectToAction("ManageQuiz", "FlashCardQuiz", new { quizId = flashCard.QuizId });
        }
        _logger.LogError("[FlashCardController] FlashCard creation failed {@flashCard}", flashCard);
        return RedirectToAction("ManageQuiz", "FlashCardQuiz", new { quizId = flashCard.QuizId });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var flashCard = await _flashCardRepository.GetFlashCardById(id);
        if (flashCard == null)
        {
            _logger.LogError("[FlashCardController] FlashCard not found when updating FlashCardId {FlashCardId: 0000}", id);
            return BadRequest("FlashCard not found for the FlashCardId");
        }
        return View(flashCard);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(FlashCard flashCard)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _flashCardRepository.UpdateFlashCard(flashCard);
            if (returnOk)
                return RedirectToAction("ManageQuiz", "FlashCardQuiz", new { quizId = flashCard.QuizId });
        }
        _logger.LogError("[FlashCardController] FlashCard update failed {@flashCard}", flashCard);
        return RedirectToAction("ManageQuiz", "FlashCardQuiz", new { quizId = flashCard.QuizId });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var flashCard = await _flashCardRepository.GetFlashCardById(id);
        if (flashCard == null)
        {
            _logger.LogError("[FlashCardController] FlashCard deletion failed for the FlashCardId {FlashCardId:0000}", id);
            return BadRequest("FlashCard not found for the FlashCardId");
        }
        return View(flashCard);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int flashCardId, int qNum, int quizId)
    {
        bool returnOk = await _flashCardRepository.DeleteFlashCard(flashCardId);
        if (!returnOk)
        {
            _logger.LogError("[FlashCardController] FlashCard deletion failed for FlashCardId {FlashCardId:0000}", flashCardId);
            return BadRequest("FlashCard deletion failed");
        }
        await _flashCardQuizService.DecQuestionCounter(quizId);
        await _flashCardQuizService.UpdateFlashCardQuestionNumbers(qNum, quizId);
        return RedirectToAction("ManageQuiz", "FlashCardQuiz", new { quizId = quizId });
    }
}