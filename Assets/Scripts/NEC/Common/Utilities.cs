using UnityEngine;

namespace NEC.Common
{
    public static class Utilities
    {
        public static void Log(string message)
        {
#if UNITY_EDITOR
            Debug.Log($"[NEC] - {message}");
#endif
        }

        public static void LogWarning(string message)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"[NEC] - {message}");
#endif
        }

        public static void LogError(string message)
        {
#if UNITY_EDITOR
            Debug.LogError($"[NEC] - {message}");
#endif
        }

        public static bool GetRandomBool()
        {
            return Random.Range(0, 2) == 0;
        }

        public static bool GetRandomBool(int chance)
        {
            if (chance < 0 || chance > 100)
            {
                LogError($"Given chance {chance} should be between 0 and 100.");
                return false;
            }

            var random = Random.Range(0, 100);
            return chance > random;
        }
    }
}
