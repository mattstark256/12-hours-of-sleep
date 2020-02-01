using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// The canvas scales with screen size, so if I want to do calculations in canvas space I need to convert from world space


public class CanvasScale : MonoBehaviour
{
    // Singleton
    private static CanvasScale instance;
    public static CanvasScale Instance { get { return instance; } }

    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private float referenceHeight = 1080;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }


    public float GetCanvasScale()
    {
        return canvas.scaleFactor;
    }


    public float GetReferenceHeight()
    {
        return referenceHeight;
    }


    public Vector3 WorldToCanvas(Vector3 vector3)
    {
        return vector3 / canvas.scaleFactor;
    }
    public float WorldToCanvas(float distance)
    {
        return distance / canvas.scaleFactor;
    }


    public Vector3 CanvasToWorld(Vector3 vector3)
    {
        return vector3 * canvas.scaleFactor;
    }
    public float CanvasToWorld(float distance)
    {
        return distance * canvas.scaleFactor;
    }
}
