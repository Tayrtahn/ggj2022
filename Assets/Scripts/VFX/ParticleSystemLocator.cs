using UnityEngine;

[CreateAssetMenu(fileName = "Particle System Locator", order = 1)]
public class ParticleSystemLocator : ScriptableObject
{
	public ParticleSystem ParticleSystemPrefab => particlePrefab;

	[SerializeField] ParticleSystem particlePrefab;
}
