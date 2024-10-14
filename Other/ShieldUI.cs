using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameCreator.Runtime.Characters;

public class ShieldUI : MonoBehaviour
{
    public string defenseTextPrefix = "Defense: ";

    public Text defenseText;
    public TextMeshProUGUI defenseTextMeshPro;
    public Image defenseBar;

    private Character character;

    private void Start()
    {
        character = GetComponent<Character>();

        if (character == null)
        {
            Debug.LogError("Character component not found on the GameObject.");
            return;
        }

        character.Combat.EventDefenseChange += UpdateDefenseUI;

        UpdateDefenseUI();
    }

    private void OnDestroy()
    {
        if (character != null)
        {
            character.Combat.EventDefenseChange -= UpdateDefenseUI;
        }
    }

    private void UpdateDefenseUI()
    {
        if (character.Combat.MaximumDefense <= 0)
        {
            character.Combat.MaximumDefense = 1;
        }

        float roundedDefense = Mathf.Round(character.Combat.CurrentDefense * 10f) / 10f;

        if (defenseText != null)
        {
            defenseText.text = defenseTextPrefix + roundedDefense.ToString("F1");
        }

        if (defenseTextMeshPro != null)
        {
            defenseTextMeshPro.text = defenseTextPrefix + roundedDefense.ToString("F1");
        }

        if (defenseBar != null)
        {
            float fillAmount = character.Combat.CurrentDefense / character.Combat.MaximumDefense;
            defenseBar.fillAmount = Mathf.Clamp01(fillAmount);
        }
    }
}
