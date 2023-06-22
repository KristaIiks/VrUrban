using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BuildTool : Tool
{
    [SerializeField] private Transform Content;
    [SerializeField] private GameObject VariantPrefab;
    [SerializeField] private AudioClip OnSelectBuild;

    private ToolParams _currentBuild;
    private AudioSource _audioSource;

    public override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
    }

    public override void OpenTool(ToolParams _params)
    {
        base.OpenTool(_params);

        foreach (Transform item in Content)
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < _params.BuildVariant.Length; i++)
        {
            BuildSlot _tmp = Instantiate(VariantPrefab, Content, false).GetComponent<BuildSlot>();
            _tmp.Init(_params.BuildVariant[i].Icon, _params.BuildVariant[i].Object, _params.BuildVariant[i]._canSelect, _params.BuildVariant[i]._isSelected);
        }
        _currentBuild = _params;
    }

    public void SelectBuild(GameObject _obj)
    {
        base.CloseTool();

        _audioSource.PlayOneShot(OnSelectBuild);
        HandSelect.Instance.Deselect();
        _currentBuild.Selected.GetComponent<Buildable>().ChangeBuild(_obj);
    }

}