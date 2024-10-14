using System;
using System.Threading.Tasks;
using ETK;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Save Skills")]
    [Description("Save all Skills")]

    [Category("Skills/Save Skills states")]

    [Keywords("Save", "Skills", "Points")]
    [Image(typeof(IconAbsolute), ColorTheme.Type.Blue)]

    [Serializable]
    public class InstructionSaveSkills : Instruction
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Save all Skills states";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            if (SkillsManager.Instance != null)
            {
                SkillsSaveData saveData = SkillsManager.Instance.GetSkillsSaveData();

                SaveLoadManagerSkills.Instance.SaveSkills(saveData);
            }
            else
            {
                Debug.LogError("SkillsManager instance not found");
            }

            return DefaultResult;
        }
    }
}
