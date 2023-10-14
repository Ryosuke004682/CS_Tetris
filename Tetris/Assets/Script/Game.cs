using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*ゲームの基盤となるロジックを書いていくよ*/
public class Game : MonoBehaviour
{

    //フィールドの大きさを定義してるよ
    const int FieldXLength = 10;
    const int FieldYLength = 20;


    //ゲームオブジェクトを格納するよ
    [SerializeField]
    private GameObject field;

    //主役のゲームオブジェクトを格納する。（今回だったらSquereプレハブ）
    [SerializeField]
    private SpriteRenderer squerePrefab;

    //ブロックの型を定義してるよ
    private SpriteRenderer[,] blockReadereObjects;


    private void Start()
    {
        GameInitialSetting();
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// 初期設定してるとこ
    /// オブジェクトの生成とか
    /// </summary>
    private void GameInitialSetting()
    {
        //指定された盤面に色を付ける
        blockReadereObjects = new SpriteRenderer[FieldYLength , FieldXLength];

        //オブジェクトを200個あらかじめ生成しておく。理由は下のコメント
        for (int y = 0; y < FieldYLength; y++)
        {
            for(int x = 0; x < FieldXLength; x++)
            {
                //生成して消して、生成して消してだと重すぎるので最初からメモリを開放する設計をする。

                SpriteRenderer block          = Instantiate(squerePrefab, field.transform);
                block.transform.localPosition = new Vector3(x - 4.5f, y - 9.5f, 0);//画角調整
                block.transform.localRotation = Quaternion .identity;//生成したときに変な方向を向かないようにローテーションの値を固定
                block.transform.localScale    = Vector3.one;         //スケールも全部1のままでいいので1に設定
                block.color                   = Color.black;
                blockReadereObjects[y, x]     = block;
            }
        }

    }

}
