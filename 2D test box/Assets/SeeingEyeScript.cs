using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeingEyeScript : MonoBehaviour
{
    [SerializeField]
    private float fov = 30;
    [Tooltip("If true will spawn Enemies, will enable fog field otherwise")]
    [SerializeField]
    private bool spawnEnemies = true;
    [SerializeField]
    private GameObject[] spawnMinions;
    [SerializeField]
    private FogFieldScript fogField;
    [SerializeField]
    private float checkTime = 1.0f;
    [SerializeField]
    private float eyeRange = 10;
    [SerializeField]
    private Color _detectedColor = Color.white;
    private Transform _player;

    private bool _detected = false;
    private int _nextEnemyIndex = 0;
    private float _cooldownTimer = 1.0f;

    private void Start()
    {
        _cooldownTimer = checkTime;
    }

    private void Update()
    {
        if (spawnEnemies)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_detected && _cooldownTimer <= 0.0f)
            {
                _cooldownTimer = checkTime;
                SpawnEnemies();
            }
        }
    }

    private void SpawnEnemies()
    {
        GameObject go = Instantiate(spawnMinions[_nextEnemyIndex], transform.position, transform.rotation);


        _nextEnemyIndex++;
        if (_nextEnemyIndex >= spawnMinions.Length)
            _nextEnemyIndex = 0;
    }

    private void CheckForPlayer()
    {
        Vector3 diffVec = _player.transform.position - transform.position;
        RaycastHit2D info = Physics2D.Raycast(transform.position, diffVec, eyeRange, 1);
        if (info.transform == _player)
        {
            _detected = true;
            GetComponent<SpriteRenderer>().color = Color.magenta;
        }
    }

    public void DetectPlayer(Transform pTransform)
    {
        _player = pTransform;
        _detected = true;
        if(!spawnEnemies)
        {
            fogField.ActivateFog();
            GetComponent<Animator>().speed = 0.0f;
        }
        GetComponent<SpriteRenderer>().color = _detectedColor;
    }
}
