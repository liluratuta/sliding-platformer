using Scripts.Infrastructure;
using System;
using System.Linq;
using UnityEngine;

namespace Scripts.Hero
{
    public class GroundScanner : MonoBehaviour
    {
        public event Action<GroundType> GroundTypeChanged;

        public Vector3 SurfaceDirection { get; private set; }
        public GroundType CurrentGroundType
        {
            get { return _currentGroundType; }
            private set
            {
                if (_currentGroundType == value)
                    return;

                GroundTypeChanged?.Invoke(value);
                _currentGroundType = value;
            }
        }

        public Transform HeroTransform;

        private GameSettings _gameSettings;
        private IUpdater _updater;
        private GroundType _currentGroundType;

        private RaycastHit[] _hitResults = new RaycastHit[5];

        public void Init(IUpdater updater, GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
            _updater = updater;
            CurrentGroundType = GroundType.None;
            _updater.Updated += OnUpdate;
        }

        private void OnDestroy()
        {
            if (_updater != null )
            {
                _updater.Updated -= OnUpdate;
            }
        }

        private void OnUpdate()
        {
            if (!IsGrounded())
            {
                CurrentGroundType = GroundType.Empty;
                SurfaceDirection = Vector3.zero;
                return;
            }

            if (!TryRaycastFromHero(Vector3.left * _gameSettings.ScanOffsetX, out var leftHitPoint))
            {
                SetHorizontalScannerState();
                return;
            }

            if (!TryRaycastFromHero(Vector3.right * _gameSettings.ScanOffsetX, out var rightHitPoint))
            {
                SetHorizontalScannerState();
                return;
            }

            UpdateScannerState(leftHitPoint, rightHitPoint);
        }

        private void SetHorizontalScannerState()
        {
            CurrentGroundType = GroundType.Horizontal;
            SurfaceDirection = Vector3.right;
        }

        private void UpdateScannerState(Vector3 leftHitPoint, Vector3 rightHitPoint)
        {
            if (IsSlant(leftHitPoint, rightHitPoint))
                CurrentGroundType = CurrentGroundType != GroundType.Slant && CurrentGroundType != GroundType.StartSlant 
                    ? GroundType.StartSlant 
                    : GroundType.Slant;
            else
                CurrentGroundType = CurrentGroundType == GroundType.Slant ? GroundType.EndSlant : GroundType.Horizontal;
            
            var direction = (rightHitPoint - leftHitPoint).normalized;
            if (direction.y > 0f)
                direction = -direction;
            SurfaceDirection = direction;
        }

        private bool IsGrounded() =>
            Physics.RaycastNonAlloc(HeroTransform.position + Vector3.up, Vector3.down,
                _hitResults, _gameSettings.GroundedDistance, _gameSettings.GroundLayerMask) > 0;

        private bool TryRaycastFromHero(Vector3 offset, out Vector3 hitPoint)
        {
            var hitCount = Physics.RaycastNonAlloc(HeroTransform.position + offset + Vector3.up, Vector3.down,
                _hitResults, _gameSettings.GroundScanerMaxDistance, _gameSettings.GroundLayerMask);

            if (hitCount == 0)
            {
                hitPoint = Vector3.zero;
                return false;
            }

            Vector3 point = _hitResults.First().point;

            for (var i = 1; i < hitCount; i++)
            {
                if (_hitResults[i].point.y < point.y)
                    continue;

                point = _hitResults[i].point;
            }

            hitPoint = point;
            return true;
        }

        private bool IsSlant(Vector3 leftPoint, Vector3 rightPoint) =>
            Mathf.Abs(rightPoint.y - leftPoint.y) >= _gameSettings.SlantHeightDifference;
    }
}