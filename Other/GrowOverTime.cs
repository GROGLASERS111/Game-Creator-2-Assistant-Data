using System;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

namespace ETK
{
    public class GrowOvertime : MonoBehaviour
    {
        public float minSize = 1f;
        public float maxSize = 3f;
        public float resizeDuration = 5f;
        private bool atMaxSize = false;
        private Transform meshTransform;
        private GameObject instantiatedObject;

        [NonSerialized] private IInteractive m_Interactive;

        [SerializeField] public InstructionList onRun = new InstructionList();
        public GameObject canvas;


        private void Start()
        {
            meshTransform = this.transform;
            StartCoroutine(GrowMesh());

            InteractionTracker tracker = InteractionTracker.Require(this.gameObject);
            this.m_Interactive = tracker;

            tracker.EventInteract -= this.OnInteract;
            tracker.EventInteract += this.OnInteract;
        }

        private IEnumerator GrowMesh()
        {
            float timer = 0;
            while (timer < resizeDuration)
            {
                timer += Time.deltaTime;
                float scale = Mathf.Lerp(minSize, maxSize, timer / resizeDuration);
                meshTransform.localScale = new Vector3(scale, scale, scale);
                yield return null;
            }
            atMaxSize = true;
        }

        private IEnumerator ResetSize()
        {
            meshTransform.localScale = new Vector3(minSize, minSize, minSize);
            atMaxSize = false;
            yield return new WaitForSeconds(1);
            StartCoroutine(GrowMesh());
        }

        private void OnInteract(Character character, IInteractive interactive)
        {
            this.RunInteract(character, interactive);
        }

        public async void RunInteract(Character character, IInteractive interactive)
        {
            if (!atMaxSize) return;
            {
                await this.onRun.Run(new Args(this.gameObject));
                StartCoroutine(ResetSize());
            }
            this.m_Interactive?.Stop();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!atMaxSize) return;

            if (other.gameObject.CompareTag("Player") && canvas != null)
            {
                instantiatedObject = Instantiate(canvas, transform.position, Quaternion.identity);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && instantiatedObject != null)
            {
                Destroy(instantiatedObject);
            }
        }
    }
}