﻿using Tabletop.Models;

namespace Tabletop.Calculators
{
    public class Calculation
    {
        //public int Force(Unit unit)
        //{
        //    return (unit.Weapon.Attack * unit.Weapon.Quality * (unit.Weapon.Range / 10) * unit.Weapon.Dices) + (unit.Defense * 50 + unit.Moving * 4) * 2;
        //}


        //public void Simulation(Unit attackerTyp, int numberOfAttackers, Unit defenderTyp, int numberOfDefenders, bool cover, int distance)
        //{
        //    Random r = new Random();

        //    for (int i = 0; i < numberOfAttackers; i++)
        //    {
        //        int rInt = r.Next(0, 100);
        //        double x = Probability(attackerTyp, defenderTyp, cover, distance);
        //    }


        //}

        //public double Probability(Unit attacker, Unit defender, bool cover, int distance)
        //{
        //    double x = 0;
        //    var (value0, value1) = AttackValueTranslator(attacker, defender);

        //    x = ((double)attacker.Weapon.Quality / 8) * ((9 - (double)value0) / 8);
        //    if (value1 != 0)
        //    {
        //        x *= ((9 - (double)value1) / 8);
        //    }
        //    if (cover)
        //    {
        //        x *= 0.375;
        //    }

        //    return x;
        //}

        //public (int, int) AttackValueTranslator(Unit attacker, Unit defender)
        //{
        //    int x = 5;

        //    if (attacker.Weapon.Attack > defender.Defense)
        //    {
        //        x -= attacker.Weapon.Attack - defender.Defense;
        //    }
        //    else if (attacker.Weapon.Attack < defender.Defense)
        //    {
        //        x += defender.Defense - attacker.Weapon.Attack;
        //    }
        //    if (x > 8)
        //    {
        //        return (8, x - 6);
        //    }
        //    if (x < 2)
        //    {
        //        x = 2;
        //    }
        //    return (x, 0);
        //}
    }
}
