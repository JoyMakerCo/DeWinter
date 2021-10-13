using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleAttractor : MonoBehaviour
{
    public Transform Target;
    public string Resource;
    public ParticleSystem Particles;

    private void Awake()
    {
        Play();
    }

    public void Play()
    {
        StartCoroutine(UpdateParticles());
    }

    IEnumerator UpdateParticles()
    {
        int count;
        ParticleSystem.Particle[] particles;
        ParticleSystem.Particle particle;
        Particles.Play();
        while (Particles.IsAlive())
        {
            count = Particles.particleCount;
            particles = new ParticleSystem.Particle[count];
            Particles.GetParticles(particles);
            for (int i=0; i<count; ++i)
            {
                particle = particles[i];
                if (Vector3.SqrMagnitude(Target.position - particle.position) > 1f)
                {
                    particle.position = Vector3.Lerp(particle.position, Target.position, Time.deltaTime * .5f);
                    particles[i] = particle;
                }
            }
            Particles.SetParticles(particles);
            yield return null;
        }
        Particles.Stop();
    }
}
