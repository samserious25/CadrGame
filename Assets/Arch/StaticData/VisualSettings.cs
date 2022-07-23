using UnityEngine;

[CreateAssetMenu(fileName = "VisualSettings", menuName = "StaticData/VisualSettings", order = 3)]
public class VisualSettings : ScriptableObject
{
    public float CardRotateTime = 0.35f;
    public float CardFadeOutTime = 0.5f;
    public float DissolveEdgeWidth = 0.5f;
    public float BonusFoundDelay = 0.5f;
    public float BonusShowWithRotateTime = 1f;
}
