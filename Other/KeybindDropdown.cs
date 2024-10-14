using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.InputSystem.LowLevel;

public class KeybindDropdown : MonoBehaviour
{
    public Dropdown keyboardDropdown;
    public Dropdown gamepadDropdown;

    private void Start()
    {
        PopulateKeyboardDropdown();
        PopulateGamepadDropdown();
    }

    private void PopulateKeyboardDropdown()
    {
        keyboardDropdown.ClearOptions();
        List<string> keyboardKeys = new List<string>();

        foreach (Key key in System.Enum.GetValues(typeof(Key)))
        {
            if (key != Key.None) // Skip the 'None' key
            {
                keyboardKeys.Add(key.ToString());
            }
        }

        keyboardDropdown.AddOptions(keyboardKeys);
    }

    private void PopulateGamepadDropdown()
    {
        gamepadDropdown.ClearOptions();
        List<string> gamepadButtons = new List<string>();

        foreach (GamepadButton button in System.Enum.GetValues(typeof(GamepadButton)))
        {
            gamepadButtons.Add(button.ToString());
        }

        gamepadDropdown.AddOptions(gamepadButtons);
    }

    // Add methods to handle the selection change from the dropdown
    public void OnKeyboardDropdownChanged(int index)
    {
        Key selectedKey = (Key)System.Enum.Parse(typeof(Key), keyboardDropdown.options[index].text);
        // Update the keybinding in your script here
    }

    public void OnGamepadDropdownChanged(int index)
    {
        GamepadButton selectedButton = (GamepadButton)System.Enum.Parse(typeof(GamepadButton), gamepadDropdown.options[index].text);
        // Update the gamepad button in your script here
    }
}
