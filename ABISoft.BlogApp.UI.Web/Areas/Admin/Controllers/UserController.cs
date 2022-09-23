using ABISoft.BlogApp.Entities.Concrete;
using ABISoft.BlogApp.Entities.Dtos;
using ABISoft.BlogApp.Shared.Utilities.Extensions;
using ABISoft.BlogApp.Shared.Utilities.Results.ComplexTypes;
using ABISoft.BlogApp.UI.Web.Areas.Admin.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ABISoft.BlogApp.UI.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;

        public UserController(UserManager<User> userManager, IWebHostEnvironment env, IMapper mapper, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _env = env;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(new UserListDto()
            {
                Users = users,
                ResultStatus = ResultStatus.Success
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<JsonResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userListDto = JsonSerializer.Serialize(new UserListDto()
            {
                Users = users,
                ResultStatus = ResultStatus.Success
            }, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(userListDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            return PartialView("_UserAddPartial");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<JsonResult> Delete(int userId)
        {
            var userToDelete = await _userManager.FindByIdAsync(userId.ToString());
            var identityResult = await _userManager.DeleteAsync(userToDelete);
            if (identityResult.Succeeded)
            {
                ImageDelete(userToDelete.Picture);
                var deletedUserJson = JsonSerializer.Serialize(new UserDto()
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"{userToDelete.UserName} adlı kullanıcı başarıyla silinmiştir.",
                    User = userToDelete
                });
                return Json(deletedUserJson);
            }
            else
            {
                string errorMessages = "";
                foreach (var error in identityResult.Errors)
                    errorMessages += "*" + error.Description + '\n';
                var deletedUserErrorJson = JsonSerializer.Serialize(new UserDto()
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{userToDelete.UserName} adlı kullanıcı başarıyla silinmiştir.\n" + errorMessages,
                    User = userToDelete
                });
                return Json(deletedUserErrorJson);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            if (ModelState.IsValid)
            {
                userAddDto.Picture = await ImageUpload(userAddDto.UserName, userAddDto.PictureFile);
                var user = _mapper.Map<User>(userAddDto);
                var identityResult = await _userManager.CreateAsync(user, userAddDto.Password); 
                if (identityResult.Succeeded) //IdentityResult
                {
                    var userAddAjaxViewModel = JsonSerializer.Serialize(new UserAddAjaxViewModel()
                    {
                        UserDto = new UserDto()
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"{user.UserName} adlı kullanıcı başarıyla eklenmiştir.",
                            User = user
                        },
                        UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
                    });
                    return Json(userAddAjaxViewModel);
                }
                else
                {
                    foreach (var error in identityResult.Errors)
                        ModelState.AddModelError("", error.Description); 

                    var userAddAjaxErrorViewModel = JsonSerializer.Serialize(new UserAddAjaxViewModel()
                    {
                        UserAddDto = userAddDto,
                        UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
                    });
                    return Json(userAddAjaxErrorViewModel);
                }
            }
            var userAddAjaxErrorModelStateViewModel = JsonSerializer.Serialize(new UserAddAjaxViewModel()
            {
                UserAddDto = userAddDto,
                UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
            });
            return Json(userAddAjaxErrorModelStateViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<PartialViewResult> Update(int userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var userUpdateDto = _mapper.Map<UserUpdateDto>(user);
            return PartialView("_UserUpdatePartial", userUpdateDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                bool isNewPictureUploaded = false;
                var previousUser = await _userManager.FindByIdAsync(userUpdateDto.Id.ToString());
                var previousUserPicture = previousUser.Picture;
                if (userUpdateDto.PictureFile != null) 
                {
                    userUpdateDto.Picture = await ImageUpload(userUpdateDto.UserName, userUpdateDto.PictureFile);
                    isNewPictureUploaded = true;
                }
                var updatedUser = _mapper.Map<UserUpdateDto, User>(userUpdateDto, previousUser);
                var identityResult = await _userManager.UpdateAsync(updatedUser);
                if (identityResult.Succeeded)
                {
                    if (isNewPictureUploaded)
                        ImageDelete(previousUserPicture);
                    var userUpdateAjaxViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel()
                    {
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto),
                        UserDto = new UserDto()
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"{updatedUser.UserName} adlı kullanıcı başarıyla güncellenmiştir",
                            User = updatedUser
                        }
                    });
                    return Json(userUpdateAjaxViewModel);
                }
                else
                {
                    foreach (var error in identityResult.Errors) 
                        ModelState.AddModelError("", error.Description); 

                    var userUpdateErrorAjaxViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel()
                    {
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto),
                        UserUpdateDto = userUpdateDto
                    });
                    return Json(userUpdateErrorAjaxViewModel);
                }
            }
            var userUpdateAjaxErrorModalStateViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel()
            {
                UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto),
                UserUpdateDto = userUpdateDto
            });
            return Json(userUpdateAjaxErrorModalStateViewModel);
        }

        [Authorize] 
        [HttpGet]
        public async Task<IActionResult> ChangeDetails()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userUpdateDto = _mapper.Map<UserUpdateDto>(user);
            return View(userUpdateDto);
        }

        [Authorize] 
        [HttpPost]
        public async Task<IActionResult> ChangeDetails(UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                bool isNewPictureUploaded = false;
                var previousUser = await _userManager.GetUserAsync(HttpContext.User);
                var previousUserPicture = previousUser.Picture;
                if (userUpdateDto.PictureFile != null) 
                {
                    userUpdateDto.Picture = await ImageUpload(userUpdateDto.UserName, userUpdateDto.PictureFile);
                    isNewPictureUploaded = true;
                }
                var updatedUser = _mapper.Map<UserUpdateDto, User>(userUpdateDto, previousUser);
                var identityResult = await _userManager.UpdateAsync(updatedUser);
                if (identityResult.Succeeded)
                {
                    if (isNewPictureUploaded)
                        ImageDelete(previousUserPicture);
                    TempData.Add("SuccessMessage", $"{updatedUser.UserName} adlı kullanıcı başarıyla güncellenmiştir");
                    return View(userUpdateDto);
                }
                else
                {
                    foreach (var error in identityResult.Errors) 
                        ModelState.AddModelError("", error.Description); 
                    return View(userUpdateDto);
                }
            }
            return View(userUpdateDto);
        }

        [HttpGet]
        public ViewResult AccessDenied()
        {
            return View("AccessDenied403");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("UserLogin");
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, userLoginDto.Password, userLoginDto.RememberMe, false);
                    if (result.Succeeded) 
                    {
                        return RedirectToAction("Index", "Home");  //action -> Index, controller -> Home
                    }
                    else
                    {
                        ModelState.AddModelError("", "E-posta adresiniz ya da şifreniz hatalıdır."); 
                        return View("UserLogin");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "E-posta adresiniz ya da şifreniz hatalıdır."); 
                    return View("UserLogin");
                }
            }
            return View("UserLogin");
        }

        [Authorize]  
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        [Authorize(Roles = "Admin,Editor")]
        [NonAction]
        public async Task<string> ImageUpload(string userName, IFormFile pictureFile)
        {
            string wwwrootPath = _env.WebRootPath;
            //string fileName = Path.GetFileNameWithoutExtension(userAddDto.Picture.FileName);
            string fileExtension = Path.GetExtension(pictureFile.FileName);
            DateTime dateTime = DateTime.Now;
            string fileName = $"{userName}_{dateTime.GetFullDateTimeStringWithUnderscore()}{fileExtension}"; //BatuIlgaz_459_3_46_10_4_2022.png  
            string path = Path.Combine($"{wwwrootPath}/img", fileName);
            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream);
            }
            return fileName;
        }

        [Authorize(Roles = "Admin,Editor")]
        [NonAction]
        public bool ImageDelete(string pictureName)
        {
            string wwwrootPath = _env.WebRootPath;
            string imagetoDeletePath = Path.Combine($"{wwwrootPath}/img", pictureName);
            if (System.IO.File.Exists(imagetoDeletePath))
            {
                System.IO.File.Delete(imagetoDeletePath);
                return true;
            }
            return false;
        }
    }
}
