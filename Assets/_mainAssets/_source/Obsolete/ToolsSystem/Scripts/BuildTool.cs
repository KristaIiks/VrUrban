using UnityEngine;
using UnityEngine.InputSystem;

namespace ToolsSystem
{
	[RequireComponent(typeof(AudioSource))]
	public class BuildTool : Tool
	{
		[SerializeField] private InputActionReference _placeBtn;

		[SerializeField] private Renderer _model;

		public static BuildTool Instance;

		private void OnValidate()
		{
			_source ??= GetComponent<AudioSource>();
			_line = GetComponent<LineRenderer>();
		}

		protected override void Awake()
		{
			Instance ??= this;
			gameObject.SetActive(false);

			_placeBtn.action.performed += (s) => TryPlace();
		}

		private void OnEnable()
		{
			_model.gameObject.SetActive(true);
		}

		private void OnDisable()
		{
			_model.gameObject.SetActive(false);
		}

		protected override void Update()
		{
			_line.SetPosition(0, transform.position);

			RaycastHit _hit;
			if (Physics.Raycast(transform.position, transform.forward, out _hit, 5f))
			{
				if (_hit.collider.tag == "Placeable")
				{
					SetPosition(_hit.transform.position, true);
					_line.SetPosition(1, _hit.transform.position);
					SetRayColor(CanSelectGrad);
				}
				else
				{
					SetPosition(_hit.point, false);
					_line.SetPosition(1, _hit.point);
					SetRayColor(ErrorSelectGrad);
				}
			}
			else
			{
				SetPosition(transform.position + transform.forward * 3f, false);
				_line.SetPosition(1, transform.position + transform.forward * 3f);
				SetRayColor(ErrorSelectGrad);
			}
		}

		private void SetPosition(Vector3 _pos, bool _canPlace)
		{
			_model.transform.position = _pos;

			if(_canPlace)
			{
				_model.material.color = Color.green;
			}
			else
			{
				_model.material.color = Color.red;
			}
		}

		private void TryPlace()
		{
			RaycastHit _hit;
			if (Physics.Raycast(transform.position, transform.forward, out _hit, 5f))
			{
				if (_hit.collider.tag == "Placeable")
				{
					_hit.collider.GetComponent<Buildable>().Place();
				}
			}
		}
		
		public void ChangeGhost(Renderer _ghost)
		{
			_model.gameObject.SetActive(false);
			
			_model = _ghost;
			_model.gameObject.SetActive(true);
		}
	}
}
