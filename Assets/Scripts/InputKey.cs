using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Note: InputKey movement is done in local space. This is because local space is relative to the canvas so is affected by the canvas scaler. 


[RequireComponent(typeof(RectTransform))]
public class InputKey : MonoBehaviour
{
    [SerializeField]
    private KeyCode keyCode;
    public KeyCode GetKeyCode() { return keyCode; }

    private InputSlot inputSlot;
    public InputSlot GetInputSlot() { return inputSlot; }
    public void SetInputSlot(InputSlot _inputSlot) { inputSlot = _inputSlot; }

    private RectTransform rectTransform;
    private Vector3 velocity = Vector3.zero;
    public void SetVelocity(Vector3 _velocity) { velocity = _velocity; }

    // These shouldn't be modified per prefab, so I'm using consts for now.
    private const float snapSpeed = 700;
    private const float drag = 12;


    private void Awake()
    {
        Text text = GetComponentInChildren<Text>();
        if (text != null)
        {
            text.text = keyCode.ToString();
        }

        rectTransform = GetComponent<RectTransform>();
    }


    public bool ContainsPoint(Vector3 point)
    {
        point = rectTransform.InverseTransformPoint(point);
        return
            point.x > rectTransform.rect.xMin &&
            point.x < rectTransform.rect.xMax &&
            point.y > rectTransform.rect.yMin &&
            point.y < rectTransform.rect.yMax;
    }

    private void Update()
    {
        // Apply velocity
        transform.localPosition += Time.deltaTime * velocity;

        // Apply drag
        velocity -= velocity * Mathf.Clamp01(Time.deltaTime * drag); // Clamp01 prevents low framerates causing it to reverse direction

        // Keep within screen. Might find a nicer way to do this
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, Screen.width), Mathf.Clamp(transform.position.y, 0, Screen.height), 0);

        // Move towards the inputSlot, if one is assigned
        if (inputSlot != null)
        {
            if (transform.localPosition != inputSlot.transform.localPosition)
            {
                float maxMoveDistance = 500 * Time.deltaTime;
                Vector3 distanceToSlot = inputSlot.transform.localPosition - transform.localPosition;
                if (distanceToSlot.magnitude <= maxMoveDistance)
                {
                    transform.localPosition = inputSlot.transform.localPosition;
                }
                else
                {
                    transform.localPosition += distanceToSlot.normalized * maxMoveDistance;
                }
            }
        }
    }
}
