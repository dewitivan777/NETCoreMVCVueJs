using System.Threading.Tasks;
using eBlocksWeb.Handlers;
using eBlocksWeb.Helpers;
using eBlocksWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace eBlocksWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICommandHandler<Order> _commandHandler;
        private readonly IQueryHandler<Order> _queryHandler;

        public OrderController(ICommandHandler<Order> OrderCommandHandler, IQueryHandler<Order> OrderQueryHandler)
        {
            _commandHandler = OrderCommandHandler;
            _queryHandler = OrderQueryHandler;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Search()
        {
            var result = await _queryHandler.GetAllAsync(Default.GetOrderEndpoint(nameof(Order)));

            return Json(new { result.Content });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetModelStateErrors());
            }

            var result = await _commandHandler.PostAsync(Default.GetOrderEndpoint(nameof(Order)), order);

            return Json(new { result });
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetModelStateErrors());
            }

            var result = await _commandHandler.PutAsync(Default.GetOrderEndpoint(nameof(Order)), order, order.Id);

            return Json(new { result });
        }

        public async Task<IActionResult> Delete(string id)
        {
            var success = false;

            if (ModelState.IsValid)
            {
                var result = await _commandHandler.DeleteAsync(Default.GetOrderEndpoint(nameof(Order)), id);

                if (!result.IsError)
                {
                    success = true;
                }
            }

            return Json(new { success });
        }
    }
}