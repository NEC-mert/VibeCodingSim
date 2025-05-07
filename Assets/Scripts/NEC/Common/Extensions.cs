using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace NEC.Common
{
    public static class Extensions
    {
        public static void LogAll<T>(this List<T> list)
        {
#if UNITY_EDITOR
            if (list == null)
            {
                Utilities.LogError("Given list is null!");
                return;
            }

            for (var i = 0; i < list.Count; i++)
            {
                Utilities.Log($"[{i}] -> {list[i]}");
            }
#endif
        }
        
        public static void LogAll<T>(this T[] array)
        {
#if UNITY_EDITOR
            if (array == null)
            {
                Utilities.LogError("Given array is null!");
                return;
            }

            for (var i = 0; i < array.Length; i++)
            {
                Utilities.Log($"[{i}] -> {array[i]}");
            }
#endif
        }
        
        public static void LogAll<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
#if UNITY_EDITOR
            if (dictionary == null)
            {
                Utilities.LogError("Given dictionary is null!");
                return;
            }

            foreach (var pair in dictionary)
            {
                Utilities.Log($"Key: {pair.Key} -> Value: {pair.Value}");
            }
#endif
        }
        
        public static void LogAll<T>(this T[,] array)
        {
#if UNITY_EDITOR
            if (array == null)
            {
                Utilities.LogError("Given array is null!");
                return;
            }
            
            var height = array.GetLength(0);
            var width = array.GetLength(1);
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    Utilities.Log($"[{i}][{j}] -> {array[i, j]}");
                }
            }
