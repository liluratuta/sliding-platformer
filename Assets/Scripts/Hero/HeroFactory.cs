using Scripts.Infrastructure;
using Scripts.Services.Input;
using UnityEngine;

namespace Scripts.Hero
{
    public class HeroFactory
    {
        private readonly AssetsRepository _assetsRepository;
        private readonly GameSettings _gameSettings;
        private readonly InputService _inputService;
        private readonly IUpdater _updater;

        public HeroFactory(AssetsRepository assetsRepository,
            GameSettings gameSettings,
            InputService inputService,
            IUpdater updater)
        {
            _assetsRepository = assetsRepository;
            _gameSettings = gameSettings;
            _inputService = inputService;
            _updater = updater;
        }

        public GameObject Create(Vector3 startPosition)
        {
            var prefab = _assetsRepository.HeroPrefab;
            var hero = GameObject.Instantiate(prefab, startPosition, Quaternion.identity);

            hero.GetComponent<GroundScanner>().Init(_updater, _gameSettings);
            hero.GetComponent<HeroMover>().Init(_inputService, _updater, _gameSettings);

            return hero;
        }
    }
}