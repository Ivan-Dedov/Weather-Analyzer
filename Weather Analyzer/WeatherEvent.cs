using System;

namespace WeatherAnalyzer
{

    /// <summary>
    /// Describes a single instance of a Weather Event.
    /// </summary>
    public class WeatherEvent
    {

        /// <summary>
        /// The ID of the WeatherEvent.
        /// </summary>
        public string EventID { get; set; }
        
        /// <summary>
        /// The type of the WeatherEvent.
        /// </summary>
        public WeatherEventType Type { get; set; }

        /// <summary>
        /// The severity of the WeatherEvent.
        /// </summary>
        public Severity Severity { get; set; }

        /// <summary>
        /// The start time of the WeatherEvent.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// The end time of the WeatherEvent.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// The time zone of the WeatherEvent.
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// The airport code of the WeatherEvent.
        /// </summary>
        public string AirportCode { get; set; }

        /// <summary>
        /// The latitude of the location of the WeatherEvent.
        /// </summary>
        public double LocationLatitude { get; set; }

        /// <summary>
        /// The longitude of the location of the WeatherEvent.
        /// </summary>
        public double LocationLongitude { get; set; }

        /// <summary>
        /// The city of the WeatherEvent.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The county of the WeatherEvent.
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// The state of the WeatherEvent.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The zip code of the WeatherEvent.
        /// </summary>
        public int ZipCode { get; set; }

    }

}
