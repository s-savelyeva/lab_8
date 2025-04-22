using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace lab_8
{
    [Serializable]
    public class BusSchedule
    {
        public int Id { get; set; }
        public string BusNumber { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public double Duration { get; set; } // в минутах

        public BusSchedule()
        {
        }

        public BusSchedule(int id, string busNumber, string destination, DateTime departureTime, int duration)
        {
            Id = id;
            BusNumber = busNumber;
            Destination = destination;
            DepartureTime = departureTime;
            Duration = duration;
        }

        public override string ToString()
        {
            return $"{Id} | {BusNumber} | {Destination} | {DepartureTime} | {Duration} mins";
        }
    }

    class Program
    {
        static void Main()
        {
            ScheduleManager manager = new ScheduleManager();
            while (true)
            {
                Console.WriteLine("\n1. Посмотреть расписание");
                Console.WriteLine("2. Добавить расписание");
                Console.WriteLine("3. Удалить расписание");
                Console.WriteLine("4. Показать расписание по месту назначения");
                Console.WriteLine("5. Получить расписание по ID");
                Console.WriteLine("6. Выйти");
                Console.Write("Выберите пункт: ");

                var choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        manager.ViewSchedules();
                        break;

                    case "2":
                        bool success = false;
                        
                        Console.Write("Введите номер автобуса: ");
                        string busNumber = Console.ReadLine();
                        Console.Write("Введите конечную остановку: ");
                        string destination = Console.ReadLine();
                        DateTime departureTime = new DateTime();
                        int duration = 0;
                        while (!success)
                        {
                            Console.Write("Введите время отправления (yyyy-mm-dd hh:mm): ");
                            if (!DateTime.TryParse(Console.ReadLine(), out departureTime))
                            {
                                Console.WriteLine("Неправильный формат даты. Введите заново");
                            }
                            else
                            {
                                success = true;
                            }
                        }
                        success = false;
                        while (!success)
                        {
                            Console.Write("Введите длительность маршрута (мин): ");
                            if (Int32.TryParse(Console.ReadLine(), out duration))
                            {
                                if (duration > 0)
                                {
                                    success = true;
                                }
                                else
                                {
                                    Console.WriteLine("Неправильная длительность. Введите заново");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Неправильная длительность. Введите заново");
                            }
                        }

                        try
                        {
                            var newSchedule = new BusSchedule(manager.LoadSchedule().Count + 1, busNumber, destination,
                                departureTime, duration);

                            manager.AddSchedule(newSchedule);
                            Console.WriteLine("Расписание добавлено.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;


                    case "3":
                        Console.Write("Введите ID расписания для удаления: ");
                        int idToDelete = int.Parse(Console.ReadLine());
                        manager.DeleteSchedule(idToDelete);
                        break;

                    case "4":
                        Console.Write("Введите конечную остановку: ");
                        string queryDestination = Console.ReadLine();
                        var results = manager.GetSchedulesByDestination(queryDestination);
                        if (results.Count() != 0)
                        {
                            foreach (var item in results)
                            {
                                Console.WriteLine(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Расписание не найдено.");
                        }

                        break;

                    case "5":
                        Console.Write("Введите ID расписания: ");
                        int idToGet = int.Parse(Console.ReadLine());
                        var result = manager.GetScheduleById(idToGet);
                        if (result != null)
                        {
                            Console.WriteLine(result);
                        }
                        else
                        {
                            Console.WriteLine("Расписание не найдено.");
                        }

                        break;

                    case "6":
                        return;

                    default:
                        Console.WriteLine("Неверный выбор. Введите снова");
                        break;
                }
            }
        }
    }
}
