using UnityEngine;

public class FootSteps : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] stoneClips;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Step()
    {
        AudioClip clip = stoneClips[UnityEngine.Random.Range(0,stoneClips.Length)];
        audioSource.PlayOneShot(clip);
    }
}