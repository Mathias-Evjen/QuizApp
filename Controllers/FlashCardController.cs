using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using QuizApp.ViewModels;
using QuizApp.DAL;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;

namespace QuizApp.Controllers;

public class FlashCardController : Controller
{
    private readonly IFlashCardRepository _flashCardRepository;
    private readonly ILogger<FlashCardController> _logger;

    public FlashCardController(IFlashCardRepository flashCardRepository, ILogger<FlashCardController> logger)
    {
        _flashCardRepository = flashCardRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> FlashCards()
    {
        var flashCards = await _flashCardRepository.GetAll();
        if (flashCards == null)
        {
            _logger.LogError("[FlashCardController] FlashCards list not found while executing _flashCardRepository.GetAll()");
            return NotFound("FlashCards not found");
        }
        var flashCardsViewModel = new FlashCardsViewModel(flashCards);
        return View(flashCardsViewModel);
    }

    // TODO: Manage POST-requests for FlashCards

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(FlashCard flashCard)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _flashCardRepository.CreateFlashCard(flashCard);
            if (returnOk)
                return RedirectToAction(nameof(FlashCards));
        }
        _logger.LogError("[FlashCardController] FlashCard creation failed {@flashCard}", flashCard);
        return View(flashCard);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int id)
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
                return RedirectToAction(nameof(FlashCards));
        }
        _logger.LogError("[FlashCardController] FlashCard update failed {@flashCard}", flashCard);
        return View(flashCard);
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
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        bool returnOk = await _flashCardRepository.DeleteFlashCard(id);
        if (!returnOk)
        {
            _logger.LogError("[FlashCardController] FlashCard deletion failed for FlashCardId {FlashCardId:0000}", id);
            return BadRequest("FlashCard deletion failed");
        }
        return RedirectToAction(nameof(FlashCards));
    }
}