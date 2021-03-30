using System;
using System.Collections.Generic;
using System.Text;

namespace Tourplaner.Models
{
    public sealed class TourLog
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
    }
}
