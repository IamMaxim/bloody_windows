using UnityEngine;

namespace Containers.UI.UIService.Utils
{
    public class UIRotator : MonoBehaviour
    {
        public float speed = 30f;

        private RectTransform _rectTransform;

        private void Start() => 
            _rectTransform = GetComponent<RectTransform>();

        // Update is called once per frame
        private void Update() => 
            _rectTransform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
    }
}