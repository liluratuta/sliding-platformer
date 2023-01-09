using Scripts.Dron;
using Scripts.Hero;
using Scripts.Services.Input;
using UnityEngine;

namespace Scripts.Infrastructure
{
    public class GameEntryPoint : MonoBehaviour
    {
        public AssetsRepository AssetsRepository;
        public GameSettings GameSettings;
        public Transform RespawnTransform;
        
        private IUpdater _updater;
        private InputService _inputService;
        private HeroFactory _heroFactory;
        private DronFactory _dronFactory;

        private GameObject _hero;
        private GameObject _dron;

        private void Awake()
        {
            _updater = gameObject.AddComponent<MonoUpdater>();
            _inputService = new InputService();
            _heroFactory = new HeroFactory(AssetsRepository, GameSettings, _inputService, _updater);
            _dronFactory = new DronFactory(AssetsRepository, _updater);
        }

        private void Start()
        {
            SpawnHero();

            _updater.Updated += OnUpdate;
        }

        private void OnDestroy()
        {
            _updater.Updated -= OnUpdate;
        }

        private void OnUpdate()
        {
            if (_inputService.IsRespawnButton())
            {
                Destroy(_hero);
                Destroy(_dron);
                
                SpawnHero();
            }
        }

        private void SpawnHero()
        {
            _hero = _heroFactory.Create(RespawnTransform.position);
            _dron = _dronFactory.Create(_hero);
        }
    }
}