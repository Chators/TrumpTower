using TrumpTower.LibraryTrumpTower.Constants;
using TrumpTower.LibraryTrumpTower.Spawns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using LibraryTrumpTower.Constants;
using LibraryTrumpTower;
using System.Runtime.Serialization;
using LibraryTrumpTower.SpecialAbilities;

namespace TrumpTower.LibraryTrumpTower
{
    [DataContract(IsReference = true)]
    public class Enemy
    {
        [DataMember]
        readonly Map _map;
        [DataMember]
        readonly Wave _wave;
        [DataMember]
        Vector2 _position;
        [DataMember]
        string _name;
        [DataMember]
        int _moveToState;
        [DataMember]
        public readonly EnemyType _type;
        [DataMember]
        public Move CurrentDirection { get; private set; }
        [DataMember]
        public double CurrentHp { get; private set; }
        [DataMember]
        public double MaxHp { get; private set; }
        [DataMember]
        public double _damage;
        [DataMember]
        readonly double _heal; // for the doc
        [DataMember]
        public double Speed { get; set; }
        [DataMember]
        public double DefaultSpeed { get; private set; }
        [DataMember]
        public int Bounty { get; private set; }
        [DataMember]
        public int TimerBeforeStarting { get; set; }
        [DataMember]
        public double ActionRadius { get; private set; } // doc & mech units
        [DataMember]
        public double _reload; // doc & mech units
        [DataMember]
        public double _healCooldown; // doc only
        [DataMember]
        public bool _hasCast;
        [DataMember]
        public bool _isCasting;
        [DataMember]
        public Tower _towerBeingCast;
        [DataMember]
        public bool _isCharging; // For boss1
        [DataMember]
        public bool _hasCharged; // For boss1
        [DataMember]
        public double _timeBeforeCharging; // For boss1
        [DataMember]
        public double _timeBeforeEndofCastingCharge; // for boss1
        [DataMember]
        public bool _isVulnerable; // for boss1 after breaching wall
        [DataMember]
        public bool _isCastingBoss1;
        [DataMember]
        public bool _canChargeBoss1;
        [DataMember]
        public double _rangeBoss; // range from where bosses attack the base. At 0, they are sitting ontop the base. The higher this number, the further they'll stand
        [DataMember]
        public double _timeofVulnerability;
        [DataMember]
        public WallBoss _WallBoss { get; private set; }
        [DataMember]
        public double _timeBetweenDeaths = 3 * 60; // Bosses need to be both killed < _timeBetweenDeaths else they'll revive
        [DataMember]
        public double _timeBeforeReviving = 5 * 60; // For Drawing Animation
        [DataMember]
        public double _enrageTimer;
        [DataMember]
        public bool _hasEnraged;
        [DataMember]
        public double _defaultReload;
        
       




