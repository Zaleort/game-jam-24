using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableMAINGAME : MonoBehaviour
{
    public  GameObject Baliza1;
    public  GameObject Baliza2;
    public  GameObject Baliza3;
    public  GameObject Nave;
    private void OnEnable()
    {
        Baliza1.SetActive(true);
        Baliza2.SetActive(false);
        Baliza3.SetActive(false);
        Nave.SetActive(false);
    }
}
