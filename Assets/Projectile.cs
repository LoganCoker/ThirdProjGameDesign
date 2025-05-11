using TMPro;
using UnityEngine;

public class Projectile : MonoBehaviour{
    
    // bullet
    public GameObject bullet;

    // bullet force 
    public float shootForce, upwardForce;

    // projectile stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magSize, bulletsPerTap;
  
}
