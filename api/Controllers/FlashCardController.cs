using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using QuizApp.ViewModels;
using QuizApp.DAL;
using QuizApp.Services;
using QuizApp.DTOs;

namespace QuizApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlashCardAPIController : ControllerBase
{
    private readonly IFlashCardRepository _flashCardRepository;
    private readonly IFlashCardQuizService _flashCardQuizService;
    private readonly ILogger<FlashCardAPIController> _logger;

    public FlashCardAPIController(IFlashCardRepository flashCardRepository, IFlashCardQuizService flashCardQuizService, ILogger<FlashCardAPIController> logger)
    {
        _flashCardRepository = flashCardRepository;
        _flashCardQuizService = flashCardQuizService;
        _logger = logger;
    }

    [HttpGet("flashCards")]
    public async Task<IActionResult> FlashCards(int quizId)
    {
        var flashCards = await _flashCardRepository.GetAll(quizId);
        if (flashCards == null)
        {
            _logger.LogError("[FlashCardAPIController] FlashCards list not found while executing _flashCardRepository.GetAll()");
            return NotFound("FlashCards not found");
        }
        var flashCardDtos = flashCards.Select(card => new FlashCardDto
        {
            FlashCardId = card.FlashCardId,
            Question = card.Question,
            Answer = card.Answer,
            QuizQuestionNum = card.QuizQuestionNum,
            BackgroundColor = _flashCardQuizService.PickRandomFlashCardColor()
        });

        return Ok(flashCardDtos);
    }


    // // Switches the ShowAnswer value of the current flash card and returns the model
    // [HttpPost]
    // public IActionResult RevealFlashCardAnswer(FlashCardsViewModel model)
    // {
    //     model.FlashCards.ElementAt(model.CurrentFlashCardNum).ShowAnswer = !model.FlashCards.ElementAt(model.CurrentFlashCardNum).ShowAnswer;

    //     return View("FlashCards", model);
    // }

    // // TODO: This will not be needed when we move over to React
    // [HttpPost]
    // public IActionResult NextFlashCard(FlashCardsViewModel model)
    // {
    //     if (model.CurrentFlashCardNum + 1 < model.FlashCards.Count())
    //         model.CurrentFlashCardNum += 1;
    //     return View("FlashCards", model);
    // }

    // [HttpPost]
    // public IActionResult PrevFlashCard(FlashCardsViewModel model)
    // {
    //     if (model.CurrentFlashCardNum - 1 >= 0)
    //         model.CurrentFlashCardNum -= 1;
    //     return View("FlashCards", model);
    // }

    // [HttpGet]
    // public IActionResult Create(int quizId, int numOfQuestions)
    // {
    //     var flashCard = new FlashCard
    //     {
    //         QuizId = quizId,
    //         QuizQuestionNum = numOfQuestions + 1
    //     };
    //     return View(flashCard);
    // }

    // [HttpPost]
    // public async Task<IActionResult> Create(FlashCard flashCard)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         bool returnOk = await _flashCardRepository.Create(flashCard);
    //         if (returnOk)
    //             await _flashCardQuizService.ChangeQuestionCount(flashCard.QuizId, true);
    //         return RedirectToAction("ManageQuiz", "FlashCardQuiz", new { quizId = flashCard.QuizId });
    //     }
    //     _logger.LogError("[FlashCardAPIController] FlashCard creation failed {@flashCard}", flashCard);
    //     return RedirectToAction("ManageQuiz", "FlashCardQuiz", new { quizId = flashCard.QuizId });
    // }

    // [HttpGet]
    // public async Task<IActionResult> Edit(int id)
    // {
    //     var flashCard = await _flashCardRepository.GetById(id);
    //     if (flashCard == null)
    //     {
    //         _logger.LogError("[FlashCardAPIController] FlashCard not found when updating FlashCardId {FlashCardId: 0000}", id);
    //         return BadRequest("FlashCard not found for the FlashCardId");
    //     }
    //     return View(flashCard);
    // }

    // [HttpPost]
    // public async Task<IActionResult> Edit(FlashCard flashCard)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         bool returnOk = await _flashCardRepository.Update(flashCard);
    //         if (returnOk)
    //             return RedirectToAction("ManageQuiz", "FlashCardQuiz", new { quizId = flashCard.QuizId });
    //     }
    //     _logger.LogError("[FlashCardAPIController] FlashCard update failed {@flashCard}", flashCard);
    //     return RedirectToAction("ManageQuiz", "FlashCardQuiz", new { quizId = flashCard.QuizId });
    // }

    // [HttpGet]
    // public async Task<IActionResult> Delete(int id)
    // {
    //     var flashCard = await _flashCardRepository.GetById(id);
    //     if (flashCard == null)
    //     {
    //         _logger.LogError("[FlashCardAPIController] FlashCard deletion failed for the FlashCardId {FlashCardId:0000}", id);
    //         return BadRequest("FlashCard not found for the FlashCardId");
    //     }
    //     return View(flashCard);
    // }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int flashCardId, int qNum, int quizId)
    {
        bool returnOk = await _flashCardRepository.Delete(flashCardId);
        if (!returnOk)
        {
            _logger.LogError("[FlashCardAPIController] FlashCard deletion failed for FlashCardId {FlashCardId:0000}", flashCardId);
            return BadRequest("FlashCard deletion failed");
        }
        await _flashCardQuizService.ChangeQuestionCount(quizId, false);
        await _flashCardQuizService.UpdateFlashCardQuestionNumbers(qNum, quizId);
        return RedirectToAction("ManageQuiz", "FlashCardQuiz", new { quizId });
    }
}