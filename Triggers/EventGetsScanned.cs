using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using ETK;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("On Get Scanned")]
    [Category("Skills/On Get Scanned")]
    [Description("Executed when an object collides with the particle mesh")]
    [Image(typeof(IconGear), ColorTheme.Type.Green)]
    [Keywords("Particle", "Collision", "Trigger")]
    [Serializable]
    public class EventParticleMeshCollision : Event
    {
        [SerializeField] private GameObject particleMeshObject;
        private bool isCollisionDetected = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == particleMeshObject)
            {
                isCollisionDetected = true;
            }
        }

        protected override void OnUpdate(Trigger trigger)
        {
            base.OnUpdate(trigger);

            if (isCollisionDetected)
            {
                _ = trigger.Execute(this.Self);
                isCollisionDetected = false;
            }
        }
    }
}
