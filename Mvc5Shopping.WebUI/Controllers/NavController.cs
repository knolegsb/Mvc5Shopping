﻿using Mvc5Shopping.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc5Shopping.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository repository;
        public NavController(IProductRepository repo)
        {
            this.repository = repo;
        }

        //GET: Nav
        public PartialViewResult Menu(string category = null/*, bool horizontalLayout = false*/)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = repository.Products.Select(x => x.Category).Distinct().OrderBy(x => x);
            //string viewName = horizontalLayout ? "MenuHorizontal" : "Menu";

            //return PartialView(viewName, categories);
            return PartialView("FlexMenu", categories);
        }

        //public string Menu()
        //{
        //    return "Hello from NavController";
        //}
    }
}