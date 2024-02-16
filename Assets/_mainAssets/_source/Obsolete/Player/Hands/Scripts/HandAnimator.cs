using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Hands
{
    [RequireComponent(typeof(Animator))]
    public class HandAnimator : MonoBehaviour
    {
        [SerializeField] private InputActionReference _trigger;
        [SerializeField] private InputActionReference _grip;

        private Animator _anim;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        private void Update()
        {
            float trigger = _trigger.action.ReadValue<float>();
            float grip = _grip.action.ReadValue<float>();

            _anim.SetFloat("Trigger", trigger);
            _anim.SetFloat("Grip", grip);
        }
    }
}
