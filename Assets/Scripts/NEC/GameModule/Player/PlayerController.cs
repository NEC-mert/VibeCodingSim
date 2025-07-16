using NEC.Common;
using NEC.GameModule.Player.Inventory;
using NEC.GameModule.Player.Items;
using UnityEngine;

namespace NEC.GameModule.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private PhoneController phoneController;
        [SerializeField] private LayerMask groundMask;

        public ItemController ActiveItem { get; private set; }
        
        private Vector3 _position = Vector3.zero;
        private Vector3 _velocity = Vector3.zero;
        private Vector3 _zero = Vector3.zero;

        private void Awake()
        {
            _position = transform.position;
            ActiveItem = phoneController;
        }

        private void Start()
        {
            var sceneCameras = FindObjectsByType<Camera>(
                FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var sceneCamera in sceneCameras)
            {
                if (ReferenceEquals(sceneCamera, playerCamera))
                    continue;
                
                sceneCamera.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            Cursor.visible = false;
            HandleMovement();
            HandleCameraRotation();
            HandleItemInput();
        }

        private void HandleMovement()
        {
            var settings = Settings.GameSettings;
            var isGrounded = characterController.isGrounded;
            if (isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
            var moveSpeed = Input.GetAxis("Fire3") > 0 ? 1.5f * settings.MoveSpeed : settings.MoveSpeed;
            var move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
            move = Vector3.ClampMagnitude(move, 1f);
            characterController.Move(Time.deltaTime * moveSpeed * move);
            if (_velocity.y > 0 && !isGrounded)
            {
                _velocity.y += settings.Gravity * settings.JumpMultiplier * Time.deltaTime;
            }
            else if (_velocity.y < 0 && !isGrounded)
            {
                _velocity.y += settings.Gravity * settings.FallMultiplier * Time.deltaTime;
            }
            else
            {
                _velocity.y += settings.Gravity * Time.deltaTime;
            }
            characterController.Move(Time.deltaTime * _velocity);
            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                _velocity.y = Mathf.Sqrt(settings.JumpHeight * -2f * settings.Gravity);
            }
        }

        private void HandleCameraRotation()
        {
            var settings = Settings.GameSettings;
            var delta = new Vector3(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0f);
            delta *= settings.MouseSensitivity * Time.deltaTime;
            delta = Vector3.ClampMagnitude(delta, 50);
            var current = new Vector3(playerCamera.transform.localRotation.eulerAngles.x,
                transform.localRotation.eulerAngles.y, 0f);
            // var current = playerCamera.transform.localRotation.eulerAngles;
            var target = new Vector3(current.x + delta.y, current.y + delta.x, 0f);
            if (target.x > 180f && target.x < 360f - settings.RotationLimit.x)
            {
                target.x = 360f - settings.RotationLimit.x;
            }
            if (target.x <= 180f && target.x > settings.RotationLimit.x)
            {
                target.x = settings.RotationLimit.x;
            }
            if (target.y > 180f && target.y < 360f - settings.RotationLimit.y)
            {
                target.y = 360f - settings.RotationLimit.y;
            }
            if (target.y <= 180f && target.y > settings.RotationLimit.y)
            {
                target.y = settings.RotationLimit.y;
            }
            current = Vector3.SmoothDamp(current, target, ref _zero, 0.1f);
            playerCamera.transform.localRotation = Quaternion.Euler(current.ResetY());
            transform.rotation = Quaternion.Euler(current.ResetX());
        }

        private void HandleItemInput()
        {
            ActiveItem.HandleItemInput();
        }

        public void ResetPlayer()
        {
            transform.position = _position;
        }
    }
}