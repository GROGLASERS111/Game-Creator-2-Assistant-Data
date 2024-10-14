using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine.UI;

namespace ETK
{
    public class ViewCone : MonoBehaviour
    {
        public float viewConeHeight = 0.6f;
        public float viewConeWidth = 45f;
        public float viewConeLength = 10f;
        public float buildUpTimeToDetect = 2.0f;
        public float buildUpTimeToLose = 1.0f;

        public GameObject detectionUI;
        public Image detectionProgressImage;
        public CanvasGroup spottedUIGroup;

        [SerializeField] public InstructionList onView = new InstructionList();
        [SerializeField] public InstructionList stopView = new InstructionList();

        private bool isPlayerInView = false;
        private float currentDetectionTime = 0f;
        private float currentLoseTime = 0f;

        void Start()
        {
            detectionUI.SetActive(false);
            spottedUIGroup.alpha = 0;
        }

        void Update()
        {
            CheckForPlayerInView();
            UpdateDetectionUI();
        }

        void UpdateDetectionUI()
        {
            if (currentDetectionTime > 0 && currentDetectionTime < buildUpTimeToDetect)
            {
                detectionUI.SetActive(true);
                detectionProgressImage.fillAmount = currentDetectionTime / buildUpTimeToDetect;
            }
            else if (currentDetectionTime >= buildUpTimeToDetect)
            {
                detectionUI.SetActive(false);
                spottedUIGroup.alpha = 1;
            }
            else
            {
                detectionUI.SetActive(false);
                spottedUIGroup.alpha = 0;
            }
        }

        public void ResetSpottedUI()
        {
            spottedUIGroup.alpha = 0;
        }

        void CheckForPlayerInView()
        {
            Vector3 forward = transform.forward;
            Vector3 origin = new Vector3(transform.position.x, transform.position.y + viewConeHeight, transform.position.z);

            bool playerFound = false;

            int steps = 30;
            for (int i = 0; i <= steps; i++)
            {
                float angle = i * (viewConeWidth / steps) - viewConeWidth / 2;
                Vector3 direction = Quaternion.Euler(0, angle, 0) * forward;

                Debug.DrawRay(origin, direction * viewConeLength, Color.green, 1.0f);

                if (Physics.Raycast(origin, direction, out RaycastHit hitInfo, viewConeLength))
                {
                    if (hitInfo.collider.CompareTag("Player"))
                    {
                        detectionUI.SetActive(true);
                        playerFound = true;
                        break;
                    }
                }
            }

            if (playerFound)
            {
                currentLoseTime = 0f;
                currentDetectionTime += Time.deltaTime;

                if (currentDetectionTime >= buildUpTimeToDetect && !isPlayerInView)
                {
                    isPlayerInView = true;
                    RunInstructions();
                }
            }
            else
            {
                if (currentDetectionTime > 0 && isPlayerInView)
                {
                    currentLoseTime += Time.deltaTime;
                    if (currentLoseTime >= buildUpTimeToLose)
                    {
                        isPlayerInView = false;
                        RunInstructionsStop();
                    }
                }
                else
                {
                    currentDetectionTime = 0f;
                }
            }
        }

        public void RunInstructions()
        {
            _ = onView.Run(new Args(this.gameObject));
        }

        public void RunInstructionsStop()
        {
            _ = stopView.Run(new Args(this.gameObject));
        }

        void OnDrawGizmosSelected()
        {
            Vector3 forward = transform.forward;
            Vector3 origin = new Vector3(transform.position.x, transform.position.y + viewConeHeight, transform.position.z);

            Gizmos.color = Color.red;

            Vector3 leftBoundary = Quaternion.Euler(0, -viewConeWidth / 2, 0) * forward;
            Gizmos.DrawRay(origin, leftBoundary * viewConeLength);

            Vector3 rightBoundary = Quaternion.Euler(0, viewConeWidth / 2, 0) * forward;
            Gizmos.DrawRay(origin, rightBoundary * viewConeLength);

            int steps = 30;
            for (int i = 0; i <= steps; i++)
            {
                float angle = i * (viewConeWidth / steps) - viewConeWidth / 2;
                Vector3 direction = Quaternion.Euler(0, angle, 0) * forward;
                Gizmos.DrawRay(origin, direction * viewConeLength);
            }
        }
    }
}

