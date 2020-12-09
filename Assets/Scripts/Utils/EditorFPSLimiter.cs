using UnityEngine;

namespace Utils
{
    public class EditorFPSLimiter : MonoBehaviour
    {
        private void Start()
        {
#if UNITY_EDITOR
            // Limit FPS in the editor
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
#else
        // Use hardware VSync instead of a software VSync (hardware VSync should be enabled in the quality settings)
        Application.targetFrameRate = -1;
#endif
        }
    }
}