        public Enemy(Map map, Wave wave, string name, EnemyType type, WallBoss Wallboss)
        {
            _type = type;
            _map = map;
            _wave = wave;
            _name = name;
            _position = wave.Position;
            _moveToState = 0;
            _isCasting = false;
            _WallBoss = null;

            if (type == EnemyType.defaultSoldier)
            {
                CurrentHp = 85;
                MaxHp = 85;
                _damage = 10;
                Speed = 3;
                DefaultSpeed = 3;
                Bounty = 100;
            } else if (type == EnemyType.kamikaze)
            {
                CurrentHp = 200;
                MaxHp = 200;
                _damage = Constant.MaxWallHp;
                Speed = 2.2;
                DefaultSpeed = 2.2;
                Bounty = 200;
            } else if (type == EnemyType.doctor)
            {
                CurrentHp = 120;
                MaxHp = 120;
                _damage = 5;
                Speed = 3;
                DefaultSpeed = 3;
                Bounty = 150;
                _heal = 20;
                ActionRadius = 400;
                _reload = 0;
                _healCooldown = 3;
            } else if(type == EnemyType.saboteur)
            {
                CurrentHp = 50;
                MaxHp = 50;
                _damage = 5;
                Speed = 4;
                DefaultSpeed = 4;
                Bounty = 150;
                ActionRadius = 500;
                _reload = Constant.DisabledTower*60;
                _hasCast = false;
                _towerBeingCast = null;
            }
            else if (type == EnemyType.boss1) // Che Guevarra
            {
                CurrentHp = 500;
                MaxHp = 500;
                _damage = _map.Wall.MaxHp / 4;
                Speed = 1.5;
                _reload = 2 * 60; // attacks every two seconds
                _defaultReload = 2 * 60;
                _isCharging = false;
                _hasCharged = false;
                _timeBeforeCharging = 6 * 60; // When it comes to 0, boss1 starts casting charge 
                _isVulnerable = false;
                _timeofVulnerability = 3 * 60; // Time where boss doesnt move and take *2 dmg before resuming actions.
                _isCastingBoss1 = false;
                _timeBeforeEndofCastingCharge = 3 * 60; // When it comes to 0, boss1 charges, doubling his speed and dammage, build a wall to stop him
                _rangeBoss = 200;
                _WallBoss = Wallboss;

            } else if (type == EnemyType.boss2 || type == EnemyType.boss2_1) // Twin Commies 
            {
                CurrentHp = 20;
                MaxHp = 20;
                _damage = 10 * (1 + _map._timesBeingRevived);
                Speed = 3 * (1 + _map._timesBeingRevived);
                _reload = 2 * 60; // Attacks every two seconds
                _defaultReload = 2 * 60;
                _rangeBoss = 200;                            
            } else if (type == EnemyType.boss3) // Kim Jung Un
            {
                CurrentHp = 200;
                MaxHp = 200;
                _damage = 30;
                Speed = 4;
                _reload = 2 * 60;
                _defaultReload = 2 * 60;
                _rangeBoss = 200;
                _enrageTimer = 10 * 60;  // when Boss enrages, he does double dmg, speed etc..
                _hasEnraged = false;
                /*
                 * 
                 * Beaucoup d'add pour ce boss
                 * Idée qu'il grabbe une tourelle, ça l'arrête pendant 2 secs, puis il arrache la tourelle du sol. 
                 * Une corde qui va jusqu'à la tour et si on tire au sniper dessus, ca la casse et il n'arrache pas la tour.
                 * 
                 * Autre idée : La maison blanche est remplacée par un bouton nucléaire, il faut pas qu'il aille dessus
                 * 
                 * Autre idée : Son premier coup contre la base en range est une charge qui fait genre midlife à la base
                 * 
                 * Autre idée : Ptet un fameux QTE 
                 * */
            }
        }

        

        private void UpdateMove()
        {
            if (_moveToState == ShortestWay.Count - 1 && WithinReach(Position, ShortestWay[_moveToState], Speed)) _position = _map.Wall.Position;
            Vector2 _moveToPosition = ShortestWay[_moveToState];
            if (WithinReach(Position, _moveToPosition, Speed))
            {
                _position = _moveToPosition;
                _moveToState++;
            }

            if (Position.X < _moveToPosition.X)
            {
                _position.X += (int)Speed;
                CurrentDirection = Move.right;
            }
            else if (Position.X > _moveToPosition.X)
            {
                _position.X -= (int)Speed;
                CurrentDirection = Move.left;
            }
            else if (Position.Y < _moveToPosition.Y)
            {
                _position.Y += (int)Speed;
                CurrentDirection = Move.down;
            }
            else if (Position.Y > _moveToPosition.Y)
            {
                _position.Y -= (int)Speed;
                CurrentDirection = Move.top;
            }
        }

       

        public void Update()
        {
            if (!IsStarting) TimerBeforeStarting--;
            else if (_type != EnemyType.boss1 && _type != EnemyType.boss2 && _type != EnemyType.boss3 && _type != EnemyType.boss2_1)
            {
                 UpdateAttackWall();
                 UpdateSaboteur(GetTowers(_position, ActionRadius));

                if (_isCasting == true)
                    StartCasting(_towerBeingCast);
                else if (_isCasting == false || _towerBeingCast == null)
                    UpdateMove(); // for the saboteur, is false by default.

                UpdateHeal(GetEnemies(_position, ActionRadius));
            }
            else if (_type == EnemyType.boss1)
            {
                if (!WithinReach(Position, _map.Wall.Position, _rangeBoss) && _isCastingBoss1 == false && _isVulnerable == false) UpdateMove();
                UpdateBoss1();
            } else if (_type == EnemyType.boss2 || _type == EnemyType.boss2_1)
            {
                if (!WithinReach(Position, _map.Wall.Position, _rangeBoss) && IsDead== false) UpdateMove();
                UpdateBossTwins();
            } else if (_type == EnemyType.boss3)
            {
                if (!WithinReach(Position, _map.Wall.Position, _rangeBoss) && IsDead == false) UpdateMove();
                UpdateBoss3();
            }
        }

