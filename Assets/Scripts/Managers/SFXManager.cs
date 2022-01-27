using System.Collections.Generic;
using UnityEngine;

public sealed class SFXManager : Singleton<SFXManager>
{
    SFXProvider Provider
    {
        get
        {
            if (_provider == null)
                _provider = new SFXProvider_Standard(this);
            return _provider;
        }
    }
    SFXProvider _provider;

    private void Update()
    {
        Provider.Process();
    }

    public static void PlaySound(SoundType type, Vector3 pos, float pitch = 1.0f)
    {
        Instance.Provider.EnqueueSound(type, pos, pitch);
    }

    public static void PlaySound(SoundType type, float pitch = 1.0f)
    {
        Instance.Provider.EnqueueSound(type, pitch);
    }

    private void OnDestroy()
    {
        if (_provider != null)
            _provider.Cleanup();
    }

    [System.Serializable]
    /// <summary>
    /// Base null SFX Provider. No sound will play if used.
    /// </summary>
    class SFXProvider
    {
        #region CTORS

        public SFXProvider(MonoBehaviour _manager)
	    {
            this.manager = _manager;
            //Debug.Log("Initializing " + this.ToString());
        }

        #endregion

        #region UPDATE

        public void Process()
        {
            if (head == tail)
                return;

            AudioClipSet clipSet = GetClipSet(pending[head].id);
            int channel = FindOpenChannel(clipSet.Priority);
            if (channel == -1)
                return;
            PlaySound(clipSet.GetRandomClip(),
                        pending[head].position,
                        channel, 
                        clipSet.GetRandomPitch() * pending[head].pitch,
                        clipSet.Volume
                        );
            head = (head + 1) % MAX_PENDING;
        }

        #endregion

        public virtual void Cleanup()
        {
        
        }

        AudioClipSet GetClipSet(SoundType id)
        {
            string key = System.Enum.GetName(typeof(SoundType), id);
            if (Database.ContainsKey(key))
                return Database[key];
            else
                return null;
        }

        protected virtual int FindOpenChannel(int priority = 0)
        {
            return 0;
        }

        public virtual void EnqueueSound(SoundType soundID, float pitch = 1) { }
        public virtual void EnqueueSound(SoundType soundID, Vector3 position, float pitch = 1) { }

        public virtual void StopAllSounds() { }

        protected virtual void PlaySound(AudioClip soundID, Vector3 position, int channel, float pitch = 1f, float volume = 1f, int priority = 1) {}

        protected Camera Camera => Camera.main;

        protected Dictionary<string, AudioClipSet> Database
	    {
		    get 
		    {
			    if (_database == null)
			    {
                    _database = new Dictionary<string, AudioClipSet>();
                    AudioClipSet[] clipSets = Resources.LoadAll<AudioClipSet>(SFX_CLIP_SET_PATH);
                    foreach (AudioClipSet clipSet in clipSets)
                    {
                        _database.Add(clipSet.name, clipSet);
                    }
			    }
			    return _database;
		    }
	    }
	    private Dictionary<string, AudioClipSet> _database;

	    private const string SFX_CLIP_SET_PATH = "Audio/SFX";
        protected const int MAX_PENDING = 16;

        protected MonoBehaviour manager; 

        protected PlayMessage[] pending = new PlayMessage[MAX_PENDING];
        protected int head;
        protected int tail;

        protected struct PlayMessage
        {
            public PlayMessage(SoundType type, Vector3 pos, float pitch = 1, float vol = 1)
            {
                this.position = pos;
                this.id = type;
                //this.priority = priority;
                this.volume = vol;
                this.pitch = pitch;
            }

            public Vector3 position;
            public SoundType id;
            //public int priority;
            public float volume;
            public float pitch;
        }
    }

    [System.Serializable]
    class SFXProvider_Standard : SFXProvider
    {
        #region CTOR

        public SFXProvider_Standard(MonoBehaviour _manager) :base(_manager)
        {
            AudioSourceHandlers = new AudioSourceHandler[NUM_CHANNELS];
            for (int i = 0; i < NUM_CHANNELS; i++)
            {
                GameObject audioGO = new GameObject(string.Format("Audio Source {0}", i));
                audioGO.transform.SetParent(_manager.transform);
                AudioSourceHandlers[i].source = audioGO.AddComponent<AudioSource>();
                AudioSourceHandlers[i].source.playOnAwake = false;
                AudioSourceHandlers[i].source.spatialBlend = GetDesiredSpatialBlend();
            }
        }

