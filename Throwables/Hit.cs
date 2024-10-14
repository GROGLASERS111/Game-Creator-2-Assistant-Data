using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using ETK;

namespace ETK
{
    public class Hit : MonoBehaviour
    {
        [SerializeField]
        private ThrowableInstructionPair[] throwableInstructionPairs;

        public void RunInstructions(string throwableID)
        {
            foreach (var pair in throwableInstructionPairs)
            {
                if (!string.IsNullOrEmpty(pair.requiredThrowableID) && throwableID == pair.requiredThrowableID)
                {
                    Debug.Log("Condition passed for ID: " + pair.requiredThrowableID + ". Running onHit instructions.");
                    _ = pair.onHit.Run(new Args(this.gameObject));
                }
            }
        }

        public void OnHit(Throwable throwable)
        {
            if (throwable != null)
            {
                RunInstructions(throwable.throwableID);
            }
        }
    }
}
