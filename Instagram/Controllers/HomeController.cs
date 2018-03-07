using System;
using System.Web.Mvc;
using Instagram.Classes;
using Instagram.Models;
using System.Web;
using System.Linq;
using System.Collections.Generic;

namespace Instagram.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            List<Images> img = new List<Images>();
            try
            {
                using (var db = new InstagramDBContext())
                {
                    img = db.Images.Take(20).ToList();
                }
            }
            catch (Exception ex)
            {

            }

            return View(img);
        }

        [Authorize]
        [HttpGet]
        public ActionResult AddPhoto()
        {
            return View();
        }

        #region Добавление фото
        [Authorize]
        [HttpPost] //POST : AddPhoto
        public ActionResult AddPhoto([Bind(Exclude = "image")]Images objNewImage)
        {
            try
            {
                HttpPostedFileBase img = Request.Files["image"]; //дергаем картинку с объекта
                using (var context = new InstagramDBContext())
                {
                    objNewImage.image = Helper.ConvertToBytes(img); // сконвертированная картинка
                    objNewImage.created_at = DateTime.Now;
                    objNewImage.updated_at = DateTime.Now;
                    objNewImage.likes = 0;
                    objNewImage.user_id = Convert.ToInt32(HttpContext.User.Identity.Name);
                    context.Images.Add(objNewImage); //Добавляем объект
                    context.SaveChanges(); //Сохраняем в базу
                    ModelState.Clear();
                    return RedirectToAction("Index", "Home"); // редирект на страницу авторизации
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = $"Some exception occured {ex}";
                return View();
            }
        }
        #endregion
    }
}