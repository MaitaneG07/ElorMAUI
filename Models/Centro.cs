namespace ElorMAUI.Models
{
    public class Centro
    {

        public int? CCEN { get; set; }//no existia
        public string? NOM { get; set; }

        public string? DMUNIC { get; set; } // Municipio
        public string? DTERRC { get; set; } // Territorio/Provincia
        public string? DTITUC { get; set; } // Titularidad
        public double LATITUD { get; set; }
        public double LONGITUD { get; set; }
        public string? DOMI { get; set; }
        public long? TEL1 { get; set; }
        public string? EMAIL { get; set; }
    }

    public class CentroResponse
    {
        public List<Centro> CENTROS { get; set; } = new();
    }
}