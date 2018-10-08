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
using LibraryTrumpTower.Constants.BalanceGame.Enemies;
using LibraryTrumpTower.SpecialAbilities;
using LibraryTrumpTower.Constants.BalanceGame.Bosses;
using LibraryTrumpTower.Constants.BalanceGame.Events;

namespace TrumpTower.LibraryTrumpTower
{
    [DataContract(IsReference = true)]
    public class Enemy
    {
        [DataMember]
        readonly Map _map;
        [DataMember]
        bool Initiliaze { get; set; } // for serialization
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
        public int TimerBeforeStarting { get; set; }
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

        public double _defaultReloadSpecial { get; set; }
        public double _reloadSpecial { get; set; }
        public double _defaultReload { get; private set; }
        public double CurrentHp { get; private set; }
        public double _reload { get; private set; }// doc & mech units}
        public double speed;
        public double Speed
        {
            get
            {
                if (BalanceEvent3.EVENT3_IS_BONUS)
                {
                    if (_type != EnemyType.boss1 && _type != EnemyType.boss2 && _type != EnemyType.boss2_1 && _type != EnemyType.boss3)
                        return speed - (BalanceEvent3.EVENT3_CURRENT_SPEED_IN_PERCENT * speed / 100);
                    else
                        return speed;
                }

                if (_type != EnemyType.boss1 && _type != EnemyType.boss2 && _type != EnemyType.boss2_1 && _type != EnemyType.boss3)
                    return speed + (BalanceEvent3.EVENT3_CURRENT_SPEED_IN_PERCENT * speed / 10);
                else
                    return speed;
            }
            set { speed = value; }
        }
        public double MaxHp
        {
            get
            {
                if (_type == EnemyType.defaultSoldier) return BalanceEnemyDefaultSoldier.ENEMY_DEFAULT_SOLDIER_MAX_HP;
                else if (_type == EnemyType.kamikaze) return BalanceEnemyKamikaze.ENEMY_KAMIKAZE_MAX_HP;
                else if (_type == EnemyType.doctor) return BalanceEnemyDoctor.ENEMY_DOCTOR_MAX_HP;
                else if (_type == EnemyType.saboteur) return BalanceEnemySaboteur.ENEMY_SABOTEUR_MAX_HP;
                else if (_type == EnemyType.boss2 || _type == EnemyType.boss2_1) return BalanceBoss2.BOSS2_MAX_HP;
                else if (_type == EnemyType.boss1) return BalanceBoss1.BOSS1_MAX_HP;
                else if (_type == EnemyType.boss3) return BalanceBoss3.BOSS3_MAX_HP;
                else return 0;
            }
        }
        public double _damage
        {
            get
            {
                if (_type == EnemyType.defaultSoldier) return BalanceEnemyDefaultSoldier.ENEMY_DEFAULT_SOLDIER_DAMAGE;
                else if (_type == EnemyType.kamikaze) return BalanceEnemyKamikaze.ENEMY_KAMIKAZE_DAMAGE;
                else if (_type == EnemyType.doctor) return BalanceEnemyDoctor.ENEMY_DOCTOR_DAMAGE;
                else if (_type == EnemyType.saboteur) return BalanceEnemySaboteur.ENEMY_SABOTEUR_DAMAGE;
                else if (_type == EnemyType.boss2 || _type == EnemyType.boss2_1) return BalanceBoss2.BOSS2_DAMAGE * (1 + Map._timesBeingRevived);
                else if (_type == EnemyType.boss1) return BalanceBoss1.BOSS1_DAMAGE;
                else if (_type == EnemyType.boss3) return BalanceBoss3.BOSS3_DAMAGE;
                else return 0;
            }
            set
            {
                _damage = value;
            }
        }
        public double DefaultSpeed
        {
            get
            {
                if (_type == EnemyType.defaultSoldier) return BalanceEnemyDefaultSoldier.ENEMY_DEFAULT_SOLDIER_DEFAULT_SPEED;
                else if (_type == EnemyType.kamikaze) return BalanceEnemyKamikaze.ENEMY_KAMIKAZE_DEFAULT_SPEED;
                else if (_type == EnemyType.doctor) return BalanceEnemyDoctor.ENEMY_DOCTOR_DEFAULT_SPEED;
                else if (_type == EnemyType.saboteur) return BalanceEnemySaboteur.ENEMY_SABOTEUR_DEFAULT_SPEED;
                else if (_type == EnemyType.boss2 || _type == EnemyType.boss2_1) return BalanceBoss2.BOSS2_DEFAULT_SPEED;
                else if (_type == EnemyType.boss1) return BalanceBoss1.BOSS1_DEFAULT_SPEED;
                else if (_type == EnemyType.boss3) return BalanceBoss3.BOSS3_DEFAULT_SPEED;
                else return 0;
            }
        }
        public int Bounty
        {
            get
            {
                if (_type == EnemyType.defaultSoldier) return BalanceEnemyDefaultSoldier.ENEMY_DEFAULT_SOLDIER_BOUNTY;
                else if (_type == EnemyType.kamikaze) return BalanceEnemyKamikaze.ENEMY_KAMIKAZE_BOUNTY;
                else if (_type == EnemyType.doctor) return BalanceEnemyDoctor.ENEMY_DOCTOR_BOUNTY;
                else if (_type == EnemyType.saboteur) return BalanceEnemySaboteur.ENEMY_SABOTEUR_BOUNTY;
                else return 0;
            }
        }
        public double ActionRadius // doc & mech units
        {
            get
            {
                if (_type == EnemyType.doctor) return BalanceEnemyDoctor.ENEMY_DOCTOR_ACTION_RADIUS;
                else if (_type == EnemyType.saboteur) return BalanceEnemySaboteur.ENEMY_SABOTEUR_ACTION_RADIUS;
                else if (_type == EnemyType.boss3) return BalanceBoss3.BOSS3_ACTION_RADIUS;
                else return 0;
            }
        }
        public double _heal // for the doc
        {
            get
            {
                if (_type == EnemyType.doctor) return BalanceEnemyDoctor.ENEMY_DOCTOR_HEAL;
                else return 0;
            }
        }
        public double _healCooldown // doc only
        {
            get
            {
                if (_type == EnemyType.doctor) return BalanceEnemyDoctor.ENEMY_DOCTOR_HEAL_COOLDOWN;
                else return 0;
            }
        }

