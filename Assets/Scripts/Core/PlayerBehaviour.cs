﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerBehaviour : MonoBehaviour {
    public int nextPusher { get; set; }
    public Transform TargetTransform { get; set; }
    public GameObject hitObject { get; set; }

    Coroutine LerpCoroutine; //здесь будем хранить выполняющуюся корутину лерпа движения игрока

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetHitObject();//устанавливаем в какой объект нажали и записываем в hitObject, если таковый был, иначе null
        }
    }

    void OnEnable () {//когда объект активирован
        GameInput.Instance.PlayerInputAction += JumpToNext; //подписываемся на эфир PlayerInputAction и ждём когда он скажет чё нам делать
	}

	void OnDisable () {//когда всё потухло
        GameInput.Instance.PlayerInputAction -= JumpToNext; //отписываемся от эфира
    }

    void JumpToNext(GameInput.PlayerAction action) //Когда в эфире PlayerInputAction что-то "прозвучит", запускается JumpToNext
    {
        if (hitObject)//если есть объект на который нажали мышкой
        {
            float dist = Mathf.Abs(transform.position.x - hitObject.transform.position.x); // дистанция от игрока до hitObject'a                                                                       

            if (dist < 0.3f && action == GameInput.PlayerAction.climb)
            { //подтягивание
                if(LerpCoroutine == null) 
                    StartCoroutine(Lerp()); 
            }

            if (dist <= 1.8f && dist >= 0.3f && action == GameInput.PlayerAction.jump)
            { //прыжок
                if (LerpCoroutine == null)
                    StartCoroutine(Lerp());
            }

            if (dist > 2.5f && action == GameInput.PlayerAction.doubleJump)
            { //двойной прыжок
                if (LerpCoroutine == null)
                    StartCoroutine(Lerp());
            }
        }
    }

    void SetHitObject(){
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.transform != null){
            this.hitObject = hit.transform.gameObject; //объект на который нажали
        }
        else{
            this.hitObject = null;
        }
    }

    IEnumerator Lerp()
    {   
        Vector2 _from = transform.position;
        Vector2 _to = hitObject.transform.position;
        float _t = 0f;
        while (_t < 1){
            _t += 0.05f;
            transform.position = Vector2.Lerp(_from, _to, _t); //перемещаем тело в позицию объекта, на который нажали
            yield return null;
        }    
       
        LerpCoroutine = null;
    }
}
