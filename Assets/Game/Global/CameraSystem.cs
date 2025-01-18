using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class CameraSystem : MonoBehaviour, ISystem, IOnInit
    {
        [field: SerializeField] public Camera mainCamera { get; private set; }
        public Camera MenuCamera;
        public void OnInit()
        {
            
        }

        public void Dispose()
        {
        }

        public void SetToMenuCamera() {
            MenuCamera.gameObject.SetActive(true);
            mainCamera.gameObject.SetActive(false);
        }

        public void SetToMainCamera()
        {
            MenuCamera.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);
        }

    }
}