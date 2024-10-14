using System;
using System.Threading.Tasks;
using ETK;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Teleport Skill")]
    [Description("Activates the Teleport Skill")]

    [Category("Skills/Teleport Skill")]

    [Parameter("Location", "The position where the Player is teleported")]

    [Keywords("Change", "Position", "Location", "Respawn", "Spawn")]
    [Image(typeof(IconCharacter), ColorTheme.Type.Blue)]

    [Serializable]
    public class InstructionTeleportSkill : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        public static TeleportationSkill Instance { get; private set; }

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Teleport the Player";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            var player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                var teleportSkill = player.GetComponent<TeleportationSkill>();
                if (teleportSkill != null)
                {
                    teleportSkill.EnableTeleportation();
                }
            }
            return DefaultResult;
        }
    }
}