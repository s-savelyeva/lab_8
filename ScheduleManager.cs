using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace lab_8
{
    /// <summary>
    /// Класс для выполнения запросов к базе данных
    /// </summary>
    public class ScheduleManager
    {
        private List<BusSchedule> schedules;
        private const string fileName = "schedule.bin";
    
        public ScheduleManager()
        {
            schedules = loadSchedule();
        }
        
        /// <summary>
        /// Загрузка всех расписаний из бинарного файла
        /// </summary>
        /// <returns>Список всех расписаний, содержащихся в файле</returns>
        public List<BusSchedule> loadSchedule()
        {
            if (!File.Exists(fileName))
                return new List<BusSchedule>();
            FileStream fs = null;
            BinaryReader reader = null;
            List<BusSchedule> tasks = new List<BusSchedule>();
            try
            {
                fs = new FileStream(fileName, FileMode.Open);
                reader = new BinaryReader(fs, Encoding.UTF8);

                while (fs.Position < fs.Length)
                {
                    var id = reader.ReadInt32();
                    var busNumber = reader.ReadString();
                    var destination = reader.ReadString();
                    var departureTime = DateTime.FromBinary(reader.ReadInt64());
                    var duration = reader.ReadDouble();
                    
                    var schedule = new BusSchedule(id, busNumber, destination, departureTime, duration);
                    tasks.Add(schedule);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении задач: {ex.Message}");
            }
            if (reader != null)
            {
                reader.Close();
            }
            if (fs != null)
            {
                fs.Close();
            }
            return tasks;
        }
        
        /// <summary>
        /// Выводит все расписания на экран
        /// </summary>
        public void viewSchedules()
        {
            if (schedules.Count == 0)
            {
                Console.WriteLine("Расписания нет");
            }
            else
            {
                Console.WriteLine("id | busNumber | destination | departureTime | duration");
                foreach (var schedule in schedules)
                {
                    Console.WriteLine(schedule);
                } 
            }
            
        }
    
        /// <summary>
        /// Удаляет расписание по полученному id
        /// </summary>
        /// <param name="id">id для удаления</param>
        public void deleteSchedule(int id)
        {
            var schedule = getScheduleById(id);
            if (schedule != null)
            {
                schedules.Remove(schedule);
                saveSchedule();
                Console.WriteLine("Расписание удалено.");
            }
            else
            {
                Console.WriteLine("Расписание не найдено.");
            }
        }
    
        /// <summary>
        /// Добавляет расписание в бинарный файл
        /// </summary>
        /// <param name="newSchedule">Расписание для добавления</param>
        public void addSchedule(BusSchedule newSchedule)
        {
            schedules.Add(newSchedule);
            saveSchedule();
        }
    
        /// <summary>
        /// Метод для записи расписаний в бинарный файл
        /// </summary>
        private void saveSchedule()
        {
            FileStream fs = null;
            BinaryWriter writer = null;

            try
            {
                
                fs = new FileStream(fileName, FileMode.Create);
                writer = new BinaryWriter(fs, Encoding.UTF8);

                foreach (var schedule in schedules)
                {
                    writer.Write(schedule.Id);
                    writer.Write(schedule.BusNumber);
                    writer.Write(schedule.Destination);
                    writer.Write(schedule.DepartureTime.ToBinary());
                    writer.Write(schedule.Duration);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при записи задач: {ex.Message}");
            }
            if (writer != null)
            {
                writer.Close();
            }
            if (fs != null)
            {
                fs.Close();
            }
        }
    
        /// <summary>
        /// Получение всех расписаний, у которых конечная остановка совпадает с заданнной
        /// </summary>
        /// <param name="destination">Конечная остановка</param>
        /// <returns>Список всех расписаний, у которых конечная остановка совпадает с заданной</returns>
        public List<BusSchedule> getSchedulesByDestination(string destination)
        {
             List<BusSchedule> resultSchedules = (from schedule in schedules
                            where schedule.Destination.Equals((destination))
                            orderby schedule
                            select schedule).ToList();
            return resultSchedules;
        }
    
        /// <summary>
        /// Получение расписания по его id
        /// </summary>
        /// <param name="id">id расписания, которое надо получить</param>
        /// <returns>Расписание по его id</returns>
        public BusSchedule getScheduleById(int id)
        {
            BusSchedule resultSchedule = (from schedule in schedules
                                        where schedule.Id == id
                                        select schedule).FirstOrDefault();
            return resultSchedule;
        }
        
        /// <summary>
        /// Получение рейсов, которые отправляются после заданного времени
        /// </summary>
        /// <param name="time">Время отправления</param>
        /// <returns>Список расписаний, которые отправляются после заданного времени</returns>
        public List<BusSchedule> getSchedulesDepartingAfter(DateTime time)
        {
             List<BusSchedule> resultSchedules = (from schedule in schedules
                            where schedule.DepartureTime >= time
                            orderby schedule
                            select schedule).ToList();
            return resultSchedules;
        }
        
        /// <summary>
        /// Получение количества рейсов, у которых конечная остановка совпадает с заданной
        /// </summary>
        /// <param name="destination">Конечная остановка</param>
        /// <returns>Количество рейсов, у которых конечная остановка совпадает с заданной</returns>
        public int getScheduleCountByDestination(string destination)
        {
            int schedulesCount = (from schedule in schedules
                                where schedule.Destination.Equals((destination))
                                select schedule).Count();
            return schedulesCount;
        }
    }
}