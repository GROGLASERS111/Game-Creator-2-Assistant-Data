using System;
using System.Threading.Tasks;
using ETK;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Reset Skills")]
    [Description("Reset all Skills and refund points")]

    [Category("Skills/Reset Skills")]

    [Keywords("Reset", "Skills", "Points")]
    [Image(typeof(IconAbsolute), ColorTheme.Type.Blue)]

    [Serializable]
    public class InstructionResetSkills : Instruction
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Reset all Skills and refund points";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            if (SkillsManager.Instance != null)
            {
                SkillsManager.Instance.ResetAndRefundSkills();
            }
            else
            {
                Debug.LogError("SkillsManager instance not found");
            }

            return DefaultResult;
        }
    }
}
