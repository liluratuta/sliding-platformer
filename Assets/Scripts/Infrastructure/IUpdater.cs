using System;

namespace Scripts.Infrastructure
{
    public interface IUpdater
    {
        event Action Updated;
        event Action<float> UpdatedWithDelta;
    }
}