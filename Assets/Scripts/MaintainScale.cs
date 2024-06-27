using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainScale : MonoBehaviour
{
    private Vector3 initialLocalScale;
    private Vector3 lastParentScale;

    void Start()
    {
        // Store the initial local scale of the child object
        initialLocalScale = transform.localScale;

        // Store the initial local scale of the parent object
        if (transform.parent != null)
        {
            lastParentScale = transform.parent.localScale;
        }
    }

    void Update()
    {
        if (transform.parent != null)
        {
            // Check if the parent's scale has changed
            if (transform.parent.localScale != lastParentScale)
            {
                // Reapply the initial Y scale while keeping the updated X and Z scales
                Vector3 currentScale = transform.localScale;
                currentScale.y = initialLocalScale.y;
                transform.localScale = currentScale;

                // Update the last parent scale
                lastParentScale = transform.parent.localScale;
            }
        }
    }
}
