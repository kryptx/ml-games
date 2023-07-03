using UnityEngine;
using System.Collections;

namespace Moltensoft.TankGame {
  public class Combatant : MonoBehaviour {

    public int hitsTaken = 0;
    public int hits = 0;
    public int shots = 0;
    public int health = 100;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
      // if (health < 0) {
      //   Destroy(gameObject);
      // }
    }

    public void TakeDamage(int damage) {
      health -= damage;
      hitsTaken ++;
    }
  }
}