        private void UpdateBoss3()
        {
            /*Faire une liste avec les tours à disposition
             * 
             * Choper une tourelle, la viser (le boss s'arrête) pendant genre 2 sec. 
             * Si les 2 sec sont passées sans que la corde soit touchée par un tir de sniper, il arrache la tour du sol donc 
             * objet turret est remove, le slot turret devient vide, comme lorsqu'on vend mais sans les thunes
             * 
             * Décrémenter l'enrage timer ou enrage()
             * 
             * 
             * 
             * */
           
        }
        private void UpdateBossTwins()
        {
            //Checks if any of them is dead
            // Revives if necessary ++ enraging
            //Checks if both dead to win
            UpdateAttackWallBoss();
            CheckIfRevive();          
        }

        private void CheckIfRevive()
        {
           // if (_BossesDead.Count == 2) ; YOU WIN
           if (_map.BossesDead.Count == 1)
            {
                if(_timeBetweenDeaths > 0)
                {
                    _timeBetweenDeaths--;
                } else if(_timeBetweenDeaths <= 0)
                {
                    _timeBetweenDeaths = 3 * 60;
                    _map.BossesDead.Clear(); //Clears the list where we store bosses that are dead
                    ReviveBosses();
                }
            }
        }

      
        

        private void ReviveBosses() 
        {
           
           foreach (Enemy enemy in _map.GetAllEnemies())
            {
                Die(); // Kills the remaining boss
            }
            _map.BossesDead.Clear(); // reclears the list to be sure.
            
            

            _map._timesBeingRevived++;

            _map.SpawnsEnemies[0].Waves[0].CreateEnemies(EnemyType.boss2, 1);
            _map.SpawnsEnemies[1].Waves[0].CreateEnemies(EnemyType.boss2_1, 1);
        }
            

       

       


        private void UpdateBoss1()
        {
            if (_timeBeforeCharging > 0) _timeBeforeCharging--;
            else if (_timeBeforeCharging == 0 && _hasCharged == false && _isCharging == false) ChargeBoss1();
            EncounterWallCreated();
            UpdateAttackWallBoss();
        }

        private void ChargeBoss1() // Stops moving for couple of secs, before truly charging
        {
            _isCastingBoss1 = true;

            if (_timeBeforeEndofCastingCharge == 0) ChargingBoss1();
            else if (_timeBeforeEndofCastingCharge > 0) _timeBeforeEndofCastingCharge--;
        }

        private void ChargingBoss1() // Is charging
        {
            _hasCharged = true;
            _isCharging = true;
            _isCastingBoss1 = false;
            _damage = _damage * 2;
            Speed = Speed * 5;
        }

        private void UpdateAttackWall()
        {
            if (WithinReach(Position, _map.Wall.Position, Speed))
            {
                _map.Wall.TakeHp(_damage);
                Die(true);
            }
        }

        private void EncounterWallCreated() // Boss1
        {
            // If boss is charging and encounters a wall created, the wall breaks.
            //Then _isCharging goes false.
            // Boss resumes normal speed and dmg after a few seconds of stun where he takes double dmg.
            // Keeps _hasCharged = true so he doesnt resume charging 
            if (_isVulnerable == false && _WallBoss._isBreached == false)
            {
                if (WithinReach(Position, _WallBoss.Position, _WallBoss.Radius))
                {
                    if (_isCharging == true)
                    {
                        Speed = Speed / 5;
                        _damage = _damage / 2;
                    }
                    _isCharging = false;
                    _WallBoss._isBreached = true;
                    _isVulnerable = true;
                    _hasCharged = true;
                }
            }
            else if (_isVulnerable == true)
            {
                if (_timeofVulnerability > 0)
                {
                    _timeofVulnerability--;
                }
                else if (_timeofVulnerability <= 0)
                {
                    _isVulnerable = false;

                }
            }
        }

        private void UpdateAttackWallBoss()
        {
            if (WithinReach(Position, _map.Wall.Position, _rangeBoss))
            {
                if (_reload != 0) _reload--;
                else
                {
                    _map.Wall.TakeHp(_damage);
                    _reload = _defaultReload;
                }
            }
        }

