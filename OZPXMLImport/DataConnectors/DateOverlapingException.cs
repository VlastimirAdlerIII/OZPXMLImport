using System;

namespace OZPXMLImport.DataConnectors
{
    [Serializable]
    public class DateOverlapingException : Exception
    {
        public DateOverlapingException()
        { }

        public DateOverlapingException(string message)
            : base(message)
        { }

        public DateOverlapingException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
