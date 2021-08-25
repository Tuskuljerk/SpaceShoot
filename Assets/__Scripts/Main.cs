using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set In Inspector")]
    public GameObject[] prefabEnemies;  //массив префабов
    public float enemySpawnPerSec = 0.5f;

    public float enemyDefaultPadding = 1.5f; // Отступ для позициирования

    public WeaponDefinition[] weaponDefinitions;

    private BoundsCheck bndCheck;

    private void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSec);

        //Словарь с ключами типа WeaponType
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }

    public void SpawnEnemy()
    {
        //Выбрать случайный префаб для спавна
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        //Разместить вражеский корабль над экраном в случайной позиции X
        float enemyPadding = enemyDefaultPadding;
        if(go.GetComponent<BoundsCheck>() !=null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        //Установить начальные координаты созданного вражеского корабля
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
    ///Статическая функция, возвращающая WeaponDefinition из статического
    ///защищенного поля WEAP_DICT класса Main.
    ///</summary>
    ///<returns> Экземпляр WeaponDefinition или, если нет такого определения
    ///для указанного WeaponType, возвращает новый экземпляр WeaponDefinition
    ///с типом none.</returns>
    ///<param name="wt">Tnn оружия WeaponType, для которого требуется получить
    ///WeaponDefinition</param>
    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        //Проверить наличие указанного ключа в словаре
        // Попытка извлечь значение по отсутствующему ключу вызовет ошибку,
        // поэтому следующая инструкция играет важную роль.
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }
        // Следующая инструкция возвращает новый экземпляр WeaponDefinition
        // с типом оружия WeaponType.попе, что означает неудачную попытку
        // найти требуемое определение WeaponDefinition
        return (new WeaponDefinition());
    }
}
