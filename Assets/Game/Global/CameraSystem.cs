using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class CameraSystem : MonoBehaviour, ISystem, IOnInit
    {
        [field: SerializeField] public Camera mainCamera { get; private set; }
        public void OnInit()
        {
        }

        public void Dispose()
        {
        }

        public void SetToMenuCamera() {

            mainCamera = transform.parent.Find("MenuCamera").GetComponent<Camera>();
        }

    }
}