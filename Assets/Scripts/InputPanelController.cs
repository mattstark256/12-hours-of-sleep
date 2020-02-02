using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPanelController : MonoBehaviour
{
    [SerializeField]
    private Transform keyParent;
    [SerializeField]
    private Transform fragmentParent;
    [SerializeField]
    private Transform keySpawnPoint;
    [SerializeField]
    private Image dropShadow;
    [SerializeField]
    private Vector3 dropShadowOffset;
    [SerializeField]
    private float snapDistance = 50;

    [SerializeField]
    private List<InputKey> keysToBreak;
    [SerializeField]
    private List<InputFragment> fragmentsToBreak;

    private List<InputKey> inputKeys;
    private List<InputSlot> inputSlots;
    private List<InputFragment> inputFragments;

    private bool draggingEnabled = false;

    private bool dragging = false;
    private InputKey draggedKey;
    private Vector3 dragOffset;

    private void Start()
    {
        inputKeys = new List<InputKey>(keyParent.GetComponentsInChildren<InputKey>());
        inputSlots = new List<InputSlot>(fragmentParent.GetComponentsInChildren<InputSlot>());
        inputFragments = new List<InputFragment>(fragmentParent.GetComponentsInChildren<InputFragment>());

        dropShadow.enabled = false;


        // Insert each key into the nearest slot
        foreach (InputKey inputKey in inputKeys)
        {
            float shortestDistance = 0;
            InputSlot nearestSlot = null;
            foreach (InputSlot inputSlot in inputSlots)
            {
                float distance = Vector3.Distance(inputKey.transform.position, inputSlot.transform.position);
                if (nearestSlot == null || distance < shortestDistance)
                {
                    nearestSlot = inputSlot;
                    shortestDistance = distance;
                }
            }
            nearestSlot.SetInputKey(inputKey);
            inputKey.SetInputSlot(nearestSlot);
            inputKey.transform.position = nearestSlot.transform.position;
            InputMapper.Instance.AddMapping(nearestSlot.GetAction(), inputKey.GetKeyCode());
        }
    }


    private void Update()
    {
        if (draggingEnabled)
        {
            if (!dragging)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    InputKey topKey = null;
                    int topKeySiblingIndex = -1;

                    foreach (InputKey inputKey in inputKeys)
                    {
                        if (inputKey.ContainsPoint(Input.mousePosition) && inputKey.transform.GetSiblingIndex() > topKeySiblingIndex)
                        {
                            topKey = inputKey;
                            topKeySiblingIndex = inputKey.transform.GetSiblingIndex();
                        }
                    }

                    if (topKey != null)
                    {
                        StartDragging(topKey);
                    }
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    ContinueDragging();
                }
                else
                {
                    StopDragging();
                }
            }
        }
    }



    private void StartDragging(InputKey inputKey)
    {
        dragging = true;
        draggedKey = inputKey;
        dragOffset = draggedKey.transform.position - Input.mousePosition;
        dropShadow.transform.SetAsLastSibling();
        inputKey.transform.SetAsLastSibling();
        inputKey.SetVelocity(Vector3.zero);

        // Check if it's being removed from a slot
        InputSlot oldInputSlot = draggedKey.GetInputSlot();
        if (oldInputSlot != null)
        {
            oldInputSlot.SetInputKey(null);
            draggedKey.SetInputSlot(null);
            InputMapper.Instance.RemoveMapping(oldInputSlot.GetAction());
        }

        dropShadow.enabled = true;
        dropShadow.transform.position = draggedKey.transform.position + CanvasScale.Instance.CanvasToWorld(dropShadowOffset);
    }


    private void ContinueDragging()
    {
        draggedKey.transform.position = Input.mousePosition + dragOffset;
        dropShadow.transform.position = draggedKey.transform.position + CanvasScale.Instance.CanvasToWorld(dropShadowOffset);
    }


    private void StopDragging()
    {
        dragging = false;
        dropShadow.enabled = false;

        // Find the nearest valid slot
        InputSlot nearestSlot = null;
        float shortestDistance = 0;
        foreach (InputSlot inputSlot in inputSlots)
        {
            float distance = CanvasScale.Instance.WorldToCanvas(Vector3.Distance(draggedKey.transform.position, inputSlot.transform.position));
            if (distance < snapDistance && (nearestSlot == null || distance < shortestDistance))
            {
                nearestSlot = inputSlot;
                shortestDistance = distance;
            }
        }

        if (nearestSlot != null)
        {
            // Remove the key that was occupying the slot
            InputKey replacedKey = nearestSlot.GetInputKey();
            if (replacedKey != null)
            {
                replacedKey.SetInputSlot(null);

                // Fire off in a random direction
                //float randomAngle = Random.Range(0, 360);
                //replacedKey.SetVelocity(1000 * new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0));

                // Fire off away from the dragged key
                replacedKey.SetVelocity(1000 * (replacedKey.transform.position - draggedKey.transform.position).normalized);

                replacedKey.transform.SetAsLastSibling();
                InputMapper.Instance.RemoveMapping(nearestSlot.GetAction());
            }

            nearestSlot.SetInputKey(draggedKey);
            draggedKey.SetInputSlot(nearestSlot);
            draggedKey.transform.SetAsFirstSibling();
            InputMapper.Instance.AddMapping(nearestSlot.GetAction(), draggedKey.GetKeyCode());
        }
    }


    public void Break()
    {
        foreach (InputKey inputKey in keysToBreak)
        {
            BreakKey(inputKey);
        }

        foreach (InputFragment inputFragment in fragmentsToBreak)
        {
            BreakFragment(inputFragment);
        }

        draggingEnabled = true;

        if (CameraEffects.Instance != null)
        {
        CameraEffects.Instance.AddScreenShakeAndChromaticAberration(1);

        }

        // Unused alternative animation
        //StartCoroutine(BreakCoroutine());
    }


    // Do an animation where they break one by one
    //private IEnumerator BreakCoroutine()
    //{
    //    while (keysToBreak.Count > 0 || fragmentsToBreak.Count > 0)
    //    {
    //        bool itemBroken = false;
    //        if (keysToBreak.Count > 0 && Random.value < 0.5)
    //        {
    //            InputKey inputKey = keysToBreak[Random.Range(0, keysToBreak.Count)];
    //            keysToBreak.Remove(inputKey);
    //            BreakKey(inputKey);
    //            itemBroken = true;
    //        }
    //        else
    //        {
    //            InputFragment inputFragment = fragmentsToBreak[Random.Range(0, fragmentsToBreak.Count)];
    //            // Only break a fragment once any keys in it have been broken
    //            bool fragmentContainsKeys = false;
    //            foreach (InputSlot inputSlot in inputFragment.GetComponentsInChildren<InputSlot>())
    //            {
    //                if (inputSlot.GetInputKey() != null)
    //                {
    //                    fragmentContainsKeys = true;
    //                    break;
    //                }
    //            }
    //            if (!fragmentContainsKeys)
    //            {
    //                fragmentsToBreak.Remove(inputFragment);
    //                BreakFragment(inputFragment);
    //                itemBroken = true;
    //            }
    //        }
    //        if (itemBroken) { yield return new WaitForSeconds(0.2f); }
    //    }
    //}


    private void BreakKey(InputKey inputKey)
    {
        if (dragging && draggedKey == inputKey)
        {
            StopDragging();
        }

        Vector3 positionVector = inputKey.transform.position - keyParent.transform.position;
        positionVector = CanvasScale.Instance.WorldToCanvas(positionVector);
        positionVector.x *= 2;
        positionVector.y *= 4;
        inputKey.Break(
            positionVector + (Vector3)Random.insideUnitCircle * 500f,
            Random.Range(-360, 360));
        InputSlot inputSlot = inputKey.GetInputSlot();
        if (inputSlot != null)
        {
            inputSlot.SetInputKey(null);
            InputMapper.Instance.RemoveMapping(inputSlot.GetAction());
        }
        inputKey.SetInputSlot(null);
        inputKeys.Remove(inputKey);
    }


    private void BreakFragment(InputFragment inputFragment)
    {
        Vector3 positionVector = inputFragment.transform.position - fragmentParent.transform.position;
        positionVector = CanvasScale.Instance.WorldToCanvas(positionVector);
        positionVector.x *= 2;
        positionVector.y *= 4;
        inputFragment.Break(
            positionVector + (Vector3)Random.insideUnitCircle * 500f,
            Random.Range(-360, 360));
        inputFragments.Remove(inputFragment);
        foreach (InputSlot inputSlot in inputFragment.GetComponentsInChildren<InputSlot>())
        {
            inputSlots.Remove(inputSlot);
            InputMapper.Instance.RemoveMapping(inputSlot.GetAction());
        }
    }


    public void AddKey(KeyPickup keyPickup) { StartCoroutine(AddKeyCoroutine(keyPickup)); }
    private IEnumerator AddKeyCoroutine(KeyPickup keyPickup)
    {
        InputKey newKey = Instantiate(keyPickup.GetKeyPrefab(), keyParent);
        newKey.transform.SetAsLastSibling();

        // keyPickup is in world space while newKey (a RectTransform) is in canvas space, so the following properties need to be transformed
        Vector3 startPosition = Camera.main.WorldToScreenPoint(keyPickup.transform.position);
        Quaternion startRotation = keyPickup.transform.rotation * Quaternion.Inverse(Camera.main.transform.rotation);
        float scaleFactor = 1.0f / (Camera.main.orthographicSize * 2);
        scaleFactor *= CanvasScale.Instance.GetReferenceHeight();
        scaleFactor /= newKey.GetComponent<RectTransform>().rect.height;
        Vector3 startScale = keyPickup.transform.localScale * scaleFactor;

        Destroy(keyPickup.gameObject);


        float f = 0;
        while (f < 1)
        {
            f += Time.deltaTime / 0.6f;
            if (f > 1) { f = 1; }

            float smoothedF = Mathf.SmoothStep(0, 1, f);

            newKey.transform.position = Vector3.Lerp(startPosition, keySpawnPoint.position, smoothedF);
            newKey.transform.rotation = Quaternion.Slerp(startRotation, keySpawnPoint.localRotation, smoothedF);
            newKey.transform.localScale = Vector3.Lerp(startScale, keySpawnPoint.localScale, smoothedF);

            yield return null;
        }

        inputKeys.Add(newKey);
    }


    public void AddFragment(FragmentPickup fragmentPickup) { StartCoroutine(AddFragmentCoroutine(fragmentPickup)); }
    private IEnumerator AddFragmentCoroutine(FragmentPickup fragmentPickup)
    {
        InputFragment newFragment = Instantiate(fragmentPickup.GetFragmentPrefab(), fragmentParent);
        Transform targetTransform = new GameObject("Fragment Target Transform").transform;
        targetTransform.position = newFragment.transform.position;
        targetTransform.rotation = newFragment.transform.rotation;
        targetTransform.localScale = newFragment.transform.localScale;

        // fragmentPickup is in world space while newFragment (a RectTransform) is in canvas space, so the following properties need to be transformed
        Vector3 startPosition = Camera.main.WorldToScreenPoint(fragmentPickup.transform.position);
        Quaternion startRotation = fragmentPickup.transform.rotation * Quaternion.Inverse(Camera.main.transform.rotation);
        float scaleFactor = 1.0f / (Camera.main.orthographicSize * 2);
        scaleFactor *= CanvasScale.Instance.GetReferenceHeight();
        scaleFactor /= newFragment.GetComponent<RectTransform>().rect.height;
        scaleFactor *= 3; // The sprite is 3 units tall because it's 100 pixels per unit and 300 pixels
        Vector3 startScale = fragmentPickup.transform.localScale * scaleFactor;

        Destroy(fragmentPickup.gameObject);


        float f = 0;
        while (f < 1)
        {
            f += Time.deltaTime / 0.6f;
            if (f > 1) { f = 1; }

            float smoothedF = Mathf.SmoothStep(0, 1, f);

            newFragment.transform.position = Vector3.Lerp(startPosition, targetTransform.position, smoothedF);
            newFragment.transform.rotation = Quaternion.Slerp(startRotation, targetTransform.localRotation, smoothedF);
            newFragment.transform.localScale = Vector3.Lerp(startScale, targetTransform.localScale, smoothedF);

            yield return null;
        }

        Destroy(targetTransform.gameObject);
        inputFragments.Add(newFragment);
        foreach (InputSlot inputSlot in newFragment.GetComponentsInChildren<InputSlot>())
        {
            inputSlots.Add(inputSlot);
        }

        AudioManager.Instance.PlayNextMusic();
    }
}
