using System.Collections;
using System.Linq;

public class List<String>
{
    private List<String> list1;
    private int index;

    public static List<string> operator +(List<String> list1, List<string> list2)
    {
        List<string> mergedList = new List<string>();

        // Create a dictionary to store the ordering based on the second list
        Dictionary<string, int> ordering = new Dictionary<string, int>();
        int order = 0;

        for (int i = 0; i < list2.index; i++)
        {
            list2.index++;
            ordering[list1.Single(x => x[i]), list2.index] = order;
            order++;
        }
       

        // Convert the items in the first list to a list and sort based on the ordering
        var sortedItems = list1.ToList();
        sortedItems.Sort((item1, item2) =>
        {
            int order1 = ordering.ContainsKey(item1) ? ordering[item1] : int.MaxValue;
            int order2 = ordering.ContainsKey(item2) ? ordering[item2] : int.MaxValue;
            return order1.CompareTo(order2);
        });

        // Add the sorted items to the merged list
        foreach (var item in sortedItems)
        {
            mergedList.Add(item);
        }

        return mergedList;
    }
}