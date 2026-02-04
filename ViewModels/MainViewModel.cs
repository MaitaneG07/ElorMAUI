using ElorMAUI.Models;
using ElorMAUI.Services;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElorMAUI.ViewModels
{
    public class MainViewModel : BindableObject
    {
        private readonly CentroService _centroService;

        public List<Centro> TodosLosCentros { get; set; } = new();
        public List<Centro> CentrosFiltrados { get; set; } = new();
        public Centro? CentroSeleccionado { get; set; }

        public List<string?> ListaTitularidades { get; set; } = new();
        public List<string?> ListaTerritorios { get; set; } = new();

        public MainViewModel(CentroService centroService)
        {
            _centroService = centroService;
        }

        public async Task CargarDatos()
        {
            var centros = await _centroService.ObtenerCentrosAsync();

            if (centros != null && centros.Any())
            {
                TodosLosCentros = centros;
                CentrosFiltrados = centros;

                // 1. CARGA DE TITULARIDADES (Usa DTITUC, es correcto)
                ListaTitularidades = TodosLosCentros
                    .Select(c => c.DTITUC)
                    .Where(t => !string.IsNullOrEmpty(t))
                    .Distinct()
                    .Cast<string?>()
                    .ToList();
            }
        }

        public void FiltrarTerritorios(string titularidad)
        {
            if (string.IsNullOrEmpty(titularidad))
            {
                ListaTerritorios = new();
            }
            else
            {
                // 2. CORRECCIÓN: Cambiado DTERRE por DTERRC para que coincida con el JSON
                ListaTerritorios = TodosLosCentros
                    .Where(c => c.DTITUC == titularidad)
                    .Select(c => c.DTERRC) // <--- Aquí estaba el fallo
                    .Where(t => !string.IsNullOrEmpty(t))
                    .Distinct()
                    .Cast<string?>()
                    .ToList();
            }
        }

        public void AplicarFiltroFinal(string titularidad, string territorio)
        {
            var query = TodosLosCentros.AsEnumerable();

            if (!string.IsNullOrEmpty(titularidad))
                query = query.Where(c => c.DTITUC == titularidad);

            if (!string.IsNullOrEmpty(territorio))
                // 3. CORRECCIÓN: Cambiado DTERRE por DTERRC
                query = query.Where(c => c.DTERRC == territorio);

            CentrosFiltrados = query.ToList();
        }
    }
}