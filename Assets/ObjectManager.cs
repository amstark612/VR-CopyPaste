using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField]
    private GameObject LeftController;
    [SerializeField]
    private GameObject RightController;
    public float GetControllerDistance()
    {
        Vector3 LPos = LeftController.transform.position;
        Vector3 RPos = RightController.transform.position;

        return Vector3.Distance(LPos, RPos);
    }

    public Vector3 GetControllerMidpoint()
    {
        Vector3 LPos = LeftController.transform.position;
        Vector3 RPos = RightController.transform.position;

        return Vector3.Lerp(LPos, RPos, 0.5f);
    }
}
