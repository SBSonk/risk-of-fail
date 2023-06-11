using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float[] rotations;
    [SerializeField] private float stayTime = 0.5f;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private LoopType loopType;

    private int indexDir = 1;
    private int currentIndex;

    private void Start()
    {
        if (rotations.Length <= 1)
        {
            Debug.LogError("No Rotations Set");
            return;
        }
        
        StartCoroutine(RotateLoop());
    }

    IEnumerator RotateLoop()
    {
        while (true)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.MoveTowardsAngle(transform.eulerAngles.z, rotations[currentIndex], rotateSpeed * Time.deltaTime));
            
            if (TransformUtils.GetInspectorRotation(transform).z.Equals(rotations[currentIndex]))
            {
                currentIndex += indexDir;

                switch (loopType)
                {
                    case LoopType.Loop:
                        if (currentIndex > rotations.Length - 1) currentIndex = 0;
                        break;
                    
                    case LoopType.Stay:
                        yield return null;
                        break;
                    
                    case LoopType.PingPong:
                        if (currentIndex > rotations.Length - 1)
                        {
                            currentIndex--;
                            indexDir = -1;
                        }
                        else if (currentIndex < 0)
                        {
                            currentIndex++;
                            indexDir = 1;
                        }
                        break;
                }

                yield return new WaitForSeconds(stayTime);
            }
            
            yield return new WaitForEndOfFrame();
        }
    }
}

public enum LoopType
{
    Stay, Loop, PingPong
}
