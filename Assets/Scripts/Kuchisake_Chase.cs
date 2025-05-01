using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kuchisake_Chase : MonoBehaviour
{
    public GameObject Chaser;
    public Quaternion ChaserSpawnDirection;
    public Vector3 StartPoint;
    public Vector3 Direction;
    public float Speed = 5f;
    public float Distance = 10f;
    public AudioSource ChaseMusic;

    private bool inChase = false;
     
    private void Update()
    {
        if (inChase)
        {
            if (Vector3.Distance(Chaser.transform.position, StartPoint) < Distance) // Only move if the object is within the stop distance
            {
                Chaser.transform.position += Direction * Speed * Time.deltaTime; // Move the object
            } else
            {
                StartCoroutine(FadeOutMusic());
                inChase = false; // Stop the chase
                Destroy(Chaser); // Destroy the chaser when done
            }
        }
    }

    void StartChase()
    {
        Chaser.transform.position = StartPoint;
        Chaser.transform.rotation = ChaserSpawnDirection;
        ChaseMusic.Play();
        inChase = true;
    }

    IEnumerator FadeOutMusic()
    {
        float startVolume = ChaseMusic.volume;
        while (ChaseMusic.volume > 0)
        {
            ChaseMusic.volume -= startVolume * Time.deltaTime / 1f; // Fade out over 1 second
            yield return null;
        }
        ChaseMusic.Stop();
        ChaseMusic.volume = startVolume; // Reset volume to original
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartChase();
        }
    }
}

