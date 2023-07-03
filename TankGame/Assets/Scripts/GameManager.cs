using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moltensoft.TankGame {
  public class GameManager : MonoBehaviour {

    public Tank tank;
    public GameObject speedometer;

    void Start() {

    }

    void Update() {
      // tell the tank how to move
      // tank.HorizontalInput = Input.GetAxis("Horizontal");
      // tank.VerticalInput = Input.GetAxis("Vertical");
      // if (Input.GetKey(KeyCode.Space)) {
      //   tank.GetComponentInChildren<Cannon>().Fire();
      // }
      // update speedometer
      var speedo = speedometer.GetComponent<TMPro.TextMeshProUGUI>();
      speedo.text = Mathf.Round(tank.CurrentSpeed).ToString();
    }
  }
}
