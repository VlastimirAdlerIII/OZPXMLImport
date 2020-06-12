using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZPXMLImport.DataModelSQL
{
    public interface IDataConnector
    {
        /// <summary>
        /// creates TypPZS in datasource
        /// </summary>
        /// <param name="kod"></param>
        /// <param name="nazev"></param>
        /// <param name="datumOd"></param>
        /// <param name="datumDo"></param>
        /// <returns></returns>
        TypPZS CreateTypPZS(string kod, string nazev, DateTime datumOd, DateTime? datumDo);

        /// <summary>
        /// gets TypPZS from datasource
        /// </summary>
        /// <param name="kod"></param>
        /// <param name="datum"></param>
        /// <returns></returns>
        TypPZS GetTypPZS(string kod, DateTime datum);

        /// <summary>
        /// creates TypSmlouvy in datasource
        /// </summary>
        /// <param name="kod"></param>
        /// <param name="nazev"></param>
        /// <param name="datumOd"></param>
        /// <param name="datumDo"></param>
        /// <returns></returns>
        TypSmlouvy CreateTypSmlouvy(string kod, string nazev, DateTime datumOd, DateTime? datumDo);

        /// <summary>
        /// gets TypSmlouvy from datasource
        /// </summary>
        /// <param name="kod"></param>
        /// <param name="nazev"></param>
        /// <param name="datumOd"></param>
        /// <param name="datumDo"></param>
        /// <returns></returns>
        TypSmlouvy GetTypSmlouvy(string kod, DateTime datum);

        /// <summary>
        /// creates Pojistovna in datasource
        /// </summary>
        /// <param name="nazev"></param>
        /// <param name="zkratka"></param>
        /// <returns></returns>
        Pojistovna CreatePojistovna(string nazev, string zkratka);

        /// <summary>
        /// gets Pojistovna from datasource
        /// </summary>
        /// <param name="nazev"></param>
        /// <returns></returns>
        Pojistovna GetPojistovna(string zkratka);

        /// <summary>
        /// creates PoskytovatelZdravotnichSluzeb in datasource
        /// </summary>
        /// <param name="nazev"></param>
        /// <param name="typPZS"></param>
        /// <returns></returns>
        PoskytovatelZdravotnichSluzeb CreatePoskytovatelZdravotnichSluzeb(string nazev, string typPZS);

        /// <summary>
        /// gets PoskytovatelZdravotnichSluzeb from datasource
        /// </summary>
        /// <param name="nazev"></param>
        /// <param name="typPZS"></param>
        /// <returns></returns>
        PoskytovatelZdravotnichSluzeb GetPoskytovatelZdravotnichSluzeb(string nazev, string typPZS);

        /// <summary>
        /// creates Smlouva in datasource
        /// </summary>
        /// <param name="smlouva"></param>
        /// <returns></returns>
        Smlouva CreateSmlouva(Smlouva smlouva);

        /// <summary>
        /// writes list to console
        /// </summary>
        /// <param name="datum"></param>
        void OutputList(DateTime datum);
    }
}
