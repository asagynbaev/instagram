using System.Linq;
using System.Web.Mvc;
using Instagram.Classes;
using Instagram.Models;
using System;
using System.Web;
using System.Web.Security;

namespace Instagram.Controllers
{
    public class AccountController : Controller
    {

        [HttpGet] // GET : Login
        public ActionResult Login()
        {
            return View();
        }

        #region Авторизация
        [HttpPost] // POST : Login
        public ActionResult Login(Account objUser)
        {
            try
            {
                using (InstagramDBContext db = new InstagramDBContext())
                {
                    var users = db.Users.Where(x => x.username == objUser.username); //Ищем имя пользователя в таблице Account
                    if (users.Count() > 0)
                    {
                        Account account = new Account(); // Создаем новый экземпляр класса
                        foreach (var user in users) // Перебор резултата возвращенного из БД
                        {
                            account = user;
                            account.password = Helper.base64Decode(user.password); //декодируем пароль
                        }
                        if (account.password == objUser.password) // Если пароли совпадают
                        {
                            FormsAuthentication.SetAuthCookie(account.id.ToString(), false); // Создаем куки с логином
                            return RedirectToAction("Index", "Home"); //Редирект на главную страницу
                        }
                        else
                        {
                            TempData["alertMessage"] = "Incorrect password, try again";
                            return View();
                        }
                    }
                    else
                    {
                        TempData["alertMessage"] = "User is not found";
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["alertMessage"] = $"Some exception occured {ex}";
                return View();
            }
        }
        #endregion

        [HttpGet] // GET : Registration
        public ActionResult Register()
        {
            return View();
        }

        #region Регистрация
        [HttpPost] //POST : Registration
        public ActionResult Register(Account objNewAccount)
        {
            try
            {
                HttpPostedFileBase img = Request.Files["image"]; //дергаем картинку с объекта
                using (var context = new InstagramDBContext())
                {
                    // Проверяем на занятость логина
                    var checkUser = (from s in context.Users where s.username == objNewAccount.username select s).FirstOrDefault();
                    if (checkUser == null) // Если логин свободен
                    {
                        objNewAccount.password = Helper.base64Encode(objNewAccount.password); // кодированный пароль
                        objNewAccount.avatar = Helper.ConvertToBytes(img); // сконвертированная картинка
                        objNewAccount.createdAt = DateTime.Now;
                        objNewAccount.updatedAt = DateTime.Now;
                        context.Users.Add(objNewAccount); //Добавляем объект
                        context.SaveChanges(); //Сохраняем в базу
                        ModelState.Clear();
                        return RedirectToAction("Login", "Account"); // редирект на страницу авторизации
                    }
                    TempData["notice"] = "User Allredy Exixts";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = $"Some exception occured {ex}";
                return View();
            }
        }
        #endregion
        
        [Authorize]
        [HttpGet] // GET : Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut(); //Стираем куки
            return RedirectToAction("Login"); //Редирект на логин
        }

        [Authorize]
        [HttpGet] // GET : UserProfile
        public ActionResult UserProfile()
        {
            var user_id = Convert.ToInt32(HttpContext.User.Identity.Name); //Берем логин из куков
            var model = new AccountImagesModel();
            try
            {
                using (var db = new InstagramDBContext())
                {
                    var users = db.Users.Where(x => x.id == user_id).ToList();
                    model.Account = users[0];
                    
                    var posts = db.Images.Where(x => x.user_id == model.Account.id);
                    model.countOfImages = posts.Count();
                    foreach (var img in posts)
                    {
                        model.Images.Add(img);
                    }
                }
            }
            catch (Exception ex)
            {
                //ignored
            }

            return View(model);
        }
    }
}
