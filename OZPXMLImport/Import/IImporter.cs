using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZPXMLImport.Import
{
    public interface IImporter
    {
        /// <summary>
        /// imports data from source
        /// </summary>
        /// <param name="path">path to source</param>
        void Import(string path);
    }
}
