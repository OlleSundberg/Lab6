using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int MessageID = 1;

        List<string> Names = new List<string>() { "Emma", "Noah", "Olivia", "Liam", "Ava", "William", "Sophia", "Mason", "Isabella", "James", "Mia", "Benjamin", "Charlotte", "Jacob", "Abigail", "Michael", "Emily", "Elijah", "Harper", "Ethan", "Amelia", "Alexander", "Evelyn", "Oliver", "Elizabeth", "Daniel", "Sofia", "Lucas", "Madison", "Matthew", "Avery", "Aiden", "Ella", "Jackson", "Scarlett", "Logan", "Grace", "David", "Chloe", "Joseph", "Victoria", "Samuel", "Riley", "Henry", "Aria", "Owen", "Lily", "Sebastian", "Aubrey", "Gabriel", "Zoey", "Carter", "Penelope", "Jayden", "Lillian", "John", "Addison", "Luke", "Layla", "Anthony", "Natalie", "Isaac", "Camila", "Dylan", "Hannah", "Wyatt", "Brooklyn", "Andrew", "Zoe", "Joshua", "Nora", "Christopher", "Leah", "Grayson", "Savannah", "Jack", "Audrey", "Julian", "Claire", "Ryan", "Eleanor", "Jaxon", "Skylar", "Levi", "Ellie", "Nathan", "Samantha", "Caleb", "Stella", "Hunter", "Paisley", "Christian", "Violet", "Isaiah", "Mila", "Thomas", "Allison", "Aaron", "Alexa", "Lincoln", "Anna", "Charles", "Hazel", "Eli", "Aaliyah", "Landon", "Ariana", "Connor", "Lucy", "Josiah", "Caroline", "Jonathan", "Sarah", "Cameron", "Genesis", "Jeremiah", "Kennedy", "Mateo", "Sadie", "Adrian", "Gabriella", "Hudson", "Madelyn", "Robert", "Adeline", "Nicholas", "Maya", "Brayden", "Autumn", "Nolan", "Aurora", "Easton", "Piper", "Jordan", "Hailey", "Colton", "Arianna", "Evan", "Kaylee", "Angel", "Ruby", "Asher", "Serenity", "Dominic", "Eva", "Austin", "Naomi", "Leo", "Nevaeh", "Adam", "Alice", "Jace", "Luna", "Jose", "Bella", "Ian", "Quinn", "Cooper", "Lydia", "Gavin", "Peyton", "Carson", "Melanie", "Jaxson", "Kylie", "Theodore", "Aubree", "Jason", "Mackenzie", "Ezra", "Kinsley", "Chase", "Cora", "Parker", "Julia", "Xavier", "Taylor", "Kevin", "Katherine", "Zachary", "Madeline", "Tyler", "Gianna", "Ayden", "Eliana", "Elias", "Elena", "Bryson", "Vivian", "Leonardo", "Willow", "Greyson", "Reagan", "Sawyer", "Brianna", "Roman", "Clara", "Brandon", "Faith", "Bentley", "Ashley", "Kayden", "Emilia", "Ryder", "Isabelle", "Nathaniel", "Annabelle", "Vincent", "Rylee", "Miles", "Valentina", "Santiago", "Everly", "Harrison", "Hadley", "Tristan", "Sophie", "Declan", "Alexandra", "Cole", "Natalia", "Maxwell", "Ivy", "Luis", "Maria", "Justin", "Josephine", "Everett", "Delilah", "Micah", "Bailey", "Axel", "Jade", "Wesley", "Ximena", "Max", "Alexis", "Silas", "Alyssa", "Weston", "Brielle", "Ezekiel", "Jasmine", "Juan", "Liliana", "Damian", "Adalynn", "Camden", "Khloe", "George", "Isla", "Braxton", "Mary", "Blake", "Andrea", "Jameson", "Kayla", "Diego", "Emery", "Carlos", "London", "Ivan", "Kimberly", "Kingston", "Morgan", "Ashton", "Lauren", "Jesus", "Sydney", "Brody", "Nova", "Emmett", "Trinity", "Abel", "Lyla", "Jayce", "Margaret", "Maverick", "Ariel", "Bennett", "Adalyn", "Giovanni", "Athena", "Eric", "Lilly", "Maddox", "Melody", "Kaiden", "Isabel", "Kai", "Jordyn", "Bryce", "Jocelyn", "Alex", "Eden", "Calvin", "Paige", "Ryker", "Teagan", "Jonah", "Valeria", "Luca", "Sara", "King", "Norah", "Timothy", "Rose", "Alan", "Aliyah", "Brantley", "Mckenzie", "Malachi", "Molly", "Emmanuel", "Raelynn", "Abraham", "Leilani", "Antonio", "Valerie", "Richard", "Emerson", "Jude", "Juliana", "Miguel", "Nicole", "Edward", "Laila", "Victor", "Makayla", "Amir", "Elise", "Joel", "Mariah", "Steven", "Mya", "Matteo", "Arya", "Hayden", "Ryleigh", "Patrick", "Adaline", "Grant", "Brooke", "Preston", "Rachel", "Tucker", "Eliza", "Jesse", "Angelina", "Finn", "Amy", "Oscar", "Reese", "Kaleb", "Alina", "Gael", "Cecilia", "Graham", "Londyn", "Elliot", "Gracie", "Alejandro", "Payton", "Rowan", "Esther", "Marcus", "Alaina", "Jeremy", "Charlie", "Zayden", "Iris", "Karter", "Arabella", "Beau", "Genevieve", "Bryan", "Finley", "Maximus", "Daisy", "Aidan", "Harmony", "Avery", "Anastasia", "Elliott", "Kendall", "August", "Daniela", "Nicolas", "Catherine", "Mark", "Adelyn", "Colin", "Vanessa", "Waylon", "Brooklynn", "Bradley", "Juliette", "Kyle", "Julianna", "Kaden" };

        public ConcurrentQueue<Patron> BarQueue = new ConcurrentQueue<Patron>();
        public ConcurrentBag<Glass> Shelf = new ConcurrentBag<Glass>() { new Glass(), new Glass(), new Glass(), new Glass(), new Glass() };

        public MainWindow()
        {
            InitializeComponent();

            Height += 10;
            Width += 10;

            Bartender bartender = new Bartender(this);
            bartender.Work();
        }
    }

    public class Glass
    {
        public const int Total = 5;
    }
}
