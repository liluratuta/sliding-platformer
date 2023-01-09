using Scripts.Hero;
using Scripts.Infrastructure;
using UnityEngine;

namespace Scripts.Dron
{
    public class DronFactory
    {
        private readonly AssetsRepository _assetsRepository;
        private readonly IUpdater _updater;

        public DronFactory(AssetsRepository assetsRepository, IUpdater updater)
        {
            _assetsRepository = assetsRepository;
            _updater = updater;
        }

        public GameObject Create(GameObject heroGameObject)
        {
            var prefab = _assetsRepository.DronPrefab;
            var startPosition = heroGameObject.GetComponentInChildren<DronSpawnPoint>().Position;
            var dron = GameObject.Instantiate(prefab, startPosition, Quaternion.identity);

            var dronSkin = dron.GetComponent<DronSkin>();
            dronSkin.Init(heroGameObject.GetComponent<HeroMover>());
            dronSkin.SetDefaultSkin();

            dron.GetComponent<DronMover>().Init(heroGameObject.GetComponentInChildren<DronSpawnPoint>().transform, _updater);

            return dron;
        }
    }
}