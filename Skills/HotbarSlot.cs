using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ETK
{
    public class HotbarSlot : MonoBehaviour, IDropHandler
    {
        public Image skillIcon;
        public Image cooldownImage;
        public KeyCode activationKey = KeyCode.None;
        public string assignedSkillTitle;

        private void OnEnable()
        {
            SkillsManager.OnSkillReset += ClearAssignedSkill;
        }

        private void OnDisable()
        {
            SkillsManager.OnSkillReset -= ClearAssignedSkill;
        }

        private void ClearAssignedSkill()
        {
            AssignSkill(null);
        }

        private void Update()
        {
            Skill assignedSkill = SkillsManager.Instance.skills.Find(s => s.Title == assignedSkillTitle);
            if (assignedSkill != null && assignedSkill.IsOnCooldown)
            {
                float cooldownElapsed = Time.time - assignedSkill.LastUsedTime;
                float cooldownDuration = assignedSkill.CooldownDuration;
                cooldownImage.fillAmount = cooldownElapsed / cooldownDuration;
            }
            else
            {
                cooldownImage.fillAmount = 1;
            }

            if (Input.GetKeyDown(activationKey))
            {
                ExecuteAssignedSkill();
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            SkillButtonUI draggedSkillButton = eventData.pointerDrag.GetComponent<SkillButtonUI>();
            if (draggedSkillButton != null && draggedSkillButton.skill.IsUnlocked)
            {
                AssignSkill(draggedSkillButton.skill);
            }
        }

        public void AssignSkill(Skill skill)
        {
            if (skill != null)
            {
                assignedSkillTitle = skill.Title;
                if (skillIcon != null)
                {
                    skillIcon.sprite = skill.IconImage;
                    skillIcon.enabled = true;
                }
            }
            else
            {
                assignedSkillTitle = null;
                if (skillIcon != null)
                {
                    skillIcon.sprite = null;
                    skillIcon.enabled = false;
                }
            }
        }

        public void ExecuteAssignedSkill()
        {
            SkillsManager.Instance.ExecuteSkill(assignedSkillTitle, gameObject);
        }
    }
}
