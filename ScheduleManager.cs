using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace lab_8
{

    public class ScheduleManager
    {
        private List<BusSchedule> schedules;
        private const string fileName = "schedule.xml";
    
        public ScheduleManager()
        {
            schedules = LoadSchedule();
        }
    
        public List<BusSchedule> LoadSchedule()
        {
            if (!File.Exists(fileName))
                return new List<BusSchedule>();
    
            XmlSerializer serializer = new XmlSerializer(typeof(List<BusSchedule>));
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                return (List<BusSchedule>)serializer.Deserialize(fs);
            }
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
            XmlSerializer serializer = new XmlSerializer(typeof(List<BusSchedule>));
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(fs, schedules);
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
    }
}