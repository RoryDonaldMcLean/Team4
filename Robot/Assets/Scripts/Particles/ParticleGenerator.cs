using UnityEngine;
using System.Collections;

public enum Direction { Right, Left }

public class ParticleGenerator : MonoBehaviour
{
    public float SPAWN_INTERVAL = 0.01f; // How much time until the next particle spawns
    float lastSpawnTime = float.MinValue; //The last spawn time
    public int MAX_PARTICLES;
    public float PARTICLE_LIFETIME = 0.3f; //How much time will each particle live

    public Transform particlesParent; // Where will the spawned particles will be parented (To avoid covering the whole inspector with them)
    
    public Direction direction;

    private int _particles = 0;
    void Start()
    {
        if (MAX_PARTICLES == 0)
        {
            MAX_PARTICLES = int.MaxValue;
        }
    }

    void Update()
    {
        if (MAX_PARTICLES > _particles && lastSpawnTime + SPAWN_INTERVAL < Time.time)
        { // Is it time already for spawning a new particle?
            GameObject newElecParticle = (GameObject)Instantiate(Resources.Load("Prefabs/Particle/ParticleElec")); //Spawn a particle
            newElecParticle.GetComponent<ParticlesLife>().SetLifeTime(PARTICLE_LIFETIME);
            newElecParticle.GetComponent<ParticlesLife>().SetStartPosition(this.transform.position);
            switch (direction)
            {
                case Direction.Left:
                    newElecParticle.GetComponent<ParticlesLife>().SetDirection(-this.transform.right);
                    break;
                case Direction.Right:
                    newElecParticle.GetComponent<ParticlesLife>().SetDirection(this.transform.right);
                    break;
            }
            newElecParticle.transform.position = transform.position;
            lastSpawnTime = Time.time; // Register the last spawnTime		
            _particles++;
        }

        else if(_particles >= MAX_PARTICLES)
        {
            Destroy(this);
        }
    }
}