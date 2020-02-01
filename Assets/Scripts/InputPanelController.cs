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

        // Unused alternative animation
        //StartCoroutine(BreakCoroutine());
    }


    // Do an animation where they break one by one
    private IEnumerator BreakCoroutine()
    {
        while (keysToBreak.Count > 0 || fragmentsToBreak.Count > 0)
        {
            bool itemBroken = false;
            if (keysToBreak.Count > 0 && Random.value < 0.5)
            {
                InputKey inputKey = keysToBreak[Random.Range(0, keysToBreak.Count)];
                keysToBreak.Remove(inputKey);
                BreakKey(inputKey);
                itemBroken = true;
            }
            else
            {
                InputFragment inputFragment = fragmentsToBreak[Random.Range(0, fragmentsToBreak.Count)];
                // Only break a fragment once any keys in it have been broken
                bool fragmentContainsKeys = false;
                foreach (InputSlot inputSlot in inputFragment.GetComponentsInChildren<InputSlot>())
                {
                    if (inputSlot.GetInputKey() != null)
                    {
                        fragmentContainsKeys = true;
                        break;
                    }
                }
                if (!fragmentContainsKeys)
                {
                    fragmentsToBreak.Remove(inputFragment);
                    BreakFragment(inputFragment);
                    itemBroken = true;
                }
            }
            if (itemBroken) { yield return new WaitForSeconds(0.2f); }
        }
    }


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
        if (inputSlot != null) { inputSlot.SetInputKey(null); }
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
        }
    }
}
