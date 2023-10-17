using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

/*�e�g���~�m�̏����������Ă�����*/
public class Tetrimino
{
    const int PatternXLength = 4;
    const int PatternYLength = 4;

    private int rollParttern;

    private Vector2Int basePosition;


    public Game.BlockType BlockType { get; private set; }
    
    private int RotationPartternNum 
    {
        get
        {
            return BlockType == Game.BlockType.TetriminoO ? 1 : 4; 
        } 
    }

    private int NextRotationPattren 
    {
        get 
        {
            return rollParttern + 1 < RotationPartternNum ? rollParttern + 1 : 0; 
        } 
    }


    public void Initialize(Game.BlockType blockType = Game.BlockType.None)
    {
        //blockType�������Ȃ������ꍇ�A
        if (blockType == Game.BlockType.None)
        {
            blockType = (Game.BlockType)Random.Range(1 , 8);
        }

        basePosition  = Vector2Int.zero;
        rollParttern  = 0;
        BlockType     = blockType;
    }

    /// <summary>
    /// �e�g���~�m�̉�]�̃p�����[�^�[���Z�b�g
    /// </summary>
    /// <returns></returns>
    public Vector2Int[] SetBlockPositions()
    {
        return GetBlockPositions(rollParttern);
    }


    /// <summary>
    /// �e�g���~�m�̈ʒu�Ɖ�]���擾
    /// </summary>
    /// <returns></returns>
    public Vector2Int[] GetBlockPositions(int rollParttern)
    {
        Vector2Int[] positions = new Vector2Int[4];
        int[,,] pattern = typePatterns[BlockType];

        int positionIndex = 0;
        
        for(int y = 0; y < PatternYLength; y++)
        {
            for(int x = 0; x < PatternXLength; x++)
            {
                if (pattern[rollParttern , y  , x] == 1)
                {
                    positions[positionIndex] =  new Vector2Int(basePosition.x + x,  basePosition.y + y);
                    positionIndex++;
                }
            }
        }
        return positions;
    }

    /// <summary>
    /// ���������������鏈��
    /// </summary>
    /// <param name="x"> �w���W�̕ϐ� </param>
    /// <param name="y"> �x���W�̕ϐ� </param>
    public void FallingMove(int x , int y)
    {
        basePosition.Set(basePosition.x + x , basePosition.y + y);
    }

    /// <summary>
    /// �ǂꂭ�炢��]���邩�̏���
    /// </summary>
    /// <returns></returns>
    public Vector2Int[] GetRolledBlockposition()
    {
        return GetBlockPositions(NextRotationPattren);
    }

    public void RotationTetrimino()
    {
        rollParttern = NextRotationPattren;
    }
    