        public Boss3State StateBoss3 { get; set; }
        public double TimeBeforeLaunch { get; set; }
        public Tower TargetTower { get; set; }
        public ChainBoss CurrentChain { get; set; }

        public Enemy(Map map, Wave wave, string name, EnemyType type, WallBoss Wallboss)
        {
            Initiliaze = false;
            _type = type;
            _map = map;
            _wave = wave;
            _name = name;
            _position = wave.Position;
            _moveToState = 0;
            _isCasting = false;
            _WallBoss = null;
            if (type == EnemyType.saboteur)
            {
                _hasCast = false;
                _towerBeingCast = null;
            }
            else if (type == EnemyType.boss1) // Che Guevarra
            {
                _WallBoss = Wallboss;
            }
            /*else if (type == EnemyType.boss3) // Kim Jung Un
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
               * *
          }*/
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
            // Deserialization
            if (!Initiliaze)
            {
                StateBoss3 = Boss3State.NONE;
                CurrentHp = MaxHp;
                if (_type == EnemyType.boss2 || _type == EnemyType.boss2_1) Speed = DefaultSpeed * (1 + Map._timesBeingRevived);
                else { Speed = DefaultSpeed; }

                if (_type == EnemyType.doctor)
                {
                    _reload = 0;
                }
                else if (_type == EnemyType.saboteur)
                {
                    _reload = BalanceEnemySaboteur.ENEMY_SABOTEUR_RELOADING;
                }
                else if (_type == EnemyType.boss2 || _type == EnemyType.boss2_1)
                {
                    ManagerSound.PlayUrssRock();
                    _reload = BalanceBoss2.BOSS2_DEFAULT_RELOAD;
                    _defaultReload = BalanceBoss2.BOSS2_DEFAULT_RELOAD;
                    _rangeBoss = BalanceBoss2.BOSS2_RANGE;
                }
                else if (_type == EnemyType.boss1)
                {
                    _reload = 2 * 60; // attacks every two seconds
                    _defaultReload = 2 * 60;
                    _isCharging = false;
                    _hasCharged = false;
                    _timeBeforeCharging = BalanceBoss1.BOSS1_TIME_BEFORE_CHARGING; // When it comes to 0, boss1 starts casting charge 
                    _isVulnerable = false;
                    _timeofVulnerability = BalanceBoss1.BOSS1_TIME_OF_VULNERABILITY; // Time where boss doesnt move and take *2 dmg before resuming actions.
                    _isCastingBoss1 = false;
                    _timeBeforeEndofCastingCharge = BalanceBoss1.BOSS1_TIME_BEFORE_END_OF_CASTING_CHARGE; // When it comes to 0, boss1 charges, doubling his speed and dammage, build a wall to stop him
                    _rangeBoss = 200;
                }
                else if (_type == EnemyType.boss3)
                {
                    _reload = 0;
                    _defaultReload = BalanceBoss3.BOSS3_DEFAULT_RELOAD;
                    _reloadSpecial = 0;
                    _defaultReloadSpecial = BalanceBoss3.BOSS3_DEFAULT_RELOAD_SPECIAL;
                    StateBoss3 = Boss3State.WALK;
                    _rangeBoss = 200;
                    TargetTower = null;
                    CurrentChain = null;
                    ManagerSound.PlayAnnouncementKim();
                }
                else if (_type == EnemyType.boss1)
                {
                    ManagerSound.PlayChe();
                }

                Initiliaze = true;
            }

            if (!IsStarting)
            {
                TimerBeforeStarting--;
            }
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
            }
            else if (_type == EnemyType.boss2 || _type == EnemyType.boss2_1)
            {
                if (!WithinReach(Position, _map.Wall.Position, _rangeBoss) && IsDead == false) UpdateMove();
                UpdateBossTwins();
            }
            else if (_type == EnemyType.boss3)
            {
                if (!WithinReach(Position, _map.Wall.Position, _rangeBoss) && IsDead == false && StateBoss3 == Boss3State.WALK) UpdateMove();
                UpdateBoss3();
                UpdateAttackWallBoss();
            }
        }

