using eBlocksWeb.Handlers;
using eBlocksWeb.Helpers;
using eBlocksWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eBlocksWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICommandHandler<Product> _commandHandler;
        private readonly ICommandHandler<string> _deleteCommandHandler;
        private readonly IQueryHandler<Product> _queryHandler;

        public ProductController(ICommandHandler<Product> ProductCommandHandler, ICommandHandler<string> deleteCommandHandler, IQueryHandler<Product> ProductQueryHandler)
        {
            _commandHandler = ProductCommandHandler;
            _deleteCommandHandler = deleteCommandHandler;
            _queryHandler = ProductQueryHandler;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Search()
        {
            var result = await _queryHandler.GetAllAsync(Default.GetProductEndpoint(nameof(Product)));

            return Json(new { result.Content });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetModelStateErrors());
            }

            var result = await _commandHandler.PostAsync(Default.GetProductEndpoint(nameof(Product)), product);

            return Json(new { result });
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetModelStateErrors());
            }

            var result = await _commandHandler.PutAsync(Default.GetProductEndpoint(nameof(Product)), product, product.Id);

            return Json(new { result });
        }

        [HttpPost]
        [Route("/product/delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = false;

            if (ModelState.IsValid)
            {
                var result = await _deleteCommandHandler.DeleteAsync(Default.GetProductEndpoint(nameof(Product)), id);

                if (!result.IsError)
                {
                    success = true;
                }
            }

            return Json(new { success });
        }
    }
}
