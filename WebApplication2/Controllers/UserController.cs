﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using WebApplication2.Models;
using WebApplication2.Models.API;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> ProfileInfo()
        {

            string token = GetToken();
            using (var httpClient = new HttpClient())
            {

                var apiUrl = "https://dev.edi.md/ISAuthService/json/GetProfileInfo?Token=" + token;

                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Чтение данных из HTTP-ответа.

                    var data = await response.Content.ReadAsAsync<GetProfileInfo>();
                    if (data.ErrorCode == 143)
                    {
                        await RefreshToken();
                        return await ProfileInfo();
                    }
                    else if (data.ErrorCode == 118)
                    {
                        return View("~/Views/Account/Login.cshtml");
                    }
                    else if (data.ErrorCode == 0)
                    {
                        return View("~/Views/User/ProfileInfo.cshtml", data.User);
                    }



                }
            }
            return View("~/Views/Account/Login.cshtml"); /////////!!!!!!
        }

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            string token = GetToken();
            using (var httpClient = new HttpClient())
            {

                var apiUrl = "https://dev.edi.md/ISAuthService/json/GetProfileInfo?Token=" + token;

                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Чтение данных из HTTP-ответа.

                    var data = await response.Content.ReadAsAsync<GetProfileInfo>();
                    if (data.ErrorCode == 143)
                    {
                        await RefreshToken();
                        return await Settings();
                    }
                    else if (data.ErrorCode == 118)
                    {
                        return View("~/Views/Account/Login.cshtml");
                    }
                    else if (data.ErrorCode == 0)
                    {
                        return View("~/Views/User/Settings.cshtml", data.User);
                    }



                }
            }
            return View("~/Views/Account/Login.cshtml"); /////////!!!!!!
        }

        [HttpGet]   //!
        public async Task<IActionResult> ChangePassword()
        {
            return PartialView("~/Views/User/_ChangePassword.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeConfirmPasswordViewModel changepwVM)   //FromBody
        {

            if (!ModelState.IsValid)
            {
                return Json(new { StatusCode = 500, Message = "No Valid" });
                //return View();
            }

            string token = GetToken();

            ChangePasswordViewModel changePassword = new ChangePasswordViewModel()
            {
                NewPassword = changepwVM.NewPassword,
                OldPassword = changepwVM.OldPassword,
                Token = token
            };

            using (var httpClient = new HttpClient())
            {
                // Замените URL на ссылку, с которой вы хотите получить данные.
                var apiUrl = "https://dev.edi.md/ISAuthService/json/ChangePassword";

                // Преобразуйте объект loginVM в JSON и отправьте его с помощью HttpContent.
                var jsonContent = new StringContent(JsonConvert.SerializeObject(changePassword), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(apiUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    // Чтение данных из HTTP-ответа.
                    var data = await response.Content.ReadAsAsync<BaseResponse>();

                    if (data.ErrorCode == 143)
                    {
                        await RefreshToken();
                        return await ChangePassword(changepwVM); ///!
                    }
                    else if (data.ErrorCode == 0)
                    {
                        //TempData["Success"] = "Password chenged";
                        //return View("~/Views/User/Settings.cshtml");

                        return Json(new { StatusCode = 200, Message = "Password changed successfully" });
                    }

                    else
                    {
                        //TempData["Error"] = "Password unchenged";
                        //return View("~/Views/User/Settings.cshtml");
                        return Json(new { StatusCode = 500, Message = data.ErrorMessage });
                    }
                }
                else
                {
                    // Обработка ошибки, если запрос не удался.
                    return View("Error");
                }
            }

        }


        [HttpGet]   //!
        public async Task<IActionResult> GetQuestionnaires()
        {
            string token = GetToken();
            using (var httpClient = new HttpClient())
            {

                var apiUrlForGetProfileInfo = "https://dev.edi.md/ISAuthService/json/GetProfileInfo?Token=" + token;

                var responseGetProfileInfo = await httpClient.GetAsync(apiUrlForGetProfileInfo);

                if (responseGetProfileInfo.IsSuccessStatusCode)
                {
                    // Чтение данных из HTTP-ответа.

                    var userData = await responseGetProfileInfo.Content.ReadAsAsync<GetProfileInfo>();
                    if (userData.ErrorCode == 143)
                    {
                        await RefreshToken();
                        return await GetQuestionnaires();
                    }
                    else if (userData.ErrorCode == 118)
                    {
                        return View("~/Views/Account/Login.cshtml");
                    }
                    else if (userData.ErrorCode == 0)
                    {
                        using (var httpClient1 = new HttpClient())
                        {

                            var apiUrlGetQuestionnairesByToken = "https://dev.edi.md/ISNPSAPI/Web/GetQuestionnaires?token=" + token;
                            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));

                            // Добавляем аутентификацию в заголовок Authorization с префиксом "Basic ".
                            httpClient1.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                            var responseGetQuestionnaires = await httpClient1.GetAsync(apiUrlGetQuestionnairesByToken);

                            if (responseGetQuestionnaires.IsSuccessStatusCode)
                            {
                                var questionnaireData = await responseGetQuestionnaires.Content.ReadAsAsync<GetQuestionnaireInfo>();
                                if (questionnaireData.errorCode == 143)
                                {
                                    await RefreshToken();
                                    return await GetQuestionnaires();
                                }
                                else if (questionnaireData.errorCode == 118)
                                {
                                    return View("~/Views/Account/Login.cshtml");
                                }
                                else if (questionnaireData.errorCode == 0)
                                {
                                    return PartialView("~/Views/User/_Questionnaire.cshtml", questionnaireData.questionnaires);
                                }



                            }
                        }
                    }



                }
            }

            return PartialView("~/Views/User/_Questionnaire.cshtml");
        }



    }
}
