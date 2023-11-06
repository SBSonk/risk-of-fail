using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FootstepManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement controller;
    
    [SerializeField] private AudioSource left, right;
    [SerializeField] private AudioClip[] woodSounds, tileSounds, grassSounds;

    [SerializeField] private float walkTime = 0.5f;
    private int currentFoot = 1;

    private void Start()
    {
        controller.OnWalkStart.AddListener(() => StartCoroutine(StepLoop()));
        controller.OnWalkEnd.AddListener(() => StopAllCoroutines());
    }

    public IEnumerator StepLoop()
    {
        while (true)
        {
            AudioClip[] floorClips = tileSounds;
            // Check floor type
            /*if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.5f, LayerMask.GetMask("Environment")))
            {
                switch (hit.collider.tag)
                {
                    case "Tile":
                        floorClips = tileSounds;
                        break;
                    
                    case "Wood":
                        floorClips = woodSounds;
                        break;
                    
                    case "Grass":
                        floorClips = grassSounds;
                        break;
                }
            }*/

            left.clip = floorClips[Random.Range(0, floorClips.Length - 1)];
            right.clip = floorClips[Random.Range(0, floorClips.Length - 1)];
            
            if (currentFoot == 1) right.Play();
            else if (currentFoot == -1) left.Play();

            yield return new WaitForSeconds(walkTime);

            currentFoot = -currentFoot;
        }
    }
}
