using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;

namespace Moltensoft.TankGame {
  public class TankAgent : Agent {
    private GameObject _enemy;    
    Rigidbody m_TankRb;
    Rigidbody m_EnemyRb;
    Tank tank;
    EnvironmentParameters m_ResetParams;
    public LayerMask bulletsLayer;
    public LayerMask wallsLayer;
    public GameObject enemyTank;
    Vector3 startingPosition;

    public override void Initialize()
    {
      m_TankRb = GetComponent<Rigidbody>();
      m_EnemyRb = enemyTank.GetComponent<Rigidbody>();
      tank = GetComponent<Tank>();
      m_ResetParams = Academy.Instance.EnvironmentParameters;
      startingPosition = transform.position;
      SetResetParameters();
    }

    public override void CollectObservations(VectorSensor sensor) {
      sensor.AddObservation(gameObject.transform.rotation.y);
      sensor.AddObservation(enemyTank.transform.rotation.y);
      sensor.AddObservation(enemyTank.transform.position - gameObject.transform.position);
      sensor.AddObservation(m_TankRb.velocity);
      sensor.AddObservation(m_EnemyRb.velocity);

      // trace rays in 8 directions
      sensor.AddObservation(Physics.Raycast(transform.position, transform.forward, 100f, wallsLayer));
      sensor.AddObservation(Physics.Raycast(transform.position, transform.right, 100f, wallsLayer));
      sensor.AddObservation(Physics.Raycast(transform.position, -transform.forward, 100f, wallsLayer));
      sensor.AddObservation(Physics.Raycast(transform.position, -transform.right, 100f, wallsLayer));
      sensor.AddObservation(Physics.Raycast(transform.position, transform.forward + transform.right, 100f, wallsLayer));
      sensor.AddObservation(Physics.Raycast(transform.position, transform.forward - transform.right, 100f, wallsLayer));
      sensor.AddObservation(Physics.Raycast(transform.position, -transform.forward + transform.right, 100f, wallsLayer));
      sensor.AddObservation(Physics.Raycast(transform.position, -transform.forward - transform.right, 100f, wallsLayer));

      // and also 10 degrees left and right of forward
      sensor.AddObservation(Physics.Raycast(transform.position, Quaternion.Euler(0, 10, 0) * transform.forward, 100f, wallsLayer));
      sensor.AddObservation(Physics.Raycast(transform.position, Quaternion.Euler(0, -10, 0) * transform.forward, 100f, wallsLayer));

      // position and velocity of nearest 5 bullets
      var bullets = Physics.OverlapSphere(transform.position, 100f, bulletsLayer)
        .OrderBy(bullet => Vector3.Distance(transform.position, bullet.transform.position))
        .Take(5);
        
      foreach (var bullet in bullets) {
        sensor.AddObservation(bullet.transform.position);
        sensor.AddObservation(bullet.GetComponent<Rigidbody>().velocity);
      }
    }    

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
      tank.HorizontalInput = actionBuffers.ContinuousActions[0];
      tank.VerticalInput = Mathf.Clamp(actionBuffers.ContinuousActions[1] + 0.5f * Mathf.Sign(actionBuffers.ContinuousActions[1]), -1, 1);

      if (actionBuffers.ContinuousActions[2] > 0) {
        tank.Fire();
      }

      AddReward((transform.position - startingPosition).magnitude * 0.01f);
    }

    public override void OnEpisodeBegin()
    {
        //Reset the parameters when the Agent is reset.
        SetResetParameters();
        DestroyAllBullets();
    }

    private void DestroyAllBullets() {
      var bullets = GameObject.FindGameObjectsWithTag("Bullet");
      foreach (var bullet in bullets) {
        Destroy(bullet);
      }
    }

    // public override void Heuristic(in ActionBuffers actionsOut)
    // {
    //   var continuousActionsOut = actionsOut.ContinuousActions;
    //   continuousActionsOut[0] = Input.GetAxis("Horizontal");
    //   continuousActionsOut[1] = Input.GetAxis("Vertical");

    //   var discreteActionsOut = actionsOut.DiscreteActions;
    //   discreteActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    // }
    
    public void SetResetParameters()
    {
        SetTank();
    }

    public void SetTank() {
      transform.position = startingPosition;
      transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
      m_TankRb.velocity = Vector3.zero;
      m_TankRb.angularVelocity = Vector3.zero;
    }
  }
}
