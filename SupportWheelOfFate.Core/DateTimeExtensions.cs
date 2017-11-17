using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace SupportWheelOfFate.Core
{
    public static class DateTimeExtensions
    {
        public static IImmutableList<DateTime> GetFollowingWorkingDays(this DateTime me, int amountOfWorkingDays)
        {
            return Enumerable
                .Range(1,  amountOfWorkingDays * 2)
                .Select(x => me.AddDays(x))
                .Where(IsWorkingDay)
                .Take(amountOfWorkingDays)
                .ToImmutableList();
        }
        
        private static bool IsWorkingDay(DateTime date) => 
            date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
    }
}