using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingThings : MonoBehaviour
{ 

    [SerializeField] private float speed = 10.0f;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private AudioClip clip;

    public void Vaporize()
    {
        PlaySfx();
        ShowParticles(transform.position, particleSystem);
        Destroy(gameObject);
    }

    private void PlaySfx() => AudioPlayer.Instance.PlaySfx(clip);

    private void ShowParticles(Vector3 position, ParticleSystem particle)
    {
        if (!particle) return;
        var ps = (ParticleSystem) Instantiate(particle);
        ps.transform.position = position;
        ps.Play();
        Destroy(ps.gameObject, ps.startLifetime);
    }

    private void Update() => gameObject.transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);
    
}