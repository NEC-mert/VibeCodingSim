using UnityEngine;

namespace NEC.GameModule.Common
{
    [CreateAssetMenu(menuName = "NEC/Settings/GameSettings", fileName = "GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private float mouseSensitivity;
        
        public float MouseSensitivity => mouseSensitivity;
    }
}