        private void UpdateBoss3()
        {
            if (_reloadSpecial <= 0 && StateBoss3 == Boss3State.WALK)
            {
                foreach (Tower tower in _map.Towers)
                {
                    bool alreadyCaptured = false;

                    if (WithinReach(Position, tower.Position, ActionRadius) && !alreadyCaptured)
                    {
                        TimeBeforeLaunch = BalanceBoss3.BOSS3_TIME_BEFORE_LAUNCH;
                        TargetTower = tower;
                        StateBoss3 = Boss3State.CAST;
                        ManagerSound.PlayGragasLaught();
                        break;
                    }
                }
            }

            else if (StateBoss3 == Boss3State.CAST)
            {
                if (CurrentChain != null)
                {
                    if (CurrentChain.IsDead())
                    {
                        CurrentChain = null;
                        StateBoss3 = Boss3State.WALK;
                    }
                }

                TimeBeforeLaunch--;
                if (TimeBeforeLaunch <= 0)
                {
                    CurrentChain = new ChainBoss(_map, BalanceBoss3.BOSS3_CHAIN_MAX_HP, _position, TargetTower, _map.Wall, BalanceBoss3.BOSS3_CHAIN_DAMAGE, this);
                    _reloadSpecial = BalanceBoss3.BOSS3_DEFAULT_RELOAD_SPECIAL;
                    StateBoss3 = Boss3State.LAUNCHITSCHAIN;
                    ManagerSound.PlayLaunchingChain();
                }
            }

            else if (StateBoss3 == Boss3State.LAUNCHITSCHAIN)
            {
                if (CurrentChain.IsDead())
                {
                    CurrentChain = null;
                    StateBoss3 = Boss3State.WALK;
                }

                if (CurrentChain.IsArrived())
                {
                    TimeBeforeLaunch = BalanceBoss3.BOSS3_TIME_BEFORE_LAUNCH;
                    StateBoss3 = Boss3State.PULL;
                    ManagerSound.PlayStalledChain();
                }
            }

            else if (StateBoss3 == Boss3State.PULL)
            {
                if (CurrentChain.IsDead())
                {
                    CurrentChain = null;
                    StateBoss3 = Boss3State.WALK;
                }

                TimeBeforeLaunch--;
                if (TimeBeforeLaunch <= 0)
                {
                    StateBoss3 = Boss3State.TURNTOWER;
                    _map.Towers.Remove(TargetTower);
                    CurrentChain.CurrentState = ChainBoss.ChainBossState.TURN;
                    _map.ChangeLocation((int)TargetTower.Position.X / Constant.imgSizeMap, (int)TargetTower.Position.Y / Constant.imgSizeMap, (int)MapTexture.grass);
                    ManagerSound.PlayGangnamStyle();
                }
            }

            else if (StateBoss3 == Boss3State.TURNTOWER)
            {
                if (CurrentChain.IsDead())
                {
                    CurrentChain = null;
                    StateBoss3 = Boss3State.WALK;
                }

                if (CurrentChain.Position4 == 4)
                {
                    StateBoss3 = Boss3State.THROWTOWER;
                    CurrentChain.CurrentState = ChainBoss.ChainBossState.STALLED;
                }
            }
            else if (StateBoss3 == Boss3State.THROWTOWER)
            {
                if (CurrentChain.IsDead())
                {
                    CurrentChain = null;
                    StateBoss3 = Boss3State.WALK;
                }

                if (CurrentChain.CurrentState == ChainBoss.ChainBossState.NONE)
                {
                    _map.DeadUnitsAir.Add(CurrentChain._position);
                    CurrentChain = null;
                    StateBoss3 = Boss3State.WALK;
                    ManagerSound.PlayExplosionC4();
                }
            }
            _reloadSpecial--;
        }

