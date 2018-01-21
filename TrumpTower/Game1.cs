using LibraryTrumpTower;
using LibraryTrumpTower.AirUnits;
using LibraryTrumpTower.Constants;
using LibraryTrumpTower.Constants.BalanceGame.Bosses;
using LibraryTrumpTower.Constants.BalanceGame.Enemies;
using LibraryTrumpTower.Constants.BalanceGame.Towers;
using LibraryTrumpTower.Decors;
using Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TrumpTower.Draw;
using TrumpTower.Draw.Animations;
using TrumpTower.Draw.ButtonsUI;
using TrumpTower.Draw.ButtonsUI.SpecialAbilities;
using TrumpTower.Draw.DollarsAnimations;
using TrumpTower.Drawing;
using TrumpTower.LibraryTrumpTower;
using TrumpTower.LibraryTrumpTower.Constants;
using TrumpTower.LibraryTrumpTower.Spawns;

namespace TrumpTower
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        #region Fields
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int nombre;
        int nombre2;
        float lowLife;
        bool lowLifeBool;
        bool lowLifeBool2;
        Texture2D[] LowLifeIndicator = new Texture2D[4];
        
        int playLowLife;
        


        #region Win condition
        bool _isWon;
        Texture2D _trumpWin;
        #endregion

        #region Lose condition
        bool isLost;
        int policeBlink;
        int policeBlink2;
        SpriteFont _gameOver;
        Texture2D _sadTrump;
        Texture2D _gameOverExplosion;
        #endregion

        #region Maps

        Map _map;
        List<Texture2D> _imgMaps;
        Dictionary<string, List<Texture2D>> _imgThemesMaps = new Dictionary<string, List<Texture2D>>();
        Dictionary<string, List<Texture2D>> _imgThemesDecorsMaps = new Dictionary<string, List<Texture2D>>();
        public float VirtualWidth { get; set; }
        public float VirtualHeight { get; set; }
        Matrix scale;

        #endregion

        #region Decors
        private List<Texture2D> _imgDecors;
        #endregion

        #region Towers
        int _towerCompteur;
        Tower _hoveredTower;
        SpriteFont _upgradeFont;
        Tower _myTow;
        Texture2D _imgTowerDamages;
        Texture2D _imgTowerAttackSpeed;
        Texture2D _imgLvl;
        Texture2D rect;
        Texture2D circleTower;
        Tower _lastHoveredTower;
        #region Image Tower

        Texture2D _imgTower1;
        Texture2D _imgTower2;
        Texture2D _imgTower3;
        Texture2D _imgTower1_2;
        Texture2D _imgTower2_2;
        Texture2D _imgTower3_2;
        Texture2D _imgTower1_3;
        Texture2D _imgTower2_3;
        Texture2D _imgTower3_3;
        Texture2D _imgWrong;
        Texture2D _imgTower4;
        Texture2D _imgTower4_empty;


        #endregion

        #region Towers Selection

        Texture2D _imgSelector;
        Vector2 _towerSelector;
        Texture2D _imgCoin;
        Vector2 _towerSelectorUpgrade;
        Texture2D _imgUpgrade;
        Texture2D _imgSell;
        //Tower _drawTower;
        bool _verif;
        bool _verif2;
        bool _verif3;
        bool _verif4;
        

        #endregion

        #endregion

        #region Missiles

        Texture2D _imgMissile;
        Texture2D _imgMissile1;

        #endregion

        #region Enemies

        #region Earthly Enemies
        Texture2D _imgEnemy1;
        Texture2D _imgKamikaze;
        #endregion
        Texture2D _imgDoctor;
        Texture2D _imgSaboteur;
        Texture2D _imgSaboteur1;
        

        #region Air Units
        Texture2D _imgPlane1;
        Texture2D _imgPlane2;
        Texture2D _imgPlaneTurbo;

        #endregion

        #endregion

        #region Wall

        Texture2D _imgWall;

        #endregion

        #region Waves

        SpriteFont _imgNextWave;
        WaveIsComingImg _waveSprite;
        Texture2D _flagNorthKorea;

        #endregion

        #region Dollars

        Texture2D _imgDollars;
        SpriteFont _spriteDollars;
        Texture2D _backgroundDollars;
        DollarsAnimationsDefinition AnimationsDollars;

        #endregion

        #region Buttons

        #region Timer Buttons

        GroupOfButtonsUITimer _groupOfButtonsUITimer;
        Texture2D _pauseButton;
        Texture2D _normalButton;
        Texture2D _fastButton;

        #endregion

        #region Abilities Buttons


        GroupOfButtonsUIAbilities _groupOfButtonsUIAbilities;
        SpriteFont _cooldownSprite;
        public Texture2D ImgExplosionButton { get; private set; }
        Texture2D _circleExplosion;

        Texture2D _buttonSniper;
        Texture2D _ammoSniper;
        Texture2D _cursorTarget;

        Texture2D _buttonMaki;
        Texture2D _circleStickyRice;

        public Texture2D _wallBoss;
        Texture2D _imgBoss1Wall;
        #endregion

        #endregion

        Texture2D _imgHearth;

        #region Cursor

        public Texture2D ImgCursor { get; set; }
        Texture2D _imgCursorBomb;
        Texture2D _imgCursorDefault;
        Texture2D _imgCursorDeliveryRice;
        Texture2D _imgCursorTrowel;

        #endregion

        #region Entity
        Texture2D _imgHappyFace;
        Texture2D _imgAngryFace;
        Texture2D _imgVeryAngryFace;
        Texture2D _imgButtonDollars;
        #endregion
        Texture2D grey;
        
        public bool realPause { get; private set; }

        #region In game menu

        public bool isNewGame;
        // Global variables
        enum BState
        {
            HOVER,
            UP,
            JUST_RELEASED,
            DOWN
        }

        const int 
                BUTTON_HEIGHT = 58,
                BUTTON_WIDTH = 250;

        Color background_color;

        Color[] button_color_pause = new Color[3];
        Rectangle[] button_rectangle_pause = new Rectangle[3];
        Texture2D[] button_texture_pause = new Texture2D[3];

        Color[] button_color_lost = new Color[3];
        Rectangle[] button_rectangle_lost = new Rectangle[3];
        Texture2D[] button_texture_lost = new Texture2D[3];

        Color[] button_color_win = new Color[2];
        Rectangle[] button_rectangle_win = new Rectangle[2];
        Texture2D[] button_texture_win = new Texture2D[2];

        //mouse pressed and mouse just pressed
        bool mpressed, prev_mpressed = false;
        //mouse location in window
        int mx, my;
        double frame_time;


        #endregion

        // RAID AIR
        Texture2D _imgWarning;
        SpriteFont _raidAirIsComming;
        int _timerRaidAirClose;
        float _shadowRaidAirClose;
        float _shadowVar;

        public int stratPause { get; set; }


        MouseState newStateMouse;
        MouseState lastStateMouse;
        KeyboardState lastStateKeyboard;
        public bool GameIsPaused { get; set; }
        public SimpleAnimationDefinition[] AnimSprites { get; private set; }

        public bool warning;
        Texture2D _imgWarning2;

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            // float lowLifeFloat = Convert.ToSingle(lowLife);
            playLowLife = 0;
            lowLifeBool = true;
            lowLifeBool2 = false;
            lowLife = 0.1f;
            isLost = false;
            #region Map INIT
            _map = BinarySerializer.Deserialize<Map>(BinarySerializer.pathCurrentMapXml);

            foreach (Spawn spawn in _map.SpawnsEnemies)
                Map.WavesTotals += spawn.Waves.Count;

            /*BOSS2
            Map.SpawnsEnemies[0].Waves[0].CreateEnemies(EnemyType.boss2, 1);
            Map.SpawnsEnemies[1].Waves[0].CreateEnemies(EnemyType.boss2_1, 1);
            */

            //BOSS1
            //Map.SpawnsEnemies[0].Waves[0].CreateEnemies(EnemyType.boss1, 1);
            
            #endregion

            #region Graphics Device 

            Window.Position = new Point(800, 600);

            Window.Title = "Trump Tower";
            graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;

            {
                int attempts = 0;
                while( true )
                {
                    try
                    {
                        graphics.IsFullScreen = true;
                        graphics.ApplyChanges();
                        break;
                    }
                    catch( SharpDX.SharpDXException ex )
                    {
                        if( ex.HResult != -2005270494 || attempts > 150 ) throw;
                        attempts++;
                    }
                }
            }

            // Résolution d'écran
            VirtualWidth = _map.WidthArrayMap * Constant.imgSizeMap;
            VirtualHeight = _map.HeightArrayMap * Constant.imgSizeMap;
            scale = Matrix.CreateScale(
                            GraphicsDevice.Viewport.Width / VirtualWidth,
                            GraphicsDevice.Viewport.Height / VirtualHeight,
                            1f);
            graphics.ApplyChanges();

            #endregion
            nombre = 0;
            nombre2 = 0;
            _waveSprite = new WaveIsComingImg(_map, Map.WaveIsComming);
            _groupOfButtonsUITimer = new GroupOfButtonsUITimer(this);
            _verif3 = false;
            _verif4 = false;
            _lastHoveredTower = null;
            //_drawTower = null;
            
            // Animations
            AnimSprites = new SimpleAnimationDefinition[5];
            AnimSprites[0] = new SimpleAnimationDefinition(this, this, "animExplosion", new Point(100, 100), new Point(9, 9), 150, false);
            AnimSprites[1] = new SimpleAnimationDefinition(this, this, "Enemies/animBlood", new Point(64, 64), new Point(6, 1), 20, false);
            AnimSprites[2] = new SimpleAnimationDefinition(this, this, "Enemies/air/animPlaneExplosion", new Point(128, 128), new Point(4, 4), 20, false);
            AnimSprites[3] = new SimpleAnimationDefinition(this, this, "Enemies/animThunderSaboteur", new Point(350, 105), new Point(5, 2), 12, true);
            AnimSprites[4] = new SimpleAnimationDefinition(this, this, "Enemies/heal", new Point(192, 192), new Point(5, 5), 100, false);

            foreach (SimpleAnimationDefinition anim in this.AnimSprites) anim.Initialize();

            GameIsPaused = false;
            isLost = false;
            policeBlink = 240;
            policeBlink2 = 240;

            #region Tower Selector
            _towerCompteur = 60;
            _towerSelector = new Vector2(-1000, -1000);
            _towerSelectorUpgrade = new Vector2(-1000, -1000);
            _verif = false;

            #endregion

            #region In game menu

            // starting x and y locations to stack buttons 
            // vertically in the middle of the screen


            int x = ((int)VirtualWidth / 2) - BUTTON_WIDTH / 2;
            int y = (int)VirtualHeight / 2 - 4 / 2 * BUTTON_HEIGHT - (4 % 2);
            for (int i = 0; i < 3; i++)
            {
                
                button_color_pause[i] = Color.White;
                
                button_rectangle_pause[i] = new Rectangle(x, y, BUTTON_WIDTH, BUTTON_HEIGHT);
                y += 100;
            }

            int x2 = ((int)VirtualWidth / 2) - BUTTON_WIDTH / 2;
            int y2 = (int)VirtualHeight / 2 - 4 / 2 * BUTTON_HEIGHT - (4 % 2);
            for (int i = 0; i < 2; i++)
            {

                button_color_lost[i] = Color.White;

                button_rectangle_lost[i] = new Rectangle(x2, y2, BUTTON_WIDTH, BUTTON_HEIGHT);
                y2 += 200;
            }

            int x3 = ((int)VirtualWidth / 2) - BUTTON_WIDTH / 2;
            int y3 = (int)VirtualHeight / 2 - 4 / 2 * BUTTON_HEIGHT - (4 % 2);
            for (int i = 0; i < 2; i++)
            {

                button_color_win[i] = Color.White;

                button_rectangle_win[i] = new Rectangle(x3, y3, BUTTON_WIDTH, BUTTON_HEIGHT);
                y3 += 200;
            }


            IsMouseVisible = false;
            background_color = Color.CornflowerBlue;


            #endregion

            warning = false;
            base.Initialize();

            int WIDTH = (int)VirtualWidth / 12;
            int HEIGHT = (int)VirtualHeight / 13;
            rect = new Texture2D(graphics.GraphicsDevice, WIDTH, HEIGHT);

            Color[] data = new Color[WIDTH * HEIGHT];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            rect.SetData(data);
        }





        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            #region In game menu
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            button_texture_pause[0] =
                 Content.Load<Texture2D>("resume");
            button_texture_pause[1] =
                Content.Load<Texture2D>("restart");
            button_texture_pause[2] =
                Content.Load<Texture2D>("quit");
            button_texture_lost[0] =
                Content.Load<Texture2D>("home");
            button_texture_lost[1] =
                Content.Load<Texture2D>("retry");
            button_texture_win[0] =
                Content.Load<Texture2D>("home");
            button_texture_win[1] =
                Content.Load<Texture2D>("next_level");


            #endregion
            for (int i = 0; i < 4; i++)
            {
                LowLifeIndicator[i] = Content.Load<Texture2D>("low_life"+i);
            }
            
            #region Maps

            _imgThemesMaps = new Dictionary<string, List<Texture2D>>();
            _imgThemesMaps["World_Jungle"] = new List<Texture2D>();
            _imgThemesMaps["World_Snow"] = new List<Texture2D>();
            _imgThemesMaps["World_City"] = new List<Texture2D>();

            _imgThemesDecorsMaps = new Dictionary<string, List<Texture2D>>();
            _imgThemesDecorsMaps["World_Jungle"] = new List<Texture2D>();
            _imgThemesDecorsMaps["World_Snow"] = new List<Texture2D>();
            _imgThemesDecorsMaps["World_City"] = new List<Texture2D>();

            foreach (string nameWorld in _imgThemesMaps.Keys)
            {
                foreach (string name in Enum.GetNames(typeof(MapTexture)))
                {
                    if (name == "dirt" || name == "grass" || name == "emptyTower" || name == "notEmptyTower" || name == "myBase")
                        _imgThemesMaps[nameWorld].Add(Content.Load<Texture2D>("Map/" + nameWorld + "/" + name));
                    else if (name != "None") _imgThemesMaps[nameWorld].Add(null);
                }
                for (int y = 1; y < 9; y++)
                {
                    _imgThemesDecorsMaps[nameWorld].Add(Content.Load<Texture2D>("Map/" + nameWorld + "/decor/decor" + y));
                }
            }

            if (_map != null)
            {
                try
                {
                    string nameMap = Enum.GetName(typeof(ThemeMap), _map.ThemeOfMap);
                    _imgMaps = _imgThemesMaps[nameMap];
                    _imgDecors = _imgThemesDecorsMaps[nameMap];
                }
                // if map is old
                catch
                {
                    _map.ThemeOfMap = ThemeMap.World_Jungle;
                    _imgMaps = _imgThemesMaps[nameof(ThemeMap.World_Jungle)];
                    _imgDecors = _imgThemesDecorsMaps[nameof(ThemeMap.World_Jungle)];
                }
            }
            #endregion


            #region Wall

            _imgWall = Content.Load<Texture2D>("wall");

            #endregion

            #region Enemies

            #region Earthly Enemies
            _imgEnemy1 = Content.Load<Texture2D>("Enemies/enemy1");
            _imgKamikaze = Content.Load<Texture2D>("Enemies/kamikaze");
            #endregion
            _imgDoctor = Content.Load<Texture2D>("Enemies/doctor");
            _imgSaboteur = Content.Load<Texture2D>("Enemies/saboteur");
            _imgSaboteur1 = Content.Load<Texture2D>("Enemies/saboteur1");
            

            #region Air Enemies
            _imgPlane1 = Content.Load<Texture2D>("Enemies/Air/plane1");
            _imgPlane2 = Content.Load<Texture2D>("Enemies/Air/plane2");
            _imgPlaneTurbo = Content.Load<Texture2D>("Enemies/Air/planeTurbo");
            #endregion

            #endregion

            #region Towers

            #region Image Towers

            _imgTower1 = Content.Load<Texture2D>("Towers/tower1");
            _imgTower2 = Content.Load<Texture2D>("Towers/tower2");
            _imgTower3 = Content.Load<Texture2D>("Towers/tower3");
            _imgTower1_2 = Content.Load<Texture2D>("Towers/tower1_2");
            _imgTower2_2 = Content.Load<Texture2D>("Towers/tower2_2");
            _imgTower3_2 = Content.Load<Texture2D>("Towers/tower3_2");
            _imgTower1_3 = Content.Load<Texture2D>("Towers/tower1_3");
            _imgTower2_3 = Content.Load<Texture2D>("Towers/tower2_3");
            _imgTower3_3 = Content.Load<Texture2D>("Towers/tower3_3");
            _imgTower4 = Content.Load<Texture2D>("Towers/bank");
            _imgTower4_empty = Content.Load<Texture2D>("Towers/bank_empty");

            #endregion
            _upgradeFont = Content.Load<SpriteFont>("Towers/dollars");
            _imgTowerDamages = Content.Load<Texture2D>("Towers/damages");
            _imgTowerAttackSpeed = Content.Load<Texture2D>("Towers/attackSpeed");
            _imgLvl = Content.Load<Texture2D>("Towers/lvl");
            #region Towers Selection

            _imgSelector = Content.Load<Texture2D>("selector");
            _imgCoin = Content.Load<Texture2D>("Towers/coin");
            _imgUpgrade = Content.Load<Texture2D>("Towers/upgrade");
            _imgSell = Content.Load<Texture2D>("Towers/sell");
            _imgWrong = Content.Load<Texture2D>("Towers/wrong");

            #endregion

            #endregion

            #region Missiles

            _imgMissile = Content.Load<Texture2D>("Missiles/missile2");
            _imgMissile1 = Content.Load<Texture2D>("Missiles/missile1");

            #endregion

            #region Dollars

            _imgDollars = Content.Load<Texture2D>("Dollars/dollarsImg");
            _spriteDollars = Content.Load<SpriteFont>("Dollars/dollars");
            _backgroundDollars = Content.Load<Texture2D>("Dollars/backgroundDollars");
            AnimationsDollars = new DollarsAnimationsDefinition(new Vector2(50, 17), _spriteDollars, spriteBatch, (int)_map.Dollars);
            _gameOver = Content.Load<SpriteFont>("gameOver");

            #endregion

            #region Waves

            _imgNextWave = Content.Load<SpriteFont>("NextWave/next_wave");
            WaveIsComingImg.LoadContent(Content);
            _flagNorthKorea = Content.Load<Texture2D>("NextWave/flagNorthKorea");

            #endregion

            #region Buttons

            #region Timer Buttons

            _groupOfButtonsUITimer = new GroupOfButtonsUITimer(this);

            #region Fast Button

            _fastButton = Content.Load<Texture2D>("ManagerTime/fastButton");
            Vector2 _positionFastButton = new Vector2(VirtualWidth - 50, 10);
            _groupOfButtonsUITimer.CreateButtonUI(new ButtonUITimer(_groupOfButtonsUITimer, "fastTimer", _positionFastButton, _fastButton));

            #endregion

            #region Normal Button

            _normalButton = Content.Load<Texture2D>("ManagerTime/normalButton");
            Vector2 _positionNormalButton = new Vector2(_positionFastButton.X - 50, 10);
            _groupOfButtonsUITimer.CreateButtonUI(new ButtonUITimer(_groupOfButtonsUITimer, "normalTimer", _positionNormalButton, _normalButton));
            _groupOfButtonsUITimer.ButtonActivated = _groupOfButtonsUITimer.ButtonsUIArray["normalTimer"];

            #endregion

            #region Pause Button

            _pauseButton = Content.Load<Texture2D>("ManagerTime/pauseButton");
            Vector2 _positionPauseButton = new Vector2(_positionNormalButton.X - 50, 10);
            _groupOfButtonsUITimer.CreateButtonUI(new ButtonUITimer(_groupOfButtonsUITimer, "pauseTimer", _positionPauseButton, _pauseButton));

            #endregion

            #endregion

            #region Abilities Buttons

            #region Explosion Button

            _cooldownSprite = Content.Load<SpriteFont>("cooldownText");
            _groupOfButtonsUIAbilities = new GroupOfButtonsUIAbilities(this, _map.Explosion, _cooldownSprite);
            ImgExplosionButton = Content.Load<Texture2D>("SpecialAbilities/explosion");
            Vector2 _positionExplosionAbilityButton = new Vector2(15, _map.MapArray.GetLength(0) * Constant.imgSizeMap - 80);
            _groupOfButtonsUIAbilities.CreateButtonUI(new ButtonUIAbility(_groupOfButtonsUIAbilities, "explosionAbility", _positionExplosionAbilityButton, ImgExplosionButton));
            _circleExplosion = createCircleText((int)_map.Explosion.Radius);

            #endregion

            #region Sniper Button
            _buttonSniper = Content.Load<Texture2D>("SpecialAbilities/Sniper/buttonSniper");
            _ammoSniper = Content.Load<Texture2D>("SpecialAbilities/Sniper/ammo");
            _cursorTarget = Content.Load<Texture2D>("SpecialAbilities/Sniper/cursorTarget");
            Vector2 _positionSniperAbilityButton = new Vector2(_positionExplosionAbilityButton.X + 80, _positionExplosionAbilityButton.Y);
            _groupOfButtonsUIAbilities.CreateButtonUI(new ButtonUIAbility(_groupOfButtonsUIAbilities, "sniperAbility", new Vector2(_positionSniperAbilityButton.X, _positionSniperAbilityButton.Y), _buttonSniper));
            #endregion

            #region Stciky Rice Button
            _buttonMaki = Content.Load<Texture2D>("SpecialAbilities/stickyMaki");
            Vector2 _positionMakiAbilityButton = new Vector2(_positionSniperAbilityButton.X + 80, _positionSniperAbilityButton.Y);
            _groupOfButtonsUIAbilities.CreateButtonUI(new ButtonUIAbility(_groupOfButtonsUIAbilities, "stickyRiceAbility", new Vector2(_positionMakiAbilityButton.X, _positionMakiAbilityButton.Y), _buttonMaki));
            _circleStickyRice = createCircleText((int)_map.StickyRice.Radius);
            #endregion

            #region WallBoss
            _wallBoss = Content.Load<Texture2D>("SpecialAbilities/wallboss");
            _groupOfButtonsUIAbilities.CreateButtonUI(new ButtonUIAbility(_groupOfButtonsUIAbilities, "wallBossAbility", new Vector2(_positionExplosionAbilityButton.X + 240, _positionExplosionAbilityButton.Y), _wallBoss));
            Vector2 _positionWallBossAbilityButton = new Vector2(_positionMakiAbilityButton.X + 80, _positionMakiAbilityButton.Y);
            _imgBoss1Wall = Content.Load<Texture2D>("SpecialAbilities/wall");
            #endregion


            #endregion

            #endregion

            #region Cursor

            _imgCursorBomb = Content.Load<Texture2D>("cursorBomb");
            _imgCursorDefault = Content.Load<Texture2D>("cursor");
            _imgCursorDeliveryRice = Content.Load<Texture2D>("deliveryRice");
            _imgCursorTrowel = Content.Load<Texture2D>("trowel");

            #endregion

            #region Entity
            _imgHappyFace = Content.Load<Texture2D>("Entity/happy");
            _imgAngryFace = Content.Load<Texture2D>("Entity/angry");
            _imgVeryAngryFace = Content.Load<Texture2D>("Entity/mad");
            _imgButtonDollars = Content.Load<Texture2D>("Entity/money");

            #region Entity Button
            Vector2 _positionEntityButton = new Vector2(VirtualWidth - 15, _positionSniperAbilityButton.Y);
            _groupOfButtonsUIAbilities.CreateButtonUI(new ButtonUIAbility(_groupOfButtonsUIAbilities, "entityAbility", new Vector2(_positionEntityButton.X - _imgButtonDollars.Width, _positionEntityButton.Y), _imgButtonDollars));
            #endregion

            #endregion

            #region Sound

            ManagerSound.LoadContent(Content);
            // MUSIQUE 
            MediaPlayer.Play(ManagerSound.Song1);
            MediaPlayer.Volume = 0.05f;
            MediaPlayer.IsRepeating = true;

            #endregion

            _imgHearth = Content.Load<Texture2D>("hearth");

            grey = Content.Load<Texture2D>("grey");

            _trumpWin = Content.Load<Texture2D>("Trum_win");

            _imgWarning2 = Content.Load<Texture2D>("Warning2");
            // RAID AIR
            _imgWarning = Content.Load<Texture2D>("warning");
            _raidAirIsComming = Content.Load<SpriteFont>("Enemies/air/RaidAirIsComming");
            _timerRaidAirClose = 0;
            _shadowRaidAirClose = 0.5f;
            _shadowVar = 0.1f;

            _sadTrump = Content.Load<Texture2D>("trump_sad");
            _gameOverExplosion = Content.Load<Texture2D>("game_over_screen");

            // ANIMATION EXPLOSION ABILITY
            foreach (SimpleAnimationDefinition anim in this.AnimSprites) anim.LoadContent(spriteBatch);
            // HEALTH BAR ON ENEMIES AND WALL
            HealthBar.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }




        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            
           
            frame_time = gameTime.ElapsedGameTime.Milliseconds / 1000.0;
            // TODO: Add your update logic here
            if (!GameIsPaused)
            {
                _map.Update();
                _waveSprite.Update(Map.WaveIsComming);
            }

            #region Prepare and Execut HandleInput

            newStateMouse = Mouse.GetState();
            newStateMouse = new MouseState((int)(newStateMouse.X * (VirtualWidth / GraphicsDevice.Viewport.Width)),
                                            (int)(newStateMouse.Y * (VirtualHeight / GraphicsDevice.Viewport.Height)),
                                            newStateMouse.ScrollWheelValue,
                                            newStateMouse.LeftButton,
                                            newStateMouse.MiddleButton,
                                            newStateMouse.RightButton,
                                            newStateMouse.XButton1,
                                            newStateMouse.XButton2);

            KeyboardState newStateKeyboard = Keyboard.GetState();
            HandleInput(newStateMouse, lastStateMouse, newStateKeyboard, lastStateKeyboard);
            lastStateMouse = newStateMouse;
            lastStateKeyboard = newStateKeyboard;

            #endregion
            // update mouse variables
            MouseState mouse_state = Mouse.GetState();
            mx = newStateMouse.X;
            my = newStateMouse.Y;
            prev_mpressed = mpressed;
            mpressed = mouse_state.LeftButton == ButtonState.Pressed;

            #region Events
            if (_map.Events.IsActivateFirstTime)
            {
                GameIsPaused = true;
                _map.Events.IsActivateFirstTime = false;
                ManagerSound.PlayPauseIn();
                _groupOfButtonsUITimer.ButtonActivated = _groupOfButtonsUITimer.ButtonsUIArray["pauseTimer"];
            }
            #endregion

            if (_map.Wall.CurrentHp <= _map.Wall.MaxHp / 4)
            {
                if (lowLifeBool && !lowLifeBool2) lowLife += 0.005f;
                if (lowLife >= 0.4)
                {
                    MediaPlayer.Volume = 0.01f;
                    ManagerSound.PlayLowLife();
                    lowLifeBool = false;
                }
                lowLifeBool2 = true;
                if (lowLifeBool2 && !lowLifeBool) lowLife -= 0.005f;
                if (lowLife <= 0) lowLifeBool = true;
                lowLifeBool2 = false;

                playLowLife += 1;
                if(playLowLife == 205)
                {
                    playLowLife = 0;
                }
            }
            foreach (Tower tow in _map.Towers)
            {
                    if (newStateMouse.X > tow.Position.X &&
                        newStateMouse.X < tow.Position.X + _imgMaps[1].Width &&
                        newStateMouse.Y > tow.Position.Y &&
                        newStateMouse.Y < tow.Position.Y + _imgMaps[1].Height)
                    {  
                        _hoveredTower = tow;
                        break;
                    }
                    else
                    {
                        _hoveredTower = null;   
                    }
            }

            if (realPause && !isLost && !_isWon)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (newStateMouse.X > button_rectangle_pause[i].X &&
                                newStateMouse.X < button_rectangle_pause[i].X + BUTTON_WIDTH &&
                                newStateMouse.Y > button_rectangle_pause[i].Y &&
                                newStateMouse.Y < button_rectangle_pause[i].Y + BUTTON_HEIGHT)
                    {
                        button_color_pause[i] = Color.LightBlue * 0.7f;
                    }
                    else
                    {
                        button_color_pause[i] = Color.White;
                    }
                }
            }

            

            if (isLost && !_isWon)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (newStateMouse.X > button_rectangle_lost[i].X &&
                                newStateMouse.X < button_rectangle_lost[i].X + BUTTON_WIDTH &&
                                newStateMouse.Y > button_rectangle_lost[i].Y &&
                                newStateMouse.Y < button_rectangle_lost[i].Y + BUTTON_HEIGHT)
                    {
                        button_color_lost[i] = Color.LightBlue * 0.7f;
                    }
                    else
                    {
                        button_color_lost[i] = Color.White;
                    }
                }
            }

            if (!isLost && _isWon)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (newStateMouse.X > button_rectangle_win[i].X &&
                                newStateMouse.X < button_rectangle_win[i].X + BUTTON_WIDTH &&
                                newStateMouse.Y > button_rectangle_win[i].Y &&
                                newStateMouse.Y < button_rectangle_win[i].Y + BUTTON_HEIGHT)
                    {
                        button_color_win[i] = Color.LightBlue * 0.7f;
                    }
                    else
                    {
                        button_color_win[i] = Color.White;
                    }
                }
            }

            if (_hoveredTower != null)
            {
                
                _towerCompteur--;
            }
            else
            {
                _towerCompteur = 60;
            }
            
            if (stratPause > 5)
                GameIsPaused = false;
          
            if (!GameIsPaused && realPause == false)
            {
                if (_map.Wall.IsDead())
                {
                    isLost = true;
                    GameIsPaused = true;
                }

                AnimationsDollars.Update((int)_map.Dollars);

                #region Air Raid Is Comming
                foreach (AirUnitsCollection _collection in _map.AirUnits)
                {
                    if (_collection.TimerBeforeStarting == 5 * 60)
                    {
                        ManagerSound.PlayAlertRaidUnitsAir();
                        _timerRaidAirClose = 5 * 60;
                        break;
                    }
                }
                #endregion

                #region Anim Sprite

                foreach (SimpleAnimationDefinition def in AnimSprites)
                {
                    for (int j = 0; j < def.AnimatedSprite.Count; j++)
                    {
                        SimpleAnimationSprite animatedSprite = def.AnimatedSprite[j];
                        animatedSprite.Update(gameTime);
                    }
                }

                #region Anim Heal
                for (int i = 0; i < _map.AnimHeal.Count; i++)
                {
                    Enemy doctor = _map.AnimHeal[i];
                    Vector2 positionDoctor = Vector2.Zero;

                    if (doctor.CurrentDirection == Move.left)
                        positionDoctor = new Vector2((int)doctor.Position.X - _imgEnemy1.Width - 25, (int)doctor.Position.Y - _imgEnemy1.Height);
                    else if (doctor.CurrentDirection == Move.right)
                        positionDoctor = new Vector2((int)doctor.Position.X - _imgEnemy1.Width / 2, (int)doctor.Position.Y - _imgEnemy1.Height);
                    if (doctor.CurrentDirection == Move.top)
                        positionDoctor = new Vector2((int)doctor.Position.X - _imgEnemy1.Width, (int)doctor.Position.Y - _imgEnemy1.Height - 10);
                    else if (doctor.CurrentDirection == Move.down)
                        positionDoctor = new Vector2((int)doctor.Position.X - _imgEnemy1.Width, (int)doctor.Position.Y - _imgEnemy1.Height / 2);

                    AnimSprites[4].AnimatedSprite.Add(new SimpleAnimationSprite(AnimSprites[4], (int)positionDoctor.X, (int)positionDoctor.Y));
                    _map.AnimHeal.Remove(doctor);
                }
                #endregion

                #region Anim Blood

                for (int i = 0; i < _map.DeadEnemies.Count; i++)
                {
                    Enemy deadEnemy = _map.DeadEnemies[i];
                    AnimSprites[1].AnimatedSprite.Add(new SimpleAnimationSprite(AnimSprites[1], (int)deadEnemy.Position.X, (int)deadEnemy.Position.Y));
                    _map.DeadEnemies.Remove(deadEnemy);
                }

                #endregion

                #region Anim Explosion Plane
                for (int i = 0; i < _map.DeadUnitsAir.Count; i++)
                {
                    AirUnit deadUnit = _map.DeadUnitsAir[i];
                    AnimSprites[2].AnimatedSprite.Add(new SimpleAnimationSprite(AnimSprites[2], (int)deadUnit.Position.X - 30, (int)deadUnit.Position.Y - 30));
                    _map.DeadUnitsAir.Remove(deadUnit);
                }
                #endregion

                #region Anim Saboteur
                for (int i = 0; i < _map.TowerDisabled.Count; i++)
                {
                    Tower tower = _map.TowerDisabled[i];
                    AnimSprites[3].AnimatedSprite.Add(new SimpleAnimationSprite(AnimSprites[3], (int)tower.Position.X - 148, (int)tower.Position.Y - 30, (int)BalanceEnemySaboteur.ENEMY_SABOTEUR_RELOADING));
                    _map.TowerDisabled.Remove(tower);
                }
                #endregion

                #endregion
            }
            if (isLost == true)
            {
                _isWon = false;
                policeBlink--;
                if (policeBlink == 0)
                {
                    policeBlink = 240;

                }else if(policeBlink == 239)
                {
                    ManagerSound.PlayGameOver();
                }
            }


            if (_isWon == true)
            {
                isLost = false;
                policeBlink2--;
                if (policeBlink2 == 0)
                {
                    policeBlink2 = 240;

                }else if(policeBlink2 == 239)
                {
                    ManagerSound.PlayYouWin();
                }
            }

            if (!isLost && _map.GetAllEnemies2().Count == 0 && _map.GetAllAirEnemies2().Count == 0)
            {
                _isWon = true;

                // Verify if map is map Campagne
                bool isMapCampagne = false;
                int nbMapCampagne = 15;
                int currentCampagne = 0;
                for (int i = 1; i < nbMapCampagne + 1; i++)
                {
                    if (_map.Name == "mapCampagne" + i.ToString())
                    {
                        isMapCampagne = true;
                        currentCampagne = i;
                        break;
                    }
                }
                if (isMapCampagne)
                {
                    // Deserialize class player
                    Player player = BinarySerializer.Deserialize<Player>(BinarySerializer.pathCurrentPlayer);
                    
                    // Verify currentCampagne and _lvlAccess
                    if (player._lvlAccess == currentCampagne)
                    {
                        player._lvlAccess++;
                        player.Serialize();
                    }
                }
            }

            if (nombre < 50) nombre += 4;
            if (nombre >= 50) nombre = 0;

            if (warning && nombre2 <=30 )
            {
                nombre2++;
            }
            if (!warning) nombre2 = 0;
            base.Update(gameTime);
        }

      

        protected void HandleInput(MouseState newStateMouse, MouseState lastStateMouse, KeyboardState newStateKeyboard, KeyboardState lastStateKeyboard)
        {
            // Not Pause
            if (!realPause)
            {
                #region Towers

                #region Buy Tower

                List<Vector2> emptyTowers = _map.SearchPositionTextureInArray(MapTexture.emptyTower);
                if (newStateMouse.LeftButton == ButtonState.Pressed &&
                    lastStateMouse.LeftButton == ButtonState.Released)
                {
                    if (_verif3 == false  && _groupOfButtonsUIAbilities.ButtonActivated == null)
                    {
                        foreach (Vector2 position in emptyTowers)
                        {
                            if (newStateMouse.X > position.X * Constant.imgSizeMap &&
                                newStateMouse.X < position.X * Constant.imgSizeMap + _imgMaps[1].Width &&
                                newStateMouse.Y > position.Y * Constant.imgSizeMap &&
                                newStateMouse.Y < position.Y * Constant.imgSizeMap + _imgMaps[1].Height)
                            {
                                _towerSelector = new Vector2(position.X * Constant.imgSizeMap, position.Y * Constant.imgSizeMap);
                                _verif = true;
                                _verif3 = true;
                            }
                        }
                    }
                }

                if (newStateMouse.LeftButton == ButtonState.Pressed &&
                lastStateMouse.LeftButton == ButtonState.Released)
                {
                    if (_towerSelector != new Vector2(-1000, -1000))
                    {

                        if (newStateMouse.X > _towerSelector.X - Constant.imgSizeMap &&
                        newStateMouse.X < (_towerSelector.X + Constant.imgSizeMap) - Constant.imgSizeMap &&
                        newStateMouse.Y > _towerSelector.Y - Constant.imgSizeMap &&
                        newStateMouse.Y < (_towerSelector.Y + Constant.imgSizeMap) - Constant.imgSizeMap)
                        {
                            if (_map.Dollars >= BalanceTowerSimple.TOWER_SIMPLE_BASE_PRICE)
                            {
                                _map.CreateTower(new Tower(_map, TowerType.simple, 1, _towerSelector));
                                _map.ChangeLocation((int)_towerSelector.X / Constant.imgSizeMap, (int)_towerSelector.Y / Constant.imgSizeMap, (int)MapTexture.notEmptyTower);
                                _map.Dollars -= BalanceTowerSimple.TOWER_SIMPLE_BASE_PRICE;
                                _towerSelector = new Vector2(-1000, -1000);
                                _verif3 = false;
                                _verif4 = true;
                            }
                        }
                        else if (newStateMouse.X > _towerSelector.X + Constant.imgSizeMap &&
                        newStateMouse.X < (_towerSelector.X + Constant.imgSizeMap) + Constant.imgSizeMap &&
                        newStateMouse.Y > _towerSelector.Y - Constant.imgSizeMap &&
                        newStateMouse.Y < (_towerSelector.Y + Constant.imgSizeMap) - Constant.imgSizeMap)
                        {
                            if (_map.Dollars >= BalanceTowerSlow.TOWER_SLOW_BASE_PRICE)
                            {
                                _map.CreateTower(new Tower(_map, TowerType.slow, 1, _towerSelector));
                                _map.ChangeLocation((int)_towerSelector.X / Constant.imgSizeMap, (int)_towerSelector.Y / Constant.imgSizeMap, (int)MapTexture.notEmptyTower);
                                _map.Dollars -= BalanceTowerSlow.TOWER_SLOW_BASE_PRICE;
                                _towerSelector = new Vector2(-1000, -1000);
                                _verif3 = false;
                                _verif4 = true;
                            }
                        }
                        else if (newStateMouse.X > _towerSelector.X - Constant.imgSizeMap &&
                        newStateMouse.X < (_towerSelector.X + Constant.imgSizeMap) - Constant.imgSizeMap &&
                        newStateMouse.Y > _towerSelector.Y + Constant.imgSizeMap &&
                        newStateMouse.Y < (_towerSelector.Y + Constant.imgSizeMap) + Constant.imgSizeMap)
                        {
                            if (_map.Dollars >= BalanceTowerArea.TOWER_AREA_BASE_PRICE)
                            {
                                _map.CreateTower(new Tower(_map, TowerType.area, 1, _towerSelector));
                                _map.ChangeLocation((int)_towerSelector.X / Constant.imgSizeMap, (int)_towerSelector.Y / Constant.imgSizeMap, (int)MapTexture.notEmptyTower);
                                _map.Dollars -= BalanceTowerArea.TOWER_AREA_BASE_PRICE;
                                _towerSelector = new Vector2(-1000, -1000);
                                _verif3 = false;
                                _verif4 = true;
                            }
                        }
                        else if (newStateMouse.X > _towerSelector.X + Constant.imgSizeMap &&
                        newStateMouse.X < (_towerSelector.X + Constant.imgSizeMap) + Constant.imgSizeMap &&
                        newStateMouse.Y > _towerSelector.Y + Constant.imgSizeMap &&
                        newStateMouse.Y < (_towerSelector.Y + Constant.imgSizeMap) + Constant.imgSizeMap)
                        {
                            if (_map.Dollars >= BalanceTowerBank.TOWER_BANK_BASE_PRICE)
                            {
                                _map.CreateTower(new Tower(_map, TowerType.bank, 1, _towerSelector));
                                _map.ChangeLocation((int)_towerSelector.X / Constant.imgSizeMap, (int)_towerSelector.Y / Constant.imgSizeMap, (int)MapTexture.notEmptyTower);
                                _map.Dollars -= BalanceTowerBank.TOWER_BANK_BASE_PRICE;
                                _towerSelector = new Vector2(-1000, -1000);
                                _verif3 = false;
                                _verif4 = true;
                            }
                        }
                        else if (_verif == false)
                        {
                            _towerSelector = new Vector2(-1000, -1000);
                            _verif3 = false;
                        }
                        _verif = false;
                    }
                    else
                    {
                        _verif3 = false;
                    }
                }

                #endregion

                #region Upgrade or sell Towers

                if (newStateMouse.LeftButton == ButtonState.Pressed &&
                lastStateMouse.LeftButton == ButtonState.Released)
                {
                    if (_towerSelector == new Vector2(-1000, -1000))
                    {
                        if (_verif4 == false)
                        {
                            foreach (Tower tow in _map.Towers)
                            {
                                if (newStateMouse.X > tow.Position.X &&
                                    newStateMouse.X < tow.Position.X + _imgMaps[1].Width &&
                                    newStateMouse.Y > tow.Position.Y &&
                                    newStateMouse.Y < tow.Position.Y + _imgMaps[1].Height)
                                {
                                    _towerSelectorUpgrade = new Vector2(tow.Position.X, tow.Position.Y);
                                    _verif2 = true;
                                    _myTow = tow;
                                    _verif3 = true;
                                    _verif4 = true;
                                }
                            }
                        }
                    }
                }
                if (newStateMouse.LeftButton == ButtonState.Pressed &&
                lastStateMouse.LeftButton == ButtonState.Released)
                {
                    if (_towerSelectorUpgrade != new Vector2(-1000, -1000))
                    {
                        if (newStateMouse.X > _towerSelectorUpgrade.X &&
                        newStateMouse.X < (_towerSelectorUpgrade.X + Constant.imgSizeMap) &&
                        newStateMouse.Y > _towerSelectorUpgrade.Y - Constant.imgSizeMap &&
                        newStateMouse.Y < (_towerSelectorUpgrade.Y + Constant.imgSizeMap) - Constant.imgSizeMap)
                        {
                            if (_myTow.Position == _towerSelectorUpgrade)
                            {
                                double priceTower = 0;
                                if (_myTow.Type == TowerType.simple) priceTower = BalanceTowerSimple.TOWER_SIMPLE_BASE_PRICE;
                                if (_myTow.Type == TowerType.slow) priceTower = BalanceTowerSlow.TOWER_SLOW_BASE_PRICE;
                                if (_myTow.Type == TowerType.area) priceTower = BalanceTowerArea.TOWER_AREA_BASE_PRICE;
                                if (_myTow.Type == TowerType.bank) priceTower = BalanceTowerBank.TOWER_BANK_BASE_PRICE;
                                if (_map.Dollars >= priceTower * 1.5 && _myTow.TowerLvl < 3)
                                {
                                    _myTow.Upgrade(_myTow);
                                    ManagerSound.PlayPowerUp();
                                    _verif4 = false;
                                    _towerSelectorUpgrade = new Vector2(-1000, -1000);
                                }
                            }
                            
                        }
                        else if (newStateMouse.X > _towerSelectorUpgrade.X &&
                        newStateMouse.X < (_towerSelectorUpgrade.X + Constant.imgSizeMap) &&
                        newStateMouse.Y > _towerSelectorUpgrade.Y + Constant.imgSizeMap &&
                        newStateMouse.Y < (_towerSelectorUpgrade.Y + Constant.imgSizeMap) + Constant.imgSizeMap)
                        {
                            if (_myTow.Position == _towerSelectorUpgrade)
                            {
                                _map.Towers.Remove(_myTow);
                                _map.ChangeLocation((int)_myTow.Position.X / Constant.imgSizeMap, (int)_myTow.Position.Y / Constant.imgSizeMap, (int)MapTexture.emptyTower);
                                _myTow.Sell(_myTow);
                                ManagerSound.PlaySell();
                                _verif4 = false;
                                _verif3 = false;
                            }
                            _towerSelectorUpgrade = new Vector2(-1000, -1000);
                        }
                        else if (_verif2 == false)
                        {
                            _towerSelectorUpgrade = new Vector2(-1000, -1000);
                            _verif4 = false;
                        }
                        _verif2 = false;
                    }
                    else
                    {
                        _verif4 = false;
                    }
                }
                if (newStateMouse.RightButton == ButtonState.Pressed &&
                        lastStateMouse.RightButton == ButtonState.Released)
                {
                    foreach (Tower tower in _map.Towers)
                    {
                        if (newStateMouse.X > tower.Position.X &&
                    newStateMouse.X < (tower.Position.X + Constant.imgSizeMap) &&
                    newStateMouse.Y > tower.Position.Y &&
                    newStateMouse.Y < (tower.Position.Y + Constant.imgSizeMap)
                    && tower.Type == TowerType.bank)
                        {
                            for (int j = 0; j < _map.Towers.Count; j++)
                            {
                                if (_map.Towers[j].Position == tower.Position)
                                {
                                    Tower tower2 = _map.Towers[j];
                                    if (tower2.Reload <= 0)
                                    {
                                        _map.Dollars += tower2.Earnings;
                                        tower2.Reload = BalanceTowerBank.TOWER_BANK_RELOADING;
                                        ManagerSound.PlaySell();
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                #endregion

                if (!GameIsPaused)
                {
                    #region Events
                    if (_map.Events.IsActivate && newStateKeyboard.IsKeyDown(Keys.S) && !lastStateKeyboard.IsKeyDown(Keys.S))
                        _map.Events.AddGauge();
                    #endregion
                }

                _groupOfButtonsUITimer.HandleInput(newStateMouse, lastStateMouse, newStateKeyboard, lastStateKeyboard);

                _groupOfButtonsUIAbilities.HandleInput(newStateMouse, lastStateMouse, newStateKeyboard, lastStateKeyboard);
            }
            // Pause
            if (realPause && !isLost && !_isWon)
            {
                
                
                    if (newStateMouse.LeftButton == ButtonState.Pressed &&
                        lastStateMouse.LeftButton == ButtonState.Released)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (newStateMouse.X > button_rectangle_pause[i].X &&
                                        newStateMouse.X < button_rectangle_pause[i].X + BUTTON_WIDTH &&
                                        newStateMouse.Y > button_rectangle_pause[i].Y &&
                                        newStateMouse.Y < button_rectangle_pause[i].Y + BUTTON_HEIGHT)
                            {
                                if (button_rectangle_pause[i] == button_rectangle_pause[0])
                                {
                                    realPause = false;
                                    break;
                                }
                                if (button_rectangle_lost[i] == button_rectangle_lost[1])
                                {
                                _map = BinarySerializer.Deserialize<Map>(BinarySerializer.pathCurrentMapXml);
                                isLost = false;
                                realPause = false;
                                GameIsPaused = false;
                                stratPause = 0;
                                Map.WavesCounter = 0;
                                break;

                            }
                                if (button_rectangle_pause[i] == button_rectangle_pause[2])
                                {
                                    Exit();
                                }
                            }
                        }
                    }
            }
            // Is Lost
            if (isLost && !_isWon)
            {
                if (newStateMouse.LeftButton == ButtonState.Pressed &&
                        lastStateMouse.LeftButton == ButtonState.Released)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (newStateMouse.X > button_rectangle_lost[i].X &&
                                    newStateMouse.X < button_rectangle_lost[i].X + BUTTON_WIDTH &&
                                    newStateMouse.Y > button_rectangle_lost[i].Y &&
                                    newStateMouse.Y < button_rectangle_lost[i].Y + BUTTON_HEIGHT)
                        {
                            if (button_rectangle_lost[i] == button_rectangle_lost[0])
                            {
                                Exit();
                            }
                            else if (button_rectangle_lost[i] == button_rectangle_lost[1])
                            {
                                _map = BinarySerializer.Deserialize<Map>(BinarySerializer.pathCurrentMapXml);
                                isLost = false;
                                realPause = false;
                                GameIsPaused = false;
                                stratPause = 0;
                                Map.WavesCounter = 0;
                                /*foreach (Spawn spawn in _map.SpawnsEnemies)
                                    Map.WavesTotals += spawn.Waves.Count;*/
                                break;
                            } 
                        }
                    }
                }
            }
            // IsWon
            if(_isWon && !isLost)
            {
                if (newStateMouse.LeftButton == ButtonState.Pressed &&
                        lastStateMouse.LeftButton == ButtonState.Released)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (newStateMouse.X > button_rectangle_win[i].X &&
                                    newStateMouse.X < button_rectangle_win[i].X + BUTTON_WIDTH &&
                                    newStateMouse.Y > button_rectangle_win[i].Y &&
                                    newStateMouse.Y < button_rectangle_win[i].Y + BUTTON_HEIGHT)
                        {
                            if (button_rectangle_win[i] == button_rectangle_win[0])
                            {
                                Exit();
                            }
                            else if (button_rectangle_win[i] == button_rectangle_win[1]) // button nextMap
                            {
                                // Verify if map is map Campagne
                                bool isMapCampagne = false;
                                int nbMapCampagne = 15;
                                int currentCampagne = 0;
                                for (int y = 1; y < nbMapCampagne + 1; y++)
                                {
                                    if (_map.Name == "mapCampagne" + y.ToString())
                                    {
                                        isMapCampagne = true;
                                        currentCampagne = y;
                                        break;
                                    }
                                }

                                if (isMapCampagne && currentCampagne != 15)
                                {
                                    int nbMap = (int)Char.GetNumericValue(_map.Name[_map.Name.Length - 1]);
                                    string _nameWorld = null;

                                    if (nbMap >= 1 && nbMap <= 5) _nameWorld = "World1";
                                    if (nbMap >= 6 && nbMap <= 10)
                                    {
                                        _nameWorld = "World2";
                                        nbMap -= 5;
                                    }
                                    if (nbMap >= 11 && nbMap <= 15)
                                    {
                                        nbMap -= 10;
                                        _nameWorld = "World3";
                                    }

                                    string[] filesInDirectory = Directory.GetFileSystemEntries(BinarySerializer.pathCampagneMap + "/" + _nameWorld);
                                    FileInfo file = new FileInfo(filesInDirectory[nbMap]);
                                    // On copie le fichier dans CurrentMap
                                    file.CopyTo(BinarySerializer.pathCurrentMapXml, true);

                                    _map = BinarySerializer.Deserialize<Map>(BinarySerializer.pathCurrentMapXml);

                                    isLost = false;
                                    _isWon = false;
                                    realPause = false;
                                    GameIsPaused = false;
                                    stratPause = 0;
                                    Map.WavesCounter = 0;
                                    foreach (Spawn spawn in _map.SpawnsEnemies)
                                        Map.WavesTotals += spawn.Waves.Count;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (newStateKeyboard.IsKeyDown(Keys.Escape) && lastStateKeyboard.IsKeyUp(Keys.Escape))
            {
                if (realPause == false)
                {
                    realPause = true;
                }
                else if (realPause)
                {
                    realPause = false;
                }

            }
            if (newStateKeyboard.IsKeyDown(Keys.Space) && lastStateKeyboard.IsKeyUp(Keys.Space))
            {
                if ( warning== false)
                    warning = true;
                else if (warning)
                    warning = false;

            }

            #region CHEATCODE
            // inflige 10 hp
            if (newStateKeyboard.IsKeyDown(Keys.N) && lastStateKeyboard.IsKeyUp(Keys.N))
            {
                List<Enemy> enemies = _map.GetAllEnemies();
                foreach (Enemy enemy in enemies)
                {
                    enemy.TakeHp(10);
                }
            }
            // reduce time to reload events
            if (newStateKeyboard.IsKeyDown(Keys.V) && lastStateKeyboard.IsKeyUp(Keys.V))
                _map.Events.Reloading = 0;

            // For look info hp
            if (newStateKeyboard.IsKeyDown(Keys.B) && lastStateKeyboard.IsKeyUp(Keys.B))
            {
                _map.Wall.ChangeHp((int)_map.Wall.MaxHp);
                _map.Wall.TakeHp(_map.Wall.MaxHp - _map.Wall.MaxHp / 4 + 5);
            }
            #endregion
        }


        /// <summary>
        /// This is called when the game should draw itself.elector
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, scale);
           

            #region Maps

            for (int y = 0; y < _map.HeightArrayMap; y++)
            {
                for (int x = 0; x < _map.WidthArrayMap; x++)
                {
                    spriteBatch.Draw(_imgMaps[_map.GetTypeArray(x, y)], new Vector2(x * Constant.imgSizeMap, y * Constant.imgSizeMap), null, Color.White);
                }
            }

            #endregion

            #region Decors
            foreach (Decor decor in _map.Decors)
                spriteBatch.Draw(_imgDecors[decor._numberDecor], new Vector2(decor._position.X - _imgDecors[decor._numberDecor].Width, decor._position.Y - _imgDecors[decor._numberDecor].Height), Color.White);
            #endregion

            #region Life Indicator
            if (_map.Wall.CurrentHp <= _map.Wall.MaxHp / 4)
            {
                spriteBatch.Draw(LowLifeIndicator[0], new Vector2(0, 0), Color.White * lowLife);
                spriteBatch.Draw(LowLifeIndicator[1], new Vector2(VirtualWidth - LowLifeIndicator[1].Width, 0), Color.White * lowLife);
                spriteBatch.Draw(LowLifeIndicator[2], new Vector2(0, VirtualHeight - LowLifeIndicator[2].Height), Color.White * lowLife);
                spriteBatch.Draw(LowLifeIndicator[3], new Vector2(0, 0), Color.White * lowLife);
            }
            #endregion

            #region StickRice
            if (_map.StickyRice.IsActivate && _map.StickyRice.PlaneIsClose)
                spriteBatch.Draw(_circleStickyRice, new Vector2(_map.StickyRice.Position.X - (_circleStickyRice.Width / 2), _map.StickyRice.Position.Y - (_circleStickyRice.Height / 2)), Color.White * 0.3f);
            if (_map.StickyRice.PositionPlaneOfRice != new Vector2(-1000, -1000))
            {
                Rectangle sourceRectangle = new Rectangle(0, 0, _imgPlaneTurbo.Width, _imgPlaneTurbo.Height);
                Vector2 origin = new Vector2(_imgPlaneTurbo.Width / 2, _imgPlaneTurbo.Height / 2);
                Vector2 direction = _map.StickyRice.PositionPlaneOfRice - new Vector2(VirtualWidth + _imgPlaneTurbo.Width, _map.StickyRice.PositionPlaneOfRice.Y);
                direction.Normalize();
                float _rotate = (float)Math.Atan2(-direction.X, direction.Y);
                spriteBatch.Draw(_imgPlaneTurbo, _map.StickyRice.PositionPlaneOfRice, null, Color.White, _rotate, origin, 1.0f, SpriteEffects.None, 1);
            }
            #endregion

            #region Wall

            Wall _wall = _map.Wall;
            spriteBatch.Draw(_imgWall, _wall.Position, Color.White);

            #endregion

            #region Enemies

            #region Earthly Enemies 
            List<Enemy> _enemies = _map.GetAllEnemies();
            foreach (Enemy enemy in _enemies)
            {
                float angle = 0;
                if (enemy.CurrentDirection == Move.right) angle = 0;
                else if (enemy.CurrentDirection == Move.down) angle = Constant.PI / 2;
                else if (enemy.CurrentDirection == Move.left) angle = Constant.PI;
                else if (enemy.CurrentDirection == Move.top) angle = 3 * Constant.PI / 2;
                // CHANGE TEXTURE ENEMY
                Texture2D _imgEnemy = null;
                if (enemy._type == EnemyType.defaultSoldier) _imgEnemy = _imgEnemy1;
                else if (enemy._type == EnemyType.boss1) _imgEnemy = _imgEnemy1;
                else if (enemy._type == EnemyType.boss2) _imgEnemy = _imgEnemy1;
                else if (enemy._type == EnemyType.boss2_1) _imgEnemy = _imgEnemy1;
                else if (enemy._type == EnemyType.boss3) _imgEnemy = _imgEnemy1;
                else if (enemy._type == EnemyType.kamikaze) _imgEnemy = _imgKamikaze;
                else if (enemy._type == EnemyType.doctor) _imgEnemy = _imgDoctor;
                else if (enemy._type == EnemyType.saboteur && enemy._hasCast == false) _imgEnemy = _imgSaboteur1;
                else if (enemy._type == EnemyType.saboteur && enemy._hasCast) _imgEnemy = _imgSaboteur; // When the saboteur used his charge.
                Rectangle sourceRectangle = new Rectangle(0, 0, _imgEnemy.Width, _imgEnemy.Height);
                Vector2 origin = new Vector2(_imgEnemy.Width / 2, _imgEnemy.Height / 2);
                spriteBatch.Draw(_imgEnemy, new Vector2(enemy.Position.X + (_imgEnemy.Width / 2), enemy.Position.Y + (_imgEnemy.Height / 2)), null, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);
                HealthBar enemyHealthBar = new HealthBar(enemy.CurrentHp, enemy.MaxHp, 1f, 1f);
                enemyHealthBar.Draw(spriteBatch, enemy.Position, _imgEnemy);
            }

            #endregion

            #region Air Enemies
            List<AirUnit> _airUnits = _map.GetAllAirEnemies();
                
                foreach (AirUnit unit in _airUnits)
                {
                    if (unit.IsStarting)
                    {
                        Rectangle sourceRectangle = new Rectangle(0, 0, _imgPlane1.Width, _imgPlane1.Height);
                        Vector2 origin = new Vector2(_imgPlane1.Width / 2, _imgPlane1.Height / 2);
                        spriteBatch.Draw(_imgPlane1, new Vector2(unit.Position.X + (_imgPlane1.Width / 2), unit.Position.Y + (_imgPlane1.Height / 2)), null, Color.White, unit.Rotate, origin, 1.0f, SpriteEffects.None, 1);
                        HealthBar enemyHealthBar = new HealthBar(unit.CurrentHp, unit.MaxHp, 1f, 1f);
                        enemyHealthBar.Draw(spriteBatch, unit.Position, _imgPlane1);
                }
                }
            #endregion

            #endregion

            #region Towers
            if (_hoveredTower != null)
            {
                if (_towerCompteur <= 0)
                {
                    if (_hoveredTower != _lastHoveredTower)
                    {
                        if (_hoveredTower.Type != TowerType.bank)
                        {
                            circleTower = createCircleText((int)_hoveredTower.Scope);
                        }
                    }
                    _lastHoveredTower = _hoveredTower;
                    if (_hoveredTower.Type != TowerType.bank)
                    {
                        spriteBatch.Draw(circleTower, new Vector2((_hoveredTower.Position.X - (circleTower.Width / 2) + Constant.imgSizeMap / 2), (_hoveredTower.Position.Y - (circleTower.Height / 2) + Constant.imgSizeMap / 2)), null, Color.Red * 0.3f);
                    }
                    spriteBatch.Draw(rect, _hoveredTower.Position + new Vector2(Constant.imgSizeMap + 5, 0), Color.White * 0.7f);
                    spriteBatch.Draw(_imgLvl, _hoveredTower.Position + new Vector2(((Constant.imgSizeMap - 10) + (VirtualWidth / 12) - ((((float)_map.WidthArrayMap / 64) * 128) - 5)), (VirtualHeight / 13) / 3), null, Color.White, 0, new Vector2(0, 0), (float)_map.WidthArrayMap / (64), SpriteEffects.None, 0);
                    spriteBatch.DrawString(_spriteDollars, "" + _hoveredTower.TowerLvl, _hoveredTower.Position + new Vector2(((Constant.imgSizeMap - 10) + (VirtualWidth / 12) - ((((float)_map.WidthArrayMap / 64) * 48))), ((VirtualHeight / 13) / 2) - 5), Color.Black);


                    if (_hoveredTower.Type != TowerType.bank)
                    {
                        spriteBatch.Draw(_imgTowerAttackSpeed, _hoveredTower.Position + new Vector2(Constant.imgSizeMap + 10, ((VirtualHeight / 13) - ((float)_map.WidthArrayMap / 64) * 64) - 5), null, Color.White, 0, new Vector2(0, 0), (float)_map.WidthArrayMap / (64), SpriteEffects.None, 0);
                        spriteBatch.DrawString(_spriteDollars, "" + _hoveredTower.Damage, _hoveredTower.Position + new Vector2(Constant.imgSizeMap + 20 + (((float)_map.WidthArrayMap / 64) * 64), ((((float)_map.WidthArrayMap / 64) * 64) / 2) - 2), Color.Red);
                        spriteBatch.Draw(_imgTowerDamages, _hoveredTower.Position + new Vector2(Constant.imgSizeMap + 10, 5), null, Color.White, 0, new Vector2(0, 0), (float)_map.WidthArrayMap / (64), SpriteEffects.None, 0);
                        spriteBatch.DrawString(_spriteDollars, "" + _hoveredTower._attackSpeed + "/s", _hoveredTower.Position + new Vector2(Constant.imgSizeMap + 20 + (((float)_map.WidthArrayMap / 64) * 64), ((VirtualHeight / 13) - ((((float)_map.WidthArrayMap / 64) * 64) / 2)) - 5), Color.Blue);
                    }
                    else
                    {
                        spriteBatch.Draw(_imgCoin, _hoveredTower.Position + new Vector2(Constant.imgSizeMap + 3, (VirtualHeight / 13) / 3), null, Color.White, 0, new Vector2(0, 0), (float)_map.WidthArrayMap / (64), SpriteEffects.None, 0);
                        spriteBatch.DrawString(_spriteDollars, "" + _hoveredTower.Earnings, _hoveredTower.Position + new Vector2((Constant.imgSizeMap + 3) + ((float)_map.WidthArrayMap / 64) * 64, (((VirtualHeight / 13) / 2) - 5)), Color.Blue);

                    }
                }
            }
            foreach (Tower tower in _map.Towers)
            {
                Texture2D _imgTower = null;
                if (tower.Type == TowerType.simple)
                {
                    if (tower.TowerLvl == 1) _imgTower = _imgTower1;
                    else if (tower.TowerLvl == 2) _imgTower = _imgTower1_2;
                    else if (tower.TowerLvl == 3) _imgTower = _imgTower1_3;
                }
                else if (tower.Type == TowerType.slow)
                {
                    if (tower.TowerLvl == 1) _imgTower = _imgTower2;
                    else if (tower.TowerLvl == 2) _imgTower = _imgTower2_2;
                    else if (tower.TowerLvl == 3) _imgTower = _imgTower2_3;
                }
                else if (tower.Type == TowerType.area)
                {
                    if (tower.TowerLvl == 1) _imgTower = _imgTower3;
                    else if (tower.TowerLvl == 2) _imgTower = _imgTower3_2;
                    else if (tower.TowerLvl == 3) _imgTower = _imgTower3_3;
                }
                else if(tower.Type == TowerType.bank)
                {
                    if (tower.Reload > 0)
                    {
                        if (tower.TowerLvl == 1) _imgTower = _imgTower4_empty;
                        else if (tower.TowerLvl == 2) _imgTower = _imgTower4_empty;
                        else if (tower.TowerLvl == 3) _imgTower = _imgTower4_empty;
                    }
                    else if(tower.Reload <= 0)
                    {
                        
                        if (tower.TowerLvl == 1) _imgTower = _imgTower4;
                        else if (tower.TowerLvl == 2) _imgTower = _imgTower4;
                        else if (tower.TowerLvl == 3) _imgTower = _imgTower4;
                    }
                    if(tower.Reload == 0)
                    {
                        ManagerSound.PlayCoinUp();
                    }
                }

                Rectangle sourceRectangle = new Rectangle(0, 0, _imgTower.Width, _imgTower.Height);
                Vector2 origin = new Vector2(_imgTower.Width / 2, _imgTower.Height / 2);
                spriteBatch.Draw(_imgTower, new Vector2(tower.Position.X + (_imgTower.Width / 2), tower.Position.Y + (_imgTower.Height / 2)), null, Color.White, tower.Angle, origin, 1.0f, SpriteEffects.None, 1);
            }

            if (_towerSelector != new Vector2(-1000, -1000))
            {
                spriteBatch.Draw(_imgSelector, _towerSelector + new Vector2(-Constant.imgSizeMap, -Constant.imgSizeMap), null, Color.White);
                spriteBatch.Draw(_imgTower1, _towerSelector + new Vector2(-Constant.imgSizeMap, -Constant.imgSizeMap), null, Color.White);
                spriteBatch.Draw(_imgSelector, _towerSelector + new Vector2(Constant.imgSizeMap, -Constant.imgSizeMap), null, Color.White);
                spriteBatch.Draw(_imgTower2, _towerSelector + new Vector2(Constant.imgSizeMap, -Constant.imgSizeMap), null, Color.White);
                spriteBatch.Draw(_imgSelector, _towerSelector + new Vector2(-Constant.imgSizeMap, Constant.imgSizeMap), null, Color.White);
                spriteBatch.Draw(_imgTower3, _towerSelector + new Vector2(-Constant.imgSizeMap, Constant.imgSizeMap), null, Color.White);
                spriteBatch.Draw(_imgSelector, _towerSelector + new Vector2(Constant.imgSizeMap, Constant.imgSizeMap), null, Color.White);
                spriteBatch.Draw(_imgTower4, _towerSelector + new Vector2(Constant.imgSizeMap, Constant.imgSizeMap), null, Color.White);


                if (_map.Dollars < BalanceTowerSimple.TOWER_SIMPLE_BASE_PRICE)
                {
                    spriteBatch.Draw(_imgWrong, _towerSelector + new Vector2(-Constant.imgSizeMap, -Constant.imgSizeMap), null, Color.White);
                }
                if (_map.Dollars < BalanceTowerSlow.TOWER_SLOW_BASE_PRICE)
                {
                    spriteBatch.Draw(_imgWrong, _towerSelector + new Vector2(Constant.imgSizeMap, -Constant.imgSizeMap), null, Color.White);
                }
                if (_map.Dollars < BalanceTowerArea.TOWER_AREA_BASE_PRICE)
                {
                    spriteBatch.Draw(_imgWrong, _towerSelector + new Vector2(-Constant.imgSizeMap, Constant.imgSizeMap), null, Color.White);
                }
                if (_map.Dollars < BalanceTowerBank.TOWER_BANK_BASE_PRICE)
                {
                    spriteBatch.Draw(_imgWrong, _towerSelector + new Vector2(Constant.imgSizeMap, Constant.imgSizeMap), null, Color.White);
                }
            }

            if (mx > _towerSelector.X - Constant.imgSizeMap &&
                    mx < (_towerSelector.X + Constant.imgSizeMap) - Constant.imgSizeMap &&
                    my > _towerSelector.Y - Constant.imgSizeMap &&
                    my < (_towerSelector.Y + Constant.imgSizeMap) - Constant.imgSizeMap)
            {
                spriteBatch.Draw(_imgTower1, _towerSelector, null, Color.White * 0.5f);
            }
            else if (mx > _towerSelector.X + Constant.imgSizeMap &&
                        mx < (_towerSelector.X + Constant.imgSizeMap) + Constant.imgSizeMap &&
                        my > _towerSelector.Y - Constant.imgSizeMap &&
                        my < (_towerSelector.Y + Constant.imgSizeMap) - Constant.imgSizeMap)
            {
                spriteBatch.Draw(_imgTower2, _towerSelector, null, Color.White * 0.5f);
            }
            else if (mx > _towerSelector.X - Constant.imgSizeMap &&
                        mx < (_towerSelector.X + Constant.imgSizeMap) - Constant.imgSizeMap &&
                        my > _towerSelector.Y + Constant.imgSizeMap &&
                        my < (_towerSelector.Y + Constant.imgSizeMap) + Constant.imgSizeMap)
            {
                spriteBatch.Draw(_imgTower3, _towerSelector, null, Color.White * 0.5f);
            }
            else if (mx > _towerSelector.X + Constant.imgSizeMap &&
                        mx < (_towerSelector.X + Constant.imgSizeMap) + Constant.imgSizeMap &&
                        my > _towerSelector.Y + Constant.imgSizeMap &&
                        my < (_towerSelector.Y + Constant.imgSizeMap) + Constant.imgSizeMap)
            {
                spriteBatch.Draw(_imgTower4, _towerSelector, null, Color.White * 0.5f);
            }

            
                
            
            
            
            

            if (_towerSelectorUpgrade != new Vector2(-1000, -1000))
            {
                spriteBatch.Draw(_imgSelector, _towerSelectorUpgrade + new Vector2(0, -(Constant.imgSizeMap + 5)), null, Color.White);
                spriteBatch.Draw(_imgUpgrade, _towerSelectorUpgrade + new Vector2(0, -(Constant.imgSizeMap + 5)), null, Color.White);
                spriteBatch.Draw(_imgSelector, _towerSelectorUpgrade + new Vector2(0, (Constant.imgSizeMap + 5)), null, Color.White);
                spriteBatch.Draw(_imgSell, _towerSelectorUpgrade + new Vector2(0, (Constant.imgSizeMap + 5)), null, Color.White);
                double priceTower = 0;
                if (_myTow.Type == TowerType.simple) priceTower = BalanceTowerSimple.TOWER_SIMPLE_BASE_PRICE;
                if (_myTow.Type == TowerType.slow) priceTower = BalanceTowerSlow.TOWER_SLOW_BASE_PRICE;
                if (_myTow.Type == TowerType.area) priceTower = BalanceTowerArea.TOWER_AREA_BASE_PRICE;
                if (_myTow.Type == TowerType.bank) priceTower = BalanceTowerBank.TOWER_BANK_BASE_PRICE;
                spriteBatch.DrawString(_upgradeFont, priceTower*1.5 +"$" ,_towerSelectorUpgrade + new Vector2(0, -(Constant.imgSizeMap + 30)), Color.White);

                if(_map.Dollars < priceTower * 1.5)
                {
                    spriteBatch.Draw(_imgWrong, _towerSelectorUpgrade +new Vector2(0,-(Constant.imgSizeMap +5)) , null, Color.White);
                }
                if(_myTow.TowerLvl == 3)
                {
                    spriteBatch.Draw(_imgWrong, _towerSelectorUpgrade + new Vector2(0, -(Constant.imgSizeMap + 5)), null, Color.White);
                }

                
            }


            #endregion

            #region WallBoss
            spriteBatch.Draw(_imgWall, _map.WallBoss.Position, Color.White);
            #endregion

            #region Missiles

            List<Missile> _missiles = _map.Missiles;
            foreach (Missile missile in _missiles)
            {
                Texture2D _imgMissileD  = null;
                if (missile.Tower == TowerType.simple) _imgMissileD = _imgMissile;
                else if (missile.Tower == TowerType.slow) _imgMissileD = _imgMissile;
                else if (missile.Tower == TowerType.area) _imgMissileD = _imgMissile;

                Rectangle sourceRectangle = new Rectangle(0, 0, _imgMissileD.Width, _imgMissileD.Height);
                Vector2 origin = new Vector2(_imgMissileD.Width / 2, _imgMissileD.Height / 2);
                spriteBatch.Draw(_imgMissileD, new Vector2(missile.Position.X + (_imgMissileD.Width / 2), missile.Position.Y + (_imgMissileD.Height / 2)), null, Color.White, missile.Angle, origin, 1.0f, SpriteEffects.None, 1);
            }

            #endregion

            #region Dollars

            Vector2 _positionDollars = new Vector2(10, 10);
            Rectangle _overlayDollars = new Rectangle(0, 0, 150, 33);
            spriteBatch.Draw(_backgroundDollars, new Vector2(5, 10), _overlayDollars, Color.Black * 0.6f);
            spriteBatch.Draw(_imgDollars, _positionDollars, Color.White);
            spriteBatch.DrawString(_spriteDollars, _map.Dollars + "", new Vector2(50, 17), Color.White);
            AnimationsDollars.Draw();

            #endregion

            #region Buttons

            #region Timer Buttons

            Vector2 _overlayManageTimePosition = new Vector2(_groupOfButtonsUITimer.ButtonsUIArray["pauseTimer"].Position.X - 5, _groupOfButtonsUITimer.ButtonsUIArray["pauseTimer"].Position.Y - 5);
            Rectangle _overlayManageTime = new Rectangle(0, 0, 143, 42);
            spriteBatch.Draw(_backgroundDollars, _overlayManageTimePosition, _overlayManageTime, Color.Black * 0.6f);
            _groupOfButtonsUITimer.Draw(spriteBatch);

            #endregion

            #region Abilities Buttons
            _groupOfButtonsUIAbilities.Draw(spriteBatch);
            
            #endregion

            #endregion

            #region North Korea is Comming

            Rectangle sourceRectanglee = new Rectangle(0, 0, 270, 33);
            spriteBatch.Draw(_backgroundDollars, new Vector2(5, 50), sourceRectanglee, Color.Black * 0.6f);
            spriteBatch.Draw(_flagNorthKorea, new Vector2(10, 50), Color.White);
            spriteBatch.DrawString(_imgNextWave, "Waves " + Map.WavesCounter + "/" + Map.WavesTotals, new Vector2(50, 57), Color.White);
            _waveSprite.Draw(GraphicsDevice, spriteBatch);

            #endregion

            #region Wall Health Bar
            HealthBar wallHealthBar = new HealthBar(_wall.CurrentHp, _wall.MaxHp, 7.8f, 3f);
            spriteBatch.Draw(_backgroundDollars, new Vector2(5, 90), sourceRectanglee, Color.Black * 0.6f);
            spriteBatch.Draw(_imgHearth, new Vector2(10, 90), Color.White);
            wallHealthBar.Draw(spriteBatch, new Vector2(123,117), _imgWall);
            #endregion

            #region Events
            if (_map.Events.IsActivate)
            {
                HealthBar entityBar = new HealthBar(_map.Events.CurrentEvent.CurrentGauge, _map.Events.CurrentEvent.MaxGauge, 7.8f, 3f);
                spriteBatch.Draw(_backgroundDollars, new Vector2(5, 150), sourceRectanglee, Color.Black * 0.6f);
                Texture2D entityFace = _imgVeryAngryFace;
                spriteBatch.Draw(entityFace, new Vector2(10, 150), Color.White);
                entityBar.Draw(spriteBatch, new Vector2(123, 177), _imgWall);
            }
            #endregion

            #region Raid Units Air is Comming
            if (_timerRaidAirClose > 0)
            {
                _shadowRaidAirClose += _shadowVar;
                Vector2 positionWarning = new Vector2((VirtualWidth / 2) - (_imgWarning.Width/2), (VirtualHeight / 2) - (_imgWarning.Height / 2));
                spriteBatch.Draw(_imgWarning, positionWarning, Color.White*_shadowRaidAirClose);
                spriteBatch.DrawString(_raidAirIsComming, "IMINENT AIR RAID !", new Vector2(positionWarning.X-110, positionWarning.Y+_imgWarning.Height), Color.Red * _shadowRaidAirClose);
                if (_shadowRaidAirClose >= 0.6) _shadowVar = -0.005f;
                else if (_shadowRaidAirClose <= 0.3) _shadowVar = 0.005f;
                _timerRaidAirClose--;
            }
            #endregion


            // ANIM EXPLOSION ABILITY
            foreach (SimpleAnimationDefinition def in AnimSprites )
            {
                foreach (SimpleAnimationSprite animatedSprite in def.AnimatedSprite) animatedSprite.Draw(gameTime, false);
            }
         
          
            #region Pause
            spriteBatch.Draw(_backgroundDollars, new Vector2(5, 133+60), sourceRectanglee, Color.Black * 0.6f);
            if (stratPause < 5)
            {
                spriteBatch.DrawString(_spriteDollars, "Pause :  " + stratPause + "/5", new Vector2(10, 140+60), Color.White);
            }
            else if (stratPause >= 5)
            {
                if(!warning) spriteBatch.DrawString(_spriteDollars, "Pause : 5/5", new Vector2(10, 140+60), Color.Red);
                {
                    if (nombre2 < 30)
                    {
                        if (warning)
                        {
                            for (int i = 0; i < nombre; i++)
                            {
                                spriteBatch.DrawString(_spriteDollars, "Pause : 5/5", new Vector2(10 + (nombre / 7), 140+60), Color.Red);
                            }

                            if (warning == true) spriteBatch.Draw(_imgWarning2, new Vector2(230 + (nombre / 7), 128+60), Color.White);
                        }
                    }
                    else
                    {
                        spriteBatch.DrawString(_spriteDollars, "Pause : 5/5", new Vector2(10, 140+60), Color.Red);
                    }
                }
            }

            if (realPause == true)
            {
                spriteBatch.Draw(grey, new Vector2(0, 0), Color.White * 0.5f);

                spriteBatch.Draw(button_texture_pause[0], button_rectangle_pause[0], button_color_pause[0]);
                spriteBatch.Draw(button_texture_pause[1], button_rectangle_pause[1], button_color_pause[1]);
                spriteBatch.Draw(button_texture_pause[2], button_rectangle_pause[2], button_color_pause[2]);


            }
            #endregion
            if (_isWon)
            {
                spriteBatch.Draw(grey, new Vector2(0, 0), Color.Black);
                Vector2 _sizeString = _gameOver.MeasureString("You Win !");
                spriteBatch.Draw(_gameOverExplosion, new Vector2(0, VirtualHeight - _gameOverExplosion.Height), Color.White);

                if (policeBlink2 > 120)
                {
                    spriteBatch.DrawString(_gameOver, "You win !", new Vector2((VirtualWidth / 2) - (_sizeString.X / 2), 0), Color.Red);
                }
                else if(policeBlink2 <=120)
                {
                    spriteBatch.DrawString(_gameOver, "You win !", new Vector2((VirtualWidth / 2) - (_sizeString.X / 2), 0), Color.White);
                }

                spriteBatch.Draw(button_texture_win[0], button_rectangle_win[0], button_color_win[0]);

                // Verify if map is map Campagne
                bool isMapCampagne = false;
                int nbMapCampagne = 15;
                int currentCampagne = 0;
                for (int y = 1; y < nbMapCampagne + 1; y++)
                {
                    if (_map.Name == "mapCampagne" + y.ToString())
                    {
                        isMapCampagne = true;
                        currentCampagne = y;
                        break;
                    }
                }

                if (isMapCampagne && currentCampagne != 15)
                    spriteBatch.Draw(button_texture_win[1], button_rectangle_win[1], button_color_win[1]);

                spriteBatch.Draw(_trumpWin, new Vector2(0, VirtualHeight-_trumpWin.Height), Color.White);
               

            }

            if (isLost)
            {
               
                spriteBatch.Draw(grey, new Vector2(0, 0), Color.Black);
                Vector2 _sizeStringLose = _gameOver.MeasureString("Game Over");
                spriteBatch.Draw(_gameOverExplosion, new Vector2(0, VirtualHeight - _gameOverExplosion.Height), Color.White);

                if (policeBlink > 120)
                {         
                    spriteBatch.DrawString(_gameOver, "Game Over", new Vector2((VirtualWidth / 2) - (_sizeStringLose.X / 2), 0), Color.Red);
                }
                else if (policeBlink <= 120)
                {
                    spriteBatch.DrawString(_gameOver, "Game Over", new Vector2((VirtualWidth / 2) - (_sizeStringLose.X / 2), 0), Color.White);
                }
                
                spriteBatch.Draw(button_texture_lost[0], button_rectangle_lost[0], button_color_lost[0]);
                spriteBatch.Draw(button_texture_lost[1], button_rectangle_lost[1], button_color_lost[1]);
                spriteBatch.Draw(_sadTrump, new Vector2(0, VirtualHeight - _sadTrump.Height), Color.White);
               
               /*spriteBatch.DrawString(_gameOver, "is won : " + _isWon, new Vector2(100, 100), Color.Red);
                spriteBatch.DrawString(_gameOver, "is lost : " + isLost, new Vector2(100, 200), Color.Red);
                spriteBatch.DrawString(_gameOver, "police blink : " + policeBlink, new Vector2(100, 300), Color.Red);
                spriteBatch.DrawString(_gameOver, "police blink2 : " + policeBlink2, new Vector2(100, 400), Color.Red);*/
            }
            #region Cursor

            if (_groupOfButtonsUIAbilities.ButtonActivated != null && _groupOfButtonsUIAbilities.ButtonActivated.Name == "explosionAbility")
            {
                spriteBatch.Draw(_imgCursorBomb, new Vector2(newStateMouse.X, newStateMouse.Y), Color.White);
                spriteBatch.Draw(_circleExplosion, new Vector2(newStateMouse.X - (_circleExplosion.Width / 2), newStateMouse.Y - (_circleExplosion.Height / 2)), Color.White * 0.1f);
            }
            else if (_groupOfButtonsUIAbilities.ButtonActivated != null && _groupOfButtonsUIAbilities.ButtonActivated.Name == "sniperAbility")
                spriteBatch.Draw(_cursorTarget, new Vector2(newStateMouse.X - _cursorTarget.Width / 2, newStateMouse.Y - _cursorTarget.Height / 2), Color.White);
            else if (_groupOfButtonsUIAbilities.ButtonActivated != null && _groupOfButtonsUIAbilities.ButtonActivated.Name == "stickyRiceAbility")
            {
                spriteBatch.Draw(_imgCursorDeliveryRice, new Vector2(newStateMouse.X - _imgCursorDeliveryRice.Width / 2, newStateMouse.Y - _imgCursorDeliveryRice.Height / 2), Color.White);
                spriteBatch.Draw(_circleStickyRice, new Vector2(newStateMouse.X - (_circleStickyRice.Width / 2), newStateMouse.Y - (_circleStickyRice.Height / 2)), Color.White * 0.1f);
            }
            else if (_groupOfButtonsUIAbilities.ButtonActivated != null && _groupOfButtonsUIAbilities.ButtonActivated.Name == "wallBossAbility")
            {
                spriteBatch.Draw(_imgBoss1Wall, new Vector2(newStateMouse.X - _imgBoss1Wall.Width / 2, newStateMouse.Y - _imgBoss1Wall.Height / 2), Color.White * 0.6f);
                spriteBatch.Draw(_imgCursorTrowel, new Vector2(newStateMouse.X - _imgCursorTrowel.Width / 2, newStateMouse.Y - _imgCursorTrowel.Height / 2), Color.White);
            }
            else
                spriteBatch.Draw(_imgCursorDefault, new Vector2(newStateMouse.X, newStateMouse.Y), Color.White);


            #endregion
         
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public Map Map => _map;
        public SpriteBatch SpriteBatch => spriteBatch;

        Texture2D createCircleText(int radius)
        {
            
            radius /= 2;

            Texture2D texture = new Texture2D(GraphicsDevice, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
    }
}
