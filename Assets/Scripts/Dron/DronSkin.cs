using Scripts.Hero;
using UnityEngine;

namespace Scripts.Dron
{
    public class DronSkin : MonoBehaviour
    {
        public MeshRenderer MeshRenderer;
        public Material ActiveMaterial;
        public Material DefaultMaterial;

        public void Init(HeroMover heroMover)
        {
            heroMover.SlidingChanged += OnSlidingChanged;
        }

        public void SetDefaultSkin()
        {
            MeshRenderer.material = DefaultMaterial;
        }

        private void OnSlidingChanged(bool isSliding)
        {
            if (isSliding)
            {
                MeshRenderer.material = ActiveMaterial;
                return;
            }

            SetDefaultSkin();
        }
    }
}