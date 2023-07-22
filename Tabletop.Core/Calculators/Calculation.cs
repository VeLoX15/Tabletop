using System.Text;
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

        public async Task<List<string>> Simulation(Unit unit1, int quantityUnit1, CoverTypes coverUnit1, Unit unit2, int quantityUnit2, CoverTypes coverUnit2, int distance)
        {
            List<string> log = new();
            StringBuilder sb = new();

            if (unit1.Id != 0 && unit2.Id != 0 && (distance <= unit1.PrimaryWeapon.Range || distance <= unit2.PrimaryWeapon.Range))
            {
                double x = await Probability(unit1, unit2, coverUnit1);
                double y = await Probability(unit2, unit1, coverUnit2);

                if (unit1.PrimaryWeaponId == unit1.SecondaryWeaponId)
                {
                    unit1.PrimaryWeapon.Dices *= 2;
                }

                if (unit2.PrimaryWeaponId == unit2.SecondaryWeaponId)
                {
                    unit2.PrimaryWeapon.Dices *= 2;
                }

                sb.AppendLine($"Round 0: {unit1.Name} ({quantityUnit1}) VS ({quantityUnit2}) {unit2.Name}");
                log.Add(sb.ToString());

                for (int i = 1; quantityUnit1 > 0 && quantityUnit2 > 0; i++)
                {
                    sb = new();
                    sb.AppendLine($"Round {(i)}:");

                    if (distance <= unit1.PrimaryWeapon.Range)
                    {
                        for (int j = 1; j <= quantityUnit1; j++)
                        {
                            for (int k = 1; k <= unit1.PrimaryWeapon.Dices; k++)
                            {
                                if (x > await RandomNumber())
                                {
                                    quantityUnit2--;
                                }
                            }
                        }
                    }

                    if (distance <= unit2.PrimaryWeapon.Range)
                    {
                        for (int j = 1; j <= quantityUnit2; j++)
                        {
                            for (int k = 1; k <= unit2.PrimaryWeapon.Dices; k++)
                            {
                                if (y > await RandomNumber())
                                {
                                    quantityUnit1--;
                                }
                            }
                        }
                    }

                    if (quantityUnit1 < 0)
                    {
                        quantityUnit1 = 0;
                    }

                    if (quantityUnit2 < 0)
                    {
                        quantityUnit2 = 0;
                    }

                    sb.AppendLine($"{unit1.Name} ({quantityUnit1}) VS ({quantityUnit2}) {unit2.Name}");
                    log.Add(sb.ToString());
                }

                return log;
            }
            log.Add("No Unit in firing range!");
            return log;
        }

        public Task<double> RandomNumber()
        {
            Random random = new();
            double r = random.Next(0, 1000);

            r /= 1000;
            return Task.FromResult(r);
        }

        public Task<double> Probability(Unit attacker, Unit defender, CoverTypes cover)
        {
            var (value0, value1) = AttackValueTranslator(attacker.PrimaryWeapon.Attack, defender.Defense);

            double x = (double)attacker.PrimaryWeapon.Quality / 8 * ((9 - (double)value0) / 8);
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

            return Task.FromResult(x);
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