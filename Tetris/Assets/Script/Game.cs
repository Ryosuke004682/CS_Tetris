using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*ゲームの基盤となるロジックを書いていくよ*/
public class Game : MonoBehaviour
{
    //フィールドの大きさを定義してるよ
    const int FieldXLength   = 10;
    const int FieldYLength   = 20;

    const float FallInterval = 0.3f;//何秒毎で落下させるのかを定義

    private DateTime lastFallTime;

    //ゲームオブジェクトを格納するよ
    [SerializeField]
    private GameObject field;

    //主役のゲームオブジェクトを格納する。（今回だったらSquereプレハブ）
    [SerializeField]
    private SpriteRenderer squerePrefab;

    //ブロックをX軸、Y軸にどれくらい生成するかどうかを格納する奴
    private SpriteRenderer[,] blockReadereObjects;
    
    private Tetrimino tetrimino = new Tetrimino();
    
    
    private BlockType[,] fieldBlocks;
    //enum = 列挙型。名前の通り処理を列挙出来ます。
    public enum BlockType
    {
        None       = 0,
        TetriminoI = 1,
        TetriminoO = 2,
        TetriminoS = 3,
        TetriminoZ = 4,
        TetriminoJ = 5,
        TetriminoL = 6,
        TetriminoT = 7
    }


    private void Start()
    {
        GameInitialSetting();
        UpdateLocation();
        Draw();
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

        fieldBlocks = new BlockType[FieldYLength, FieldXLength];

    }


    /// <summary>
    /// 0.3秒ごとに1ブロックずつ落としていく
    /// </summary>
    private void UpdateLocation()
    {
        tetrimino.Initialize();

        lastFallTime = DateTime.UtcNow;

        for(int y = 0; y < FieldYLength; y++)
        {
            for(int x = 0; x < FieldXLength; x++)
            {
                fieldBlocks[y, x] = BlockType.None;
            }
        }
    }


    /// <summary>
    /// 落とした先で描画されてないと困るから描画処理をかける
    /// </summary>
    private void Draw()
    {
        for (int y = 0; y < FieldYLength; y++)
        {
            for (int x = 0; x < FieldXLength; x++)
            {
                SpriteRenderer blockObj  = blockReadereObjects[y , x];
                Game.BlockType blockType = fieldBlocks[y , x];

                blockObj.color = GetBlockColor(blockType);//描画させる
            }
        }

        Vector2Int[] positions = tetrimino.GetBlockPositions();

        foreach (Vector2Int position in positions)
        {
            SpriteRenderer tetriminoBlock = blockReadereObjects[position.y , position.x];
            tetriminoBlock.color = GetBlockColor(tetrimino.BlockType);
        }

    }


    /// <summary>
    /// ブロックに色を付けてるところ
    /// </summary>
    /// <param name="blockType"> ブロックの種類を引数で持つ </param>
    /// <returns></returns>
    private Color GetBlockColor(BlockType blockType)
    {
        switch (blockType)
        {
            case BlockType.None       : return Color.black;
            case BlockType.TetriminoI : return Color.cyan;

            default : return Color.white;//例外処理
        }
    }
}
