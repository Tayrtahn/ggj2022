using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    TimeProvider Provider
    {
        get
        {
            if (_provider == null)
                _provider = new TimeProvider(this);
            return _provider;
        }
    }
    TimeProvider _provider;

    private void Update()
    {
        Provider.Process(Time.unscaledDeltaTime);
    }

    public static void AddPauseSource(object source)
    {
        Instance.Provider.AddPauseSource(source);
    }

    public static void RemovePauseSource(object source)
    {
        Instance.Provider.RemovePauseSource(source);
    }

    public static void AddTimescaleSource(object source, float multiplier)
    {
        Instance.Provider.AddTimescaleSource(source, multiplier);
    }

    public static void RemoveTimescaleSource(object source)
    {
        Instance.Provider.RemoveTimescaleSource(source);
    }

    private void OnDestroy()
    {
        if (_provider != null)
            _provider.Cleanup();
    }

    class TimeProvider
    {
        public TimeProvider(MonoBehaviour _manager)
        {
            timescaleMods = new Dictionary<object, float>();
            pauseSources = new HashSet<object>();
            manager = _manager;
        }

        public void Process(float delta)
        {
            if (isPaused)
            {
                Time.timeScale = 0;
                return;
            }

            Time.timeScale = Mathf.SmoothStep(Time.timeScale, GetTargetTimescale(), 6.0f * delta);
        }

        public void AddTimescaleSource(object source, float multiplier)
        {
            if (timescaleMods.ContainsKey(source))
                timescaleMods[source] = multiplier;
            else
                timescaleMods.Add(source, multiplier);
        }

        public void RemoveTimescaleSource(object source)
        {
            timescaleMods.Remove(source);
        }

        public void AddPauseSource(object source)
        {
            if (pauseSources.Add(source))
                UpdatePause();
        }

        public void RemovePauseSource(object source)
        {
            if (pauseSources.Remove(source))
                UpdatePause();
        }

        public void Cleanup()
        {
            timescaleMods.Clear();
            pauseSources.Clear();
            UpdatePause();
        }

        float GetTargetTimescale()
        {
            float result = 1.0f;
            foreach (KeyValuePair<object, float> kvp in timescaleMods)
                result *= kvp.Value;
            return result;
        }

        void UpdatePause()
        {
            isPaused = pauseSources.Count > 0;
        }

        bool isPaused;
        Dictionary<object, float> timescaleMods;
        HashSet<object> pauseSources;
        MonoBehaviour manager;
    }
}
