using System;
using System.Net;
using System.Xml.Linq;
using TechwuliArsenal.GoogleMapsWebServices.Exceptions;
using TechwuliArsenal.GoogleMapsWebServices.Models;

namespace TechwuliArsenal.GoogleMapsWebServices
{
    public class Directions
    {
        public static float GetDistance(Coordinate orgin, Coordinate destination)
        {
            try
            {
                var url = "http://maps.googleapis.com/maps/api/directions/xml?origin=" + orgin.Textual
                          + "&destination=" + destination.Textual + "&sensor=false";
                var webclient = new WebClient();
                var resultString = webclient.DownloadString(url);
                var xElement = XElement.Parse(resultString);

                var routeElement = xElement.Element("route");
                if (routeElement != null)
                {
                    var legElement = routeElement.Element("leg");
                    if (legElement != null)
                    {
                        var distanceElement = legElement.Element("distance");
                        if (distanceElement != null)
                        {
                            var valueElement = distanceElement.Element("value");
                            if (valueElement != null) return float.Parse(valueElement.Value);
                        }
                    }
                }

                throw new DirectionsException("Unable to parse returned data.");
            }
            catch (Exception exception)
            {
                throw new DirectionsException(exception.Message);
            }
        }
    }
}