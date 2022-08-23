using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _rightEngine, _leftEngine;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private AudioClip _explosionSound;
    [SerializeField]
    private int _score = 0;
    private float _speedMultiplier = 2f;
    private float _fireRate =  0.2f;
    private float _nextFire = -1f;
    private bool _tripleShot = false;
    private bool _shield = false;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private AudioSource _audioSource;

    void Start() {
        transform.position = new Vector3(0, -4.2f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (!_spawnManager) {
            Debug.LogError("The Spawn Manager is null.");
        }

        if (!_uiManager) {
            Debug.LogError("The UI Manager is null.");
        }

        if (!_audioSource) {
            Debug.LogError("The audio source on the player is null.");
        }
    }

    // Update is called once per frame
    void Update() {
        CalculateMovement();

        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire) {
            FireLaser();
        }
    }

    void CalculateMovement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * Time.deltaTime * _speed);

        if (transform.position.y >= 3.0f) {
            transform.position = new Vector3(transform.position.x, 3.0f, transform.position.z);
        } else if (transform.position.y <= -4.5f) {
            transform.position = new Vector3(transform.position.x, -4.5f, transform.position.z);
        }

        if (transform.position.x >= 9.4f) {
            transform.position = new Vector3(9.4f, transform.position.y, transform.position.z);
        } else if (transform.position.x <= -9.4f) {
            transform.position = new Vector3(-9.4f, transform.position.y, transform.position.z);
        }
    }

    void FireLaser() {
        _audioSource.clip = _laserSound;
        _nextFire =  Time.time + _fireRate;

        if (_tripleShot) {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        } else {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void Damage() {
        if (_shield) {
            _shield = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        
        _audioSource.clip = _explosionSound;
        _lives--;
        _uiManager.UpdateLives(_lives);
        _audioSource.Play();

        if (_lives == 2) {
            _rightEngine.SetActive(true);
        } else if (_lives == 1) {
            _leftEngine.SetActive(true);
        }

        if (_lives < 1) {
            _spawnManager.OnPlayerDeath();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            _uiManager.GameOver();
        }
    }

    public void TripleShot() {
        _tripleShot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedBoost() {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    public void Shield() {
        _shield = true;
        _shieldVisualizer.SetActive(true);
    }

    public void IncreaseScore(int points) {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    IEnumerator TripleShotPowerDownRoutine() {
        yield return new WaitForSeconds(5.0f);
        _tripleShot = false;
    }

    IEnumerator SpeedBoostPowerDownRoutine() {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
    }
}
