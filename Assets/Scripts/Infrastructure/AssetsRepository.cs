using UnityEngine;

namespace Scripts.Infrastructure
{
    [CreateAssetMenu(fileName = "AssetsRepository", menuName = "Game/AssetsRepository")]
    public class AssetsRepository : ScriptableObject
    {
        public GameObject HeroPrefab;
        public GameObject DronPrefab;
    }
}
