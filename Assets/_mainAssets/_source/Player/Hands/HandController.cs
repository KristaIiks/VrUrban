using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class HandController : MonoBehaviour
{
	[SerializeField] private InputActionReference gripInput;
	[SerializeField] private InputActionReference triggerInput;
	[SerializeField] private InputActionReference indexInput;
	[SerializeField] private InputActionReference thumbInput;

	private Animator _animator;

	private void OnValidate()
	{
		_animator ??= GetComponent<Animator>();
	}

	private void Update()
	{
		if (!_animator) { return; }
		
		float grip = gripInput.action.ReadValue<float>();
		float trigger = triggerInput.action.ReadValue<float>();
		float indexTouch = indexInput.action.ReadValue<float>();
		float thumbTouch = thumbInput.action.ReadValue<float>();

		_animator.SetFloat("Grip", grip);
		_animator.SetFloat("Trigger", trigger);
		_animator.SetFloat("Index", indexTouch);
		_animator.SetFloat("Thumb", thumbTouch);
	}
}
