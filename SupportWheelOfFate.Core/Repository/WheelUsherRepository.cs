using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SupportWheelOfFate.Core.Dto;
using SupportWheelOfFate.Core.Generator;
using SupportWheelOfFate.Core.Queries;

namespace SupportWheelOfFate.Core.Repository
{
    public interface IWheelUsherRepository
    {
        IImmutableList<ServiceDayDto> TurnWheel(TurnWheelQuery turnWheelQuery);
    }
    
    public class WheelUsherRepository : IWheelUsherRepository
    {
        private readonly DateTime _startDate = DateTime.UtcNow.Date;
        private readonly IEngineersGenerator _engineersGenerator;
        private readonly Func<int, bool> _isEven = x => x % 2 == 0;

        public WheelUsherRepository(IEngineersGenerator engineersGenerator)
        {
            _engineersGenerator = engineersGenerator ?? throw new ArgumentNullException(nameof(engineersGenerator));
        }

        public IImmutableList<ServiceDayDto> TurnWheel(TurnWheelQuery turnWheelQuery)
        {
            if (turnWheelQuery.InvolvedEngineersCount < EngineersBoundary.Min.Value)
            {
                throw new ArgumentOutOfRangeException(nameof(turnWheelQuery), $"Engineers count cannot be less than {EngineersBoundary.Min.Value}");
            }

            var engineers = PrepareEngineersSequence(turnWheelQuery.InvolvedEngineersCount);

            return _isEven(turnWheelQuery.InvolvedEngineersCount) 
                ? TurnWheelForEvenParticipants(engineers) 
                : TurnWheelForOddParticipants(engineers);
        }

        private ImmutableList<(EngineerDto, EngineerDto)> PrepareEngineersSequence(int involvedEngineersCount)
        {
            var engineers = _engineersGenerator
                .Generate(involvedEngineersCount)
                .Shuffle()
                .ToImmutableList();
            
            return engineers.AsEnumerable()
                .Reverse()
                .Zip(engineers, (x, y) => (forenoon: new EngineerDto(x.Item1, x.Item2), 
                                           afternoon: new EngineerDto(y.Item1, y.Item2)))
                .ToImmutableList();
        }

        private IImmutableList<ServiceDayDto> TurnWheelForOddParticipants(ImmutableList<(EngineerDto, EngineerDto)> engineers)
        {
            const int secondIndex = 1;
            var middleIndex = engineers.Count / 2;
            var middle = (engineers.First().Item1, engineers[middleIndex].Item2);
            var first = (engineers[middleIndex].Item1, engineers[secondIndex].Item2);
            var second = (engineers[secondIndex].Item1, engineers.First().Item2);
            return AssembleServiceDays(
                new[] {first, second}
                    .Concat(engineers.Skip(2).Take(middleIndex - 2))
                    .Concat(new[] {middle})
                    .Concat(engineers.Skip(middleIndex + 1))
            );
        }

        private IImmutableList<ServiceDayDto> TurnWheelForEvenParticipants(ImmutableList<(EngineerDto, EngineerDto)> engineers)
        {
            var middleIndex = engineers.Count / 2;
            var middle = engineers[middleIndex];
            return AssembleServiceDays(
                new [] {middle}.Concat(engineers.Where(x => !Equals(x, middle)))
             );
        }

        private IImmutableList<ServiceDayDto> AssembleServiceDays(IEnumerable<(EngineerDto, EngineerDto)> engineers)
        {
            var engineersList = engineers as IList<(EngineerDto, EngineerDto)> ?? engineers.ToList();
            var followingWorkingDays = _startDate.GetFollowingWorkingDays(engineersList.Count);
            var assembledServiceDays = engineersList
                .Zip(followingWorkingDays, (x, y) => new ServiceDayDto(x.Item1, x.Item2, y))
                .ToImmutableList();
            return assembledServiceDays;
        }
    }
}
