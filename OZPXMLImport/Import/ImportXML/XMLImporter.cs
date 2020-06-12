using EF = OZPXMLImport.DataModelSQL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OZPXMLImport.Import.ImportXML
{
    /// <summary>
    /// imports data from XML
    /// </summary>
    public class XMLImporter : IImporter
    {
        private EF.IDataConnector _dataConnector;

        public XMLImporter(EF.IDataConnector dataConnector)
        {
            _dataConnector = dataConnector;
        }

        public void Import(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportBatch));
            ImportBatch importBatch;
#if DEBUG
            //creates xml to import ...
            //serializer
            importBatch = new ImportBatch();

            importBatch.TypyPZS.Add(new TypPZS() { Kod = "P4", Nazev = "Anesteziolog", DatumOd = new DateTime(2020, 6, 1) });
            importBatch.TypyPZS.Add(new TypPZS() { Kod = "P5", Nazev = "Chirurg", DatumOd = new DateTime(2020, 6, 1) });

            importBatch.PoskytovateleZdravotnichSluzeb.Add(new PoskytovatelZdravotnichSluzeb() { Nazev = "FN U svaté Anny", TypPZS = "P4" });
            importBatch.PoskytovateleZdravotnichSluzeb.Add(new PoskytovatelZdravotnichSluzeb() { Nazev = "FN U svaté Anny", TypPZS = "P1" });

            importBatch.Pojistovny.Add(new Pojistovna() { Nazev = "Oborová zdravotní pojišťovna zaměstnanců bank, pojišťoven a stavebnictví", Zkratka = "OZP" });

            importBatch.TypySmlouvy.Add(new TypSmlouvy() { Kod = "A003", Nazev = "Smlouva o vyšetření", DatumOd = new DateTime(2020, 6, 1) });

            importBatch.Smlouvy.Add(new Smlouva()
            {
                PoskytovatelZdravotnichSluzebNazev = "FN U svaté Anny",
                PoskytovatelZdravotnichSluzebTypPZS = "P4",
                PojistovnaZkratka = "OZP",
                TypSmlouvy = "A003",
                DatumOd = new DateTime(2020, 6, 1),
                DatumDo = null
            });

            importBatch.Smlouvy.Add(new Smlouva()
            {
                PoskytovatelZdravotnichSluzebNazev = "MDDr. Pavel Zoubek",
                PoskytovatelZdravotnichSluzebTypPZS = "P1",
                PojistovnaZkratka = "VZP",
                TypSmlouvy = "A002",
                DatumOd = new DateTime(2020, 6, 1),
                DatumDo = null
            });

            using (TextWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, importBatch);
            }
#endif
            //deserializer
            TextReader reader = new StreamReader(path);
            object obj = serializer.Deserialize(reader);
            importBatch = (ImportBatch)obj;
            reader.Close();

            ImportTypPZS(importBatch);
            ImportPoskytovatelZdravotnichSluzeb(importBatch);
            ImportPojistovna(importBatch);
            ImportTypSmlouvy(importBatch);
            ImportSmlouva(importBatch);


        }

        private void ImportTypPZS(ImportBatch importBatch)
        {
            foreach (TypPZS item in importBatch.TypyPZS)
            {
                _dataConnector.CreateTypPZS(item.Kod, item.Nazev, item.DatumOd, item.DatumDo);
            }
        }

        private void ImportPoskytovatelZdravotnichSluzeb(ImportBatch importBatch)
        {
            foreach (PoskytovatelZdravotnichSluzeb item in importBatch.PoskytovateleZdravotnichSluzeb)
            {
                _dataConnector.CreatePoskytovatelZdravotnichSluzeb(item.Nazev, item.TypPZS);
            }
        }

        private void ImportPojistovna(ImportBatch importBatch)
        {
            foreach (Pojistovna item in importBatch.Pojistovny)
            {
                _dataConnector.CreatePojistovna(item.Nazev, item.Zkratka);
            }
        }

        private void ImportTypSmlouvy(ImportBatch importBatch)
        {
            foreach (TypSmlouvy item in importBatch.TypySmlouvy)
            {
                _dataConnector.CreateTypSmlouvy(item.Kod, item.Nazev, item.DatumOd, item.DatumDo);
            }
        }

        private void ImportSmlouva(ImportBatch importBatch)
        {
            foreach (Smlouva item in importBatch.Smlouvy)
            {
                int typSmlouvyId = _dataConnector.GetTypSmlouvy(item.TypSmlouvy, item.DatumOd).Id;
                int pojistovnaId = _dataConnector.GetPojistovna(item.PojistovnaZkratka).Id;
                int poskytovatelZdravotnichSluzebId = _dataConnector.GetPoskytovatelZdravotnichSluzeb(item.PoskytovatelZdravotnichSluzebNazev, item.PoskytovatelZdravotnichSluzebTypPZS).Id;

                EF.Smlouva smlouva = new EF.Smlouva()
                {
                    PoskytovatelZdravotnichSluzebId = poskytovatelZdravotnichSluzebId,
                    PojistovnaId = pojistovnaId,
                    TypSmlouvyId = typSmlouvyId,
                    DatumOd = item.DatumOd,
                    DatumDo = item.DatumDo
                };

                _dataConnector.CreateSmlouva(smlouva);
            }
        }
    }
}
