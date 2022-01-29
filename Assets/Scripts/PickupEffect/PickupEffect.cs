using UnityEngine;

public abstract class PickupEffect
{
    public PickupEffect(GameObject _target, float _duration = 5.0f)
    {
        target = _target;
        duration = _duration;
        timer = duration;
    }

    protected enum State
    {
        Inactive,
        Active,
        Complete,
        Cleanup
    }

    public bool Update(float delta)
    {
        switch (state)
        {
            case State.Inactive:
                Begin();
                state = State.Active;
                break;

            case State.Complete:
                End();
                break;

            case State.Active:
                Process(delta);

                if (timer <= 0.0f)
                    state = State.Complete;
                else
                    timer -= delta;
                break;

            default:
                // should be cleaned up
                return true;
        }
        return false;
    }

    protected abstract void Begin();

    protected virtual void Process(float delta)
    {

    }

    protected abstract void End();

    protected GameObject target;
    protected float timer;
    protected float duration;
    protected State state = State.Inactive;
}
