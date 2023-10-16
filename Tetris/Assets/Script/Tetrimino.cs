using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*テトリミノの処理を書いていくよ*/
public class Tetrimino
{
    private Vector2Int basePosition;

    public Game.BlockType BlockType { get; private set; }


    public void Initialize(Game.BlockType blockType = Game.BlockType.None)
    {
        //blockTypeが何もなかった場合、
        if (blockType == Game.BlockType.None)
        {
            //TODOランダムにテトリミノのタイプを決定
            blockType = Game.BlockType.TetriminoI;
        }

        basePosition  = Vector2Int.zero;
        BlockType     = blockType;
    }

    /// <summary>
    /// 縦に4つ並んだテトリミノを作る。
    /// ■
    /// ■
    /// ■
    /// ■のイメージ
    /// </summary>
    /// <returns></returns>
    public Vector2Int[] GetBlockPositions()
    {
        return new Vector2Int[]
        {
            basePosition,                                       //( 0 ，0 )
            new Vector2Int(basePosition.x , basePosition.y + 1),//( 0 , 1 )
            new Vector2Int(basePosition.x , basePosition.y + 2),//( 0 , 2 )
            new Vector2Int(basePosition.x , basePosition.y + 3),//( 0 , 3 )
        };
    }

    /// <summary>
    /// 引数分落下させる処理
    /// </summary>
    /// <param name="x"> Ｘ座標の変数 </param>
    /// <param name="y"> Ｙ座標の変数 </param>
    public void FallingMove(int x , int y)
    {
        basePosition.Set(basePosition.x + x , basePosition.y + y);
    }
}
