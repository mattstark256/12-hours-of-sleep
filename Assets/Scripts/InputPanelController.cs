using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPanelController : MonoBehaviour
{
    [SerializeField]
    private Transform inputPanelTransform;
    [SerializeField]
    private float snapDistance = 50;

    private List<InputKey> inputKeys;
    private List<InputSlot> inputSlots;

    private bool dragging;
    private InputKey draggedKey;
    private Vector3 dragOffset;

    private void Start()
    {
        inputKeys = new List<InputKey>(inputPanelTransform.GetComponentsInChildren<InputKey>());
        inputSlots = new List<InputSlot>(inputPanelTransform.GetComponentsInChildren<InputSlot>());
    }


    private void Update()
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



    private void StartDragging(InputKey inputKey)
    {
        dragging = true;
        draggedKey = inputKey;
        dragOffset = draggedKey.transform.position - Input.mousePosition;
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
    }


    private void ContinueDragging()
    {
        draggedKey.transform.position = Input.mousePosition + dragOffset;
    }


    private void StopDragging()
    {
        // Find the nearest valid slot
        InputSlot closestSlot = null;
        float shortestDistance = 0;
        foreach (InputSlot inputSlot in inputSlots)
        {
            float distance = Vector3.Distance(draggedKey.transform.position, inputSlot.transform.position);
            if (distance < snapDistance && (closestSlot == null || distance < shortestDistance))
            {
                closestSlot = inputSlot;
                shortestDistance = distance;
            }
        }

        if (closestSlot != null)
        {
            // Remove the key that was occupying the slot
            InputKey replacedKey = closestSlot.GetInputKey();
            if (replacedKey != null)
            {
                replacedKey.SetInputSlot(null);

                // Fire off in a random direction
                //float randomAngle = Random.Range(0, 360);
                //replacedKey.SetVelocity(1000 * new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0));

                // Fire off away from the dragged key
                replacedKey.SetVelocity(1000 * (replacedKey.transform.localPosition - draggedKey.transform.localPosition).normalized);

                replacedKey.transform.SetAsLastSibling();
                InputMapper.Instance.RemoveMapping(closestSlot.GetAction());
            }

            closestSlot.SetInputKey(draggedKey);
            draggedKey.SetInputSlot(closestSlot);
            draggedKey.transform.SetAsFirstSibling();
            InputMapper.Instance.AddMapping(closestSlot.GetAction(), draggedKey.GetKeyCode());
        }

        dragging = false;
    }
}
