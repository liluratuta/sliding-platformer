namespace Scripts.Services.Input
{
    public class InputService
    {
        private const string HorizontalAxis = "Horizontal";

        public float Horizontal => UnityEngine.Input.GetAxis(HorizontalAxis);

        public bool IsRespawnButton() =>
            UnityEngine.Input.GetKeyUp(UnityEngine.KeyCode.R);
    }
}