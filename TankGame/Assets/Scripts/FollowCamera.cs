using UnityEngine;


namespace Moltensoft.TankGame {
    
  [ExecuteInEditMode]
  public class FollowCamera : MonoBehaviour {
    public Transform target;
    public Vector3 offset;

    void LateUpdate() {
      transform.position = target.position + offset;
    }
  }
}
