using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class ShootAction : MonoBehaviour
{
    //Dommage que le Gun inflige
    public int gunDamage = 1;
 
    //Port�e du tir
    public float weaponRange = 200f;
 
    //Force de l'impact du tir
    public float hitForce = 100f;
 
    //La cam�ra
    private Camera fpsCam;
 
    //Temps entre chaque tir (en secondes) 
    public float fireRate = 0.25f;
 
    //Float : m�morise le temps du prochain tir possible
    private float nextFire;
 
    //D�termine sur quel Layer on peut tirer
    public LayerMask layerMask;
 
 
    // Start is called before the first frame update
    void Start()
    {
 
        //R�f�rence de la cam�ra. GetComponentInParent<Camera> permet de chercher une Camera
        //dans ce GameObject et dans ses parents.
        fpsCam = GetComponentInParent<Camera>();
    }
 
    // Update is called once per frame
    void Update()
    {
        // V�rifie si le joueur a press� le bouton pour faire feu (ex:bouton gauche souris)
        // Time.time > nextFire : v�rifie si suffisament de temps s'est �coul� pour pouvoir tirer � nouveau
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            //Nouveau tir
 
            //Met � jour le temps pour le prochain tir
            //Time.time = Temps �coul� depuis le lancement du jeu
            //temps du prochain tir = temps total �coul� + temps qu'il faut attendre
            nextFire = Time.time + fireRate;
 
            print(nextFire);
 
            //On va lancer un rayon invisible qui simulera les balles du gun
 
            //Cr�e un vecteur au centre de la vue de la cam�ra
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
 
            //RaycastHit : permet de savoir ce que le rayon a touch�
            RaycastHit hit;
 
             
            // V�rifie si le raycast a touch� quelque chose
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange, layerMask))
            {
                print("Target");
 
                // V�rifie si la cible a un RigidBody attach�
                if (hit.rigidbody != null)
                {
 
                    //AddForce = Ajoute Force = Pousse le RigidBody avec la force de l'impact
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
 
                    //S'assure que la cible touch�e a un composant ReceiveAction
                    if (hit.collider.gameObject.GetComponent<ReceivedAction>() != null)
                    {
                        //Envoie les dommages � la cible
                        hit.collider.gameObject.GetComponent<ReceivedAction>().GetDamage(gunDamage);
                    }
                }
            }
        }
    }
}