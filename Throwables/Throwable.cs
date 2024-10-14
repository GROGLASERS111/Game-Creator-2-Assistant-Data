using UnityEngine;
using GameCreator.Runtime.Common; 
using GameCreator.Runtime.VisualScripting;

namespace ETK
{
    public class Throwable : MonoBehaviour
    {
        public string throwableID;
        public float sphereRadius = 1f;
        public LayerMask collisionLayers;
        public Vector3 colliderOffset;
        public bool hasCollided = false;

        [SerializeField] public InstructionList onHit = new InstructionList();

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + colliderOffset, sphereRadius);
        }

        private void Update()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + colliderOffset, sphereRadius, collisionLayers);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject != this.gameObject)
                {
                    hasCollided = true;

                    Hit hitComponent = hitCollider.GetComponent<Hit>();
                    if (hitComponent != null)
                    {
                        hitComponent.OnHit(this);
                    }

                    RunInstructions();
                    break;
                }
            }
        }

        public void RunInstructions()
        {
            _ = onHit.Run(new Args(this.gameObject));
        }
    }
}
