using OZPXMLImport.DataModelSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZPXMLImport.DataConnectors
{
    public class DBContextSingleton

    {
        private static OZPXMLImportEntities _instance;
        private static readonly object syncLock = new object();

        // Constructor (protected)
        protected DBContextSingleton()
        {
        }

        public static OZPXMLImportEntities GetDBContext()
        {
            if (_instance == null)
            {
                lock (syncLock)
                {
                    if (_instance == null)
                    {
                        _instance = new OZPXMLImportEntities();
                    }
                }
            }

            return _instance;
        }
    }
}
