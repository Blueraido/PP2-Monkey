using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteGameObject : MonoBehaviour
{
    [SerializeField] GameObject currentObject;
    [SerializeField] int delay;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(currentObject,delay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
