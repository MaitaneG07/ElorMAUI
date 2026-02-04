using ElorMAUI.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ElorMAUI.Services
{
    public class CentroService
    {
        private readonly HttpClient _http;
        private List<Centro>? _cacheCentros; // Guardamos los datos aquí para no leer el archivo cada vez

        public CentroService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Centro>> ObtenerCentrosAsync()
        {
            try
            {
                // 1. Abrir y leer el archivo
                using var stream = await FileSystem.OpenAppPackageFileAsync("jsonReto2.txt");
                using var reader = new StreamReader(stream);
                var contents = await reader.ReadToEndAsync();

                if (string.IsNullOrWhiteSpace(contents)) return new List<Centro>();

                // 2. Configurar opciones (insensible a mayúsculas para evitar fallos)
                var opciones = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,

                    NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
                };

                // 3. SERIALIZACIÓN CLAVE:
                // En lugar de deserializar una List, deserializamos el OBJETO que contiene la lista
                var respuesta = JsonSerializer.Deserialize<CentroResponse>(contents, opciones);

                // Devolvemos la lista que está dentro de la propiedad CENTROS
                return respuesta?.CENTROS ?? new List<Centro>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error de serialización: {ex.Message}");
                return new List<Centro>();
            }
        }
    }
}
 
