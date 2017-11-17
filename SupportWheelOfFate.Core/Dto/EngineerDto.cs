using System;

namespace SupportWheelOfFate.Core.Dto
{
    public sealed class EngineerDto
    {
        public string LastName { get; }
        public string FirstName { get; }

        public EngineerDto(string firstName, string lastName)
        {
            LastName = lastName;
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        }

        private bool Equals(EngineerDto other)
        {
            return string.Equals(LastName, other.LastName) && string.Equals(FirstName, other.FirstName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((EngineerDto) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((LastName != null ? LastName.GetHashCode() : 0) * 397) ^ (FirstName != null ? FirstName.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return $"{nameof(LastName)}: {LastName}, {nameof(FirstName)}: {FirstName}";
        }
    }
}