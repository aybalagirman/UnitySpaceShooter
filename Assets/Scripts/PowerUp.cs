using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    [SerializeField]
    private int _powerupID; // id for powerups: 0 -> triple shot, 1 -> speed, 2 -> shield
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _points = 20;
    [SerializeField]
    private AudioClip _clip;
 
    // Update is called once per frame
    void Update() {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if(transform.position.y < -6.58f) {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {

        if (other.tag == "Player") {
            Player player = other.transform.GetComponent<Player>();
            Vector3 position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
            AudioSource.PlayClipAtPoint(_clip, position, 1f);
            if (player) {
               switch (_powerupID) {
                case 0:
                    player.TripleShot();
                    player.IncreaseScore(_points);
                    break;
                case 1:
                    player.SpeedBoost();
                    player.IncreaseScore(_points);
                    break;
                case 2:
                    player.Shield();
                    player.IncreaseScore(_points);
                    break;
               }
            }

            Destroy(this.gameObject);
        }
    }
}
