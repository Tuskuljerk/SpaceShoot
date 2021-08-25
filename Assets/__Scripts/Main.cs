using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set In Inspector")]
    public GameObject[] prefabEnemies;  //������ ��������
    public float enemySpawnPerSec = 0.5f;

    public float enemyDefaultPadding = 1.5f; // ������ ��� ��������������

    public WeaponDefinition[] weaponDefinitions;

    private BoundsCheck bndCheck;

    private void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSec);

        //������� � ������� ���� WeaponType
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }

    public void SpawnEnemy()
    {
        //������� ��������� ������ ��� ������
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        //���������� ��������� ������� ��� ������� � ��������� ������� X
        float enemyPadding = enemyDefaultPadding;
        if(go.GetComponent<BoundsCheck>() !=null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        //���������� ��������� ���������� ���������� ���������� �������
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;

        Invoke("SpawnEnemy", 1f / enemySpawnPerSec);
    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    ///<summary>
    ///����������� �������, ������������ WeaponDefinition �� ������������
    ///����������� ���� WEAP_DICT ������ Main.
    ///</summary>
    ///<returns> ��������� WeaponDefinition ���, ���� ��� ������ �����������
    ///��� ���������� WeaponType, ���������� ����� ��������� WeaponDefinition
    ///� ����� none.</returns>
    ///<param name="wt">Tnn ������ WeaponType, ��� �������� ��������� ��������
    ///WeaponDefinition</param>
    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        //��������� ������� ���������� ����� � �������
        // ������� ������� �������� �� �������������� ����� ������� ������,
        // ������� ��������� ���������� ������ ������ ����.
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }
        // ��������� ���������� ���������� ����� ��������� WeaponDefinition
        // � ����� ������ WeaponType.����, ��� �������� ��������� �������
        // ����� ��������� ����������� WeaponDefinition
        return (new WeaponDefinition());
    }
}