using LibraryTrumpTower.AirUnits;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower;
using TrumpTower.LibraryTrumpTower.Constants;

namespace LibraryTrumpTower.SpecialAbilities
{
    [DataContract(IsReference = true)]
    public class Sniper
    {
        #region Fields
        [DataMember]
        public Map Ctx { get; private set; }
        [DataMember]
        public int Cost { get; private set; }
        [DataMember]
        public double Damage { get; private set; }
        [DataMember]
        public int Reload { get; private set; }
        #endregion

        public Sniper (Map map)
        {
            Ctx = map;
            Cost = 25;
            Damage = 50;
            Reload = 0;
        }

        public void Update()
        {
            if (!IsReload) Reload--;
        }

        public void AttackOn(Vector2 position)
        {
            if (!IsReload) throw new Exception();

            /* On analyse tous les ennemis Air, on enregistre dans un tableau sur toutes les sprites sur lesquelles on a cliqué */
            List<AirUnit> _airUnits = Ctx.GetAllAirEnemies();
            List<AirUnit> _candidateAirUnits = new List<AirUnit>();
            AirUnit unit;
            for (int i = 0; i < _airUnits.Count; i++)
            {
                unit = _airUnits[i];
                if (position.X > unit.Position.X && position.X < unit.Position.X + Constant.ImgSizePlane &&
                    position.Y > unit.Position.Y && position.Y < unit.Position.Y + Constant.ImgSizePlane)
                {
                    _candidateAirUnits.Add(unit);
                }
            }
            /* On enregistre ensuite le plus près */
            AirUnit _candidateUnit = null;
            if (_candidateAirUnits.Count > 0)
            {
                _candidateUnit = _candidateAirUnits[0];
                double lastDistanceAir = Vector2.Distance(position, _candidateUnit.Position);
                for (int i = 1; i < _candidateAirUnits.Count; i++)
                {
                    if (Vector2.Distance(position, _candidateAirUnits[i].Position) < lastDistanceAir)
                        _candidateUnit = _candidateAirUnits[i];
                }
            }

            /* On analyse tous les ennemis au Sol, on enregistre dans un tableau sur toutes les sprites sur lesquelles on a cliqué */
            List<Enemy> _enemies = Ctx.GetAllEnemies();
            List<Enemy> _candidateEnemies = new List<Enemy>();
            Enemy enemy;
            for (int i = 0; i < _enemies.Count; i++)
            {
                enemy = _enemies[i];
                if (position.X > enemy.Position.X && position.X < enemy.Position.X + Constant.ImgSizeEnemy &&
                    position.Y > enemy.Position.Y && position.Y < enemy.Position.Y + Constant.ImgSizeEnemy)
                {
                    _candidateEnemies.Add(enemy);
                }
            }
            /* On enregistre ensuite le plus près */
            Enemy _candidateEnemy = null;
            if (_candidateEnemies.Count > 0)
            {
                _candidateEnemy = _candidateEnemies[0];
                double lastDistanceEnemy = Vector2.Distance(position, _candidateEnemy.Position);
                for (int i = 1; i < _candidateEnemies.Count; i++)
                {
                    if (Vector2.Distance(position, _candidateEnemies[i].Position) < lastDistanceEnemy)
                        _candidateEnemy = _candidateEnemies[i];
                }
            }

            /* On regarde lequelle est le plus près entre ennemi Air et ennemi Sol */
            if (_candidateEnemy != null && _candidateUnit != null) AttackAir(_candidateUnit);
            else if (_candidateEnemy == null && _candidateUnit != null) AttackAir(_candidateUnit);
            else if (_candidateUnit == null && _candidateEnemy != null) AttackEarthly(_candidateEnemy);

            // On retire les dollars
            Ctx.Dollars -= Cost;
        }

        public void AttackAir(AirUnit enemy)
        {
            Reload = 1 * 0;
            enemy.TakeHp(Damage);
            if (enemy.IsDead) enemy.Die();
        }

        public void AttackEarthly(Enemy enemy)
        {
            Reload = 1 * 60;
            enemy.TakeHp(Damage);
            if (enemy.IsDead) enemy.Die(); 
        }

        public bool IsReload => Reload <= 0;
    }
}
