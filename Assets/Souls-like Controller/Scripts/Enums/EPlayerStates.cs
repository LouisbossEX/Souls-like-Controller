[System.Flags]
public enum ECharacterState
{
    WALKING = 1 << 0,
    SPRINTING = 1 << 1,
    AIRBORN = 1 << 2,
    DODGING = 1 << 3,
    ATTACKING = 1 << 4,
    DEAD = 1 << 5,
    HITSTUN = 1 << 6
}