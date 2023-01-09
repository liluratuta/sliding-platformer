using Scripts.Infrastructure;
using UnityEngine;

namespace Scripts.Dron
{
    public class DronMover : MonoBehaviour
    {
        private Transform _targetTransform;
        private IUpdater _updater;

        public void Init(Transform targetTransform, IUpdater updater)
        {
            _targetTransform = targetTransform;
            _updater = updater;
            _updater.Updated += OnUpdate;
        }

        private void OnDestroy()
        {
            if (_updater != null)
            {
                _updater.Updated -= OnUpdate;
            }
        }

        private void OnUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, _targetTransform.position, 0.01f);
        }
    }
}