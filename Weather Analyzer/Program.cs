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
            Console.WriteLine(Environment.NewLine + "----- -2 -----");
            foreach (var group in listOfWeatherEvents.GroupBy(x => x.StartTime.Year))
            {
                Console.WriteLine(group.Key + " - " + group.Count());
            }
            Console.WriteLine("===<>====<>===");
            var grouping = from item in listOfWeatherEvents
                           group item by item.StartTime.Year;
            foreach (var group in grouping)
            {
                Console.WriteLine(group.Key + " - " + group.Count());
            }


            // Number of weather events in the US in 2018.
            Console.WriteLine(Environment.NewLine + "----- -1 -----");
            Console.WriteLine($"Number of weather events in 2018: {listOfWeatherEvents.Where(x => x.StartTime.Year == 2018).Count()}");
            Console.WriteLine("===<>====<>===");
            var answer = from item in listOfWeatherEvents
                         where item.StartTime.Year == 2018
                         select item;
            Console.WriteLine($"Number of weather events in 2018: {answer.Count()}");

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
            List<WeatherEvent> list = listOfWeatherEvents.Where(x => x.StartTime.Year == 2019 && x.Type == WeatherEventType.Rain)
                                                         .OrderByDescending(x => x.EndTime - x.StartTime)
                                                         .ToList();
            Console.WriteLine("Top-3 cities by rainfall in 2019: ");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"{i + 1} - {list[i].City}");
            }
            Console.WriteLine("===<>====<>===");
            var ranking = from item in listOfWeatherEvents
                       where item.StartTime.Year == 2019 && item.Type == WeatherEventType.Rain
                       orderby item.EndTime - item.StartTime
                       select item;
            // Тут появляется IOrderedEnumerable, у которой нет индексатора, так что я хз как к ней обратиться надо было :(
            // поэтому пришлось сделать ToList()
            var top3 = ranking.ToList();
            Console.WriteLine("Top-3 cities by rainfall in 2019: ");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"{i + 1} - {top3[top3.Count() - (i + 1)].City}");
            }

            // Information about the top-1 snowstorm in each year (start, end times and city).
            Console.WriteLine(Environment.NewLine + "----- 02 -----");
            foreach (var group in listOfWeatherEvents.GroupBy(x => x.StartTime.Year))
            {
                WeatherEvent e = group.Where(x => x.Type == WeatherEventType.Snow).OrderBy(x => x.EndTime - x.StartTime).Last();
                Console.WriteLine($"{group.Key} - City: {e.City} | From {e.StartTime} to {e.EndTime}");
            }
            Console.WriteLine("===<>====<>===");
            var groups = from item in listOfWeatherEvents
                         group item by item.StartTime.Year;
            foreach(var groupItem in groups)
            {
                var snowstorms = from item in groupItem
                                        where item.Type == WeatherEventType.Snow
                                        orderby item.EndTime - item.StartTime
                                        select item;
                // Тут появляется IOrderedEnumerable, у которой нет индексатора, так что я хз как к ней обратиться надо было :(
                // поэтому сделал ToList()
                var e = snowstorms.ToList()[^1];
                Console.WriteLine($"{groupItem.Key} - City: {e.City} | From {e.StartTime} to {e.EndTime}");
            }
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
