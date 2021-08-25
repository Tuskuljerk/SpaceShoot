using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S; //��������
    [Header("Set in Inspector")]

    public float speed = 30f;
    public float rollMult = -45f;
    public float pitchMult = 30f;

    public float gameRestartDelay = 2f;

    public GameObject projectilePrefab;
    public float projectileSpeed = 40f;

    [SerializeField]

    private float _shieldLevel = 1;

    //���������� ������ ������ �� ��������� ������������� ������ 
    private GameObject lastTriggerGO = null;

    // ���������� ������ �������� ���� WeaponFireDelegate
    public delegate void WeaponFireDelegate();
    // ������� ���� ���� WeaponFireDelegate � ������ fireDelegate
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
        //������� ���������� �� ������ Input
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        //�������� �������

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        //�������� �������
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);


        //��������� ������� ����������
        //if(Input.GetKeyDown(KeyCode.Space))
        // {
        //  TempFire();
        //}
        //���������� ������� �� ���� ����� ������ ������� FireDelegate
        //������� ��������� ������� ������� Jump
        //����� ���������, ��� �������� fireDelegate �� ����� null, ����� �������� ������
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

        //������������� ������������� ���������� ������������
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
                //�������� ������� Main.S ������������� ���� (������� �����)
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }


}
