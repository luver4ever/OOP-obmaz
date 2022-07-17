using System;

namespace Napilnik
{
    class Program
    {
        static void Main(string[] args)
        {
            Weapon weapon = new Weapon(2, 2);
            Bot bot = new Bot(weapon, 10);
            Player player = new Player(10);

            while (player.IsAlive == true)
            {
                bot.OnSeePlayer(player);

                Console.ReadKey();
            }
        }
        interface IWeapon
        {
            void Fire(Creature player);
        }
        class Weapon : IWeapon
        {
            private int _bullets;
            private readonly int _damage;
            private readonly int _ammoSize;

            public Weapon(int ammoSize, int damage)
            {
                if (damage < 0)
                    throw new ArgumentOutOfRangeException(nameof(damage));
                if (ammoSize < 0)
                    throw new ArgumentOutOfRangeException(nameof(ammoSize));

                _ammoSize = ammoSize;
                Reload(_ammoSize);

                _damage = damage;
            }

            private void Reload(int ammoSize)
            {
                _bullets = ammoSize;

                Console.WriteLine($"{this} : Id reloaded");
            }

            public void Fire(Creature player)
            {
                _bullets -= 1;

                if (_bullets < 0)
                    Reload(_ammoSize);

                player.TakeDamage(_damage);
            }
        }

        interface IDamagable
        {
            void TakeDamage(int damage);
        }

        public abstract class Creature : IDamagable
        {
            public int Health { get; private set; }

            public bool IsAlive { get; private set; }

            public Creature(int health)
            {
                IsAlive = true;

                if (health < 0)
                    throw new ArgumentOutOfRangeException(nameof(health));

                Health = health;
            }
            public void TakeDamage(int damage)
            {
                if (damage < 0)
                    throw new ArgumentOutOfRangeException(nameof(damage));

                Health -= damage;

                Console.WriteLine($"{this} : I taked Damage({damage}) and my health now equal {Health}");

                if (Health <= 0)
                {
                    Console.WriteLine("Я сдох");

                    IsAlive = false;
                }
            }
        }

        class Player : Creature
        {
            public Player(int health) : base(health)
            {
                
            }
        }

        class Bot : Creature
        {
            private readonly IWeapon _weapon;

            public Bot(IWeapon weapon, int health) : base(health)
            {
                _weapon = weapon;
            }

            public void OnSeePlayer(Creature player)
            {
                _weapon.Fire(player);
            }

        }
    }
}
