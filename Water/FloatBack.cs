using UnityEngine;
using System.Collections;
using GameCreator.Runtime.Common;

public class FloatBack : MonoBehaviour
{
    [SerializeField] private PropertyGetGameObject targetObjectProperty = new PropertyGetGameObject();
    public float lowerYValue = 17.6f; // Set the lower Y value
    public float upperYValue = 17.8f; // Set the upper Y value
    public float moveSpeed = 20f;     // Set the speed of moving the object

    private float middleYValue;       // Calculated middle Y value
    private bool isObjectBeingCorrected = false;

    void Start()
    {
        // Calculate the middle Y value
        middleYValue = (lowerYValue + upperYValue) / 2f;
    }

    void Update()
    {
        GameObject targetGameObject = targetObjectProperty.Get(gameObject);
        if (targetGameObject != null && targetGameObject.transform.position.y < lowerYValue && !isObjectBeingCorrected)
        {
            isObjectBeingCorrected = true;
            StartCoroutine(MoveObjectToMiddle(targetGameObject));
        }
    }

    IEnumerator MoveObjectToMiddle(GameObject targetGameObject)
    {
        while (targetGameObject.transform.position.y < middleYValue)
        {
            Vector3 targetPosition = new Vector3(targetGameObject.transform.position.x, middleYValue, targetGameObject.transform.position.z);
            targetGameObject.transform.position = Vector3.MoveTowards(targetGameObject.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isObjectBeingCorrected = false;
    }
}
