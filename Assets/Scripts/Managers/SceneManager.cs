using System.Collections.Generic;
using USceneManager = UnityEngine.SceneManagement.SceneManager;
using UnityEngine;

public sealed class SceneManager : Singleton<SceneManager>
{
    SceneProvider Provider
    {
        get
        {
            if (_provider == null)
                _provider = new SceneProvider(this);
            return _provider;
        }
    }
    SceneProvider _provider;

    private void Update()
    {
        Provider.Process();
    }

    public static void Goto(string sceneName)
    {
        Instance.Provider.Goto(sceneName);
    }

    private void OnDestroy()
    {
        if (_provider != null)
            _provider.Cleanup();
    }


    class SceneProvider
    {
        #region CTORS

        public SceneProvider(MonoBehaviour _manager)
        {
            this.manager = _manager;
            //Debug.Log("Initializing " + this.ToString());
        }

        #endregion

        #region UPDATE

        public void Process()
        {
            
        }

        #endregion

        public virtual void Goto(string sceneName)
        {
            USceneManager.LoadScene(sceneName);
        }

        public virtual void Cleanup()
        {

        }

        MonoBehaviour manager;
    }
}