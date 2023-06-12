using System;

[Flags]
public enum ECharacterType
{
    PLAYER = 1 << 0,
    ALLY = 1 << 1,
    NEUTRAL = 1 << 2,
    ENEMY = 1 << 3,
}
