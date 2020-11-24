using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingThings : MonoBehaviour {

    [SerializeField] private float speed = 10.0f;
    [SerializeField] private ParticleSystem particleSystem;
    
    // Update is called once per frame
    private void Update () => gameObject.transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);

    public void Vaporize()
    {
        ShowParticles(transform.position, particleSystem);
        Destroy(gameObject);
    }

    private void ShowParticles(Vector3 position, ParticleSystem particle)
    {
        if (!particle) return;
        var ps = (ParticleSystem)Instantiate(particle);
        ps.transform.position = position;
        ps.Play();
        Destroy(ps.gameObject, ps.startLifetime);
    }
}
