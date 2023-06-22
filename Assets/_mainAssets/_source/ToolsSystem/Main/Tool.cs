using UnityEngine;

public class Tool : MonoBehaviour
{
    [SerializeField] private GameObject Panel;

    public static Tool Instance;

    public virtual void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            Panel.SetActive(false);
        }
    }

    public virtual void OpenTool(ToolParams _params)
    {
        Panel.SetActive(true);
    }

    public virtual void CloseTool()
    {
        Panel.SetActive(false);
    }

}

[System.Serializable]
public struct ToolParams
{
    public GameObject Selected;
    public BuildVariant[] BuildVariant;
}
