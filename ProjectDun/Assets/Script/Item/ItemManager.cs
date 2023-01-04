using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    [SerializeField] DropUIManager dropUIManager;
    // �÷��̾� ���� ����.. inven?
    [SerializeField] PlayerMovement player;
    [SerializeField] Inventory inven;
    [SerializeField] Item[] dropItems;
    [SerializeField] DropItem dropitem;
    private void Awake()
    {
        Instance = this;
    }

    public void DropItem(EnemyController enemy)
    {

        dropItems = enemy.enemyInfo.dropItem;
        int num = Random.Range(1, 100);
        if (num < 20)
        {
            Debug.Log("아이템미드랍");
        }
        else if (num >= 20)
        {

            inven.gold += DecideGold(enemy);

            Vector3 enemyPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 3f, enemy.transform.position.z);

            DropItem newDropItem = Instantiate(dropitem, enemyPos, enemy.transform.rotation);
            newDropItem.player = player;
            newDropItem.SetUp(DecideItem(dropItems));
        }
    }
    public Item DecideItem(Item[] items)
    {
        int num = Random.Range(1, 100);
        Item newItem = null;
        try
        {
            if (num <= 20)
            {
                newItem = items[0];
            }
            if (20 < num && num <= 40)
            {
                newItem = items[1];
            }
            if (40 < num && num <= 60)
            {
                newItem = items[2];
            }
            if (60 < num && num <= 80)
            {
                newItem = items[3];
            }
            if (80 < num && num <= 100)
            {
                newItem = items[4];
            }
        }
        catch
        {

            return null;
        }
        return newItem;
    }
    public int DecideGold(EnemyController enemy)
    {
        int gold = Mathf.RoundToInt((float)(enemy.enemyInfo.level * 1.5));

        return gold;
    }
}
