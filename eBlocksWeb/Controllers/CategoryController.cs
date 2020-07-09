using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eBlocksWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace eBlocksWeb.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public async Task<JsonResult> Search()
        {
            var result = new List<Category>()
            {
                {new Category(){Id="23", Name="Test", Description = "Test", Images = "Test"} }
            };


            return Json(new { result });
        }

        public async Task<IActionResult> Add()
        {
            var result = new List<Category>()
            {
                {new Category(){Id="23", Name="Test", Description = "Test", Images = "Test"} }
            };


            return Json(new { result });
        }

        public async Task<IActionResult> Edit()
        {
            var result = new List<Category>()
            {
                {new Category(){Id="23", Name="Test", Description = "Test", Images = "Test"} }
            };


            return Json(new { result });
        }

        public async Task<IActionResult> Delete()
        {
            var result = new List<Category>()
            {
                {new Category(){Id="23", Name="Test", Description = "Test", Images = "Test"} }
            };


            return Json(new { result });
        }
    }
}