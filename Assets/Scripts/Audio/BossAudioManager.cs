using UnityEngine;
namespace LuckyThief.ThangScripts
{
    public class BossAudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource effectAudioSource;
        [SerializeField] private AudioSource bossAudioSource;
        [SerializeField] private AudioClip shootClip;
        [SerializeField] private AudioClip reloadClip;
        [SerializeField] private AudioClip laserClip;
        [SerializeField] private AudioClip missleClip;
        [SerializeField] private AudioClip healClip;
        [SerializeField] private AudioClip roundBulletClip;
        [SerializeField] private AudioClip takeDamageClip;
        [SerializeField] private AudioClip dieClip;
        [SerializeField] private AudioClip explosionClip;
        [SerializeField] private AudioClip alertClip;
        [SerializeField] private AudioClip robotExplosion;
        public void PlayShootSound()
        {
            effectAudioSource.PlayOneShot(shootClip);
        }
        public void PlayReloadSound()
        {
            effectAudioSource.PlayOneShot(reloadClip);
        }
        public void PlayLaserSound()
        {
            effectAudioSource.PlayOneShot(laserClip);
        }
        public void PlayMissleSound()
        {
            effectAudioSource.PlayOneShot(missleClip);
        }
        public void PlayHealSound()
        {
            effectAudioSource.PlayOneShot(healClip);
        }
        public void PlayRoundBulletSound()
        {
            effectAudioSource.PlayOneShot(roundBulletClip);
        }
        public void PlayTakeDamagePlayer()
        {
            effectAudioSource.PlayOneShot(takeDamageClip);
        }
        public void PlayDead()
        {
            effectAudioSource.PlayOneShot(dieClip);
        }
        public void PlayExplosion()
        {
            effectAudioSource.PlayOneShot(explosionClip);
        }
        public void PlayAlert()
        {
            effectAudioSource.clip = alertClip;
            effectAudioSource.Play();
        }
        public void StopAlert()
        {
            effectAudioSource.Stop();
        }
        public void PlayRobotExplosion()
        {
            effectAudioSource.PlayOneShot(robotExplosion);
        }
        public void PlayBossAudio()
        {
            bossAudioSource.Play();
        }
    }
}
