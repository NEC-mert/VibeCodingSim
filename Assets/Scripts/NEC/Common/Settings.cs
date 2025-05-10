using NEC.GameModule.Common;
using UnityEngine;

namespace NEC.Common
{
    public static class Settings
    {
        private static GameSettings _gameSettings;

        public static GameSettings GameSettings
        {
            get
            {
                if (!_gameSettings)
                {
                    _gameSettings = Resources.Load<GameSettings>("GameSettings");
                }

                return _gameSettings;
            }
        }

        public static void Dispose()
        {
            _gameSettings = null;
        }
    }
}