        #endregion

        public override void Cleanup()
        {
            base.Cleanup();
            for (int i = 0; i < NUM_CHANNELS; i++)
            {
                AudioSourceHandlers[i].source.enabled = false;
                Destroy(AudioSourceHandlers[i].source.gameObject);
            }
        }

        public override void EnqueueSound(SoundType soundID, float pitch = 1)
        {
            EnqueueSound(soundID, Camera.transform.position, pitch);
        }

        public override void EnqueueSound(SoundType soundID, Vector3 position, float pitch = 1)
        {
            if (soundID == SoundType.None)
                return;

            // Aggregate requests
            for (int i = head; i != tail; i = (i + 1) % MAX_PENDING)
            {
                if (pending[i].id == soundID)
                {
                    // Average pitches
                    pending[i].pitch = (pitch + pending[i].pitch) * 0.5f;
                    // Average pos
                    pending[i].position = Vector3.Lerp(pending[i].position, position, 0.5f);

                    // Don't need to enqueue.
                    return;
                }
            }

            Debug.Assert((tail + 1) % MAX_PENDING != head);

            pending[tail].id = soundID;
            pending[tail].pitch = pitch;
            pending[tail].position = position;
            tail = (tail + 1) % MAX_PENDING;
        }

        protected override void PlaySound(AudioClip clip, Vector3 position, int channel, float pitch = 1f, float volume = 1f, int priority = 1)
        {
            AudioSourceHandlers[channel].source.Stop();
            AudioSourceHandlers[channel].source.volume = volume;
            AudioSourceHandlers[channel].source.clip = clip;
            AudioSourceHandlers[channel].source.pitch = pitch;
            AudioSourceHandlers[channel].source.transform.position = position;
            AudioSourceHandlers[channel].source.Play();
            AudioSourceHandlers[channel].priority = priority;
        }
        
        public override void StopAllSounds()
        {
            for (int i = 0; i < NUM_CHANNELS; i++)
            {
                AudioSourceHandlers[i].source.Stop();
            }
        }

        protected override int FindOpenChannel(int priority)
        {
            int emptyIndex = -1;
            for (int i = 0; i < NUM_CHANNELS; i++)
            {
                if (!AudioSourceHandlers[i].source.isPlaying)
                {
                    // favor a free audio source over an override of lower priority
                    emptyIndex = i;
                    break;
                }
                if (priority < AudioSourceHandlers[i].priority)
                    emptyIndex = i;
            }
            return emptyIndex;
        }

        protected virtual float GetDesiredSpatialBlend()
        {
            return 0.75f;
        }

        protected AudioSourceHandler[] AudioSourceHandlers { get; }

        #region FIELDS

        public float panFactor = 1;

        const int NUM_CHANNELS = 10;

        #endregion

        protected struct AudioSourceHandler
        {
            public AudioSource source;
            public int priority;
        }
    }

    [System.Serializable]
    /// <summary>
    /// Uses panning only.
    /// </summary>
    class SFXProvider_2D :SFXProvider_Standard
    {
        #region CTOR

        public SFXProvider_2D(MonoBehaviour _manager) : base(_manager)
        {
            
        }

        #endregion

        protected override void PlaySound(AudioClip clip, Vector3 position, int channel, float pitch = 1f, float volume = 1f, int priority = 1)
        {
            base.PlaySound(clip, position, channel, pitch, volume, priority);
            SetAudioSourcePan(AudioSourceHandlers[channel].source, position.x);
        }

        void SetAudioSourcePan(AudioSource source, float xPos)
        {
            if (Camera)
            {
                Vector3 viewportPoint = Camera.WorldToViewportPoint(new Vector3(xPos, Camera.transform.position.y, Camera.nearClipPlane));
                source.panStereo = (viewportPoint.x * 2) - 1;
            }
            else
                source.panStereo = 0;

            source.panStereo = Mathf.Max(Mathf.Min(MAX_PAN, source.panStereo), -MAX_PAN) * panFactor;
        }

        protected override float GetDesiredSpatialBlend()
        {
            return 0.0f;
        }

        const float MAX_PAN = 1f;
    }
}
