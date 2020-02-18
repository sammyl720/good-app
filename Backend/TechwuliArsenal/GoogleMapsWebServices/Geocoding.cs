using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using TechwuliArsenal.GoogleMapsWebServices.Exceptions;
using TechwuliArsenal.GoogleMapsWebServices.Models;

namespace TechwuliArsenal.GoogleMapsWebServices
{

    public class Geocoding
    {
        public static Location Request(string address)
        {
            var location = new Location
            {
                Address = string.Empty,
                City = string.Empty,
                State = string.Empty,
                ZipCode = string.Empty
            };
            try
            {
                var encodedAddress = HttpContext.Current != null
                    ? HttpContext.Current.Server.UrlEncode(address)
                    : Uri.EscapeUriString(address);
                var webClient = new WebClient {Encoding = Encoding.UTF8};
                var url = "http://maps.googleapis.com/maps/api/geocode/xml?address=" + encodedAddress
                          + "&sensor=false";
                var resultString = webClient.DownloadString(url);
                var reader = new StringReader(resultString);
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(reader);
                XmlNode statusNode = xmlDoc.SelectSingleNode(@"/GeocodeResponse/status");
                if (statusNode == null || statusNode.InnerText != "OK")
                {
                    throw new GeocodingException("Unable to parse returned data.");
                }

                XmlNode latNode = xmlDoc.SelectSingleNode(@"/GeocodeResponse/result/geometry/location/lat");
                if (latNode != null)
                {
                    location.Lat = float.Parse(latNode.InnerText, CultureInfo.InvariantCulture);
                }

                XmlNode lonNode = xmlDoc.SelectSingleNode(@"/GeocodeResponse/result/geometry/location/lng");
                if (lonNode != null)
                {
                    location.Lon = float.Parse(lonNode.InnerText, CultureInfo.InvariantCulture);
                }

                XmlNode addressNode = xmlDoc.SelectSingleNode(@"/GeocodeResponse/result/formatted_address");
                location.Address = addressNode != null ? addressNode.InnerText : "Unknown";

                XmlNode zipNode =
                    xmlDoc.SelectSingleNode(@"/GeocodeResponse/result/address_component[type='postal_code']/long_name");
                location.ZipCode = zipNode != null ? zipNode.InnerText : "Unknown";

                XmlNode cityNode =
                    xmlDoc.SelectSingleNode(@"/GeocodeResponse/result/address_component[type='locality']/long_name");
                location.City = cityNode != null ? cityNode.InnerText : "Unknown";

                XmlNode stateNode =
                    xmlDoc.SelectSingleNode(
                        @"/GeocodeResponse/result/address_component[type='administrative_area_level_1']/short_name");
                location.State = stateNode != null ? stateNode.InnerText : "Unknown";
            }
            catch (Exception exception)
            {
                throw new GeocodingException(exception.Message);
            }

            return location;
        }

        public static async Task<Location> RequestAsync(string address)
        {
            var location = new Location
            {
                Address = string.Empty,
                City = string.Empty,
                State = string.Empty,
                ZipCode = string.Empty
            };
            try
            {
                var encodedAddress = HttpContext.Current.Server.UrlEncode(address);
                var webClient = new WebClient {Encoding = Encoding.UTF8};
                var url = "http://maps.googleapis.com/maps/api/geocode/xml?address=" + encodedAddress
                          + "&sensor=false";
                var resultString = await webClient.DownloadStringTaskAsync(url);
                var reader = new StringReader(resultString);
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(reader);
                var statusNode = xmlDoc.SelectSingleNode(@"/GeocodeResponse/status");
                if (statusNode == null || statusNode.InnerText != "OK")
                {
                    throw new GeocodingException("Unable to parse returned data.");
                }

                var latNode = xmlDoc.SelectSingleNode(@"/GeocodeResponse/result/geometry/location/lat");
                if (latNode != null)
                {
                    location.Lat = float.Parse(latNode.InnerText, CultureInfo.InvariantCulture);
                }

                var lonNode = xmlDoc.SelectSingleNode(@"/GeocodeResponse/result/geometry/location/lng");
                if (lonNode != null)
                {
                    location.Lon = float.Parse(lonNode.InnerText, CultureInfo.InvariantCulture);
                }

                var addressNode = xmlDoc.SelectSingleNode(@"/GeocodeResponse/result/formatted_address");
                location.Address = addressNode != null ? addressNode.InnerText : "Unknown";

                var zipNode =
                    xmlDoc.SelectSingleNode(@"/GeocodeResponse/result/address_component[type='postal_code']/long_name");
                location.ZipCode = zipNode != null ? zipNode.InnerText : "Unknown";

                var cityNode =
                    xmlDoc.SelectSingleNode(@"/GeocodeResponse/result/address_component[type='locality']/long_name");
                location.City = cityNode != null ? cityNode.InnerText : "Unknown";

                var stateNode =
                    xmlDoc.SelectSingleNode(
                        @"/GeocodeResponse/result/address_component[type='administrative_area_level_1']/short_name");
                location.State = stateNode != null ? stateNode.InnerText : "Unknown";
            }
            catch (Exception exception)
            {
                throw new GeocodingException(exception.Message);
            }

            return location;
        }

    }
}