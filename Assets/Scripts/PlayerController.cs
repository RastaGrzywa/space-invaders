using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeanTween.rotateZ(gameObject, -45, 0.3f);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 newPosition = transform.position;
            newPosition.x -= playerData.movementSpeed * Time.deltaTime;
            transform.position = newPosition;
        }   
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            LeanTween.rotateZ(gameObject, 0, 0.3f);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            LeanTween.rotateZ(gameObject, 45, 0.3f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 newPosition = transform.position;
            newPosition.x += playerData.movementSpeed * Time.deltaTime;
            transform.position = newPosition;
        }    
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            LeanTween.rotateZ(gameObject, 0, 0.3f);
        }
    }
}
