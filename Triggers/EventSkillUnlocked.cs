using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using ETK;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("On Skill Unlocked")]
    [Category("Skills/On Skill Unlocked")]
    [Description("Executed when a specified skill is unlocked")]
    [Image(typeof(IconGear), ColorTheme.Type.Green)]
    [Keywords("Skill", "Unlock", "Trigger")]
    [Serializable]
    public class EventSkillUnlocked : Event
    {
        [SerializeField] private string skillTitle;
        private bool skillWasUnlocked = false;

        protected override void OnUpdate(Trigger trigger)
        {
            base.OnUpdate(trigger);

            ETK.Skill skill = SkillsManager.Instance.skills.Find(s => s.Title == skillTitle);
            if (skill != null && skill.IsUnlocked && !skillWasUnlocked)
            {
                _ = trigger.Execute(this.Self);
                skillWasUnlocked = true;
            }
        }
    }
}
