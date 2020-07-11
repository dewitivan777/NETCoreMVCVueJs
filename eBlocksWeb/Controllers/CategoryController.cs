using System.Threading.Tasks;
using eBlocksWeb.Handlers;
using eBlocksWeb.Helpers;
using eBlocksWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace eBlocksWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICommandHandler<Category> _commandHandler;
        private readonly IQueryHandler<Category> _queryHandler;


        public CategoryController(ICommandHandler<Category> CategoryCommandHandler, IQueryHandler<Category> CategoryQueryHandler)
        {
            _commandHandler = CategoryCommandHandler;
            _queryHandler = CategoryQueryHandler;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Search()
        {
            var result = await _queryHandler.GetAllAsync(Default.GetClassificationEndpoint(nameof(Category)));

            return Json(new { result.Content });
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Category category)
        {
            var success = false;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetModelStateErrors());
            }

            var result = await _commandHandler.PostAsync(Default.GetClassificationEndpoint(nameof(Category)), category);

            return Json(new { result });
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Category category)
        {
            var success = false;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetModelStateErrors());
            }

            var result = await _commandHandler.PutAsync(Default.GetClassificationEndpoint(nameof(Category)), category, category.Id);

            return Json(new { result });
        }

        public async Task<IActionResult> Delete(string id)
        {
            var success = false;

            if (ModelState.IsValid)
            {
                var result = await _commandHandler.DeleteAsync(Default.GetClassificationEndpoint(nameof(Category)), id);

                if (!result.IsError)
                {
                    success = true;
                }
            }

            return Json(new { success });
        }
    }
}