using System;
using System.Collections.Generic;
using System.Text;
using Tourplaner.Infrastructure;
using Tourplaner.Models;

namespace Tourplaner.ViewModels
{
    public sealed class TourLogViewModel : ValidatedViewModel<TourLog>
    {
        public DateTime TourDate
        {
            get
            {
                return Model.TourDate;
            }
            set
            {
                if (Model.TourDate != value)
                {
                    Model.TourDate = value;
                    NotifyPropertyChanged(nameof(TourDate));
                }
            }
        }

        public double Distance
        {
            get
            {
                return Model.Distance;
            }
            set
            {
                if (Model.Distance != value)
                {
                    Model.Distance = value;
                    NotifyPropertyChanged(nameof(Distance));
                }
            }
        }

        public double AvgSpeed
        {
            get
            {
                return Model.AvgSpeed;
            }
            set
            {
                if (Model.AvgSpeed != value)
                {
                    Model.AvgSpeed = value;
                    NotifyPropertyChanged(nameof(AvgSpeed));
                }
            }
        }

        public int Breaks
        {
            get
            {
                return Model.Breaks;
            }
            set
            {
                if (Model.Breaks != value)
                {
                    Model.Breaks = value;
                    NotifyPropertyChanged(nameof(Breaks));
                }
            }
        }

        public int Brawls
        {
            get
            {
                return Model.Brawls;
            }
            set
            {
                if (Model.Brawls != value)
                {
                    Model.Brawls = value;
                    NotifyPropertyChanged(nameof(Brawls));
                }
            }
        }

        public int Abductions
        {
            get
            {
                return Model.Abductions;
            }
            set
            {
                if (Model.Abductions != value)
                {
                    Model.Abductions = value;
                    NotifyPropertyChanged(nameof(Abductions));
                }
            }
        }

        public int HobgoblinSightings
        {
            get
            {
                return Model.HobgoblinSightings;
            }
            set
            {
                if (Model.HobgoblinSightings != value)
                {
                    Model.HobgoblinSightings = value;
                    NotifyPropertyChanged(nameof(HobgoblinSightings));
                }
            }
        }

        public int UFOSightings
        {
            get
            {
                return Model.UFOSightings;
            }
            set
            {
                if (Model.UFOSightings != value)
                {
                    Model.UFOSightings = value;
                    NotifyPropertyChanged(nameof(UFOSightings));
                }
            }
        }
        public double TotalTime
        {
            get
            {
                return Model.TotalTime;
            }
            set
            {
                if (Model.TotalTime != value)
                {
                    Model.TotalTime = value;
                    NotifyPropertyChanged(nameof(TotalTime));
                }
            }
        }

        public int Rating
        {
            get
            {
                return Model.Rating;
            }
            set
            {
                if (Model.Rating != value)
                {
                    Model.Rating = value;
                    NotifyPropertyChanged(nameof(Rating));
                }
            }
        }

        public TourLogViewModel()
        {
            this.Model = new TourLog();
        }
    }
}
