using System;

namespace SupportWheelOfFate.Core.Queries
{
    public class TurnWheelQuery
    {
        public int InvolvedEngineersCount { get; }

        public TurnWheelQuery(int involvedEngineersCount)
        {
            InvolvedEngineersCount = involvedEngineersCount;
        }

        public bool IsValid() => InvolvedEngineersCount >= EngineersBoundary.Min.Value &&
                                 InvolvedEngineersCount <= EngineersBoundary.Max.Value;
    }
}