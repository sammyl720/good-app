using System;

namespace TechwuliArsenal.GoogleMapsWebServices.Exceptions
{
    public class GeocodingException : Exception
    {
        public GeocodingException(string message) : base(message)
        {
        }
    }
}