using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace lab_8
{
    public class BusSchedule
    {
        public int Id { get; set; }
        public string BusNumber { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public double Duration { get; set; } // в минутах

        public BusSchedule(int id, string busNumber, string destination, DateTime departureTime, double duration)
        {
            this.Id = id;
            this.BusNumber = busNumber;
            this.Destination = destination;
            this.DepartureTime = departureTime;
            this.Duration = duration;
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
                bool success;
                Console.WriteLine("\n1. Посмотреть расписание");
                Console.WriteLine("2. Добавить расписание");
                Console.WriteLine("3. Удалить расписание");
                Console.WriteLine("4. Показать расписание по месту назначения");
                Console.WriteLine("5. Получить расписание по ID");
                Console.WriteLine("6. Получить рейсы, отправляющиеся после указанного времени");
                Console.WriteLine("7. Получить общее количество рейсов в пункт назначения");
                Console.WriteLine("8. Выйти");
                Console.Write("Выберите пункт: ");

                var choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        manager.ViewSchedules();
                        break;

                    case "2":
                        success = false;
                        
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
                        success = false;
                        int idToDelete = 1;
                        while (!success)
                        {
                            Console.Write("Введите ID расписания для удаления: ");
                            if (Int32.TryParse(Console.ReadLine(), out idToDelete))
                            {
                                if (idToDelete > 0)
                                {
                                    success = true;
                                }
                                else
                                {
                                    Console.WriteLine("Неправильный ID. Введите заново");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Неправильный ID. Введите заново");
                            }
                        }
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
                        success = false;
                        int idToGet = 1;
                        while (!success)
                        {
                            Console.Write("Введите ID расписания: ");
                            if (Int32.TryParse(Console.ReadLine(), out idToGet))
                            {
                                if (idToGet > 0)
                                {
                                    success = true;
                                }
                                else
                                {
                                    Console.WriteLine("Неправильный ID. Введите заново");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Неправильный ID. Введите заново");
                            }
                        }
                        
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
                        DateTime departureTimeAfter = new DateTime();
                        success = false;
                        while (!success)
                        {
                            Console.Write("Введите время (yyyy-mm-dd hh:mm) для получения рейсов: ");
                            if (!DateTime.TryParse(Console.ReadLine(), out departureTimeAfter))
                            {
                                Console.WriteLine("Неправильный формат даты. Введите заново");
                            }
                            else
                            {
                                success = true;
                            }
                        }
                        var departingSchedules = manager.GetSchedulesDepartingAfter(departureTimeAfter);
                        if (departingSchedules.Count > 0)
                        {
                            Console.WriteLine("Рейсы, отправляющиеся после указанного времени:");
                            foreach (var schedule in departingSchedules)
                            {
                                Console.WriteLine(schedule);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Рейсы не найдены.");
                        }
                        break;
                    
                    case "7":
                        Console.Write("Введите пункт назначения для подсчета рейсов: ");
                        string countDestination = Console.ReadLine();
                        int totalCount = manager.GetScheduleCountByDestination(countDestination);
                        Console.WriteLine($"Общее количество рейсов в '{countDestination}': {totalCount}");
                        break;
                    
                    case "8":
                        return;

                    default:
                        Console.WriteLine("Неверный выбор. Введите снова");
                        break;
                }
            }
        }
    }
}
