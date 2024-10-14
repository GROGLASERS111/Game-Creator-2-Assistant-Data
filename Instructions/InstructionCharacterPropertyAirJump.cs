using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Change Air Jumps")]
    [Description("Changes the Character's number of air jumps")]

    [Category("Characters/Properties/Change Air Jumps")]
    
    [Parameter("Jump Force", "The target Jump Force value for the Character")]
    [Parameter("Duration", "How long it will take to perform the transition")]
    [Parameter("Easing", "The change rate of the parameter over time")]
    [Parameter("Wait to Complete", "Whether to wait until the transition is finished")]

    [Keywords("Hop", "Build", "Wind", "Fly")]
    [Image(typeof(IconBust), ColorTheme.Type.Yellow)]

    [Serializable]
    public class InstructionCharacterPropertyAirJump : TInstructionCharacterProperty
	{

		// MEMBERS: -------------------------------------------------------------------------------

		[SerializeField]
		private PropertyGetInteger m_Jumps = new PropertyGetInteger(1);

		// PROPERTIES: ----------------------------------------------------------------------------

		public override string Title => $"Change Air Jumps to {this.m_Jumps} on {this.m_Character}";

		// RUN METHOD: ----------------------------------------------------------------------------

		protected override Task Run(Args args)
		{
			Character character = this.m_Character.Get<Character>(args);
			if (character == null) return DefaultResult;

			character.Motion.AirJumps = (int)Math.Floor(this.m_Jumps.Get(args));

			return DefaultResult;
		}
	}
}