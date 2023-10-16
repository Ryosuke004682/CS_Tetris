using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting.FullSerializer.Internal;

/*ゲームの基盤となるロジックを書いていくよ*/
public class Game : MonoBehaviour
{
    //フィールドの大きさを定義してるよ
    const int FieldXLength     = 10;
    const int FieldYLength     = 20;
    const int NextFieldXLength =  4;//追加
    const int NextFieldYLength =  4;//追加

    [Tooltip("何秒毎で落下させるか設定出来る")]
    public float FallInterval = 0.3f;//何秒毎で落下させるのかを定義

    private DateTime      lastFallTime;
    private DateTime lastControlleTime;


    //ゲームオブジェクトを格納するよ
    [SerializeField]
    private GameObject field;

    //主役のゲームオブジェクトを格納する。（今回だったらSquereプレハブ）
    [SerializeField]
    private SpriteRenderer squerePrefab;

    //次に落ちてくるミノを格納するよ。
    [SerializeField]
    private GameObject nextField;

    //ブロックをX軸、Y軸にどれくらい生成するかどうかを格納する奴
    private SpriteRenderer[,] blockReadereObjects;
    private SpriteRenderer[,]    nextBlockObjects;
    
    private Tetrimino tetrimino     = new Tetrimino();
    private Tetrimino nextTetrimino = new Tetrimino();
    
    
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
        ControlTetrimino();
        Draw();

        DateTime now = DateTime.UtcNow;//現在の時間を取得

        //現在の時刻から最後に落下した時間を秒に変換し、FallIntervalよりも小さかったら特に処理をおこなわずに処理を抜ける。
        if ((now - lastFallTime).TotalSeconds < FallInterval) { return; }

        lastFallTime = now;

        if (!TryMoveTetrimino(0, 1))
        {
            Vector2Int[] positions = tetrimino.GetBlockPositions();

            foreach(Vector2Int position in positions)
            {
                fieldBlocks[position.y, position.x] = nextTetrimino.BlockType;
            }

            tetrimino.Initialize(nextTetrimino.BlockType);
            nextTetrimino.Initialize();
        }
    }

    /// <summary>
    /// テトリミノをユーザーの入力から操作させてるところ。
    /// </summary>
    private bool ControlTetrimino()
    {
        DateTime now = DateTime.UtcNow;

        if ((now - lastControlleTime).TotalSeconds < 0.1f) { return false; }


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if(TryMoveTetrimino(-1 , 0))
            {
                lastControlleTime = now;
                return true;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (TryMoveTetrimino(1, 0))
            {
                lastControlleTime = now;
                return true;
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (TryMoveTetrimino(0, 1))
            {
                lastControlleTime = now;
                return true;
            }
        }
        return false;
    }



    /// <summary>
    /// 動かしてる所
    /// </summary>
    /// <returns></returns>
    private bool TryMoveTetrimino(int moveX , int moveY)
    {
        if(CanMoveTetrimino(moveX , moveY))
        {
            tetrimino.FallingMove(moveX , moveY);
            return true;
        }
        return false;

    }



    /// <summary>
    /// 動かせる範囲の判定
    /// </summary>
    /// <param name="moveX"> X軸方向にどれくらい動かすか </param>
    /// <param name="moveY"> Y軸方向にどれくらい動かすか </param>
    /// <returns></returns>
    private bool CanMoveTetrimino(int moveX , int moveY)
    {
        Vector2Int[] blockPositions = tetrimino.GetBlockPositions();

        foreach(Vector2 blockPosition in blockPositions)
        {
            float x = blockPosition.x + moveX;
            float y = blockPosition.y + moveY;

            //画面外に行かないように判定
            if      (x < 0 || FieldXLength <= x) { return false; }
            else if (y < 0 || FieldYLength <= y) { return false; }

            if (fieldBlocks[(int)y, (int)x] != BlockType.None) { return false; }
        }
        return true;
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

        //追加
        nextBlockObjects = new SpriteRenderer[NextFieldYLength , NextFieldXLength];
        for(int y = 0; y < NextFieldYLength; y++)
        {
            for (int x = 0; x < NextFieldXLength; x++)
            {
                SpriteRenderer block = Instantiate(squerePrefab, nextField.transform);
                block.transform.localPosition = new Vector3(x- 1.5f, y - 1.5f, 0);//画角調整
                block.transform.localRotation = Quaternion.identity;//生成したときに変な方向を向かないようにローテーションの値を固定
                block.transform.localScale = Vector3.one;         //スケールも全部1のままでいいので1に設定
                block.color = Color.black;
                nextBlockObjects[y, x] = block;
            }
        }
        fieldBlocks      = new BlockType[FieldYLength, FieldXLength];
    }



    /// <summary>
    /// 0.3秒ごとに1ブロックずつ落としていく　追加したとこ
    /// </summary>
    private void UpdateLocation()
    {
        tetrimino.Initialize();
        nextTetrimino.Initialize();
        lastFallTime      = DateTime.UtcNow;
        lastControlleTime = DateTime.UtcNow;

        for(int y = 0; y < FieldYLength; y++)
        {
            for(int x = 0; x < FieldXLength; x++)
            {
                fieldBlocks[y, x] = BlockType.None;
            }
        }
    }



    /// <summary>
    /// テトリミノを描画してるところ　追加したとこ
    /// </summary>
    private void Draw()
    {
        //フィールドを描画
        for (int y = 0; y < FieldYLength; y++)
        {
            for (int x = 0; x < FieldXLength; x++)
            {
                SpriteRenderer blockObj  = blockReadereObjects[y , x];
                Game.BlockType blockType = fieldBlocks[y , x];

                blockObj.color = GetBlockColor(blockType);
            }
        }


        //テトリミノを描画
        {
            Vector2Int[] positions = tetrimino.GetBlockPositions();
            foreach (Vector2Int position in positions)
            {
                SpriteRenderer tetriminoBlock = blockReadereObjects[position.y, position.x];
                tetriminoBlock.color = GetBlockColor(tetrimino.BlockType);
            }
        }


        //Nextフィールドを描画
        for(int y = 0; y < NextFieldXLength; y++)
        {
            for(int x = 0; x < NextFieldYLength; x++)
            {
                nextBlockObjects[y, x].color = GetBlockColor(BlockType.None);
            }
        }


        //Nextテトリミノを描画
        {
            Vector2Int[] nextTetrimino_Positions = nextTetrimino.GetBlockPositions();
            
            foreach(Vector2Int position in nextTetrimino_Positions)
            {
                SpriteRenderer tetriminoBlock = nextBlockObjects[position.y , position.x + 1];
                tetriminoBlock.color = GetBlockColor(nextTetrimino.BlockType);
            }
        }
    }



    /// <summary>
    /// ブロックに色を付けてるところ 追加したとこ
    /// </summary>
    /// <param name="blockType"> ブロックの種類を引数で持つ </param>
    /// <returns></returns>
    private Color GetBlockColor(BlockType blockType)
    {
        switch (blockType)
        {
            //各形のテトリミノの色分け（テトリスの規則にのっとって設定）
            case BlockType.None       : return Color.black;
            case BlockType.TetriminoI : return Color.cyan;
            case BlockType.TetriminoO : return Color.yellow;
            case BlockType.TetriminoT : return Color.magenta;
            case BlockType.TetriminoJ : return Color.blue;
            case BlockType.TetriminoL : return new Color(1.0f , 0.5f , 0.0f);
            case BlockType.TetriminoS : return Color.green;
            case BlockType.TetriminoZ : return Color.red;

            default : return Color.white;//例外処理
        }
    }


}
