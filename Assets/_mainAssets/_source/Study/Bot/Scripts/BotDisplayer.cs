using UnityEngine;

namespace StudySystem.Bot
{
    public class BotDisplayer: MonoBehaviour
    {
        [SerializeField] private GameObject Content;

        public static void Display(GameObject _menu, bool _auto = true, bool _canClose = true)
        {
            // TODO: instantiate menu add event to auto close
        }

        public static void CloseMenu()
        {
            // TODO: close current/one/all
        }
    }
}
