using UnityEngine;
using TMPro;
namespace LuckyThief.ThangScripts
{
    public class Gun : MonoBehaviour
    {
        //private float rotateOffSet = 180f;
        private SpriteRenderer spriteRenderer;
        [SerializeField] private Transform firePos;
        [SerializeField] private GameObject bulletPrefabs;
        [SerializeField] private float shotDelay = 0.15f;
        private float nextShot;
        [SerializeField] private int maxAmmo = 24;
        private int currentAmmo;
        [SerializeField] private TextMeshProUGUI ammoText;
        [SerializeField] private BossAudioManager audioManager;
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            currentAmmo = maxAmmo;
            UpdateAmmoText();
        }

        // Update is called once per frame
        void Update()
        {
            RotateGun();
            Shoot();
            Reload();
        }

        void RotateGun()
        {
            if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
            {
                return;
            }
            Vector3 directionToMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            var angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            if (angle > 90 || angle < -90)
            {
                spriteRenderer.flipY = true;
            }
            else
            {
                spriteRenderer.flipY = false;
            }
        }
        void Shoot()
        {
            if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && Time.time > nextShot)
            {
                nextShot = Time.time + shotDelay;
                Instantiate(bulletPrefabs, firePos.position, firePos.rotation);
                currentAmmo--;
                UpdateAmmoText();
                audioManager.PlayShootSound();
            }
        }
        void Reload()
        {
            if (Input.GetMouseButtonDown(1) && currentAmmo < maxAmmo)
            {
                currentAmmo = maxAmmo;
                UpdateAmmoText();
                audioManager.PlayReloadSound();
            }
        }
        private void UpdateAmmoText()
        {
            if (ammoText != null)
            {
                if (currentAmmo > 0)
                {
                    ammoText.text = currentAmmo.ToString();
                }
                else
                {
                    ammoText.text = "Empty";
                }
            }
        }
    }
}
