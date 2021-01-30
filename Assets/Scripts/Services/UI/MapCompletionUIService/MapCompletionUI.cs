using UIService.ScreenStacks;
using UnityEngine.UI;

namespace Services.UI.MapCompletionUIService
{
    public class MapCompletionUI : UIScreen
    {
        public Text infoText;
        
        public void Show(string duration, string score)
        {
            infoText.text = $"Duration: {duration}\n" +
                            $"Score: {score}";
            Push();
        }
    }
}