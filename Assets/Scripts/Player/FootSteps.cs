using UnityEngine;

public class FootSteps : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] stoneClips;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Step()
    {
        AudioClip clip = stoneClips[UnityEngine.Random.Range(0,stoneClips.Length)];
        audioSource.PlayOneShot(clip);
    }
}