        private void UpdateHeal(List<Enemy> _enemiesToHeal)
        {
            if (_type == EnemyType.doctor)
            {
                if (!IsReload) Reloading();
                if (IsReload)
                {
                    foreach (Enemy enemy in _enemiesToHeal)
                    {

                        if (enemy.CurrentHp + _heal > enemy.MaxHp) enemy.CurrentHp = enemy.MaxHp;
                        else enemy.CurrentHp += _heal;
                        _reload = _healCooldown * 60;
                    }
                    _map.AnimHeal.Add(this);
                }
            }
            
        }

        public void UpdateSaboteur(List<Tower> _towersToDisable)
        {

            if (_type == EnemyType.saboteur)
            {
                if (_hasCast == false)
                {
                    foreach (Tower tower in _towersToDisable)
                    {
                        if (!tower.IsDisabled && !tower.IsCasted)
                        {
                            _towerBeingCast = tower;
                            _isCasting = true;
                            StartCasting(tower);
                            tower.IsCasted = true;
                            break;
                        }
                    }
                }
            }
            /*
            
             * La tour est disable pendant un certain temps puis revient à la normale.
             */
        }
        
        public void StartCasting(Tower tower)
        {
            List<Tower> towers = GetTowers(_position, ActionRadius);
            if (!towers.Contains(tower))
            {
                _isCasting = false;
                _towerBeingCast = null;
            }
            else if (_reload <= 0)
            {
                if (tower.Type == TowerType.bank)
                {
                    tower._reload = 15 * 60;
                }
                else
                {
                    tower._reload = Constant.DisabledTower * 60;
                }
                _hasCast = true; // this minion cannot disable turrets anymore 
                _isCasting = false; // Resumes moving
                tower.IsDisabled = true;
                tower.IsCasted = false;
                _map.TowerDisabled.Add(tower);
                _towerBeingCast = null;

            }  else {
                _reload--;
            }
            
        }

        
      

        internal bool IsReload => _reload <= 0;
        internal void Reloading() => _reload--;

         

        private List<Enemy> GetEnemies(Vector2 position, double radius)
        {
            List<Enemy> _enemiesToHeal = new List<Enemy>();
            List<Enemy> _currentEnemies = _map.GetAllEnemies();

            foreach (Enemy enemy in _currentEnemies)
            {
                if (WithinReach(position, enemy.Position, radius)) _enemiesToHeal.Add(enemy);
            }

            return _enemiesToHeal;
        }

        private List<Tower> GetTowers(Vector2 position, double radius)
        {
            List<Tower> _towersToDisable = new List<Tower>();
            

            foreach (Tower tower in _map.Towers)
            {
                if (WithinReach(position, tower.Position, radius)) _towersToDisable.Add(tower);
            }

            return _towersToDisable;
        }


        public Vector2 Position => _position;
        public bool IsStarting => TimerBeforeStarting <= 0;
        public bool IsDead => CurrentHp <= 0;
        public void TakeHp(double damage)
        {
            CurrentHp -= damage;
            if (_type == EnemyType.boss1 && _isVulnerable == true) CurrentHp -= damage * 2;
        }
        public List<Vector2> ShortestWay => _wave.ShortestWay;
        private bool WithinReach(Vector2 myPosition, Vector2 target, double speed)
        {
            double distanceEnemy = DistanceOf(myPosition, target);
            return distanceEnemy < speed * Constant.imgSizeMap;
        }

        private double DistanceOf(Vector2 myPosition, Vector2 target)
        {
            return (target.X - myPosition.X) * (target.X - myPosition.X) + (target.Y - myPosition.Y) * (target.Y - myPosition.Y);
        }

        public void Die()
        {
            if (_type == EnemyType.boss2 || _type == EnemyType.boss2_1) _map.BossesDead.Add(this); // For revive mechanism

            ManagerSound.PlayManDie();
            _map.Dollars += Bounty;
            _wave.Enemies.Remove(this);
            _map.DeadEnemies.Add(this);



           

            if (_type == EnemyType.saboteur && _towerBeingCast != null) _towerBeingCast.IsCasted = false;
            // IS DISABLED







        }

        public void Die(bool Passedthebase)
        // Overload. If the ennemny unit passes the base, it dies but does not gives gold.
        {
            ManagerSound.PlayManDie();
            if (Passedthebase == false)
            {
                _map.Dollars += Bounty;
            }
            _wave.Enemies.Remove(this);
            _map.DeadEnemies.Add(this);
        }
    }
}
