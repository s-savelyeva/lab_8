using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace lab_8
{

    public class ScheduleManager
    {
        private List<BusSchedule> schedules;
        private const string fileName = "schedule.bin";
    
        public ScheduleManager()
        {
            schedules = LoadSchedule();
        }
    
        public List<BusSchedule> LoadSchedule()
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
    
        public void ViewSchedules()
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
    
        public void DeleteSchedule(int id)
        {
            var schedule = schedules.FirstOrDefault(s => s.Id == id);
            if (schedule != null)
            {
                schedules.Remove(schedule);
                SaveSchedule();
                Console.WriteLine("Расписание удалено.");
            }
            else
            {
                Console.WriteLine("Расписание не найдено.");
            }
        }
    
        public void AddSchedule(BusSchedule newSchedule)
        {
            schedules.Add(newSchedule);
            SaveSchedule();
        }
    
        private void SaveSchedule()
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
    
        public List<BusSchedule> GetSchedulesByDestination(string destination)
        {
            return schedules.Where(s => s.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    
        public BusSchedule GetScheduleById(int id)
        {
            return schedules.FirstOrDefault(s => s.Id == id);
        }
        
        public List<BusSchedule> GetSchedulesDepartingAfter(DateTime time)
        {
            return schedules.Where(s => s.DepartureTime >= time).ToList();
        }
        
        public int GetScheduleCountByDestination(string destination)
        {
            return schedules.Count(s => s.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase));
        }
    }
}