using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {
    [SerializeField]
    private float _rotationSpeed = 25.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private int _points = 15;
    private Player _player;
    private SpawnManager _spawnManager;

    void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (!_player) {
            Debug.LogError("The player is null.");
        }

        if (!_spawnManager) {
            Debug.LogError("The spawn manager is null.");
        }
    }

    // Update is called once per frame
    void Update() {
        transform.Rotate(Vector3.forward * Time.deltaTime * _rotationSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Player player = other.transform.GetComponent<Player>();

            if(player) {
                player.Damage();
            }

            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            _spawnManager.StartSpawning();        }

        if (other.tag == "Laser") {
            if (_player) {
                _player.IncreaseScore(_points);
            }

            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
            _spawnManager.StartSpawning();
        }
    }
}
