namespace TechwuliArsenal.GoogleMapsWebServices.Models
{
    public struct Coordinate
    {
        public double Lat;

        public double Lon;

        public string Textual
        {
            get { return Lat + "," + Lon; }
        }
    }
}
