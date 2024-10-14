using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GroupManager;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Set Group State")]
    [Description("Set which bool is set on a character")]

    [Category("Characters/Set Group State")]

    [Parameter("Character", "The character to modify")]
    [Parameter("State", "Which state to change")]
    [Parameter("Value", "The value to set the state to")]

    [Keywords("Group", "Character", "State")]
    [Image(typeof(IconBust), ColorTheme.Type.Yellow)]

    [Serializable]
    public class InstructionGroup : Instruction
    {
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        [SerializeField] private States m_State;
        [SerializeField] private bool m_Value;

        public enum States
        {
            IsDead,
            IsMoving,
            IsAttacking
        }

        public override string Title => $"Set {m_State} to {m_Value} on {this.m_Character}";

        protected override async Task Run(Args args)
        {
            GameObject target = this.m_Character.Get(args);
            if (target == null) return;

            EnemyCharacter character = target.GetComponent<EnemyCharacter>();
            if (character == null) return;

            switch (m_State)
            {
                case States.IsDead:
                    character.SetDead(m_Value);
                    break;
                case States.IsMoving:
                    character.SetMoving(m_Value);
                    break;
                case States.IsAttacking:
                    character.SetAttacking(m_Value);
                    break;
            }

            await Task.Yield();
        }
    }
}
