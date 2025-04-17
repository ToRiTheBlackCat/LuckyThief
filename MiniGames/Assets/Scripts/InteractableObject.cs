using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public GameObject interaction;
    public GameObject minigameUI;
    public Collider2D collider2D;
    [SerializeField]public ContactFilter2D contactFilter2D;

    public List<Collider2D> colliders = new List<Collider2D>(1);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider2D = GetComponent<Collider2D>();
        minigameUI = GameObject.Find("CanvasMiniGame");        
        minigameUI.SetActive(false);
        interaction = GameObject.Find("F");
        interaction.SetActive(false);        
    }

    // Update is called once per frame
    void Update()
    {
        if(collider2D!= null)
        {
            interaction.SetActive(true);
            if (collider2D.Overlap(contactFilter2D, colliders) > 0)
            {
                foreach (Collider2D collider in colliders)
                {
                    OnCollideded(collider.gameObject);
                }
            }
        }
    }

    protected void OnCollideded(GameObject interacted)
    {
        Debug.Log("Collided with " + interacted.name);
        if(Input.GetKeyDown(KeyCode.F))
        {
            OnInteracted(interacted);
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F))
        {
            minigameUI.SetActive(false);
        }
    }

    protected void OnInteracted(GameObject interacted)
    {
        minigameUI.SetActive(true);
    }
}
