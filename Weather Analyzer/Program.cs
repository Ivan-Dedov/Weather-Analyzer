using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WeatherAnalyzer
{

    /// <summary>
    /// The type of the WeatherEvent.
    /// </summary>
    public enum WeatherEventType
    {
        Snow,
        Fog,
        Cold,
        Storm,
        Rain,
        Precipitation,
        Hail,
    }

    /// <summary>
    /// The severity of the WeatherEvent.
    /// </summary>
    public enum Severity
    {
        Light,
        Severe,
        Moderate,
        Heavy,
        UNK,
        Other,
    }

    class Program
    {

        // https://www.kaggle.com/sobhanmoosavi/us-weather-events

        // Написать Linq-запросы, используя синтаксис методов расширений
        // и продублировать его, используя синтаксис запросов
        // (возможно с вкраплениями методов расширений, ибо иногда первого может быть недостаточно)

        // 0. Linq - сколько различных городов есть в датасете.
        // 1. Сколько записей за каждый из годов имеется в датасете.
        // Потом будут еще запросы

        /// <summary>
        /// The path to the data file.
        /// </summary>
        public static readonly string Path = @"..\..\..\..\..\WeatherEvents_Jan2016-Dec2020.csv";
        /// <summary>
        /// The string separator used in the file.
        /// </summary>
        public static readonly string CommaSeparatorValue = ",";

        /// <summary>
        /// The list of WeatherEvents.
        /// </summary>
        private static List<WeatherEvent> listOfWeatherEvents = new List<WeatherEvent>();

        static void Main(string[] args)
        {
            ExtractWeatherEventsFromFile(Path);
        }

        /// <summary>
        /// Gets all possible unique values for the weather type of the WeatherEvent and prints them to Console.
        /// </summary>
        private static void WriteUniqueWeatherTypesToConsole()
        {
            // Comment out all the extra lines beforehand and set the
            // type of the autoproperty to string.
            IEnumerable<WeatherEvent> uniqueWeatherTypes =
                listOfWeatherEvents.GroupBy(x => x.Type).Select(x => x.First());
            foreach (WeatherEvent weatherEvent in uniqueWeatherTypes)
            {
                Console.WriteLine(weatherEvent.Type.ToString());
            }
        }

        /// <summary>
        /// Gets all possible unique values for the severity of the WeatherEvent and prints them to Console.
        /// </summary>
        private static void WriteUniqueWeatherSeveritiesToConsole()
        {
            // Comment out all the extra lines beforehand and set the
            // type of the autoproperty to string.
            IEnumerable<WeatherEvent> uniqueWeatherSeverities =
                listOfWeatherEvents.GroupBy(x => x.Severity).Select(x => x.First());
            foreach (WeatherEvent weatherEvent in uniqueWeatherSeverities)
            {
                Console.WriteLine(weatherEvent.Severity.ToString());
            }
        }

        /// <summary>
        /// Reads the file and converts the data to a list.
        /// </summary>
        private static void ExtractWeatherEventsFromFile(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    try
                    {
                        string[] line = sr.ReadLine().Split(CommaSeparatorValue);
                        listOfWeatherEvents.Add(new WeatherEvent()
                        {
                            EventID = line[0],
                            Type = Enum.Parse<WeatherEventType>(line[1]),
                            Severity = Enum.Parse<Severity>(line[2]),
                            StartTime = DateTime.Parse(line[3]),
                            EndTime = DateTime.Parse(line[4]),
                            TimeZone = line[5],
                            AirportCode = line[6],
                            LocationLatitude = double.Parse(line[7], new CultureInfo("en-US")),
                            LocationLongitude = double.Parse(line[8], new CultureInfo("en-US")),
                            City = line[9],
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

    }
}
