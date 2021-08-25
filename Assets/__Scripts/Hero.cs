using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S; //одиночка
    [Header("Set in Inspector")]

    public float speed = 30f;
    public float rollMult = -45f;
    public float pitchMult = 30f;

    public float gameRestartDelay = 2f;

    public GameObject projectilePrefab;
    public float projectileSpeed = 40f;

    [SerializeField]

    private float _shieldLevel = 1;

    //Переменная хранит ссылку на последний столкнувшийся объект 
    private GameObject lastTriggerGO = null;

    // Объявление нового делегата типа WeaponFireDelegate
    public delegate void WeaponFireDelegate();
    // Создать поле типа WeaponFireDelegate с именем fireDelegate
    public WeaponFireDelegate fireDelegate;

    private void Awake()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Hero.Awake() - attempted to assign second Hero.S!");
        }
        //fireDelegate += TempFire;
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        //извлечь информацию из класса Input
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        //изменить позицию

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        //вращение корабля
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);


        //Позволить кораблю выстрелить
        //if(Input.GetKeyDown(KeyCode.Space))
        // {
        //  TempFire();
        //}
        //Произвести выстрел из всех видов оружия вызовом FireDelegate
        //Сначала проверить нажатие клавиши Jump
        //Затем убедиться, что значение fireDelegate не равно null, чтобы избежать ошибки
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }
    }

    void TempFire()
    {
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);
        projGO.transform.position = transform.position;
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        //rigidB.velocity = Vector3.up * projectileSpeed;

        Projectile proj = projGO.GetComponent<Projectile>();
        proj.type = WeaponType.blaster;
        float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;
        rigidB.velocity = Vector3.up * tSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        print("Triggered:" + rootT);

        //Гарантировать невозможность повторного столкновения
        if (go == lastTriggerGO)
        {
            return;
        }
        lastTriggerGO = go;

        if (go.tag == "Enemy")
        {
            shieldLevel--;
            Destroy(go);
        } else
        {
            print("Triggered by non-Enemy: " + go.name);
        }
    }

    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            if (value < 0)
            {
                Destroy(this.gameObject);
                //Сообщить обьекту Main.S перезапустить игру (вызвать метод)
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }


}
