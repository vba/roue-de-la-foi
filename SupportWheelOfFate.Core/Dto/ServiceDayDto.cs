using System;

namespace SupportWheelOfFate.Core.Dto
{
    public sealed class ServiceDayDto
    {
        public EngineerDto ForenoonEngineer { get; }
        public EngineerDto AfternoonEngineer { get; }
        public DateTime Day { get; }

        public ServiceDayDto(EngineerDto forenoonEngineer, EngineerDto afternoonEngineer, DateTime day)
        {
            ForenoonEngineer = forenoonEngineer ?? throw new ArgumentNullException(nameof(forenoonEngineer));
            AfternoonEngineer = afternoonEngineer ?? throw new ArgumentNullException(nameof(afternoonEngineer));
            Day = day;
        }

        private bool Equals(ServiceDayDto other)
        {
            return ForenoonEngineer.Equals(other.ForenoonEngineer) && AfternoonEngineer.Equals(other.AfternoonEngineer) && Day.Equals(other.Day);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ServiceDayDto) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ForenoonEngineer.GetHashCode();
                hashCode = (hashCode * 397) ^ AfternoonEngineer.GetHashCode();
                hashCode = (hashCode * 397) ^ Day.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{nameof(ForenoonEngineer)}: {ForenoonEngineer}, {nameof(AfternoonEngineer)}: {AfternoonEngineer}, {nameof(Day)}: {Day}";
        }
    }
}