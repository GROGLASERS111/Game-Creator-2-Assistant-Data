using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using ETK;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Is Skill Unlocked")]
    [Description("Returns true if the specified skill is unlocked")]

    [Category("Skills/Is Skill Unlocked")]

    [Parameter("Skill Title", "The title of the skill to check")]

    [Keywords("Skills", "Unlocked", "Condition")]

    [Image(typeof(IconCubeSolid), ColorTheme.Type.Blue)]
    [Serializable]
    public class ConditionSkillUnlocked : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private string skillTitle;

        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string Summary => $"Is skill '{skillTitle}' unlocked";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            if (SkillsManager.Instance == null) return false;

            ETK.Skill skill = SkillsManager.Instance.skills.Find(s => s.Title == skillTitle);
            if (skill == null) return false;

            return skill.IsUnlocked;
        }
    }
}
