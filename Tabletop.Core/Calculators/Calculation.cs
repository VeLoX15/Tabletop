using Tabletop.Core.Models;

namespace Tabletop.Core.Calculators
{
    public class Calculation
    {
        public static int Force(Unit unit)
        {
            return (unit.PrimaryWeapon.Attack * unit.PrimaryWeapon.Quality * (unit.PrimaryWeapon.Range / 10) * unit.PrimaryWeapon.Dices) + (unit.Defense * 50 + unit.Moving * 4) * 2;
        }


        public void Simulation(Unit attackerTyp, int numberOfAttackers, Unit defenderTyp, int numberOfDefenders, bool cover, int distance)
        {
            Random r = new();

            for (int i = 0; i < numberOfAttackers; i++)
            {
                int randNum = r.Next(0, 100);
                double x = Probability(attackerTyp, defenderTyp, cover, distance);
            }


        }

        public double Probability(Unit attacker, Unit defender, bool cover, int distance)
        {
            double x = 0;
            var (value0, value1) = AttackValueTranslator(attacker.PrimaryWeapon.Attack, defender.Defense);

            x = ((double)attacker.PrimaryWeapon.Quality / 8) * ((9 - (double)value0) / 8);
            if (value1 != 0)
            {
                x *= ((9 - (double)value1) / 8);
            }
            if (cover)
            {
                x *= 0.375;
            }

            return x;
        }

        public static (int, int) AttackValueTranslator(int attacker, int defender)
        {
            int x = 5;

            if (attacker > defender)
            {
                x -= attacker - defender;
            }
            else if (attacker < defender)
            {
                x += defender - attacker;
            }
            if (x > 8)
            {
                return (8, x - 6);
            }
            if (x < 2)
            {
                x = 2;
            }
            return (x, 0);
        }
    }
}
