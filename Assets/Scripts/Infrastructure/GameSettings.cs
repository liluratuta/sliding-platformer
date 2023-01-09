using UnityEngine;

namespace Scripts.Infrastructure
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Common")]
        public LayerMask GroundLayerMask;
        [Header("Platform Generator")]
        public float GenerationDistance;
        [Header("Scanner")]
        public float GroundScanerMaxDistance;
        public float GroundedDistance;
        public float ScanOffsetX;
        public float SlantHeightDifference;
        [Header("Hero")]
        public float HeroMoveSpeed;
        public float HeroSlideSpeed;
        public float TimeToSlant;
    }
}
