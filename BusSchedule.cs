namespace lab_8;

/// <summary>
/// Класс для расписания автобусов
/// busNumber - номер автобуса
/// destination - конечная остановка рейса
/// departureTime - время отправления
/// duration - время в пути
/// </summary>
public class BusSchedule
{
    private int _id;
    private string _busNumber;
    private string _destination;
    private DateTime _departureTime;
    private double _duration;
    
    public int Id { get => _id; set => _id = value; }
    public string BusNumber { get => _busNumber; set => _busNumber = value; }
    public string Destination { get => _destination; set => _destination = value; }
    public DateTime DepartureTime { get => _departureTime; set => _departureTime = value; }
    public double Duration { get => _duration; set => _duration = value; }

    public BusSchedule(int id, string busNumber, string destination, DateTime departureTime, double duration)
    {
        this._id = id;
        this._busNumber = busNumber;
        this._destination = destination;
        this._departureTime = departureTime;
        this._duration = duration;
    }

    public override string ToString()
    {
        return $"{_id} | {_busNumber} | {_destination} | {_departureTime} | {_duration} mins";
    }
}