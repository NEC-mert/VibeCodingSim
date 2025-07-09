using NEC.GameModule.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NEC.TestModule
{
    public class TestController : MonoBehaviour
    {
        private PlayerController _player;
        
        private void Awake()
        {
            _player = FindFirstObjectByType<PlayerController>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _player.ResetPlayer();
            }
        }

        [Button]
        private void TestFunction()
        {
            
        }
    }
}
