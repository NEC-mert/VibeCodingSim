using NEC.Common;
using NEC.GameModule;
using NEC.UIModule;
using UnityEngine;

namespace NEC.MainModule
{
    public class MainManager : AManager<MainManager>
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private UIManager uiManager;
        
        public GameManager GameManager => gameManager;
        public UIManager UIManager => uiManager;
    }
}