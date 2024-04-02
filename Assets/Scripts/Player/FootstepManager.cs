using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FootstepManager : MonoBehaviour
{
    public static FootstepManager instance;
    [SerializeField] private PlayerMovement controller;
    
    [SerializeField] private AudioSource left, right;
    [SerializeField] private AudioClip[] woodSounds, tileSounds, grassSounds;

    [SerializeField] private float walkTime = 0.5f;
    private int currentFoot = 1;
    private FloorType floorType = FloorType.Tile;

    private void Awake()
    {
        instance = this;
    }

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
            switch (floorType)
            {
                case FloorType.Tile:
                    floorClips = tileSounds;
                    break;
                
                case FloorType.Wood:
                    floorClips = woodSounds;
                    break;
                
                case FloorType.Grass:
                    floorClips = grassSounds;
                    break;
            }

            left.clip = floorClips[Random.Range(0, floorClips.Length - 1)];
            right.clip = floorClips[Random.Range(0, floorClips.Length - 1)];
            
            if (currentFoot == 1) right.Play();
            else if (currentFoot == -1) left.Play();

            yield return new WaitForSeconds(walkTime);

            currentFoot = -currentFoot;
        }
    }

    public void SetFloor(FloorType f) => floorType = f;

    public void ResetFloor() => floorType = FloorType.Tile;
}

public enum FloorType
{
    Tile, Wood, Grass
}
