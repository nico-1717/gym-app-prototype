using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    int speed = 18;
    double position = 1.5;

    private float rotateZ;
    private bool increase;
    //key = list, value = coroutine
    private Dictionary<List<Transform>, Coroutine> shakeCoroutines = new Dictionary<List<Transform>, Coroutine>();

    public void Shake(List<Transform> objectsToShake)
    {
        //in shakeCoroutines ci sono gli oggetti di ShakeCoroutine attive, la funzione utilizza un loop foreach per iterare attraverso gli oggetti nella lista. Per ogni oggetto, la funzione esegue la stessa logica di Shake originale, utilizzando le variabili locali per evitare conflitti tra gli oggetti e memorizzando le coroutine avviate nella dictionary shakeCoroutines.
        StopShake(objectsToShake);
        Coroutine shakeCoroutine = StartCoroutine(ShakeCoroutine(objectsToShake));
        shakeCoroutines.Add(objectsToShake, shakeCoroutine);
    }

    private void StopShake(List<Transform> objectsToStop)
    {
        if (shakeCoroutines.ContainsKey(objectsToStop))
        {
            StopCoroutine(shakeCoroutines[objectsToStop]);
            shakeCoroutines.Remove(objectsToStop);
        }
    }

    private IEnumerator ShakeCoroutine(List<Transform> objectsToShake)
    {
        while (true)
        {
            if (rotateZ >= position)
            {
                increase = false;
            }
            if (rotateZ <= -position)
            {
                increase = true;
            }
            if (increase)
            {
                rotateZ += speed * Time.deltaTime;
            }
            else
            {
                rotateZ -= speed * Time.deltaTime;
            }
            foreach (Transform objectToShake in objectsToShake)
            {
                objectToShake.rotation = Quaternion.Euler(0.0f, 0.0f, rotateZ);
            }
            yield return null;
        }
    }

    public void StopAllShakes()
    {
        foreach (KeyValuePair<List<Transform>, Coroutine> shakeCoroutine in shakeCoroutines)
        {
            List<Transform> objectsToStop = shakeCoroutine.Key;
            foreach (Transform objectToStop in objectsToStop)
            {
                objectToStop.rotation = Quaternion.identity;
            }
            StopCoroutine(shakeCoroutine.Value);
        }
        shakeCoroutines.Clear();
    }

}