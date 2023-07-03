using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moltensoft.TankGame {
  public class Bullet : MonoBehaviour {
    public Rigidbody rigidBody;
    public GameObject explosion;
    public LayerMask whatIsEnemies;

    [Range(0, 1)]
    public float bounciness;
    public bool useGravity;
    public int explosionDamage;
    public float explosionRange;
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;
    public bool exploded = false;
    public GameObject owner;
    int collisions;
    PhysicMaterial physics_mat;

    void Setup() {
      // create a new Physic material
      physics_mat = new PhysicMaterial();
      physics_mat.bounciness = bounciness;
      physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
      physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;
      // assign material to collider
      GetComponent<SphereCollider>().material = physics_mat;
      rigidBody.useGravity = useGravity;
      // add the "Bullet" tag
      gameObject.tag = "Bullet";
    }

    void Start() {
      Setup();
    }

    private void Explode() {
      if (exploded) return;
      exploded = true;

      if (explosion != null) {
        explosion = Instantiate(explosion, transform.position, Quaternion.identity);
        explosion.GetComponent<ParticleSystem>().Emit(1);
      }
      Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
      for (int i = 0; i < enemies.Length; i++) {
        enemies[i].GetComponent<Combatant>().TakeDamage(explosionDamage);
        enemies[i].GetComponent<TankAgent>().AddReward(-1.0f);
        if (enemies[i].gameObject != owner) {
          owner.GetComponent<TankAgent>().AddReward(1.0f);
        } else {
          // you shot yourself!
          owner.GetComponent<TankAgent>().AddReward(-3.0f);
        }
        owner.GetComponent<Combatant>().hits++;
      }

      Destroy(explosion, 1f);
      Destroy(gameObject, 0.05f);

      // immediately remove the mesh
      GetComponent<MeshRenderer>().enabled = false;
    }

    void OnCollisionEnter(Collision collision) {
      // the first collision can never be with the owner
      if (collisions == 0 && collision.gameObject == owner) return;

      collisions++;
      // explode if we hit a thing in the whatIsEnemies layer
      if (explodeOnTouch && ((1 << collision.gameObject.layer) & whatIsEnemies) != 0) {
        Debug.Log("Hit enemy! its name is " + collision.gameObject.name);
        Explode();
      }
    }


    void Update() {
      // explode if collisions exceed max collisions
      if (collisions > maxCollisions) {
        Explode();
        owner.GetComponent<TankAgent>().AddReward(-0.1f);
      }
      // destroy bullet after set seconds
      maxLifetime -= Time.deltaTime;
      if (maxLifetime <= 0) {
        Explode();
        owner.GetComponent<TankAgent>().AddReward(-0.1f);
      }
    }

  }
}
