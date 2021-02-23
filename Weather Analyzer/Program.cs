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

        // -1. Вывести количество зафиксированных природных явлений в Америке в 2018 году
        // 0. Вывести количество штатов, количество городов в датасете
        // 1. Вывести топ 3 самых дождливых города в 2019 году в порядке убывания количества дождей
        // (вывести город и количество дождей)
        // 2. Вывести данные самых долгих(топ-1) снегопадов в Америке по годам(за каждый из годов) -
        // с какого времени, по какое время, в каком городе
        //(Для простоты и красивости предлагается использовать анонимные объекты)

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
            try
            {
                ExtractWeatherEventsFromFile(Path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }


            // Entries in year.
            Console.WriteLine("----- -2 -----");
            Console.WriteLine(string.Join(Environment.NewLine,
                             listOfWeatherEvents.GroupBy(x => x.StartTime.Year)
                                                .Select(x => x.Key + " - " + x.Count())
                                                .ToList()));
            Console.WriteLine("===<>====<>===");
            var eventsGroupedByYear = from item in listOfWeatherEvents
                                      group item by item.StartTime.Year;
            var entriesInYear = from item in eventsGroupedByYear
                                select item.Key + " - " + item.Count();
            Console.WriteLine(string.Join(Environment.NewLine, entriesInYear));


            // Number of weather events in the US in 2018.
            Console.WriteLine(Environment.NewLine + "----- -1 -----");
            Console.WriteLine($"Number of weather events in 2018: {listOfWeatherEvents.Where(x => x.StartTime.Year == 2018).Count()}");
            Console.WriteLine("===<>====<>===");
            var entriesIn2018 = from item in listOfWeatherEvents
                                where item.StartTime.Year == 2018
                                select item;
            Console.WriteLine($"Number of weather events in 2018: {entriesIn2018.Count()}");


            // Number of unique states and cities in the entire dataset.
            Console.WriteLine(Environment.NewLine + "----- 00 -----");
            Console.WriteLine($"Number of unique states: {listOfWeatherEvents.GroupBy(x => x.State).Count()}");
            Console.WriteLine($"Number of unique cities: {listOfWeatherEvents.GroupBy(x => x.City).Count()}");
            Console.WriteLine("===<>====<>===");
            var uniqueStates = from item in listOfWeatherEvents
                               group item by item.State;
            var uniqueCities = from item in listOfWeatherEvents
                               group item by item.City;
            Console.WriteLine($"Number of unique states: {uniqueStates.Count()}");
            Console.WriteLine($"Number of unique cities: {uniqueCities.Count()}");


            // Top-3 cities by rainfall in 2019 (in descending order).
            Console.WriteLine(Environment.NewLine + "----- 01 -----");
            Console.WriteLine("Top-3 cities by rainfall in 2019: ");
            var top3CitiesByRainfall = string.Join(Environment.NewLine,
                                       listOfWeatherEvents.Where(x => x.StartTime.Year == 2019 && x.Type == WeatherEventType.Rain)
                                                          .GroupBy(x => x.City)
                                                          .OrderByDescending(x => x.Count())
                                                          .Select(x => x.Key)
                                                          .Take(3)
                                                          .ToList());
            Console.WriteLine(top3CitiesByRainfall);
            Console.WriteLine("===<>====<>===");
            Console.WriteLine("Top-3 cities by rainfall in 2019: ");
            var rainsGroupedByCity = from item in listOfWeatherEvents
                                     where item.StartTime.Year == 2019 && item.Type == WeatherEventType.Rain
                                     group item by item.City;
            var rainCountInCity = from item in rainsGroupedByCity
                                  orderby item.Count()
                                  select item.Key;
            Console.WriteLine(string.Join(Environment.NewLine,
                                          rainCountInCity.Reverse()
                                                         .Take(3)
                                                         .ToList()));


            // Information about the top-1 snowstorm in each year (start, end times and city).
            Console.WriteLine(Environment.NewLine + "----- 02 -----");
            Console.WriteLine("Biggest snowstorm each year:");
            var topSnowstormsByYear = string.Join(Environment.NewLine,
                                      listOfWeatherEvents.Where(x => x.Type == WeatherEventType.Snow)
                                                         .OrderByDescending(x => x.EndTime - x.StartTime)
                                                         .GroupBy(x => x.StartTime.Year)
                                                         .Select(x => x.First())
                                                         .OrderBy(x => x.StartTime.Year)
                                                         .Select(x => $"{x.StartTime.Year} - {x.City} | From {x.StartTime} to {x.EndTime}"));
            Console.WriteLine(topSnowstormsByYear);
            Console.WriteLine("===<>====<>===");
            Console.WriteLine("Biggest snowstorm each year:");
            var snowsGroupedByYear = from item in listOfWeatherEvents
                                     where item.Type == WeatherEventType.Snow
                                     orderby item.EndTime - item.StartTime
                                     group item by item.StartTime.Year;
            var topSnowstormByYear = from item in snowsGroupedByYear
                                     select item.Last();
            var orderedSnowstormsByYear = from item in topSnowstormByYear
                                          orderby item.StartTime.Year
                                          select $"{item.StartTime.Year} - {item.City} | From {item.StartTime} to {item.EndTime}";
            Console.WriteLine(string.Join(Environment.NewLine, orderedSnowstormsByYear));
        }

        /// <summary>
        /// Reads the file and converts the data to a list.
        /// </summary>
        private static void ExtractWeatherEventsFromFile(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                if (!sr.EndOfStream)
                {
                    sr.ReadLine();
                }
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
                            County = line[10],
                            State = line[11],
                            ZipCode = line[12],
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return;
                    }
                }
            }
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

    }
}
