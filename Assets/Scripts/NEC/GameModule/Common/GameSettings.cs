using UnityEngine;

namespace NEC.GameModule.Common
{
    [CreateAssetMenu(menuName = "NEC/Settings/GameSettings", fileName = "GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private float mouseSensitivity;
        [SerializeField] private Vector2 rotationLimit;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float gravity;
        [SerializeField] private float jumpHeight;
        [SerializeField] private float jumpMultiplier;
        [SerializeField] private float fallMultiplier;
        
        public float MouseSensitivity => mouseSensitivity * 1000;
        public Vector2 RotationLimit => rotationLimit;
        public float MoveSpeed => moveSpeed;
        public float Gravity => gravity;
        public float JumpHeight => jumpHeight;
        public float JumpMultiplier => jumpMultiplier;
        public float FallMultiplier => fallMultiplier;
    }
}