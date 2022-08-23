using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
    [SerializeField]
    private float _speed = 8.0f;
    private bool _isEnemy = false;

    void Update() {
        if (!_isEnemy) {
            MoveUp();
        } else {
            MoveDown();
        }
    }

    void MoveUp() {
        transform.Translate(Vector3.up * Time.deltaTime * _speed);

        if(transform.position.y > 6.1f) {
            if (transform.parent) {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    void MoveDown() {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if(transform.position.y < -6.1f) {
            if (transform.parent) {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void SwitchLasers() {
        _isEnemy = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && _isEnemy) {
            Player player = other.GetComponent<Player>();

            if (player) {
                player.Damage();

                if (transform.parent) {
                    Destroy(transform.parent.gameObject);
                }
                 
                Destroy(this.gameObject);
            }
        }
    }
}
