using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

using Enemy;
using UnityEngine;

namespace Enemy
{
    public class SnailEnemy : EnemyController
    {
        [Header("Snail"), Space]
        [SerializeField] private Sprite snailNoShellPrefab;

        [Header("Shell / Phase Config"), Space]
        // 0 -> not stomped, 1 -> phase1 done, 2 -> phase2 done, 3 -> phase3 done
        [SerializeField, Min(0)] private int stompCount = 0; 
        [SerializeField] private float shellSpeedMultiplier = 1.5f;
        [SerializeField] private float destroyDelay = 1.5f;

        private GameObject _snailSprite;
        private bool itsPhaseTwo;

        private static readonly int HitSnail = Animator.StringToHash("HitSnail");
        private static readonly int ShellHitTop = Animator.StringToHash("ShellHitTop");
        private static readonly int ShellHitWall = Animator.StringToHash("ShellHitWall");

        protected override void Update()
        {
            base.Update();

            // When shell is moving (after 2nd stomp), bounce off walls.
            if (stompCount >= 2 && isWallDetected)
            {
                Animator.SetTrigger(ShellHitWall);
                HandleFacingDirection();
            }
            
            if (itsPhaseTwo) SetLinearVelocity(moveSpeed * shellSpeedMultiplier * facingDirection, Rigidbody.linearVelocity.y);
        }

        /// <summary>
        /// Called by the player when stomping this enemy FROM ABOVE.
        /// 1st -> Phase 1
        /// 2nd -> Phase 2
        /// 3rd -> Phase 3
        /// </summary>
        public void OnStompedFromAbove()
        {
            // Already finished (3 stomps) -> ignore extra stomps.
            if (stompCount >= 3) return;

            stompCount++;

            if (stompCount == 1)
            {
                IsDead = true; // stops base enemy logic from killing/destroying it
                PhaseOneJustShellIdle();
                return;
            }

            if (stompCount == 2)
            {
                PhaseTwoShellMoves();
                return;
            }

            // stompCount == 3
            PhaseThreeShellKilled();
        }

        protected override void HandleEnemyDies()
        {
            // Prevent base "death" behaviour (disable colliders + destroy) because the snail uses phases.
            if (isDead) return;
        }

        // 1st stomp: snail gets knocked out, shell stays idle
        private void PhaseOneJustShellIdle()
        {
            SetLinearVelocity(Vector2.zero);
            Animator.SetTrigger(HitSnail);
            KnockBackVisualEffect();
            
            if (snailNoShellPrefab != null)
            {
                _snailSprite = CreateSnailSpriteGameObject(snailNoShellPrefab, transform.position);
                _snailSprite.GetComponent<Rigidbody2D>().AddForce(-customForce * -facingDirection, ForceMode2D.Impulse);
                Destroy(_snailSprite, destroyDelay);
            }
        }

        // 2nd stomp: shell starts moving
        private void PhaseTwoShellMoves()
        {
            // Example behaviour: start sprinting in current facing direction.
            // Tweak speed as you like (could also use a separate serialized shellSpeed).
            itsPhaseTwo = true;

            // Optional: animator trigger for "shell got hit from top"
            Animator.SetTrigger(ShellHitTop);
        }

        // 3rd stomp: shell dies
        private void PhaseThreeShellKilled()
        {
            SetLinearVelocity(Vector2.zero);
            itsPhaseTwo = false;https://docs.google.com/spreadsheets/d/1FV8fqkk9W8kOiuTdIjD20IiRGeERON-2SYLjFZVdKo4/edit?usp=sharing
            Animator.SetTrigger(ShellHitTop);
            KnockBackVisualEffect();

            Destroy(gameObject, destroyDelay);
        }

        private GameObject CreateSnailSpriteGameObject(Sprite sprite, Vector3 position)
        {
            GameObject snailSprite = new GameObject($"{sprite.name}_GO");
            SpriteRenderer spriteRenderer = snailSprite.AddComponent<SpriteRenderer>();
            Rigidbody2D rigidbody2D = snailSprite.AddComponent<Rigidbody2D>();

            spriteRenderer.sprite = sprite;
            snailSprite.transform.position = position;

            // Optional tiny defaults so it doesn't go crazy
            rigidbody2D.gravityScale = 1f;

            return snailSprite;
        }
    }
}