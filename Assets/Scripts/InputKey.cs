using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 


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

    private bool breaking = false;

    // These shouldn't be modified per prefab, so I'm using consts for now.
    private const float snapLerpSpeed = 30;
    private const float maxSnapLerp = 0.7f;
    private const float drag = 12;
    private const float breakDuration = 2;


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
        if (breaking) { return; }

        // Apply velocity
        transform.position += CanvasScale.Instance.CanvasToWorld( velocity * Time.deltaTime);

        // Apply drag
        velocity -= velocity * Mathf.Clamp01(Time.deltaTime * drag); // Clamp01 prevents low framerates causing it to reverse direction

        // Keep within screen. Might find a nicer way to do this
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, Screen.width), Mathf.Clamp(transform.position.y, 0, Screen.height), 0);

        // Move towards the inputSlot, if one is assigned
        if (inputSlot != null)
        {
            // Make it scale by deltaTime, but limit it in case the fps is really low.
            float lerpAmount = Mathf.Clamp(Time.deltaTime * snapLerpSpeed, 0, maxSnapLerp);
            transform.position = Vector3.Lerp(transform.position, inputSlot.transform.position, lerpAmount);
        }
    }


    public void Break(Vector3 breakVelocity, float breakRotationSpeed) { StartCoroutine(BreakCoroutine(breakVelocity, breakRotationSpeed)); }
    private IEnumerator BreakCoroutine(Vector3 breakVelocity, float breakRotationSpeed)
    {
        breaking = true;
        velocity = breakVelocity;
 
        float f = 0;
        while(f<1)
        {
            f += Time.deltaTime / breakDuration;
            if (f > 1) { f = 1; }

            velocity += Vector3.down * 2000 * Time.deltaTime;
            transform.position += CanvasScale.Instance.CanvasToWorld(velocity * Time.deltaTime);
            transform.localRotation *= Quaternion.Euler(0, 0, breakRotationSpeed * Time.deltaTime);

            yield return null;
        }

        Destroy(gameObject);
    }
}
