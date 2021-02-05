using System;

namespace WeatherAnalyzer
{
    public class WeatherEvent
    {
        public string EventId { get; set; }
        public WeatherEventType Type { get; set; }
        public Severity Severity { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeZone TimeZone { get; set; }
        public string AirportCode { get; set; }
        public double LocationLatitude { get; set; }
        public double LocationLongitude { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }

    }
}