#endif
        }
        
        public static void LogAll<T>(this T[][] array)
        {
#if UNITY_EDITOR
            if (array == null)
            {
                Utilities.LogError("Given array is null!");
                return;
            }
            
            var outerLength = array.Length;
            for (var i = 0; i < outerLength; i++)
            {
                var innerLength = array[i].Length;
                for (var j = 0; j < innerLength; j++)
                {
                    Utilities.Log($"[{i}][{j}] -> {array[i][j]}");
                }
            }
#endif
        }

        public static void ForEach<T>(this List<T> list, Action<T> action)
        {
            if (list == null)
            {
                Utilities.LogError("Given list is null!");
                return;
            }

            foreach (var item in list)
            {
                action?.Invoke(item);
            }
        }

        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            if (array == null)
            {
                Utilities.LogError("Given array is null!");
                return;
            }

            foreach (var item in array)
            {
                action?.Invoke(item);
            }
        }

        public static void ForEach<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Action<TKey> action)
        {
            if (dictionary == null)
            {
                Utilities.LogError("Given dictionary is null!");
                return;
            }

            foreach (var pair in dictionary)
            {
                action?.Invoke(pair.Key);
            }
        }
        
        public static void ForEach<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Action<TValue> action)
        {
            if (dictionary == null)
            {
                Utilities.LogError("Given dictionary is null!");
                return;
            }

            foreach (var pair in dictionary)
            {
                action?.Invoke(pair.Value);
            }
        }
        
        public static void ForEach<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Action<TKey, TValue> action)
        {
            if (dictionary == null)
            {
                Utilities.LogError("Given dictionary is null!");
                return;
            }

            foreach (var pair in dictionary)
            {
                action?.Invoke(pair.Key, pair.Value);
            }
        }
        
        public static void ForEach<T>(this T[,] array, Action<T> action)
        {
            if (array == null)
            {
                Utilities.LogError("Given array is null!");
                return;
            }
            
            var height = array.GetLength(0);
            var width = array.GetLength(1);
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var unit = array[i, j];
                    action?.Invoke(unit);
                }
            }
        }
        
        public static void ForEach<T>(this T[][] array, Action<T> action)
        {
            if (array == null)
            {
                Utilities.LogError("Given array is null!");
                return;
            }
            
            var outerLength = array.Length;
            for (var i = 0; i < outerLength; i++)
            {
                var innerLength = array[i].Length;
                for (var j = 0; j < innerLength; j++)
                {
                    var unit = array[i][j];
                    action?.Invoke(unit);
                }
            }
        }

        public static void Shuffle<T>(this List<T> list)
        {
            if (list == null)
            {
                Utilities.LogError("Given list is null!");
                return;
            }
            
            var count = list.Count;
            for (var i = 0; i < count - 1; i++)
            {
                var r = UnityEngine.Random.Range(i, count);
                (list[i], list[r]) = (list[r], list[i]);
            }
        }

        public static void Shuffle<T>(this T[] array)
        {
            if (array == null)
            {
                Utilities.LogError("Given array is null!");
                return;
            }
            
            var length = array.Length;
            for (var i = 0; i < length - 1; i++)
            {
                var r = UnityEngine.Random.Range(i, length);
                (array[i], array[r]) = (array[r], array[i]);
            }
        }

        public static void Shuffle<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                Utilities.LogError("Given dictionary is null!");
                return;
            }
            
            var count = dictionary.Count;
            var keyList = new List<TKey>();
            var valueList = new List<TValue>();
            foreach (var pair in dictionary)
            {
                keyList.Add(pair.Key);
                valueList.Add(pair.Value);
            }
            
            keyList.Shuffle();
            valueList.Shuffle();
            dictionary.Clear();

            for (var i = 0; i < count; i++)
            {
                dictionary.Add(keyList[i], valueList[i]);
            }
        }
        
        public static void Shuffle<T>(this T[,] array)
        {
            if (array == null)
            {
                Utilities.LogError("Given array is null!");
                return;
            }
            
            var height = array.GetLength(0);
            var width = array.GetLength(1);
            var total = height * width;

            var indices = new int[total];
            for (var i = 0; i < total; i++)
            {
                indices[i] = i;
            }
            
            indices.Shuffle();

            var temp = array.GetCopy();
            var counter = 0;
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var index = indices[counter];
                    var x = index / width;
                    var y = index % height;
                    array[i, j] = temp[x, y];
                    counter++;
                }
            }
        }
        
        public static void Shuffle<T>(this T[][] array)
        {
            if (array == null)
            {
                Utilities.LogError("Given array is null!");
                return;
            }
            
            var height = array.GetLength(0);
            var width = array.GetLength(1);
            var total = height * width;

            var indices = new int[total];
            for (var i = 0; i < total; i++)
            {
                indices[i] = i;
            }
            
            indices.Shuffle();

            var temp = array.GetCopy();
            var counter = 0;
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var index = indices[counter];
                    var x = index / width;
                    var y = index % height;
                    array[i][j] = temp[x][y];
                    counter++;
                }
            }
        }
        
        public static List<T> GetCopy<T>(this List<T> list)
        {
            if (list == null)
            {
                Utilities.LogError("Given list is null!");
                return null;
            }
            
            var result = new List<T>(list.Count);
            result.AddRange(list);
            return result;
        }
        
        public static T[] GetCopy<T>(this T[] array)
        {
            if (array == null)
            {
                Utilities.LogError("Given array is null!");
                return null;
            }
            
            var result = new T[array.Length];
            array.CopyTo(result, 0);
            return result;
        }
        
        public static Dictionary<TKey, TValue> GetCopy<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                Utilities.LogError("Given dictionary is null!");
                return null;
            }
            
            var result = new Dictionary<TKey, TValue>();
            foreach (var pair in dictionary)
            {
                if (!result.ContainsKey(pair.Key))
                {
                    result.Add(pair.Key, pair.Value);
                }
            }

            return result;
        }
        
        public static T[,] GetCopy<T>(this T[,] array)
        {
            if (array == null)
            {
                Utilities.LogError("Given array is null!");
                return null;
            }
            
            var height = array.GetLength(0);
            var width = array.GetLength(1);
            var result = new T[height, width];
            array.CopyTo(result, 0);
            return result;
        }
        
        public static T[][] GetCopy<T>(this T[][] array)
        {
            if (array == null)
            {
                Utilities.LogError("Given array is null!");
                return null;
            }
            
            var outer = array.Length;
            var result = new T[outer][];
            for (var i = 0; i < outer; i++)
            {
                var inner = array[i].Length;
                var temp = new T[inner];
                for (var j = 0; j < inner; j++)
                {
                    temp[j] = array[i][j];
                }
                
                result[i] = temp;
            }
            
            return result;
        }

        public static void FadeOutImmediate(this Graphic graphic)
        {
            var color = graphic.color;
            color.a = 0f;
            graphic.color = color;
        }

        public static void FadeInImmediate(this Graphic graphic)
        {
            var color = graphic.color;
            color.a = 1f;
            graphic.color = color;
        }
    }
}