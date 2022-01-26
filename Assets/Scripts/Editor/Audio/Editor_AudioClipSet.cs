using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


[CustomEditor(typeof(AudioClipSet), true)]
public sealed class Editor_AudioClipSet : Editor
{
	public override void OnInspectorGUI () 
	{
		base.OnInspectorGUI();

        AudioClipSet at = target as AudioClipSet;
        if (!at)
            return;

        EditorGUILayout.Space();

        GUI.skin.label.richText = true;
        GUI.skin.button.richText = true;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<color=green>▶</color>"))
        {
            AudioSource source;
            if (lookup.TryGetValue(at, out source))
            {
                source.clip = at.GetRandomClip();
                source.pitch = at.GetRandomPitch();
                source.Play();
            }
            else
                PlayClipAt(at, at.Volume, at.GetRandomPitch());
        }
        if (GUILayout.Button("<color=red>II</color>"))
        {
            ClearClips();
        }
        GUILayout.EndHorizontal();

        if (lookup != null && lookup.ContainsKey(at))
        {
            GUILayout.Label(lookup[at].isPlaying ? "<color=green>PLAYING</color>" : "<color=red>FINISHED</color>");
            if (!lookup[at].isPlaying)
            {
                editorSources.Remove(lookup[at].gameObject);
                lookup.Remove(at);
                return;
            }
        }

        ///
        //AudioWaveform(at.GetRandomClip(), 60, 10, Color.blue);
		//EditorUtility.SetDirty( target );
	}

    public static AudioSource PlayClipAt(AudioClipSet clipSet, float volume, float pitch)
    {
        AudioClip clip = clipSet.GetRandomClip();

        var tempGO = new GameObject("TempAudio");
        tempGO.hideFlags = HideFlags.HideAndDontSave;
        var aSource = tempGO.AddComponent<AudioSource>();
        aSource.hideFlags = HideFlags.HideAndDontSave;

        lookup.Add(clipSet, aSource);
        editorSources.Add(tempGO);

        aSource.clip = clip;
        aSource.pitch = pitch;
        aSource.volume = volume;
        //aSource.outputAudioMixerGroup = clipSet.m_AudioMixerGroup;
        //aSource.bypassEffects = clipSet.ShouldBypassEffects;
        //aSource.bypassListenerEffects = clipSet.ShouldBypassEffects;

        aSource.Play(); // start the sound
        return aSource; // return the AudioSource reference
    }

    public static void ClearClips()
    {
        foreach (KeyValuePair<AudioClipSet, AudioSource> kvp in lookup)
        {
            kvp.Value.Stop();
        }

        for (int i = editorSources.Count - 1; i >= 0; i--)
        {
            // destroy immediate needed?
            DestroyImmediate(editorSources[i]);
            editorSources.RemoveAt(i);
        }
        lookup.Clear();
    }

    static Dictionary<AudioClipSet, AudioSource> lookup = new Dictionary<AudioClipSet, AudioSource>();
    static List<GameObject> editorSources = new List<GameObject>();
}
