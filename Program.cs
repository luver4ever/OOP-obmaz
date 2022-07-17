using System;

namespace Napilnik
{
    class Program
    {
        static void Main(string[] args)
        {
            ShotGun shotGun = new ShotGun(4, 4);
            Bot bot = new Bot(shotGun);
            PlayerHealth player = new PlayerHealth(new Health(10), 1);


            while (true)
            {
                bot.OnSeePlayer(player);

                Console.ReadKey();
            }
        }
        interface IWeapon
        {
            void Fire(PlayerHealth player);
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

            public void Fire(PlayerHealth player)
            {
                _bullets -= 1;

                if (_bullets < 0)
                    Reload(_ammoSize);

                player.TakeDamage(_damage);
            }
        }
        class ShotGun : Weapon
        {
            public ShotGun(int ammoSize, int damage) : base(ammoSize, damage)
            {
            }
        }
        /////////////////////////////////////////////////////////
        // Decorator Pattern on Health and IDamagable #EXAMPLE //
        /////////////////////////////////////////////////////////
        #region Decorator
        interface IDamagable
        {
            void TakeDamage(int damage);
        }
        
        class Health : IDamagable
        {
            public int Value { get; private set; }

            public Health(int value)
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                Value = value;
            }
            public void TakeDamage(int damage)
            {
                if (damage < 0)
                    throw new ArgumentOutOfRangeException(nameof(damage));

                Value -= damage;

                if (Value < 0)
                    Value = 0;

                Console.WriteLine($"{this} : I taked Damage({damage}) and my health now equal {Value}");

                if (Value == 0)
                    Console.WriteLine("Я сдох");
                
            }
        }

        sealed class PlayerHealth : IDamagable 
        {
            private readonly int _armor;
            private readonly IDamagable _health; 

            public PlayerHealth(IDamagable health, int armor) 
            {
                if (armor < 0)
                    throw new ArgumentOutOfRangeException(nameof(armor));

                _armor = armor;
                _health = health;
            }

            public void TakeDamage(int damage)
            {
                _health.TakeDamage(damage - _armor);
            }
        }
        #endregion
        sealed class Bot 
        {
            private readonly IWeapon _weapon;

            public Bot(IWeapon weapon) 
            {
                _weapon = weapon;
            }

            public void OnSeePlayer(PlayerHealth player)
            {
                _weapon.Fire(player);
            }

        }
    }
}