        #region Boss2
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
            if (_map.BossesDead.Count == 2) {  }
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

            // BalanceBoss2.BOSS2_TIMES_BEING_REVIVED++;
            Map._timesBeingRevived++;
            ManagerSound.PlayUrssRock();
            _map.SpawnsEnemies[0].Waves[4].CreateEnemies(EnemyType.boss2, 1);
            _map.SpawnsEnemies[1].Waves[4].CreateEnemies(EnemyType.boss2_1, 1);
        }
        #endregion

        #region Boss1
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
            if(_timeBeforeEndofCastingCharge == 60)
            ManagerSound.PlayChe();
            if (_timeBeforeEndofCastingCharge == 0) ChargingBoss1();
            else if (_timeBeforeEndofCastingCharge > 0) _timeBeforeEndofCastingCharge--;
        }

        private void ChargingBoss1() // Is charging
        {
            
            _hasCharged = true;
            _isCharging = true;
            _isCastingBoss1 = false;
            BalanceBoss1.BOSS1_DAMAGE *= 2;
            Speed = Speed * 5;
        }
        #endregion

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
            if (_isCharging == true && _WallBoss._isBreached == false)
            {
                if (WithinReach(Position, _WallBoss.Position, _WallBoss.Radius))
                {
                    
                        Speed = Speed / 5;
                        BalanceBoss1.BOSS1_DAMAGE /= 2;
                    
                    _isCharging = false;
                    _WallBoss._isBreached = true;
                    ManagerSound.PlayWallBreak();
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
                if (_reload > 0) _reload--;
                else
                {
                    ManagerSound.PlayPunch();
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
                    tower._reload = BalanceEnemySaboteur.ENEMY_SABOTEUR_RELOADING;
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
