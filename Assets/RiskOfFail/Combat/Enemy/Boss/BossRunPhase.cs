using System.Collections;
using Cinemachine;
//using Dreamteck.Splines;
using RiskOfFail.Combat;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BossRunPhase : MonoBehaviour
{
    /*
        //public SplineFollower follower;
        public BossRoomFirstPhase firstPhase;

        public CinemachineVirtualCamera vcam;
        public Collider2D phaseCameraBounds, lastPhaseBounds;

        public Enemy[] enemyPrefabs;
        public int minEnemiesPerWave = 2, maxEnemiesPerWave = 4;
        public float timeBetweenEnemies = .25f;
        public float minTimeBetweenWaves = 2.5f, maxTimeBetweenWaves = 5f;

        public float normalSpeed = 12;
        [FormerlySerializedAs("spawnSpeed")] public float fastSpeed = 6f;
        public float slowTime = 6f;

        public UnityEvent OnPhaseStart;

        public UnityEvent OnReachFirstWall, OnReachSecondWall;

        private int index;

        private float targetSpeed;

        private void Start()
        {
            follower.enabled = true;
            follower.onEndReached += ReachedEnd;
            targetSpeed = normalSpeed;

            vcam.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = phaseCameraBounds;
            vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(0, 2);
        }

        private void FixedUpdate()
        {
            follower.followSpeed = Mathf.Lerp(follower.followSpeed, targetSpeed, .25f);
        }

        private void OnEnable()
        {
            follower.followSpeed = normalSpeed;
            firstPhase.enabled = false;
            StartCoroutine(SpawnLoop());

            OnPhaseStart?.Invoke();
        }

        private void OnDisable()
        {
            StopLoop();
        }

        private void OnDestroy()
        {
            StopLoop();
        }

        public void ResetSplinePosition()
        {
            follower.SetPercent(0);
        }

        private void ReachedEnd(double _)
        {
            if (index == 0) OnReachFirstWall?.Invoke();
            if (index == 1)
            {
                OnReachSecondWall?.Invoke();
                vcam.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = lastPhaseBounds;
                vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(0, 2);
            }

            index++;

            StopLoop();
        }

        private void StopLoop()
        {
            StopAllCoroutines();
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(minTimeBetweenWaves, maxTimeBetweenWaves));

                var amountToSpawn = Random.Range(minEnemiesPerWave, maxEnemiesPerWave);

                for (var i = 0; i < amountToSpawn; i++)
                {
                    var enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                    Instantiate(enemyToSpawn, transform.position, quaternion.identity);

                    yield return new WaitForSeconds(timeBetweenEnemies);
                }
            }
        }

        public void SpeedUp()
        {
            targetSpeed = fastSpeed;
        }

        public void SlowDown()
        {
            targetSpeed = normalSpeed;
        }
    */
}