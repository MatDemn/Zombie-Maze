using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Weapon : MonoBehaviour
{
    private Animator anim;
    private AudioSource _AudioSource;

    public Text ammoOverlay;
    public Text healthOverlay;
    public Text scoreOverlay;
    public Text scoreGameOver;
    public Text grenadeOverlay;

    public Canvas gameWinCanvas;
    public Text scoreGameWin;

    public Canvas gameOverCanvas;

    [Header("Mechanika strzalu")]
    public float range = 100f; // dystans na na jaki mozna strzelac
    public int magazineAmmo = 30; // max ilosc naboi w magazynku
    public int totalBullets = 200; // calkowita ilosc naboi w ekwipunku

    public int maxGrenades = 5;
    public int currentGrenades;

    public int bulletsLeft; // ile zostalo aktualnie naboi w magazynku

    public float fireRate = 0.1f;
    float fireTimer;

    [Header("Inne")]
    public Transform shootPoint;
    public AudioClip shootingClip;
    public AudioClip hurtSound;

    public GameObject hitParticles;
    public GameObject bulletImpact;
    public ParticleSystem flash;


    public int maxhealthAmount = 100;
    public int healthAmount;

    public int currentScore = 0;

    bool isReloading;

    public bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _AudioSource = GetComponent<AudioSource>();
        bulletsLeft = magazineAmmo;
        healthAmount = maxhealthAmount;
        currentGrenades = maxGrenades;
        RefreshAmmoUI();
        RefreshHealthUI();
        RefreshGrenadeUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            if (Input.GetButton("Fire1"))
            {
                if (bulletsLeft > 0)
                    Fire();
                else if (totalBullets > 0)
                    DoReload();
            }
            if (Input.GetButtonDown("Reload"))
            {
                if (bulletsLeft < magazineAmmo && totalBullets > 0)
                    DoReload();
            }
            if (Input.GetKeyDown(KeyCode.K)) {
                UnityEngine.Debug.Log("Pressed k!");
                SceneManager.LoadScene("Level1", LoadSceneMode.Single);
            }
            if(Input.GetButtonDown("Grenade"))
            {
                if(currentGrenades > 0)
                {
                    ThrowGranade();
                }
            }


            if (fireTimer < fireRate)
                fireTimer += Time.deltaTime;

            if (healthAmount <= 0)
                RunGameOver();
        }
    }

    void FixedUpdate()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        isReloading = info.IsName("AK47_Reload");
    }

    void Fire()
    {
        if (fireTimer < fireRate || bulletsLeft <= 0 || isReloading) return;

        RaycastHit hit;
        if(Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range))
        {
            if(hit.transform.tag == "Enemy")
            {
                if(hit.collider.name == "Head")
                    hit.transform.gameObject.GetComponent<Destroyable>().gotHit(Random.Range(98, 100), hit);
                else if (hit.collider.name == "Torso")
                    hit.transform.gameObject.GetComponent<Destroyable>().gotHit(Random.Range(25, 50), hit);
                else
                    hit.transform.gameObject.GetComponent<Destroyable>().gotHit(Random.Range(5, 25), hit);

                UnityEngine.Debug.Log("Trafiles: " + hit.collider.name);
            }
            else
            {
                //UnityEngine.Debug.Log(hit.transform.name + "found!");

                GameObject hitParticleEffect = Instantiate(hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                GameObject hitHoleEffect = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

                Destroy(hitParticleEffect, 1f);
                Destroy(hitHoleEffect, 2f);
            }
        }

        anim.CrossFadeInFixedTime("AK47_Fire", 0.1f);
        flash.Play();
        PlayShootingSound();

        bulletsLeft--;
        fireTimer = 0.0f;

        RefreshAmmoUI();
    }

    public void ThrowGranade()
    {
        currentGrenades--;
        RefreshGrenadeUI();
    }

    public void Reload()
    {
        if (totalBullets <= 0) return;

        int howManyToLoad = magazineAmmo - bulletsLeft;
        int howManyReally = (howManyToLoad > totalBullets) ? totalBullets : howManyToLoad;

        totalBullets -= howManyReally;
        bulletsLeft += howManyReally;

        RefreshAmmoUI();
    }

    void DoReload()
    {
        if (isReloading) return;

        anim.CrossFadeInFixedTime("AK47_Reload", 0.1f);
    }

    public void addAmmo(int amount)
    {
        totalBullets += amount;
        RefreshAmmoUI();
    }

    public void gotHit(int amount)
    {
        healthAmount -= amount;
        _AudioSource.PlayOneShot(hurtSound);
        RefreshHealthUI();
    }

    void PlayShootingSound()
    {
        _AudioSource.PlayOneShot(shootingClip);
    }

    public void changeScore(int amount)
    {
        currentScore += amount;
        RefreshScore();
    }

    void RefreshAmmoUI()
    {
        ammoOverlay.text = bulletsLeft + "/" + totalBullets;
    }

    void RefreshHealthUI()
    {
        healthOverlay.text = healthAmount + "/" + maxhealthAmount;
    }

    void RefreshScore()
    {
        scoreOverlay.text = "Score: " + currentScore;
    }

    void RefreshGrenadeUI()
    {
        grenadeOverlay.text = currentGrenades + "/" + maxGrenades;
    }
    public void addGrenades(int amount)
    {
        currentGrenades += amount;
        currentGrenades = Mathf.Clamp(currentGrenades, 0, maxGrenades);
        RefreshGrenadeUI();
    }
    public void RunGameOver()
    {
        gameOverCanvas.gameObject.SetActive(true);
        scoreGameOver.text = "Total score: " + currentScore;
        isGameOver = true;
    }

    public void RunGameWin()
    {
        gameWinCanvas.gameObject.SetActive(true);
        scoreGameWin.text = "Total score: " + currentScore;
        isGameOver = true;
    }
}
