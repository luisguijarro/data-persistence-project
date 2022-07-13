using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float Speed = 2.0f;
    public float MaxMovement = 2.0f;
    private MainManager mainManager;
    private AudioSource audioSource;
    private GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        this.gameManager = GameManager.Instance;
        this.audioSource = this.gameObject.GetComponent<AudioSource>();
        this.mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.mainManager.IsPaused)
        {
            float input = Input.GetAxis("Horizontal");

            Vector3 pos = transform.position;
            pos.x += input * Speed * Time.deltaTime;

            if (pos.x > MaxMovement)
                pos.x = MaxMovement;
            else if (pos.x < -MaxMovement)
                pos.x = -MaxMovement;

            transform.position = pos;
        }        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (this.audioSource != null)
        {
            this.audioSource.volume = this.gameManager.SoundsVolume;
        }
        this.audioSource.Play();
    }
}
