using System.Collections.Generic;
using UnityEngine;

public sealed class ParticleManager : Singleton<ParticleManager>
{
    ParticleProvider Provider
    {
        get
        {
            if (_provider == null)
                _provider = new ParticleProvider_Standard(this);
            return _provider;
        }
    }
    ParticleProvider _provider;

    private void Update()
    {
        Provider.Process();
    }

    public static void Emit(ParticleType type, Vector3 pos)
    {
        Instance.Provider.Emit(type, pos);
    }

    private void OnDestroy()
    {
        if (_provider != null)
            _provider.Cleanup();
    }


    [System.Serializable]
    class ParticleProvider
    {
        #region CTORS

        public ParticleProvider(MonoBehaviour _manager)
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

            ParticleSystem clipSet = GetParticleSystem(pending[head].id);
            if (!clipSet)
                return;
            PlayParticleSystem(clipSet, pending[head].position);
            head = (head + 1) % MAX_PENDING;
        }

        #endregion

        public virtual void Cleanup()
        {
            foreach (KeyValuePair<string, ParticleSystemPool> kvp in Database)
                kvp.Value.Cleanup();
        }

        ParticleSystem GetParticleSystem(ParticleType id)
        {
            string key = System.Enum.GetName(typeof(ParticleType), id);
            if (Database.ContainsKey(key))
                return Database[key].GetPooledObject();
            else
                return null;
        }

        protected virtual int GetAvailableInstance()
        {
            return 0;
        }

        public virtual void Emit(ParticleType type, Vector3 position) { }

        protected virtual void PlayParticleSystem(ParticleSystem p, Vector3 position)
        {
            p.transform.position = position;
            p.Play();
        }

        protected Camera Camera => Camera.main;

        protected Dictionary<string, ParticleSystemPool> Database
	    {
		    get 
		    {
			    if (_database == null)
			    {
                    _database = new Dictionary<string, ParticleSystemPool>();
                    ParticleSystemLocator[] set = Resources.LoadAll<ParticleSystemLocator>(PARTICLE_PATH);
                    foreach (ParticleSystemLocator ps in set)
                    {
                        _database.Add(ps.name, new ParticleSystemPool(manager.transform, ps.ParticleSystemPrefab, MAX_PENDING));
                    }
			    }
			    return _database;
		    }
	    }
	    private Dictionary<string, ParticleSystemPool> _database;

	    private const string PARTICLE_PATH = "VFX/ParticleSystems";
        protected const int MAX_PENDING = 6;

        protected MonoBehaviour manager;

        protected PlayMessage[] pending = new PlayMessage[MAX_PENDING];
        protected int head;
        protected int tail;

        protected struct PlayMessage
        {
            public PlayMessage(ParticleType type, Vector3 pos)
            {
                this.position = pos;
                this.id = type;
            }

            public Vector3 position;
            public ParticleType id;
        }
    }

    [System.Serializable]
    class ParticleProvider_Standard : ParticleProvider
    {
        #region CTOR

        public ParticleProvider_Standard(MonoBehaviour _manager) :base(_manager)
        {
            
        }

        #endregion

        public override void Emit(ParticleType id, Vector3 position)
        {
            // Aggregate requests
            for (int i = head; i != tail; i = (i + 1) % MAX_PENDING)
            {
                if (pending[i].id == id)
                {
                    // Average pos
                    pending[i].position = Vector3.Lerp(pending[i].position, position, 0.5f);

                    // Don't need to enqueue.
                    return;
                }
            }

            Debug.Assert((tail + 1) % MAX_PENDING != head);

            pending[tail].id = id;
            pending[tail].position = position;
            tail = (tail + 1) % MAX_PENDING;
        }
    }

    class ParticleSystemPool
    {
        #region FIELDS

        public Transform parent;
        public int poolSize = 10;
        public ParticleSystem pooledObject;
        public bool shouldGrow = false;
        GameObject parentGo;
        List<ParticleSystem> pooledObjects = new List<ParticleSystem>();

        #endregion

        public ParticleSystemPool(Transform _parent, ParticleSystem _prefab, int _size, bool _shouldGrow = false)
        {
            parentGo = new GameObject(_prefab.name);
            parentGo.transform.SetParent(_parent);
            parent = parentGo.transform;
            pooledObject = _prefab;
            poolSize = _size;
            shouldGrow = _shouldGrow;
            for (int i = 0; i < poolSize; i++)
            {
                ParticleSystem po = Object.Instantiate(pooledObject);
                po.Stop();
                po.transform.SetParent(parent);
                pooledObjects.Add(po);
            }
        }

        public void Cleanup()
        {
            for (int i = pooledObjects.Count - 1; i >= 0; i--)
            {
                Object.Destroy(pooledObjects[i].gameObject);
                pooledObjects.RemoveAt(i);
            }
            Destroy(parentGo);
        }

        public ParticleSystem GetPooledObject()
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (pooledObjects[i].isPlaying)
                    continue;

                return pooledObjects[i];
            }

            if (shouldGrow)
            {
                ParticleSystem po = Object.Instantiate(pooledObject);
                po.transform.SetParent(parent);
                //po.gameObject.SetActive(false);
                pooledObjects.Add(po);
                return po;
            }

            return null;
        }
    }

}