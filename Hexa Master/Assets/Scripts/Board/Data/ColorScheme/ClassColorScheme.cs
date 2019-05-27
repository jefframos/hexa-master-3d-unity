using UnityEngine;

[CreateAssetMenu(menuName = "Classes Color Scheme")]
public class ClassColorScheme : ScriptableObject
{
    [System.Serializable]
    public class ColorClassData
    {
        public ClassType classType;
        public Color classColor;
    }
    public ColorClassData[] classColors;
}

