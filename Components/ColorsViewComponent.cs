﻿using Intex.Models;
using Microsoft.AspNetCore.Mvc;

namespace Intex.Components
{
    public class ColorsViewComponent : ViewComponent
    {
        private IIntexRepository _repo;
        //Constructor
        public ColorsViewComponent(IIntexRepository temp)
        {
            _repo = temp;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedColor = RouteData?.Values["color"];

            var primaryColors = _repo.Products
                .Select(x => x.PrimaryColor)
                .Distinct();

            var secondaryColors = _repo.Products
                .Select(x => x.SecondaryColor)
                .Distinct();

            var colors = primaryColors.Union(secondaryColors).Distinct().OrderBy(x => x);


            return View(colors);



        }

    }
}