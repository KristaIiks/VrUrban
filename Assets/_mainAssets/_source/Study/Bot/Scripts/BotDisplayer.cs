using System.Collections.Generic;
using UnityEngine;

namespace StudySystem.Bot
{
    public class BotDisplayer: MonoBehaviour
    {
        [SerializeField] private Transform Content;
        private List<GameObject> _openMenus = new List<GameObject>();

        /// <summary>
        /// Display menu in bot content
        /// </summary>
        /// <param name="_menu">Menu obj</param>
        /// <param name="_auto"></param>
        /// <param name="_canClose">Can close by menu btn</param>
        /// <param name="_closeAll">Close all menus</param>
        public void Display(GameObject _menu, bool _auto = true, bool _canClose = true, bool _closeAll = false) // Auto close???
        {
            if (_closeAll) { CloseAll(); }

            Instantiate(_menu, Content);

            if (_canClose)
            {
                _openMenus.Add(_menu);

                if (_auto)
                {
                    // TODO: add event
                }
            }
        }

        public void CloseMenu(GameObject _obj = null)
        {
            if(_obj != null)
            {
                _openMenus.Remove(_obj);
                Destroy(_obj);
                return;
            }
            _openMenus.Remove(_openMenus?[0]);
            Destroy(_openMenus?[0]);
        }

        public void CloseAll()
        {
            foreach (GameObject _obj in _openMenus)
            {
                Destroy(_obj);
            }
            _openMenus.Clear();
        }

    }
}
