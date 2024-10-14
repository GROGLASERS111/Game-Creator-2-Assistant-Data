using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;
using ETK;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Throw Object")]
    [Description("Throws an object in a straight line and optionally returns it back to its origin")]

    [Image(typeof(IconRotation), ColorTheme.Type.Yellow)]

    [Category("Skills/Throw Object")]

    [Parameter("Object To Throw", "The game object to be thrown")]
    [Parameter("Throw Distance", "The distance the object is thrown")]
    [Parameter("Throw Duration", "The duration over which the object is thrown")]
    [Parameter("Use Gravity", "Whether to use gravity when throwing the object")]
    [Parameter("Throw Force", "The force with which the object is thrown")]

    [Keywords("Throw", "Toss", "Move", "Translate")]

    [Serializable]
    public class InstructionThrow : Instruction
    {
        [SerializeField] private PropertyGetInstantiate m_ThrowObject = new PropertyGetInstantiate();
        [Space]
        [SerializeField] private float m_ThrowDistance = 10f;
        [SerializeField] private float m_ThrowDuration = 1f;
        [SerializeField] private bool m_UseGravity = false;
        [SerializeField] private float m_ThrowForce = 10f;
        [SerializeField] private bool m_ReturnToOrigin = false;
        [Space]
        [SerializeField] private PropertyGetPosition m_Position = GetPositionCharactersPlayer.Create;
        [SerializeField] private PropertyGetRotation m_Rotation = GetRotationCharactersPlayer.Create;
        [Space]

        private Transform originalParent;
        private Vector3 originalPosition;

        public override string Title => $"Throw object";

        protected override async Task Run(Args args)
        {
            Vector3 position = this.m_Position.Get(args);
            Quaternion rotation = this.m_Rotation.Get(args);

            GameObject throwObjectInstance = this.m_ThrowObject.Get(args, position, rotation);
            if (throwObjectInstance == null) return;

            originalParent = throwObjectInstance.transform.parent;
            originalPosition = throwObjectInstance.transform.position;

            Rigidbody rb = throwObjectInstance.GetComponent<Rigidbody>();
            if (rb == null) rb = throwObjectInstance.AddComponent<Rigidbody>();

            rb.useGravity = m_UseGravity;
            rb.isKinematic = false;

            Vector3 throwDirection = (Camera.main.transform.forward * m_ThrowDistance).normalized;
            rb.AddForce(throwDirection * m_ThrowForce, ForceMode.Impulse);

            await Task.Delay(TimeSpan.FromSeconds(m_ThrowDuration));

            if (throwObjectInstance != null && m_ReturnToOrigin)
            {
                // Check if the object has not collided
                Throwable throwableComponent = throwObjectInstance.GetComponent<Throwable>();
                if (throwableComponent != null && !throwableComponent.hasCollided)
                {
                    throwableComponent.RunInstructions();
                }

                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
                throwObjectInstance.transform.parent = originalParent;
                throwObjectInstance.transform.position = originalPosition;
            }
        }
    }
}
