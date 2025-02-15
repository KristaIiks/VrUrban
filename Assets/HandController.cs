using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
public class HandController : MonoBehaviour
{
    public InputActionReference gripInput;
    public InputActionReference triggerInput;
    public InputActionReference indexInput;
    public InputActionReference thumbInput;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(!animator)return;
        float grip = gripInput.action.ReadValue<float>();
        float trigger = triggerInput.action.ReadValue<float>();
        float indexTouch = indexInput.action.ReadValue<float>();
        float thumbTouch = thumbInput.action.ReadValue<float>();

        animator.SetFloat("Grip", grip);
        animator.SetFloat("Trigger", trigger);
        animator.SetFloat("Index", indexTouch);
        animator.SetFloat("Thumb", thumbTouch);
    }
}
