using UnityEngine;

public class AudioManager : PersistentSingleleton<AudioManager>
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private AudioClip bgmBoss;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip portal;
    [SerializeField] private AudioClip shoot;
    [SerializeField] private AudioClip enemyHit;
    [SerializeField] private AudioClip enemyDead;
    [SerializeField] private AudioClip playerDead;
    [SerializeField] private AudioClip pickUp;
    [SerializeField] private AudioClip swithGun;
    [SerializeField] private AudioClip getCoin;
    [SerializeField] private AudioClip openChest;
    

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private void Start()
    {
        // Only start BGM when we don't already have the same clip playing.
        if (bgmClip != null && (musicSource == null || musicSource.clip != bgmClip || !musicSource.isPlaying))
        {
            PlayBGM(bgmClip);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        // Ensure sources exist (safe if you prefer to assign in inspector)
        if (musicSource == null)
        {
            var go = new GameObject("MusicSource");
            go.transform.SetParent(transform);
            musicSource = go.AddComponent<AudioSource>();
            musicSource.loop = true;
        }

        if (sfxSource == null)
        {
            var go = new GameObject("SfxSource");
            go.transform.SetParent(transform);
            sfxSource = go.AddComponent<AudioSource>();
            sfxSource.loop = false;
        }
    }

    // BGM
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;

        // Avoid restarting the same clip when it's already playing.
        if (musicSource != null && musicSource.isPlaying && musicSource.clip == clip)
        {
            return;
        }

        musicSource.clip = clip;
        musicSource.Play();
    }

    // Play the boss BGM (uses serialized bgmBoss)
    public void PlayBossBGM()
    {
        if (bgmBoss == null) return;
        PlayBGM(bgmBoss);
    }

    // Play the default/main BGM (when boss dies or boss room ends)
    public void PlayMainBGM()
    {
        if (bgmClip == null) return;
        PlayBGM(bgmClip);
    }

    public void StopBGM()
    {
        musicSource.Stop();
        musicSource.clip = null;
    }

    // SFX (non-spatial)
    public void PlaySfx(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    // Spatial / 3D one-shot
    public void PlaySfxAtPoint(AudioClip clip, Vector3 position )
    {
        if (clip == null) return;
        AudioSource.PlayClipAtPoint(clip, position);
    }

    public void OnClick()
    {
        PlaySfx(click);
    }

    // Convenience wrappers to use serialized clips (call from other scripts)
    public void PlayClick() => PlaySfx(click);
    public void PlayPortal() => PlaySfx(portal);
    public void PlayShoot() => PlaySfx(shoot);
    public void PlayEnemyHit() => PlaySfx(enemyHit);
    public void PlayEnemyDead() => PlaySfx(enemyDead);
    public void PlayPlayerDead() => PlaySfx(playerDead);
    public void PlayPickUp() => PlaySfx(pickUp);
    public void PlaySwitchGun() => PlaySfx(swithGun);
    public void PlayGetCoin() => PlaySfx(getCoin);
    public void PlayOpenChest() => PlaySfx(openChest);
}