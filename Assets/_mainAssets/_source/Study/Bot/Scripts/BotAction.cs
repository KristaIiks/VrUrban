using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace StudySystem.Bot
{
    [RequireComponent(typeof(XRSimpleInteractable), typeof(AudioSource))]
    public class BotAction : MonoBehaviour
    {
        [SerializeField] private AudioClip _clickSound;

        public static UnityAction<SelectEnterEventArgs> ClickEvent;

        private XRSimpleInteractable _inter;
        private AudioSource _audioSource;

        private void Awake()
        {
            _inter = GetComponent<XRSimpleInteractable>();
            _audioSource = GetComponent<AudioSource>();

            ClickEvent += (s) => _audioSource.PlayOneShot(_clickSound);
            _inter.selectEntered.AddListener(ClickEvent);
        }
    }
}
