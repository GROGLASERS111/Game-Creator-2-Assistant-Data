using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;
using ETK;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Toggle Hand IK")]
    [Description("Enables or disables Hand IK and sets hand target positions")]

    [Category("Car/Toggle Hand IK")]

    [Keywords("Hand", "IK", "Enable", "Disable", "Position")]

    [Image(typeof(IconIK), ColorTheme.Type.Blue)]

    [Serializable]
    public class InstructionToggleHandIK : Instruction
    {
        [SerializeField] private bool toggleHandIK = true;
        [SerializeField] private PropertyGetGameObject m_GameObject = new PropertyGetGameObject();
        [SerializeField] private PropertyGetGameObject m_RightHandObj = new PropertyGetGameObject();
        [SerializeField] private PropertyGetGameObject m_LeftHandObj = new PropertyGetGameObject();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"{(toggleHandIK ? "Enable" : "Disable")} Hand IK for {this.m_GameObject}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async Task Run(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            if (gameObject == null) return;

            HandIKControl handIK = gameObject.GetComponent<HandIKControl>();
            if (handIK != null)
            {
                handIK.ikActive = toggleHandIK;

                GameObject rightHandObj = this.m_RightHandObj.Get(args);
                if (rightHandObj != null) handIK.rightHandObj = rightHandObj.transform;

                GameObject leftHandObj = this.m_LeftHandObj.Get(args);
                if (leftHandObj != null) handIK.leftHandObj = leftHandObj.transform;

                await Task.Delay(10);
            }
        }
    }
}
