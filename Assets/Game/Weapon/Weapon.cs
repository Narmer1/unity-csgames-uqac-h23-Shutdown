using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject myBullet;

    public GameObject Shell;

    public GameObject myShellposition;

  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()

    {
        if(Input.GetMouseButtonDown(0))
        {
            Object balle = Instantiate(myBullet, transform.position, transform.rotation);

            Object douille = Instantiate(Shell, myShellposition.transform.position, myShellposition.transform.rotation);
            DestroyObject(balle, 0.5f);
            DestroyObject(douille, 10f);


        }


    }
}
