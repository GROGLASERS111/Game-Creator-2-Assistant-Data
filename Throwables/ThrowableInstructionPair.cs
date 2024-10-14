using GameCreator.Runtime.VisualScripting;

[System.Serializable]
public struct ThrowableInstructionPair
{
    public string requiredThrowableID;
    public InstructionList onHit;
}
