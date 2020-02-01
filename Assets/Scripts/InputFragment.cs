using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFragment : MonoBehaviour
{   
    // These shouldn't be modified per prefab, so I'm using consts for now.
    private const float breakDuration = 2;


    public void Break(Vector3 breakVelocity, float breakRotationSpeed) { StartCoroutine(BreakCoroutine(breakVelocity, breakRotationSpeed)); }
    private IEnumerator BreakCoroutine(Vector3 breakVelocity, float breakRotationSpeed)
    {
        Vector3 velocity = breakVelocity;

        float f = 0;
        while (f < 1)
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
