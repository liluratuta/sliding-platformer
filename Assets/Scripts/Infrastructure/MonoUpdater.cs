using System;
using UnityEngine;

namespace Scripts.Infrastructure
{
    public class MonoUpdater : MonoBehaviour, IUpdater
    {
        public event Action Updated;
        public event Action<float> UpdatedWithDelta;

        private void Update()
        {
            Updated?.Invoke();
            UpdatedWithDelta?.Invoke(Time.deltaTime);
        }
    }
}