using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OZPXMLImport.Import.ImportXML
{
    [XmlRoot("Davka")]
    public class ImportBatch
    {
        public ImportBatch()
        {
            TypyPZS = new List<TypPZS>();
            PoskytovateleZdravotnichSluzeb  = new List<PoskytovatelZdravotnichSluzeb>();
            TypySmlouvy = new List<TypSmlouvy>();
            Pojistovny = new List<Pojistovna>();
            Smlouvy = new List<Smlouva>();
        }

        public List<TypPZS> TypyPZS { get; set; }

        public List<PoskytovatelZdravotnichSluzeb> PoskytovateleZdravotnichSluzeb { get; set; }

        public List<TypSmlouvy> TypySmlouvy { get; set; }

        public List<Pojistovna> Pojistovny { get; set; }

        public List<Smlouva> Smlouvy { get; set; }
    }
}
