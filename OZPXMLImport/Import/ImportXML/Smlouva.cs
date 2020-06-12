using System;

namespace OZPXMLImport.Import.ImportXML
{
    public class Smlouva
    {
        public string PoskytovatelZdravotnichSluzebNazev { get; set; }

        public string PoskytovatelZdravotnichSluzebTypPZS { get; set; }

        public string PojistovnaZkratka { get; set; }

        public string TypSmlouvy { get; set; }

        public DateTime DatumOd { get; set; }

        public DateTime? DatumDo { get; set; }
    }
}
