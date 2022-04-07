using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiRubiBehaviour : MonoBehaviour
{
    RectTransform thisUiTransform;
    public float speedToRotateInSeconds;
    private Quaternion currentRotation, desiredRotation;
    Coroutine coroRotating;

    [SerializeField] private List<GameObject> bullets;
    private void Awake()
    {
        thisUiTransform = GetComponent<RectTransform>();
    }
    public void RotateForward()
    {
        if (coroRotating != null)
        {
            //Interupt rotation
            StopCoroutine(coroRotating);

            //Immediately set rotation to final desiredRotation
            thisUiTransform.SetPositionAndRotation(thisUiTransform.position, desiredRotation);
        }

        //Collect data on rotations
        currentRotation = thisUiTransform.rotation;
        thisUiTransform.Rotate(Vector3.forward, -60);

        desiredRotation = thisUiTransform.rotation;
        thisUiTransform.rotation = currentRotation;

        //Start real-time rotation
        coroRotating = StartCoroutine(RealTimeRotating(currentRotation, desiredRotation));

        //Counteract rotation of child
        foreach (uiRubiBulletBehaviour _bullet in transform.GetComponentsInChildren<uiRubiBulletBehaviour>())
            _bullet.GetComponent<RectTransform>().Rotate(Vector3.forward, 60);
        
    }
    public void RotateBackward()
    {
        if (coroRotating != null)
        {
            //Interupt rotation
            StopCoroutine(coroRotating);

            //Immediately set rotation to final desiredRotation
            thisUiTransform.SetPositionAndRotation(thisUiTransform.position, desiredRotation);
        }

        //Collect data
        currentRotation = thisUiTransform.rotation;
        thisUiTransform.Rotate(Vector3.forward, 60);

        desiredRotation = thisUiTransform.rotation;
        thisUiTransform.rotation = currentRotation;

        //Start real-time rotation
        coroRotating = StartCoroutine(RealTimeRotating(currentRotation, desiredRotation));

        //Counteract rotation of child
        foreach (uiRubiBulletBehaviour _bullet in transform.GetComponentsInChildren<uiRubiBulletBehaviour>())
            _bullet.GetComponent<RectTransform>().Rotate(Vector3.forward, -60);
    }
    
    IEnumerator RealTimeRotating(Quaternion _initial, Quaternion _final)
    {
        for (int t = 0; t < speedToRotateInSeconds * 60; t++)
        {
            thisUiTransform.rotation = Quaternion.Slerp(_initial, _final, t/(speedToRotateInSeconds * 60));
            yield return new WaitForFixedUpdate();
        }            
    }

    public void SetBullet(int _bulletNumber, int _bulletType)
    {
        bullets[_bulletNumber].GetComponent<uiRubiBulletBehaviour>().SetNewBullet(_bulletType);
    }

    public void FiredBullet()
    {
        GameObject _firedBullet = bullets[0];
        _firedBullet.GetComponent<uiRubiBulletBehaviour>().RemoveBullet();
        bullets.RemoveAt(0);
        bullets.Add(_firedBullet);        
    }
    public void AddBullet(int _bulletType)
    {
        int _lastBullet = bullets.Count - 1;
        SetBullet(_lastBullet, _bulletType);
        GameObject _newBullet = bullets[_lastBullet];
        bullets.Insert(0, _newBullet);
        bullets.RemoveAt(bullets.Count-1);
    }
}
