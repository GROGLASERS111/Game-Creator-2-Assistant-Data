using UnityEngine;
using UnityEngine.UI;

namespace ETK
{
    public class HotbarMirror : MonoBehaviour
    {
        public HotbarSlot[] targetHotbarSlots;

        private void Update()
        {
            if (Hotbar.Instance == null || Hotbar.Instance.slots == null)
                return;

            for (int i = 0; i < Hotbar.Instance.slots.Length; i++)
            {
                if (i < targetHotbarSlots.Length)
                {
                    CopySlot(Hotbar.Instance.slots[i], targetHotbarSlots[i]);
                }
            }
        }

        private void CopySlot(HotbarSlot source, HotbarSlot target)
        {
            if (source != null && target != null)
            {
                Skill sourceSkill = SkillsManager.Instance.skills.Find(s => s.Title == source.assignedSkillTitle);
                if (sourceSkill != null)
                {
                    target.AssignSkill(sourceSkill);

                    if (source.skillIcon != null && target.skillIcon != null)
                    {
                        target.skillIcon.sprite = source.skillIcon.sprite;
                        target.skillIcon.enabled = source.skillIcon.enabled;
                    }

                    if (source.cooldownImage != null && target.cooldownImage != null && sourceSkill.IsOnCooldown)
                    {
                        float cooldownElapsed = Time.time - sourceSkill.LastUsedTime;
                        float cooldownDuration = sourceSkill.CooldownDuration;
                        target.cooldownImage.fillAmount = cooldownElapsed / cooldownDuration;
                    }
                    else
                    {
                        target.cooldownImage.fillAmount = 1;
                    }
                }
            }
        }

    }
}
