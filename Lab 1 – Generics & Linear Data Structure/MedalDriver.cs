using System;

public class Class1
{
	public static void Main(string[] args)
	{



        var list = new SinglyLinkedList<Athlete>();
        var lines = File.ReadAllLines("Medals.csv", System.Text.Encoding.Latin1);

        for (int i = 1; i < lines.Length; i++)
        {
            var tokens = lines[i].Split(',');
            if (tokens.Length < 5) continue;

            var athlete = new Athlete();
            athlete.name = tokens[0].Trim();
            athlete.year = int.Parse(tokens[1].Trim());
            athlete.goldMedals = int.Parse(tokens[2].Trim());
            athlete.silverMedals = int.Parse(tokens[3].Trim());
            athlete.bronzeMedals = int.Parse(tokens[4].Trim());






            list.InsertAtEnd(athlete);
        }


        Console.WriteLine("Athlete                       | Year | Gold | Silver | Bronze");
        Console.WriteLine(new string('-', 60));
        list.printAll();
    }
}
