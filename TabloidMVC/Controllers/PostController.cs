using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
        }

        public IActionResult Index()
        {
            int userId = GetCurrentUserProfileId();
            var posts = _postRepository.GetAllPublishedPosts();
            PostUserViewModel vm = new PostUserViewModel()
            {
                Posts = posts,
                currentUserId = userId
            };
            return View(vm);
        }
        public IActionResult MyIndex()
        {
            int userId = GetCurrentUserProfileId();
            var posts = _postRepository.GetAllUserPosts(userId);
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            var postTagsOnPosts = _tagRepository.GetPostTagsByPostId(id);
            int userId = GetCurrentUserProfileId();
            PostEditViewModel vm = new PostEditViewModel()
            {
                Post = post,
                PostTags = postTagsOnPosts,
                currentUserId = userId
            };
            if (post == null)
            {
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            return View(vm);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            } 
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        public ActionResult Edit(int id)
        {
            var vm = new PostEditViewModel();
            vm.Post = _postRepository.GetPublishedPostById(id);
            vm.CategoryOptions = _categoryRepository.GetAll();
            if (vm.Post == null)
            {
                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Post post)
        {
            try
            {
                _postRepository.EditPost(post);
                return RedirectToAction("Details", new { id = post.Id });
            }
            catch (Exception ex)
            {
                return View(post);
            }
        }

        public ActionResult Delete(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);
            return View(post);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.DeletePost(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(post);
            }
        }
    }
}
