using System.Collections;
using UnityEngine;

public class shootingScript : MonoBehaviour
{
    

    [SerializeField]
    private AudioClip gunShot;

    [SerializeField]
    private AudioClip unload;

    [SerializeField]
    private AudioClip reload;

    [SerializeField]
    private Transform gunPoint;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    public int damagePerHit; //not implemented, change public damage int on corresponding bullet (attached to shootingScript)

    [SerializeField]
    private float fireRate;

    [SerializeField]
    private float readyToShoot;

    [SerializeField]
    private float recoilPower; //used to be "GunForce" in Sean's player controller script - Part of Sean's recoil scripting

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private int numberOfBullets;

    [SerializeField]
    private float spread;

    [SerializeField]
    public int ammoLoaded;

    [SerializeField]
    public int ammoReserve;

    [SerializeField]
    public int magSize;

    [SerializeField]
    private int maxAmmoReserves;

    [SerializeField]
    public int totalAmmoAllowed;

    [SerializeField]
    public int totalAmmoHeld;

    [SerializeField]
    private float reloadTime;

    [SerializeField]
    public int maximumAmmoPickup;

    [SerializeField]
    private HapticEffectSO gunShotRumble;

    private bool finishedReload = true;

    private bool noMag;

    private bool reloadStarted;

    public bool playMuzzleSmoke;

    private Vector3 playerPos;

    Vector2 ForceDir; //Part of Sean's recoil scripting

    //private PlayerController Player;

    private Player newPlayer;

    private void Start()
    {
        
    }

    private void Awake()
    {
        
        //Player = GetComponentInParent<PlayerController>();
        newPlayer = GetComponentInParent<Player>();

        playerPos = newPlayer.gameObject.transform.position;

        ammoLoaded = magSize;
        reloadStarted = false;
        noMag = false;
        totalAmmoAllowed = magSize + maxAmmoReserves;
        totalAmmoHeld = ammoLoaded + ammoReserve;
        maximumAmmoPickup = totalAmmoAllowed - totalAmmoHeld;
    }

    public void Shoot(bool ShootInput, bool ReloadInput)
    {
        totalAmmoHeld = ammoLoaded + ammoReserve;
        maximumAmmoPickup = totalAmmoAllowed - totalAmmoHeld;

        if (!finishedReload)
        {
            if(reloadStarted)
            {
                StartCoroutine(Reload());
            }
            return;

        }
        else if (ammoLoaded > 0)
        {
            if (ShootInput)
            {
                if (Time.time > readyToShoot)
                {
                    FireBullet();
                    //HapticManager.PlayEffect(gunShotRumble, newPlayer.gameObject.transform.position);
                }

            }
        }
        else
        {
            StartCoroutine(Reload());
        }
        /*if (ReloadInput)
        {

            StartCoroutine(Reload());
            return;

        }*/

    }

    private void FireBullet() // called every time fire is pressed - Arch
    {
        ammoLoaded -= 1;
        
        for (int i = 0; i < numberOfBullets; i++)
        {
            ForceDir = newPlayer.shootDirection;
            
            //Player.ForceToApply = (ForceDir * recoilPower * -1.0f); //Part of Sean's recoil scripting         
            newPlayer.rb.AddForce(-ForceDir * recoilPower, ForceMode2D.Impulse);
            GetComponent<SFX>().PlaySound("Gun Shot");
            
            playMuzzleSmoke = true;
            GameObject firedBullet = Instantiate(bullet, gunPoint.position, gunPoint.rotation); //creates an instance of bullet at the position of the "gun" - Arch
            Vector2 bulletDir = gunPoint.right;
            Vector2 spreader = Vector2.Perpendicular(bulletDir) * Random.Range(-spread, spread);
            firedBullet.GetComponent<Rigidbody2D>().velocity = (bulletDir + spreader) * bulletSpeed; //adds force to the bullet - Arch
            
        }
        readyToShoot = Time.time + (1 / fireRate);
    }

    private IEnumerator Reload()
    {
        if(!reloadStarted)
        {
            GetComponent<SFX>().PlaySound("Unload");

        }

        reloadStarted = true;
        finishedReload = false;
        noMag = true;


        yield return new WaitForSeconds(reloadTime);

        ammoLoaded = magSize;
        newPlayer.reloadTriggered = false;

        finishedReload = true;
        reloadStarted = false;
        ammoReserve -= (magSize - ammoLoaded);

        if (noMag) { GetComponent<SFX>().PlaySound("Reload"); noMag = false; }
        
        
        


    }
}

