using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControler : MonoBehaviour
{
    // Assign the target GameObject in the Unity Inspector
    public GameObject targetObject;

    // Method to enable the target GameObject
    public void EnableTargetObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
            Debug.Log($"{targetObject.name} has been enabled!");
        }
        else
        {
            Debug.LogWarning("Target Object is not assigned!");
        }
    }
}
