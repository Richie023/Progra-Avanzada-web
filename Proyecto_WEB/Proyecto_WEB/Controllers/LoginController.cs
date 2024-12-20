using Microsoft.AspNetCore.Mvc;
using Proyecto_WEB.Models;
using static System.Net.WebRequestMethods;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using Proyecto_WEB.Servicios;

namespace Proyecto_WEB.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _conf;
        private readonly IMetodosComunes _comunes;
        public LoginController(IHttpClientFactory http, IConfiguration conf, IMetodosComunes comunes)
        {
            _http = http;
            _conf = conf;
            _comunes = comunes;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Usuario model)
        {
            using (var client = _http.CreateClient())
            {
                var url = _conf.GetSection("Variables:UrlApi").Value + "Login/IniciarSesion";

                model.Contrasenna = Encrypt(model.Contrasenna);
                JsonContent datos = JsonContent.Create(model);

                var response = client.PostAsync(url, datos).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

              
                if (result != null && result.Codigo == 0)
                {
                    var datosUsuario = JsonSerializer.Deserialize<Usuario>((JsonElement)result.Contenido!);
                    if (datosUsuario!.ClaveTemp == true)
                    {
                        HttpContext.Session.SetString("Consecutivo", datosUsuario!.UsuarioID.ToString());
                        return RedirectToAction("ChangePassword", "Login");
                    }
                    else
                    {
                        HttpContext.Session.SetString("Consecutivo", datosUsuario!.UsuarioID.ToString());
                        HttpContext.Session.SetString("NombreUsuario", datosUsuario!.Username);
                        HttpContext.Session.SetString("Rol", datosUsuario!.NombreRol);


                        var datosMembresiaMiembro = _comunes.ConsultarMembresiaMiembro();
                        HttpContext.Session.SetString("TipoMembresia", datosMembresiaMiembro.FirstOrDefault()?.TipoMembresia ?? "");

                        return RedirectToAction("Index", "Home");
                    }
                   
                }
                else
                {
                    ViewBag.Mensaje = result!.Mensaje;
                    return View();
                }
            }
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Cliente model)
        {
            using (var client = _http.CreateClient())
            {
                var url = _conf.GetSection("Variables:UrlApi").Value + "Login/CrearCliente";

                model.Contrasenna = Encrypt(model.Contrasenna);
                JsonContent datos = JsonContent.Create(model);

                var response = client.PostAsync(url, datos).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    ViewBag.Mensaje = result!.Mensaje;
                    return View();
                }
            }
        }

        public IActionResult Recovery()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Recovery(Usuario model)
        {
            using (var client = _http.CreateClient())
            {
                var url = _conf.GetSection("Variables:UrlApi").Value + "Login/RecuperarAcceso";

                JsonContent datos = JsonContent.Create(model.Email);

                var response = client.PostAsync(url, datos).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 1)
                {
                    return RedirectToAction("Login", "Login");
                }
                else if (result!.Codigo == 0)
                {
                    ViewBag.Mensaje = "Ocurrió un error";
                    return View();
                }
                else
                {
                    ViewBag.Mensaje = result!.Mensaje;
                    return View();
                }
            }
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(Usuario model)
        {
            model.Contrasenna = Encrypt(model.Contrasenna);
            model.ConfirmarContrasenna = Encrypt(model.ConfirmarContrasenna);

            if (model.Contrasenna != model.ConfirmarContrasenna)
            {
                ViewBag.Mensaje = "La confirmación de su contraseña no coincide";
                return View();
            }

            model.UsuarioID = long.Parse(HttpContext.Session.GetString("Consecutivo")!.ToString());

            using (var client = _http.CreateClient())
            {
                var url = _conf.GetSection("Variables:UrlApi").Value + "Login/ActualizarContrasenna";

                JsonContent datos = JsonContent.Create(model);

                var response = client.PutAsync(url, datos).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    return RedirectToAction("Logout", "Login");
                }
                else
                {
                    ViewBag.Mensaje = result!.Mensaje;
                    return View();
                }
            }
        }
        public string Encrypt(string texto)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                
                
                aes.Key = Encoding.UTF8.GetBytes(_conf.GetSection("Variables:Llave").Value!);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(texto);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }


        public string Decrypt(string texto)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(texto);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_conf.GetSection("Variables:Llave").Value!);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

    }
}
