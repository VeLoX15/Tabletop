using Tabletop.Core.Constants;
using Tabletop.Core.Models;

namespace Tabletop.Core.Calculators
{
    public class Calculation
    {
        public static int Force(Unit unit)
        {
            return (unit.PrimaryWeapon.Attack * unit.PrimaryWeapon.Quality * (unit.PrimaryWeapon.Range / 10) * unit.PrimaryWeapon.Dices) + (unit.Defense * 50 + unit.Moving * 4) * 2;
        }

        //public List<string> Simulation(Unit Unit1, int quantityUnit1, CoverTypes CoverUnit1, Unit Unit2, int quantityUnit2, CoverTypes CoverUnit2, int distance)
        //{
        //    Random r = new();
        //    List<string> result = new();

        //    for (int i = 1; quantityUnit1 >= 0 || quantityUnit2 >= 0; i++)
        //    {
        //        if (i % 2 == 0)
        //        {
        //            for (int j = 1; j <= quantityUnit1; j++)
        //            {
        //                double x = Probability(Unit1, Unit2, CoverUnit1, distance);


        //            }
        //        }
        //        else if (i % 2 == 1)
        //        {
        //            for (int j = 1; j <= quantityUnit2; j++)
        //            {
        //                double x = Probability(Unit1, Unit2, CoverUnit2, distance);
        //            }
        //        }

        //        result.Add("");
        //    }
        //    return result;
        //}

        public double Probability(Unit attacker, Unit defender, CoverTypes cover, int distance)
        {
            double x = 0;
            var (value0, value1) = AttackValueTranslator(attacker.PrimaryWeapon.Attack, defender.Defense);

            x = ((double)attacker.PrimaryWeapon.Quality / 8) * ((9 - (double)value0) / 8);
            if (value1 != 0)
            {
                x *= ((9 - (double)value1) / 8);
            }

            if (Convert.ToInt32(cover) == 1)
            {
                x *= 0.285;
            }
            else if (Convert.ToInt32(cover) == 2)
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