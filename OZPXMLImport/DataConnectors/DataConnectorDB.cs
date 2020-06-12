using OZPXMLImport.DataConnectors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZPXMLImport.DataModelSQL
{
    /// <summary>
    /// connects to DB as datasource
    /// </summary>
    public class DataConnectorDB : IDataConnector
    {
        #region Pojistovna
        public Pojistovna CreatePojistovna(string nazev, string zkratka)
        {
            var context = DBContextSingleton.GetDBContext();
            Pojistovna pojistovna;
            pojistovna = context.Pojistovna.FirstOrDefault(t => t.Nazev == nazev && t.Zkratka == zkratka);
            if (pojistovna == null)
            {
                pojistovna = new Pojistovna
                {
                    Nazev = nazev,
                    Zkratka = zkratka
                };

                context.Pojistovna.Add(pojistovna);

                context.SaveChanges();
                return context.Pojistovna.FirstOrDefault(t => t.Nazev == nazev && t.Zkratka == zkratka);
            }
            else
            {
                return pojistovna;
            }
        }

        public Pojistovna GetPojistovna(string zkratka)
        {
            var context = DBContextSingleton.GetDBContext();
            return context.Pojistovna.FirstOrDefault(t => t.Zkratka == zkratka);
        }
        #endregion

        #region PoskytovatelZdravotnichSluzeb
        public PoskytovatelZdravotnichSluzeb CreatePoskytovatelZdravotnichSluzeb(string nazev, string typPZS)
        {
            var context = DBContextSingleton.GetDBContext();
            PoskytovatelZdravotnichSluzeb poskytovatelZdravotnichSluzeb;
            int typPZSId = GetTypPZS(typPZS, DateTime.Today).Id;
            poskytovatelZdravotnichSluzeb = context.PoskytovatelZdravotnichSluzeb.FirstOrDefault(t => t.Nazev == nazev && t.TypPZSId == typPZSId);
            if (poskytovatelZdravotnichSluzeb == null)
            {
                poskytovatelZdravotnichSluzeb = new PoskytovatelZdravotnichSluzeb
                {
                    Nazev = nazev,
                    TypPZSId = typPZSId
                };

                context.PoskytovatelZdravotnichSluzeb.Add(poskytovatelZdravotnichSluzeb);

                context.SaveChanges();
                return context.PoskytovatelZdravotnichSluzeb.FirstOrDefault(t => t.Nazev == nazev && t.TypPZSId == typPZSId);
            }
            else
            {
                return poskytovatelZdravotnichSluzeb;
            }
        }

        public PoskytovatelZdravotnichSluzeb GetPoskytovatelZdravotnichSluzeb(string nazev, string typPZS)
        {
            var context = DBContextSingleton.GetDBContext();
            int typPZSId = GetTypPZS(typPZS, DateTime.Today).Id;
            return context.PoskytovatelZdravotnichSluzeb.FirstOrDefault(t => t.Nazev == nazev && t.TypPZSId == typPZSId);
        }
        #endregion

        #region TypPZS
        public TypPZS CreateTypPZS(string kod, string nazev, DateTime datumOd, DateTime? datumDo)
        {
            var context = DBContextSingleton.GetDBContext();
            TypPZS typPZS;
            typPZS = context.TypPZS.FirstOrDefault(t => t.Kod == kod && t.DatumOd == datumOd && t.DatumDo == datumDo);
            if (typPZS == null)
            {
                //check overlaping interval
                if (context.TypPZS.FirstOrDefault(t => t.Kod == kod && ((t.DatumDo != null && datumDo >= t.DatumOd && datumDo <= t.DatumDo) //datumDo inside closed interval
                                                                     || (t.DatumDo != null && datumOd <= t.DatumDo && datumOd >= t.DatumOd) //datumOd inside closed interval
                                                                     || (t.DatumDo == null && datumOd <= t.DatumOd) //datumOd before not closed interval
                                                                     || (t.DatumDo == null && datumDo <= t.DatumOd)) //datumDo before not closed interval
                                                                     ) != null)
                {
                    throw new DateOverlapingException("Dates overlap importing TypPZS");
                }

                typPZS = new TypPZS
                {
                    Kod = kod,
                    Nazev = nazev,
                    DatumOd = datumOd,
                    DatumDo = datumDo
                };

                context.TypPZS.Add(typPZS);

                //close previous interval(s)
                var toUpdate = context.TypPZS.Where(t => t.Kod == kod && t.DatumDo == null).ToList();
                toUpdate.ForEach(t => t.DatumDo = datumOd.AddDays(-1));

                context.SaveChanges();
                return context.TypPZS.FirstOrDefault(t => t.Kod == kod && t.DatumOd == datumOd && t.DatumDo == datumDo);
            }
            else
            {
                return typPZS;
            }
        }

        public TypPZS GetTypPZS(string kod, DateTime datum)
        {
            var context = DBContextSingleton.GetDBContext();
            return context.TypPZS.FirstOrDefault(t => t.Kod == kod && datum >= t.DatumOd && (t.DatumDo == null || datum <= t.DatumDo));
        }
        #endregion

        #region TypSmlouvy
        public TypSmlouvy CreateTypSmlouvy(string kod, string nazev, DateTime datumOd, DateTime? datumDo)
        {
            var context = DBContextSingleton.GetDBContext();
            TypSmlouvy typSmlouvy;
            typSmlouvy = context.TypSmlouvy.FirstOrDefault(t => t.Kod == kod && t.Nazev == nazev && t.DatumOd == datumOd && t.DatumDo == datumDo);
            if (typSmlouvy == null)
            {
                //check overlaping interval
                if (context.TypSmlouvy.FirstOrDefault(t => t.Kod == kod && ((t.DatumDo != null && datumDo >= t.DatumOd && datumDo <= t.DatumDo) //datumDo inside closed interval
                                                                         || (t.DatumDo != null && datumOd <= t.DatumDo && datumOd >= t.DatumOd) //datumOd inside closed interval
                                                                         || (t.DatumDo == null && datumOd <= t.DatumOd) //datumOd before not closed interval
                                                                         || (t.DatumDo == null && datumDo <= t.DatumOd)) //datumDo before not closed interval
                                                                         ) != null)
                {
                    throw new DateOverlapingException("Dates overlap importing TypSmlouvy");
                }

                typSmlouvy = new TypSmlouvy
                {
                    Kod = kod,
                    Nazev = nazev,
                    DatumOd = datumOd,
                    DatumDo = datumDo
                };

                context.TypSmlouvy.Add(typSmlouvy);

                //close previous interval(s)
                var toUpdate = context.TypSmlouvy.Where(t => t.Kod == kod && t.DatumDo == null).ToList();
                toUpdate.ForEach(t => t.DatumDo = datumOd.AddDays(-1));

                context.SaveChanges();
                return context.TypSmlouvy.FirstOrDefault(t => t.Kod == kod && t.Nazev == nazev && t.DatumOd == datumOd && t.DatumDo == datumDo);
            }
            else
            {
                return typSmlouvy;
            }
        }

        public TypSmlouvy GetTypSmlouvy(string kod, DateTime datum)
        {
            var context = DBContextSingleton.GetDBContext();
            return context.TypSmlouvy.FirstOrDefault(t => t.Kod == kod && datum >= t.DatumOd && (t.DatumDo == null || datum <= t.DatumDo));
        }
        #endregion

        public Smlouva CreateSmlouva(Smlouva smlouva)
        {
            var context = DBContextSingleton.GetDBContext();
            //check overlaping interval
            if (context.Smlouva.FirstOrDefault(t => t.PoskytovatelZdravotnichSluzebId == smlouva.PoskytovatelZdravotnichSluzebId
                                                 && t.PojistovnaId == smlouva.PojistovnaId
                                                 && t.TypSmlouvyId == smlouva.TypSmlouvyId
                                                 && ((t.DatumDo != null && smlouva.DatumDo >= t.DatumOd && smlouva.DatumDo <= t.DatumDo) //datumDo inside closed interval
                                                  || (t.DatumDo != null && smlouva.DatumOd <= t.DatumDo && smlouva.DatumOd >= t.DatumOd) //datumOd inside closed interval
                                                  || (t.DatumDo == null && smlouva.DatumOd <= t.DatumOd) //datumOd before not closed interval
                                                  || (t.DatumDo == null && smlouva.DatumDo <= t.DatumOd)) //datumDo before not closed interval
                                                  ) != null)
            {
                throw new DateOverlapingException("Dates overlap importing Smlouva");
            }
            context.Smlouva.Add(smlouva);

            //close previous interval(s)
            var toUpdate = context.Smlouva.Where(t => t.PoskytovatelZdravotnichSluzebId == smlouva.PoskytovatelZdravotnichSluzebId
                                                   && t.PojistovnaId == smlouva.PojistovnaId
                                                   && t.TypSmlouvyId == smlouva.TypSmlouvyId
                                                   && t.DatumDo == null).ToList();
            toUpdate.ForEach(t => t.DatumDo = smlouva.DatumOd.AddDays(-1));

            context.SaveChanges();

            return context.Smlouva.FirstOrDefault(t => t.PoskytovatelZdravotnichSluzebId == smlouva.PoskytovatelZdravotnichSluzebId
                                                    && t.PojistovnaId == smlouva.PojistovnaId
                                                    && t.TypSmlouvyId == smlouva.TypSmlouvyId
                                                    && t.DatumDo == null);
        }

        public void OutputList(DateTime datum)
        {
            Console.WriteLine($"{"Id Smlouvy",12}{"Název PZS",30}{"Kód Typu PZS",13}{"Pojišťovna Název",35}{"Pojišťovna Zkratka",19}{"Typ Smlouvy Kód",16}{"Typ Smlouvy Název",30}{"Datum Od",13}{"Datum Do",13}");
            Console.WriteLine($"{Line(13)}{Line(30)}{Line(13)}{Line(35)}{Line(19)}{Line(16)}{Line(30)}{Line(13)}{Line(13)}");
            var context = DBContextSingleton.GetDBContext();
            var list = context.GetListOfSmlouva(datum);
            foreach (var item in list)
            {
                Console.WriteLine($"{item.IdSmlouvy,12}{item.NazevPZS,30}{item.KodTypuPZS,13}{item.PojistovnaNazev,35}{item.PojistovnaZkratka,19}{item.TypSmlouvyKod,16}{item.TypSmlouvyNazev,30}{item.DatumOd,13:yyyy-MM-dd}{item.DatumDo,13:yyyy-MM-dd}");
            }
        }

        /// <summary>
        /// returns dashed line
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static string Line(int n)
        {
            return new String('-', n - 1) + " ";
        }
    }
}
