
public class CollectableInfo
{
    public void UpdateValues(int obtained, int required, int total)
    {
        obtainedCollectables = obtained;
        requiredCollectables = required;
        totalCollectables = total;
    }

    public void ResetValues()
    {
        totalCollectables = 0;
        requiredCollectables = 0;
        obtainedCollectables = 0;
    }

    public int totalCollectables;
    public int requiredCollectables;
    public int obtainedCollectables;

   
    }
