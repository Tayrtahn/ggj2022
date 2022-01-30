using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// Reference: https://docs.unity3d.com/ScriptReference/AudioSource.PlayScheduled.html


public class Jukebox : MonoBehaviour
{ 
    enum State
    {
        PlayScheduled,
        Playing
    }

    public static Jukebox instance;

    public UnityEvent onBeat;

    public float volume = 1.0f;
    public float pitch = 1.0f;
    public float startOffset = 1.0f;
    public float bpm = 140.0f;
    public float interpolateMultiplier = 0.5f;
    //public int numBeatsPerSegment = 16;
    public AudioClip[] clips = new AudioClip[2];

    private double nextEventTime;
    private int flip = 0;
    private AudioSource[] audioSources;
    private bool running = false;

    State state = State.PlayScheduled;

    private void Awake()
    {
        instance = this;
        audioSources = new AudioSource[clips.Length];
    }

    void Start()
    {
        for (int i = 0; i < clips.Length; i++)
        {
            GameObject child = new GameObject("MusicPlayer");
            child.transform.parent = gameObject.transform;
            audioSources[i] = child.AddComponent<AudioSource>();
            audioSources[i].clip = clips[i];
            audioSources[i].volume = 0.0f;
            audioSources[i].pitch = pitch;
            audioSources[i].loop = true;
            //audioSources[i].panStereo = (i+1) % 2 == 0 ? -1.0f : 1.0f;
        }

        nextEventTime = AudioSettings.dspTime + startOffset;
        running = true;
    }

    public void Flip()
    {
        flip += 1;
        if (flip >= clips.Length)
            flip = 0;
    }

    void Update()
    {
        if (!running)
        {
            return;
        }

        double time = AudioSettings.dspTime;

      
        switch (state)
        {
            case State.PlayScheduled:
                if (time + 1.0f > nextEventTime)
                {
                    // We are now approx. 1 second before the time at which the sound should play,
                    // so we will schedule it now in order for the system to have enough time
                    // to prepare the playback at the specified time. This may involve opening
                    // buffering a streamed file and should therefore take any worst-case delay into account.
                    for (int i = 0; i < clips.Length; i++)
                    {
                        audioSources[i].clip = clips[i];
                        audioSources[i].PlayScheduled(nextEventTime);
                    }

                    //Debug.Log("Scheduled source " + flip + " to start at time " + nextEventTime);

                    nextEventTime += 60.0f / bpm;

                    // Flip between two audio sources so that the loading process of one does not interfere with the one that's playing out
                    //flip = 1 - flip;
                    state = State.Playing;
                }
                break;

            case State.Playing:
                if (time > nextEventTime)
                {
                    onBeat.Invoke();
                    nextEventTime += 60.0f / bpm;
                }

                for (int i = 0; i < clips.Length; i++)
                {
                    audioSources[i].volume = Mathf.SmoothStep(audioSources[i].volume, flip == i ? volume : 0.0f, interpolateMultiplier * Time.unscaledDeltaTime);
                }

                //if (Keyboard.current.spaceKey.wasPressedThisFrame)
                //    Flip();
                break;

            default:
                break;
        }
    }
}