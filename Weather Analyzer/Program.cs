using System;
using System.IO;
using System.Collections.Generic;

namespace WeatherAnalyzer
{

    public enum WeatherEventType
    {

    }

    public enum Severity
    {

    }

    public enum TimeZone
    {

    }

    class Program
    {

        // Нужно дополнить модель WeatherEvent, создать список этого типа List<>
        // И заполнить его, читая файл с данными построчно через StreamReader
        // Ссылка на файл https://www.kaggle.com/sobhanmoosavi/us-weather-events

        // Написать Linq-запросы, используя синтаксис методов расширений
        // и продублировать его, используя синтаксис запросов
        // (возможно с вкраплениями методов расширений, ибо иногда первого может быть недостаточно)

        // 0. Linq - сколько различных городов есть в датасете.
        // 1. Сколько записей за каждый из годов имеется в датасете.
        // Потом будут еще запросы

        public static readonly string Path = @"..\..\..\WeatherEvents_Jan2016-Dec2020.csv";
        public static readonly string Separator = ",";

        private static List<WeatherEvent> listOfWeatherEvents = new List<WeatherEvent>();

        static void Main(string[] args)
        {

            // Reading from file.
            using (StreamReader sr = new StreamReader(Path))
            {
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(Separator);
                    for (int i = 0; i < line.Length; i++)
                    {
                        // READ FROM FILE, PARSE AND PUT ITEMS TO THE LIST.
                    }
                }
            }

            /*WeatherEvent we = new WeatherEvent()
            {
                EventId = "W-1",
                Type = WeatherEventType.Rain,
                Severity = Severity.Light,
                StartTime = DateTime.Now
            };*/


            // ANALYZE ITEMS USING LINQ
        }
    }
}
