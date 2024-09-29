using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attached to an audio source parented to the Texting Dialogue Controller prefab. Triggers different texting-related SFX when certain events fire from inside of <see cref="TextingDialogueCanvas"/>.
/// </summary>
public class TextingAudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] AudioClip amikaMessageSound;
    [SerializeField] AudioClip emeraldMessageSound;
    [SerializeField] AudioClip amikaTypingSound;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        TextingDialogueCanvas.AmikaMessageSound += PlayAmikaMessageSound;
        TextingDialogueCanvas.EmeraldMessageSound += PlayEmeraldMessageSound;
        TextingDialogueCanvas.AmikaTypingSound += PlayAmikaTypingSound;
        TextingDialogueCanvas.StopSounds += StopSounds;
    }

    private void OnDisable()
    {
        TextingDialogueCanvas.AmikaMessageSound -= PlayAmikaMessageSound;
        TextingDialogueCanvas.EmeraldMessageSound -= PlayEmeraldMessageSound;
        TextingDialogueCanvas.AmikaTypingSound -= PlayAmikaTypingSound;
        TextingDialogueCanvas.StopSounds -= StopSounds;
    }

    /// <summary>
    /// Plays when a message from Amika appears.
    /// </summary>
    void PlayAmikaMessageSound()
    {
        audioSource.loop = false;
        audioSource.clip = amikaMessageSound;
        audioSource.time = 0f;
        audioSource.Play();
    }

    /// <summary>
    /// Plays when a message from Emerald appears.
    /// </summary>
    void PlayEmeraldMessageSound()
    {
        audioSource.loop = false;
        audioSource.clip = emeraldMessageSound;
        audioSource.time = 0f;
        audioSource.Play();
    }

    /// <summary>
    /// Plays when Amika is typing something.
    /// </summary>
    void PlayAmikaTypingSound()
    {
        audioSource.loop = true;
        audioSource.clip = amikaTypingSound;
        audioSource.time = Random.Range(0f, audioSource.clip.length);
        audioSource.Play();
    }

    /// <summary>
    /// Stops all sounds from playing.
    /// </summary>
    void StopSounds()
    {
        audioSource.Stop();
    }
}
