public enum DreamworldVoidEventEnum
{
    DEATH,
    COLLECT,
    RESET_TEMP_COLLECT,
    CHECKPOINT_ENTER,
    DIALOGUE_END,
    GAME_END,
    GAME_START,
    COUNTDOWN_FINISH,
    INPUT_DASH,
    INPUT_PAUSE,
    REGISTER_COLLECTABLE
}

public enum DreamworldVector3EventEnum
{
    CHECKPOINT_POSITION
}

public enum DreamworldBoolEventEnum
{
    PAUSE
}

public enum DreamworldEventResponseType
{
    VOID,
    VECTOR3,
    COLLECTABLE
}