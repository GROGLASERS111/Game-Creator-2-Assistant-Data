using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ETK
{
    public class Hotbar : MonoBehaviour
    {
        public static Hotbar Instance { get; private set; }

        public HotbarSlot[] slots;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Update()
        {
            foreach (var slot in slots)
            {
                if (slot != null && Input.GetKeyDown(slot.activationKey))
                {
                    slot.ExecuteAssignedSkill();
                }
            }
        }

        public void AssignSkillToSlot(Skill skill, int slotIndex)
        {
            if (slotIndex >= 0 && slotIndex < slots.Length)
            {
                slots[slotIndex].AssignSkill(skill);
            }
        }

        public void ExecuteSkillFromSlot(int slotIndex)
        {
            if (slotIndex >= 0 && slotIndex < slots.Length)
            {
                slots[slotIndex].ExecuteAssignedSkill();
            }
        }
    }

    [System.Serializable]
    public class SkillSlot
    {
        public Image icon;
        public KeyCode activationKey = KeyCode.None;
        private Skill assignedSkill;

        public void AssignSkill(Skill skill)
        {
            assignedSkill = skill;
            icon.sprite = skill.IconImage;
            icon.enabled = skill != null;
        }

        public void ExecuteAssignedSkill()
        {
            if (assignedSkill != null && assignedSkill.IsUnlocked && !assignedSkill.IsOnCooldown)
            {
                assignedSkill.Execute(null, new List<Skill>());
            }
        }

        public bool IsKeyTriggered()
        {
            return Input.GetKeyDown(activationKey);
        }
    }
}
