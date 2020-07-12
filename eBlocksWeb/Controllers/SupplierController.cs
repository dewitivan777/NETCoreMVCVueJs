using eBlocksWeb.Handlers;
using eBlocksWeb.Helpers;
using eBlocksWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eBlocksWeb.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ICommandHandler<Supplier> _commandHandler;
        private readonly IQueryHandler<Supplier> _queryHandler;

        public SupplierController(ICommandHandler<Supplier> SupplierCommandHandler, IQueryHandler<Supplier> SupplierQueryHandler)
        {
            _commandHandler = SupplierCommandHandler;
            _queryHandler = SupplierQueryHandler;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Search()
        {
            var result = await _queryHandler.GetAllAsync(Default.GetClassificationEndpoint(nameof(Supplier)));

            return Json(new { result.Content });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetModelStateErrors());
            }

            var result = await _commandHandler.PostAsync(Default.GetClassificationEndpoint(nameof(Supplier)), supplier);

            return Json(new { result });
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetModelStateErrors());
            }

            var result = await _commandHandler.PutAsync(Default.GetClassificationEndpoint(nameof(Supplier)), supplier, supplier.Id);

            return Json(new { result });
        }

        public async Task<IActionResult> Delete(string id)
        {
            var success = false;

            if (ModelState.IsValid)
            {
                var result = await _commandHandler.DeleteAsync(Default.GetClassificationEndpoint(nameof(Supplier)), id);

                if (!result.IsError)
                {
                    success = true;
                }
            }

            return Json(new { success });
        }
    }
}
