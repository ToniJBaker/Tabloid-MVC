using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TabloidMVC.Repositories;
using TabloidMVC.Models;
using System;

namespace TabloidMVC.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepo;

        public TagController(ITagRepository tagRepository)
        {
            _tagRepo = tagRepository;
        }
        public ActionResult Index()
        {
            List<Tag> tags = _tagRepo.GetAllTags();
            return View(tags);
        }

        public ActionResult Details(int id)
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tag tag)
        {
            try
            {
                _tagRepo.AddTag(tag);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            Tag tag = _tagRepo.GetTagById(id);
            return View(tag);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Tag tag)
        {
            try
            {
                _tagRepo.DeleteTag(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(tag);
            }
        }
    }
}
