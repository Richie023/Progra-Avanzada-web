using Microsoft.AspNetCore.Mvc;
using Proyecto_WEB.Models;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using static System.Net.WebRequestMethods;
using System.Text.Json;

namespace Proyecto_WEB.Servicios
{
    public class MetodosComunes : IMetodosComunes
    {
        private readonly IConfiguration _conf;
        private readonly IHttpClientFactory _http;
        private readonly IHttpContextAccessor _accesor;
        public MetodosComunes(IConfiguration conf, IHttpClientFactory http, IHttpContextAccessor accesor)
        {
            _conf = conf;
            _http = http;
            _accesor = accesor;
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

        public List<Miembro> ConsultarMembresiaMiembro()
        {
            using (var client = _http.CreateClient())
            {
                var usuarioId = long.Parse(_accesor.HttpContext!.Session.GetString("Consecutivo")!.ToString());
                var url = _conf.GetSection("Variables:UrlApi").Value + $"Membresias/ConsultarMembresiaMiembro/{usuarioId}";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accesor.HttpContext.Session.GetString("Consecutivo"));
                var response = client.GetAsync(url).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    var datosContenido = JsonSerializer.Deserialize<List<Miembro>>((JsonElement)result.Contenido!);
                    return datosContenido!.ToList();
                }

                return new List<Miembro>();
            }
        }

        public List<Carrito> ConsultarCarrito()
        {
            using (var client = _http.CreateClient())
            {
                var usuarioId = long.Parse(_accesor.HttpContext!.Session.GetString("Consecutivo")!.ToString());
                var url = _conf.GetSection("Variables:UrlApi").Value + $"Carrito/ConsultarCarrito/{usuarioId}";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accesor.HttpContext.Session.GetString("Consecutivo"));
                var response = client.GetAsync(url).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    var datosContenido = JsonSerializer.Deserialize<List<Carrito>>((JsonElement)result.Contenido!);
                    return datosContenido!.ToList();
                }

                return new List<Carrito>();
            }
        }
    }
}
