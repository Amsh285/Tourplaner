using System;
using Tourplaner.Infrastructure.Data;

namespace Tourplaner.Models
{
    public sealed class TourLog : IIdentity<int>
    {
        public int ID { get; set; }

        public DateTime TourDate { get; set; }

        public double Distance { get; set; }

        public double AvgSpeed { get; set; }

        public int Breaks { get; set; }

        public int Brawls { get; set; }

        public int Abductions { get; set; }

        public int HobgoblinSightings { get; set; }

        public int UFOSightings { get; set; }

        public double TotalTime { get; set; }

        public int Rating { get; set; }

        public TourLog()
        {
            TourDate = DateTime.Now;
        }

        public TourLog Copy()
        {
            return new TourLog()
            {
                ID = ID,
                TourDate = TourDate,
                Distance = Distance,
                AvgSpeed = AvgSpeed,
                Breaks = Breaks,
                Brawls = Brawls,
                Abductions = Abductions,
                HobgoblinSightings = HobgoblinSightings,
                UFOSightings = UFOSightings,
                TotalTime = TotalTime,
                Rating = Rating
            };
        }
    }
}
