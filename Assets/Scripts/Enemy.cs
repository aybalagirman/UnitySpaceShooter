using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private int _points = 10;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1; 
    private Player _player;
    private Animator _animator;
    private AudioSource _audioSource;

    void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (!_player) {
            Debug.LogError("The player is null.");
        }


        if (!_animator) {
            Debug.LogError("The animator is null.");
        }

        if (!_audioSource) {
            Debug.LogError("The audio source of the enemy is null.");
        }
    }

    // Update is called once per frame
    void Update() {
        CalculateMovement();
        Fire();
    }

    void CalculateMovement() {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < -6.4f) {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 6.4f, 0);
        }
    }

    void Fire() {
        if (Time.time > _canFire) {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            
            for (int i = 0; i < lasers.Length; i++) {
                lasers[i].SwitchLasers();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Player player = other.transform.GetComponent<Player>();

            if (player) {
                player.Damage();
            }
            
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<PolygonCollider2D>());
            Destroy(this.gameObject, 1.28f);
        }

        if (other.tag == "Laser") {
            if (_player) {
                _player.IncreaseScore(_points);
            }

            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(other.gameObject);
            Destroy(GetComponent<PolygonCollider2D>());
            Destroy(this.gameObject, 1.28f);
        }
    }
}