    static readonly Dictionary<Game.BlockType, int[,,]> typePatterns = new Dictionary<Game.BlockType, int[,,]>
    {
        {
            Game.BlockType.TetriminoI,
            new int[,,]
            {
                {
                    {0 , 1 , 0 , 0},
                    {0 , 1 , 0 , 0},
                    {0 , 1 , 0 , 0},
                    {0 , 1 , 0 , 0}
                },
                {
                    {0 , 0 , 0 , 0},
                    {1 , 1 , 1 , 1},
                    {0 , 0 , 0 , 0},
                    {0 , 0 , 0 , 0}
                },
                {
                    {0 , 0 , 1 , 0},
                    {0 , 0 , 1 , 0},
                    {0 , 0 , 1 , 0},
                    {0 , 0 , 1 , 0}
                },
                {
                    {0 , 0 , 0 , 0},
                    {1 , 1 , 1 , 1},
                    {0 , 0 , 0 , 0},
                    {0 , 0 , 0 , 0}
                },
            }
        },
        {
            Game.BlockType.TetriminoO,
            new int[,,]
            {
                {
                    {0 , 1 , 1 , 0},
                    {0 , 1 , 1 , 0},
                    {0 , 0 , 0 , 0},
                    {0 , 0 , 0 , 0}
                },
            }
        },
        {
            Game.BlockType.TetriminoS,
            new int[,,]
            {
                {
                    {0 , 1 , 1 , 0},
                    {1 , 1 , 0 , 0},
                    {0 , 0 , 0 , 0},
                    {0 , 0 , 0 , 0}
                },
                {
                    {0 , 1 , 0 , 0},
                    {0 , 1 , 1 , 0},
                    {0 , 0 , 1 , 0},
                    {0 , 0 , 0 , 0}
                },
                {
                    {0 , 0 , 0 , 0},
                    {0 , 1 , 1 , 0},
                    {1 , 1 , 0 , 0},
                    {0 , 0 , 0 , 0}
                },
                {
                    {1 , 0 , 0 , 0},
                    {1 , 1 , 0 , 0},
                    {0 , 1 , 0 , 0},
                    {0 , 0 , 0 , 0}
                },


            }
        },
        {
            Game.BlockType.TetriminoZ,
            new int[,,]
            {
                {
                    {1 , 1 , 0 , 0},
                    {0 , 1 , 1 , 0},
                    {0 , 0 , 0 , 0},
                    {0 , 0 , 0 , 0}
                },
                {
                    {0 , 0 , 1 , 0},
                    {0 , 1 , 1 , 0},
                    {0 , 1 , 0 , 0},
                    {0 , 0 , 0 , 0}
                },
                {
                    {0 , 0 , 0 , 0},
                    {1 , 1 , 0 , 0},
                    {0 , 1 , 1 , 0},
                    {0 , 0 , 0 , 0}
                },
                {
                    {0 , 1, 0 , 0},
                    {1 , 1 , 0 , 0},
                    {1 , 0 , 0 , 0},
                    {0 , 0 , 0 , 0}
                },
            }
        },
        {
            Game.BlockType.TetriminoJ,
            new int[,,]
            {
                {
                    {1 , 0 , 0 , 0},
                    {1 , 1 , 1 , 0},
                    {0 , 0 , 0 , 0},
                    {0 , 0 , 0 , 0}
                },
                {
                    {0 , 1 , 1 , 0},
                    {0 , 1 , 0 , 0},
                    {0 , 1 , 0 , 0},
                    {0 , 0 , 0 , 0}
                },
                {
                    {0 , 0 , 0 , 0},
                    {1 , 1 , 1 , 0},
                    {0 , 0 , 1 , 0},
                    {0 , 0 , 0 , 0}
                },
                {
                    {0 , 1 , 0 , 0},
                    {0 , 1 , 0 , 0},
                    {1 , 1 , 0 , 0},
                    {0 , 0 , 0 , 0}
                },
            }
        },
        {
            Game.BlockType.TetriminoL,
            new int[,,]
            {
                {
                    {0 , 0 , 1 , 0},
                    {1 , 1 , 1 , 0},
                    {0 , 0 , 0 , 0},
                    {0 , 0 , 0 , 0}
                },
                {
                    {0 , 1 , 0 , 0},
                    {0 , 1 , 0 , 0},
                    {0 , 1 , 1 , 0},
                    {0 , 0 , 0 , 0}
                },
                {
                    {0 , 0 , 0 , 0},
                    {1 , 1 , 1 , 0},
                    {1 , 0 , 0 , 0},
                    {0 , 0 , 0 , 0}
                },
                {
                    {1 , 1 , 0 , 0},
                    {0 , 1 , 0 , 0},
                    {0 , 1 , 0 , 0},
                    {0 , 0 , 0 , 0}
                },
            }
        },
        {
            Game.BlockType.TetriminoT,
            new int[,,]
            {
                {
                    {0 , 1 , 0 , 0},
                    {1 , 1 , 1 , 0},
                    {0 , 0 , 0 , 0},
                    {0 , 0 , 0 , 0}
                },
                 {
                    {0 , 1 , 0 , 0},
                    {0 , 1 , 1 , 0},
                    {0 , 1 , 0 , 0},
                    {0 , 0 , 0 , 0}
                },
                  {
                    {0 , 0 , 0 , 0},
                    {1 , 1 , 1 , 0},
                    {0 , 1 , 0 , 0},
                    {0 , 0 , 0 , 0}
                },
                   {
                    {0 , 1 , 0 , 0},
                    {1 , 1 , 0 , 0},
                    {0 , 1 , 0 , 0},
                    {0 , 0 , 0 , 0}
                },

            }
        },
    };

}
