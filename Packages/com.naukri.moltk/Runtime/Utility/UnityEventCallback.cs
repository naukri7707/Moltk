using System;
using Naukri.InspectorMaid;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Naukri.Moltk.Utility
{
    [
        Slot(nameof(callback)),
        GroupScope(nameof(Event.InitAndDestroy), false), ShowIf(nameof(callback), Event.InitAndDestroy, ConditionLogic.Flag),
            Slot(
            nameof(awake),
            nameof(start),
            nameof(onDestroy)
            ),
        EndScope,
        GroupScope(nameof(Event.Update), false), ShowIf(nameof(callback), Event.Update, ConditionLogic.Flag),
            Slot(
            nameof(update),
            nameof(lateUpdate),
            nameof(fixedUpdate)
            ),
        EndScope,
        GroupScope(nameof(Event.Active), false), ShowIf(nameof(callback), Event.Active, ConditionLogic.Flag),
            Slot(
            nameof(onEnable),
            nameof(onDisable)
            ),
        EndScope,
        GroupScope(nameof(Event.Trigger), false), ShowIf(nameof(callback), Event.Trigger, ConditionLogic.Flag),
            Slot(
            nameof(onTriggerEnter),
            nameof(onTriggerExit),
            nameof(onTriggerStay)
            ),
        EndScope,
        GroupScope(nameof(Event.Collision), false), ShowIf(nameof(callback), Event.Collision, ConditionLogic.Flag),
            Slot(
            nameof(onCollisionEnter),
            nameof(onCollisionExit),
            nameof(onCollisionStay)
            ),
        EndScope,
        GroupScope(nameof(Event.Mouse), false), ShowIf(nameof(callback), Event.Mouse, ConditionLogic.Flag),
            Slot(
            nameof(onMouseEnter),
            nameof(onMouseExit),
            nameof(onMouseDown),
            nameof(onMouseUp),
            nameof(onMouseOver)
            ),
        EndScope,
        GroupScope(nameof(Event.GUI), false), ShowIf(nameof(callback), Event.GUI, ConditionLogic.Flag),
            Slot(nameof(onGUI)),
        EndScope,
    ]
    public class UnityEventCallback : MonoBehaviour
    {
        [
            HelpBox("Select your callback!", HelpBoxMessageType.Warning), HideIf(nameof(callback),
                Event.InitAndDestroy, Event.Update, Event.Active, Event.Collision, Event.Trigger, Event.Mouse, Event.GUI, ConditionLogic.Flag),
            Target, Style(marginBottom: "10"),
        ]
        public Event callback;

        public UnityEvent awake;

        public UnityEvent start;

        public UnityEvent onDestroy;

        public UnityEvent update;

        public UnityEvent lateUpdate;

        public UnityEvent fixedUpdate;

        public UnityEvent onEnable;

        public UnityEvent onDisable;

        public UnityEvent onTriggerEnter;

        public UnityEvent onTriggerExit;

        public UnityEvent onTriggerStay;

        public UnityEvent onCollisionEnter;

        public UnityEvent onCollisionExit;

        public UnityEvent onCollisionStay;

        public UnityEvent onMouseEnter;

        public UnityEvent onMouseExit;

        public UnityEvent onMouseDown;

        public UnityEvent onMouseUp;

        public UnityEvent onMouseOver;

        public UnityEvent onGUI;

        [Flags]
        public enum Event
        {
            InitAndDestroy = Flag._00,

            Update = Flag._01,

            Active = Flag._02,

            Collision = Flag._03,

            Trigger = Flag._04,

            Mouse = Flag._05,

            GUI = Flag._06,
        }

        protected virtual void Awake()
        {
            if (callback.HasFlag(Event.InitAndDestroy))
            {
                awake?.Invoke();
            }
        }

        protected virtual void Start()
        {
            if (callback.HasFlag(Event.InitAndDestroy))
            {
                start?.Invoke();
            }
        }

        protected virtual void OnDestroy()
        {
            if (callback.HasFlag(Event.InitAndDestroy))
            {
                onDestroy?.Invoke();
            }
        }

        protected virtual void Update()
        {
            if (callback.HasFlag(Event.Update))
            {
                update?.Invoke();
            }
        }

        protected virtual void LateUpdate()
        {
            if (callback.HasFlag(Event.Update))
            {
                lateUpdate?.Invoke();
            }
        }

        protected virtual void FixedUpdate()
        {
            if (callback.HasFlag(Event.Update))
            {
                fixedUpdate?.Invoke();
            }
        }

        protected virtual void OnEnable()
        {
            if (callback.HasFlag(Event.Active))
            {
                onEnable?.Invoke();
            }
        }

        protected virtual void OnDisable()
        {
            if (callback.HasFlag(Event.Active))
            {
                onDisable?.Invoke();
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (callback.HasFlag(Event.Trigger))
            {
                onTriggerEnter?.Invoke();
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (callback.HasFlag(Event.Trigger))
            {
                onTriggerEnter?.Invoke();
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (callback.HasFlag(Event.Trigger))
            {
                onTriggerExit?.Invoke();
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (callback.HasFlag(Event.Trigger))
            {
                onTriggerExit?.Invoke();
            }
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (callback.HasFlag(Event.Trigger))
            {
                onTriggerStay?.Invoke();
            }
        }

        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            if (callback.HasFlag(Event.Trigger))
            {
                onTriggerStay?.Invoke();
            }
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (callback.HasFlag(Event.Collision))
            {
                onCollisionEnter?.Invoke();
            }
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (callback.HasFlag(Event.Collision))
            {
                onCollisionEnter?.Invoke();
            }
        }

        protected virtual void OnCollisionExit(Collision collision)
        {
            if (callback.HasFlag(Event.Collision))
            {
                onCollisionExit?.Invoke();
            }
        }

        protected void OnCollisionExit2D(Collision2D collision)
        {
            if (callback.HasFlag(Event.Collision))
            {
                onCollisionExit?.Invoke();
            }
        }

        protected virtual void OnCollisionStay(Collision collision)
        {
            if (callback.HasFlag(Event.Collision))
            {
                onCollisionStay?.Invoke();
            }
        }

        protected void OnCollisionStay2D(Collision2D collision)
        {
            if (callback.HasFlag(Event.Collision))
            {
                onCollisionStay?.Invoke();
            }
        }

        protected virtual void OnMouseEnter()
        {
            if (callback.HasFlag(Event.Mouse))
            {
                onMouseEnter?.Invoke();
            }
        }

        protected virtual void OnMouseExit()
        {
            if (callback.HasFlag(Event.Mouse))
            {
                onMouseExit?.Invoke();
            }
        }

        protected virtual void OnMouseDown()
        {
            if (callback.HasFlag(Event.Mouse))
            {
                onMouseDown?.Invoke();
            }
        }

        protected virtual void OnMouseUp()
        {
            if (callback.HasFlag(Event.Mouse))
            {
                onMouseUp?.Invoke();
            }
        }

        protected virtual void OnMouseOver()
        {
            if (callback.HasFlag(Event.Mouse))
            {
                onMouseOver?.Invoke();
            }
        }

        protected virtual void OnGUI()
        {
            if (callback.HasFlag(Event.GUI))
            {
                onGUI?.Invoke();
            }
        }
    }
}
