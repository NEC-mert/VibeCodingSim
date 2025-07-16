using System.Collections.Generic;
using NEC.Common;
using NEC.GameModule.Player.Inventory;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace NEC.GameModule.Player.Assistant
{
    public class AssistantController : ItemController
    {
        [Header("References")]
        [SerializeField] protected TextMeshProUGUI assistantText;
        
        [Header("Options")]
        [SerializeField] protected AssistantType assistantType;
        [SerializeField] protected Color selectedColor;
        [SerializeField] protected Color unselectedColor;

        private string selectedHex => selectedColor.ToHexString().Substring(0, 6);
        private string unselectedHex => unselectedColor.ToHexString().Substring(0, 6);

        public override void HandleItemInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(FinalizeSelection());
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SelectPreviousOption();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SelectNextOption();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SelectOptionAtIndex(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SelectOptionAtIndex(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SelectOptionAtIndex(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SelectOptionAtIndex(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SelectOptionAtIndex(5);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                SelectOptionAtIndex(6);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                SelectOptionAtIndex(7);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                SelectOptionAtIndex(8);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                SelectOptionAtIndex(9);
            }
            else if (Input.GetKey(KeyCode.LeftCommand) && Input.GetKeyDown(KeyCode.C))
            {
                Debug.Log("Command + C pressed.");
            }
            else if (Input.GetKey(KeyCode.LeftCommand) && Input.GetKeyDown(KeyCode.V))
            {
                Debug.Log("Command + V pressed.");
            }
        }

        private void SelectNextOption()
        {
            var currentText = assistantText.text;
            var array = currentText.Split("\n");
            var optionToLineIndex = new Dictionary<int, int>();
            var currentOptionIndex = 0;
            var selectedOptionIndex = -1;
            for (var i = 0; i < array.Length; i++)
            {
                var text = array[i];
                if (!text.Contains("<color=#"))
                    continue;
                
                optionToLineIndex.Add(currentOptionIndex, i);
                if (text.Contains($"<color=#{selectedHex}>"))
                {
                    selectedOptionIndex = currentOptionIndex;
                }
                    
                currentOptionIndex++;
            }

            if (currentOptionIndex == 0)
            {
                Utilities.LogError("No options found in the assistant text.");
                return;
            }

            if (selectedOptionIndex == -1)
            {
                var lineIndex = optionToLineIndex[0];
                array[lineIndex] = array[lineIndex].Replace($"<color=#{unselectedHex}>", $"<color=#{selectedHex}>");
                assistantText.text = string.Join("\n", array);
                return;
            }

            var oldLineIndex = optionToLineIndex[selectedOptionIndex];
            array[oldLineIndex] = array[oldLineIndex].Replace($"<color=#{selectedHex}>", $"<color=#{unselectedHex}>");
            
            var newOptionIndex = (selectedOptionIndex + 1) % currentOptionIndex;
            
            var nextLineIndex = optionToLineIndex[newOptionIndex];
            array[nextLineIndex] = array[nextLineIndex].Replace($"<color=#{unselectedHex}>", $"<color=#{selectedHex}>");
            
            assistantText.text = string.Join("\n", array);
        }

        private void SelectPreviousOption()
        {
            var currentText = assistantText.text;
            var array = currentText.Split("\n");
            var optionToLineIndex = new Dictionary<int, int>();
            var currentOptionIndex = 0;
            var selectedOptionIndex = -1;
            for (var i = 0; i < array.Length; i++)
            {
                var text = array[i];
                if (!text.Contains("<color=#"))
                    continue;
                
                optionToLineIndex.Add(currentOptionIndex, i);
                if (text.Contains($"<color=#{selectedHex}>"))
                {
                    selectedOptionIndex = currentOptionIndex;
                }
                    
                currentOptionIndex++;
            }

            if (currentOptionIndex == 0)
            {
                Utilities.LogError("No options found in the assistant text.");
                return;
            }

            if (selectedOptionIndex == -1)
            {
                var lineIndex = optionToLineIndex[0];
                array[lineIndex] = array[lineIndex].Replace($"<color=#{unselectedHex}>", $"<color=#{selectedHex}>");
                assistantText.text = string.Join("\n", array);
                return;
            }
            
            var oldLineIndex = optionToLineIndex[selectedOptionIndex];
            array[oldLineIndex] = array[oldLineIndex].Replace($"<color=#{selectedHex}>", $"<color=#{unselectedHex}>");
            
            var newOptionIndex = (selectedOptionIndex - 1 + currentOptionIndex) % currentOptionIndex;
            
            var newLineIndex = optionToLineIndex[newOptionIndex];
            array[newLineIndex] = array[newLineIndex].Replace($"<color=#{unselectedHex}>", $"<color=#{selectedHex}>");
            
            assistantText.text = string.Join("\n", array);
        }

        private void SelectOptionAtIndex(int index)
        {
            var currentText = assistantText.text;
            var array = currentText.Split("\n");
            var isValidIndex = false;
            for (var i = 0; i < array.Length; i++)
            {
                var text = array[i];
                if (text.Contains("<color=#"))
                {
                    if (text.Contains($">{index}.") && text.Contains($"<color=#{unselectedHex}>"))
                    {
                        isValidIndex = true;
                        array[i] = text.Replace($"<color=#{unselectedHex}>", $"<color=#{selectedHex}>");
                    }
                    else if (text.Contains($"<color=#{selectedHex}>"))
                    {
                        array[i] = text.Replace($"<color=#{selectedHex}>", $"<color=#{unselectedHex}>");
                    }
                }
            }

            if (isValidIndex)
            {
                assistantText.text = string.Join("\n", array);
            }
        }

        private string FinalizeSelection()
        {
            var currentText = assistantText.text;
            var array = currentText.Split("\n");
            foreach (var text in array)
            {
                if (text.Contains($"<color=#{selectedHex}>"))
                {
                    return text.Split(".")[1].Split("<")[0].Trim();
                }
            }

            return null;
        }

        [Button]
        private void TestFunction()
        {
            var array = assistantText.text.Split("\n");
            var counter = 1;
            foreach (var text in array)
            {
                if (text.Contains($"{counter.ToString()}."))
                {
                    var message = text.Split(".")[1].Split("<")[0].Trim();
                    var color = text.Split("=")[1].Split(">")[0].Trim();
                    Debug.Log($"{color} - {message}");
                    counter++;
                }
            }
        }
    }
}
