using UnityEngine;

public class CollisionPlane : MonoBehaviour {

    // Delete everything that collides with the invisible plane
    void OnCollisionEnter(Collision col)
    {
        Destroy(col.gameObject);
    }
}
