using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 중복 방지
        }
        UpdateFinalStats();
    }
    #region GameObject
    public Image curHpImage;
    public Image curMpImage;
    public Image ExpImage;
    public Image SkillIcon;
    public Image RollIcon;
    public Text curHpText;
    public Text curMpText;
    public Text Level;
    public Text AtkText;
    public Text DefText;
    public Text HpText;
    public Text MpText;
    public GameObject CutScene;
    #endregion
    #region Component
    Rigidbody rb;
    AnimationManagerUI _anim;
    Transform _character;
    Camera _camera;
    Transform _camAxis;
    #endregion
    #region rotation
    [SerializeField]      // 타겟이 될 게임오브젝트
    private Vector3 point = Vector3.zero;   // 타겟의 위치(바라볼 위치)
    public float _camSpeed = 4f;
    float _mouseX = 0;
    float _mouseY = 4;
    float _wheel = -10;
    float _characterRotX = 0;
    float _characterRotY = 0;
    #endregion
    #region Count
    private int ComboCount = 0;
    public int JumpCount = 0;
    #endregion
    #region Bool

    public bool isAttacking = false;
    public bool isGround = false;
    public bool isJumping = false;
    public bool isRolling = false;
    public bool isUnHit = false;
    public bool isTalkable = false;
    public bool isActiving = false;
    bool controlAble = true;
    #endregion
    #region Time
    private float AttackingTime = 0f;
    private float rollingCoolTime = 6f;
    private float rollingTime = 0.5f;
    private float skillCoolTime = 15f;
    public float cutSceneTime = 0;
    #endregion
    #region Stat
    public float baseHp = 100;
    public float baseMp = 100;
    public float baseAtk = 10;
    public float baseDef = 2;
    public float equipAtk = 0;
    public float equipDef = 0;
    public float equipHp = 0;
    public float equipMp = 0;
    public float FinalAtk { get; private set; }
    public float FinalDef { get; private set; }
    public float FinalHp { get; private set; }
    public float FinalMp { get; private set; }
    public float curHp;
    public float curMp;

    public int level = 1;
    public float maxExp = 100;
    public float curExp;
    #endregion
    #region AttackProcess
    void Attack()

    {
        if (Input.GetMouseButtonDown(0) && ComboCount == 0 && isGround == true && AttackingTime < 0.2f)
        {
            isActiving = true;
            rb.linearVelocity = Vector3.zero;
            isAttacking = true;
            _anim.SetAnimation_Hit01();
            AttackingTime = 0.8f;
            ComboCount++;
        }
        else if (Input.GetMouseButtonDown(0) && ComboCount == 1 && isGround == true && AttackingTime < 0.3f)
        {
            _anim.SetAnimation_Hit02();
            isAttacking = true;
            AttackingTime = 0.8f;
            ComboCount++;
        }
        else if (Input.GetMouseButtonDown(0) && ComboCount == 2 && isGround == true && AttackingTime < 0.4f)
        {
            isAttacking = true;
            _anim.SetAnimation_Hit03();
            AttackingTime = 1.3f;

            ComboCount = 0;
        }
        if (AttackingTime <= 0f)
        {
            ComboCount = 0;
            isAttacking = false;
        }

    }
    void Combo1()
    {

        if (Input.GetMouseButtonDown(1) && isGround == true && skillCoolTime >=1 && curMp >= 30)
        {
            curMp -= 30;
            skillCoolTime = 0;
            cutSceneTime = 1f;
            isActiving = true;
            rb.linearVelocity = Vector3.zero;
            isAttacking = true;
            _anim.SetAnimation_Combo();
            AttackingTime = 2.5f;
        }
    }
    #endregion
    #region Camera
    void CameraMove()
    {
        _mouseX = Input.GetAxis("Mouse X");
        _mouseY = Input.GetAxis("Mouse Y");
        _camAxis.position = transform.position + new Vector3(0, 3, 0);

        if (_characterRotY > 10)
            _characterRotY = 10;
        if (_characterRotY < 0)
            _characterRotY = 0;

        if (!Input.GetKey(KeyCode.LeftAlt))
        {
            _characterRotX += _mouseX; _characterRotY -= _mouseY;
            _camAxis.rotation = Quaternion.Euler(new Vector3(_camAxis.rotation.x + _characterRotY, _camAxis.rotation.y + _characterRotX, 0) * _camSpeed);
        }
        else if (Input.GetKey(KeyCode.LeftAlt))
        {
            _camAxis.rotation = Quaternion.Euler(new Vector3(_camAxis.rotation.x + _characterRotY, _camAxis.rotation.y + _characterRotX, 0) * _camSpeed);
        }

    }
    void Zoom()
    {
        _wheel += Input.GetAxis("Mouse ScrollWheel") * 5;
        if (_wheel >= -3)
        {
            _wheel = -3f;
        }
        if (_wheel <= -15)
        {
            _wheel = -15;
        }
        _camera.transform.localPosition = new Vector3(0, 0, _wheel);

        for (int i = 0; i < 13; i++)
        {
            if (_wheel > -(15 - i))
            {
                for (int j = 1; j < (2 * i); j++)
                {
                    _camera.transform.localPosition = new Vector3(0, -0.075f * j, _wheel);
                }
            }
        }
    }
    #endregion
    #region Move
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isAttacking == false && isGround == true)
        {
            isActiving = true;
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(Vector3.up * 250);
        }
    }
    void Roll()
    {
        if (Input.GetKeyDown(KeyCode.G) && rollingCoolTime >= 6 && isGround == true)
        {
            _anim.SetAnimation_Roll();
            rollingTime = 0.5f;
            rollingCoolTime = 0f;
            isRolling = true;
            rb.AddForce(_character.forward * 500);
            isActiving = true;
        }
        if (rollingTime <= 0f && rollingTime >= -0.1f)
        {
            rb.linearVelocity = Vector3.zero;
            isRolling = false;
            isAttacking = false;
        }

    }
    void Move()
    {
        if (isAttacking == false && isRolling == false)
        {

            if ((Input.GetButton("Horizontal") || Input.GetButton("Vertical")))
            {
                isActiving = true;
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");
                float speed = 1.5f;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    speed = 3 * speed;
                    if (isJumping == false)
                    {
                        _anim.SetAnimation_Run();
                    }
                }
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    speed = speed / 3;
                }
                if (!Input.GetKey(KeyCode.LeftShift) && isJumping == false)
                {
                    _anim.SetAnimation_Walk();
                }
                transform.rotation = Quaternion.Euler(new Vector3(0, _camAxis.rotation.y + _characterRotX, 0) * _camSpeed);

                Vector3 movement = new Vector3(h, 0, v);
                transform.Translate(movement.normalized * Time.deltaTime * speed);

                _character.transform.localRotation = Quaternion.Slerp(_character.transform.localRotation, Quaternion.LookRotation(movement), speed * Time.deltaTime);
            }
            else
            {
                _anim.SetAnimation_Idle();
                isActiving = false;
            }
            _character.eulerAngles = new Vector3(0, _character.eulerAngles.y, 0);
        }

    }
    #endregion
    #region CoolTime
    void TimeProcess()
    {
        AttackingTime -= Time.deltaTime;
        rollingTime -= Time.deltaTime;
        cutSceneTime -= Time.deltaTime;
        #region CollTime
        rollingCoolTime += Time.deltaTime;
        skillCoolTime += Time.deltaTime;
        #endregion
    }
    #endregion
    #region OnDamage
    public void OnDamage(float causerAtk)
    {

        float HitDmg = causerAtk - baseDef;
        if (HitDmg <= 0) { HitDmg = 0; }
        curHp -= HitDmg;
        if (curHp > 0)
        {
            DamageTextSpawn(transform.position, HitDmg);
            isUnHit = true;
            StartCoroutine(UnHitable());
        }
        else if (curHp <= 0) { curHp = 0; /*HpBar();*/ /*DeathPos = transform.position;*/ gameObject.SetActive(false); /*PlayerDieUI.SetActive(true);*/ }
    }
    void DamageTextSpawn(Vector3 position, float damage)
    {
        var DmgTextPlayer = ObjectPoolManager.instance.GetObject("DamageTextPlayer");
        DmgTextPlayer.transform.position = position;
        DmgTextPlayer.GetComponent<DamageText>().SetText(damage);
    }
    IEnumerator UnHitable()
    {
        int countTime = 0;
        while (countTime < 10)
        {
            yield return new WaitForSeconds(0.2f);
            countTime++;
        }
        isUnHit = false;
        yield return null;
    }
    #endregion
    #region LevelUp
    void LevelUp()
    {
        if (curExp >= maxExp)
        {
            float surPlusExp = curExp - maxExp;
            level++;
            var LevelUpObj = ObjectPoolManager.instance.GetObject("LevelUp");
            LevelUpObj.transform.position = this.transform.position + new Vector3(0, 0.5f, 0);
            LevelUpObj.transform.rotation = this.transform.rotation;
            LevelUpEffect.LifeTime = 2f;
            maxExp += 100;
            baseAtk += 5;
            baseDef += 2;
            baseHp += 20;
            baseMp += 20;
            curExp = 0;
            curExp += surPlusExp;
            StartCoroutine(SetStat());
            curHp = FinalHp;
            curMp = FinalMp;
        }
    }
    #endregion
    #region ImageFilled
    void HpBar()
    {
        curHpImage.fillAmount = curHp / FinalHp;
        curHpText.text = $"{curHp} / {FinalHp}";
    }
    void MpBar()
    {
        curMpImage.fillAmount = curMp / FinalMp;
        curMpText.text = $"{curMp}/{FinalMp}";
    }
    void ExpBar()
    {
        ExpImage.fillAmount = curExp / maxExp;
        Level.text = $"LV. {level}";
    }
    void CutScenePro()
    {
        if (cutSceneTime >= 0)
        {
            CutScene.SetActive(true);
        }
        else CutScene.SetActive(false);
    }
    void Icon()
    {
        SkillIcon.fillAmount = skillCoolTime / 15;
        RollIcon.fillAmount = rollingCoolTime / 6;
    }
    #endregion
    #region InterectNPC
    void TalkToNpc()
    {
        if (DialogueManager.Instance.isTalking == true)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    #endregion
    #region SetStats
    public void EquipItemBonusStat(ItemData item)
    {
    
        equipAtk += item.equipAtkBonus;
        equipDef += item.equipDefBonus;
        equipHp += item.equipHpBonus;
        equipMp += item.equipMpBonus;
        
        StartCoroutine(SetStat());
    }
    public void EquipItemMinusStat(ItemData item)
    {

        equipAtk -= item.equipAtkBonus;
        equipDef -= item.equipDefBonus;
        equipHp -= item.equipHpBonus;
        equipMp -= item.equipMpBonus;

        StartCoroutine(SetStat());
    }

    public void UsePotion(ItemData item)
    {
        curHp += item.hpRecovery;
        curMp += item.mpRecovery;
        if (curHp > FinalHp)
        {
            curHp = FinalHp;
        }
        if (curMp > FinalMp)
        {
            curMp = FinalMp;
        }

    }
    public void UpdateFinalStats()
    {
        FinalAtk = baseAtk + equipAtk;
        FinalDef = baseDef + equipDef;
        FinalHp = baseHp + equipHp;
        FinalMp = baseMp + equipMp;
        AtkText.text = $"{FinalAtk}";
        DefText.text = $"{FinalDef}";
        HpText.text = $"{FinalHp}";
        MpText.text = $"{FinalMp}";
        if (curHp > FinalHp)
        {
            curHp = FinalHp;
        }
        if (curMp > FinalMp)
        {
            curMp = FinalMp;
        }
    }
    IEnumerator SetStat()
    {
        yield return new WaitForSeconds(0.01f);
        UpdateFinalStats();
        yield return null;
    }
    #endregion
    #region Trigger,Collision Event
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("EnemyAtkBox") && isUnHit == false)
        {
            Enemy enemy = other.GetComponentInParent<Enemy>();
            if(enemy!=null)
            OnDamage(enemy.atk);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Npc"))
        {
            isTalkable = true;
        }
        //if (other.CompareTag("QuestObject"))
        //{
        //    isTalkable = true;
        //}
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Npc"))
        {
            isTalkable = false;
        }
        //if (other.CompareTag("QuestObject"))
        //{
        //    isTalkable = false;
        //}
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            _anim.SetAnimation_Idle();
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGround = true;
            isJumping = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isJumping = true;
            isGround = false;
            JumpCount = 1;
        }
    }
    #endregion
    #region PlayerController
    void PlayerControlAble()
    {
        if (   DialogueManager.Instance.isTalking == true 
            || InventoryManager.Instance.isOpenInven == true
            || EquipmentManager.Instance.isOpenEquip == true)
        { controlAble = false; }
        else
        { controlAble = true; }
        
        if (controlAble == true)
        {
            Jump();
            Combo1();
            Attack();
            CameraMove();
            Zoom(); Roll();
            Move();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            _anim.SetAnimation_Idle();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    #endregion

    #region LifeCycle
    void Start()
    {
        _character = transform.GetChild(0);
        rb = gameObject.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.mass = 5f;
        // * Collider
        CapsuleCollider capsule = gameObject.AddComponent<CapsuleCollider>();
        capsule.center = new Vector3(0, 1, 0);
        capsule.radius = 0.4f;
        capsule.height = 2f;
        
        

        rb.mass = 1;
        // * 카메라
        _camera = Camera.main;
        _camAxis = new GameObject("CamAxis").transform;
        _camera.transform.parent = _camAxis;
        _camera.transform.position = new Vector3(0, 3, -5);

        
        _anim = GameObject.Find("AnimationManagerUI").GetComponent<AnimationManagerUI>();
        curHp = FinalHp;
        curMp = FinalMp;
    }
    private void Update()
    {
        point = gameObject.transform.position;
        TimeProcess();
        TalkToNpc();
        LevelUp();
        CutScenePro();
        PlayerControlAble();
        
        if (isGround == false)
        {
            _anim.SetAnimation_Jump();
        }
        MpBar(); 
        HpBar();
        ExpBar();
        Icon();
    }
    
    #endregion
}
