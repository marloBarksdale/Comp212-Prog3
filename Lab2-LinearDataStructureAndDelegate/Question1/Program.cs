using Question1;

public class Program
{
    public static void Main()
    {
        var map = new Map_DoublyLinkedList<string, string>();

        map.Put("USA", "Washington, D.C.");
        map.Put("Germany", "Berlin");
        map.Put("Japan", "Tokyo");
        map.Put("China", "Beijing");
        map.Put("Canada", "Ottawa");

        Console.WriteLine("\nCapital of Japan: " + map.Get("Japan"));

        Console.WriteLine("\nRemoving Germany...");
        map.Remove("Germany");

        Console.WriteLine("\nAll values:");


        Console.WriteLine(map.ToString());

    }
}
