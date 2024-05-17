using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;

/// <summary>
/// 游戏进程类
/// </summary>
public class GameDeiver : MonoBehaviour
{
    public CardBank startingDack;
    public Canvas canvas;
    private Camera mainCamera;

    [Header("Managers")]
    [SerializeField] private CardManager cardManager;
    [SerializeField] private CardDeckManager cardDeckManager;
    [SerializeField] private CardDisplayManager cardDisplayManager;
    [SerializeField] private EffectResolutionManager effectResolutionManager;
    [SerializeField] private CardSelectionHasArrow cardSelectionHasArrow;

    private List<CardTemplate> _playerDeck = new List<CardTemplate>();

    private GameObject player;
    private List<GameObject> enemies = new List<GameObject>();

    [SerializeField] private GameObject hpWidget;

    [Header("Character Pivots")]
    [SerializeField] private Transform enemyPivot;
    [SerializeField] private AssetReference enemyTemplate;

    [Header("Character Pivots")]
    [SerializeField] private Transform playerPivot;
    [SerializeField] private AssetReference playerTemplate;

    [SerializeField] private IntVariable enemyHp;
    [SerializeField] private IntVariable playerHp;


    private void Start()
    {
        mainCamera = Camera.main;
        cardManager.Initialize();
        CreatePlayer(playerTemplate);
        CreateEnemy(enemyTemplate);
    }

    /// <summary>
    /// 创建玩家,并创建初始牌库
    /// </summary>
    private void CreatePlayer(AssetReference templateReference)
    {
        var handle = Addressables.LoadAssetAsync<HeroTemplate>(templateReference);
        handle.Completed += operationResult =>
        {
            var template = operationResult.Result;
            player = Instantiate(template.Prefab, playerPivot);
            Assert.IsNotNull(player);

            foreach (var item in template.StartDeck.Items)
            {
                for (int i = 0; i < item.Amount; i++)
                {
                    _playerDeck.Add(item.Card);
                }
            }

            var obj = player.GetComponent<CharacterObject>();
            obj.characterTemplate = template;
            obj.Character = new RuntimeCharacter
            {
                Hp = playerHp,
                Shield = 100,
                Mana = 100,
                MaxHp = 100,
            };
            Initialize();
        };
        
    }

    /// <summary>
    /// 异步加载敌人
    /// </summary>
    /// <param name="templateReference"></param>
    private void CreateEnemy(AssetReference templateReference)
    {
        var handle = Addressables.LoadAssetAsync<EnemyTemplate>(templateReference);
        handle.Completed += operationResult =>
        {
            var pivot = enemyPivot;
            var template = operationResult.Result;
            var enemy = Instantiate(template.Prefab, pivot);
            Assert.IsNotNull(enemy);

            enemyHp.Value = 20;
            CreateHpWidget(hpWidget, enemy, enemyHp, 20);

            var obj = enemy.GetComponent<CharacterObject>();
            obj.characterTemplate = template;
            obj.Character = new RuntimeCharacter
            {
                Hp = enemyHp,
                Shield = 100,
                Mana = 100,
                MaxHp = 100,
            };
            enemies.Add(enemy);
        };
    }

    /// <summary>
    /// 创建初始摸牌堆，洗牌，并抽取初始手牌
    /// </summary>
    private void Initialize()
    {
        cardDeckManager.LaodDeck(_playerDeck);
        cardDeckManager.DeckShuffle();
        cardDisplayManager.Initialize(cardManager);
        cardDeckManager.DrawCardsFormDeck(5);

        var playerCharacter = player.GetComponent<CharacterObject>();
        var enemyCharacters = new List<CharacterObject>();

        foreach(var enemy in enemies)
        {
            enemyCharacters.Add(enemy.GetComponent<CharacterObject>());
        }
        cardSelectionHasArrow.Initialize(playerCharacter, enemyCharacters);
        effectResolutionManager.Initialize(playerCharacter, enemyCharacters);
    }

    /// <summary>
    /// 创建血条
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="character"></param>
    /// <param name="hp"></param>
    /// <param name="maxHp"></param>
    private void CreateHpWidget(GameObject prefab, GameObject character, IntVariable hp, int maxHp)
    {
        var hpObj = Instantiate(prefab, canvas.transform, false);
        var pivot = character.transform;
        var canvasPosition = mainCamera.WorldToViewportPoint(pivot.position + new Vector3(0.0f, -0.3f, 0.0f));
        hpObj.GetComponent<RectTransform>().anchorMin = canvasPosition;
        hpObj.GetComponent<RectTransform>().anchorMax = canvasPosition;
        hpObj.GetComponent<HpWidget>().Initialize(hp, maxHp);
    }
}
