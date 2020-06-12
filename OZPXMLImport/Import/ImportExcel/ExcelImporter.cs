using OZPXMLImport.DataModelSQL;
using OZPXMLImport.Import;
using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace OZPXMLImport.ImportExcel
{
    /// <summary>
    /// imports data from Excel
    /// </summary>
    public class ExcelImporter : IImporter
    {
        private IDataConnector _dataConnector;

        public ExcelImporter(IDataConnector dataConnector)
        {
            _dataConnector = dataConnector;
        }

        public void Import(string excelPath)
        {
            string conString = string.Empty;
            string extension = Path.GetExtension(excelPath);
            switch (extension)
            {
                case ".xls": //Excel 97-03
                    conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx": //Excel 07 or higher
                    conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                    break;

            }
            conString = string.Format(conString, excelPath);
            using (OleDbConnection excel_con = new OleDbConnection(conString))
            {
                excel_con.Open();

                ImportTypPZS(excel_con);
                ImportPoskytovatelZdravotnichSluzeb(excel_con);
                ImportPojistovna(excel_con);
                ImportTypSmlouvy(excel_con);
                ImportSmlouva(excel_con);

                excel_con.Close();
            }
        }

        /// <summary>
        /// import from sheet TypPZS
        /// </summary>
        /// <param name="excel_con"></param>
        private void ImportTypPZS(OleDbConnection excel_con)
        {
            DataTable dtExcelData = new DataTable();

            //[OPTIONAL]: It is recommended as otherwise the data will be considered as String by default.
            dtExcelData.Columns.AddRange(new DataColumn[5] {
                    new DataColumn("Id", typeof(int)),
                    new DataColumn("Kod", typeof(string)),
                    new DataColumn("Nazev",typeof(string)),
                    new DataColumn("DatumOd", typeof(DateTime)),
                    new DataColumn("DatumDo",typeof(DateTime))
                });

            using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [TypPZS$]", excel_con))
            {
                oda.Fill(dtExcelData);

                foreach (DataRow row in dtExcelData.Rows)
                {
                    _dataConnector.CreateTypPZS((string)row["Kod"], (string)row["Nazev"], (DateTime)row["DatumOd"], row.IsNull("DatumDo") ? null : (DateTime?)row["DatumDo"]);
                }
            }
        }

        /// <summary>
        /// import from sheet PoskytovatelZdravotnichSluzeb
        /// </summary>
        /// <param name="excel_con"></param>
        private void ImportPoskytovatelZdravotnichSluzeb(OleDbConnection excel_con)
        {
            DataTable dtExcelData = new DataTable();

            dtExcelData.Columns.AddRange(new DataColumn[3] {
                    new DataColumn("Id", typeof(int)),
                    new DataColumn("Nazev",typeof(string)),
                    new DataColumn("TypPZS",typeof(string))
                });

            using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [PoskytovatelZdravotnichSluzeb$]", excel_con))
            {
                oda.Fill(dtExcelData);

                foreach (DataRow row in dtExcelData.Rows)
                {
                    _dataConnector.CreatePoskytovatelZdravotnichSluzeb((string)row["Nazev"], (string)row["TypPZS"]);
                }
            }
        }

        /// <summary>
        /// import from sheet Pojistovna
        /// </summary>
        /// <param name="excel_con"></param>
        private void ImportPojistovna(OleDbConnection excel_con)
        {
            DataTable dtExcelData = new DataTable();

            dtExcelData.Columns.AddRange(new DataColumn[3] {
                    new DataColumn("Id", typeof(int)),
                    new DataColumn("Nazev",typeof(string)),
                    new DataColumn("Zkratka",typeof(string))
                });

            using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [Pojistovna$]", excel_con))
            {
                oda.Fill(dtExcelData);

                foreach (DataRow row in dtExcelData.Rows)
                {
                    _dataConnector.CreatePojistovna((string)row["Nazev"], (string)row["Zkratka"]);
                }
            }
        }

        /// <summary>
        /// import from sheet Pojistovna
        /// </summary>
        /// <param name="excel_con"></param>
        private void ImportTypSmlouvy(OleDbConnection excel_con)
        {
            DataTable dtExcelData = new DataTable();

            dtExcelData.Columns.AddRange(new DataColumn[5] {
                    new DataColumn("Id", typeof(int)),
                    new DataColumn("Kod",typeof(string)),
                    new DataColumn("Nazev",typeof(string)),
                    new DataColumn("DatumOd", typeof(DateTime)),
                    new DataColumn("DatumDo",typeof(DateTime))
                });

            using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [TypSmlouvy$]", excel_con))
            {
                oda.Fill(dtExcelData);

                foreach (DataRow row in dtExcelData.Rows)
                {
                    _dataConnector.CreateTypSmlouvy((string)row["Kod"], (string)row["Nazev"], (DateTime)row["DatumOd"], row.IsNull("DatumDo") ? null : (DateTime?)row["DatumDo"]);
                }
            }
        }

        /// <summary>
        /// import from sheet Smlouva
        /// </summary>
        /// <param name="excel_con"></param>
        private void ImportSmlouva(OleDbConnection excel_con)
        {
            DataTable dtExcelData = new DataTable();

            dtExcelData.Columns.AddRange(new DataColumn[6] {
                    new DataColumn("Id", typeof(int)),
                    new DataColumn("PoskytovatelZdravotnichSluzebId", typeof(int)),
                    new DataColumn("PojistovnaId", typeof(int)),
                    new DataColumn("TypSmlouvy",typeof(string)),
                    new DataColumn("DatumOd", typeof(DateTime)),
                    new DataColumn("DatumDo",typeof(DateTime))
                });

            using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [Smlouva$]", excel_con))
            {
                oda.Fill(dtExcelData);

                foreach (DataRow row in dtExcelData.Rows)
                {
                    int typSmlouvyId = _dataConnector.GetTypSmlouvy((string)row["TypSmlouvy"], (DateTime)row["DatumOd"]).Id;
                    Smlouva smlouva = new Smlouva()
                    {
                        PoskytovatelZdravotnichSluzebId = (int)row["PoskytovatelZdravotnichSluzebId"],
                        PojistovnaId = (int)row["PojistovnaId"],
                        TypSmlouvyId = typSmlouvyId,
                        DatumOd = (DateTime)row["DatumOd"],
                        DatumDo = row.IsNull("DatumDo") ? null : (DateTime?)row["DatumDo"]
                    };

                    _dataConnector.CreateSmlouva(smlouva);
                }
            }
        }

    }
}
