using UnityEngine;

namespace ToolsSystem
{
    public abstract class ToolWindow : MonoBehaviour
    {
        public string _toolName;

        public abstract void Open(ToolParams _params);
        public abstract void Close();

    }
}
