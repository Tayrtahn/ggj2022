using UnityEngine;

[CreateAssetMenu(fileName = "Audio Clip Set", order = 1)]
public class AudioClipSet : ScriptableObject
{
	public int Priority
	{ get { return priority; } }
	public float Volume
	{ get { return volume; } }

	[SerializeField] AudioClip[] clips;
	[SerializeField] float volume = 1.0f;
	[SerializeField] int priority = 0;
	[SerializeField] Vector2 pitchRange = Vector2.one;

	public AudioClip GetRandomClip()
	{
		return clips[Random.Range(0, clips.Length)];
	}

	public float GetRandomPitch()
	{
		return Random.Range(pitchRange.x, pitchRange.y);
	}
}
