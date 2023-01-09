using Scripts.Infrastructure;
using Scripts.Services.Input;
using System;
using UnityEngine;

namespace Scripts.Hero
{
    public class HeroMover : MonoBehaviour
    {
        public event Action<bool> SlidingChanged;

        public GroundScanner GroundScanner;
        public HeroAnimator HeroAnimator;
        public CharacterController CharacterController;

        private InputService _inputService;
        private GameSettings _gameSettings;
        private IUpdater _updater;

        private float _timeToSlant = -1;
        private bool _isSliding;

        public void Init(InputService inputService, IUpdater updater, GameSettings gameSettings)
        {
            _inputService = inputService;
            _gameSettings = gameSettings;
            _updater = updater;

            _updater.UpdatedWithDelta += OnUpdate;
        }

        private void OnDestroy()
        {
            _updater.UpdatedWithDelta -= OnUpdate;
        }

        private void OnUpdate(float deltaTime)
        {
            switch (GroundScanner.CurrentGroundType)
            {
                case GroundType.None: break;
                case GroundType.Empty: HandleEmptyGroundType(); break;
                case GroundType.Horizontal: HandleHorizontalGroundType(deltaTime); break;
                case GroundType.StartSlant: HandleStartSlantGroundType(); break;
                case GroundType.Slant: HandleSlantGroundType(deltaTime); break;
                case GroundType.EndSlant: HandleEndSlantGroundType(); break;
            }
        }

        private void HandleEndSlantGroundType() =>
            SetSliding(false);

        private void HandleStartSlantGroundType()
        {
            if (IsSurfaceDirectionSameInput())
            {
                SetSliding(true);
                return;
            }

            SetSliding(false);
            _timeToSlant = _gameSettings.TimeToSlant;
        }

        private void HandleSlantGroundType(float deltaTime)
        {
            if (_isSliding)
            {
                if (!HeroAnimator.IsSliding())
                    HeroAnimator.PlaySliding();

                CharacterController.Move(SlidingDirection() * _gameSettings.HeroSlideSpeed * deltaTime);

                return;
            }

            if (_timeToSlant > 0 && _inputService.Horizontal != 0)
            {
                _timeToSlant -= deltaTime;
                HandleHorizontalGroundType(deltaTime);
                return;
            }

            SetSliding(true);
        }

        private bool IsSurfaceDirectionSameInput() =>
            Mathf.Sign(GroundScanner.SurfaceDirection.x) == Mathf.Sign(_inputService.Horizontal);

        private void HandleHorizontalGroundType(float deltaTime)
        {
            var movementDirection = MovementDirection();

            if (Mathf.Abs(movementDirection.x) > 0f)
            {
                PerformMoving(movementDirection, deltaTime);
                return;
            }

            PerformIdle();
        }

        private void PerformMoving(Vector3 movementDirection, float deltaTime)
        {
            if (!HeroAnimator.IsMoving())
                HeroAnimator.PlayMoving();

            CharacterController.Move(movementDirection * _gameSettings.HeroMoveSpeed * deltaTime);
        }

        private void PerformIdle()
        {
            if (!HeroAnimator.IsIdle())
                HeroAnimator.PlayIdle();
        }

        private void HandleEmptyGroundType()
        {
            if (!HeroAnimator.IsIdle())
                HeroAnimator.PlayIdle();

            CharacterController.Move(Physics.gravity);
        }

        private Vector3 MovementDirection() =>
            new Vector3(_inputService.Horizontal, 0, 0) + Physics.gravity;

        private Vector3 SlidingDirection() =>
            GroundScanner.SurfaceDirection + Physics.gravity;

        private void SetSliding(bool isSliding)
        {
            SlidingChanged?.Invoke(isSliding);
            _isSliding = isSliding;
        }
    }
}