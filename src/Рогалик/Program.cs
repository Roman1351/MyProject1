using System;
using System.Collections.Generic;

public class Player
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public Aid Aid { get; set; }
    public Weapon Weapon { get; set; }
    public int Score { get; set; }

    public Player(string name, int maxHealth)
    {
        Name = name;
        MaxHealth = maxHealth;
        Health = maxHealth;
        Score = 0;
    }

    public void Heal()
    {
        if (Aid != null)
        {
            int healAmount = Aid.HealAmount;
            Health = Math.Min(Health + healAmount, MaxHealth);
            Console.WriteLine($"{Name} использовал аптечку. Здоровье восстановлено на {healAmount} hp.");
            Aid = null;
        }
        else
        {
            Console.WriteLine("У тебя нет аптечки!");
        }
    }

    public void Attack(Enemy enemy)
    {
        if (Weapon != null)
        {
            int damage = Weapon.Damage;
            enemy.TakeDamage(damage);
            Console.WriteLine($"{Name} ударил противника {enemy.Name}. Урон: {damage} hp.");
        }
        else
        {
            Console.WriteLine("У тебя нет оружия!");
        }
    }
}

public class Enemy
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public Weapon Weapon { get; set; }


    public Enemy(string name, int maxHealth, Weapon weapon)
    {
        Name = name;
        MaxHealth = maxHealth;
        Health = maxHealth;
        Weapon = weapon;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0) Health = 0;
    }

    public void Attack(Player player)
    {
        if (Weapon != null)
        {
            int damage = Weapon.Damage;
            player.Health -= damage;
            Console.WriteLine($"Противник {Name} ударил тебя! Урон: {damage} hp.");
        }
    }
}

public class Aid
{
    public string Name { get; set; }
    public int HealAmount { get; set; }

    public Aid(string name, int healAmount)
    {
        Name = name;
        HealAmount = healAmount;
    }
}

public class Weapon
{
    public string Name { get; set; }
    public int Damage { get; set; }
    public int Durability { get; set; }

    public Weapon(string name, int damage)
    {
        Name = name;
        Damage = damage;
    }
}

public class Game
{
    public static void Main(string[] args)
    {
        Random rnd = new Random();

        Console.WriteLine("Добро пожаловать, воин!");
        Console.Write("Назови себя: ");
        string playerName = Console.ReadLine();

        Player player = new Player(playerName, 100);
        player.Weapon = GenerateWeapon(rnd);
        player.Aid = new Aid("Средняя аптечка", 10);

        Console.WriteLine($"\nВаше имя {player.Name}!");
        Console.WriteLine($"Вам был ниспослан {player.Weapon.Name} ({player.Weapon.Damage}), а также {player.Aid.Name} ({player.Aid.HealAmount}hp).");
        Console.WriteLine($"У вас {player.Health}hp.");


        while (player.Health > 0)
        {
            Enemy enemy = GenerateEnemy(rnd);
            Console.WriteLine($"\n{player.Name} встречает врага {enemy.Name} ({enemy.Health}hp), у врага на поясе сияет оружие {enemy.Weapon.Name} ({enemy.Weapon.Damage})");

            while (player.Health > 0 && enemy.Health > 0)
            {
                Console.WriteLine("\nЧто вы будете делать?");
                Console.WriteLine("1. Ударить");
                Console.WriteLine("2. Пропустить ход");
                Console.WriteLine("3. Использовать аптечку");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        player.Attack(enemy);
                        if (enemy.Health > 0) enemy.Attack(player);
                        break;
                    case "2":
                        enemy.Attack(player);
                        break;
                    case "3":
                        player.Heal();
                        break;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
                Console.WriteLine($"У противника {enemy.Health}hp, у вас {player.Health}hp");
            }

            if (enemy.Health <= 0)
            {
                player.Score += enemy.MaxHealth;
                Console.WriteLine($"\nТы победил! Набрано {enemy.MaxHealth} очков! Текущий счет: {player.Score}");
            }
            else
            {
                Console.WriteLine("\nGAME OVER!");
                break;
            }
        }
    }

    static Weapon GenerateWeapon(Random rnd)
    {
        string[] weaponNames = { "Меч Фламберг", "Экскалибур", "Топор Варвара" };
        int damage = rnd.Next(10, 21);
        return new Weapon(weaponNames[rnd.Next(weaponNames.Length)], damage);
    }

    static Enemy GenerateEnemy(Random rnd)
    {
        string[] enemyWeapons = { "Булава", "Кинжал", "Дубина" };
        int enemyHealth = rnd.Next(30, 61);
        Weapon enemyWeapon = new Weapon(enemyWeapons[rnd.Next(enemyWeapons.Length)], rnd.Next(5, 16));
        return new Enemy("Варвар", enemyHealth, enemyWeapon);
    }
}
