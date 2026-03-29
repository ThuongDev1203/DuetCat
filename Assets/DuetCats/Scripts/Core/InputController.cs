using UnityEngine;
using UnityEngine.InputSystem;
using Spine.Unity;
using System.Collections.Generic;
using DuetCats.Scripts.Gameplay;

namespace DuetCats.Scripts.Core
{
    public class InputController : MonoBehaviour
    {
        public static InputController Instance;

        [Header("Cats")]
        public Transform leftCat;
        public Transform rightCat;

        [Header("Mouth Points (for collision)")]
        public Transform leftMouthPoint;
        public Transform rightMouthPoint;

        [Header("Animation")]
        public SkeletonAnimation leftAnim;
        public SkeletonAnimation rightAnim;
        public SkeletonAnimation leftTailAnim;
        public SkeletonAnimation rightTailAnim;

        string idle = "Idle_Start";
        string idlePlaying = "Idle_Playing";
        bool isPlayingAnim = false;
        bool isWinState = false;

        [Header("Smooth Movement")]
        public float smoothTime = 0.05f;
        public float maxSpeed = 25f;

        [Header("Lane Boundary")]
        [SerializeField] float leftMinVX = 0.15f;
        [SerializeField] float leftMaxVX = 0.35f;
        [SerializeField] float rightMinVX = 0.65f;
        [SerializeField] float rightMaxVX = 0.85f;

        Camera cam;
        float zDepth;
        float worldLeftMin, worldLeftMax, worldRightMin, worldRightMax;
        Vector3 velocityLeft, velocityRight;
        float lastLeftX, lastRightX;

        class TouchData { public bool isLeft; }
        Dictionary<int, TouchData> touches = new Dictionary<int, TouchData>();
        bool mouseLeft;

        [Header("Collision Check")]
        public float checkRadius = 0.2f;
        public LayerMask noteLayer;

        //Can Control
        bool canControl = false;

        void Awake()
        {
            Instance = this;
            cam = Camera.main;
            zDepth = Mathf.Abs(cam.transform.position.z);
        }

        void Start()
        {
            worldLeftMin = cam.ViewportToWorldPoint(new Vector3(leftMinVX, 0, zDepth)).x;
            worldLeftMax = cam.ViewportToWorldPoint(new Vector3(leftMaxVX, 0, zDepth)).x;
            worldRightMin = cam.ViewportToWorldPoint(new Vector3(rightMinVX, 0, zDepth)).x;
            worldRightMax = cam.ViewportToWorldPoint(new Vector3(rightMaxVX, 0, zDepth)).x;

            lastLeftX = leftCat.position.x;
            lastRightX = rightCat.position.x;

            SetAnimation(false);

            //Tail looping
            PlayTailLoop(leftTailAnim, "Tail");
            PlayTailLoop(rightTailAnim, "Tail");
        }

        void Update()
        {
            if (!canControl) return;

            HandleTouch();
            HandleMouse();

            if (GameManager.Instance.IsPlaying)
            {
                CheckCatNoteCollision(leftMouthPoint, leftAnim);
                CheckCatNoteCollision(rightMouthPoint, rightAnim);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("TEST WIN ANIM");
                PlayWinAnimation();
            }
        }

        public void StartAfterTutorial()
        {
            canControl = true;
            SetAnimation(true);
        }

        public void ResetInput()
        {
            canControl = false;
            isWinState = false;

            SetAnimation(false);
        }

        //TAIL HELPER
        void PlayTailLoop(SkeletonAnimation tailAnim, string animName)
        {
            if (tailAnim == null) return;
            if (tailAnim.Skeleton.Data.FindAnimation(animName) == null) return;

            tailAnim.AnimationState.SetAnimation(1, animName, true);
        }

        void SetAnimation(bool playing)
        {
            if (GameManager.Instance.IsGameOver || isWinState) return;

            if (isPlayingAnim == playing) return;
            isPlayingAnim = playing;

            string anim = playing ? idlePlaying : idle;

            if (leftAnim.Skeleton.Data.FindAnimation(anim) != null)
                leftAnim.AnimationState.SetAnimation(0, anim, true); // track 0
            if (rightAnim.Skeleton.Data.FindAnimation(anim) != null)
                rightAnim.AnimationState.SetAnimation(0, anim, true); // track 0
        }

