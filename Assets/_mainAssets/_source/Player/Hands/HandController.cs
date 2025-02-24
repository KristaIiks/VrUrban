using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class HandController : MonoBehaviour
{
	[SerializeField] private InputActionReference gripInput;
	[SerializeField] private InputActionReference triggerInput;
	[SerializeField] private InputActionReference indexInput;
	[SerializeField] private InputActionReference thumbInput;

	[SerializeField] private Animator Animator;

	private void OnValidate()
	{
		Animator ??= GetComponent<Animator>();
	}

	private void Update()
	{
		if (!Animator) { return; }
		
		float grip = gripInput.action.ReadValue<float>();
		float trigger = triggerInput.action.ReadValue<float>();
		float indexTouch = indexInput.action.ReadValue<float>();
		float thumbTouch = thumbInput.action.ReadValue<float>();

		Animator.SetFloat("Grip", grip);
		Animator.SetFloat("Trigger", trigger);
		Animator.SetFloat("Index", indexTouch);
		Animator.SetFloat("Thumb", thumbTouch);
	}
}
