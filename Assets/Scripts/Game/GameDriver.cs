using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;

/// <summary>
/// 游戏类
/// </summary>
public class GameDeiver : MonoBehaviour
{
    public CardBank startingDack;

    [Header("Managers")]
    [SerializeField] private CardManager cardManager;
    [SerializeField] private CardDeckManager cardDeckManager;
    [SerializeField] private CardDisplayManager cardDisplayManager;
    [SerializeField] private EffectResolutionManager effectResolutionManager;
    [SerializeField] private CardSelectionHasArrow cardSelectionHasArrow;

    private List<CardTemplate> _playerDeck = new List<CardTemplate>();

    private GameObject player;
    private List<GameObject> enemies = new List<GameObject>();

    [Header("Character Pivots")]
    [SerializeField] private Transform enemyPivot;
    [SerializeField] private AssetReference enemyTemplate;

    [Header("Character Pivots")]
    [SerializeField] private Transform playerPivot;
    [SerializeField] private AssetReference playerTemplate;


    private void Start()
    {
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
                Hp = 100,
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

            var obj = enemy.GetComponent<CharacterObject>();
            obj.characterTemplate = template;
            obj.Character = new RuntimeCharacter
            {
                Hp = 100,
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
}
