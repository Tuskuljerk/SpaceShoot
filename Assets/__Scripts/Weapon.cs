using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponType
{
    none,   //нет оружия
    blaster,    // простой бластер
    spread,     //Веерная пушка, стреляющая несколькими снарядами
    phaser,     //[HP] Волновой фазер
    missile,    //[HP] Самонаводящиеся ракеты
    laser,      //[HP] Наносит повреждения при долговременном воздействии
    shield      //Увеличивает shieldLevel
}

[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;       //Буква на кубике, изображающем бонус

    public Color color = Color.white;  //Цвет ствола оружия и кубика бонуса 
    public GameObject projectilePrefab; //шаблон снарядов
    public Color projectileColor = Color.white;
    public float damageOnHit = 0; //разрушительная мощность
    public float continuousDamage = 0;  //Степень разрушения в секунду для (Laser)

    public float delayBetweenShots = 0;
    public float velocity = 20; //Скорость полета снарядов

}
public class Weapon : MonoBehaviour
{
    /// summary
    /// Это перечисление всех возможных типов оружия.
    /// Также включает тип "shield", чтобы дать возможность совершенствовать защиту.
    /// Аббревиатурой [HP] ниже отмечены элементы, не реализованные в этой книге.
    /// </summary>
    
}
