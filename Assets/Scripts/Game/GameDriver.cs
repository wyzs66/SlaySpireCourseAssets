using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;

/// <summary>
/// ��Ϸ������
/// ��Ϸ������ڣ�����ϵͳ�ĳ�ʼ�������￪ʼ
/// </summary>
public class GameDeiver : MonoBehaviour
{
    //��ʼ�ƶ�
    public CardBank startingDack;

    public Canvas canvas;
    private Camera mainCamera;

    //�������
    public Texture2D cursorTexture2D;
    public CursorMode cursorMode = CursorMode.Auto;

    [Header("Managers")]
    [SerializeField] private CardManager cardManager;
    [SerializeField] private CardDeckManager cardDeckManager;
    [SerializeField] private CardDisplayManager cardDisplayManager;
    [SerializeField] private EffectResolutionManager effectResolutionManager;
    [SerializeField] private CardSelectionHasArrow cardSelectionHasArrow;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private EnemyAIManager enemyAIManager;

    private List<CardTemplate> _playerDeck = new List<CardTemplate>();

    private GameObject player;
    private List<GameObject> enemies = new List<GameObject>();

    [SerializeField] private GameObject enemyHpWidget;//����Ѫ��UI
    [SerializeField] private GameObject playerHpWidget;//���Ѫ��UI

    [Header("Character Pivots")]
    [SerializeField] private Transform enemyPivot;
    [SerializeField] private AssetReference enemyTemplate;

    [Header("Character Pivots")]
    [SerializeField] private Transform playerPivot;
    [SerializeField] private AssetReference playerTemplate;

    [SerializeField] private IntVariable enemyHp;
    [SerializeField] private IntVariable playerHp;

    [SerializeField] private IntVariable enemyShield;//���˻���ֵ
    [SerializeField] private IntVariable playerShield;//��һ���ֵ


    private void Start()
    {
        SetCursor();
        mainCamera = Camera.main;
        cardManager.Initialize();
        CreatePlayer(playerTemplate);
        CreateEnemy(enemyTemplate);
    }

    private void SetCursor()
    {
        var x = cursorTexture2D.width / 2.0f;
        var y = cursorTexture2D.height / 2.0f;
        Cursor.SetCursor(cursorTexture2D, new Vector2(x, y), cursorMode);

    }

    /// <summary>
    /// �������,��������ʼ�ƿ�
    /// </summary>
    private void CreatePlayer(AssetReference templateReference)
    {
        var handle = Addressables.LoadAssetAsync<HeroTemplate>(templateReference);
        handle.Completed += operationResult =>
        {
            var template = operationResult.Result;
            player = Instantiate(template.Prefab, playerPivot);
            Assert.IsNotNull(player);
            playerHp.Value = 20;
            playerShield.Value = 0;
            CreateHpWidget(playerHpWidget, player, playerHp, 30, playerShield);

            foreach (var item in template.StartDeck.Items)
            {
                for (int i = 0; i < item.Amount; i++)
                {
                    _playerDeck.Add(item.Card);
                }
            }

            var obj = player.GetComponent<CharacterObject>();
            obj.Template = template;
            obj.Character = new RuntimeCharacter
            {
                Hp = playerHp,
                Shield = playerShield,
                Mana = 100,
                MaxHp = 100,
            };
            Initialize();
        };
        
    }

    /// <summary>
    /// �첽���ص���
    /// </summary>
    /// <param name="templateReference">���˵Ļ���ģ��</param>
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
            enemyShield.Value = 0;
            CreateHpWidget(enemyHpWidget, enemy, enemyHp, 20, enemyShield);

            var obj = enemy.GetComponent<CharacterObject>();
            obj.Template = template;
            obj.Character = new RuntimeCharacter
            {
                Hp = enemyHp,
                Shield = enemyShield,
                Mana = 100,
                MaxHp = 100,
            };
            enemies.Add(enemy);
        };
    }

    /// <summary>
    /// ����ϵͳ�ĳ�ʼ��
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
        enemyAIManager.Initialize(playerCharacter, enemyCharacters);
        effectResolutionManager.Initialize(playerCharacter, enemyCharacters);
        turnManager.BeginGame();
    }

    /// <summary>
    /// ����Ѫ��
    /// </summary>
    /// <param name="prefab">Ѫ��Ԥ����</param>
    /// <param name="character">Ѫ����Ŀ��ʵ��</param>
    /// <param name="hp">Ѫ����ǰѪ��</param>
    /// <param name="maxHp">���Ѫ��</param>
    /// <param name="shield">����ֵ</param>
    private void CreateHpWidget(GameObject prefab, GameObject character, IntVariable hp, int maxHp, IntVariable shield)
    {
        var hpObj = Instantiate(prefab, canvas.transform, false);
        var pivot = character.transform;
        var canvasPosition = mainCamera.WorldToViewportPoint(pivot.position + new Vector3(0.0f, -0.3f, 0.0f));
        hpObj.GetComponent<RectTransform>().anchorMin = canvasPosition;
        hpObj.GetComponent<RectTransform>().anchorMax = canvasPosition;
        hpObj.GetComponent<HpWidget>().Initialize(hp, maxHp, shield);
    }
}
