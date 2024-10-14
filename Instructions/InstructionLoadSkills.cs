using System;
using System.Threading.Tasks;
using ETK;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Load Skills")]
    [Description("Load all Skills states")]

    [Category("Skills/Load Skills")]

    [Keywords("Load", "Skills", "Points")]
    [Image(typeof(IconAbsolute), ColorTheme.Type.Blue)]

    [Serializable]
    public class InstructionLoadSkills : Instruction
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Load all Skills states";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            if (SkillsManager.Instance != null)
            {
                SkillsSaveData saveData = SaveLoadManagerSkills.Instance.LoadSkills();

                if (saveData != null)
                {
                    SkillsManager.Instance.ApplySkillsSaveData(saveData);
                }
                else
                {
                    Debug.LogError("Failed to load skills data");
                }
            }
            else
            {
                Debug.LogError("SkillsManager instance not found");
            }

            return DefaultResult;
        }
    }
}
