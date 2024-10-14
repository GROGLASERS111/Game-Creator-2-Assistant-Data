using UnityEngine;

namespace ETK
{
    public class HandIKControl : MonoBehaviour
    {
        protected Animator animator;

        public bool ikActive = false;
        public Transform rightHandObj = null;
        public Transform leftHandObj = null;

        void Start()
        {
            animator = GetComponentInChildren<Animator>();

            if (!animator)
            {
                Debug.LogWarning("Animator component not found in child objects.");
            }
        }

        void OnAnimatorIK()
        {
            if (animator)
            {
                if (ikActive)
                {
                    if (rightHandObj != null)
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                    }

                    if (leftHandObj != null)
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
                        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
                    }
                }
                else
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                }
            }
        }
    }
}