        void HandleTouch()
        {
            if (Touchscreen.current == null) return;

            foreach (var touch in Touchscreen.current.touches)
            {
                int id = touch.touchId.ReadValue();
                Vector2 pos = touch.position.ReadValue();

                if (touch.press.wasPressedThisFrame)
                {
                    touches[id] = new TouchData() { isLeft = pos.x / Screen.width < 0.5f };

                    // if (!GameManager.Instance.IsPlaying && !GameManager.Instance.IsGameOver)
                    // {
                    //     GameManager.Instance.StartGame();
                    //     SetAnimation(true);
                    // }
                }

                if (touch.press.isPressed && touches.ContainsKey(id))
                    MoveByScreen(pos, touches[id].isLeft);

                if (touch.press.wasReleasedThisFrame)
                    touches.Remove(id);
            }
        }

        void HandleMouse()
        {
            if (Mouse.current == null) return;
            Vector2 pos = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                mouseLeft = pos.x / Screen.width < 0.5f;
                // if (!GameManager.Instance.IsPlaying && !GameManager.Instance.IsGameOver)
                // {
                //     GameManager.Instance.StartGame();
                //     SetAnimation(true);
                // }
            }

            if (Mouse.current.leftButton.isPressed)
                MoveByScreen(pos, mouseLeft);
        }

        void MoveByScreen(Vector2 screenPos, bool isLeft)
        {
            Vector3 world = cam.ScreenToWorldPoint(new Vector3(screenPos.x, Screen.height * 0.5f, zDepth));
            if (isLeft) SmoothMove(leftCat, ref velocityLeft, ref lastLeftX, worldLeftMin, worldLeftMax, world.x);
            else SmoothMove(rightCat, ref velocityRight, ref lastRightX, worldRightMin, worldRightMax, world.x);
        }

        void SmoothMove(Transform cat, ref Vector3 velocity, ref float lastX, float min, float max, float targetX)
        {
            targetX = Mathf.Clamp(targetX, min, max);
            Vector3 target = new Vector3(targetX, cat.position.y, 0);
            cat.position = Vector3.SmoothDamp(cat.position, target, ref velocity, smoothTime, maxSpeed);

            float delta = cat.position.x - lastX;
            if (Mathf.Abs(delta) > 0.001f)
                cat.rotation = Quaternion.Euler(0, delta > 0 ? 0 : 180, 0);

            lastX = cat.position.x;
        }

        //CAT NOTE COLLISION
        void CheckCatNoteCollision(Transform mouthPoint, SkeletonAnimation anim)
        {
            if (!GameManager.Instance.IsPlaying) return;

            Collider2D[] hits = Physics2D.OverlapCircleAll(mouthPoint.position, checkRadius, noteLayer);

            foreach (var hit in hits)
            {
                Note note = hit.GetComponent<Note>();
                if (note != null)
                {
                    note.OnHit();

                    if (isWinState) return;

                    if (anim != null)
                    {
                        anim.AnimationState.SetAnimation(0, "Eating", false);
                        anim.AnimationState.AddAnimation(0, isPlayingAnim ? idlePlaying : idle, true, 0f);
                    }
                }
            }
        }

        public void PlayWinAnimation()
        {
            Debug.Log("PLAY WIN ANIM");

            isWinState = true;

            if (leftAnim != null)
            {
                leftAnim.AnimationState.ClearTracks();
                leftAnim.AnimationState.SetAnimation(0, "Cheering_Happy _Victory", true);
            }

            if (rightAnim != null)
            {
                rightAnim.AnimationState.ClearTracks();
                rightAnim.AnimationState.SetAnimation(0, "Cheering_Happy _Victory", true);
            }
        }

        public void PlayLoseAnimation()
        {
            SetAnimation(false);

            if (leftAnim != null)
                leftAnim.AnimationState.SetAnimation(0, "Miss_Object_Lose_2", true);

            if (rightAnim != null)
                rightAnim.AnimationState.SetAnimation(0, "Miss_Object_Lose_2", true);
        }

        void OnDrawGizmosSelected()
        {
            if (leftMouthPoint != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(leftMouthPoint.position, checkRadius);
            }
            if (rightMouthPoint != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(rightMouthPoint.position, checkRadius);
            }
        }
    }
}