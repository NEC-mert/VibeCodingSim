using UnityEngine;

namespace NEC.SaveModule
{
    public static class Memory
    {
        public static int LoadInt(IntSave save)
        {
            return PlayerPrefs.GetInt(save.ToString());
        }

        public static void SaveInt(IntSave save, int value)
        {
            PlayerPrefs.SetInt(save.ToString(), value);
        }

        public static float LoadFloat(FloatSave save)
        {
            return PlayerPrefs.GetFloat(save.ToString());
        }

        public static void SaveFloat(FloatSave save, float value)
        {
            PlayerPrefs.SetFloat(save.ToString(), value);
        }

        public static string LoadString(StringSave save)
        {
            return PlayerPrefs.GetString(save.ToString());
        }

        public static void SaveString(StringSave save, string value)
        {
            PlayerPrefs.SetString(save.ToString(), value);
        }

        public static void Dispose()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}