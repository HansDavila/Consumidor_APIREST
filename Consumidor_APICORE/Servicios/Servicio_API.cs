using Consumidor_APICORE.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Text;

namespace Consumidor_APICORE.Servicios
{
    public class Servicio_API : IServicio_API
    {
        private static string _correo;
        private static string _clave;
        private static string _baseurl;
        private static string _token;

        public Servicio_API()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            _correo = builder.GetSection("ApiSettings:correo").Value;
            _clave = builder.GetSection("ApiSettings:clave").Value;
            _baseurl = builder.GetSection("ApiSettings:baseUrl").Value;
        }

        public async Task Autenticar()
        {
            var cliente = new HttpClient();

            cliente.BaseAddress = new Uri(_baseurl);

            var credenciales = new Credencial()
            {
                correo = _correo,
                clave = _clave
            };

            var content = new StringContent(JsonConvert.SerializeObject(credenciales), Encoding.UTF8, "application/json");

            var response = await cliente.PostAsync("api/Autenticacion/Validar", content); //peticion Post
            var json_respuesta = await response.Content.ReadAsStringAsync();

            var resultado = JsonConvert.DeserializeObject<ResultadoCredencial>(json_respuesta);

            _token = resultado.token;
        }

        public async Task<List<Producto>> Lista()
        {
            List<Producto> lista = new List<Producto>();

            await Autenticar();

            var cliente = new HttpClient();

            //Autenticacion
            cliente.BaseAddress = new Uri(_baseurl);
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            //Peticion GET
            var response = await cliente.GetAsync("api/Producto/Lista");

            if(response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi>(json_respuesta);
                lista = resultado.response;
            }

            return lista;
        }

        public async Task<Producto> Obtener(int idProducto)
        {
            Producto objeto = new Producto();

            await Autenticar();

            var cliente = new HttpClient();

            //Autenticacion
            cliente.BaseAddress = new Uri(_baseurl);
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            //Peticion GET
            var response = await cliente.GetAsync($"api/Producto/Obtener/{idProducto}");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ResultadoApi>(json_respuesta);
                objeto = resultado.objeto;
            }

            return objeto;
        }

        public async Task<bool> Guardar(Producto objeto)
        {
            bool respuesta = false;

            await Autenticar();

            var cliente = new HttpClient();

            //Autenticacion
            cliente.BaseAddress = new Uri(_baseurl);
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            //Peticion POST
            var content = new StringContent(JsonConvert.SerializeObject(objeto), Encoding.UTF8, "application/json");

            var response = await cliente.PostAsync($"api/Producto/Guardar/", content);

            if (response.IsSuccessStatusCode)
            {
                respuesta = true;
            }

            return respuesta;
        }

        public async Task<bool> Editar(Producto objeto)
        {
            bool respuesta = false;

            await Autenticar();

            var cliente = new HttpClient();

            //Autenticacion
            cliente.BaseAddress = new Uri(_baseurl);
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            //Peticion PUT
            var content = new StringContent(JsonConvert.SerializeObject(objeto), Encoding.UTF8, "application/json");

            var response = await cliente.PutAsync($"api/Producto/Editar/", content);

            if (response.IsSuccessStatusCode)
            {
                respuesta = true;
            }

            return respuesta;
        }

        public async Task<bool> Eliminar(int idProducto)
        {
            bool respuesta = false;

            await Autenticar();

            var cliente = new HttpClient();

            //Autenticacion
            cliente.BaseAddress = new Uri(_baseurl);
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            //Peticion DELETE
            var response = await cliente.DeleteAsync($"api/Producto/Eliminar/{idProducto}");

            if (response.IsSuccessStatusCode)
            {
                respuesta = true;
            }

            return respuesta;
        }

        

        

        
    }
}
