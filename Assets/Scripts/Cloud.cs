using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

    public GameObject cloudParticle;
    public static float rate = 100f;

    float timeSinceLastSpawn = 0f;
    float correctTimeBetweenSpawns = 1f / rate;
    int c = 0;

    public GameObject vehicle;
    public AnimationCurve fadeOutCurve;

    private List<ParticleInstance> particles;


    // Use this for initialization
    void Start () {
        particles = new List<ParticleInstance>();
	}
	
	// Update is called once per frame
	void Update () {
        timeSinceLastSpawn += Time.deltaTime;
        c = (c + 1) % 4;

        while(timeSinceLastSpawn > correctTimeBetweenSpawns) {
            Transform parent = vehicle.transform;
            BoxCollider2D box = vehicle.GetComponent<BoxCollider2D>();
            Vector3 center = box.bounds.center;
            Vector3 extents = box.bounds.extents;
            Vector3[] corners = new Vector3[] { center - extents, center + extents, new Vector3(center.x - extents.x, center.y + extents.y, center.z), new Vector3(center.x + extents.x, center.y - extents.y, center.z) };

            GameObject o = Instantiate(cloudParticle, corners[c], Quaternion.identity);
            ParticleInstance particle = new ParticleInstance(o, fadeOutCurve);
            particles.Add(particle);
            timeSinceLastSpawn -= correctTimeBetweenSpawns;
        }

        for(int i = particles.Count-1; i>=0; i--) {
            ParticleInstance particle = particles[i];
            if(particle.Update()) {
                particles.Remove(particle);
                Destroy(particle.instance);
            }
        }
    }
}

class ParticleInstance : Object {

    public GameObject instance;

    private float lifeSpan = 1f;
    private float timeAlive = 0f;
    private Vector3 velocity = new Vector3(Random.Range(-0.02f, 0.02f), Random.Range(-0.07f, -0.03f), 0);
    private float scaleUp = Random.Range(0.001f, 0.004f);
    private Vector3 initialScale;
    private AnimationCurve fadeOut;

    public ParticleInstance(GameObject instance, AnimationCurve fadeOut) {
        this.instance = instance;
        float random = Random.Range(0.5f, 1f);
        initialScale = new Vector3(random, random, 1);
        instance.transform.localScale = initialScale;
        instance.transform.Rotate(Vector3.forward*Random.Range(0f, 360f));
        this.fadeOut = fadeOut;
    }

    public bool Update() {
        timeAlive += Time.deltaTime;

        if(timeAlive >= lifeSpan) {
            return true;
        }

        instance.transform.position += velocity;
        instance.transform.localScale += new Vector3(scaleUp, scaleUp, 0);
        Color currentColor = instance.GetComponent<Renderer>().material.color;
        instance.GetComponent<Renderer>().material.color = new Color(currentColor.r, currentColor.g, currentColor.b, fadeOut.Evaluate(timeAlive / lifeSpan));
        return false;
    }
}
